using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityController : MonoBehaviour
{
    public UnityEvent onAbilityUsed;
    
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

    private void UseAbility(AbilityType action)
    {
        Debug.Log("Ability used: " + action);
        onAbilityUsed.Invoke();
    }
}