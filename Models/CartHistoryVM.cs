using PrachiIndia.Sql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PrachiIndia.Web.Areas.Model;
namespace PrachiIndia.Portal.Models
{
    public class CartHistoryVM : FilterOrder
    {
        public string Title { get; set; }
        public decimal price { get; set; }
        public int Quantity { get; set; }
        public string date { get; set; }
        public bool status { get; set; }
        public string TransactionId { get; set; }
        public string Image { get; set; }
        public string OrderId { get; set; }
        public decimal Discount { get; set; }
        public string BookType { get; set; }
        public int Type { get; set; }
        public string Dispatchby { get; set; }
        public string AWBNO { get; set; }
        public int Book { get; set; }
        public decimal TaxPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
