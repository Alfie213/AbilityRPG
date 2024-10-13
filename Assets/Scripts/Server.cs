using System;
using UnityEngine;

public class Server : MonoBehaviour, IGameServerAdapter
{
    [SerializeField] private Client client;

    public GameState GameState { get; } = new();

    private void Start()
    {
        client.InitializeServerAdapter(this);
    }

    public void SubmitAbilityUsage(AbilityType abilityType)
    {
        ApplyPlayerAbilityUsage(abilityType);

        ApplyEnemyAction();

        CheckGameOver();
    }

    private void ApplyPlayerAbilityUsage(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Attack:
                break;
            case AbilityType.Barrier:
                break;
            case AbilityType.Regeneration:
                break;
            case AbilityType.Fireball:
                break;
            case AbilityType.Cleanse:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    private void ApplyEnemyAction()
    {
        // Логика случайного выбора действия противника и его применения
    }

    private void CheckGameOver()
    {
        // Логика завершения игры (если один из юнитов умер)
    }
}
