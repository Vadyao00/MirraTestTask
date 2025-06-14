using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MirraApi.Domain.Entities;

namespace MirraApi.Persistance.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.BalanceT)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.HasMany(c => c.Payments)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId);
    }
}