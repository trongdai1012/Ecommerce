using KLTN.Common;
using Microsoft.EntityFrameworkCore;
using System;

namespace KLTN.DataAccess.Models
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductWareHoure> ProductWareHoures { get; set; }
        public DbSet<RepresentativeOffice> RepresentativeOffices { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Supplier> Suppliers{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasColumnType(TypeOfSql.VarChar).HasMaxLength(30);
                entity.HasIndex(x => x.ParrentCategoryId);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity.Property(x => x.FirstName).HasColumnType(TypeOfSql.NVarChar).HasMaxLength(15);
                entity.Property(x => x.LastName).HasColumnType(TypeOfSql.NVarChar).HasMaxLength(30);
                entity.Property(x => x.Address).HasColumnType(TypeOfSql.NVarChar).HasMaxLength(100);
                entity.Property(x => x.Phone).HasColumnType(TypeOfSql.VarChar).HasMaxLength(20);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.Rank).HasColumnType(TypeOfSql.TinyInt).HasDefaultValue(0);
                entity.Property(x => x.FbAddress).HasColumnType(TypeOfSql.VarChar).HasMaxLength(100);
                entity.Property(x => x.GmailAddress).HasColumnType(TypeOfSql.VarChar).HasMaxLength(100);
                entity.Property(x => x.ZaloAddress).HasColumnType(TypeOfSql.VarChar).HasMaxLength(100);
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                //entity.HasOne(x => x.)
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.CreateBy);
                entity.Property(x => x.TotalPrice).HasColumnType(TypeOfSql.Decimal).HasDefaultValue(0);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.AddressRecipient).HasColumnType(TypeOfSql.NVarChar).HasMaxLength(100);
                entity.Property(x => x.PhoneRecipient).HasColumnType(TypeOfSql.VarChar).HasMaxLength(20);
                entity.Property(x => x.NameRecipient).HasColumnType(TypeOfSql.NVarChar).HasMaxLength(50);
                entity
                   .HasOne(x => x.Delivery)
                   .WithOne(x => x.Order)
                   .HasForeignKey<Delivery>(x=>x.OrderId);
            });
        }
    }
}
