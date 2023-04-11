using LoanStatusWebAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoanStatusWebAPI.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;

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

        [HttpPost("statistics")]
        public IActionResult GetStatistics(DateTime dateTime)
        {
            // Check if account number is null or empty
            if (dateTime == null)
            {
                LogAuditTrail("Authorised user", "Statistics", "error", "Date cannot be null or empty.");
                return StatusCode(99, new { status = "error", message = "Date number cannot be null or empty." });
            }

            var statistics = new
            {
                TotalRequests = GetTotalRequests(),
                SuccessfulRequests = GetSuccessfulRequests(),
                FailedRequests = GetFailedRequests(),
                ConflictedRequests = GetConflictedRequests(),
            };

            return StatusCode(100, new { status = "success", Message = statistics });
        }

        [NonAction]
        public int GetTotalRequests()
        {
            var setcount = _context.Logtrails.Count();
            if (setcount >= 0)
            {
                return setcount;
            }
            else
            {
                return 0;
            }
        }

        [NonAction]
        public int GetSuccessfulRequests()
        {
            var setcount = _context.Logtrails.Where(u => u.Status == "success").Count();
            if (setcount >= 0)
            {
                return setcount;
            }
            else
            {
                return 0;
            }
        }

        [NonAction]
        public int GetFailedRequests()
        {
            var setcount = _context.Logtrails.Where(u => u.Status == "error").Count();
            if (setcount >= 0)
            {
                return setcount;
            }
            else
            {
                return 0;
            }
        }

        [NonAction]
        public int GetConflictedRequests()
        {
            var setcount = _context.Logtrails.Where(u => u.Status == "conflict").Count();
            if (setcount >= 0)
            {
                return setcount;
            }
            else
            {
                return 0;
            }
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

    }
}
