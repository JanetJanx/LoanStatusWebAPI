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
using System.Text.Json;
using Newtonsoft.Json;

namespace LoanStatusWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LoanStatusWebAPIContext _context;

        public LoansController(LoanStatusWebAPIContext context)
        {
            _context = context;
        }
    }
}
