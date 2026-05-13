using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderCode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.Status)
                   .IsRequired()
                   .HasMaxLength(30)
                   .HasDefaultValue("Pending");

            builder.Property(x => x.Address)
                   .IsRequired()
                   .HasMaxLength(255)
                   .IsUnicode(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            // Quan hệ User
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ OrderItems
            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId);

            builder.ToTable("Orders");
        }
    }
}
