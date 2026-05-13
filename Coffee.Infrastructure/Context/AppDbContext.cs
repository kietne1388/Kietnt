using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using FastFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Combo> Combos => Set<Combo>();
        public DbSet<ComboItem> ComboItems => Set<ComboItem>();
        public DbSet<Voucher> Vouchers => Set<Voucher>();
        public DbSet<UserVoucher> UserVouchers => Set<UserVoucher>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Attendance> Attendances => Set<Attendance>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );
        }
    }

}
