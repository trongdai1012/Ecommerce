using KLTN.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services.Repositories
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork
    {
        //declare entities Repository
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Admin> AdminRepository { get; }
        IGenericRepository<Brand> BrandRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Contact> ContactRepository { get; }
        IGenericRepository<Customer> CustomerRepository { get; }
        IGenericRepository<Delivery> DeliveryRepository { get; }
        IGenericRepository<Employee> EmployeeRepository { get; }
        IGenericRepository<Feedback> FeedbackRepository { get; }
        IGenericRepository<Image> ImageRepository { get; }
        IGenericRepository<Laptop> LaptopRepository { get; }
        IGenericRepository<Mobile> MobileRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<UserConfirm> UserConfirmRepository { get; }
        IGenericRepository<Warranty> WarrantyRepository { get; }

        /// <summary>
        /// Save db
        /// </summary>
        void Save();
    }
}
