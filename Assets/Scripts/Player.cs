using System.Collections.Generic;

public class Player
{
    public int Health { get; private set; } = 100;
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
        Health -= damage;
        if (Health < 0)
        {
            Health = 0; // Убеждаемся, что здоровье не станет отрицательным
        }
    }

    public void Heal(int amount)
    {
        Health += amount;
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