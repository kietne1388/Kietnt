using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.HasIndex(x => x.Name)
                   .IsUnique();

            builder.Property(x => x.Description)
                   .HasMaxLength(255)
                   .IsUnicode(true);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            builder.HasMany(x => x.Products)
                   .WithOne(x => x.Category)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Categories");
        }
    }
}
