using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class AspNetUserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }
        public bool Status { get; set; }
        public bool IsVerified { get; set; }
        public string AboutMe { get; set; }
        public string ProfileImage { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string Industry { get; set; }
        public string PANId { get; set; }
        public string PassportNo { get; set; }
        public string DlNo { get; set; }
        public string Remark { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
