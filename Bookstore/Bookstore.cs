using CommunicationLibrary.Bookstore;
using CommunicationLibrary.Models;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Newtonsoft.Json;
using System.Fabric;

namespace Bookstore
{
    internal sealed class Bookstore : StatefulService, IBookstore, CommunicationLibrary.TransactionCoordinator.ITransaction
    {
        private IReliableDictionary<long, Book> _bookDictionary;

        public Bookstore(StatefulServiceContext context)
            : base(context)
        { }

        private async Task InitializeBookDictionaryAsync()
        {
            _bookDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<long, Book>>("bookDictionary");
        }

        #region ITransaction

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

        #region IBookstoreImplementation

        public async Task<List<string>> ListAvailableItems()
        {
            await InitializeBookDictionaryAsync();

            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "To Kill a Mockingbird", Author = "Harper Lee", PagesNumber = 281, PublicationYear = 1960, Price = 9.99, Quantity = 10 },
                new Book(){ Id = 2, Title = "1984", Author = "George Orwell", PagesNumber = 328, PublicationYear = 1949, Price = 10.60, Quantity = 10 },
                new Book(){ Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", PagesNumber = 180, PublicationYear = 1925, Price = 22.35, Quantity = 10 },
                new Book(){ Id = 4, Title = "The Hobbit", Author = "J.R.R. Tolkien", PagesNumber = 310 , PublicationYear = 1937, Price = 50.00, Quantity = 10 },
                new Book(){ Id = 5, Title = "Pride and Prejudice", Author = "Jane Austen", PagesNumber = 279, PublicationYear = 1813, Price = 14.85, Quantity = 10 }
            };

            using (var transaction = this.StateManager.CreateTransaction())
            {
                foreach (Book book in books)
                    await _bookDictionary.AddOrUpdateAsync(transaction, book.Id!.Value, book, (k, v) => v);

                await transaction.CommitAsync();
            }

            var booksJson = new List<string>();

            using (var transaction = this.StateManager.CreateTransaction())
            {
                var enumerator = (await _bookDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

                while (await enumerator.MoveNextAsync(CancellationToken.None))
                {
                    var book = enumerator.Current.Value;
                    booksJson.Add(JsonConvert.SerializeObject(book));
                }
            }

            return booksJson;
        }

        public async Task<string> EnlistPurchase(long? bookId, uint? count)
        {
            using (var transaction = this.StateManager.CreateTransaction())
            {
                ConditionalValue<Book> book = await _bookDictionary.TryGetValueAsync(transaction, bookId!.Value);

                if (!book.HasValue)
                {
                    throw new KeyNotFoundException($"Book with Id {bookId} was not found.");
                }

                if (book.Value.Quantity == 0)
                {
                    throw new InvalidOperationException("There are no available books.");
                }

                var bookToUpdate = book.Value;

                bookToUpdate.Quantity -= Convert.ToInt32(count);

                await _bookDictionary.TryUpdateAsync(transaction, bookId!.Value, bookToUpdate, book.Value);

                await transaction.CommitAsync();

                return string.Empty;
            }
        }

        public async Task<string> GetItemPrice(long? bookId)
        {
            using (var transaction = this.StateManager.CreateTransaction())
            {
                var book = await _bookDictionary.TryGetValueAsync(transaction, bookId!.Value);

                return book.Value.Price!.Value.ToString();
            }

            throw null!;
        }

        public async Task<string> GetItem(long? bookId)
        {
            using (var transaction = this.StateManager.CreateTransaction())
            {
                var book = await _bookDictionary.TryGetValueAsync(transaction, bookId!.Value);

                return JsonConvert.SerializeObject(book.Value);
            }

            throw null!;
        }

        #endregion

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }
    }
}
