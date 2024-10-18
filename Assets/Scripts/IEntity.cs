using System.Collections.Generic;
using R3;

public interface IEntity
{
    ReactiveProperty<int> Health { get; }
    List<EffectBase> Effects { get; }

    void ApplyDamage(int damage);
    void Heal(int amount);
    void AddEffect(EffectBase effect);
    void RemoveEffect(EffectBase effect);
}