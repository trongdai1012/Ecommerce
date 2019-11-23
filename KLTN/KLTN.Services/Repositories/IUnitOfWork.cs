using KLTN.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KLTN.Services.Repositories
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork
    {
        //declare entities Repository
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Brand> BrandRepository { get; }
        IGenericRepository<Contact> ContactRepository { get; }
        IGenericRepository<ConfirmForgot> ForgotRepository { get; }
        IGenericRepository<DataTest> DataTestRepository { get; }
        IGenericRepository<DataTrain> DataTrainRepository { get; }
        IGenericRepository<Delivery> DeliveryRepository { get; }
        IGenericRepository<Feedback> FeedbackRepository { get; }
        IGenericRepository<Image> ImageRepository { get; }
        IGenericRepository<Laptop> LaptopRepository { get; }
        IGenericRepository<Mobile> MobileRepository { get; }
        IGenericRepository<News> NewsRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<UserConfirm> UserConfirmRepository { get; }

        /// <summary>
        /// Save db
        /// </summary>
        void Save();

        Task SaveAsync();
    }
}
