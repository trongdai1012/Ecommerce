using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Orders;
using System;
using System.Collections.Generic;

namespace KLTN.Services
{
    public interface IOrderService
    {
        Tuple<OrderViewModel, int> GetOrderById(int? id);

        IEnumerable<OrderViewModel> GetListOrderByUser();

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderByUser(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrder(DTParameters dtParameters);

        int Create(OrderViewModel orderView, IEnumerable<OrderDetailViewModel> orderDetails);

        Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int> GetOrderDetailById(int id);

        IEnumerable<User> GetAllShipper();

        IEnumerable<User> GetAllWareHouseStaff();

        int OrderConfirm(int id);

        int StartPrepareOrder(int id);

        int FinishPrepareOrder(int id);

        int StartDeliveryOrder(int id);

        int FinishDeliveryOrder(int id);

        int CancelOrder(int id, string content);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderWaitConfirm(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderWaitPrepare(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderPreparing(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderWaitDelivery(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderDelivering(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderFinish(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadOrderCancel(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadTaskByUserId(DTParameters dtParameters);

        Tuple<IEnumerable<OrderViewModel>, int> GetMission();

        Tuple<OrderViewModel, IEnumerable<OrderDetailViewModel>, DeliveryViewModel, int> GetOrderDetailUserId(int id);
    }
}
