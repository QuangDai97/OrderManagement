using OrderManagement.Models;

namespace OrderManagement.Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasKey(o => o.OrderNo);
        
        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderNo, od.SoRowNo });

        base.OnModelCreating(modelBuilder);
    }
}
