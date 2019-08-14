using System;

namespace DataAccess.Models
{
    class Delivery
    {
        public int OrderId { get; set; }
        public string ShipperName { get; set; }
        public DateTime ConfirmAt { get; set; }
        public string UserConfirm { get; set; }
        public DateTime PreparingOrderAt { get; set; }
        public string UserPreparingOrder { get; set; }
        public DateTime FinishPreparingOrderAt { get; set; }
        public string Deliver { get; set; }
        public DateTime StartDeliveryAt { get; set; }
        public DateTime FinishDeliveryAt { get; set; }
        public DateTime CancelDeliveryAt { get; set; }
        public bool Status { get; set; }
    }
}
