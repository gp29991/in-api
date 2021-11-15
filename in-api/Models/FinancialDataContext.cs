using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace in_api.Models
{
    public class FinancialDataContext : DbContext
    {
        public FinancialDataContext(DbContextOptions<FinancialDataContext> options) : base(options)
        {

        }

        public DbSet<FinancialData> FinancialData { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
