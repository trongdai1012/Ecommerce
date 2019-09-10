using KLTN.DataAccess.Models;

namespace KLTN.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<User> UserRepository { get; set; }

        public IGenericRepository<Admin> AdminRepository { get; set; }

        public IGenericRepository<Brand> BrandRepository { get; set; }

        public IGenericRepository<Category> CategoryRepository { get; set; }

        public IGenericRepository<Customer> CustomerRepository { get; set; }

        public IGenericRepository<Delivery> DeliveryRepository { get; set; }

        public IGenericRepository<Employee> EmployeeRepository { get; set; }

        public IGenericRepository<Feedback> FeedbackRepository { get; set; }

        public IGenericRepository<Image> ImageRepository { get; set; }

        public IGenericRepository<Laptop> LaptopRepository { get; set; }

        public IGenericRepository<Mobile> MobileRepository { get; set; }

        public IGenericRepository<Order> OrderRepository { get; set; }

        public IGenericRepository<OrderDetail> OrderDetailRepository { get; set; }

        public IGenericRepository<Product> ProductRepository { get; set; }

        public IGenericRepository<UserConfirm> UserConfirmRepository { get; set; }

        public IGenericRepository<Warranty> WarrantyRepository { get; set; }

        private EcommerceDbContext _ecommerceDbContext;

        /// <summary>
        /// Constructor initial context and repository entities
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(EcommerceDbContext ecommerceDbContext)
        {
            _ecommerceDbContext = ecommerceDbContext;
            InitRepositories();
        }

        /// <summary>
        /// This method Initial Repository entities
        /// </summary>
        private void InitRepositories()
        {
            UserRepository = new GenericRepository<User>(_ecommerceDbContext);
            AdminRepository = new GenericRepository<Admin>(_ecommerceDbContext);
            BrandRepository = new GenericRepository<Brand>(_ecommerceDbContext);
            CategoryRepository = new GenericRepository<Category>(_ecommerceDbContext);
            CustomerRepository = new GenericRepository<Customer>(_ecommerceDbContext);
            DeliveryRepository = new GenericRepository<Delivery>(_ecommerceDbContext);
            EmployeeRepository = new GenericRepository<Employee>(_ecommerceDbContext);
            FeedbackRepository = new GenericRepository<Feedback>(_ecommerceDbContext);
            ImageRepository = new GenericRepository<Image>(_ecommerceDbContext);
            LaptopRepository = new GenericRepository<Laptop>(_ecommerceDbContext);
            MobileRepository = new GenericRepository<Mobile>(_ecommerceDbContext);
            OrderRepository = new GenericRepository<Order>(_ecommerceDbContext);
            OrderDetailRepository = new GenericRepository<OrderDetail>(_ecommerceDbContext);
            ProductRepository = new GenericRepository<Product>(_ecommerceDbContext);
            UserConfirmRepository = new GenericRepository<UserConfirm>(_ecommerceDbContext);
            WarrantyRepository = new GenericRepository<Warranty>(_ecommerceDbContext);
        }

        /// <summary>
        /// This method save db
        /// </summary>
        public void Save()
        {
            _ecommerceDbContext.SaveChanges();
        }
    }
}
