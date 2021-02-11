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
}
