using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PrachiIndia.Portal.Models
{
    
    public class ExternalPayment
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name="Country")]
        public string CounteryId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Pincode { get; set; }
    }
}