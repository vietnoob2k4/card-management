// src/Controllers/CreatorController.cs
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using QLCapThe.Model;

namespace AppCapThe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorController : ControllerBase
    {
        private readonly QLCapTheV2Context _context;

        public CreatorController(QLCapTheV2Context context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var creator = _context.Creators
                .FirstOrDefault(c => c.Username == request.Username && c.Password == request.Password);

            if (creator != null)
            {
                return Ok(new { Message = "Login successful", CreatorId = creator.CreatorId });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
