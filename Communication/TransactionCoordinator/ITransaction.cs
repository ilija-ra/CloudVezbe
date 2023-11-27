namespace Communication.TransactionCoordinator
{
    public interface ITransaction
    {
        Task<bool> Prepare();

        Task Commit();

        Task RollBack();
    }
}
