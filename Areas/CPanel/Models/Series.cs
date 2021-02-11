using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrachiIndia.Web.Areas.Model
{
    public class SubjectModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Requried")]
        public string Title { get; set; }
        public int OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public bool Status { get; set; }
        public string IpAddress { get; set; }

    }
    public class SeriesModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public int OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public bool Status { get; set; }
        public string IpAddress { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public long SubjectId { get; set; }


        [Required]
        [Display(Name = "Dvd Type")]
        public int DVDType { get; set; }
        public string DVDName { get; set; }

    }
    public class ClassModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public int OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

    }
    public class BoardModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public int OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
    public class BookModel
    {
        public long Id { get; set; }
        public long BoardId { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long SeriesId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsEbook { get; set; }
        public bool IsMultiMedia { get; set; }
        public bool IsSolutions { get; set; }
        public bool IsWorkbook { get; set; }
        public bool IsVideo { get; set; }
        public bool IsLessonPlan { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string EpubPath { get; set; }
        public string EPubName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Status { get; set; }
        public int OrderNo { get; set; }
        public string EncriptionKey { get; set; }
        public string Size { get; set; }
    }
    public class UserLibraryModel : Portal.Models.BaseClass
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public long BookId { get; set; }
        public string EPubPath { get; set; }
        public string EpubName { get; set; }
        public string EncriptionKey { get; set; }
        public List<ChapterModel> Chapters { get; set; }
    }
    public class ChapterModel
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public int ChapterIndex { get; set; }
        public List<ChapterContentModel> ChapterContents { get; set; }
    }
    public class ChapterContentModel
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public long ChapterId { get; set; }
        public long Type { get; set; }
        public string Name { get; set; }
        public int ContentType { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Status { get; set; }
        public long OrderId { get; set; }
    }
    public class UserLogin
    {
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
    public class AspNetUserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }
        public int Status { get; set; }
        public int IsVerified { get; set; }
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
        public string PANId { get; set; }
        public string PassportNo { get; set; }
        public string DlNo { get; set; }
        public string Remark { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

    }

    public class Questions {

        public long Id { get; set; }
        public long BookId { get; set; }
        public long ChapterId { get; set; }
        public long CategoryId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Image { get; set; }
        public string Extension { get; set; }
        public string AnsImage { get; set; }
        public string AnsExtension { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Type { get; set; }
        public int Topic { get; set; }
    }

    public class Answers {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string Option { get; set; }
        public string ChildOption { get; set; }
        public string Ans { get; set; }
        public string Image { get; set; }
        public string Extension { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Status { get; set; }
        public string AnsImage { get; set; }
        public string AnsExtension { get; set; }
    }

    public class Topics {
        public int Id { get; set; }

        public long BookId { get; set; }
        public long ChapterId { get; set; }
        public string Topic { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }

    }

}