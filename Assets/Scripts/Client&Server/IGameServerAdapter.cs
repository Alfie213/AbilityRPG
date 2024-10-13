public interface IGameServerAdapter
{
    void SubmitAbilityUsage(AbilityType abilityType);
    GameState RequestGameState();
}
