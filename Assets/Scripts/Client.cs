using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private AbilityController abilityController;
    
    private IGameServerAdapter _serverAdapter;

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
        // UpdateUI();
    }

    // Обновление интерфейса на основе состояния игры
    // private void UpdateUI()
    // {
    //     GameState state = serverAdapter.GetGameState();
    //     // Логика обновления интерфейса (полоски здоровья, эффекты и т.д.)
    // }
}
