using CommunicationLibrary.GateInterfaces;
using CommunicationLibrary.Models;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Newtonsoft.Json;
using System.Fabric;

namespace ValidatorStatefulService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ValidatorStatefulService : StatefulService, IBookAsync
    {
        public ValidatorStatefulService(StatefulServiceContext context)
            : base(context)
        { }

        #region IBookAsyncImplementation

        public async Task SaveBookAsync(string book)
        {
            var stateManager = this.StateManager;
            var bookDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<long, Book>>("bookDictionary");
            Book bookModel = JsonConvert.DeserializeObject<Book>(book)!;

            using (var transaction = stateManager.CreateTransaction())
            {
                await bookDictionary.AddOrUpdateAsync(transaction, bookModel.Id!.Value, bookModel, (k, v) => v);
                await transaction.CommitAsync();
            }
        }

        public async Task<string> GetBookByIdAsync(long id)
        {
            var stateManager = this.StateManager;
            var bookDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<long, Book>>("bookDictionary");

            using (var transaction = stateManager.CreateTransaction())
            {
                var book = await bookDictionary.TryGetValueAsync(transaction, id);
                return JsonConvert.SerializeObject(book.Value);
            }

            throw new Exception();
        }

        public async Task<List<string>> GetAllBooksAsync()
        {
            var stateManager = this.StateManager;
            var bookDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<long, Book>>("bookDictionary");
            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "To Kill a Mockingbird", Author = "Harper Lee", PagesNumber = 281, PublicationYear = 1960, Price = 9.99, Quantity = 10 },
                new Book(){ Id = 2, Title = "1984", Author = "George Orwell", PagesNumber = 328, PublicationYear = 1949, Price = 10.60, Quantity = 10 },
                new Book(){ Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", PagesNumber = 180, PublicationYear = 1925, Price = 22.35, Quantity = 10 },
                new Book(){ Id = 4, Title = "The Hobbit", Author = "J.R.R. Tolkien", PagesNumber = 310 , PublicationYear = 1937, Price = 50.00, Quantity = 10 },
                new Book(){ Id = 5, Title = "Pride and Prejudice", Author = "Jane Austen", PagesNumber = 279, PublicationYear = 1813, Price = 14.85, Quantity = 10 }
            };

            using (var transaction = stateManager.CreateTransaction())
            {
                foreach (Book book in books)
                    await bookDictionary.AddOrUpdateAsync(transaction, book.Id!.Value, book, (k, v) => v);

                await transaction.CommitAsync();
            }

            var booksJson = new List<string>();

            using (var transaction = stateManager.CreateTransaction())
            {
                var enumerator = (await bookDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var book = enumerator.Current.Value;
                    booksJson.Add(JsonConvert.SerializeObject(book));
                }
            }

            return booksJson;
        }

        public async Task DeleteBookAsync(long id)
        {
            var stateManager = this.StateManager;
            var bookDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<long, Book>>("bookDictionary");

            using (var transaction = stateManager.CreateTransaction())
            {
                ConditionalValue<Book> existingBook = await bookDictionary.TryGetValueAsync(transaction, id);

                if (existingBook.HasValue)
                    await bookDictionary.TryRemoveAsync(transaction, id);
                else
                    throw new KeyNotFoundException($"Book with ID {id} not found.");

                await transaction.CommitAsync();
            }
        }

        public async Task PurchaseBookAsync(long bookId)
        {
            var stateManager = this.StateManager;
            var bookDictionary = await stateManager.GetOrAddAsync<IReliableDictionary<long, Book>>("bookDictionary");

            using (var transaction = stateManager.CreateTransaction())
            {
                ConditionalValue<Book> existingBook = await bookDictionary.TryGetValueAsync(transaction, bookId);

                if (existingBook.HasValue)
                {
                    var bookToUpdate = existingBook.Value;

                    if (bookToUpdate.Quantity > 0)
                    {
                        bookToUpdate.Quantity--;
                        await bookDictionary.TryUpdateAsync(transaction, bookId, bookToUpdate, existingBook.Value);
                    }
                    else
                        throw new InvalidOperationException("Quantity is already at 0.");
                }
                else
                    throw new KeyNotFoundException($"Book with ID {bookId} not found.");

                await transaction.CommitAsync();
            }
        }

        #endregion

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            //return new ServiceReplicaListener[0];
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
