using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class UserLogin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
        public string UserName { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
        public string UserRoles { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public partial class AspNetUsers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }
        public DateTime dtmAdd { get; set; }
        public DateTime dtmUpdate { get; set; }
        public DateTime dtmDelete { get; set; }
        public int Status { get; set; }
        public int IsVerified { get; set; }
        public DateTime dtmDob { get; set; }
        public string AboutMe { get; set; }
        public string  ProfileImage { get; set; }
        public string  City { get; set; }
        public string State { get; set; }
        public string  Country { get; set; }
        public long CountryId { get; set; }
        public long StateId { get; set; }
        public long CityId { get; set; }
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
        public long idServer { get; set; }
        public string ReaderKey { get; set; }
        public string extra { get; set; }
        public int idextra { get; set; }
        public int idextra1 { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public int DeviceCount { get; set; }
    }

    public partial class AspNetRoles
    {
        public string Id { get; set; }
        public string Name { get; set; }
     
    }

    public partial class UserBookIds
    {
        public string Id { get; set; }
        public Int64 BookId { get; set; }
    }

    public partial class ReadEdgeLogins
    {
        public int Id { get; set; }
        public int AllowedSystems { get; set; }
        public int CurrentLogins { get; set; }
        public string Userid { get; set; }
        public bool LoginAllowed { get; set; }
    }

    public partial class ClassSubjects
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        public int OredrNo { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
    }
    public partial class ReadEdgeUserLoginInfo
    {
        public int Id { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string Userid { get; set; }
        public bool LogedOut { get; set; }
    }
    public partial class ReadEdgeTrialUsers
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
    }
    public partial class ErrorLog
    {
        public Int64 Id { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public partial class TeacherSubjectClass
    {
        public Int64 Id { get; set; }
        public string Teacherid { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
    }

    public partial class SubscriptionPayment {
        public Int64 Id { get; set; }
        public string UserId  { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string RequestLog { get; set; }
        public string RequestHash { get; set; }
        public string Error { get; set; }
        public string PGType { get; set; }
        public string PayUMoneyId { get; set; }
        public string AddtionalCharge { get; set; }
        public string ResponseLog { get; set; }
        public string ResponseHas { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int DiscountPer { get; set; }
        public decimal TotalAmount { get; set; }
        public int SubscriptionId { get; set; }
    }

    public partial class UserSubscription
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string TransactionId { get; set; }
        public Nullable<bool> Active { get; set; }

        public virtual AspNetUsers AspNetUser { get; set; }
        public virtual SubscriptionMaster SubscriptionMaster { get; set; }
    }

    public partial class SubscriptionMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubscriptionMaster()
        {
            this.SubscriptionPayments = new HashSet<SubscriptionPayment>();
            this.UserSubscriptions = new HashSet<UserSubscription>();
        }

        public int Id { get; set; }
        public string SubscriptionType { get; set; }
        public Nullable<int> DaysValidity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Image { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Prefix { get; set; }
        public string Suffix { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubscriptionPayment> SubscriptionPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }

    public partial class ReadEdgeSubscriptionPayment
    {
        public Int64 Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string RequestLog { get; set; }
        public string RequestHash { get; set; }
        public string Error { get; set; }
        public string PGType { get; set; }
        public string PayUMoneyId { get; set; }
        public string AddtionalCharge { get; set; }
        public string ResponseLog { get; set; }
        public string ResponseHas { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int DiscountPer { get; set; }
        public decimal TotalAmount { get; set; }
        public int SubscriptionId { get; set; }
    }

    public partial class AspNetUsersModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public DateTime? dtmAdd { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public int? Status { get; set; }
        public int? IsVerified { get; set; }
        public DateTime? dtmDob { get; set; }
        public string ProfileImage { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }  
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        
    }
}
