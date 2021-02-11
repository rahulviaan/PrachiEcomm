using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using PrachiIndia.Sql;
using System.Drawing;

namespace PrachiIndia.Web.Areas.Model
{

    public class Books
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Please select your subject.")]
        [Display(Name = "Subject")]
        public long SubjectId { get; set; }
        [Required(ErrorMessage = "Please select your series.")]
        [Display(Name = "Series")]
        public long SeriesId { get; set; }
        [Required(ErrorMessage = "Please enter the title.")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string BookTitle { get; set; }
        [Required(ErrorMessage = "Please enter the author.")]
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? Ebook { get; set; }
        public int? MultiMedia { get; set; }
        public int? Solutions { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime? dtmAdd { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public DateTime? dtmDelete { get; set; }
        public int? Status { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public int orderno { get; set; }
        public int TaxId { get; set; }
        public HttpPostedFileBase PostedImage { get; set; }

        [Display(Name = "Board")]
        [Required]
        public string Board { get; set; }
        [Required(ErrorMessage = "Please choose the board.")]
        public List<string> Boards { get; set; }

        public List<CatalogBoard> CatalogBoard { get; set; }
        [Display(Name = "Class")]
        [Required(ErrorMessage = "Please choose the class.")]
        public string Class { get; set; }
        public List<GroupValues> Classes { get; set; }
        public List<CatalogClass> CatalogClass { get; set; }

        public long? idServer { get; set; }
        public string EncriptionKey { get; set; }
        public string ShortCut { get; set; }
        [Display(Name = "Subject")]
        public long? idSubject { get; set; }
        public int idBoard { get; set; }
        public int idClass { get; set; }
        [Required]
        [Display(Name = "Series")]
        public long? idSeries { get; set; }
        public string is_size { get; internal set; }

        public List<string> PostedClasses { get; set; }
        public long? EbookPrice { get; set; }
        public decimal? PrintPrice { get; set; }
        public int? PageCount { get; set; }
        public int? Colour { get; set; }
        public int? EbookSize_MB_ { get; set; }
        public bool? LessonPlan { get; set; }
        public bool? TestPaper { get; set; }
        public bool? TestPaperSolution { get; set; }
        public bool? Published { get; set; }
        public int? PublishedBy { get; set; }
        public DateTime? PublishedDate { get; set; }
    }

    public class GroupValues
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class GSTModel
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string HSNCode { get; set; }
        public decimal Rate { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}