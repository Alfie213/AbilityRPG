public interface IGameServerAdapter
{
    void SubmitAbilityUsage(AbilityType abilityType);
    GameState GameState { get; }
}
