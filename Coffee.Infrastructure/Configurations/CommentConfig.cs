using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                   .IsRequired()
                   .HasMaxLength(1000)
                   .IsUnicode(true);

            builder.Property(x => x.Rating)
                   .IsRequired();

            builder.Property(x => x.IsHidden)
                   .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            // Quan hệ User
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Product
            builder.HasOne(x => x.Product)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.ProductId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ Combo
            builder.HasOne(x => x.Combo)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.ComboId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Comments");
        }
    }
}
