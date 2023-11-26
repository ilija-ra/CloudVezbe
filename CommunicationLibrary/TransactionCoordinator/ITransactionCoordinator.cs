using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.TransactionCoordinator
{
    public interface ITransactionCoordinator : IService
    {
        // BookstoreService
        Task<List<string>> ListAvailableItems();

        Task<int> EnlistPurchase(long? bookId, uint? count);

        Task<double> GetItemPrice(long? bookId);

        // BankService
        Task ListClients();

        Task<int> EnlistMoneyTransfer(long userId, double amount);
    }
}
