public class ServerEffectController
{
    public void ApplyEffects(GameState gameState)
    {
        foreach (EffectBase effect in gameState.AllEffects)
        {
            effect.ApplyEffect();
        }
    }

    public void ReduceCurrentDuration(GameState gameState)
    {
        gameState.Player.ReduceEffectDurations();
        gameState.Enemy.ReduceEffectDurations();
    }
}