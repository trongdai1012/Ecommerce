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
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CommentFeedback> CommentFeedbacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ConfirmForgot> ConfirmForgots { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DataTest> DataTests { get; set; }
        public DbSet<DataTrain> DataTrains { get; set; }
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

            modelBuilder.Entity<UserConfirm>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity
                .HasOne(x => x.User)
                .WithOne(x => x.UserConfirm)
                .HasForeignKey<UserConfirm>(x => x.UserId);
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

            modelBuilder.Entity<BrandCategory>(entity =>
            {
                entity.HasKey(x => new { x.BrandId, x.CategoryId });
                entity
                .HasOne(x => x.Brand)
                .WithMany(x => x.BrandCategories)
                .HasForeignKey(x => x.BrandId);
                entity
                .HasOne(x => x.Category)
                .WithMany(x => x.BrandCategories)
                .HasForeignKey(x => x.CategoryId);
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
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                    new Brand
                    {
                        Id = 1,
                        Name = "IPhone",
                        Address = "US",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasData(
                    new Category
                    {
                        Id = 1,
                        Name = "Laptop",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1,
                        Status = true
                    });
                entity.HasData(
                    new Category
                    {
                        Id = 2,
                        Name = "Điện thoại",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1,
                        Status = true
                    });
            });

            modelBuilder.Entity<CommentFeedback>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.FeedbackId);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.HandlerId);
                entity.Property(x => x.Title).HasColumnType(TypeOfSql.NVarChar + "(50)");
                entity.Property(x => x.Content).HasColumnType(TypeOfSql.NText);
                entity.Property(x => x.ContentReply).HasColumnType(TypeOfSql.NText);
                entity
                .HasOne(x => x.User)
                .WithMany(x => x.Contacts)
                .HasForeignKey(x => x.CreateBy)
                .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity<DataTest>(entity =>
            {
                entity.HasKey(x => x.Id);
                for (int i = 0; i < 200000; i++)
                {
                    entity.HasData(
                        new DataTest
                        {
                            Id = i + 1,
                            UserId = new Random().Next(1, 100),
                            ProductId = new Random().Next(1, 10000),
                            Rating = (byte)new Random().Next(1, 6)
                        });
                }
            });

            modelBuilder.Entity<DataTrain>(entity =>
            {
                entity.HasKey(x => x.Id);
                for (int i = 0; i < 800000; i++)
                {
                    entity.HasData(
                        new DataTrain
                        {
                            Id = i + 1,
                            UserId = new Random().Next(1, 100),
                            ProductId = new Random().Next(1, 10000),
                            Rating = (byte)new Random().Next(1, 6)
                        });
                }
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(x => x.OrderId);
                entity.Property(x => x.ShipperName).HasColumnType(TypeOfSql.NVarChar + "(50)");
                entity.Property(x => x.ShipperPhone).HasColumnType(TypeOfSql.VarChar + "(20)");
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
                entity.Property(x => x.Rate).HasColumnType(TypeOfSql.TinyInt);
                entity.Property(x => x.Comment).HasColumnType(TypeOfSql.NText);
                entity
                .HasOne(x => x.User)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                entity
                .HasOne(x => x.Product)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Mobile>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.TotalPrice).HasColumnType(TypeOfSql.Decimal).HasDefaultValue(0);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.UpdateAt).HasDefaultValue(DateTime.Now);
                entity.Property(x => x.RecipientAddress).HasColumnType(TypeOfSql.NVarChar + "(100)");
                entity.Property(x => x.RecipientPhone).HasColumnType(TypeOfSql.VarChar + "(20)");
                entity.Property(x => x.RecipientFirstName).HasColumnType(TypeOfSql.NVarChar + "(15)");
                entity.Property(x => x.RecipientLastName).HasColumnType(TypeOfSql.NVarChar + "(30)");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(x => x.Id);
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
                entity.HasIndex(x => new { x.BrandId, x.CategoryId });
                entity.Property(x => x.ProductCode).HasColumnType(TypeOfSql.VarChar + "(20)");
                entity.Property(x => x.Name).HasColumnType(TypeOfSql.NVarChar + "(30)");
                entity.Property(x => x.InitialPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.CurrentPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.PromotionPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.MetaTitle).HasColumnType(TypeOfSql.VarChar + "(20)");
                entity.Property(x => x.Description).HasColumnType(TypeOfSql.NText);
                entity.Property(x => x.Rate).HasColumnType(TypeOfSql.TinyInt);
                entity
                .HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
                entity
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
                for (int i = 1; i <= 100; i++)
                {
                    entity.HasData(
                        new Product
                        {
                            Id = i,
                            ProductCode = "lap-mi-" + i,
                            Name = "Laptop Xiaomi " + i,
                            BrandId = 1,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = new Random().Next(50, 5000) * 10000,
                            CurrentPrice = new Random().Next(50, 5000) * 10000,
                            PromotionPrice = new Random().Next(5, 500) * 10000,
                            DurationWarranty = 12,
                            MetaTitle = "lap-top-xiaomi" + i,
                            Description = "Laptop thuong hieu Xiaomi",
                            Rate = (byte)(new Random().Next(1, 5)),
                            ViewCount = new Random().Next(1, 50000),
                            LikeCount = new Random().Next(1, 10000),
                            TotalSold = new Random().Next(1, 5000),
                            Quantity = new Random().Next(1, 2000),
                            Status = true,
                            CreateAt = DateTime.UtcNow,
                            CreateBy = 1,
                            UpdateAt = DateTime.UtcNow,
                            UpdateBy = 1
                        });
                }
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.ProductId);
                entity.Property(x => x.Url).HasColumnType(TypeOfSql.NVarChar + "(100)");
                entity
                .HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
                for (int i = 1; i <= 100; i++)
                {
                    entity.HasData(
                        new Image
                        {
                            Id = i,
                            ProductId = i,
                            Url = "laptop-mi-01.jpg"
                        });
                }
            });

            modelBuilder.Entity<Laptop>(entity =>
            {
                entity.HasKey(x => x.Id);
                for (int i = 1; i <= 100; i++)
                {
                    entity.HasData(
                        new Laptop
                        {
                            Id = i,
                            ProductId = i,
                            Screen = "15.6 inch, Full HD (1920 x 1080)",
                            CPU = "Intel Core i5 Coffee Lake, 8265U, 1.60 GHz",
                            RAM = "4 GB, DDR4 (On board +1 khe), 2133 MHz",
                            ROM = "HDD: 1 TB SATA3, Intel Optane 16GB",
                            Card = "Card đồ họa tích hợp, Intel® UHD Graphics 620",
                            PortSupport = "2 x USB 3.0, HDMI, LAN (RJ45), USB Type-C",
                            OperatingSystem = "Windows 10 Home SL",
                            Design = "Vỏ nhựa, PIN liền",
                            Size = "Dày 22.45 mm, 2.0 kg",
                            Camera = "8.0mpx",
                            Color = "Đen",
                            Pin = "Pin liền, 3 cell"
                        });
                }
            });

            modelBuilder.Entity<UserConfirm>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId).IsUnique();
                entity.Property(x => x.ConfirmString).HasColumnType(TypeOfSql.VarChar + "(36)");
                entity
                .HasOne(x => x.User)
                .WithOne(x => x.UserConfirm)
                .HasForeignKey<UserConfirm>(x => x.UserId);
            });
        }
    }
}
