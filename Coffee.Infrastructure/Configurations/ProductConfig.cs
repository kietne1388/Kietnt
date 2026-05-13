using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150)
                   .IsUnicode(true);

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(1000)
                   .IsUnicode(true);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            // Quan hệ Category
            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ OrderItem
            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);

            // Quan hệ Comment
            builder.HasMany(x => x.Comments)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);

            // Quan hệ ComboItem
            builder.HasMany(x => x.ComboItems)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);

            builder.ToTable("Products", t => t.HasCheckConstraint("CK_Products_Price", "[Price] > 0"));
        }
    }
}
