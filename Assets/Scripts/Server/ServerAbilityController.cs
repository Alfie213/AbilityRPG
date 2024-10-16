using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ServerAbilityController
{
    private static readonly Random Random = new();

    private void AddAndTrackEffectExpiring(AbilityBase ability, EffectBase effect, Player player)
    {
        player.AddEffect(effect);

        Observable.Merge(
            effect.CurrentDuration.Where(duration => duration <= 0),
            effect is EffectBarrier barrier
                ? barrier.CurrentBarrier.Where(value => value <= 0)
                : Observable.Empty<int>()
        ).Subscribe(_ =>
        {
            player.RemoveEffect(effect);
            CooldownAbility(ability);
        });
    }

    private void CooldownAbility(AbilityBase ability)
    {
        ability.CurrentCooldown = ability.MaxCooldown;
    }
    
    public bool TrySubmitPlayerAbilityUsage(GameState gameState, AbilityType abilityType)
    {
        AbilityBase ability = gameState.Player.Abilities[abilityType];
        
        if (ability.CurrentCooldown > 0)
            return false;
        
        switch (ability.Type)
        {
            case AbilityType.Attack:
                ApplyPlayerDamage(new AbilityAttack().AttackValue, gameState);
                CooldownAbility(ability);
                break;

            case AbilityType.Barrier:
                EffectBarrier effectBarrier = new EffectBarrier();
                effectBarrier.CurrentBarrier.Value = effectBarrier.MaxBarrierValue;
                AddAndTrackEffectExpiring(ability, effectBarrier, gameState.Player);
                break;

            case AbilityType.Regeneration:
                EffectRegeneration effectRegeneration = new EffectRegeneration();
                AddAndTrackEffectExpiring(ability, effectRegeneration, gameState.Player);
                break;

            case AbilityType.Fireball:
                ApplyPlayerDamage(new AbilityFireball().AttackValue, gameState);
                EffectBurning effectBurning = new EffectBurning();
                AddAndTrackEffectExpiring(ability, effectBurning, gameState.Enemy);
                break;

            case AbilityType.Cleanse:
                EffectBase activeEffectBurning = gameState.Player.Effects.Find(burning => burning is EffectBurning);
                gameState.Player.RemoveEffect(activeEffectBurning);
                CooldownAbility(ability);
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
        switch (abilityType)
        {
            case AbilityType.Attack:
                ApplyEnemyDamage(new AbilityAttack().AttackValue, gameState);
                break;

            case AbilityType.Barrier:
                EffectBarrier effectBarrier = new EffectBarrier();
                effectBarrier.CurrentBarrier.Value = effectBarrier.MaxBarrierValue;
                gameState.Enemy.AddEffect(effectBarrier);
                break;

            case AbilityType.Regeneration:
                gameState.Enemy.AddEffect(new EffectRegeneration());
                break;

            case AbilityType.Fireball:
                ApplyEnemyDamage(new AbilityFireball().AttackValue, gameState);
                gameState.Player.AddEffect(new EffectBurning());
                break;

            case AbilityType.Cleanse:
                EffectBase effectBurning = gameState.Enemy.Effects.Find(ability => ability is EffectBurning);
                gameState.Enemy.RemoveEffect(effectBurning);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
        
        Debug.Log("<color=red>Enemy</color> Ability used: " + abilityType);
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
            int damage = attackValue - effectBarrier.CurrentBarrier.Value;
            if (damage >= 0)
            {
                player.ApplyDamage(damage);
                effects.Remove(effectBarrier);
            }
            else
            {
                effectBarrier.CurrentBarrier.Value = damage;
            }
        }
        else
        {
            player.ApplyDamage(attackValue);
        }
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