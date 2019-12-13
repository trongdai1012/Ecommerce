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
                        Password = Encryptor.Md5Hash("123456"),
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
                        Password = Encryptor.Md5Hash("123456"),
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
                        Password = Encryptor.Md5Hash("123456"),
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
                        Password = Encryptor.Md5Hash("123456"),
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
                for (int i = 0; i < 2000; i++)
                {
                    entity.HasData(
                        new DataTest
                        {
                            Id = i + 1,
                            UserId = new Random().Next(1, 100),
                            ProductId = new Random().Next(1, 101),
                            Rating = (byte)new Random().Next(1, 6)
                        });
                }
            });

            modelBuilder.Entity<DataTrain>(entity =>
            {
                entity.HasKey(x => x.Id);
                for (int i = 0; i < 8000; i++)
                {
                    entity.HasData(
                        new DataTrain
                        {
                            Id = i + 1,
                            UserId = new Random().Next(1, 100),
                            ProductId = new Random().Next(1, 101),
                            Rating = (byte)new Random().Next(1, 6)
                        });
                }
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
                entity.HasData(
                    new Product
                    {
                        Id = 17,
                        Name = "Điện thoại Samsung Galaxy S10+ (512GB)",
                        BrandId = 2,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 23990000,
                        CurrentPrice = 22990000,
                        DurationWarranty = 12,
                        Description = "Samsung Galaxy S10+ 512GB - phiên bản kỷ niệm 10 năm chiếc Galaxy S đầu tiên ra mắt, là một chiếc smartphone hội tủ đủ các yếu tố mà bạn cần ở một chiếc máy cao cấp trong năm 2019",
                        Rate = 5,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
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
                         Id = 18,
                         Name = "Điện thoại Samsung Galaxy Note 10+",
                         BrandId = 2,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 27990000,
                         CurrentPrice = 26900000,
                         DurationWarranty = 12,
                         Description = "Trông ngoại hình khá giống nhau, tuy nhiên Samsung Galaxy Note 10+ sở hữu khá nhiều điểm khác biệt so với Galaxy Note 10 và đây được xem là một trong những chiếc máy đáng mua nhất trong năm 2019, đặc biệt dành cho những người thích một chiếc máy màn hình lớn, camera chất lượng hàng đầus",
                         Rate = 5,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
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
                         Id = 19,
                         Name = "Điện thoại Samsung Galaxy Note 10",
                         BrandId = 2,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 23990000,
                         CurrentPrice = 22990000,
                         DurationWarranty = 12,
                         Description = "Nếu như từ trước tới nay dòng Galaxy Note của Samsung thường ít được các bạn nữ sử dụng bởi kích thước màn hình khá lớn khiến việc cầm nắm trở nên khó khăn thì Samsung Galaxy Note 10 sẽ là chiếc smartphone nhỏ gọn, phù hợp với cả những bạn có bàn tay nhỏ",
                         Rate = 5,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
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
                         Id = 20,
                         Name = "Điện thoại Samsung Galaxy Note 9",
                         BrandId = 2,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 23990000,
                         CurrentPrice = 22990000,
                         DurationWarranty = 12,
                         Description = "Mang lại sự cải tiến đặc biệt trong cây bút S Pen, siêu phẩm Samsung Galaxy Note 9 còn sở hữu dung lượng pin khủng lên tới 4.000 mAh cùng hiệu năng mạnh mẽ vượt bậc, xứng đáng là một trong những chiếc điện thoại cao cấp nhất của Samsung.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
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
                         Id = 21,
                         Name = "Điện thoại Samsung Galaxy A80",
                         BrandId = 2,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 12990000,
                         CurrentPrice = 12490000,
                         DurationWarranty = 12,
                         Description = "Samsung Galaxy A80 là chiếc smartphone mang trong" +
                                        " mình rất nhiều đột phá của Samsung và hứa hẹn sẽ là \"ngọn cờ đầu\"" +
                                        " cho những chiếc smartphone sở hữu một màn hình tràn viền thật sự",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
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
                         Id = 22,
                         Name = "Điện thoại Xiaomi Mi 9 SE",
                         BrandId = 5,
                         CategoryId = (int)EnumCategory.Mobile,
                         InitialPrice = 7990000,
                         CurrentPrice = 7490000,
                         DurationWarranty = 12,
                         Description = "Vẫn như thường lệ thì bên cạnh chiếc flagship Xiaomi Mi 9" +
                                        " cao cấp của mình thì Xiaomi cũng giới thiệu thêm phiên bản rút gọn là chiếc" +
                                        " Xiaomi Mi 9 SE. Máy vẫn sở hữu cho mình nhiều trang bị cao cấp tương tự đàn anh.",
                         Rate = 0,
                         ViewCount = 0,
                         LikeCount = 0,
                         TotalSold = 0,
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
                        Id = 23,
                        Name = "Điện thoại Xiaomi Mi 9T",
                        BrandId = 5,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 8490000,
                        CurrentPrice = 7990000,
                        DurationWarranty = 12,
                        Description = "Xiaomi Mi 9T (tên khác là Redmi K20) là chiếc smartphone" +
                                        " vừa được giới thiệu gây rất nhiều chú ý với người dùng bởi nó hội tụ đủ" +
                                        " 3 yếu tố ngon-bổ-rẻ.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
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
                        Id = 24,
                        Name = "Điện thoại Xiaomi Redmi Note 8 Pro (6GB/128GB)",
                        BrandId = 5,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 6990000,
                        CurrentPrice = 6490000,
                        DurationWarranty = 12,
                        Description = "Là phiên bản nâng cấp của chiếc Xiaomi Redmi Note 7 Pro đã " +
                                        "\"làm mưa làm gió\" trên thị trường trước đó, chiếc Xiaomi Redmi Note 8 Pro" +
                                        " (6GB/128GB) sẽ là sự lựa chọn hàng đầu dành cho người dùng quan tâm nhiều về" +
                                        " hiệu năng và camera của một chiếc máy tầm trung.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
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
                        Id = 25,
                        Name = "Điện thoại OPPO Reno 10x Zoom Edition",
                        BrandId = 12,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 17599000,
                        CurrentPrice = 16990000,
                        DurationWarranty = 12,
                        Description = "Những chiếc flagship trong năm 2019 không chỉ có một " +
                                        "camera chụp ảnh đẹp, chụp xóa phông ảo diệu mà còn phải chụp thật xa và" +
                                        " với chiếc OPPO Reno 10x Zoom Edition chính thức gia nhập thị trường smartphone" +
                                        " có camera siêu zoom.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
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
                        Id = 26,
                        Name = "Điện thoại OPPO Reno2",

                        BrandId = 12,
                        CategoryId = (int)EnumCategory.Mobile,
                        InitialPrice = 15490000,
                        CurrentPrice = 14990000,
                        DurationWarranty = 12,
                        Description = "Sau sự thành công của chiếc OPPO Reno với thiết kế mới lạ, camera chất lượng thì mới đây OPPO tiếp tục giới thiệu phiên bản nâng cấp của chiếc smartphone này là chiếc OPPO Reno2.",
                        Rate = 0,
                        ViewCount = 0,
                        LikeCount = 0,
                        TotalSold = 0,
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
                        Id = 27,
                        Name = "Laptop HP 15 da0359TU N4417/4GB/500GB/Win10",
                        BrandId = 9,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 7490000,
                        CurrentPrice = 7290000,
                        DurationWarranty = 12,
                        Description = "Laptop HP 15 da0359TU N4417 (6KD00PA) là chiếc laptop " +
                                        "văn phòng giá rẻ được trang bị cấu hình vừa đủ sử dụng tính năng cơ bản " +
                                        "và cài sẵn hệ điều hành Windows 10 bản quyền giúp sử dụng ổn định, tiện lợi hơn." +
                                        " Đây sẽ là laptop phù hợp cho học, sinh viên, hay dân văn phòng có điều kiện kinh tế thấp.",
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
                        Id = 28,
                        Name = "Laptop HP 14 ck0068TU i3 7020U/4GB/500GB/Win10",
                        BrandId = 9,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 10590000,
                        CurrentPrice = 10390000,
                        DurationWarranty = 12,
                        Description = "Laptop HP 14 ck0068TU là thuộc dòng laptop văn phòng với" +
                                        " thiết kế nhỏ gọn, dễ dàng di chuyển. Chiếc máy có cấu hình vừa phải đáp ứng" +
                                        " nhu cầu học tập và giải trí của người dùng.",
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
                            Id = 29,
                            Name = "Laptop HP 348 G5 i3 7020U/4GB/256GB/Win10 (7XJ62PA)",
                            BrandId = 9,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 10690000,
                            CurrentPrice = 9990000,
                            DurationWarranty = 12,
                            Description = "Với hiệu năng ổn định khi xử lí tác các tác vụ cơ bản như" +
                                        " lướt web, xem phim, làm việc văn phòng Word, Excel, Powerpoint,... laptop" +
                                        " HP 348 G5 7XJ62PA là lựa chọn phù hợp cho công việc văn phòng, học tập.",
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
                            Id = 30,
                            Name = "Laptop Dell Vostro 3468 i3 7020U/4GB/1TB/Win10",
                            BrandId = 8,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 11690000,
                            CurrentPrice = 11190000,
                            DurationWarranty = 12,
                            Description = "Dell Vostro 3468 i3 7020U là chiếc laptop được trang" +
                                        " bị chip Intel Core i3 cùng hệ điều hành Windows 10 cho hiệu năng ổn định." +
                                        " Máy phù hợp với học sinh, sinh viên, các nhân viên văn phòng với nhu cầu cơ" +
                                        " bản như học tập hay làm việc.",
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
                            Id = 31,
                            Name = "Laptop Asus VivoBook X507MA N4000/4GB/256GB/Win10",
                            BrandId = 6,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 6990000,
                            CurrentPrice = 6490000,
                            DurationWarranty = 12,
                            Description = "Laptop Asus X507MA (BR318T) là chiếc laptop văn phòng" +
                                        " - học tập sở hữu thiết kế mỏng nhẹ, hoạt động nhanh mượt với ổ cứng SSD." +
                                        " Máy tính xách tay này còn được trang bị tính năng bảo mật bằng vân tay," +
                                        " giúp mở máy nhanh chóng và an toàn.",
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
                            Id = 32,
                            Name = "Laptop Asus VivoBook X509U i3 7020U 4GB/1TB/Chuột/Win10",
                            BrandId = 6,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 10990000,
                            CurrentPrice = 10690000,
                            DurationWarranty = 12,
                            Description = "Laptop ASUS VivoBook X509U i3 (EJ063T) là mẫu laptop học tập - văn phòng tầm trung. Nếu bạn đang tìm kiếm một chiếc laptop có cấu hình ổn định và mức giá rẻ thì hãy tham khảo những tính năng của chiếc ASUS VivoBook X590U.",
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
                            Id = 33,
                            Name = "Laptop Asus Gaming TUF FX505D R7 3750H/8GB/512GB/4GB",
                            BrandId = 6,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 23490000,
                            CurrentPrice = 22490000,
                            DurationWarranty = 12,
                            Description = "Cỗ máy chiến game ASUS Gaming TUF FX505D R7 (AL003T) với cấu hình mạnh cùng thiết kế gaming độc đáo, cá tính. Đây là chiếc máy gaming có nhiều cải tiến, phù hợp với các game thủ hiện đại.",
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
                            Id = 34,
                            Name = "Laptop Acer Aspire A514 52 33AB i3 10110U/4GB/256GB/Win10",
                            BrandId = 10,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 12490000,
                            CurrentPrice = 11990000,
                            DurationWarranty = 12,
                            Description = "Laptop Acer Aspire A514 i3 (NX.HMHSV.001) được thiết kế mỏng nhẹ phù hợp với giới trẻ, đặc biệt là các bạn học sinh sinh viên cần di chuyển nhiều. Máy sử dụng con chip Intel thế hệ thứ 10 hiện đại, ổ cứng SSD khởi động cực nhanh, màn hình Full HD góc nhìn siêu rộng đem đến những trải nghiệm tuyệt vời.",
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
                            Id = 35,
                            Name = "Laptop Lenovo ideapad S145 15IWL i5 8265U/8GB/256GB/2GB MX110/Win10",
                            BrandId = 7,
                            CategoryId = (int)EnumCategory.Laptop,
                            InitialPrice = 15290000,
                            CurrentPrice = 14790000,
                            DurationWarranty = 12,
                            Description = "Laptop Lenovo IdeaPad S145 15IWL (81MV00T9VN) vừa được ra mắt đem đến cho giới văn phòng, sinh viên có thêm sự lựa chọn tốt. Đây là chiếc laptop văn phòng có hiệu năng cao cùng các tính năng hiện đại giúp bạn hoàn thành tốt công việc mỗi ngày.",
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 27,
                        ProductId = 27,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 28,
                        ProductId = 28,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 29,
                        ProductId = 29,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 30,
                        ProductId = 30,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 31,
                        ProductId = 31,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 32,
                        ProductId = 32,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 33,
                        ProductId = 33,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 34,
                        ProductId = 34,
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
                entity.HasData(
                    new Laptop
                    {
                        Id = 35,
                        ProductId = 35,
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
                entity.HasData(
                    new Mobile
                    {
                        Id = 17,
                        ProductId = 17,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 18,
                        ProductId = 18,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 19,
                        ProductId = 19,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 20,
                        ProductId = 20,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 21,
                        ProductId = 21,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 22,
                        ProductId = 22,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 23,
                        ProductId = 23,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 24,
                        ProductId = 24,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 25,
                        ProductId = 25,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
                    });
                entity.HasData(
                    new Mobile
                    {
                        Id = 26,
                        ProductId = 26,
                        Screen = "Dynamic AMOLED 6.4inch 2K+ (1440 x 3040 Pixels)",
                        FrontCamera = "Chính 10 MP & Phụ 8 MP",
                        RearCamera = "Chính 12 MP & Phụ 12 MP, 16 MP",
                        OperatingSystem = "Android 9.0 (Pie)",
                        CPU = "Exynos 9820 8 nhân 2 nhân 2.7 GHz, 2 nhân 2.3 GHz & 4 nhân 1.9 GHz",
                        RAM = "8 GB",
                        ROM = "512 GB",
                        SIM = "2 SIM Nano (SIM 2 chung khe thẻ nhớ)",
                        Color = "Grey",
                        Pin = "Pin chuẩn Li-Ion 4100 mAh",
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
                           Url = "1net4.png"
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
                           Url = "2net4.png"
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
                           Url = "3net4.png"
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
                           Url = "4net4.png"
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
                           Url = "5net4.png"
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
                           Url = "6net4.png"
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
                           Url = "7net4.png"
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
                           Url = "8net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 59,
                             ProductId = 17,
                             Url = "9net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 60,
                            ProductId = 17,
                            Url = "9net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 61,
                            ProductId = 17,
                            Url = "9net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 62,
                           ProductId = 17,
                           Url = "9net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 63,
                             ProductId = 18,
                             Url = "10net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 64,
                            ProductId = 18,
                            Url = "10net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 65,
                            ProductId = 18,
                            Url = "10net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 66,
                           ProductId = 18,
                           Url = "10net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 67,
                             ProductId = 19,
                             Url = "11net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 68,
                            ProductId = 19,
                            Url = "11net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 69,
                            ProductId = 19,
                            Url = "11net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 70,
                           ProductId = 19,
                           Url = "11net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 71,
                             ProductId = 20,
                             Url = "12net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 72,
                            ProductId = 20,
                            Url = "12net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 73,
                            ProductId = 20,
                            Url = "12net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 74,
                           ProductId = 20,
                           Url = "12net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 75,
                             ProductId = 21,
                             Url = "13net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 76,
                            ProductId = 21,
                            Url = "13net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 77,
                            ProductId = 21,
                            Url = "13net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 78,
                           ProductId = 21,
                           Url = "13net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 79,
                             ProductId = 22,
                             Url = "14net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 80,
                            ProductId = 22,
                            Url = "14net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 81,
                            ProductId = 22,
                            Url = "14net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 82,
                           ProductId = 22,
                           Url = "14net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 83,
                             ProductId = 23,
                             Url = "15net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 84,
                            ProductId = 23,
                            Url = "15net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 85,
                            ProductId = 23,
                            Url = "15net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 86,
                           ProductId = 23,
                           Url = "15net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 87,
                             ProductId = 24,
                             Url = "16net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 88,
                            ProductId = 24,
                            Url = "16net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 89,
                            ProductId = 24,
                            Url = "16net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 90,
                           ProductId = 24,
                           Url = "16net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 91,
                             ProductId = 25,
                             Url = "17net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 92,
                            ProductId = 25,
                            Url = "17net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 93,
                            ProductId = 25,
                            Url = "17net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 94,
                           ProductId = 25,
                           Url = "17net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 95,
                             ProductId = 26,
                             Url = "18net1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 96,
                            ProductId = 26,
                            Url = "18net2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 97,
                            ProductId = 26,
                            Url = "18net3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 98,
                           ProductId = 26,
                           Url = "18net4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 99,
                             ProductId = 27,
                             Url = "9dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 100,
                            ProductId = 27,
                            Url = "9dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 101,
                            ProductId = 27,
                            Url = "9dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 102,
                           ProductId = 27,
                           Url = "9dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 103,
                             ProductId = 28,
                             Url = "10dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 104,
                            ProductId = 28,
                            Url = "10dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 105,
                            ProductId = 28,
                            Url = "10dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 106,
                           ProductId = 28,
                           Url = "10dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 107,
                             ProductId = 29,
                             Url = "11dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 108,
                            ProductId = 29,
                            Url = "11dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 109,
                            ProductId = 29,
                            Url = "11dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 110,
                           ProductId = 29,
                           Url = "11dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 111,
                             ProductId = 30,
                             Url = "12dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 112,
                            ProductId = 30,
                            Url = "12dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 113,
                            ProductId = 30,
                            Url = "12dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 114,
                           ProductId = 30,
                           Url = "12dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 115,
                             ProductId = 31,
                             Url = "13dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 116,
                            ProductId = 31,
                            Url = "13dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 117,
                            ProductId = 31,
                            Url = "13dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 118,
                           ProductId = 31,
                           Url = "13dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 119,
                             ProductId = 32,
                             Url = "14dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 120,
                            ProductId = 32,
                            Url = "14dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 121,
                            ProductId = 32,
                            Url = "14dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 122,
                           ProductId = 32,
                           Url = "14dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 123,
                             ProductId = 33,
                             Url = "15dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 124,
                            ProductId = 33,
                            Url = "15dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 125,
                            ProductId = 33,
                            Url = "15dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 126,
                           ProductId = 33,
                           Url = "15dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 127,
                             ProductId = 34,
                             Url = "16dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 128,
                            ProductId = 34,
                            Url = "16dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 129,
                            ProductId = 34,
                            Url = "16dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 130,
                           ProductId = 34,
                           Url = "16dot4.png"
                       });
                entity.HasData(
                         new Image
                         {
                             Id = 131,
                             ProductId = 35,
                             Url = "17dot1.png",
                             IsDefault = true
                         });
                entity.HasData(
                        new Image
                        {
                            Id = 132,
                            ProductId = 35,
                            Url = "17dot2.png"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 133,
                            ProductId = 35,
                            Url = "17dot3.png"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 134,
                           ProductId = 35,
                           Url = "17dot4.png"
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
