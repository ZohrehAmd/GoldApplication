using GoldApplication.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Contexts
{
    public class GoldApplicationContext : DbContext
    {
        public GoldApplicationContext(DbContextOptions<GoldApplicationContext> options): base (options)
        {}
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductUser> ProductUsers { get; set; }
        public DbSet<ProductEvent> ProductEvents { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
    }
}
