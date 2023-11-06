using CommunicationLibrary.GateInterfaces;
using CommunicationLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Newtonsoft.Json;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        [HttpPost]
        [Route("Save")]
        public async Task Save([FromBody] Book book)
        {
            IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(book.Id!.Value % 3));

            string bookJson = JsonConvert.SerializeObject(book);

            await bookProxy.SaveBookAsync(bookJson);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<Book> GetById(long id)
        {
            IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(id % 3));

            Book bookModel = JsonConvert.DeserializeObject<Book>(await bookProxy.GetBookByIdAsync(id))!;

            return bookModel;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<List<Book>> GetAll()
        {
            IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(new Random().Next() % 3));

            var books = new List<Book>();

            List<string> booksJson = await bookProxy.GetAllBooksAsync();

            booksJson.ForEach(x => books.Add(JsonConvert.DeserializeObject<Book>(x)!));

            return books;
        }
    }
}
