using Microsoft.ServiceFabric.Services.Remoting;

namespace CommunicationLibrary.Bank
{
    public interface IBank : IService
    {
        Task<List<string>> ListClients();

        Task<string> EnlistMoneyTransfer(long? userSend, long? userReceive, double? amount);
    }
}
