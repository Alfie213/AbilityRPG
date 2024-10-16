using System.Collections.Generic;
using R3;

public class Player
{
    public readonly ReactiveProperty<int> Health = new(100);
    public readonly List<EffectBase> Effects = new();

    public readonly IReadOnlyDictionary<AbilityType, AbilityBase> Abilities = new Dictionary<AbilityType, AbilityBase>
    {
        { AbilityType.Attack, new AbilityAttack() },
        { AbilityType.Barrier, new AbilityBarrier() },
        { AbilityType.Regeneration, new AbilityRegeneration() },
        { AbilityType.Fireball, new AbilityFireball() },
        { AbilityType.Cleanse, new AbilityCleanse() }
    };
    
    public void ApplyDamage(int damage)
    {
        Health.Value -= damage;
        if (Health.Value < 0)
            Health.Value = 0;
    }

    public void Heal(int amount)
    {
        Health.Value += amount;
    }

    public void AddEffect(EffectBase effect)
    {
        Effects.Add(effect);
    }

    public void RemoveEffect(EffectBase effect)
    {
        Effects.Remove(effect);
    }
}