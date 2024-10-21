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

    [Header("EnemyAbilities")]
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
        if (gameState.CurrentState == GameStateType.Playing || gameState.CurrentState == GameStateType.GameOver)
        {
            UpdateGameStateUI(gameState);
            if (gameState.CurrentState == GameStateType.GameOver)
            {
                ShowGameOverLayout();
            }
        }
    }

    private void UpdateGameStateUI(GameState gameState) // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        DisplayHealth(gameState);
        DisplayEnemyAbilities(gameState);
        DisplayAllEffects(gameState);
    }

    private void ClearEffects(Transform effectsParent) // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        foreach (Transform effect in effectsParent)
        {
            Destroy(effect.gameObject);
        }
    }

    private void DisplayHealth(GameState gameState)
    {
        playerHealthTMP.text = gameState.Player.Health.Value.ToString();
        enemyHealthTMP.text = gameState.Enemy.Health.Value.ToString();
    }

    private void DisplayEnemyAbilities(GameState gameState)
    {
        UpdateAbilityButton(enemyAttackButton, gameState.Enemy.Abilities[AbilityType.Attack], AttackButtonTMP);
        UpdateAbilityButton(enemyBarrierButton, gameState.Enemy.Abilities[AbilityType.Barrier], BarrierButtonTMP);
        UpdateAbilityButton(enemyRegenerationButton, gameState.Enemy.Abilities[AbilityType.Regeneration], RegenerationButtonTMP);
        UpdateAbilityButton(enemyFireballButton, gameState.Enemy.Abilities[AbilityType.Fireball], FireballButtonTMP);
        UpdateAbilityButton(enemyCleanseButton, gameState.Enemy.Abilities[AbilityType.Cleanse], CleanseButtonTMP);
    }

    private void UpdateAbilityButton(Button button, AbilityBase ability, string defaultText)
    {
        var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = ability.CurrentCooldown <= 0 ? defaultText : ability.CurrentCooldown.ToString();
    }

    private void DisplayAllEffects(GameState gameState) // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        ClearEffects(playerEffectsParent);
        ClearEffects(enemyEffectsParent);

        DisplayEffects(gameState.Player.Effects, playerEffectsParent);
        DisplayEffects(gameState.Enemy.Effects, enemyEffectsParent);
    }

    private void DisplayEffects(List<EffectBase> effects, Transform effectsParent) // Requires optimization to minimize the calls to Destroy and Instantiate.
    {
        foreach (var effect in effects)
        {
            GameObject effectUI = effect.Type switch
            {
                EffectType.Barrier => Instantiate(barrierEffectUIPrefab, effectsParent),
                EffectType.Burning => Instantiate(burningEffectUIPrefab, effectsParent),
                EffectType.Regeneration => Instantiate(regenerationEffectUIPrefab, effectsParent),
                _ => throw new ArgumentOutOfRangeException()
            };
            effectUI.GetComponentInChildren<TextMeshProUGUI>().text = effect.CurrentDuration.ToString();
        }
    }

    private void ShowGameOverLayout()
    {
        gameOverLayout.SetActive(true);
    }
}
