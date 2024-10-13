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
    public abstract int Cooldown { get; set; }
}

public abstract class AbilityAttack : Ability
{
    public abstract int AttackValue { get; set; }
}

public class Attack : AbilityAttack
{
    public override AbilityType Type => AbilityType.Attack;
    public override int Cooldown { get; set; } = 0;
    public override int AttackValue { get; set; } = 8;
}

public class Barrier : Ability
{
    public override AbilityType Type => AbilityType.Barrier;
    public override int Cooldown { get; set; } = 4;
}

public class Regeneration : Ability
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int Cooldown { get; set; } = 5;
}

public class Fireball : AbilityAttack
{
    public override AbilityType Type => AbilityType.Fireball;
    public override int Cooldown { get; set; } = 6;
    public override int AttackValue { get; set; } = 5;
}

public class Cleanse : Ability
{
    public override AbilityType Type => AbilityType.Cleanse;
    public override int Cooldown { get; set; } = 5;
}