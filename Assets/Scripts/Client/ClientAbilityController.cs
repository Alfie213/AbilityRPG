using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientAbilityController : MonoBehaviour
{
    public event Action<AbilityType> OnAbilityUsed;

    [SerializeField] private Client client;
    [SerializeField] private ClientAbilityView clientAbilityView;

    private readonly Dictionary<int, AbilityType> _abilityMap = new()
    {
        { 0, AbilityType.Attack },
        { 1, AbilityType.Barrier },
        { 2, AbilityType.Regeneration },
        { 3, AbilityType.Fireball },
        { 4, AbilityType.Cleanse }
    };

    private void OnEnable()
    {
        client.OnGameStateReceived += clientAbilityView.DisplayPlayerAbilities;
    }

    private void OnDisable()
    {
        client.OnGameStateReceived -= clientAbilityView.DisplayPlayerAbilities;
    }

    public void UseAbilityByIndex(int abilityIndex)
    {
        if (_abilityMap.TryGetValue(abilityIndex, out AbilityType selectedAbility))
            OnAbilityUsed?.Invoke(selectedAbility);
        else
            throw new ArgumentOutOfRangeException($"Incorrect ability index: {abilityIndex}");
    }
}
