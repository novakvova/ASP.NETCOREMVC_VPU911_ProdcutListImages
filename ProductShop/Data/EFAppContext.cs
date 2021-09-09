using Microsoft.EntityFrameworkCore;
using ProductShop.Data.Configuration;
using ProductShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Data
{
    public class EFAppContext : DbContext
    {
        public EFAppContext(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductImage> productImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Product
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            #endregion

            #region ProductImage
            modelBuilder.ApplyConfiguration(new ProductImagesConfiguration());
            #endregion
        }
    }
}
