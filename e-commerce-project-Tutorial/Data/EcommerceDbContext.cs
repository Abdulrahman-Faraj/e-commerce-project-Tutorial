using e_commerce_project_Tutorial.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_project_Tutorial.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            :base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Product>().HasData(
                
                new Product
                {
                    ProductId = 1,
                    Name = "Alpha Watch",
                    Description = "A smart and stylish watch from AlphaTech.",
                    Brand = "AlphaTech",
                    Price = 199.99m,
                    Discount = 5
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Alpha Mobile",
                    Description = "A high-performance mobile device by AlphaTech.",
                    Brand = "AlphaTech",
                    Price = 499.99m,
                    Discount = 10
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Alpha Laptop",
                    Description = "A lightweight and powerful laptop from AlphaTech.",
                    Brand = "AlphaTech",
                    Price = 999.99m,
                    Discount = 15
                },
                // BetaWorks products
                new Product
                {
                    ProductId = 4,
                    Name = "Beta Watch",
                    Description = "An elegant watch featuring modern functionalities by BetaWorks.",
                    Brand = "BetaWorks",
                    Price = 149.99m,
                    Discount = 5
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Beta Mobile",
                    Description = "An advanced mobile device with cutting-edge technology from BetaWorks.",
                    Brand = "BetaWorks",
                    Price = 599.99m,
                    Discount = 10
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Beta Laptop",
                    Description = "A powerful laptop built for both gaming and professional use by BetaWorks.",
                    Brand = "BetaWorks",
                    Price = 1099.99m,
                    Discount = 20
                },
                // GammaCorp products
                new Product
                {
                    ProductId = 7,
                    Name = "Gamma Watch",
                    Description = "A sporty watch with fitness tracking features from GammaCorp.",
                    Brand = "GammaCorp",
                    Price = 129.99m,
                    Discount = 5
                },
                new Product
                {
                    ProductId = 8,
                    Name = "Gamma Mobile",
                    Description = "A compact mobile device with excellent battery life by GammaCorp.",
                    Brand = "GammaCorp",
                    Price = 399.99m,
                    Discount = 5
                },
                new Product
                {
                    ProductId = 9,
                    Name = "Gamma Laptop",
                    Description = "A versatile laptop with long-lasting battery performance from GammaCorp.",
                    Brand = "GammaCorp",
                    Price = 899.99m,
                    Discount = 10
                }
            );
        }
       
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
