using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrachiIndia.Web.Areas.Model
{

    public class MasterBoards
    {

        public long Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public int? OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime? dtmAdd { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }
        public int? Status { get; set; }
        public string IpAddress { get; set; }
        public string USerId { get; set; }
        public int IdServer { get;   set; }
        [Display(Name = "Map Board")]
        public int IdBoard { get; set; }
    }

}