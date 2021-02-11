using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class DispatchOrderResponse
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public long ItemId { get; set; }
        public string Flag { get; set; }
        public string Image { get; set; }
        public string AWBNO { get; set; }
        public string DispatchedBy { get; set; }
        public string TransactionId { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public string TotalAmount { get; set; }
        public string Status { get; set; }
    }
}