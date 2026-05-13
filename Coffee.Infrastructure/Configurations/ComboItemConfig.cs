using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class ComboItemConfig : IEntityTypeConfiguration<ComboItem>
    {
        public void Configure(EntityTypeBuilder<ComboItem> builder)
        {
            // Composite Key
            builder.HasKey(x => new { x.ComboId, x.ProductId });

            builder.Property(x => x.Quantity)
                   .IsRequired();

            builder.HasOne(x => x.Combo)
                   .WithMany(x => x.ComboItems)
                   .HasForeignKey(x => x.ComboId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                   .WithMany(x => x.ComboItems)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("ComboItems");
        }
    }
}
