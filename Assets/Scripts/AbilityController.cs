using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public event Action<AbilityType> OnAbilityUsed;
    
    private readonly Dictionary<int, AbilityType> _abilityMap = new()
    {
        { 0, AbilityType.Attack },
        { 1, AbilityType.Barrier },
        { 2, AbilityType.Regeneration },
        { 3, AbilityType.Fireball },
        { 4, AbilityType.Cleanse }
    };

    public void UseAbilityByIndex(int abilityIndex)
    {
        if (_abilityMap.TryGetValue(abilityIndex, out AbilityType selectedAbility))
        {
            UseAbility(selectedAbility);
        }
        else
        {
            Debug.LogError("Incorrect ability index: " + abilityIndex);
        }
    }

    private void UseAbility(AbilityType abilityType)
    {
        Debug.Log("Ability used: " + abilityType);
        OnAbilityUsed?.Invoke(abilityType);
    }
}