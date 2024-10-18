public class ServerEffectController
{
    public void ApplyEffects(GameState gameState)
    {
        gameState.Player.ApplyEffects();
        gameState.Enemy.ApplyEffects();
    }

    public void ReduceCurrentDuration(GameState gameState)
    {
        gameState.Player.ReduceEffectDurations();
        gameState.Enemy.ReduceEffectDurations();
    }
}