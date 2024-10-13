using UnityEngine;

public class Client : MonoBehaviour
{
    private IGameServerAdapter _serverAdapter;

    public void InitializeServerAdapter(IGameServerAdapter serverAdapter)
    {
        _serverAdapter = serverAdapter;
    }
    
    public void SubmitAbilityUsage(AbilityType abilityType)
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
