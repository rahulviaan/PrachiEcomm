using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.Marketing.Models
{
    public class MarketingModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Mobile { get; set; }
      
        public string Area { get; set; }
        [Required]
        public long DeviceCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool MobileConfirmed { get; set; }

    }
}