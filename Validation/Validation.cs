using Communication.TransactionCoordinator;
using Communication.Validation;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;

namespace Validation
{
    internal sealed class Validation : StatelessService, IValidation
    {
        private readonly string transactionCoordinatorPath = @"fabric:/CloudVezbe/TransactionCoordinator";

        public Validation(StatelessServiceContext context)
            : base(context)
        { }

        #region IValidationImplementation

        public async Task<List<string>> ListAvailableItems()
        {
            ITransactionCoordinator? transactionProxy = ServiceProxy.Create<ITransactionCoordinator>(new Uri(transactionCoordinatorPath));

            return await transactionProxy.ListAvailableItems();
        }

        public async Task<string> EnlistPurchase(long? bookId, uint? count)
        {
            if (bookId is null || count is null)
            {
                return null!;
            }

            ITransactionCoordinator? transactionProxy = ServiceProxy.Create<ITransactionCoordinator>(new Uri(transactionCoordinatorPath));

            try
            {
                await transactionProxy.EnlistPurchase(bookId!.Value, count!.Value);

                return string.Empty;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public async Task<string> GetItemPrice(long? bookId)
        {
            if (bookId is null)
            {
                return null!;
            }

            ITransactionCoordinator? transactionProxy = ServiceProxy.Create<ITransactionCoordinator>(new Uri(transactionCoordinatorPath));

            return (await transactionProxy.GetItemPrice(bookId!.Value)).ToString();
        }

        public async Task<string> GetItem(long? bookId)
        {
            if (bookId is null)
            {
                return null!;
            }

            ITransactionCoordinator? transactionProxy = ServiceProxy.Create<ITransactionCoordinator>(new Uri(transactionCoordinatorPath));

            return (await transactionProxy.GetItem(bookId!.Value)).ToString();
        }

        public async Task<List<string>> ListClients()
        {
            ITransactionCoordinator? transactionProxy = ServiceProxy.Create<ITransactionCoordinator>(new Uri(transactionCoordinatorPath));

            return await transactionProxy.ListClients();
        }

        public async Task<string> EnlistMoneyTransfer(long? userSend, long? userReceive, double? amount)
        {
            if (userSend is null || userReceive is null || amount is null)
            {
                return null!;
            }

            ITransactionCoordinator? transactionProxy = ServiceProxy.Create<ITransactionCoordinator>(new Uri(transactionCoordinatorPath));

            return (await transactionProxy.EnlistMoneyTransfer(userSend!.Value, userReceive!.Value, amount!.Value)).ToString();
        }

        #endregion

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}
