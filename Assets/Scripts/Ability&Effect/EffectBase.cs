using R3;
using UnityEngine;

public enum EffectType
{
    Barrier,
    Burning,
    Regeneration
}

public abstract class EffectBase
{
    public abstract EffectType Type { get; }
    public abstract int MaxDuration { get; }
    public ReactiveProperty<int> CurrentDuration { get; private set; }
    protected readonly Player Target;

    protected EffectBase(Player target)
    {
        Target = target;
        InitializeCurrentDuration();
    }

    private void InitializeCurrentDuration()
    {
        CurrentDuration = new ReactiveProperty<int>(MaxDuration);
        Debug.Log(CurrentDuration.Value);
    }
    public virtual void ApplyEffect()
    {
        ReduceDuration();
    }
    private void ReduceDuration()
    {
        CurrentDuration.Value--;
        if (CurrentDuration.Value <= 0)
            Target.RemoveEffect(this);
    }
}

public class EffectBarrier : EffectBase
{
    public override EffectType Type => EffectType.Barrier;
    public override int MaxDuration => 2;
    public readonly ReactiveProperty<int> CurrentBarrier = new(MaxBarrierValue);
    private const int MaxBarrierValue = 5;
    
    public EffectBarrier(Player target) : base(target)
    {
    }
    
    public override void ApplyEffect()
    {
        if (CurrentBarrier.Value <= 0)
        {
            Target.Effects.Remove(this);
        }
        base.ApplyEffect();
    }
}

public class EffectBurning : EffectBase
{
    public override EffectType Type => EffectType.Burning;
    public override int MaxDuration => 5;
    public AbilityFireball SourceAbility { get; }
    private const int BurningValue = 1;

    public EffectBurning(Player target, AbilityFireball sourceAbility) : base(target)
    {
        SourceAbility = sourceAbility;
    }
    
    public override void ApplyEffect()
    {
        Target.ApplyDamage(BurningValue);
        base.ApplyEffect();
    }
}

public class EffectRegeneration : EffectBase
{
    public override EffectType Type => EffectType.Regeneration;
    public override int MaxDuration => 3;
    private const int RegenerationValue = 2;

    public EffectRegeneration(Player target) : base(target)
    {
    }
    
    public override void ApplyEffect()
    {
        Target.Heal(RegenerationValue);
        base.ApplyEffect();
    }
}