using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class UserMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }
        public bool Status { get; set; }
        public bool IsVerified { get; set; }
        public string AboutMe { get; set; }
        public string ProfileImage { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string Industry { get; set; }
        public string Panid { get; set; }
        public string PassportNo { get; set; }
        public string DlNo { get; set; }
        public string Remark { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public bool DownloadImage { get; set; }
    }
}
