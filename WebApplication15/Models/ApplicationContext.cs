using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication15.Models
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(a => a.UserId).IsRequired(false);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<DepositContract> DepositContracts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<CreditAccount> CreditAccounts { get; set; }
    }
}
