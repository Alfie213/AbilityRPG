public enum EffectType
{
    Barrier,
    Burning,
    Regeneration
}

public abstract class EffectBase
{
    public abstract EffectType Type { get; }
    public abstract int Duration { get; set; }
}

public class EffectBarrier : EffectBase
{
    public override EffectType Type => EffectType.Barrier;
    public override int Duration { get; set; } = 2;
    public int BarrierValue { get; private set; } = 5;
}

public class EffectRegeneration : EffectBase
{
    public override EffectType Type => EffectType.Regeneration;
    public override int Duration { get; set; } = 3;
    public int RegenerationValue { get; private set; } = 2;
}

public class EffectBurning : EffectBase
{
    public override EffectType Type => EffectType.Burning;
    public override int Duration { get; set; } = 5;
    public int BurningValue { get; private set; } = 1;
}
