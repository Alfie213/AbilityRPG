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

        Player target;
        
        switch (abilityType)
        {
            case AbilityType.Attack:
            case AbilityType.Fireball:
                target = gameState.Enemy;
                break;

            case AbilityType.Regeneration:
            case AbilityType.Barrier:
            case AbilityType.Cleanse:
                target = gameState.Player;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        
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
        AbilityBase ability = gameState.Enemy.Abilities[abilityType];

        Player target;
        
        switch (abilityType)
        {
            case AbilityType.Attack:
            case AbilityType.Fireball:
                target = gameState.Player;
                break;

            case AbilityType.Regeneration:
            case AbilityType.Barrier:
            case AbilityType.Cleanse:
                target = gameState.Enemy;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
        
        ability.Cast(target);

        Debug.Log("<color=red>Enemy</color> Ability casted: " + abilityType);
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