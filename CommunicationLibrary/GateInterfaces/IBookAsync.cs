using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.GateInterfaces
{
    public interface IBookAsync : IService
    {
        Task SaveBookAsync(string book);

        Task<string> GetBookByIdAsync(long bookId);

        Task<List<string>> GetAllBooksAsync();

        Task DeleteBookAsync(long bookId);

        Task PurchaseBookAsync(long bookId);
    }
}
