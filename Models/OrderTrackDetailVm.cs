using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class OrderTrackDetailVm
    {
      
        public string TransactionId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Status { get; set; }
        public string UserBillingAddress { get; set; }
        public string ItemDestination { get; set; }
        public string UserId { get; set; }
        public string orderId { get; set; }
      
    }
}