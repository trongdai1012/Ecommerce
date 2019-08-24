using System;
using System.Collections.Generic;
using System.Text;
using KLTN.DataAccess.Models;

namespace KLTN.Services.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<User> UserRepository { get; set; }

        public IGenericRepository<Admin> AdminRepository { get; set; }

        public IGenericRepository<Category> CategoryRepository { get; set; }

        public IGenericRepository<Customer> CustomerRepository { get; set; }

        public IGenericRepository<Delivery> DeliveryRepository { get; set; }

        public IGenericRepository<Employee> EmployeeRepository { get; set; }

        public IGenericRepository<Feedback> FeedbackRepository { get; set; }

        public IGenericRepository<Order> OrderRepository { get; set; }

        public IGenericRepository<OrderDetail> OrderDetailRepository { get; set; }

        public IGenericRepository<Product> ProductRepository { get; set; }

        public IGenericRepository<ProductWareHoure> ProductWareHoureRepository { get; set; }

        public IGenericRepository<Store> StoreRepository { get; set; }

        public IGenericRepository<Supplier> SupplierRepository { get; set; }

        public IGenericRepository<WareHouse> WareHouseRepository { get; set; }

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
            AdminRepository = new GenericRepository<Admin>(_ecommerceDbContext);
            CategoryRepository = new GenericRepository<Category>(_ecommerceDbContext);
            CustomerRepository = new GenericRepository<Customer>(_ecommerceDbContext);
            DeliveryRepository = new GenericRepository<Delivery>(_ecommerceDbContext);
            EmployeeRepository = new GenericRepository<Employee>(_ecommerceDbContext);
            FeedbackRepository = new GenericRepository<Feedback>(_ecommerceDbContext);
            OrderRepository = new GenericRepository<Order>(_ecommerceDbContext);
            OrderDetailRepository = new GenericRepository<OrderDetail>(_ecommerceDbContext);
            ProductRepository = new GenericRepository<Product>(_ecommerceDbContext);
            ProductWareHoureRepository = new GenericRepository<ProductWareHoure>(_ecommerceDbContext);
            StoreRepository = new GenericRepository<Store>(_ecommerceDbContext);
            SupplierRepository = new GenericRepository<Supplier>(_ecommerceDbContext);
            WareHouseRepository = new GenericRepository<WareHouse>(_ecommerceDbContext);
            UserConfirmRepository = new GenericRepository<UserConfirm>(_ecommerceDbContext);
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
