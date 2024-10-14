using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    public event Action<AbilityType> OnAbilityUsed;

    [SerializeField] private Client client;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button barrierButton;
    [SerializeField] private Button regenerationButton;
    [SerializeField] private Button fireballButton;
    [SerializeField] private Button cleanseButton;
    
    private readonly Dictionary<int, AbilityType> _abilityMap = new()
    {
        { 0, AbilityType.Attack },
        { 1, AbilityType.Barrier },
        { 2, AbilityType.Regeneration },
        { 3, AbilityType.Fireball },
        { 4, AbilityType.Cleanse }
    };
    
    private readonly Dictionary<AbilityType, AbilityBase> _abilities = new()
    {
        { AbilityType.Attack, new AbilityAttack() },
        { AbilityType.Barrier, new AbilityBarrier() },
        { AbilityType.Regeneration, new AbilityRegeneration() },
        { AbilityType.Fireball, new AbilityFireball() },
        { AbilityType.Cleanse, new AbilityCleanse() }
    };

    private const string AttackButtonTMP = "Attack";
    private const string BarrierButtonTMP = "Barrier";
    private const string RegenerationButtonTMP = "Regeneration";
    private const string FireballButtonTMP = "Fireball";
    private const string CleanseButtonTMP = "Cleanse";

    private void OnEnable()
    {
        client.OnGameStateReceived += ReduceCooldown;
    }

    private void OnDisable()
    {
        client.OnGameStateReceived -= ReduceCooldown;
    }

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
        CooldownAbility(abilityType);
        OnAbilityUsed?.Invoke(abilityType);
    }

    private void CooldownAbility(AbilityType abilityType)
    {
        AbilityBase ability = _abilities[abilityType];
        ability.CurrentCooldown = ability.Cooldown;
        
        switch (abilityType)
        {
            case AbilityType.Attack:
                attackButton.interactable = false;
                attackButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown.ToString();
                break;
            case AbilityType.Barrier:
                barrierButton.interactable = false;
                barrierButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown.ToString();
                break;
            case AbilityType.Regeneration:
                regenerationButton.interactable = false;
                regenerationButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown.ToString();
                break;
            case AbilityType.Fireball:
                fireballButton.interactable = false;
                fireballButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown.ToString();
                break;
            case AbilityType.Cleanse:
                cleanseButton.interactable = false;
                cleanseButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    private void ReduceCooldown(GameState _)
    {
        foreach (KeyValuePair<AbilityType,AbilityBase> abilityKeyValuePair in _abilities)
        {
            AbilityBase ability = abilityKeyValuePair.Value;

            if (ability.CurrentCooldown <= 0)
                return;

            ability.CurrentCooldown -= 1;
            
            switch (abilityKeyValuePair.Key)
            {
                case AbilityType.Attack:
                    attackButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown > 0 ? ability.CurrentCooldown.ToString() : AttackButtonTMP;
                    break;
                case AbilityType.Barrier:
                    barrierButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown > 0 ? ability.CurrentCooldown.ToString() : BarrierButtonTMP;
                    break;
                case AbilityType.Regeneration:
                    regenerationButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown > 0 ? ability.CurrentCooldown.ToString() : RegenerationButtonTMP;
                    break;
                case AbilityType.Fireball:
                    fireballButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown > 0 ? ability.CurrentCooldown.ToString() : FireballButtonTMP;
                    break;
                case AbilityType.Cleanse:
                    cleanseButton.GetComponentInChildren<TextMeshProUGUI>().text = ability.CurrentCooldown > 0 ? ability.CurrentCooldown.ToString() : CleanseButtonTMP;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityKeyValuePair), abilityKeyValuePair, null);
            }

            if (ability.CurrentCooldown > 0)
                return;
            
            switch (abilityKeyValuePair.Key)
            {
                case AbilityType.Attack:
                    attackButton.interactable = true;
                    break;
                case AbilityType.Barrier:
                    barrierButton.interactable = true;
                    break;
                case AbilityType.Regeneration:
                    regenerationButton.interactable = true;
                    break;
                case AbilityType.Fireball:
                    fireballButton.interactable = true;
                    break;
                case AbilityType.Cleanse:
                    cleanseButton.interactable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityKeyValuePair), abilityKeyValuePair, null);
            }
        }
    }
}