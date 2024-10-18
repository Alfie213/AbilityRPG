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

public abstract class AbilityWithEffectBase : AbilityBase
{
    protected abstract EffectBase CreateEffect();

    public override void Cast(Player target)
    {
        base.Cast(target);
        var effect = CreateEffect();
        target.AddEffect(effect);
        effect.CurrentDuration.Where(duration => duration <= 0).Subscribe(_ =>
        {
            target.RemoveEffect(effect);
            IsWaitingForEffectToExpire = false;
        });
    }
}

public class AbilityAttack : AbilityBase
{
    public override AbilityType Type => AbilityType.Attack;
    public override int MaxCooldown => 0;
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
    public override int MaxCooldown => 4;

    protected override EffectBase CreateEffect() => new EffectBarrier();

    public override void Cast(Player target)
    {
        base.Cast(target);
        var effectBarrier = target.Effects.OfType<EffectBarrier>().FirstOrDefault();
        if (effectBarrier != null)
        {
            effectBarrier.CurrentBarrier.Where(currentBarrier => currentBarrier <= 0).Subscribe(_ =>
            {
                target.RemoveEffect(effectBarrier);
                IsWaitingForEffectToExpire = false;
            });
        }
    }
}

public class AbilityRegeneration : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Regeneration;
    public override int MaxCooldown => 5;

    protected override EffectBase CreateEffect() => new EffectRegeneration();
}

public class AbilityFireball : AbilityWithEffectBase
{
    public override AbilityType Type => AbilityType.Fireball;
    public override int MaxCooldown => 6;
    private const int FireballDamage = 5;

    public override void Cast(Player target)
    {
        base.Cast(target);
        target.ApplyDamage(FireballDamage);
    }

    protected override EffectBase CreateEffect() => new EffectBurning();
}

public class AbilityCleanse : AbilityBase
{
    public override AbilityType Type => AbilityType.Cleanse;
    public override int MaxCooldown => 5;

    public override void Cast(Player target)
    {
        base.Cast(target);
        EffectBase effect = target.Effects.Find(e => e is EffectBurning);
        if (effect != null)
            target.RemoveEffect(effect);
    }
}
