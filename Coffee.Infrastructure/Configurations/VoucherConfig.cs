using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class VoucherConfig : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => x.Code)
                   .IsUnique();

            builder.Property(x => x.DiscountAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Description)
                   .HasMaxLength(500)
                   .IsUnicode(true);

            builder.Property(x => x.DiscountPercent);

            builder.Property(x => x.Quantity)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

            builder.Property(x => x.ExpiredAt)
                   .IsRequired();

            // Quan hệ UserVoucher
            builder.HasMany(x => x.UserVouchers)
                   .WithOne(x => x.Voucher)
                   .HasForeignKey(x => x.VoucherId);

            builder.ToTable("Vouchers");
        }
    }
}
