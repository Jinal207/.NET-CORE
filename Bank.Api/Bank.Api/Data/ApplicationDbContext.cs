using Bank.Api.Models;
using FileUploadExample.Models;
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
        public DbSet<UserAccounts> UserAccount { get; set; }
        public DbSet<FileUploadModel> UploadedFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            _ = modelBuilder.Entity<UserAccounts>().HasData([
                new UserAccounts{
                    Id = 1,
                    UserName = "admin",
                    Password = "admin"
                }
                ]);

            modelBuilder.Entity<FileUploadModel>().Ignore(f => f.File);

            base.OnModelCreating(modelBuilder);
        }
    }
}
