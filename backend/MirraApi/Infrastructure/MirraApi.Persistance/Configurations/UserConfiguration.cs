using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MirraApi.Domain.Entities;

namespace MirraApi.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
            
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(256);
            
        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(256);
            
        builder.Property(e => e.RefreshToken)
            .HasMaxLength(512);
        
        builder.HasIndex(e => e.Email)
            .IsUnique();
    }
}