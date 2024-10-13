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

public abstract class AbilityWithDuration : Ability
{
    public abstract int Duration { get; set; }
}

public class AbilityAttack : Ability
{
    public override AbilityType Type => AbilityType.Attack;
    
}