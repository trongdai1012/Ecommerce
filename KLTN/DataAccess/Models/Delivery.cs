using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Delivery
    {
        public int OrderId { get; set; }
        public DateTime ConfirmAt { get; set; }
        public int UserConfirmId { get; set; }
        public DateTime PreparingOrderAt { get; set; }
        public int UserPreparingOrderId { get; set; }
        public DateTime FinishPreparingOrderAt { get; set; }
        public string ShipperName { get; set; }
        public int ShipperId { get; set; }
        public string ShipperPhone { get; set; }
        public DateTime StartDeliveryAt { get; set; }
        public DateTime FinishDeliveryAt { get; set; }
        public DateTime CancelDeliveryAt { get; set; }
        public byte Status { get; set; }

        public virtual Order Order { get; set; }
    }
}
