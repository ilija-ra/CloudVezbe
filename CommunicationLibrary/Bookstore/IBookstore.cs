using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.Bookstore
{
    public interface IBookstore : IService
    {
        Task<List<string>> ListAvailableItems();

        Task EnlistPurchase(long? bookId, uint? count);

        Task<double> GetItemPrice(long? bookId);
    }
}
