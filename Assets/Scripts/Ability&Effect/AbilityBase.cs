using System;

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
    // private static readonly Random Random = new();
    // public static AbilityType GetRandomAbilityType()
    // {
    //     Array values = Enum.GetValues(typeof(AbilityType));
    //     return (AbilityType)values.GetValue(Random.Next(values.Length));
    // }
    
    public abstract AbilityType Type { get; }
    public abstract int MaxCooldown { get; }
    public int CurrentCooldown { get; set; }
    public bool IsWaitingForEffectToExpire { get; set; } = true;
}

public abstract class AbilityAttackBase : AbilityBase
{
    public abstract int AttackValue { get; }
}

public class AbilityAttack : AbilityAttackBase
{
    public override AbilityType Type => AbilityType.Attack;
    public override int MaxCooldown => 0;
    public override int AttackValue => 8;
}

public class AbilityBarrier : AbilityBase
{
    public override AbilityType Type => AbilityType.Barrier;
    public override int MaxCooldown => 4;
}

public class AbilityRegeneration : AbilityBase
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int MaxCooldown => 5;
}

public class AbilityFireball : AbilityAttackBase
{
    public override AbilityType Type => AbilityType.Fireball;
    public override int MaxCooldown => 6;
    public override int AttackValue => 5;
}

public class AbilityCleanse : AbilityBase
{
    public override AbilityType Type => AbilityType.Cleanse;
    public override int MaxCooldown => 5;
}