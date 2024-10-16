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
        if (gameState.Player.Abilities[abilityType].CurrentCooldown > 0)
            return false;
        
        CooldownAbility(gameState.Player.Abilities[abilityType]);

        switch (abilityType)
        {
            case AbilityType.Attack:
                ApplyPlayerDamage(new AbilityAttack().AttackValue, gameState);
                break;

            case AbilityType.Barrier:
                EffectBarrier effectBarrier = new EffectBarrier();
                effectBarrier.CurrentBarrierValue = effectBarrier.MaxBarrierValue;
                AddEffect(effectBarrier, gameState.Player.Effects);
                break;

            case AbilityType.Regeneration:
                AddEffect(new EffectRegeneration(), gameState.Player.Effects);
                break;

            case AbilityType.Fireball:
                ApplyPlayerDamage(new AbilityFireball().AttackValue, gameState);
                AddEffect(new EffectBurning(), gameState.Enemy.Effects);
                break;

            case AbilityType.Cleanse:
                gameState.Player.Effects.RemoveAll(ability => ability is EffectBurning);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
        
        Debug.Log("<color=green>Player</color> Ability used: " + abilityType);
        return true;
    }

    public void ImitateEnemyAbilityUsage(GameState gameState)
    {
        var availableAbilities = gameState.Enemy.Abilities
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
        CooldownAbility(gameState.Enemy.Abilities[abilityType]);
        
        switch (abilityType)
        {
            case AbilityType.Attack:
                ApplyEnemyDamage(new AbilityAttack().AttackValue, gameState);
                break;

            case AbilityType.Barrier:
                EffectBarrier effectBarrier = new EffectBarrier();
                effectBarrier.CurrentBarrierValue = effectBarrier.MaxBarrierValue;
                AddEffect(effectBarrier, gameState.Enemy.Effects);
                break;

            case AbilityType.Regeneration:
                AddEffect(new EffectRegeneration(), gameState.Enemy.Effects);
                break;

            case AbilityType.Fireball:
                ApplyEnemyDamage(new AbilityFireball().AttackValue, gameState);
                AddEffect(new EffectBurning(), gameState.Player.Effects);
                break;

            case AbilityType.Cleanse:
                gameState.Enemy.Effects.RemoveAll(ability => ability is EffectBurning);
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
        ApplyDamage(attackValue, gameState.Enemy.Effects, gameState.Enemy);
    }

    private void ApplyEnemyDamage(int attackValue, GameState gameState)
    {
        ApplyDamage(attackValue, gameState.Player.Effects, gameState.Player);
    }
    
    private void ApplyDamage(int attackValue, List<EffectBase> effects, Player player)
    {
        var effectBarrier = (EffectBarrier)effects.Find(effect => effect is EffectBarrier);

        if (effectBarrier != null)
        {
            int damage = attackValue - effectBarrier.CurrentBarrierValue;
            if (damage >= 0)
            {
                // player.Health -= damage;
                effects.Remove(effectBarrier);
            }
            else
            {
                effectBarrier.CurrentBarrierValue = damage;
            }
        }
        else
        {
            // health -= attackValue;
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