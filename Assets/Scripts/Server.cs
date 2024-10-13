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
        
        if (CheckGameOver())
        {
            Debug.Log("GameOver");
            return;
        }

        ApplyEnemyAction();
    }

    private void ApplyPlayerAbilityUsage(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Attack:
                GameState.EnemyHealth -= new AbilityAttack().AttackValue;
                break;
            case AbilityType.Barrier:
                GameState.PlayerEffects.Add(new EffectBarrier());
                break;
            case AbilityType.Regeneration:
                GameState.PlayerEffects.Add(new EffectRegeneration());
                break;
            case AbilityType.Fireball:
                GameState.EnemyHealth -= new AbilityFireball().AttackValue;
                GameState.EnemyEffects.Add(new EffectBurning());
                break;
            case AbilityType.Cleanse:
                GameState.PlayerEffects.RemoveAll(ability => ability is EffectBurning);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
        }
    }

    private bool CheckGameOver()
    {
        return GameState.EnemyHealth <= 0;
    }

    private void ApplyEnemyAction()
    {
        // Логика случайного выбора действия противника и его применения
    }
}
