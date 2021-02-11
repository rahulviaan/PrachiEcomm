using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class OrderVm
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProductInfo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string TransactionId { get; set; }
        public string RequestLog { get; set; }
        public string RequestHash { get; set; }
        public string Error { get; set; }
        public string PGType { get; set; }
        public string PayUMoneyId { get; set; }
        public string AddtionalCharge { get; set; }
        public string ResponseLog { get; set; }
        public string ResponseHas { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
       
    }
}