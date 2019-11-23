using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Models.Orders
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ConfirmAt { get; set; }
        public string UserConfirmEmail { get; set; }
        public DateTime PreparingOrderAt { get; set; }
        public string UserPreparingOrderEmail { get; set; }
        public DateTime FinishPreparingOrderAt { get; set; }
        public string ShipperEmail { get; set; }
        public DateTime StartDeliveryAt { get; set; }
        public DateTime FinishDeliveryAt { get; set; }
        public DateTime CancelOrderAt { get; set; }
        public string CancelByEmail { get; set; }
        public string CancelContent { get; set; }
    }
}
