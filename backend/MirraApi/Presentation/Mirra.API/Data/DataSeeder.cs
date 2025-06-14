using Microsoft.EntityFrameworkCore;
using MirraApi.Domain.Entities;
using MirraApi.Persistance;

namespace Mirra.API.Data;

public static class DataSeeder
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, isProd);
    }

    private static void SeedData(AppDbContext context, bool isProd)
    {
        if (isProd)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not run migrations: {e.Message}");
            }
        }

        if (!context.Users.Any())
        {
            Console.WriteLine("--> Seeding User Data...");
            var adminUser = new User
            {
                Email = "admin@mirra.dev",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                RefreshToken = null,
                RefreshTokenExpiryTime = null
            };
            context.Users.Add(adminUser);
            context.SaveChanges();
        }
        
        if (context.Clients.Any())
        {
            Console.WriteLine("--> We already have data");
            return;
        }

        Console.WriteLine("--> Seeding Data...");
        
        var clients = new[]
        {
            new Client { Name = "Иван", Email = "ivan@gmail.com", BalanceT = 130.50m },
            new Client { Name = "Мария", Email = "maria@gmail.com", BalanceT = 280.00m },
            new Client { Name = "Алексей", Email = "alexey@gmail.com", BalanceT = 75.25m }
        };
        
        context.Clients.AddRange(clients);
        context.SaveChanges();
        
        var payments = new[]
        {
            new Payment { ClientId = 3, Amount = 50.00m, Date = DateTime.UtcNow.AddDays(-5), Description = "Оплата жилья" },
            new Payment { ClientId = 2, Amount = 56.13m, Date = DateTime.UtcNow.AddDays(-20), Description = "Оплата корзины интернет магазина" },
            new Payment { ClientId = 1, Amount = 25.50m, Date = DateTime.UtcNow.AddDays(-300), Description = "Покупка телефено" },
            new Payment { ClientId = 3, Amount = 75.25m, Date = DateTime.UtcNow.AddDays(-21), Description = "Первоначальный взнос" },
            new Payment { ClientId = 2, Amount = 2220.01m, Date = DateTime.UtcNow.AddDays(-3), Description = "Покупка квартиры" }
        };
        
        context.Payments.AddRange(payments);
        
        var initialRate = new ExchangeRate
        {
            Rate = 10.0,
            UpdatedAt = DateTime.UtcNow
        };
        
        context.ExchangeRates.Add(initialRate);
        context.SaveChanges();
    }
}