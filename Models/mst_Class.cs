using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public partial class mst_Class
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public int OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> dtmAdd { get; set; }
        public Nullable<System.DateTime> dtmUpdate { get; set; }
        public Nullable<System.DateTime> dtmDelete { get; set; }
        public int Status { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public int idServer { get; set; }

    }

}