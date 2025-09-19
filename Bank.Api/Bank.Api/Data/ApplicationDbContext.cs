using Bank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        // ctor :- shortcut key to create constructor
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        //{
        //type is optional 
        //}

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BankDetails> BankDetails { get; set; }
    }
}
