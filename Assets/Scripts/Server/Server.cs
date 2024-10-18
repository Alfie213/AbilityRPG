using System;
using R3;
using UnityEngine;

public class Server : MonoBehaviour, IGameServerAdapter
{
    [SerializeField] private Client client;

    private readonly ServerAbilityController _serverAbilityController = new();
    private readonly ServerEffectController _serverEffectController = new();
    private readonly GameState _gameState = new();

    private IDisposable _healthSubscription;

    private void Awake()
    {
        _gameState.CurrentState = GameStateType.Playing;
    }

    private void Start()
    {
        client.InitializeServerAdapter(this);
        _healthSubscription = Observable.Merge(
                _gameState.Player.Health.Where(health => health <= 0),
                _gameState.Enemy.Health.Where(health => health <= 0)
            )
            .Subscribe(health =>
            {
                _gameState.CurrentState = GameStateType.GameOver;
                Debug.Log("GameOver");
            });
    }

    public void SubmitAbilityUsage(AbilityType abilityType)
    {
        // Debug.Log($"Player: {_gameState.Player.Health}");
        // Debug.Log($"Enemy: {_gameState.Enemy.Health}");
        
        if (!_serverAbilityController.TrySubmitPlayerAbilityUsage(_gameState, abilityType))
        {
            // Handle cheating :) (Using cooldown abilities)
            // Logger.Log(...);
            Debug.LogError("Player is trying to use cooldown ability.");
        }

        _serverAbilityController.ImitateEnemyAbilityUsage(_gameState);
        
        _serverEffectController.ApplyEffects(_gameState);
        
        _serverAbilityController.ReduceCooldown(_gameState);
    }

    public GameState RequestGameState()
    {
        return _gameState;
    }

    private void OnDestroy()
    {
        _healthSubscription.Dispose();
    }
}
