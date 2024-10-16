using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ServerAbilityController
{
    private static readonly Random Random = new();
    
    public bool TrySubmitPlayerAbilityUsage(GameState gameState, AbilityType abilityType)
    {
        if (gameState.PlayerAbilities[abilityType].CurrentCooldown > 0)
            return false;
        
        CooldownAbility(gameState.PlayerAbilities[abilityType]);

        EffectBarrier effectBarrier;
        switch (abilityType)
        {
            case AbilityType.Attack:
                effectBarrier = (EffectBarrier)gameState.EnemyEffects.Find(effect => effect is EffectBarrier);
                if (effectBarrier != null)
                {
                    int damage = new AbilityAttack().AttackValue - effectBarrier.CurrentBarrierValue;
                    if (damage >= 0)
                    {
                        gameState.EnemyHealth -= damage;
                        gameState.EnemyEffects.Remove(effectBarrier);
                    }
                    else
                        effectBarrier.CurrentBarrierValue = damage;
                }
                else
                    gameState.EnemyHealth -= new AbilityAttack().AttackValue;
                break;
            case AbilityType.Barrier:
                EffectBase newEffectBarrier = new EffectBarrier();
                newEffectBarrier.CurrentDuration = newEffectBarrier.MaxDuration;
                gameState.PlayerEffects.Add(newEffectBarrier);
                break;
            case AbilityType.Regeneration:
                EffectBase effectRegeneration = new EffectRegeneration();
                effectRegeneration.CurrentDuration = effectRegeneration.MaxDuration;
                gameState.PlayerEffects.Add(effectRegeneration);
                break;
            case AbilityType.Fireball:
                effectBarrier = (EffectBarrier)gameState.EnemyEffects.Find(effect => effect is EffectBarrier);
                if (effectBarrier != null)
                {
                    int damage = new AbilityFireball().AttackValue - effectBarrier.CurrentBarrierValue;
                    if (damage >= 0)
                    {
                        gameState.EnemyHealth -= damage;
                        gameState.EnemyEffects.Remove(effectBarrier);
                    }
                    else
                        effectBarrier.CurrentBarrierValue = damage;
                }
                else
                    gameState.EnemyHealth -= new AbilityFireball().AttackValue;
                
                EffectBase effectBurning = new EffectBurning();
                effectBurning.CurrentDuration = effectBurning.MaxDuration;
                gameState.EnemyEffects.Add(effectBurning);
                break;
            case AbilityType.Cleanse:
                gameState.PlayerEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
        
        Debug.Log("<color=green>Player</color> Ability used: " + abilityType);
        return true;
    }

    public void ImitateEnemyAbilityUsage(GameState gameState)
    {
        var availableAbilities = gameState.EnemyAbilities
            .Where(entry => entry.Value.CurrentCooldown <= 0)
            .Select(entry => entry.Key)
            .ToList();
        // Debug.Log($"After this turn <color=red>Enemy</color> will have {availableAbilities.Count} abilities.");
        
        if (availableAbilities.Count > 0)
        {
            AbilityType randomAbilityType = availableAbilities[Random.Next(availableAbilities.Count)];
            SubmitEnemyAbilityUsage(gameState, randomAbilityType);
        }
        else
            throw new NotImplementedException();
    }
    
    private void SubmitEnemyAbilityUsage(GameState gameState, AbilityType abilityType)
    {
        CooldownAbility(gameState.EnemyAbilities[abilityType]);
        
        switch (abilityType)
        {
            case AbilityType.Attack:
                gameState.PlayerHealth -= new AbilityAttack().AttackValue;
                break;
            case AbilityType.Barrier:
                EffectBase effectBarrier = new EffectBarrier();
                effectBarrier.CurrentDuration = effectBarrier.MaxDuration;
                gameState.EnemyEffects.Add(effectBarrier);
                break;
            case AbilityType.Regeneration:
                EffectBase effectRegeneration = new EffectRegeneration();
                effectRegeneration.CurrentDuration = effectRegeneration.MaxDuration;
                gameState.EnemyEffects.Add(effectRegeneration);
                break;
            case AbilityType.Fireball:
                gameState.PlayerHealth -= new AbilityFireball().AttackValue;
                EffectBase effectBurning = new EffectBurning();
                effectBurning.CurrentDuration = effectBurning.MaxDuration;
                gameState.PlayerEffects.Add(effectBurning);
                break;
            case AbilityType.Cleanse:
                gameState.EnemyEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
        
        Debug.Log("<color=red>Enemy</color> Ability used: " + abilityType);
    }
    
    private void CooldownAbility(AbilityBase ability)
    {
        ability.CurrentCooldown = ability.MaxCooldown;
    }

    public void ReduceCurrentCooldown(GameState gameState)
    {
        foreach (AbilityBase ability in gameState.AllAbilities)
        {
            if (ability.CurrentCooldown < 0)
                continue;

            ability.CurrentCooldown--;
        }
    }
}