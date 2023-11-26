using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.Bank
{
    public interface IBank : IService
    {
        Task ListClients();

        Task EnlistMoneyTransfer(long userId, double amount);
    }
}
