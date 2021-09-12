using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Data.Services
{
    public static class DbSeeder
    {
        public static void SeedAll(this IApplicationBuilder app) 
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                EFAppContext context = scope.ServiceProvider.GetRequiredService<EFAppContext>();
                if (!context.Products.Any())
                {
                    SeedProducts(context);
                }
            }
        }
        private static void SeedProducts(EFAppContext context) 
        {

            var product1 = new Product
            {
                Name = "Airpods PRO",
                Price = 6000
            };

            var product2 = new Product
            {
                Name = "Airpods",
                Price = 3600
            };
            context.Products.AddRange(new List<Product> {
                product1,
               product2
            });
            context.SaveChanges();



            ProductImage image1 = new ProductImage
            {
                Name = "image1.jpg",
                Product = product1
            };

            ProductImage image2 = new ProductImage
            {
                Name = "image2.jpg",
                Product = product2
            };

            context.ProductImages.AddRange(new List<ProductImage> {
                image1,
                image2
            });
            context.SaveChanges();
        }
    }
}
