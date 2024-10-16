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
        Debug.Log($"Player: {_gameState.Player.Health}");
        Debug.Log($"Enemy: {_gameState.Enemy.Health}");
        
        if (!_serverAbilityController.TrySubmitPlayerAbilityUsage(_gameState, abilityType))
        {
            // Handle cheating :) (Using cooldown abilities)
            // Logger.Log(...);
            Debug.LogError("Player is trying to use cooldown ability.");
        }

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
        if (!(_gameState.Player.Health <= 0 || _gameState.Enemy.Health <= 0))
            return false;
        
        _gameState.CurrentState = GameStateType.GameOver;
        Debug.Log("GameOver");
        return true;
    }
}
