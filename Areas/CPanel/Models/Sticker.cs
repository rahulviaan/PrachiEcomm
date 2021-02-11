using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class Sticker
    {
        public virtual string SchoolName{get;set;}
        public virtual string PONumber {get;set;}
        public virtual string Email {get;set;}
        public virtual string PhoneNumber {get;set; }
        public virtual string MObileNo {get;set; }
        public virtual string Address {get;set; }
        public virtual string Dvds { get; set; }
    }
}