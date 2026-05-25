namespace MinimalApiLamarDiContainerApp.Services
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
