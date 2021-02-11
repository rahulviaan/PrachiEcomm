using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class BookModel
    {
        public BookModel()
        {
            Chapters = new List<ChapterModel>();
        }
        public long Id { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }

        public string Series { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public decimal Price { get; set; }
        public int Ebook { get; set; }
        public int MultiMedia { get; set; }
        public int Solutions { get; set; }
        public List<ChapterModel> Chapters { get; set; }
    }

    public class ChapterModel
    {
        public long Id { get; set; }
        [Required]
        public long BookId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int OrderNo { get; set; }
        [Required]
        public string Title { get; set; }
        public string Descreption { get; set; }
        public Web.Areas.Model.TextType isType { get; set; }
        [Required]
        public int FromPage { get; set; }
        [Required]
        public int ToPage { get; set; }
        public Int16? Status { get; set; }
    }
    public class ChapterContentModel
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        [Required]
        public long ChapterId { get; set; }
        [Required(ErrorMessage = "Requried")]
        public long OrderNo { get; set; }
        [Required(ErrorMessage = "Requried")]
        public string Title { get; set; }

        public string Descreption { get; set; }
        public string MathInput { get; set; }
        [Required(ErrorMessage = "Requried")]
        public int Type { get; set; }

        public int ContentType { get; set; }
        public string FileName { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }
    }
}