using Microsoft.EntityFrameworkCore;
using MirraApi.Domain.Entities;
using MirraApi.Persistance.Configurations;

namespace MirraApi.Persistance;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<ExchangeRate> ExchangeRates { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ExchangeRateConfiguration());
    }
}