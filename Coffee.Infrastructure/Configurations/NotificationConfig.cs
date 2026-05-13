using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFood.Infrastructure.Configurations
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200)
                   .IsUnicode(true);

            builder.Property(x => x.Message)
                   .IsRequired()
                   .IsUnicode(true);

            builder.Property(x => x.Url)
                   .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable("Notifications");
        }
    }
}
