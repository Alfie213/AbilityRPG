using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateUIUpdater : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private TextMeshProUGUI playerHealthTMP;
    [SerializeField] private TextMeshProUGUI enemyHealthTMP;
    [SerializeField] private Transform playerEffectsParent;
    [SerializeField] private Transform enemyEffectsParent;
    [SerializeField] private GameObject barrierEffectUIPrefab;
    [SerializeField] private GameObject burningEffectUIPrefab;
    [SerializeField] private GameObject regenerationEffectUIPrefab;
    [SerializeField] private GameObject gameOverLayout;

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
        DisplayEffects(gameState);
        DisplayEnemyAbilities(gameState);
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

    private void DisplayEffects(GameState gameState) // Requires optimization to minimize the calls to Destroy and Instantiate.
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

    private void DisplayEnemyAbilities(GameState gameState)
    {
        foreach (KeyValuePair<AbilityType,AbilityBase> enemyAbilityKeyValuePair in gameState.EnemyAbilities)
        {
            switch (expression)
            {
                
            }
        }
    }
    
    private void ShowGameOverLayout()
    {
        gameOverLayout.SetActive(true);
    }
}
