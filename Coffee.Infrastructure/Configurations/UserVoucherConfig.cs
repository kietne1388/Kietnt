using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class UserVoucherConfig : IEntityTypeConfiguration<UserVoucher>
    {
        public void Configure(EntityTypeBuilder<UserVoucher> builder)
        {
            // 🔑 Khóa chính ghép
            builder.HasKey(x => new { x.UserId, x.VoucherId });

            builder.HasOne(x => x.User)
                   .WithMany(x => x.UserVouchers)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Voucher)
                   .WithMany(x => x.UserVouchers)
                   .HasForeignKey(x => x.VoucherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.IsUsed)
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(x => x.AssignedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UsedAt)
                   .IsRequired(false);

            builder.ToTable("UserVouchers");
        }
    }
}
