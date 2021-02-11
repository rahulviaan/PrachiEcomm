using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace PrachiIndia.Portal.Models
{
    public class OrderTrackVm : CartHistoryVM
    {
        [Required]
        public string AWBNO { get; set; }
        [Required]
        public string DispatchedBy { get; set; }
        public string sttt { get; set; }
        public string OrderRecivedBy { get; set; }
        public bool IsRecive { get; set; }
        public string Remark { get; set; }
        public string ReciveDate { get; set; }
        public string IsProduct { get; set; }
        public string InventoryMessage { get; set; }
        public string classs { get; set; }
        public string Board { get; set; }
        public decimal TaxPrice { get; set; }
        public decimal TotalAmount { get; set; }
    }

}