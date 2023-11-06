using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.GateInterfaces
{
    public interface IBookAsync : IService
    {
        Task SaveBookAsync(string book);

        Task<string> GetBookByIdAsync(long id);

        Task<List<string>> GetAllBooksAsync();
    }
}
