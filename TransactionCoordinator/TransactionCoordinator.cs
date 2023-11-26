using CommunicationLibrary.Bookstore;
using CommunicationLibrary.Enums;
using CommunicationLibrary.TransactionCoordinator;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace TransactionCoordinator
{
    internal sealed class TransactionCoordinator : StatelessService, ITransactionCoordinator, ITransaction
    {
        private readonly string bookstorePath = @"fabric:/CloudVezbe/Bookstore";

        public TransactionCoordinator(StatelessServiceContext context)
            : base(context)
        { }

        #region ITransactionCoordinatorImplementation

        public async Task<List<string>> ListAvailableItems()
        {
            IBookstore? bookstoreProxy = ServiceProxy.Create<IBookstore>(new Uri(bookstorePath), new ServicePartitionKey((int)PartiotionKeys.One));

            return await bookstoreProxy.ListAvailableItems();
        }

        public async Task<string> EnlistPurchase(long? bookId, uint? count)
        {
            IBookstore? bookstoreProxy = ServiceProxy.Create<IBookstore>(new Uri(bookstorePath), new ServicePartitionKey((int)PartiotionKeys.One));

            return (await bookstoreProxy.EnlistPurchase(bookId!.Value, count!.Value)).ToString();
        }

        public async Task<string> GetItemPrice(long? bookId)
        {
            IBookstore? bookstoreProxy = ServiceProxy.Create<IBookstore>(new Uri(bookstorePath), new ServicePartitionKey((int)PartiotionKeys.One));

            return (await bookstoreProxy.GetItemPrice(bookId!.Value)).ToString();
        }

        public async Task<string> GetItem(long? bookId)
        {
            IBookstore? bookstoreProxy = ServiceProxy.Create<IBookstore>(new Uri(bookstorePath), new ServicePartitionKey((int)PartiotionKeys.One));

            return await bookstoreProxy.GetItem(bookId!.Value);
        }

        public async Task<List<string>> ListClients()
        {
            throw new NotImplementedException();
        }

        public async Task<string> EnlistMoneyTransfer(long? userId, double? amount)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITransactionImplementation

        public Task Commit()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Prepare()
        {
            throw new NotImplementedException();
        }

        public Task RollBack()
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}
