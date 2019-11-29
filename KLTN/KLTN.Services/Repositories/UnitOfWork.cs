using KLTN.DataAccess.Models;
using System.Threading.Tasks;

namespace KLTN.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<User> UserRepository { get; set; }

        public IGenericRepository<Brand> BrandRepository { get; set; }

        public IGenericRepository<BrandHasCate> BrandHasCateRepository { get; set; }

        public IGenericRepository<Contact> ContactRepository { get; set; }

        public IGenericRepository<ConfirmForgot> ForgotRepository { get; set; }

        public IGenericRepository<DataTest> DataTestRepository { get; set; }

        public IGenericRepository<DataTrain> DataTrainRepository { get; set; }

        public IGenericRepository<Delivery> DeliveryRepository { get; set; }

        public IGenericRepository<Feedback> FeedbackRepository { get; set; }

        public IGenericRepository<Image> ImageRepository { get; set; }

        public IGenericRepository<Laptop> LaptopRepository { get; set; }

        public IGenericRepository<Mobile> MobileRepository { get; set; }

        public IGenericRepository<News> NewsRepository { get; set; }

        public IGenericRepository<Order> OrderRepository { get; set; }

        public IGenericRepository<OrderDetail> OrderDetailRepository { get; set; }

        public IGenericRepository<Product> ProductRepository { get; set; }

        public IGenericRepository<UserConfirm> UserConfirmRepository { get; set; }

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
            BrandRepository = new GenericRepository<Brand>(_ecommerceDbContext);
            BrandHasCateRepository = new GenericRepository<BrandHasCate>(_ecommerceDbContext);
            ContactRepository = new GenericRepository<Contact>(_ecommerceDbContext);
            ForgotRepository = new GenericRepository<ConfirmForgot>(_ecommerceDbContext);
            DataTestRepository = new GenericRepository<DataTest>(_ecommerceDbContext);
            DataTrainRepository = new GenericRepository<DataTrain>(_ecommerceDbContext);
            DeliveryRepository = new GenericRepository<Delivery>(_ecommerceDbContext);
            FeedbackRepository = new GenericRepository<Feedback>(_ecommerceDbContext);
            ImageRepository = new GenericRepository<Image>(_ecommerceDbContext);
            LaptopRepository = new GenericRepository<Laptop>(_ecommerceDbContext);
            MobileRepository = new GenericRepository<Mobile>(_ecommerceDbContext);
            NewsRepository = new GenericRepository<News>(_ecommerceDbContext);
            OrderRepository = new GenericRepository<Order>(_ecommerceDbContext);
            OrderDetailRepository = new GenericRepository<OrderDetail>(_ecommerceDbContext);
            ProductRepository = new GenericRepository<Product>(_ecommerceDbContext);
            UserConfirmRepository = new GenericRepository<UserConfirm>(_ecommerceDbContext);
        }

        /// <summary>
        /// This method save db
        /// </summary>
        public void Save()
        {
            _ecommerceDbContext.SaveChanges();
        }

        /// <summary>
        /// This method save db
        /// </summary>
        public async Task SaveAsync()
        {
            await _ecommerceDbContext.SaveChangesAsync();
        }
    }
}
