using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers
{

    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOrder()
        {
            var order = new
            {
                OrderId = 1001,
                UserId = 1,
                Product = "Mouse",
                Amount = 599,
                Status = "Confirmed"
            };
            return Ok(order);
        }
    }

}
