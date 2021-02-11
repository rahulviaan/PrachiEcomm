using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class CategoryFilter
    {
        public int ParrentId { get; set; }
        public string Series { get; set; }
        public int Default { get; set; } = 64;
    }
    
}