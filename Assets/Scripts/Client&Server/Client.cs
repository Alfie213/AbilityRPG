using System;
using UnityEngine;

public class Client : MonoBehaviour
{
    public event Action<GameState> OnGameStateReceived;
    
    [SerializeField] private AbilityController abilityController;
    
    private IGameServerAdapter _serverAdapter;
    private GameState _gameState;

    private void OnEnable()
    {
        abilityController.OnAbilityUsed += SubmitAbilityUsage;
    }

    private void OnDisable()
    {
        abilityController.OnAbilityUsed -= SubmitAbilityUsage;
    }

    public void InitializeServerAdapter(IGameServerAdapter serverAdapter)
    {
        _serverAdapter = serverAdapter;
    }

    private void SubmitAbilityUsage(AbilityType abilityType)
    {
        _serverAdapter.SubmitAbilityUsage(abilityType);
        _gameState = _serverAdapter.RequestGameState();
        OnGameStateReceived?.Invoke(_gameState);
    }
}
