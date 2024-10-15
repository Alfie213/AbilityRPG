using System.Collections.Generic;

public enum GameStateType
{
    Playing,
    GameOver
}

public class GameState
{
    public int PlayerHealth = 100;
    public int EnemyHealth = 100;
    public readonly List<EffectBase> PlayerEffects = new();
    public readonly List<EffectBase> EnemyEffects = new();
    public GameStateType CurrentState;
}