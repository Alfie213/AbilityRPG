public enum AbilityType
{
    Attack,
    Barrier,
    Regeneration,
    Fireball,
    Cleanse
}

public abstract class Ability
{
    public abstract AbilityType Type { get; }
    public abstract int InstantValue { get; set; }
    public abstract int Cooldown { get; set; }
}

public abstract class AbilityWithDuration : Ability
{
    public abstract int Duration { get; set; }
    public abstract int DurationValue { get; set; }
}

public class Attack : Ability
{
    public override AbilityType Type => AbilityType.Attack;
    public override int InstantValue { get; set; } = 8;
    public override int Cooldown { get; set; } = 0;
}

public class Barrier : AbilityWithDuration
{
    public override AbilityType Type => AbilityType.Barrier;
    public override int Cooldown { get; set; } = 4;
    public override int Duration { get; set; } = 2;
    public override int InstantValue { get; set; }
    public override int DurationValue { get; set; } = 0;
}

public class Regeneration : AbilityWithDuration
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int Cooldown { get; set; } = 5;
    public override int Duration { get; set; } = 3;
}

public class Fireball : AbilityWithDuration
{
    public override AbilityType Type => AbilityType.Fireball;
    public override int Cooldown { get; set; } = 6;
    public override int Duration { get; set; } = 5;
}

public class Cleanse : Ability
{
    public override AbilityType Type => AbilityType.Cleanse;
    public override int Cooldown { get; set; } = 5;
}