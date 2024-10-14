using System;
using UnityEngine;

public class Server : MonoBehaviour, IGameServerAdapter
{
    [SerializeField] private Client client;

    private readonly GameState _gameState = new();

    private void Start()
    {
        client.InitializeServerAdapter(this);
    }

    public void SubmitAbilityUsage(AbilityType abilityType)
    {
        ApplyPlayerAbilityUsage(abilityType);
        
        if (CheckGameOver())
        {
            Debug.Log("GameOver");
            return;
        }

        ApplyEnemyAction();

        ApplyEffects();
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
                _gameState.PlayerEffects.Add(new EffectBarrier());
                break;
            case AbilityType.Regeneration:
                _gameState.PlayerEffects.Add(new EffectRegeneration());
                break;
            case AbilityType.Fireball:
                _gameState.EnemyHealth -= new AbilityFireball().AttackValue;
                _gameState.EnemyEffects.Add(new EffectBurning());
                break;
            case AbilityType.Cleanse:
                _gameState.PlayerEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }
    
    private void ApplyEnemyAbilityUsage(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Attack:
                _gameState.PlayerHealth -= new AbilityAttack().AttackValue;
                break;
            case AbilityType.Barrier:
                _gameState.EnemyEffects.Add(new EffectBarrier());
                break;
            case AbilityType.Regeneration:
                _gameState.EnemyEffects.Add(new EffectRegeneration());
                break;
            case AbilityType.Fireball:
                _gameState.PlayerHealth -= new AbilityFireball().AttackValue;
                _gameState.PlayerEffects.Add(new EffectBurning());
                break;
            case AbilityType.Cleanse:
                _gameState.EnemyEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    private bool CheckGameOver()
    {
        return _gameState.EnemyHealth <= 0;
    }

    private void ApplyEnemyAction()
    {
        AbilityType randomAbilityType = AbilityBase.GetRandomAbilityType();
        ApplyEnemyAbilityUsage(randomAbilityType);
    }

    private void ApplyEffects()
    {
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
                _gameState.PlayerEffects.Remove(playerEffect);
        }

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
                _gameState.EnemyEffects.Remove(enemyEffect);
        }
    }
}
