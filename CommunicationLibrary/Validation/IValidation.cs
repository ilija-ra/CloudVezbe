using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.Validation
{
    public interface IValidation : IService
    {
        // BookstoreService
        Task<List<string>> ValidateBookstoreListAvailableItems();

        Task<int> ValidateBookstoreEnlistPurchase(long? bookId, uint? count);

        Task<string> ValidateBookstoreGetItemPrice(long? id);

        // BankService
        Task<int> ValidateBankEnlistMoneyTransfer(long? userId, double? amount);
    }
}
