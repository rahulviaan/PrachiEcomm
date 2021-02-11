using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class OrderTrackUpdateVm
    {
        public string OrderId { get; set; }
        public string OrderRecivedBy { get; set; }
        public bool IsRecive { get; set; }
        public string Remark { get; set; }
        public DateTime ReciveDate { get; set; }
        public string Title { get; set; }
        public string price { get; set; }
        public string Quantity { get; set; }
        public string date { get; set; }
        public bool status { get; set; }
        public string TransactionId { get; set; }
        public string Image { get; set; }
        public string Discount { get; set; }
        public string BookType { get; set; }
        public string Dispatchby { get; set; }
        public string AWBNO { get; set; }


    }
}