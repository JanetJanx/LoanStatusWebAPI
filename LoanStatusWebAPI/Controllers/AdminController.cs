using LoanStatusWebAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoanStatusWebAPI.Models;

namespace LoanStatusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LoanStatusWebAPIContext _context;

        public AdminController(LoanStatusWebAPIContext context)
        {
            _context = context;
        }

        [HttpGet("statistics")]
        public IActionResult GetStatistics()
        {
            var statistics = new
            {
                TotalRequests = _context.Logtrails.Count(),
                SuccessfulRequests = _context.Logtrails.Where(u => u.Status == "error").Count(),
                FailedRequests = _context.Logtrails.Where(u => u.Status == "success").Count(),
                ConflictRequests = _context.Logtrails.Where(u => u.Status == "conflict").Count(),
            };

            return StatusCode(100, new { status = "success", Message = statistics });
        }
    }
}
