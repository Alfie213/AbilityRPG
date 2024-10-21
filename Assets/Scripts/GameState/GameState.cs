using System.Collections.Generic;
using System.Linq;

public enum GameStateType
{
    Playing,
    GameOver
}

public class GameState
{
    public GameStateType CurrentState;
    
    public readonly Player Player = new();
    public readonly Player Enemy = new();
    
    public IEnumerable<AbilityBase> AllAbilities => Player.Abilities.Values.Concat(Enemy.Abilities.Values);
    
    public IEnumerable<EffectBase> AllEffects => Player.Effects.Concat(Enemy.Effects);
}
