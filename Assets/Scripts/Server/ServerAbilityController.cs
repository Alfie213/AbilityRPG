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

        switch (abilityType)
        {
            case AbilityType.Attack:
                ApplyPlayerDamage(new AbilityAttack().AttackValue, gameState);
                break;

            case AbilityType.Barrier:
                AddEffect(new EffectBarrier(), gameState.PlayerEffects);
                break;

            case AbilityType.Regeneration:
                AddEffect(new EffectRegeneration(), gameState.PlayerEffects);
                break;

            case AbilityType.Fireball:
                ApplyPlayerDamage(new AbilityFireball().AttackValue, gameState);
                AddEffect(new EffectBurning(), gameState.EnemyEffects);
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
                ApplyEnemyDamage(new AbilityAttack().AttackValue, gameState);
                break;

            case AbilityType.Barrier:
                AddEffect(new EffectBarrier(), gameState.EnemyEffects);
                break;

            case AbilityType.Regeneration:
                AddEffect(new EffectRegeneration(), gameState.EnemyEffects);
                break;

            case AbilityType.Fireball:
                ApplyPlayerDamage(new AbilityFireball().AttackValue, gameState);
                AddEffect(new EffectBurning(), gameState.PlayerEffects);
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
        
    private void ApplyPlayerDamage(int attackValue, GameState gameState)
    {
        ApplyDamage(attackValue, gameState.EnemyEffects, ref gameState.EnemyHealth);
    }

    private void ApplyEnemyDamage(int attackValue, GameState gameState)
    {
        ApplyDamage(attackValue, gameState.PlayerEffects, ref gameState.PlayerHealth);
    }
    
    private void ApplyDamage(int attackValue, List<EffectBase> effects, ref int health)
    {
        var effectBarrier = (EffectBarrier)effects.Find(effect => effect is EffectBarrier);

        if (effectBarrier != null)
        {
            int damage = attackValue - effectBarrier.CurrentBarrierValue;
            if (damage >= 0)
            {
                health -= damage;
                effects.Remove(effectBarrier);
            }
            else
            {
                effectBarrier.CurrentBarrierValue = damage;
            }
        }
        else
        {
            health -= attackValue;
        }
    }

    private void AddEffect(EffectBase effect, List<EffectBase> effectsList)
    {
        effect.CurrentDuration = effect.MaxDuration;
        effectsList.Add(effect);
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