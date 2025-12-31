using MarketPlace.Domain;
using MarketPlace.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Infrastructure.Persistance.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options) { }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "System";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "System";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary key and table mapping for entities
            modelBuilder.Entity<CartItem>().ToTable("CartItems").HasKey(e => e.Id);
            modelBuilder.Entity<Category>().ToTable("Categories").HasKey(e => e.Id);
            modelBuilder.Entity<Order>().ToTable("Orders").HasKey(e => e.Id);
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems").HasKey(e => e.Id);
            modelBuilder.Entity<Payment>().ToTable("Payments").HasKey(e => e.Id);
            modelBuilder.Entity<Product>().ToTable("Products").HasKey(e => e.Id);
            modelBuilder.Entity<Review>().ToTable("Reviews").HasKey(e => e.Id);
            modelBuilder.Entity<User>().ToTable("Users").HasKey(e => e.Id);

            // CartItem relationships
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.Cart) // User.Cart
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.InCart) // Product.InCart
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category -> Product (Category.Productos)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> Seller (User)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany() // No navegación inversa en User para productos vendidos
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany() // No navegación inversa en User para orders
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order -> OrderItems
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem -> Product (Product.Products representa los OrderItems en el dominio)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.Products)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order <-> Payment (one-to-one). Payment.PedidoId is FK
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Pago)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review -> User and Review -> Product
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany() // User no tiene colección de Reviews definida
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
