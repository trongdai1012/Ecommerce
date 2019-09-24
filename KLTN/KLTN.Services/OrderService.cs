using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Orders;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLTN.Services
{
    public class OrderService : IOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor category service
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="httpContext"></param>
        /// <param name="genericRepository"></param>
        /// <param name="genericRepositoryUser"></param>
        public OrderService(IMapper mapper, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext.HttpContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool Create(OrderViewModel orderView, IEnumerable<OrderDetailViewModel> orderDetails)
        {
            try
            {
                var order = new Order
                {
                    TotalPrice = orderView.TotalPrice,
                    RecipientAddress = orderView.RecipientAddress,
                    RecipientDistrictCode = orderView.RecipientDistrictCode,
                    RecipientPrecinctCode = orderView.RecipientPrecinctCode,
                    RecipientProvinceCode = orderView.RecipientProvinceCode,
                    CreateBy = GetUserId(),
                    CreateAt = DateTime.UtcNow,
                    RecipientName = orderView.RecipientFirstName + orderView.RecipientLastName,
                    RecipientPhone = orderView.RecipientPhone,
                    Status = true
                };
                var ord = _unitOfWork.OrderRepository.Create(order);

                foreach (var item in orderDetails)
                {
                    var ordDetail = new OrderDetail
                    {
                        OrderId = ord.Id,
                        Price = item.Price,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Image = item.Image
                    };
                    _unitOfWork.OrderDetailRepository.Create(ordDetail);
                }

                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create order", e);
                return false;
            }
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<OrderViewModel, int> GetOrderById(int? id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);
                if (order == null) return new Tuple<OrderViewModel, int>(null, 0);
                var orderModel = _mapper.Map<OrderViewModel>(order);
                return new Tuple<OrderViewModel, int>(orderModel, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get order by id in order service", e);
                return new Tuple<OrderViewModel, int>(null, -1);
            }
        }

        public bool CreateOrderDetail(OrderDetailViewModel orderDetail)
        {
            try
            {
                var ordDetail = new OrderDetail
                {
                    OrderId = orderDetail.OrderId,
                    Price = orderDetail.Price,
                    ProductId = orderDetail.ProductId,
                    Quantity = orderDetail.Quantity,
                    Image = orderDetail.Image
                };
                _unitOfWork.OrderDetailRepository.Create(ordDetail);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create OrderDetail in OrderService", e);
                return false;
            }

        }

        public Tuple<IEnumerable<OrderDetailViewModel>, int> GetOrdDetailByOrderId(int id)
        {
            try
            {
                var ordDetail = from ordDT in _unitOfWork.OrderDetailRepository.ObjectContext
                                join pro in _unitOfWork.ProductRepository.ObjectContext on ordDT.ProductId equals pro.Id
                                where ordDT.OrderId == id
                                select new OrderDetailViewModel
                                {
                                    OrderId = ordDT.OrderId,
                                    ProductId = ordDT.ProductId,
                                    Price = ordDT.Price,
                                    Image = ordDT.Image,
                                    Quantity = ordDT.Quantity
                                };
                return new Tuple<IEnumerable<OrderDetailViewModel>, int>(ordDetail, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get OrderDetail by OrderId in OrderService", e);
                return null;
            }
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadLaptop(DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            string orderCriteria;
            bool orderAscendingDirection;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == ParamConstants.Asc;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = ParamConstants.Id;
                orderAscendingDirection = true;
            }

            var listOrder = _unitOfWork.OrderRepository.GetAll();
            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = _mapper.Map<IEnumerable<OrderViewModel>>(listOrder);
            var filteredResultsCount = viewModels.ToArray().Count();
            var totalResultsCount = viewModels.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        private int GetUserId()
        {
            var userId = Convert.ToInt32(_httpContext.User.FindFirst(c => c.Type == "Id"));
            return userId;
        }
    }
}
