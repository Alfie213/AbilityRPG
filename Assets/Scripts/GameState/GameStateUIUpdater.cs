using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUIUpdater : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private GameObject gameOverLayout;
    
    [Header("Health")]
    [SerializeField] private TextMeshProUGUI playerHealthTMP;
    [SerializeField] private TextMeshProUGUI enemyHealthTMP;
    
    [Header("EnemyAbilities")] // ClientAbilityView displays the player's abilities.
    [SerializeField] private Button enemyAttackButton;
    [SerializeField] private Button enemyBarrierButton;
    [SerializeField] private Button enemyRegenerationButton;
    [SerializeField] private Button enemyFireballButton;
    [SerializeField] private Button enemyCleanseButton;
    private const string AttackButtonTMP = "Attack";
    private const string BarrierButtonTMP = "Barrier";
    private const string RegenerationButtonTMP = "Regeneration";
    private const string FireballButtonTMP = "Fireball";
    private const string CleanseButtonTMP = "Cleanse";
    
    [Header("Effects")]
    [SerializeField] private Transform playerEffectsParent;
    [SerializeField] private Transform enemyEffectsParent;
    [SerializeField] private GameObject barrierEffectUIPrefab;
    [SerializeField] private GameObject burningEffectUIPrefab;
    [SerializeField] private GameObject regenerationEffectUIPrefab;

    private void OnEnable()
    {
        client.OnGameStateReceived += Handle_OnGameStateReceived;
    }

    private void OnDisable()
    {
        client.OnGameStateReceived -= Handle_OnGameStateReceived;
    }

    private void Handle_OnGameStateReceived(GameState gameState)
    {
        // Debug.Log($"Player has {gameState.PlayerEffects.Count} effects.");
        // Debug.Log($"Enemy has {gameState.EnemyEffects.Count} effects.");
        switch (gameState.CurrentState)
        {
            case GameStateType.Playing:
                UpdateGameStateUI(gameState);
                break;
            case GameStateType.GameOver:
                UpdateGameStateUI(gameState);
                ShowGameOverLayout();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateGameStateUI(GameState gameState) // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        DisplayHealth(gameState);
        DisplayEnemyAbilities(gameState);
        DisplayAllEffects(gameState);
    }

    private void ClearEffects() // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        foreach (Transform playerEffect in playerEffectsParent)
            Destroy(playerEffect.gameObject);
        foreach (Transform enemyEffect in enemyEffectsParent)
            Destroy(enemyEffect.gameObject);
    }

    private void DisplayHealth(GameState gameState)
    {
        playerHealthTMP.text = gameState.PlayerHealth.ToString();
        enemyHealthTMP.text = gameState.EnemyHealth.ToString();
    }

    private void DisplayEnemyAbilities(GameState gameState)
    {
        foreach (KeyValuePair<AbilityType,AbilityBase> enemyAbilityKeyValuePair in gameState.EnemyAbilities)
        {
            switch (enemyAbilityKeyValuePair.Key)
            {
                case AbilityType.Attack:
                    enemyAttackButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        enemyAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? AttackButtonTMP
                            : enemyAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    break;
                case AbilityType.Barrier:
                    enemyBarrierButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        enemyAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? BarrierButtonTMP
                            : enemyAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    break;
                case AbilityType.Regeneration:
                    enemyRegenerationButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        enemyAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? RegenerationButtonTMP
                            : enemyAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    break;
                case AbilityType.Fireball:
                    enemyFireballButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        enemyAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? FireballButtonTMP
                            : enemyAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    break;
                case AbilityType.Cleanse:
                    enemyCleanseButton.GetComponentInChildren<TextMeshProUGUI>().text =
                        enemyAbilityKeyValuePair.Value.CurrentCooldown <= 0
                            ? CleanseButtonTMP
                            : enemyAbilityKeyValuePair.Value.CurrentCooldown.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void DisplayAllEffects(GameState gameState) // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        ClearEffects();
        
        foreach (EffectBase playerEffect in gameState.PlayerEffects)
        {
            GameObject effect = playerEffect.Type switch
            {
                EffectType.Barrier => Instantiate(barrierEffectUIPrefab, playerEffectsParent),
                EffectType.Burning => Instantiate(burningEffectUIPrefab, playerEffectsParent),
                EffectType.Regeneration => Instantiate(regenerationEffectUIPrefab, playerEffectsParent),
                _ => throw new ArgumentOutOfRangeException()
            };
            effect.GetComponentInChildren<TextMeshProUGUI>().text = playerEffect.CurrentDuration.ToString();
        }
        
        foreach (EffectBase enemyEffect in gameState.EnemyEffects)
        {
            GameObject effect = enemyEffect.Type switch
            {
                EffectType.Barrier => Instantiate(barrierEffectUIPrefab, enemyEffectsParent),
                EffectType.Burning => Instantiate(burningEffectUIPrefab, enemyEffectsParent),
                EffectType.Regeneration => Instantiate(regenerationEffectUIPrefab, enemyEffectsParent),
                _ => throw new ArgumentOutOfRangeException()
            };
            effect.GetComponentInChildren<TextMeshProUGUI>().text = enemyEffect.CurrentDuration.ToString();
        }
    }
    
    private void ShowGameOverLayout()
    {
        gameOverLayout.SetActive(true);
    }
}
