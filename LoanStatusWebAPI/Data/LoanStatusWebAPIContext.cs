using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoanStatusWebAPI.Models;

namespace LoanStatusWebAPI.Data
{
    public class LoanStatusWebAPIContext : DbContext
    {
        public LoanStatusWebAPIContext (DbContextOptions<LoanStatusWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<LoanStatusWebAPI.Models.Customer> Customer { get; set; } = default!;

        public DbSet<LoanStatusWebAPI.Models.Loan>? Loan { get; set; }

        public DbSet<LoanStatusWebAPI.Models.User>? User { get; set; }
        public DbSet<LoanStatusWebAPI.Models.Logtrails>? Logtrails { get; set; }
    }
}
