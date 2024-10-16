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
    public int CurrentDuration { get; set; }
}

public class EffectBarrier : EffectBase
{
    public override EffectType Type => EffectType.Barrier;
    public override int MaxDuration => 2;
    public int MaxBarrierValue => 5;
    public int CurrentBarrierValue { get; set; }
}

public class EffectBurning : EffectBase
{
    public override EffectType Type => EffectType.Burning;
    public override int MaxDuration => 5;
    public int BurningValue => 1;
}

public class EffectRegeneration : EffectBase
{
    public override EffectType Type => EffectType.Regeneration;
    public override int MaxDuration => 3;
    public int RegenerationValue => 2;
}
