using KLTN.Common;
using KLTN.Common.Infrastructure;
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
        public DbSet<Brand> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Mobile> Mobiles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserConfirm> UserConfirms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => new { x.Role, x.FirstName });
                entity.HasIndex(x => x.Email).IsUnique();
                entity.Property(x => x.Email).HasColumnType(TypeOfSql.VarChar + "(50)");
                entity.Property(x => x.Password).HasColumnType(TypeOfSql.NVarChar + "(32)");
                entity.Property(x => x.Role).HasColumnType(TypeOfSql.TinyInt);
                entity.Property(x => x.FirstName).HasColumnType(TypeOfSql.NVarChar + "(15)");
                entity.Property(x => x.LastName).HasColumnType(TypeOfSql.NVarChar + "(30)");
                entity.Property(x => x.Phone).HasColumnType(TypeOfSql.VarChar + "(20)");
                entity.Property(x => x.Address).HasColumnType(TypeOfSql.NVarChar + "(100)");
                entity.HasData(
                    new User
                    {
                        Id = 1,
                        Email = "admin@gmail.com",
                        Password = "123456",
                        Role = (byte)EnumRole.Admin,
                        FirstName = "Dai",
                        LastName = "Than Trong",
                        Gender = true,
                        BirthDay = Convert.ToDateTime("12-10-1994"),
                        Phone = "+84981965080",
                        Address = "Bac Giang",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1,
                        Status = true,
                        IsConfirm = true
                    },
                    new User
                    {
                        Id = 2,
                        Email = "admin1@gmail.com",
                        Password = "123456",
                        Role = (byte)EnumRole.Admin,
                        FirstName = "Admin",
                        LastName = "Manager",
                        Gender = true,
                        BirthDay = Convert.ToDateTime("10-12-1994"),
                        Phone = "+84981965080",
                        Address = "Bac Giang",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1,
                        Status = true,
                        IsConfirm = true
                    }
                );
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity.Property(x => x.PassEmail).HasColumnType(TypeOfSql.NVarChar + "(200)");
                entity
                .HasOne(x => x.User)
                .WithOne(x => x.Admin)
                .HasForeignKey<Admin>(x => x.UserId);
                entity.HasData
                (
                    new Admin
                    {
                        Id = 1,
                        PassEmail = "123456",
                        UserId = 1
                    }
                );
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasColumnType(TypeOfSql.NVarChar + "(30)");
                entity.Property(x => x.Address).HasColumnType(TypeOfSql.NVarChar + "(100)");
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
                entity
                .HasOne(x => x.User)
                .WithMany(x => x.Brands)
                .HasForeignKey(x => x.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);
                entity.HasData(
                    new Brand
                    {
                        Id = 1,
                        Name = "IPorn",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity.Property(x => x.Rank).HasColumnType(TypeOfSql.TinyInt).HasDefaultValue(0);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
                entity
                .HasOne(x => x.User)
                .WithOne(x => x.Customer)
                .HasForeignKey<Customer>(x => x.UserId);
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(x => x.OrderId);
                entity.Property(x => x.ShipperName).HasColumnType(TypeOfSql.NVarChar + "(50)");
                entity.Property(x => x.ShipperPhone).HasColumnType(TypeOfSql.VarChar + "(20)");
                entity.Property(x => x.Status).HasColumnType(TypeOfSql.TinyInt);
                entity
                .HasOne(x => x.Order)
                .WithOne(x => x.Delivery)
                .HasForeignKey<Delivery>(x => x.OrderId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity.Property(x => x.PassEmail).HasColumnType(TypeOfSql.NVarChar + "(200)");
                entity
                .HasOne(x => x.User)
                .WithOne(x => x.Employee)
                .HasForeignKey<Employee>(x => x.UserId);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.HandlerId);
                entity.Property(x => x.Title).HasColumnType(TypeOfSql.NVarChar + "(50)");
                entity.Property(x => x.Content).HasColumnType(TypeOfSql.NText);
                entity.Property(x => x.ContentReply).HasColumnType(TypeOfSql.NText);
                entity
                .HasOne(x => x.Customer)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(entity=>
            {
                entity.HasKey(x=>x.Id);
                entity.HasIndex(x => x.ProductId);
                entity.Property(x => x.Url).HasColumnType(TypeOfSql.NVarChar+"(100)");
            });

            modelBuilder.Entity<Laptop>(entity=>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.BrandId);
                entity
                .HasOne(x => x.Brand)
                .WithMany(x => x.Laptops)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Mobile>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.BrandId);
                entity
                .HasOne(x => x.Brand)
                .WithMany(x => x.Mobiles)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.TotalPrice).HasColumnType(TypeOfSql.Decimal).HasDefaultValue(0);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.RecipientAddress).HasColumnType(TypeOfSql.NVarChar + "(100)");
                entity.Property(x => x.RecipientPhone).HasColumnType(TypeOfSql.VarChar + "(20)");
                entity.Property(x => x.RecipientName).HasColumnType(TypeOfSql.NVarChar + "(50)");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(x => x.OrderId);
                entity.Property(x => x.Price).HasColumnType(TypeOfSql.Decimal).HasDefaultValue(0);
                entity
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
                entity
                .HasOne(x => x.Product)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.ProductCode).HasColumnType(TypeOfSql.VarChar + "(10)");
                entity.Property(x => x.Name).HasColumnType(TypeOfSql.NVarChar + "(30)");
                entity.Property(x => x.InitialPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.CurrentPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.PromotionPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.MetaTitle).HasColumnType(TypeOfSql.VarChar + "(10)");
                entity.Property(x => x.Description).HasColumnType(TypeOfSql.NText);
                entity.Property(x => x.Rate).HasColumnType(TypeOfSql.TinyInt);
            });

            modelBuilder.Entity<UserConfirm>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity.Property(x => x.ConfirmString).HasColumnType(TypeOfSql.VarChar + "(36)");
                entity
                .HasOne(x => x.User)
                .WithOne(x => x.UserConfirm)
                .HasForeignKey<UserConfirm>(x=>x.UserId);
            });

            modelBuilder.Entity<Warranty>(entity=>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.ProducId);
            });
        }
    }
}
