using System.Collections.Generic;
using R3;

public class Player
{
    public readonly ReactiveProperty<int> Health = new(MaxHealth);
    public readonly List<EffectBase> Effects = new();

    public readonly IReadOnlyDictionary<AbilityType, AbilityBase> Abilities = new Dictionary<AbilityType, AbilityBase>
    {
        { AbilityType.Attack, new AbilityAttack() },
        { AbilityType.Barrier, new AbilityBarrier() },
        { AbilityType.Regeneration, new AbilityRegeneration() },
        { AbilityType.Fireball, new AbilityFireball() },
        { AbilityType.Cleanse, new AbilityCleanse() }
    };

    private const int MaxHealth = 100;
    
    public void ApplyDamage(int damage)
    {
        int remainingDamage = ProcessBarrier(damage);

        if (remainingDamage > 0)
        {
            Health.Value -= remainingDamage;
        
            if (Health.Value < 0)
                Health.Value = 0;
        }

        if (Health.Value < 0)
            Health.Value = 0;
    }
    
    private int ProcessBarrier(int damage)
    {
        var effectBarrier = (EffectBarrier)Effects.Find(effect => effect is EffectBarrier);

        if (effectBarrier != null)
        {
            int pureDamage = damage - effectBarrier.CurrentBarrier.Value;
        
            if (pureDamage >= 0)
            {
                RemoveEffect(effectBarrier);
                return pureDamage;
            }
            else
            {
                effectBarrier.CurrentBarrier.Value = -pureDamage;
                return 0;
            }
        }
    
        return damage;
    }

    public void Heal(int amount)
    {
        Health.Value += amount;
        if (Health.Value > MaxHealth)
            Health.Value = MaxHealth;
    }

    public void AddEffect(EffectBase effect)
    {
        effect.CurrentDuration.Value = effect.MaxDuration;
        Effects.Add(effect);
    }

    public void RemoveEffect(EffectBase effect)
    {
        Effects.Remove(effect);
    }
}