using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrachiIndia.Portal.Helpers;


namespace PrachiIndia.Portal.Models
{
    public class VMCatalogue:LoginViewModel
    {
        public string Returnurl { get; set; }
        public string BookId { get; set; }
    }
}