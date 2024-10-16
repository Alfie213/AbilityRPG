using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientAbilityView : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    [SerializeField] private Button barrierButton;
    [SerializeField] private Button regenerationButton;
    [SerializeField] private Button fireballButton;
    [SerializeField] private Button cleanseButton;
    
    private const string AttackButtonTMP = "Attack";
    private const string BarrierButtonTMP = "Barrier";
    private const string RegenerationButtonTMP = "Regeneration";
    private const string FireballButtonTMP = "Fireball";
    private const string CleanseButtonTMP = "Cleanse";
    
    public void DisplayPlayerAbilities(GameState gameState)
    {
        foreach (KeyValuePair<AbilityType,AbilityBase> playerAbilityKeyValuePair in gameState.PlayerAbilities)
        {
            switch (playerAbilityKeyValuePair.Key)
            {
                case AbilityType.Attack:
                    attackButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? AttackButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    attackButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Barrier:
                    barrierButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? BarrierButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    barrierButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Regeneration:
                    regenerationButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? RegenerationButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    regenerationButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Fireball:
                    fireballButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? FireballButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    fireballButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Cleanse:
                    cleanseButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? CleanseButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    cleanseButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
