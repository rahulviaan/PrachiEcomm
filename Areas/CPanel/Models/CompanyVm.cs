using System;
using System.ComponentModel.DataAnnotations;
namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class CompanyVm
    {
        public int id { get; set; }
        [Required]
        public string StateName { get; set; }
        [Required]
        public string H_No { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string NearBy { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Pincode { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string mobileno { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public Nullable<bool> IsMainOffice { get; set; }
        public string Color { get; set; }
    }
}