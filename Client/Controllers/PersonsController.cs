using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        [HttpPost]
        [Route("BuyBook")]
        public async Task<IActionResult> BuyBook()
        {
            return Ok(null!);
        }
    }
}
