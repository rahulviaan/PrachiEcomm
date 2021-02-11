using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Models
{
    public class objSuccess
    {

        public int isSuccess { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        //AdditionalFields = "Category_ID",
        [Required]
        [EmailAddress]
        //[Remote("doesUserNameExist", "PDP", ErrorMessage = "entered username or password may be invalid.")]
        [Display(Name = "Email")]
        [Remote("EmailExists", "Account", ErrorMessage = "Email already exists...")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        // [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [MinLength(6, ErrorMessage = "Password will be at least 6 character long")]
        public string Password { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Remote("doesUserPasswordExist", "Account", AdditionalFields = "Email", ErrorMessage = "entered password is invalid.")]
        public string OldPassword { get; set; }
        //[Required]
        public string newPassword { get; set; }
        //[System.ComponentModel.DataAnnotations.Compare("newPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password does not match")]
        public string confirmPassword { get; set; }
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string Returnurl { get; set; }
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please Select Country")]
        public int CounteryId { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please Select State")]
        [Required]
        public int StateId { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }

        public int CityId { get; set; }
        public IEnumerable<SelectListItem> CityList { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string Id { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }

        public string ProfilePic { get; set; }
        public string SubscriptionId { get; set; }
        public int idServer { get; set; }
        public string MachineKey { get; set; }
        public string Description { get; set; }
        public string ReaderKey { get; set; }
        public string ReaderId { get; set; }
        public string UserId { get; set; }
        public DeviceType devType { get; set; }
        public string Role { get; set; }
        public string RoleId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }


    public class StaffModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [Remote("EmailExists", "Account", ErrorMessage = "Email already exists...")]
        public string Email { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
        public string Pincode { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
    }
}
