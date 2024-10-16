using System;
using UnityEngine;

public class Client : MonoBehaviour
{
    public event Action<GameState> OnGameStateReceived;

    [SerializeField] private ClientAbilityController clientAbilityController;
    
    private IGameServerAdapter _serverAdapter;
    private GameState _gameState;

    private void OnEnable()
    {
        clientAbilityController.OnAbilityUsed += SubmitAbilityUsage;
    }

    private void OnDisable()
    {
        clientAbilityController.OnAbilityUsed -= SubmitAbilityUsage;
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
