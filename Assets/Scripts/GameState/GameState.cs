using System.Collections.Generic;
using System.Linq;

public enum GameStateType
{
    Playing,
    GameOver
}

public class GameState
{
    public int PlayerHealth = 100;
    public int EnemyHealth = 100;
    public GameStateType CurrentState;
    
    public IEnumerable<AbilityBase> AllAbilities => PlayerAbilities.Values.Concat(EnemyAbilities.Values);
    public readonly Dictionary<AbilityType, AbilityBase> PlayerAbilities = new()
    {
        { AbilityType.Attack, new AbilityAttack() },
        { AbilityType.Barrier, new AbilityBarrier() },
        { AbilityType.Regeneration, new AbilityRegeneration() },
        { AbilityType.Fireball, new AbilityFireball() },
        { AbilityType.Cleanse, new AbilityCleanse() }
    };
    public readonly Dictionary<AbilityType, AbilityBase> EnemyAbilities = new()
    {
        { AbilityType.Attack, new AbilityAttack() },
        { AbilityType.Barrier, new AbilityBarrier() },
        { AbilityType.Regeneration, new AbilityRegeneration() },
        { AbilityType.Fireball, new AbilityFireball() },
        { AbilityType.Cleanse, new AbilityCleanse() }
    };

    public IEnumerable<EffectBase> AllEffects => PlayerEffects.Concat(EnemyEffects);
    public readonly List<EffectBase> PlayerEffects = new();
    public readonly List<EffectBase> EnemyEffects = new();
}