using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MirraApi.Domain.Entities;

namespace MirraApi.Persistance.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.Date)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(p => p.Description)
            .HasMaxLength(500);
        
        builder.HasOne(p => p.Client)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.ClientId)
            .IsRequired();
    }
}