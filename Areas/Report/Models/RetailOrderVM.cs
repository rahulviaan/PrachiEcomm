using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.Report.Models
{
    public class RetailOrderVM
    {
        public string TransactionID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string classes { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public List<RetailOrderVM> RetailOrderVMsList { get; set; }


    }
}