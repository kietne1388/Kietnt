using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class ComboConfig : IEntityTypeConfiguration<Combo>
    {
        public void Configure(EntityTypeBuilder<Combo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150)
                   .IsUnicode(true);

            builder.Property(x => x.Description)
                   .HasMaxLength(255)
                   .IsUnicode(true);

            builder.Property(x => x.OriginalPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.ComboPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            builder.HasMany(x => x.ComboItems)
                   .WithOne(x => x.Combo)
                   .HasForeignKey(x => x.ComboId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Combos");
        }
    }
}
