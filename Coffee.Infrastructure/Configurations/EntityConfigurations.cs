using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class ComboItemConfiguration : IEntityTypeConfiguration<ComboItem>
    {
        public void Configure(EntityTypeBuilder<ComboItem> builder)
        {
            builder.HasKey(ci => new { ci.ComboId, ci.ProductId });

            builder.HasOne(ci => ci.Combo)
                .WithMany(c => c.ComboItems)
                .HasForeignKey(ci => ci.ComboId);

            builder.HasOne(ci => ci.Product)
                .WithMany(p => p.ComboItems)
                .HasForeignKey(ci => ci.ProductId);
        }
    }

    public class UserVoucherConfiguration : IEntityTypeConfiguration<UserVoucher>
    {
        public void Configure(EntityTypeBuilder<UserVoucher> builder)
        {
            builder.HasKey(uv => new { uv.UserId, uv.VoucherId });

            builder.HasOne(uv => uv.User)
                .WithMany(u => u.UserVouchers)
                .HasForeignKey(uv => uv.UserId);

            builder.HasOne(uv => uv.Voucher)
                .WithMany(v => v.UserVouchers)
                .HasForeignKey(uv => uv.VoucherId);
        }
    }
}
