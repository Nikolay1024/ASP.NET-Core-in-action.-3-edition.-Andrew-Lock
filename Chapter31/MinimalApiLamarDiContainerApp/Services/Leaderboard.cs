namespace MinimalApiLamarDiContainerApp.Services
{
    public class Leaderboard<T> : ILeaderboard<T>
    {
        public int GetPosition(object user) => 1;
    }
}
