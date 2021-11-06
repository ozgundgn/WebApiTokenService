using System;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
    public class NorthwindContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=OZGUN;Database=Northwind;Trusted_Connection=true");
        }

        public DbSet<User> Users { get; set; }
    }
}
