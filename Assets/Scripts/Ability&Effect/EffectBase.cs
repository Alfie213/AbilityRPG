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

    protected EffectBase()
    {
        CurrentDuration.Value = MaxDuration;
    }

    public void ReduceDuration()
    {
        CurrentDuration.Value--;
    }

    public abstract void ApplyEffect(Player player);
}

public class EffectBarrier : EffectBase
{
    public override EffectType Type => EffectType.Barrier;
    public override int MaxDuration => 2;
    public ReactiveProperty<int> CurrentBarrier { get; }
    private int MaxBarrierValue => 5;

    public EffectBarrier()
    {
        CurrentBarrier = new ReactiveProperty<int>(MaxBarrierValue);
    }

    public override void ApplyEffect(Player player)
    {
        if (CurrentBarrier.Value <= 0)
        {
            player.Effects.Remove(this);
        }
    }
}

public class EffectBurning : EffectBase
{
    public override EffectType Type => EffectType.Burning;
    public override int MaxDuration => 5;
    public AbilityFireball SourceAbility { get; }
    private int BurningValue => 1;

    public EffectBurning(AbilityFireball sourceAbility)
    {
        SourceAbility = sourceAbility;
    }

    public override void ApplyEffect(Player player)
    {
        player.ApplyDamage(BurningValue);
    }
}

public class EffectRegeneration : EffectBase
{
    public override EffectType Type => EffectType.Regeneration;
    public override int MaxDuration => 3;
    private int RegenerationValue => 2;

    public override void ApplyEffect(Player player)
    {
        player.Heal(RegenerationValue);
    }
}