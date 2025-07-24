using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser()
        {
            var user = new UserDto
            {
                Id = 1,
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Role = "Admin"
            };

            return Ok(user);
        }
    }
}
