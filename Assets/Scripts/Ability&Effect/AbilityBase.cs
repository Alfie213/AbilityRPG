using R3;

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
    public bool IsReady => CurrentCooldown <= 0;
    public bool IsWaitingForEffectToExpire { get; set; } = true;

    public virtual void Cast(Player target)
    {
        CooldownAbility();
    }
    public void ReduceCooldown()
    {
        CurrentCooldown -= 1;
    }
    
    private void CooldownAbility()
    {
        CurrentCooldown = MaxCooldown;
    }
}

// public abstract class AbilityAttackBase : AbilityBase
// {
//     public abstract int AttackValue { get; }
// }

public abstract class AbilityWithEffectBase : AbilityBase
{
    public override AbilityType Type { get; }
    public override int MaxCooldown { get; }
    public override void Cast(Player target)
    {
        base.Cast(target);
        AddEffect(target);
    }
    protected abstract void AddEffect(Player target);
}

public class AbilityAttack : AbilityBase
{
    public override AbilityType Type => AbilityType.Attack;
    public override int MaxCooldown => 0;
    public override int AttackValue => 8;
    public override void Cast(Player target)
    {
        base.Cast(target);
        target.ApplyDamage(AttackValue);
    }
}

public class AbilityBarrier : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Barrier;
    public override int MaxCooldown => 4;
    public override void Cast(Player target)
    {
        base.Cast(target);
        AddEffect(target);
    }
    protected override void AddEffect(Player target)
    {
        EffectBarrier effectBarrier = new EffectBarrier();
        target.AddEffect(effectBarrier);
        effectBarrier.CurrentBarrier.Where(currentBarrier => currentBarrier <= 0).Subscribe(_ =>
        {
            target.RemoveEffect(effectBarrier);
            IsWaitingForEffectToExpire = false;
        });
    }
}

public class AbilityRegeneration : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int MaxCooldown => 5;
    public override void Cast(Player target)
    {
        base.Cast(target);
        target.AddEffect(new EffectRegeneration());
    }
    protected override void AddEffect(Player target)
    {
        EffectRegeneration effectRegeneration = new EffectRegeneration();
        target.AddEffect(effectRegeneration);
        effectRegeneration.CurrentDuration.Where(currentDuration => currentDuration <= 0).Subscribe(_ =>
        {
            target.RemoveEffect(effectRegeneration);
            IsWaitingForEffectToExpire = false;
        });
    }
}

public class AbilityFireball : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Fireball;
    public override int MaxCooldown => 6;
    public override int AttackValue => 5;
    public override void Cast(Player target)
    {
        base.Cast(target);
        target.ApplyDamage(AttackValue);
        target.AddEffect(new EffectBurning());
    }
    protected override void AddEffect(Player target)
    {
        EffectBurning effectBurning = new EffectBurning();
        target.AddEffect(effectBurning);
        effectBurning.CurrentDuration.Where(currentDuration => currentDuration <= 0).Subscribe(_ =>
        {
            target.RemoveEffect(effectBurning);
            IsWaitingForEffectToExpire = false;
        });
    }
}

public class AbilityCleanse : AbilityBase
{
    public override AbilityType Type => AbilityType.Cleanse;
    public override int MaxCooldown => 5;
    public override void Cast(Player target)
    {
        base.Cast(target);
        EffectBase activeEffectBurning = target.Effects.Find(burning => burning is EffectBurning);
        if (activeEffectBurning != null)
            target.RemoveEffect(activeEffectBurning);
    }
}