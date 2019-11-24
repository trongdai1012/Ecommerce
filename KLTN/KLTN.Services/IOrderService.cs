using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface IOrderService
    {
        Tuple<OrderViewModel, int> GetOrderById(int? id);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrder(DTParameters dtParameters);

        bool Create(OrderViewModel orderView, IEnumerable<OrderDetailViewModel> orderDetails);

        Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int> GetOrderDetailById(int id);

        IEnumerable<User> GetAllShipper();

        IEnumerable<User> GetAllClerk();

        IEnumerable<User> GetAllWareHouseStaff();

        int OrderConfirm(int id);

        int StartPrepareOrder(int id);

        int FinishPrepareOrder(int id);

        int StartDeliveryOrder(int id);

        int FinishDeliveryOrder(int id);

        int CancelOrder(int id, string content);
    }
}
