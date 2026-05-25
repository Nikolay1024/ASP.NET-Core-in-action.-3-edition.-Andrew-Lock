namespace MinimalApiLamarDiContainerApp.Services
{
    public interface ILeaderboard<T>
    {
        int GetPosition(object user);
    }
}
