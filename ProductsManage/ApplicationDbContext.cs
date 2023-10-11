using Microsoft.EntityFrameworkCore;
using ProductsManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManage
{
  public class ApplicationDbContext:DbContext  
{
        protected override void OnConfiguring
           (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ProductDB");
        }
        public DbSet<ProductInfo> product { get; set; }
        public DbSet<Login> user { get; set; }
    }
}
