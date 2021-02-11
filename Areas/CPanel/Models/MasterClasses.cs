using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrachiIndia.Web.Areas.Model
{
    
    public   class MasterClasses
    {


        public long Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public Nullable<int> OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> dtmAdd { get; set; }
        public Nullable<System.DateTime> dtmUpdate { get; set; }
        public Nullable<System.DateTime> dtmDelete { get; set; }
        public Nullable<int> Status { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public int IdServer { get;  set; }

        [Display(Name = "Map Class")]
        public int IdClass { get; set; }
    }

}