using Microsoft.EntityFrameworkCore;

namespace SandBox.EFCoe;

public class AppDbContext : DbContext
{
    public DbSet<Fruit> Fruits { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fruit>().Property<int>("Id");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Database");
    }
}
public class Fruit
{
    public string Name { get; set; }
    public int Weight { get; set; }
    public Address Address { get; set; }
}
public class Address
{
    public int Id { get; set; }
    public string PostCode { get; set; }
}
public class FruitVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Weight { get; set; }
    public string PostCode { get; set; }
}