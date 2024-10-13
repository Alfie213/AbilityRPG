using System;
using TMPro;
using UnityEngine;

public class UIGameStateObserver : MonoBehaviour
{
    [SerializeField] private Client client;
    [SerializeField] private TextMeshProUGUI playerHealthTMP;
    [SerializeField] private TextMeshProUGUI enemyHealthTMP;
    [SerializeField] private Transform playerEffectsParent;
    [SerializeField] private Transform enemyEffectsParent;
    [SerializeField] private GameObject barrierEffectUIPrefab;
    [SerializeField] private GameObject burningEffectUIPrefab;
    [SerializeField] private GameObject regenerationEffectUIPrefab;

    private void OnEnable()
    {
        client.OnGameStateReceived += UpdateUI;
    }

    private void OnDisable()
    {
        client.OnGameStateReceived -= UpdateUI;
    }

    private void UpdateUI(GameState gameState)
    {
        ClearEffects();
        playerHealthTMP.text = gameState.PlayerHealth.ToString();
        enemyHealthTMP.text = gameState.EnemyHealth.ToString();
        
        foreach (EffectBase playerEffect in gameState.PlayerEffects)
        {
            GameObject effect = playerEffect.Type switch
            {
                EffectType.Barrier => Instantiate(barrierEffectUIPrefab, playerEffectsParent),
                EffectType.Burning => Instantiate(burningEffectUIPrefab, playerEffectsParent),
                EffectType.Regeneration => Instantiate(regenerationEffectUIPrefab, playerEffectsParent),
                _ => throw new ArgumentOutOfRangeException()
            };
            effect.GetComponentInChildren<TextMeshProUGUI>().text = playerEffect.Duration.ToString();
        }
        
        foreach (EffectBase enemyEffect in gameState.EnemyEffects)
        {
            GameObject effect = enemyEffect.Type switch
            {
                EffectType.Barrier => Instantiate(barrierEffectUIPrefab, playerEffectsParent),
                EffectType.Burning => Instantiate(burningEffectUIPrefab, playerEffectsParent),
                EffectType.Regeneration => Instantiate(regenerationEffectUIPrefab, playerEffectsParent),
                _ => throw new ArgumentOutOfRangeException()
            };
            effect.GetComponentInChildren<TextMeshProUGUI>().text = enemyEffect.Duration.ToString();
        }
    }

    private void ClearEffects()
    {
        foreach (Transform playerEffect in playerEffectsParent)
            Destroy(playerEffect.gameObject);
        foreach (Transform enemyEffect in enemyEffectsParent)
            Destroy(enemyEffect.gameObject);
    }
}
