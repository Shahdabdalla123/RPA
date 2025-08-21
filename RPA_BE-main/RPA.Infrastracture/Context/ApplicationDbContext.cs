using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPA.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RPA.Infrastracture.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SeedRoles(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id="1", ConcurrencyStamp = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id="2", ConcurrencyStamp = "2", Name = "Employee", NormalizedName = "EMPLOYEE" },
                new IdentityRole { Id="3", ConcurrencyStamp = "3", Name = "Customer", NormalizedName = "CUSTOMER" }
            );
        }

    }
}
