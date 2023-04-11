using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LoanStatusWebAPI.Data;
using LoanStatusWebAPI.Models;
using NuGet.Protocol;

namespace LoanStatusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LoanStatusWebAPIContext _context;
        private readonly IConfiguration _configuration;
        public UsersController(LoanStatusWebAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Permissions))
            {
                LogAuditTrail(user.Username, "CreateUser", "error", "Username, email, permissions and password are required.");
                return StatusCode(99, new { status = "error", message = "Username, email, permissions and password are required." });
            }
            if (_context.User.Any(u => u.Username == user.Username))
            {
                LogAuditTrail(user.Username, "CreateUser", "conflict", "Username already exists.");
                return StatusCode(99, new { status = "conflict", message = "Username already exists." });
            }
            _context.User.Add(user);
            _context.SaveChanges();
            LogAuditTrail(user.Username, "CreateUser", "success", "Username: " +user.Username+" created successfully");
            return StatusCode(100, new { status = "success", message = CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user)});
        }

        [HttpPost("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                LogAuditTrail(user.Username, "GetUserById", "error", "User with ID: " +id+ " not found successfully");
                return StatusCode(99, new { status = "error", message = "User not found" });
            }
            LogAuditTrail(user.Username, "GetUserById", "success", "User with ID: " + id + " exists");
            return StatusCode(100, new { status = "success", message = user });
        }

        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken([FromBody] User credentials)
        {
            var user = _context.User.SingleOrDefault(u => u.Username == credentials.Username && u.Password == credentials.Password);
            if (user == null)
            {
                LogAuditTrail(user.Username, "GenerateToken", "error", "User with username: " + credentials.Username + " isn't authorised");
                return StatusCode(99, new { status = "error", message = "Unauthorized user" });
            }
            var token = GenerateTokenForUser(credentials.Username, credentials.Password);
            LogAuditTrail(user.Username, "GenerateToken", "success", "Generated User token for: " + credentials.Username);
            return Ok(new { Token = token });
        }

        private string GenerateTokenForUser(string username, string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ygfyerfyu3ere25762763896e8hvchvhfv");

            // Authenticate User and create claims
            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, password),
                new Claim(ClaimTypes.AuthenticationMethod, "JWT")
            };
            // Create token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [NonAction]
        public void LogAuditTrail(string username, string endpoint, string status, string message)
        {
            try
            {
                Logtrails auditTrails = new Logtrails();
                auditTrails.EndPoint = endpoint;
                auditTrails.Status = status;
                auditTrails.DateCreated = DateTime.Now;
                auditTrails.ActionbY = username;
                auditTrails.Message = message;
                _context.Add(auditTrails);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //do nothing;
            }
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
