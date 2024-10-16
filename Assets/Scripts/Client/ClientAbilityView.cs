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
        foreach (KeyValuePair<AbilityType,AbilityBase> playerAbilityKeyValuePair in gameState.Player.Abilities)
        {
            switch (playerAbilityKeyValuePair.Key)
            {
                case AbilityType.Attack:
                    playerAttackButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? AttackButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    playerAttackButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Barrier:
                    playerBarrierButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? BarrierButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    playerBarrierButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Regeneration:
                    playerRegenerationButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? RegenerationButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    playerRegenerationButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Fireball:
                    playerFireballButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? FireballButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    playerFireballButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                case AbilityType.Cleanse:
                    playerCleanseButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        playerAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? CleanseButtonTMP
                            : playerAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    playerCleanseButton.interactable = playerAbilityKeyValuePair.Value.CurrentCooldown <= 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
