using System;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour, IGameServerAdapter
{
    [SerializeField] private Client client;

    private readonly GameState _gameState = new();

    private void Awake()
    {
        _gameState.CurrentState = GameStateType.Playing;
    }

    private void Start()
    {
        client.InitializeServerAdapter(this);
    }

    public void SubmitAbilityUsage(AbilityType abilityType)
    {
        ApplyPlayerAbilityUsage(abilityType);

        if (CheckGameOver())
            return;

        ApplyEnemyAction();
        
        if (CheckGameOver())
            return;

        ApplyEffects();
        
        if (CheckGameOver())
            return;
    }

    public GameState RequestGameState()
    {
        return _gameState;
    }

    private void ApplyPlayerAbilityUsage(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Attack:
                _gameState.EnemyHealth -= new AbilityAttack().AttackValue;
                break;
            case AbilityType.Barrier:
                EffectBase effectBarrier = new EffectBarrier();
                effectBarrier.CurrentDuration = effectBarrier.Duration;
                _gameState.PlayerEffects.Add(effectBarrier);
                break;
            case AbilityType.Regeneration:
                EffectBase effectRegeneration = new EffectRegeneration();
                effectRegeneration.CurrentDuration = effectRegeneration.Duration;
                _gameState.PlayerEffects.Add(effectRegeneration);
                break;
            case AbilityType.Fireball:
                _gameState.EnemyHealth -= new AbilityFireball().AttackValue;
                EffectBase effectBurning = new EffectBurning();
                effectBurning.CurrentDuration = effectBurning.Duration;
                _gameState.EnemyEffects.Add(effectBurning);
                break;
            case AbilityType.Cleanse:
                _gameState.PlayerEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
        Debug.Log("<color=green>Player</color> Ability used: " + abilityType);
    }
    
    private void ApplyEnemyAbilityUsage(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Attack:
                _gameState.PlayerHealth -= new AbilityAttack().AttackValue;
                break;
            case AbilityType.Barrier:
                EffectBase effectBarrier = new EffectBarrier();
                effectBarrier.CurrentDuration = effectBarrier.Duration;
                _gameState.EnemyEffects.Add(effectBarrier);
                break;
            case AbilityType.Regeneration:
                EffectBase effectRegeneration = new EffectRegeneration();
                effectRegeneration.CurrentDuration = effectRegeneration.Duration;
                _gameState.EnemyEffects.Add(effectRegeneration);
                break;
            case AbilityType.Fireball:
                _gameState.PlayerHealth -= new AbilityFireball().AttackValue;
                EffectBase effectBurning = new EffectBurning();
                effectBurning.CurrentDuration = effectBurning.Duration;
                _gameState.PlayerEffects.Add(effectBurning);
                break;
            case AbilityType.Cleanse:
                _gameState.EnemyEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
        Debug.Log("<color=red>Enemy</color> Ability used: " + abilityType);
    }

    private bool CheckGameOver()
    {
        if (!(_gameState.PlayerHealth <= 0 || _gameState.EnemyHealth <= 0))
            return false;
        
        _gameState.CurrentState = GameStateType.GameOver;
        Debug.Log("GameOver");
        return true;
    }

    private void ApplyEnemyAction()
    {
        AbilityType randomAbilityType = AbilityBase.GetRandomAbilityType();
        ApplyEnemyAbilityUsage(randomAbilityType);
    }

    private void ApplyEffects()
    {
        List<EffectBase> playerEffectsToRemove = new();
        foreach (EffectBase playerEffect in _gameState.PlayerEffects)
        {
            switch (playerEffect.Type)
            {
                case EffectType.Barrier:
                    break;
                case EffectType.Burning:
                    _gameState.PlayerHealth -= ((EffectBurning)playerEffect).BurningValue;
                    break;
                case EffectType.Regeneration:
                    _gameState.PlayerHealth += ((EffectRegeneration)playerEffect).RegenerationValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            playerEffect.CurrentDuration -= 1;
            if (playerEffect.CurrentDuration <= 0)
                playerEffectsToRemove.Add(playerEffect);
        }
        foreach (EffectBase effect in playerEffectsToRemove)
            _gameState.PlayerEffects.Remove(effect);

        List<EffectBase> enemyEffectsToRemove = new();
        foreach (EffectBase enemyEffect in _gameState.EnemyEffects)
        {
            switch (enemyEffect.Type)
            {
                case EffectType.Barrier:
                    break;
                case EffectType.Burning:
                    _gameState.EnemyHealth -= ((EffectBurning)enemyEffect).BurningValue;
                    break;
                case EffectType.Regeneration:
                    _gameState.EnemyHealth += ((EffectRegeneration)enemyEffect).RegenerationValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            enemyEffect.CurrentDuration -= 1;
            if (enemyEffect.CurrentDuration <= 0)
                enemyEffectsToRemove.Add(enemyEffect);
        }
        foreach (EffectBase effect in enemyEffectsToRemove)
            _gameState.EnemyEffects.Remove(effect);
    }
}
