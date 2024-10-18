public class ServerEffectController
{
    public void ApplyEffects(GameState gameState)
    {
        foreach (EffectBase effect in gameState.AllEffects)
            effect.ApplyEffect();
    }
}