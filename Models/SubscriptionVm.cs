using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class SubscriptionVm
    {
        public long Id { get; set; }
        public string UserId { get; set; }    
        public Nullable<decimal> Amount { get; set; }
        public string TransactionId { get; set; }
        public int SubscriptionId { get; set; }
        public string Class { get; set; }
        public string Board { get; set; }
        public string PlanName { get; set; }
        public string PlanTime { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string subscriptionType { get; set; }
        public Nullable<int> Exp { get; set; }
      
        public Nullable<bool> IsActive { get; set; }
      
        public string BookId { get; set; }
    }
}