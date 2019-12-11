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

        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandHasCate> BrandHasCates { get; set; }
        public DbSet<CommentFeedback> CommentFeedbacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ConfirmForgot> ConfirmForgots { get; set; }
        public DbSet<DataTest> DataTests { get; set; }
        public DbSet<DataTrain> DataTrains { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Mobile> Mobiles { get; set; }
        public DbSet<News> News { get; set; }
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
                        Email = "manager@gmail.com",
                        Password = "123456",
                        Role = (byte)EnumRole.Manager,
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
                    },
                    new User
                    {
                        Id = 3,
                        Email = "warehouse@gmail.com",
                        Password = "123456",
                        Role = (byte)EnumRole.WareHouseStaff,
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
                    },
                    new User
                    {
                        Id = 4,
                        Email = "shipper@gmail.com",
                        Password = "123456",
                        Role = (byte)EnumRole.Shipper,
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
                        Name = "Apple",
                        Address = "US",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 2,
                        Name = "Samsung",
                        Address = "Korea",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 3,
                        Name = "Sony",
                        Address = "Japan",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 4,
                        Name = "LG",
                        Address = "Korea",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 5,
                        Name = "Xiaomi",
                        Address = "China",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 6,
                        Name = "Asus",
                        Address = "China",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 7,
                        Name = "Lenovo",
                        Address = "China",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 8,
                        Name = "Dell",
                        Address = "China",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 9,
                        Name = "HP",
                        Address = "US",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 10,
                        Name = "Acer",
                        Address = "Taiwan",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 11,
                        Name = "Vivo",
                        Address = "China",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
                entity.HasData(
                    new Brand
                    {
                        Id = 12,
                        Name = "Oppo",
                        Address = "China",
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 2,
                        Status = true
                    });
            });

            modelBuilder.Entity<BrandHasCate>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 1,
                        BrandId = 1,
                        CategoryId = 1
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 2,
                        BrandId = 1,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 3,
                        BrandId = 2,
                        CategoryId = 1
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 4,
                        BrandId = 2,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 5,
                        BrandId = 3,
                        CategoryId = 1
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 6,
                        BrandId = 3,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 7,
                        BrandId = 4,
                        CategoryId = 1
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 8,
                        BrandId = 4,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 9,
                        BrandId = 5,
                        CategoryId = 1
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 10,
                        BrandId = 5,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 11,
                        BrandId = 6,
                        CategoryId = 1
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 12,
                        BrandId = 6,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 13,
                        BrandId = 7,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 14,
                        BrandId = 8,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 15,
                        BrandId = 9,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 16,
                        BrandId = 10,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 17,
                        BrandId = 11,
                        CategoryId = 2
                    });
                entity.HasData(
                    new BrandHasCate
                    {
                        Id = 18,
                        BrandId = 12,
                        CategoryId = 2
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

            modelBuilder.Entity<DataTest>(entity =>
            {
                entity.HasKey(x => x.Id);
                //for (int i = 0; i < 20000; i++)
                //{
                //    entity.HasData(
                //        new DataTest
                //        {
                //            Id = i + 1,
                //            UserId = new Random().Next(1, 100),
                //            ProductId = new Random().Next(1, 101),
                //            Rating = (byte)new Random().Next(1, 6)
                //        });
                //}
            });

            modelBuilder.Entity<DataTrain>(entity =>
            {
                entity.HasKey(x => x.Id);
                //for (int i = 0; i < 80000; i++)
                //{
                //    entity.HasData(
                //        new DataTrain
                //        {
                //            Id = i + 1,
                //            UserId = new Random().Next(1, 100),
                //            ProductId = new Random().Next(1,101),
                //            Rating = (byte)new Random().Next(1, 6)
                //        });
                //}
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity
                .HasOne(x => x.Order)
                .WithOne(x => x.Delivery)
                .HasForeignKey<Delivery>(x => x.OrderId);
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

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Title).HasMaxLength(100);
                entity.Property(x => x.Content).HasColumnType(TypeOfSql.NText);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.TotalPrice).HasColumnType(TypeOfSql.Decimal).HasDefaultValue(0);
                entity.Property(x => x.CreateAt).HasDefaultValue(DateTime.Now);
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
                entity.Property(x => x.Name).HasColumnType(TypeOfSql.NVarChar + "(100)");
                entity.Property(x => x.InitialPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.CurrentPrice).HasColumnType(TypeOfSql.Decimal);
                entity.Property(x => x.Description).HasColumnType(TypeOfSql.NText);
                entity
                .HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Macbook Air 13 128GB 2017",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 22990000,
                        CurrentPrice = 21490000,
                        DurationWarranty = 12,
                        Description = "Macbook Air 13 128 GB MQD32SA/A (2017) với thiết kế không thay đổi," +
                        " vỏ nhôm sang trọng, siêu mỏng và siêu nhẹ, hiệu năng được nâng cấp, thời lượng pin cực lâu," +
                        " phù hợp cho nhu cầu làm việc văn phòng nhẹ nhàng, không cần quá chú trọng vào hiển thị của màn hình.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });
                entity.HasData(
                    new Product
                    {
                        Id = 2,
                        Name = "Macbook Air 13 128GB 2018",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 26990000,
                        CurrentPrice = 26490000,
                        DurationWarranty = 12,
                        Description = "Macbook Air 13 128GB 2018 là sự đột phá về công nghệ và thiết kế. Chiếc MacBook giờ " +
                        "đây còn mỏng nhẹ hơn, cao cấp đáng kinh ngạc với màn hình Retina tràn viền tuyệt đỉnh.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });
                entity.HasData(
                    new Product
                    {
                        Id = 3,
                        Name = "Macbook Air 13 256GB 2018",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 33990000,
                        CurrentPrice = 33490000,
                        DurationWarranty = 12,
                        Description = "MacBook Air 13 256GB 2018 đánh dấu sự thay đổi toàn diện của huyền thoại" +
                        " MacBook siêu mỏng nhẹ luôn được rất nhiều người dùng yêu thích từ trước đến nay.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });
                entity.HasData(
                    new Product
                    {
                        Id = 4,
                        Name = "Macbook Air 13 128GB 2019",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 27990000,
                        CurrentPrice = 27490000,
                        DurationWarranty = 12,
                        Description = "MacBook Air 128GB 2019 có thiết kế tuyệt đẹp, màn hình Retina Truetone" +
                        " đẳng cấp và trải nghiệm bàn phím tuyệt vời nhất từ trước đến nay. Đây sẽ là chiếc laptop" +
                        " nhỏ gọn hoàn hảo nhất dành cho bạn.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });
                entity.HasData(
                    new Product
                    {
                        Id = 5,
                        Name = "Macbook Air 13 256GB 2019",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 34990000,
                        CurrentPrice = 34490000,
                        DurationWarranty = 12,
                        Description = "MacBook Air 256GB 2019 không chỉ là phương tiện làm việc cơ động lý tưởng" +
                        " mà còn là một tuyệt tác về thiết kế, với màn hình Retina tuyệt mỹ cùng kiểu dáng sang trọng," +
                        " mỏng nhẹ đến không ngờ.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });

                entity.HasData(
                    new Product
                    {
                        Id = 6,
                        Name = "Macbook 12 512GB 2017",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 32990000,
                        CurrentPrice = 32490000,
                        DurationWarranty = 12,
                        Description = "MacBook 12 2017 với đường nét thiết kế không có thay đổi so với" +
                        " phiên bản 2016 nhưng Apple đã nâng cấp thêm bộ nhớ và giới thiệu bộ vi xử lý" +
                        " Intel thế hệ thứ 7 (Kaby Lake).",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });

                entity.HasData(
                    new Product
                    {
                        Id = 7,
                        Name = "Macbook Pro 13 inch 256GB 2017",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 38990000,
                        CurrentPrice = 38490000,
                        DurationWarranty = 12,
                        Description = "Thế hệ MacBook Pro 13 inch 2017 ngoài việc cập nhật bộ vi xử lý Intel" +
                        " thế hệ thứ 7 (Kaby Lake) thì còn được nâng cấp gấp đôi dung lượng bộ nhớ. Ngoài ra" +
                        " thiết kế cũng như một số tính năng nổi bật vẫn không có thay đổi so với dòng MacBook 2016.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });
                entity.HasData(
                    new Product
                    {
                        Id = 8,
                        Name = "Macbook Pro 13 Touch Bar i5 1.4GHz/8G/128GB",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 38990000,
                        CurrentPrice = 38490000,
                        DurationWarranty = 12,
                        Description = "MacBook Pro 13 Touch Bar (2019) có thiết kế siêu nhỏ gọn" +
                        " nhưng bên trong lại là cấu hình hết sức mạnh mẽ, màn hình Retina tuyệt mỹ" +
                        " cùng bàn phím cánh bướm thế hệ mới.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });

                entity.HasData(
                    new Product
                    {
                        Id = 9,
                        Name = "Điện thoại Vivo Y19",
                        BrandId = 11,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 6900000,
                        CurrentPrice = 6590000,
                        DurationWarranty = 12,
                        Description = "Vivo Y19 là chiếc smartphone tập trung mạnh vào" +
                        " khả năng chụp ảnh ở camera chính lẫn camera selfie với công nghệ AI," +
                        " hứa hẹn sẽ nhận được sự đón nhận tới từ người dùng là những bạn trẻ năng" +
                        " động và cá tính.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });
                entity.HasData(
                    new Product
                    {
                        Id = 10,
                        Name = "Điện thoại Samsung Galaxy A50s",
                        BrandId = 2,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 7900000,
                        CurrentPrice = 6900000,
                        DurationWarranty = 12,
                        Description = "Nằm trong sứ mệnh nâng cao khả năng cạnh tranh với các smartphone đến từ nhiều nhà sản xuất Trung" +
                        " Quốc, mới đây Samsung tiếp tục giới thiệu phiên bản Samsung Galaxy A50s với nhiều tính năng mà trước đây chỉ xuất " +
                        "hiện trên dòng cao cấp.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });

                entity.HasData(
                    new Product
                    {
                        Id = 11,
                        Name = "Điện thoại iPhone 11 64GB",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 22990000,
                        CurrentPrice = 21990000,
                        DurationWarranty = 12,
                        Description = "Sau bao nhiêu chờ đợi cũng như đồn đoán thì cuối cùng Apple đã" +
                                        " chính thức giới thiệu bộ 3 siêu phẩm iPhone 11 mạnh mẽ nhất của mình vào tháng 9/2019. " +
                                        "Có mức giá rẻ nhất nhưng vẫn được nâng cấp mạnh mẽ như chiếc iPhone Xr năm ngoái, đó chính là" +
                                        " phiên bản iPhone 11 64GB.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
                        TotalRate = 0,
                        TotalMark = 0,
                        Quantity = 100,
                        Status = true,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = 1,
                        UpdateAt = DateTime.UtcNow,
                        UpdateBy = 1
                    });

                entity.HasData(
                     new Product
                     {
                         Id = 12,
                         Name = "Điện thoại Samsung Galaxy A20s 64GB",
                         BrandId = 2,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 5590000,
                         CurrentPrice = 4990000,
                         DurationWarranty = 12,
                         Description = "Samsung Galaxy A20s 64GB là phiên bản cải tiến của chiếc " +
                                        "Samsung Galaxy A20 64GB  đã ra mắt trước đó với những nâng cấp về mặt camera" +
                                        " và kích thước màn hình.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
                         TotalRate = 0,
                         TotalMark = 0,
                         Quantity = 100,
                         Status = true,
                         CreateAt = DateTime.UtcNow,
                         CreateBy = 1,
                         UpdateAt = DateTime.UtcNow,
                         UpdateBy = 1
                     });

                entity.HasData(
                     new Product
                     {
                         Id = 13,
                         Name = "Điện thoại Xiaomi Redmi 8 (4GB/64GB)",
                         BrandId = 5,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 3990000,
                         CurrentPrice = 3590000,
                         DurationWarranty = 12,
                         Description = "Với nhiều ưu điểm vượt trội so với các đối thủ, Xiaomi Redmi" +
                                        " 8 4GB/64GB hứa hẹn là một con bài chiến lược của Xiaomi trong phân khúc smartphone giá rẻ," +
                                        " hiện đang rất sôi động hiện nay.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
                         TotalRate = 0,
                         TotalMark = 0,
                         Quantity = 100,
                         Status = true,
                         CreateAt = DateTime.UtcNow,
                         CreateBy = 1,
                         UpdateAt = DateTime.UtcNow,
                         UpdateBy = 1
                     });

                entity.HasData(
                     new Product
                     {
                         Id = 14,
                         Name = "Điện thoại OPPO A5 (2020) 64GB",
                         BrandId = 12,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 4590000,
                         CurrentPrice = 4290000,
                         DurationWarranty = 12,
                         Description = "OPPO A5 (2020) 64GB là mẫu smartphone tầm trung với giá thành" +
                                        " phải chăng nhưng được trang bị nhiều công nghệ hấp dẫn hứa hẹn sẽ " +
                                        "lấy được lòng các bạn trẻ năng động, thời trang.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
                         TotalRate = 0,
                         TotalMark = 0,
                         Quantity = 100,
                         Status = true,
                         CreateAt = DateTime.UtcNow,
                         CreateBy = 1,
                         UpdateAt = DateTime.UtcNow,
                         UpdateBy = 1
                     });

                entity.HasData(
                     new Product
                     {
                         Id = 15,
                         Name = "Điện thoại iPhone 11 Pro Max 512GB",
                         BrandId = 1,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 45990000,
                         CurrentPrice = 43990000,
                         DurationWarranty = 12,
                         Description = "Để tìm kiếm một chiếc smartphone có hiệu năng mạnh mẽ " +
                                        "và có thể sử dụng mượt mà trong 2-3 năm tới thì không có chiếc máy nào " +
                                        "xứng đang hơn chiếc iPhone 11 Pro Max 512GB mới ra mắt trong năm 2019 của Apple.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
                         TotalRate = 0,
                         TotalMark = 0,
                         Quantity = 100,
                         Status = true,
                         CreateAt = DateTime.UtcNow,
                         CreateBy = 1,
                         UpdateAt = DateTime.UtcNow,
                         UpdateBy = 1
                     });

                entity.HasData(
                     new Product
                     {
                         Id = 16,
                         Name = "Điện thoại iPhone 11 Pro Max 256GB",
                         BrandId = 1,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 38990000,
                         CurrentPrice = 37990000,
                         DurationWarranty = 12,
                         Description = "iPhone 11 Pro Max 256GB là chiếc iPhone cao " +
                                        "cấp nhất trong bộ 3 iPhone Apple giới thiệu trong năm 2019 và quả " +
                                        "thực chiếc smartphone này mang trong mình nhiều trang bị xứng đáng với tên gọi Pro.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
                         TotalRate = 0,
                         TotalMark = 0,
                         Quantity = 100,
                         Status = true,
                         CreateAt = DateTime.UtcNow,
                         CreateBy = 1,
                         UpdateAt = DateTime.UtcNow,
                         UpdateBy = 1
                     });
            });

            modelBuilder.Entity<Laptop>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasData(
                    new Laptop
                    {
                        Id = 1,
                        ProductId = 1,
                        CPU = "Intel Core i5 Dual-Core 1.8 Ghz 3MB(L3 Cache)",
                        RAM = "LPDDR3 8GB 1600 Mhz",
                        ROM = "SSD 128GB",
                        Card = "Intel HD Graphics 6000 1440 x 900 pixels",
                        Screen = "LED-backlit 13.3 inch",
                        PortSupport = "	2xUSB 3.0, 2xThunderbolt 2, 1xSDXC Card, 1xMagSafe 2, 1xHeadphone",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.35 Kg",
                        Size = "",
                        Camera = "8.0 MP",
                        Color = "Xám",
                        Pin = "Lithium-polymer",
                    });

                entity.HasData(
                    new Laptop
                    {
                        Id = 2,
                        ProductId = 2,
                        CPU = "Intel Core i5 Dual-Core 3.6 GHz 4MB L3 Cache",
                        RAM = "LPDDR3 8GB 2133 MHz",
                        ROM = "SSD 128GB",
                        Card = "Intel UHD Graphics 617",
                        Screen = "LED-backlit 13.3 inch Retina(2560 x 1600 Pixels)",
                        PortSupport = "2 cổng USB-C tích hợp Thunderbolt 3 và 1 cổng tai nghe 3.5 mm",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.25 Kg",
                        Size = "Dài 304.1mm - Rộng 212.4mm - Dày 4.1–15.6mm",
                        Camera = "8.0 MP",
                        Color = "Xám",
                        Pin = "Lithium-Polymer, Liền, 50.3‑watt‑hour",
                    });

                entity.HasData(
                    new Laptop
                    {
                        Id = 3,
                        ProductId = 3,
                        CPU = "Intel Core i5 Dual-Core 3.6 GHz 4MB L3 Cache",
                        RAM = "LPDDR3 8GB 2133 MHz",
                        ROM = "SSD 128GB",
                        Card = "Intel UHD Graphics 617",
                        Screen = "LED-backlit 13.3 inch Retina(2560 x 1600 Pixels)",
                        PortSupport = "2 cổng USB-C tích hợp Thunderbolt 3 và 1 cổng tai nghe 3.5 mm",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.25 Kg",
                        Size = "Dài 304.1mm - Rộng 212.4mm - Dày 4.1–15.6mm",
                        Camera = "8.0 MP",
                        Color = "Xám",
                        Pin = "Lithium-Polymer, Liền, 50.3‑watt‑hour",
                    });

                entity.HasData(
                    new Laptop
                    {
                        Id = 4,
                        ProductId = 4,
                        CPU = "Intel Core i5 Dual-Core 3.6 GHz 4MB L3 Cache",
                        RAM = "LPDDR3 8GB 2133 MHz",
                        ROM = "SSD 128GB",
                        Card = "Intel UHD Graphics 617",
                        Screen = "LED-backlit Retina display with IPS and True Tone",
                        PortSupport = "USB type C, 3.5 mm",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.25 Kg",
                        Size = "Cao 0.41–1.56 cm x Rộng 30.41 cm x Sâu 21.24 cm",
                        Camera = "8.0 MP",
                        Color = "Xám",
                        Pin = "49.9 W h Li-Poly",
                    });
                entity.HasData(
                    new Laptop
                    {
                        Id = 5,
                        ProductId = 5,
                        CPU = "Intel Core i5 Dual-Core 3.6 GHz 4MB L3 Cache",
                        RAM = "LPDDR3 8GB 2133 MHz",
                        ROM = "SSD 128GB",
                        Card = "Intel UHD Graphics 617",
                        Screen = "LED-backlit Retina display with IPS and True Tone",
                        PortSupport = "USB type C, 3.5 mm",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.25 Kg",
                        Size = "Cao 0.41–1.56 cm x Rộng 30.41 cm x Sâu 21.24 cm",
                        Camera = "720p FaceTime HD camera",
                        Color = "Xám",
                        Pin = "49.9 W h Li-Poly",
                    });
                entity.HasData(
                    new Laptop
                    {
                        Id = 6,
                        ProductId = 6,
                        CPU = "Intel Core i5 Dual-Core 3.2 GHz 4MB L3 Cache",
                        RAM = "LPDDR3 8GB 1866 MHz",
                        ROM = "SSD 512GB",
                        Card = "Intel HD Graphics 615",
                        Screen = "LED-backlit 2304 x 1440 Pixels",
                        PortSupport = "1xUSB Type-C, 1x 3.5mm headphone jack",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "0.92 kg",
                        Size = "",
                        Camera = "8.0 MP",
                        Color = "Xám",
                        Pin = "Lithium-polymer",
                    });
                entity.HasData(
                    new Laptop
                    {
                        Id = 7,
                        ProductId = 7,
                        CPU = "Intel Core i5 Dual-Core 3.6 GHz 4MB L3 Cache",
                        RAM = "LPDDR3 8GB 2133MHz",
                        ROM = "SSD 256GB",
                        Card = "Intel Iris Plus Graphics 640",
                        Screen = "LED-backlit 2560x1600 pixels",
                        PortSupport = "2xThunderbolt 3(USB-C), 1xHeadphone",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.37 kg",
                        Size = "",
                        Camera = "8.0 MP",
                        Color = "Xám",
                        Pin = "Lithium-polymer",
                    });
                entity.HasData(
                    new Laptop
                    {
                        Id = 8,
                        ProductId = 8,
                        CPU = "Intel Core i5 Quad-Core 3.9GHz 6MB L3 Cache",
                        RAM = "LPDDR3 8GB 2133MHz",
                        ROM = "SSD 128GB",
                        Card = "Intel Iris Plus Graphics 645",
                        Screen = "LED-backlit Retina display with IPS and True Tone",
                        PortSupport = "USB type C, 3.5 mm",
                        OperatingSystem = "MacOS",
                        Design = "Vỏ nhôm nguyên khối",
                        Weight = "1.37 kg",
                        Size = "Cao 1.49 cm x Rộng 30.41 cm x Sâu 21.24 cm",
                        Camera = "720p FaceTime HD camera",
                        Color = "Xám",
                        Pin = "58.2 W h Li-Poly",
                    });
            });

            modelBuilder.Entity<Mobile>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasData(
                    new Mobile
                    {
                        Id = 9,
                        ProductId = 9,
                        CPU = "MediaTek MT6768 8 nhân (Helio P65)",
                        RAM = "6 GB",
                        ROM = "128 GB",
                        SIM = "2 Nano SIM",
                        Screen = "IPS LCD 6.53inch",
                        OperatingSystem = "Android 9.0 (Pie)",
                        RearCamera = "Chính 16 MP & Phụ 8 MP, 2 MP",
                        FrontCamera = "16 MP",
                        Color = "Xanh dương",
                        Pin = "Pin chuẩn Li-Po 5000 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 10,
                        ProductId = 10,
                        Screen = "Super AMOLED 6.4inch Full HD+ (1080 x 2340 Pixels)",
                        FrontCamera = "Chính 48 MP & Phụ 8 MP, 5 MP",
                        RearCamera = "32 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9610 8 nhân",
                        RAM = "4 GB",
                        ROM = "64 GB",
                        SIM = "2 Nano SIM",
                        Color = "Xanh dương",
                        Pin = "Pin chuẩn Li-Po 4000 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 11,
                        ProductId = 11,
                        Screen = "IPS LCD 6.1inch 828 x 1792 Pixels",
                        FrontCamera = "Chính 12 MP & Phụ 12 MP",
                        RearCamera = "12 MP",
                        OperatingSystem = "iOS 13",
                        CPU = "Apple A13 Bionic 6 nhân",
                        RAM = "4 GB",
                        ROM = "64 GB",
                        SIM = "1 eSIM & 1 Nano SIM",
                        Color = "Đỏ",
                        Pin = "Pin chuẩn Li-Ion 3110 mAh",
                    });

                entity.HasData(
                    new Mobile
                    {
                        Id = 12,
                        ProductId = 12,
                        Screen = "IPS LCD 6.5inch HD+ (720 x 1520 Pixels)",
                        FrontCamera = "Chính 13 MP & Phụ 8 MP, 5 MP",
                        RearCamera = "8 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Snapdragon 450 8 nhân",
                        RAM = "4 GB",
                        ROM = "64 GB",
                        SIM = "2 Nano SIM",
                        Color = "Đỏ",
                        Pin = "Pin chuẩn Li-Po 4000 mAh",
                    });

                entity.HasData(
                    new Mobile
                    {
                        Id = 13,
                        ProductId = 13,
                        Screen = "IPS LCD 6.22inch HD+ (720 x 1440 Pixels)",
                        FrontCamera = "Chính 12 MP & Phụ 2 MP",
                        RearCamera = "8 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Snapdragon 439 8 nhân",
                        RAM = "4 GB",
                        ROM = "64 GB",
                        SIM = "2 Nano SIM",
                        Color = "Đỏ",
                        Pin = "Pin chuẩn Li-Po 5000 mAh",
                    });

                entity.HasData(
                    new Mobile
                    {
                        Id = 14,
                        ProductId = 14,
                        Screen = "TFT 6.5inch HD+ (720 x 1600 Pixels)",
                        FrontCamera = "Chính 12 MP & Phụ 8 MP, 2 MP, 2 MP",
                        RearCamera = "8 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Snapdragon 665 8 nhân",
                        RAM = "3 GB",
                        ROM = "64 GB",
                        SIM = "2 Nano SIM",
                        Color = "Xám đen",
                        Pin = "Pin chuẩn Li-Po 5000 mAh",
                    });

                entity.HasData(
                    new Mobile
                    {
                        Id = 15,
                        ProductId = 15,
                        Screen = "OLED 6.5inch 1242 x 2688 Pixels",
                        FrontCamera = "3 camera 12 MP",
                        RearCamera = "12 MP",
                        OperatingSystem = "iOS 13",
                        CPU = "Apple A13 Bionic 6 nhân",
                        RAM = "4 GB",
                        ROM = "512 GB",
                        SIM = "1 eSIM & 1 Nano SIM",
                        Color = "Trắng sữa",
                        Pin = "Pin chuẩn Li-Ion 3969 mAh",
                    });

                entity.HasData(
                    new Mobile
                    {
                        Id = 16,
                        ProductId = 16,
                        Screen = "OLED 6.5inch 1242 x 2688 Pixels",
                        FrontCamera = "3 camera 12 MP",
                        RearCamera = "12 MP",
                        OperatingSystem = "iOS 13",
                        CPU = "Apple A13 Bionic 6 nhân",
                        RAM = "4 GB",
                        ROM = "256 GB",
                        SIM = "1 eSIM & 1 Nano SIM",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 3969 mAh",
                    });


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
                entity.HasData(
                        new Image
                        {
                            Id = 1,
                            ProductId = 1,
                            Url = "1dot1.png",
                            IsDefault = true
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 2,
                            ProductId = 1,
                            Url = "1dot2.jpg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 3,
                            ProductId = 1,
                            Url = "1dot3.jpg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 4,
                           ProductId = 2,
                           Url = "2dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 5,
                            ProductId = 2,
                            Url = "2dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 6,
                            ProductId = 2,
                            Url = "2dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 7,
                           ProductId = 3,
                           Url = "3dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 8,
                            ProductId = 3,
                            Url = "3dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 9,
                            ProductId = 3,
                            Url = "3dot3.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 10,
                            ProductId = 3,
                            Url = "3dot4.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 11,
                           ProductId = 4,
                           Url = "4dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 12,
                            ProductId = 4,
                            Url = "4dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 13,
                            ProductId = 4,
                            Url = "4dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 14,
                           ProductId = 5,
                           Url = "5dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 15,
                            ProductId = 5,
                            Url = "5dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 16,
                            ProductId = 5,
                            Url = "5dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 17,
                           ProductId = 6,
                           Url = "6dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 18,
                            ProductId = 6,
                            Url = "6dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 19,
                            ProductId = 6,
                            Url = "6dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 20,
                           ProductId = 7,
                           Url = "7dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 21,
                            ProductId = 7,
                            Url = "7dot2.jpg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 22,
                            ProductId = 7,
                            Url = "7dot3.jpg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 23,
                            ProductId = 7,
                            Url = "7dot4.jpg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 24,
                           ProductId = 8,
                           Url = "8dot1.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 25,
                            ProductId = 8,
                            Url = "8dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 26,
                            ProductId = 8,
                            Url = "8dot3.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 27,
                            ProductId = 9,
                            Url = "1net1.png",
                            IsDefault = true
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 28,
                            ProductId = 9,
                            Url = "1net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 29,
                            ProductId = 9,
                            Url = "1net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 30,
                           ProductId = 9,
                           Url = "1net4.png",
                           IsDefault = true
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 31,
                            ProductId = 10,
                            Url = "2net1.png",
                            IsDefault = true
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 32,
                            ProductId = 10,
                            Url = "2net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 33,
                            ProductId = 10,
                            Url = "2net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 34,
                           ProductId = 10,
                           Url = "2net4.png",
                           IsDefault = true
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 35,
                             ProductId = 11,
                             Url = "3net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 36,
                            ProductId = 11,
                            Url = "3net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 37,
                            ProductId = 11,
                            Url = "3net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 38,
                           ProductId = 11,
                           Url = "3net4.png",
                           IsDefault = true
                       }); entity.HasData(
                         new Image
                         {
                             Id = 39,
                             ProductId = 12,
                             Url = "4net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 40,
                            ProductId = 12,
                            Url = "4net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 41,
                            ProductId = 12,
                            Url = "4net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 42,
                           ProductId = 12,
                           Url = "4net4.png",
                           IsDefault = true
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 43,
                             ProductId = 13,
                             Url = "5net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 44,
                            ProductId = 13,
                            Url = "5net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 45,
                            ProductId = 13,
                            Url = "5net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 46,
                           ProductId = 13,
                           Url = "5net4.png",
                           IsDefault = true
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 47,
                             ProductId = 14,
                             Url = "6net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 48,
                            ProductId = 14,
                            Url = "6net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 49,
                            ProductId = 14,
                            Url = "6net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 50,
                           ProductId = 14,
                           Url = "6net4.png",
                           IsDefault = true
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 51,
                             ProductId = 15,
                             Url = "7net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 52,
                            ProductId = 15,
                            Url = "7net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 53,
                            ProductId = 15,
                            Url = "7net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 54,
                           ProductId = 15,
                           Url = "7net4.png",
                           IsDefault = true
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 55,
                             ProductId = 16,
                             Url = "8net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 56,
                            ProductId = 16,
                            Url = "8net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 57,
                            ProductId = 16,
                            Url = "8net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 58,
                           ProductId = 16,
                           Url = "8net4.png",
                           IsDefault = true
                       });
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
