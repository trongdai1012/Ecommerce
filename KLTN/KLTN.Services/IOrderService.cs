using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.Services
{
    public interface IOrderService
    {
        Tuple<OrderViewModel, int> GetOrderById(int? id);

        Tuple<IEnumerable<OrderViewModel>, int, int> LoadLaptop(DTParameters dtParameters);

        bool Create(OrderViewModel orderView, IEnumerable<OrderDetailViewModel> orderDetails);
    }
}
