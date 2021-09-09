using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("tblProducts");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired().HasMaxLength(255);

            builder.Property(x => x.Price)
                .IsRequired();

        }
    }
}
