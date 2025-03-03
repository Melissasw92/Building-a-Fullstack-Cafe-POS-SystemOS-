using FourthWallCafe.MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options)
        : base(options)
    {
    }

    public DbSet<Server> Servers { get; set; }
    public DbSet<CafeOrder> CafeOrders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<ItemPrice> ItemPrices { get; set; }
    public DbSet<TimeOfDay> TimeOfDays { get; set; } // ✅ Added for completeness

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ CafeOrder Mapping
        modelBuilder.Entity<CafeOrder>()
            .HasKey(o => o.OrderID);
        modelBuilder.Entity<CafeOrder>()
            .ToTable("CafeOrder")
            .HasOne(o => o.Server)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.ServerID);

        modelBuilder.Entity<CafeOrder>()
            .ToTable("CafeOrder")
            .HasOne(o => o.PaymentType)
            .WithMany()
            .HasForeignKey(o => o.PaymentTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ Server Mapping
        modelBuilder.Entity<Server>()
            .ToTable("Server");

            

        // ✅ OrderItem Mapping
        modelBuilder.Entity<OrderItem>()
            .ToTable("OrderItem")
            .HasKey(oi => oi.OrderItemID);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderID);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.ItemPrice)
            .WithMany() // ItemPrice doesn't have a collection of OrderItems
            .HasForeignKey(oi => oi.ItemPriceID);

        // ✅ ItemPrice Mapping
        modelBuilder.Entity<ItemPrice>()
            .ToTable("ItemPrice");
        modelBuilder.Entity<ItemPrice>()
        .Property(ip => ip.ItemID)
        .IsRequired();


        modelBuilder.Entity<ItemPrice>()
            .HasOne(ip => ip.Item)
            .WithMany(i => i.Prices)
            .HasForeignKey(ip => ip.ItemID); // ✅ Linking ItemPrice to Item

        modelBuilder.Entity<ItemPrice>()
            .HasOne(ip => ip.TimeOfDay)
            .WithMany()  // Assuming TimeOfDay doesn’t have a collection of ItemPrices
            .HasForeignKey(ip => ip.TimeOfDayID);

        // ✅ Item Mapping
        modelBuilder.Entity<Item>()
            .ToTable("Item")
             .HasKey(i => i.ItemID);

        // ✅ TimeOfDay Mapping
        modelBuilder.Entity<TimeOfDay>()
            .ToTable("TimeOfDay");  // Optional if this table exists
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging(); // ✅ Helpful for debugging EF Core issues
    }
}

