using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEffectController
{
    public void ApplyEffects(GameState gameState)
    {
        List<EffectBase> playerEffectsToRemove = new();
        foreach (EffectBase playerEffect in gameState.PlayerEffects)
        {
            switch (playerEffect.Type)
            {
                case EffectType.Barrier:
                    break;
                case EffectType.Burning:
                    gameState.PlayerHealth -= ((EffectBurning)playerEffect).BurningValue;
                    break;
                case EffectType.Regeneration:
                    gameState.PlayerHealth += ((EffectRegeneration)playerEffect).RegenerationValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            playerEffect.CurrentDuration -= 1;
            if (playerEffect.CurrentDuration <= 0)
                playerEffectsToRemove.Add(playerEffect);
        }
        foreach (EffectBase effect in playerEffectsToRemove)
            gameState.PlayerEffects.Remove(effect);

        List<EffectBase> enemyEffectsToRemove = new();
        foreach (EffectBase enemyEffect in gameState.EnemyEffects)
        {
            switch (enemyEffect.Type)
            {
                case EffectType.Barrier:
                    break;
                case EffectType.Burning:
                    gameState.EnemyHealth -= ((EffectBurning)enemyEffect).BurningValue;
                    break;
                case EffectType.Regeneration:
                    gameState.EnemyHealth += ((EffectRegeneration)enemyEffect).RegenerationValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            enemyEffect.CurrentDuration -= 1;
            if (enemyEffect.CurrentDuration <= 0)
                enemyEffectsToRemove.Add(enemyEffect);
        }
        foreach (EffectBase effect in enemyEffectsToRemove)
            gameState.EnemyEffects.Remove(effect);
    }

    public void ReduceCurrentDuration(GameState gameState)
    {
        foreach (EffectBase effect in gameState.AllEffects)
        {
            if (effect.CurrentDuration < 0)
                continue;

            effect.CurrentDuration--;
        }
    }
}