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
                for (int i = 0; i < 20000; i++)
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
                for (int i = 0; i < 80000; i++)
                {
                    entity.HasData(
                        new DataTrain
                        {
                            Id = i + 1,
                            UserId = new Random().Next(1, 100),
                            ProductId = new Random().Next(1,101),
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

            modelBuilder.Entity<Mobile>(entity =>
            {
                entity.HasKey(x => x.Id);
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
                entity.Property(x => x.Rate).HasColumnType(TypeOfSql.TinyInt);
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
                        Id = 2,
                        Name = "Macbook Air 13 128GB 2018",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 26990000,
                        CurrentPrice = 26490000,
                        DurationWarranty = 12,
                        Description = "Macbook Air 13 128GB 2018 là sự đột phá về công nghệ và thiết kế. Chiếc MacBook giờ " +
                        "đây còn mỏng nhẹ hơn, cao cấp đáng kinh ngạc với màn hình Retina tràn viền tuyệt đỉnh.",
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
                        Id = 3,
                        Name = "Macbook Air 13 256GB 2018",
                        BrandId = 1,
                        CategoryId = (int)EnumCategory.Laptop,
                        InitialPrice = 33990000,
                        CurrentPrice = 33490000,
                        DurationWarranty = 12,
                        Description = "MacBook Air 13 256GB 2018 đánh dấu sự thay đổi toàn diện của huyền thoại" +
                        " MacBook siêu mỏng nhẹ luôn được rất nhiều người dùng yêu thích từ trước đến nay.",
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
                            Url = "1dot1.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 2,
                            ProductId = 1,
                            Url = "1dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 3,
                            ProductId = 1,
                            Url = "1dot3.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 4,
                           ProductId = 2,
                           Url = "2dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 5,
                            ProductId = 2,
                            Url = "2dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 6,
                            ProductId = 2,
                            Url = "2dot3.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 7,
                           ProductId = 3,
                           Url = "3dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 8,
                            ProductId = 3,
                            Url = "3dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 9,
                            ProductId = 3,
                            Url = "3dot3.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 10,
                            ProductId = 3,
                            Url = "3dot4.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 11,
                           ProductId = 4,
                           Url = "4dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 12,
                            ProductId = 4,
                            Url = "4dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 13,
                            ProductId = 4,
                            Url = "4dot3.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 14,
                           ProductId = 5,
                           Url = "5dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 15,
                            ProductId = 5,
                            Url = "5dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 16,
                            ProductId = 5,
                            Url = "5dot3.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 17,
                           ProductId = 6,
                           Url = "6dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 18,
                            ProductId = 6,
                            Url = "6dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 19,
                            ProductId = 6,
                            Url = "6dot3.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 20,
                           ProductId = 7,
                           Url = "6dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 21,
                            ProductId = 7,
                            Url = "7dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 22,
                            ProductId = 7,
                            Url = "7dot3.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 23,
                            ProductId = 7,
                            Url = "7dot4.jpeg"
                        });
                entity.HasData(
                       new Image
                       {
                           Id = 24,
                           ProductId = 8,
                           Url = "8dot1.jpeg"
                       });
                entity.HasData(
                        new Image
                        {
                            Id = 25,
                            ProductId = 8,
                            Url = "8dot2.jpeg"
                        });
                entity.HasData(
                        new Image
                        {
                            Id = 26,
                            ProductId = 8,
                            Url = "8dot3.jpeg"
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
