using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrachiIndia.Web.Areas.Model
{
    
    public   class CatalogClass
    {


        public long Id { get; set; }
        public Nullable<long> CatalogId { get; set; }
        public Nullable<long> ClassId { get; set; }
        public Nullable<System.DateTime> dtmAdd { get; set; }
        public Nullable<System.DateTime> dtmUpdate { get; set; }
        public Nullable<System.DateTime> dtmDelete { get; set; }
        public Nullable<int> Status { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }

    }

}