using System.Linq;
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
    public bool IsReady => CurrentCooldown <= 0;
    public int CurrentCooldown { get; private set; }
    protected abstract int MaxCooldown { get; }
    
    public virtual void Cast(Player target)
    {
        CooldownAbility();
    }
    public virtual void ReduceCooldown()
    {
        if (!IsReady)
            CurrentCooldown--;
    }
    private void CooldownAbility()
    {
        CurrentCooldown = MaxCooldown;
    }
    
    public virtual bool IsWaitingForEffect => false;
}

public abstract class AbilityWithEffectBase : AbilityBase
{
    public bool IsWaitingForEffectToExpire { get; set; }

    public override void Cast(Player target)
    {
        base.Cast(target);
        IsWaitingForEffectToExpire = true;
        var effect = CreateEffect(target);
        target.AddEffect(effect);
        effect.CurrentDuration.Where(duration => duration <= 0).Subscribe(_ =>
        {
            IsWaitingForEffectToExpire = false;
        });
    }

    public override void ReduceCooldown()
    {
        if (IsWaitingForEffectToExpire)
            return;
        base.ReduceCooldown();
    }

    protected abstract EffectBase CreateEffect(Player target);
    public override bool IsWaitingForEffect => IsWaitingForEffectToExpire;
}

public class AbilityAttack : AbilityBase
{
    public override AbilityType Type => AbilityType.Attack;
    protected override int MaxCooldown => 0;
    private const int AttackDamage = 8;

    public override void Cast(Player target)
    {
        base.Cast(target);
        target.ApplyDamage(AttackDamage);
    }
}

public class AbilityBarrier : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Barrier;
    protected override int MaxCooldown => 4;

    public override void Cast(Player target)
    {
        base.Cast(target);
        var effectBarrier = target.Effects.OfType<EffectBarrier>().FirstOrDefault();
        if (effectBarrier != null)
        {
            effectBarrier.CurrentBarrier.Where(currentBarrier => currentBarrier <= 0).Subscribe(_ =>
            {
                IsWaitingForEffectToExpire = false;
            });
        }
    }
    
    protected override EffectBase CreateEffect(Player target) => new EffectBarrier(target);
    public override bool IsWaitingForEffect => IsWaitingForEffectToExpire;
}

public class AbilityRegeneration : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Regeneration;
    protected override int MaxCooldown => 5;
    
    protected override EffectBase CreateEffect(Player target) => new EffectRegeneration(target);
    public override bool IsWaitingForEffect => IsWaitingForEffectToExpire;
}

public class AbilityFireball : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Fireball;
    protected override int MaxCooldown => 6;
    private const int FireballDamage = 5;

    public override void Cast(Player target)
    {
        base.Cast(target);
        target.ApplyDamage(FireballDamage);
    }

    protected override EffectBase CreateEffect(Player target) => new EffectBurning(target, this);
    public override bool IsWaitingForEffect => IsWaitingForEffectToExpire;
}

public class AbilityCleanse : AbilityBase
{
    public override AbilityType Type => AbilityType.Cleanse;
    protected override int MaxCooldown => 5;

    public override void Cast(Player target)
    {
        base.Cast(target);
        var effectBurning = target.Effects.OfType<EffectBurning>().FirstOrDefault();
        if (effectBurning != null)
        {
            target.RemoveEffect(effectBurning);
            effectBurning.SourceAbility.IsWaitingForEffectToExpire = false;
        }
    }
}
