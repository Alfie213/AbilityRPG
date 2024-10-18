using R3;

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
    public ReactiveProperty<int> CurrentDuration { get; } = new();
    protected readonly Player Target;

    protected EffectBase(Player target)
    {
        Target = target;
    }
    
    public void ReduceDuration()
    {
        CurrentDuration.Value--;
        if (CurrentDuration.Value <= 0)
            Target.RemoveEffect(this);
    }
    public virtual void ApplyEffect()
    {
        CurrentDuration.Value = MaxDuration;
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
        base.ApplyEffect();
        if (CurrentBarrier.Value <= 0)
        {
            Target.Effects.Remove(this);
        }
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
        base.ApplyEffect();
        Target.ApplyDamage(BurningValue);
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
        base.ApplyEffect();
        Target.Heal(RegenerationValue);
    }
}