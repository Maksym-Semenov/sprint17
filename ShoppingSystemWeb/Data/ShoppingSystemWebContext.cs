#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingSystemWeb.Models;

namespace ShoppingSystemWeb.Data
{
    public class ShoppingSystemWebContext : DbContext
    {
        public ShoppingSystemWebContext (DbContextOptions<ShoppingSystemWebContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
