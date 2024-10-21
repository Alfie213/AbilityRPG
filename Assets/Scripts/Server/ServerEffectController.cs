using System.Linq;

public class ServerEffectController
{
    public void ApplyEffects(GameState gameState)
    {
        var effectsCopy = gameState.AllEffects.ToList();
        
        foreach (EffectBase effect in effectsCopy)
        {
            effect.ApplyEffect();
        }
    }
}