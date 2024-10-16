using System;
using System.Collections.Generic;

public class ServerEffectController
{
    public void ApplyEffects(GameState gameState)
    {
        foreach (EffectBase playerEffect in gameState.Player.Effects)
        {
            switch (playerEffect.Type)
            {
                case EffectType.Barrier:
                    break;
                case EffectType.Burning:
                    gameState.Player.ApplyDamage(((EffectBurning)playerEffect).BurningValue);
                    break;
                case EffectType.Regeneration:
                    gameState.Player.Heal(((EffectRegeneration)playerEffect).RegenerationValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (EffectBase enemyEffect in gameState.Enemy.Effects)
        {
            switch (enemyEffect.Type)
            {
                case EffectType.Barrier:
                    break;
                case EffectType.Burning:
                    gameState.Enemy.ApplyDamage(((EffectBurning)enemyEffect).BurningValue);
                    break;
                case EffectType.Regeneration:
                    gameState.Enemy.Heal(((EffectRegeneration)enemyEffect).RegenerationValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void ReduceCurrentDuration(GameState gameState)
    {
        List<EffectBase> playerEffectsToRemove = new();
        foreach (EffectBase playerEffect in gameState.Player.Effects)
        {
            playerEffect.CurrentDuration -= 1;
            if (playerEffect.CurrentDuration <= 0)
                playerEffectsToRemove.Add(playerEffect);
        }
        foreach (EffectBase effect in playerEffectsToRemove)
            gameState.Player.Effects.Remove(effect);
        
        List<EffectBase> enemyEffectsToRemove = new();
        foreach (EffectBase enemyEffect in gameState.Enemy.Effects)
        {
            enemyEffect.CurrentDuration -= 1;
            if (enemyEffect.CurrentDuration <= 0)
                enemyEffectsToRemove.Add(enemyEffect);
        }
        foreach (EffectBase effect in enemyEffectsToRemove)
            gameState.Enemy.Effects.Remove(effect);
    }
}
