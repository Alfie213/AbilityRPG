public enum EffectType
{
    Barrier,
    Burning,
    Regeneration
}

public abstract class EffectBase
{
    public abstract EffectType Type { get; }
    public abstract int MaxDuration { get; set; }
    public int CurrentDuration { get; set; }
}

public class EffectBarrier : EffectBase
{
    public override EffectType Type => EffectType.Barrier;
    public override int MaxDuration { get; set; } = 2;
    public int BarrierValue { get; private set; } = 5;
}

public class EffectBurning : EffectBase
{
    public override EffectType Type => EffectType.Burning;
    public override int MaxDuration { get; set; } = 5;
    public int BurningValue { get; private set; } = 1;
}

public class EffectRegeneration : EffectBase
{
    public override EffectType Type => EffectType.Regeneration;
    public override int MaxDuration { get; set; } = 3;
    public int RegenerationValue { get; private set; } = 2;
}
