using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Orders;
using KLTN.DataModels.Models.ReportRevenue;
using KLTN.DataModels.Models.Users;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public int Create(OrderViewModel orderView, IEnumerable<OrderDetailViewModel> orderDetails)
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
                    RecipientDistrictName = orderView.RecipientDistrictName,
                    RecipientPrecinctName = orderView.RecipientPrecinctName,
                    RecipientProvinceName = orderView.RecipientProvinceName,
                    CreateBy = GetUserId(),
                    CreateAt = DateTime.UtcNow,
                    RecipientFirstName = orderView.RecipientFirstName,
                    RecipientLastName = orderView.RecipientLastName,
                    RecipientPhone = orderView.RecipientPhone,
                    RecipientEmail = orderView.RecipientEmail
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
                        ImageProduct = item.Image,
                        TotalPrice = item.TotalPrice
                    };
                    _unitOfWork.OrderDetailRepository.Create(ordDetail);
                    var product = _unitOfWork.ProductRepository.GetById(ordDetail.ProductId);
                    if (item.Quantity > product.Quantity) return 2;
                    product.TotalSold += 1;
                    product.Quantity -= ordDetail.Quantity;
                    var feedback = _unitOfWork.FeedbackRepository.Get(x =>
                                    x.ProductId == ordDetail.ProductId && x.UserId == order.CreateBy);
                    if (feedback == null)
                    {
                        var feed = new Feedback
                        {
                            ProductId = ordDetail.ProductId,
                            UserId = order.CreateBy,
                            IsBought = true,
                            Status = true,
                            CreateAt = DateTime.UtcNow
                        };

                        _unitOfWork.FeedbackRepository.Create(feed);
                    }
                    else
                    {
                        feedback.IsBought = true;
                    }
                }

                var delivery = new Delivery
                {
                    OrderId = ord.Id
                };

                _unitOfWork.DeliveryRepository.Create(delivery);

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create order", e);
                return -1;
            }
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple<OrderViewModel, int> GetOrderById(int? id)
        {
            if (id == null) return new Tuple<OrderViewModel, int>(null, -1);
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id.Value);
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
                                    Image = ordDT.ImageProduct,
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
        public Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int> GetOrderDetailById(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);
                var orderModel = _mapper.Map<OrderViewModel>(order);

                var ordDetail = from ordDT in _unitOfWork.OrderDetailRepository.ObjectContext
                                join pro in _unitOfWork.ProductRepository.ObjectContext on ordDT.ProductId equals pro.Id
                                where ordDT.OrderId == id
                                select new OrderDetailViewModel
                                {
                                    OrderId = ordDT.OrderId,
                                    ProductId = ordDT.ProductId,
                                    Price = ordDT.Price,
                                    Image = ordDT.ImageProduct,
                                    Quantity = ordDT.Quantity,
                                    ProductName = pro.Name,
                                    TotalPrice = ordDT.TotalPrice
                                };

                var delivery = (from deli in _unitOfWork.DeliveryRepository.ObjectContext
                                where deli.OrderId == id
                                select new DeliveryViewModel
                                {
                                    Id = deli.Id,
                                    OrderId = id,
                                    ConfirmAt = deli.ConfirmAt,
                                    PreparingOrderAt = deli.PreparingOrderAt,
                                    FinishPreparingOrderAt = deli.FinishPreparingOrderAt,
                                    StartDeliveryAt = deli.StartDeliveryAt,
                                    FinishDeliveryAt = deli.FinishDeliveryAt,
                                    CancelOrderAt = deli.CancelOrderAt,
                                    CancelContent = deli.CancelContent
                                }).FirstOrDefault();

                var deliveryModel = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                delivery.UserConfirmEmail = GetEmailById(deliveryModel.UserConfirmId);
                delivery.UserPreparingOrderEmail = GetEmailById(deliveryModel.UserPreparingOrderId);
                delivery.ShipperEmail = GetEmailById(deliveryModel.ShipperId);
                delivery.CancelByEmail = GetEmailById(deliveryModel.CancelBy);

                var tuple = new Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int>(orderModel, ordDetail, delivery, 1);

                return tuple;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get order detail in service", e);
                var tuple = new Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int>(null, null, null, -1);
                return tuple;
            }
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int> GetOrderDetailUserId(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.Get(x => x.Id == id && x.CreateBy == GetUserId());
                var orderModel = _mapper.Map<OrderViewModel>(order);

                var ordDetail = from ordDT in _unitOfWork.OrderDetailRepository.ObjectContext
                                join pro in _unitOfWork.ProductRepository.ObjectContext on ordDT.ProductId equals pro.Id
                                where ordDT.OrderId == id
                                select new OrderDetailViewModel
                                {
                                    OrderId = ordDT.OrderId,
                                    ProductId = ordDT.ProductId,
                                    Price = ordDT.Price,
                                    Image = ordDT.ImageProduct,
                                    Quantity = ordDT.Quantity,
                                    ProductName = pro.Name,
                                    TotalPrice = ordDT.TotalPrice
                                };

                var delivery = (from deli in _unitOfWork.DeliveryRepository.ObjectContext
                                where deli.OrderId == id
                                select new DeliveryViewModel
                                {
                                    Id = deli.Id,
                                    OrderId = id,
                                    ConfirmAt = deli.ConfirmAt,
                                    PreparingOrderAt = deli.PreparingOrderAt,
                                    FinishPreparingOrderAt = deli.FinishPreparingOrderAt,
                                    StartDeliveryAt = deli.StartDeliveryAt,
                                    FinishDeliveryAt = deli.FinishDeliveryAt,
                                    CancelOrderAt = deli.CancelOrderAt,
                                    CancelContent = deli.CancelContent
                                }).FirstOrDefault();

                var deliveryModel = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                delivery.UserConfirmEmail = GetEmailById(deliveryModel.UserConfirmId);
                delivery.UserPreparingOrderEmail = GetEmailById(deliveryModel.UserPreparingOrderId);
                delivery.ShipperEmail = GetEmailById(deliveryModel.ShipperId);
                delivery.CancelByEmail = GetEmailById(deliveryModel.CancelBy);

                var tuple = new Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int>(orderModel, ordDetail, delivery, 1);

                return tuple;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when get order detail in service", e);
                var tuple = new Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int>(null, null, null, -1);
                return tuple;
            }
        }

        public int OrderConfirm(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);

                if (order.StatusOrder > 0) return 0;

                var delivery = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);
                order.StatusOrder = 1;
                delivery.ConfirmAt = DateTime.UtcNow;
                delivery.UserConfirmId = GetUserId();

                _unitOfWork.Save();
                return 1;

            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in service", e.Message);
                return -1;
            }
        }

        public int StartPrepareOrder(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);
                if (order.StatusOrder < 1) return 2;
                if (order.StatusOrder > 1) return 3;
                var delivery = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                order.StatusOrder = 2;
                delivery.PreparingOrderAt = DateTime.UtcNow;
                delivery.UserPreparingOrderId = GetUserId();

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in service", e.Message);
                return -1;
            }
        }

        public int FinishPrepareOrder(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);

                if (order.StatusOrder < 2) return 2;

                if (order.StatusOrder > 2) return 3;

                var delivery = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                if (delivery.UserPreparingOrderId != GetUserId()) return 4;

                order.StatusOrder = 3;
                delivery.FinishPreparingOrderAt = DateTime.UtcNow;

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in service", e.Message);
                return -1;
            }
        }

        public int StartDeliveryOrder(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);
                if (order.StatusOrder < 3) return 2;
                if (order.StatusOrder > 3) return 3;

                var delivery = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                order.StatusOrder = 4;
                delivery.StartDeliveryAt = DateTime.UtcNow;
                delivery.ShipperId = GetUserId();

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in service", e.Message);
                return -1;
            }
        }

        public int FinishDeliveryOrder(int id)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);

                if (order.StatusOrder < 4) return 2;

                if (order.StatusOrder > 4) return 3;

                var delivery = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                if (delivery.UserPreparingOrderId != GetUserId()) return 4;

                order.StatusOrder = 5;
                delivery.FinishDeliveryAt = DateTime.UtcNow;

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in service", e.Message);
                return -1;
            }
        }

        public int CancelOrder(int id, string content)
        {
            try
            {
                var order = _unitOfWork.OrderRepository.GetById(id);

                if (order.StatusOrder == 5) return 2;
                if (order.StatusOrder == 6) return 3;

                var delivery = _unitOfWork.DeliveryRepository.Get(x => x.OrderId == id);

                var userId = GetUserId();

                if (order.StatusOrder < 1 || order.StatusOrder == 2 && delivery.UserPreparingOrderId == userId || order.StatusOrder == 4 && delivery.ShipperId == userId)
                {
                    order.StatusOrder = 6;
                    delivery.CancelOrderAt = DateTime.UtcNow;
                    delivery.CancelContent = content;
                    delivery.CancelBy = userId;

                    var ord = _unitOfWork.OrderDetailRepository.GetMany(x => x.OrderId == order.Id);

                    foreach (var item in ord)
                    {
                        var product = _unitOfWork.ProductRepository.GetById(item.ProductId);
                        product.Quantity += item.Quantity;
                        product.TotalSold -= item.Quantity;
                    }

                    _unitOfWork.Save();
                    return 1;
                }

                return 4;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when confirm order in service", e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrder(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.StatusOrder.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderByUser(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.StatusOrder.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderViewModel> GetListOrderByUser()
        {
            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.CreateBy == GetUserId()
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };
            listOrder = listOrder.OrderBy(x => x.CreateAt);

            return listOrder;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderWaitConfirm(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 0
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderWaitPrepare(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 1
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderPreparing(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 2
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderWaitDelivery(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 3
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderDelivering(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 4
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderFinish(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 5
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        /// Get all list categories
        /// </summary>
        /// <returns></returns>
        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderCancel(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            where order.StatusOrder == 6
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        public Tuple<IEnumerable<OrderViewModel>, int, int> LoadTaskByUserId(DTParameters dtParameters)
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

            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            join deli in _unitOfWork.DeliveryRepository.ObjectContext on order.Id equals deli.OrderId
                            where deli.UserPreparingOrderId == GetUserId() && order.StatusOrder == 2 ||
                            deli.UserPreparingOrderId == GetUserId() && order.StatusOrder == 4
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };

            if (!string.IsNullOrEmpty(searchBy))
            {
                listOrder = listOrder.Where(r =>
                    r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.CreateBy.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                    r.RecipientFirstName.ToString().ToUpper().Contains(searchBy.ToUpper()));
            }

            listOrder = orderAscendingDirection
                ? listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : listOrder.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var filteredResultsCount = listOrder.ToArray().Count();
            var totalResultsCount = listOrder.Count();

            var tuple = new Tuple<IEnumerable<OrderViewModel>, int, int>(listOrder, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        //public IEnumerable<OrderDetailViewModel> GetOrderDe

        public Tuple<IEnumerable<OrderViewModel>, int> GetMission()
        {
            var listOrder = from order in _unitOfWork.OrderRepository.ObjectContext
                            join usc in _unitOfWork.UserRepository.ObjectContext on order.CreateBy equals usc.Id
                            join deli in _unitOfWork.DeliveryRepository.ObjectContext on order.Id equals deli.OrderId
                            where deli.UserPreparingOrderId == GetUserId() && order.StatusOrder == 2 ||
                            deli.UserPreparingOrderId == GetUserId() && order.StatusOrder == 4
                            select new OrderViewModel
                            {
                                Id = order.Id,
                                TotalPrice = order.TotalPrice,
                                RecipientPhone = order.RecipientPhone,
                                RecipientFirstName = order.RecipientFirstName,
                                RecipientLastName = order.RecipientLastName,
                                RecipientProvinceCode = order.RecipientProvinceCode,
                                RecipientProvinceName = order.RecipientProvinceName,
                                RecipientDistrictCode = order.RecipientDistrictCode,
                                RecipientDistrictName = order.RecipientDistrictName,
                                RecipientPrecinctCode = order.RecipientPrecinctCode,
                                RecipientPrecinctName = order.RecipientPrecinctName,
                                RecipientAddress = order.RecipientAddress,
                                RecipientEmail = order.RecipientEmail,
                                CreateAt = order.CreateAt,
                                CreateBy = order.CreateBy,
                                StatusOrder = order.StatusOrder
                            };
            listOrder = listOrder.OrderBy(x => x.Id).Take(3);
            var totalMission = listOrder.Count();
            return new Tuple<IEnumerable<OrderViewModel>, int>(listOrder, totalMission);
        }

        public IEnumerable<User> GetAllShipper()
        {
            var listShipper = _unitOfWork.UserRepository.GetMany(x => x.Role == (int)EnumRole.Shipper && x.Status == true);

            return listShipper;
        }

        public IEnumerable<User> GetAllWareHouseStaff()
        {
            var listWareHouseStaff = _unitOfWork.UserRepository.GetMany(x => x.Role == (int)EnumRole.WareHouseStaff && x.Status == true);

            return listWareHouseStaff;
        }

        public bool Update(int id, int status, int userId)
        {
            return true;
        }

        private int GetUserId()
        {
            var userId = Convert.ToInt32(_httpContext.User.FindFirst(c => c.Type == "Id").Value);
            return userId;
        }

        private string GetEmailById(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            return user != null ? user.Email : "";
        }

        public IEnumerable<OrderChartModel> GetOrderChart(FilterModel isMonth)
        {
            var soldOrder = new List<OrderChartModel>();

            if (!string.IsNullOrEmpty(isMonth.Filter))
            {
                var listOrder = _unitOfWork.OrderRepository.GetMany(x => x.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && x.CreateAt.Year == DateTime.UtcNow.Year);

                var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var dateStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                for (var i = 0; i < daysInMonth; i++)
                {
                    var orderInDay = new OrderChartModel
                    {
                        Date = dateStart.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        TotalOrder = listOrder.Where(x => x.CreateAt.Date.Day == dateStart.Date.Day
                                                    && x.CreateAt.Month == dateStart.Month).Count(),
                        TotalOrderFinish = listOrder.Where(x => x.CreateAt.Date.Day == dateStart.Date.Day
                                                    && x.CreateAt.Month == dateStart.Month && x.StatusOrder == 5).Count()
                    };
                    soldOrder.Add(orderInDay);
                    dateStart = dateStart.AddDays(1);
                }
            }
            else
            {

                var listOrder = _unitOfWork.OrderRepository.GetMany(x => x.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && x.CreateAt.Year == DateTime.UtcNow.Year);


                var dateStartOfWeek = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);

                for (var i = 0; i < 7; i++)
                {
                    var orderInDay = new OrderChartModel
                    {
                        Date = dateStartOfWeek.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        TotalOrder = listOrder.Where(x => x.CreateAt.Date.Day == dateStartOfWeek.Date.Day
                                                    && x.CreateAt.Month == dateStartOfWeek.Month).Count(),
                        TotalOrderFinish = listOrder.Where(x => x.CreateAt.Date.Day == dateStartOfWeek.Date.Day
                                                    && x.CreateAt.Month == dateStartOfWeek.Month && x.StatusOrder == 5).Count()
                    };
                    soldOrder.Add(orderInDay);
                    dateStartOfWeek = dateStartOfWeek.AddDays(1);
                }
            }

            return soldOrder;
        }

        public IEnumerable<ProductChartModel> GetTopSoldChart(FilterModel isMonth)
        {
            var listGetTopProduct = new List<ProductChartModel>();

            if (!string.IsNullOrEmpty(isMonth.Filter))
            {
                var listOrdDTSold = from ordDT in _unitOfWork.OrderDetailRepository.ObjectContext
                                    join ord in _unitOfWork.OrderRepository.ObjectContext on ordDT.OrderId equals ord.Id
                                    where ord.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && ord.CreateAt.Year == DateTime.UtcNow.Year
                                                                    && ord.StatusOrder == 5
                                    select new
                                    {
                                        ProductId = ordDT.ProductId,
                                        Quantity = ordDT.Quantity
                                    };


                listGetTopProduct = listOrdDTSold.GroupBy(x => x.ProductId).Select(y =>
                                    new ProductChartModel
                                    {
                                        IdProduct = y.Key,
                                        Count = y.Sum(x => x.Quantity)
                                    }).OrderBy(x => x.Count).Take(6).ToList();

                foreach (var item in listGetTopProduct)
                {
                    item.NameProduct = _unitOfWork.ProductRepository.GetById(item.IdProduct).Name;
                }
            }
            else
            {

                var listOrder = _unitOfWork.OrderRepository.GetMany(x => x.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && x.CreateAt.Year == DateTime.UtcNow.Year & x.StatusOrder == 5);


                var dateStartOfWeek = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);

                var listOrdDTSold = from ordDT in _unitOfWork.OrderDetailRepository.ObjectContext
                                    join ord in _unitOfWork.OrderRepository.ObjectContext on ordDT.OrderId equals ord.Id
                                    where ord.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && ord.CreateAt.Year == DateTime.UtcNow.Year
                                                                    && DateTime.Compare(ord.CreateAt, dateStartOfWeek) >= 0
                                                                    && DateTime.Compare(dateStartOfWeek.AddDays(7), ord.CreateAt) < 0
                                                                    && ord.StatusOrder == 5
                                    select new
                                    {
                                        ProductId = ordDT.ProductId,
                                        Quantity = ordDT.Quantity
                                    };


                listGetTopProduct = listOrdDTSold.GroupBy(x => x.ProductId).Select(y =>
                                    new ProductChartModel
                                    {
                                        IdProduct = y.Key,
                                        Count = y.Sum(x => x.Quantity)
                                    }).OrderBy(x => x.Count).Take(6).ToList();

                foreach (var item in listGetTopProduct)
                {
                    item.NameProduct = _unitOfWork.ProductRepository.GetById(item.IdProduct).Name;
                }
            }

            return listGetTopProduct;
        }

        public IEnumerable<RevenueModel> GetRevenue(FilterModel isMonth)
        {
            var revenueModel = new List<RevenueModel>();

            if (!string.IsNullOrEmpty(isMonth.Filter))
            {
                var listOrder = _unitOfWork.OrderRepository.GetMany(x => x.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && x.CreateAt.Year == DateTime.UtcNow.Year
                                                                    && x.StatusOrder == 5);

                var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                var dateStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                for (var i = 0; i < daysInMonth; i++)
                {
                    var revenueInday = new RevenueModel
                    {
                        Date = dateStart.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        Revenue = listOrder.Where(x => x.CreateAt.Date.Day == dateStart.Date.Day
                                                    && x.CreateAt.Month == dateStart.Month).Sum(x => x.TotalPrice),
                    };
                    revenueModel.Add(revenueInday);
                    dateStart = dateStart.AddDays(1);
                }
            }
            else
            {

                var listOrder = _unitOfWork.OrderRepository.GetMany(x => x.CreateAt.Month == DateTime.UtcNow.Month
                                                                    && x.CreateAt.Year == DateTime.UtcNow.Year
                                                                    && x.StatusOrder == 5);

                var dateStartOfWeek = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);

                for (var i = 0; i < 7; i++)
                {
                    var revenueInDay = new RevenueModel
                    {
                        Date = dateStartOfWeek.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        Revenue = listOrder.Where(x => x.CreateAt.Date.Day == dateStartOfWeek.Date.Day
                                                    && x.CreateAt.Month == dateStartOfWeek.Month).Sum(x => x.TotalPrice),
                    };
                    revenueModel.Add(revenueInDay);
                    dateStartOfWeek = dateStartOfWeek.AddDays(1);
                }
            }

            return revenueModel;
        }

        public IEnumerable<ReportRevenueModel> GetReportRevenue(FilterDateModel model)
        {
            var listRevenue = new List<ReportRevenueModel>();

            if (model.StartDate == null && model.EndDate != null)
            {
                var listOrderInDay = _unitOfWork.OrderRepository.GetMany(x=>DateTime.Compare(model.EndDate,x.CreateAt)==0);
                var revenue = new ReportRevenueModel {
                    Date = model.EndDate.ToString("dd-MMM", new CultureInfo("vi-VN")),
                    TotalOrder = listOrderInDay.Count(),
                    TotalOrderCancel = listOrderInDay.Where(x => x.StatusOrder == 6).Count(),
                    TotalOrderFinish = listOrderInDay.Where(x => x.StatusOrder == 5).Count(),
                    TotalRevenue = listOrderInDay.Sum(x => x.TotalPrice)
                };

                listRevenue.Add(revenue);
            }

            if (model.StartDate != null && model.EndDate == null)
            {
                var listOrderInDay = _unitOfWork.OrderRepository.GetMany(x => DateTime.Compare(model.StartDate, x.CreateAt) == 0);
                var revenue = new ReportRevenueModel
                {
                    Date = model.StartDate.ToString("dd-MMM", new CultureInfo("vi-VN")),
                    TotalOrder = listOrderInDay.Count(),
                    TotalOrderCancel = listOrderInDay.Where(x => x.StatusOrder == 6).Count(),
                    TotalOrderFinish = listOrderInDay.Where(x => x.StatusOrder == 5).Count(),
                    TotalRevenue = listOrderInDay.Sum(x => x.TotalPrice)
                };

                listRevenue.Add(revenue);
            }

            if(model.StartDate != null && model.EndDate != null)
            {
                var listOrderInDay = _unitOfWork.OrderRepository.GetMany(x => DateTime.Compare(model.StartDate, x.CreateAt) >= 0 
                                                                         && DateTime.Compare(model.EndDate,x.CreateAt) <= 0);

                var totalDay = (model.EndDate - model.StartDate).TotalDays;

                for (int i = 0; i < totalDay; i++)
                {
                    var revenue = new ReportRevenueModel
                    {
                        Date = model.StartDate.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        TotalOrder = listOrderInDay.Where(x => DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalOrderCancel = listOrderInDay.Where(x => x.StatusOrder == 6
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalOrderFinish = listOrderInDay.Where(x => x.StatusOrder == 5
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalRevenue = listOrderInDay.Where(x=>x.StatusOrder==5 
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Sum(x => x.TotalPrice)
                    };

                    listRevenue.Add(revenue);
                    model.StartDate.AddDays(1);
                }
            }

            if(!string.IsNullOrEmpty(model.MonthDate) && !string.IsNullOrEmpty(model.YearDate))
            {
                var year = Convert.ToInt32(model.YearDate);
                var month = Convert.ToInt32(model.MonthDate);

                var startDate = new DateTime(year,month,1);
                var totalDayInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

                var listOrderInDay = _unitOfWork.OrderRepository.GetMany(x => DateTime.Compare(startDate, x.CreateAt) >= 0
                                                                         && DateTime.Compare(startDate.AddDays(totalDayInMonth-1)
                                                                         , x.CreateAt) <= 0);

                var totalDay = (model.EndDate - model.StartDate).TotalDays;

                for (int i = 0; i < totalDayInMonth; i++)
                {
                    var revenue = new ReportRevenueModel
                    {
                        Date = startDate.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        TotalOrder = listOrderInDay.Where(x => DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalOrderCancel = listOrderInDay.Where(x => x.StatusOrder == 6
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalOrderFinish = listOrderInDay.Where(x => x.StatusOrder == 5
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalRevenue = listOrderInDay.Where(x => x.StatusOrder == 5
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Sum(x => x.TotalPrice)
                    };

                    listRevenue.Add(revenue);
                    startDate.AddDays(1);
                }
            }
            else if(!string.IsNullOrEmpty(model.MonthDate) && string.IsNullOrEmpty(model.YearDate))
            {
                var month = Convert.ToInt32(model.MonthDate);

                var startDate = new DateTime(DateTime.UtcNow.Year, month, 1);
                var totalDayInMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year, startDate.Month);

                var listOrderInDay = _unitOfWork.OrderRepository.GetMany(x => DateTime.Compare(startDate, x.CreateAt) >= 0
                                                                         && DateTime.Compare(startDate.AddDays(totalDayInMonth-1)
                                                                         , x.CreateAt) <= 0);

                for (int i = 0; i < totalDayInMonth; i++)
                {
                    var revenue = new ReportRevenueModel
                    {
                        Date = startDate.ToString("dd-MMM", new CultureInfo("vi-VN")),
                        TotalOrder = listOrderInDay.Where(x => DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalOrderCancel = listOrderInDay.Where(x => x.StatusOrder == 6
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalOrderFinish = listOrderInDay.Where(x => x.StatusOrder == 5
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Count(),
                        TotalRevenue = listOrderInDay.Where(x => x.StatusOrder == 5
                                            && DateTime.Compare(model.StartDate, x.CreateAt) == 0).Sum(x => x.TotalPrice)
                    };

                    listRevenue.Add(revenue);
                    startDate.AddDays(1);
                }
            }

            return listRevenue;
        }
    }
}
public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}



