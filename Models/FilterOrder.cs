using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class FilterOrder
    {
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string TransctionId { get; set; }
        public string TranId { get; set; }
    }
}