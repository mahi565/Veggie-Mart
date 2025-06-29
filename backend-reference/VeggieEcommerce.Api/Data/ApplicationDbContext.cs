using Microsoft.EntityFrameworkCore;
using VeggieEcommerce.Api.Models;

namespace VeggieEcommerce.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Vegetable> Vegetables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.State).HasMaxLength(50);
                entity.Property(e => e.ZipCode).HasMaxLength(20);
            });
            
            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
            });
            
            // Vegetable configuration
            modelBuilder.Entity<Vegetable>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.Unit).HasMaxLength(50);
                entity.Property(e => e.NutritionInfo).HasMaxLength(1000);
                entity.Property(e => e.Origin).HasMaxLength(200);
                
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Vegetables)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShippingAddress).HasMaxLength(1000);
                entity.Property(e => e.PaymentMethod).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.Vegetable)
                      .WithMany(v => v.OrderItems)
                      .HasForeignKey(e => e.VegetableId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // Seed data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Leafy Greens", Description = "Fresh leafy vegetables" },
                new Category { Id = 2, Name = "Root Vegetables", Description = "Nutritious root vegetables" },
                new Category { Id = 3, Name = "Fruits", Description = "Fresh vegetables that are technically fruits" },
                new Category { Id = 4, Name = "Herbs", Description = "Fresh aromatic herbs" }
            );
            
            // Seed Vegetables
            modelBuilder.Entity<Vegetable>().HasData(
                new Vegetable
                {
                    Id = 1,
                    Name = "Fresh Spinach",
                    Description = "Organic fresh spinach leaves, perfect for salads and cooking",
                    Price = 3.99m,
                    ImageUrl = "https://images.pexels.com/photos/2255925/pexels-photo-2255925.jpeg",
                    CategoryId = 1,
                    InStock = true,
                    StockQuantity = 50,
                    Unit = "bunch",
                    IsOrganic = true,
                    Origin = "Local Farm"
                },
                new Vegetable
                {
                    Id = 2,
                    Name = "Organic Carrots",
                    Description = "Sweet and crunchy organic carrots, great for snacking and cooking",
                    Price = 2.49m,
                    ImageUrl = "https://images.pexels.com/photos/143133/pexels-photo-143133.jpeg",
                    CategoryId = 2,
                    InStock = true,
                    StockQuantity = 75,
                    Unit = "lb",
                    IsOrganic = true,
                    Origin = "California"
                }
            );
        }
    }
}