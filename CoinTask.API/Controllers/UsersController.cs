using System.Linq;
using CoinTask.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoinTask.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers() 
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
    }
}