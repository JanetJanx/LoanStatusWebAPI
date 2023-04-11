using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanStatusWebAPI.Data;
using LoanStatusWebAPI.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LoanStatusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly LoanStatusWebAPIContext _context;

        public CustomersController(LoanStatusWebAPIContext context)
        {
            _context = context;
        }

        [HttpPost("ValidateAccountNumber")]
        public IActionResult ValidateAccountNumber([FromBody] string accountNumber)
        {
            // Check if account number is null or empty
            if (string.IsNullOrEmpty(accountNumber))
            {
                LogAuditTrail("Authorised user", "ValidateAccountNumber", "error", "Account number cannot be null or empty.");
                return StatusCode(99, new { status = "error", message = "Account number cannot be null or empty." });
            }
            // Check if account number has invalid characters
            Regex regex = new Regex("^[0-9]{10}$");
            if (!regex.IsMatch(accountNumber))
            {
                LogAuditTrail("Authorised user", "ValidateAccountNumber", "error", "Invalid account number. Account number must be 10 digits.");
                return StatusCode(99, new { status = "error", message = "Invalid account number. Account number must be 10 digits." });
            }
            // Retrieve the customer from the database
            Customer customer = _context.Customer.FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (customer == null)
            {
                LogAuditTrail("Authorised user", "ValidateAccountNumber", "error", "Invalid account number.");
                return StatusCode(99, new { status = "error", message = "Invalid account number." });
            }

            // Return the customer details upon successful validation
            var customerDetails = new
            {
                CustomerName = customer.Name,
                AccountNumber = customer.AccountNumber,
                CustomerAddress = customer.Address
            };
            LogAuditTrail("Authorised user", "ValidateAccountNumber", "success", "Account number validated successfully.");
            return StatusCode(100, new { status = "success", message = "Account number validated successfully.", CustomerDetails = customerDetails });
        }

        [HttpPost("GetLoanStatus")]
        public async Task<ActionResult> GetLoanStatus([FromBody] string accountNumber)
        {
            // Check if account number is null or empty
            if (string.IsNullOrEmpty(accountNumber))
            {
                LogAuditTrail("Authorised user", "GetLoanStatus", "error", "Account number cannot be null or empty.");
                return StatusCode(99, new { status = "error", message = "Account number cannot be null or empty." });
            }

            // Check if account number has invalid characters
            Regex regex = new Regex("^[0-9]{10}$");
            if (!regex.IsMatch(accountNumber))
            {
                LogAuditTrail("Authorised user", "GetLoanStatus", "error", "Invalid account number. Account number must be 10 digits.");
                return StatusCode(99, new { status = "error", message = "Invalid account number. Account number must be 10 digits." });
            }

            // Retrieve the customer from the database using the account number
            Customer customer = _context.Customer.Include(c => c.Loans).FirstOrDefault(c => c.AccountNumber == accountNumber);

            if (customer == null)
            {
                LogAuditTrail("Authorised user", "GetLoanStatus", "error", "Customer not found.");
                return StatusCode(99, new { status = "error", message = "Customer not found." }); // Customer not found
            }
            // Retrieve list of customer loans
            List<Loan> outstandingLoans = customer.Loans.Where(l => l.OutstandingAmount > 0).ToList();

            if (outstandingLoans.Count == 0)
            {
                LogAuditTrail("Authorised user", "GetLoanStatus", "success", "No outstanding loans found for customer.");
                return StatusCode(100, new { status = "success", Message = "No outstanding loans found for customer." });
            }
            // Otherwise, create a JSON object containing the outstanding loans information
            var loanStatus = new
            {
                CustomerId = customer.Id,
                AccountNumber = customer.AccountNumber,
                OutstandingLoans = outstandingLoans.Select(l => new
                {
                    LoanId = l.Id,
                    DisbursementDate = l.DisbursementDate,
                    OutstandingAmount = l.OutstandingAmount,
                    Status = (DateTime.Now - l.DisbursementDate).TotalDays > l.LoanPeriod ? "Overdue" : "Running"
                })
            };

            // Return the JSON object
            LogAuditTrail("Authorised user", "GetLoanStatus", "success", "Customer has standing loans : "+ JsonConvert.SerializeObject(loanStatus));
            return StatusCode(100, new { status = "success", Message = JsonConvert.SerializeObject(loanStatus) });
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
