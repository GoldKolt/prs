using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication15.ViewModels;

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
            modelBuilder.Entity<DepositAccount>()
                .HasOne(p => p.DepositContract)
                .WithMany(t => t.Accounts)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CreditAccount>()
                .HasOne(p => p.CreditContract)
                .WithMany(t => t.Accounts)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<DepositContract> DepositContracts { get; set; }
        public DbSet<CreditContract> CreditContracts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<CreditAccount> CreditAccounts { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
