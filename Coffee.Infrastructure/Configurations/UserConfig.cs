using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => x.Username)
                   .IsUnique();

            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(x => x.Email)
                   .IsUnique();

            builder.Property(x => x.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.Role)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("User");

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            // Quan hệ Orders
            builder.HasMany(x => x.Orders)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            // Quan hệ Comments
            builder.HasMany(x => x.Comments)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            // Quan hệ UserVouchers
            builder.HasMany(x => x.UserVouchers)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            builder.ToTable("Users");
        }
    }
}
