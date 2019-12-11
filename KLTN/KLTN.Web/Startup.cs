using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using KLTN.Common;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels.AutoMapper;
using KLTN.DataModels.Models.Brands;
using KLTN.DataModels.Models.Contact;
using KLTN.DataModels.Models.News;
using KLTN.DataModels.Models.Orders;
using KLTN.DataModels.Models.Products;
using KLTN.DataModels.Models.Users;
using KLTN.DataModels.Validations.Brands;
using KLTN.DataModels.Validations.Contact;
using KLTN.DataModels.Validations.News;
using KLTN.DataModels.Validations.Orders;
using KLTN.DataModels.Validations.Product;
using KLTN.DataModels.Validations.Users;
using KLTN.Services;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace KLTN.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
                mc.AddProfile(new BrandProfiles());
                mc.AddProfile(new ProductProfiles());
                mc.AddProfile(new OrderProfiles());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IRecommenderService, RecommenderService>();

            services.AddMvc(setup => { }).AddFluentValidation();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<EcommerceDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(Constants.DefaultConnection),
                    assembly => assembly.MigrationsAssembly(Settings.NameSpaceWeb)));

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = new PathString("/Notification/NotAuthorized");
                    options.LoginPath = new PathString(RedirectConfig.AccountAuthentication);
                    options.ReturnUrlParameter = Constants.RequestPath;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });

            //Add authorization with Policy
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(nameof(EnumRole.Admin), policy => policy.RequireClaim(ClaimTypes.Role, nameof(EnumRole.Admin)));
                }
            );

            //Add Transient
            services.AddTransient<IValidator<RegisterUserViewModel>, RegisterValidator>();
            services.AddTransient<IValidator<CreateEmployeeViewModel>, CreateEmployeeValidator>();
            services.AddTransient<IValidator<CreateAdminViewModel>, CreateAdminValidator>();
            services.AddTransient<IValidator<CreateLaptopViewModel>, CreateLaptopValidator>();
            services.AddTransient<IValidator<OrderViewModel>, PaymentValidator>();
            services.AddTransient<IValidator<CreateBrandModel>, CreateBrandValidator>();
            services.AddTransient<IValidator<BrandViewModel>, UpdateBrandValidator>();
            services.AddTransient<IValidator<ContactViewModel>, SendContactValidator>();
            services.AddTransient<IValidator<NewsViewModel>, CreateNewsValidator>();
            services.AddTransient<IValidator<CreateMoblieViewModel>, CreateMobileValidator>();
            services.AddTransient<IValidator<UpdateLaptopViewModel>, UpdateLaptopValidator>();
            services.AddTransient<IValidator<UpdateMoblieViewModel>, UpdateMobileValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: MapRouteNames.AreaRoute,
                    template: MapRoutesConfig.AreasExistsHomeIndex
                );

                routes.MapRoute(
                    name: MapRouteNames.Default,
                    template: MapRoutesConfig.HomeIndexId);
            });
        }
    }
}
