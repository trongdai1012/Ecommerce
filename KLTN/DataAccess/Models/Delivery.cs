﻿using System;
using System.Collections.Generic;

namespace KLTN.DataAccess.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ConfirmAt { get; set; }
        public int UserConfirmId { get; set; }
        public DateTime PreparingOrderAt { get; set; }
        public int UserPreparingOrderId { get; set; }
        public DateTime FinishPreparingOrderAt { get; set; }
        public int ShipperId { get; set; }
        public DateTime StartDeliveryAt { get; set; }
        public DateTime FinishDeliveryAt { get; set; }
        public DateTime CancelOrderAt { get; set; }
        public int CancelBy { get; set; }
        public string CancelContent { get; set; }

        public virtual Order Order { get; set; }
        public virtual User User { get; set; }
    }
}
