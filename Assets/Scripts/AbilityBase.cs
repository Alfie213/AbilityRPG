public enum AbilityType
{
    Attack,
    Barrier,
    Regeneration,
    Fireball,
    Cleanse
}

public abstract class AbilityBase
{
    public abstract AbilityType Type { get; }
    public abstract int Cooldown { get; set; }
}

public abstract class AbilityAttackBase : AbilityBase
{
    public abstract int AttackValue { get; set; }
}

public class AttackBase : AbilityAttackBase
{
    public override AbilityType Type => AbilityType.Attack;
    public override int Cooldown { get; set; } = 0;
    public override int AttackValue { get; set; } = 8;
}

public class Barrier : AbilityBase
{
    public override AbilityType Type => AbilityType.Barrier;
    public override int Cooldown { get; set; } = 4;
}

public class Regeneration : AbilityBase
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int Cooldown { get; set; } = 5;
}

public class Fireball : AbilityAttackBase
{
    public override AbilityType Type => AbilityType.Fireball;
    public override int Cooldown { get; set; } = 6;
    public override int AttackValue { get; set; } = 5;
}

public class Cleanse : AbilityBase
{
    public override AbilityType Type => AbilityType.Cleanse;
    public override int Cooldown { get; set; } = 5;
}