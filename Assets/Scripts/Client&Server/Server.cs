using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Server : MonoBehaviour, IGameServerAdapter
{
    [SerializeField] private Client client;

    private readonly ServerAbilityController _serverAbilityController = new();
    private readonly ServerEffectController _serverEffectController = new();
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
        _serverAbilityController.SubmitPlayerAbilityUsage(_gameState, abilityType);

        if (CheckGameOver())
            return;

        _serverAbilityController.ImitateEnemyAbilityUsage(_gameState);
        
        if (CheckGameOver())
            return;

        _serverEffectController.ApplyEffects(_gameState);
        
        if (CheckGameOver())
            return;
        
        _serverAbilityController.ReduceCurrentCooldown(_gameState);
        _serverEffectController.ReduceCurrentDuration(_gameState);
    }

    public GameState RequestGameState()
    {
        return _gameState;
    }

    private bool CheckGameOver()
    {
        if (!(_gameState.PlayerHealth <= 0 || _gameState.EnemyHealth <= 0))
            return false;
        
        _gameState.CurrentState = GameStateType.GameOver;
        Debug.Log("GameOver");
        return true;
    }
}
