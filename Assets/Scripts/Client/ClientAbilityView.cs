using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientAbilityView : MonoBehaviour
{
    [SerializeField] private Button playerAttackButton;
    [SerializeField] private Button playerBarrierButton;
    [SerializeField] private Button playerRegenerationButton;
    [SerializeField] private Button playerFireballButton;
    [SerializeField] private Button playerCleanseButton;
    
    private const string AttackButtonTMP = "Attack";
    private const string BarrierButtonTMP = "Barrier";
    private const string RegenerationButtonTMP = "Regeneration";
    private const string FireballButtonTMP = "Fireball";
    private const string CleanseButtonTMP = "Cleanse";
    
    public void DisplayPlayerAbilities(GameState gameState)
    {
        foreach (KeyValuePair<AbilityType, AbilityBase> playerAbilityKeyValuePair in gameState.Player.Abilities)
        {
            AbilityBase ability = playerAbilityKeyValuePair.Value;
            bool isWaitingForEffect = ability.IsWaitingForEffect;
            string buttonText = GetButtonText(ability);
            bool interactable = !isWaitingForEffect && ability.IsReady;

            UpdateButton(playerAbilityKeyValuePair.Key, buttonText, interactable);
        }
    }

    private string GetButtonText(AbilityBase ability)
    {
        if (ability.IsWaitingForEffect || ability.IsReady)
        {
            return ability.Type switch
            {
                AbilityType.Attack => AttackButtonTMP,
                AbilityType.Barrier => BarrierButtonTMP,
                AbilityType.Regeneration => RegenerationButtonTMP,
                AbilityType.Fireball => FireballButtonTMP,
                AbilityType.Cleanse => CleanseButtonTMP,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
        return ability.CurrentCooldown.ToString();
    }
    
    private void UpdateButton(AbilityType abilityType, string buttonText, bool interactable)
    {
        Button button = abilityType switch
        {
            AbilityType.Attack => playerAttackButton,
            AbilityType.Barrier => playerBarrierButton,
            AbilityType.Regeneration => playerRegenerationButton,
            AbilityType.Fireball => playerFireballButton,
            AbilityType.Cleanse => playerCleanseButton,
            _ => throw new ArgumentOutOfRangeException()
        };

        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        button.interactable = interactable;
    }
}
