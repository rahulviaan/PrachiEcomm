using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class SubscriptionMail
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string TransactionId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}