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
    
    public readonly Dictionary<AbilityType, AbilityBase> EnemyAbilities = new()
    {
        { AbilityType.Attack, new AbilityAttack() },
        { AbilityType.Barrier, new AbilityBarrier() },
        { AbilityType.Regeneration, new AbilityRegeneration() },
        { AbilityType.Fireball, new AbilityFireball() },
        { AbilityType.Cleanse, new AbilityCleanse() }
    };
}