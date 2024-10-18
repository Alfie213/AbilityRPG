using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ServerAbilityController
{
    private static readonly Random Random = new();

    public bool TrySubmitPlayerAbilityUsage(GameState gameState, AbilityType abilityType)
    {
        AbilityBase ability = gameState.Player.Abilities[abilityType];

        if (!ability.IsReady)
            return false;

        IEntity target = GetTarget(gameState, abilityType, isPlayer: true);
        ability.Cast(target);

        Debug.Log("<color=green>Player</color> Ability casted: " + abilityType);
        return true;
    }

    public void ImitateEnemyAbilityUsage(GameState gameState)
    {
        var availableAbilities = gameState.Enemy.Abilities
            .Where(entry => entry.Value.IsReady)
            .Select(entry => entry.Key)
            .ToList();

        if (availableAbilities.Count > 0)
        {
            AbilityType randomAbilityType = availableAbilities[Random.Next(availableAbilities.Count)];
            SubmitEnemyAbilityUsage(gameState, randomAbilityType);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    
    private void SubmitEnemyAbilityUsage(GameState gameState, AbilityType abilityType)
    {
        AbilityBase ability = gameState.Enemy.Abilities[abilityType];
        IEntity target = GetTarget(gameState, abilityType, isPlayer: false);
        ability.Cast(target);

        Debug.Log("<color=red>Enemy</color> Ability casted: " + abilityType);
    }

    private IEntity GetTarget(GameState gameState, AbilityType abilityType, bool isPlayer)
    {
        return abilityType switch
        {
            AbilityType.Attack or AbilityType.Fireball => isPlayer ? gameState.Enemy : gameState.Player,
            AbilityType.Regeneration or AbilityType.Barrier or AbilityType.Cleanse => isPlayer ? gameState.Player : gameState.Enemy,
            _ => throw new ArgumentOutOfRangeException(nameof(abilityType), "Unknown ability type")
        };
    }

    public void ReduceCooldown(GameState gameState)
    {
        foreach (AbilityBase ability in gameState.AllAbilities)
            ability.ReduceCooldown();
    }
}
