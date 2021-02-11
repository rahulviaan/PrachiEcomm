using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class OrderDashboardVm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string CreatedDate { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}