using CommunicationLibrary.Models;
using CommunicationLibrary.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Newtonsoft.Json;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookstoresController : ControllerBase
    {
        [HttpGet]
        [Route("ListAvailableItems")]
        public async Task<IActionResult> ListAvailableItems()
        {
            IValidation? validationProxy = ServiceProxy.Create<IValidation>(new Uri("fabric:/CloudVezbe/Validation"));

            var books = new List<Book>();

            List<string> booksJson = await validationProxy.ValidateBookstoreListAvailableItems();

            booksJson.ForEach(x => books.Add(JsonConvert.DeserializeObject<Book>(x)!));

            return Ok(books);
        }

        [HttpPost]
        [Route("EnlistPurchase")]
        public async Task<IActionResult> EnlistPurchase(long bookId, uint count)
        {
            IValidation? validationProxy = ServiceProxy.Create<IValidation>(new Uri("fabric:/CloudVezbe/Validation"));

            return Ok(await validationProxy.ValidateBookstoreEnlistPurchase(bookId, count));
        }

        [HttpGet]
        [Route("GetItemPrice/{id}")]
        public async Task<IActionResult> GetItemPrice(long id)
        {
            var validationProxy = ServiceProxy.Create<IValidation>(new Uri("fabric:/CloudVezbe/Validation"));

            return Ok(await validationProxy.ValidateBookstoreGetItemPrice(id));
        }

        //[HttpPost]
        //[Route("Save")]
        //public async Task Save([FromBody] Book book)
        //{
        //    IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(book.Id!.Value % 3));

        //    string bookJson = JsonConvert.SerializeObject(book);

        //    await bookProxy.SaveBookAsync(bookJson);
        //}

        //[HttpGet]
        //[Route("GetById/{id}")]
        //public async Task<Book> GetById(long id)
        //{
        //    IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(id % 3));

        //    Book bookModel = JsonConvert.DeserializeObject<Book>(await bookProxy.GetBookByIdAsync(id))!;

        //    return bookModel;
        //}

        //[HttpGet]
        //[Route("GetAll")]
        //public async Task<List<Book>> GetAll()
        //{
        //    IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(new Random().Next() % 3));

        //    var books = new List<Book>();

        //    List<string> booksJson = await bookProxy.GetAllBooksAsync();

        //    booksJson.ForEach(x => books.Add(JsonConvert.DeserializeObject<Book>(x)!));

        //    return books;
        //}

        //[HttpDelete]
        //[Route("Delete/{id}")]
        //public async Task Delete(long id)
        //{
        //    IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(id % 3));

        //    await bookProxy.DeleteBookAsync(id);
        //}

        //[HttpPost]
        //[Route("Purchase/{id}")]
        //public async Task Purchase(long id)
        //{
        //    IBookAsync? bookProxy = ServiceProxy.Create<IBookAsync>(new Uri("fabric:/CloudVezbe/ValidatorStatefulService"), new ServicePartitionKey(id % 3));

        //    await bookProxy.PurchaseBookAsync(id);
        //}
    }
}
