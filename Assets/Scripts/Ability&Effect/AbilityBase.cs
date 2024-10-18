using System;
using System.Collections.Generic;
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

    protected virtual void AddEffect(Player target, EffectBase effect)
    {
        target.AddEffect(effect);
        effect.CurrentDuration.Where(currentDuration => currentDuration <= 0).Subscribe(_ =>
        {
            target.RemoveEffect(effect);
            IsWaitingForEffectToExpire = false;
        });
    }
    private void CooldownAbility()
    {
        CurrentCooldown = MaxCooldown;
    }
}

public abstract class AbilityAttackBase : AbilityBase
{
    public abstract int AttackValue { get; }
}

// effectableAbility

public class AbilityAttack : AbilityAttackBase
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

public class AbilityBarrier : AbilityBase
{
    public override AbilityType Type => AbilityType.Barrier;
    public override int MaxCooldown => 4;
    public override void Cast(Player target)
    {
        base.Cast(target);
        target.AddEffect(new EffectBarrier());
    }

    protected override void AddEffect(Player target, EffectBase effect)
    {
        base.AddEffect(target, effect);
        ((EffectBarrier)effect).CurrentBarrier.Where(currentBarrier => currentBarrier <= 0).Subscribe(_ =>
        {
            target.RemoveEffect(effect);
            IsWaitingForEffectToExpire = false;
        });
    }
}

public class AbilityRegeneration : AbilityBase
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int MaxCooldown => 5;
    public override void Cast(Player target)
    {
        base.Cast(target);
        target.AddEffect(new EffectRegeneration());
    }
}

public class AbilityFireball : AbilityAttackBase
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