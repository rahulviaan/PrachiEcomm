using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Models
{
    public class objBook
    {
        public objBook() { }
        [Display(Name = "Audio")]
        public bool audio { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Author")]
        [StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string author { get; set; }
        [Display(Name = "Board")]
        public string board { get; set; }
        [Display(Name = "Board")]
        public string board_id { get; set; }
        public int book_id { get; set; }
        public int chatcount { get; set; }
        [Display(Name = "Class / Section")]
        public string class_id { get; set; }
        [Display(Name = "Create Date")]
        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? created_date { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string description { get; set; }
        public DateTime? dtm_Feedback { get; set; }
        [Display(Name = "e-Book")]
        public bool ebook { get; set; }
        [Display(Name = "Edition")]
        [StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string edition { get; set; }
        public string first_name { get; set; }
        public int Id { get; set; }
        public int idFeedback { get; set; }
        public int idReader { get; set; }
        public int idUser { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Book Cover Image")]
        [StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string image_path { get; set; }
        [Display(Name = "Extension")]
        public contentType img_extension { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "ISBN")]
        [StringLength(50, ErrorMessage = "The {0} not be exceed 50 char.")]
        public string isbn { get; set; }
        public string isChapter { get; set; }
        public string isEncKey { get; set; }
        public string isFeedback { get; set; }
        public IsValid is_active { get; set; }
        public string last_name { get; set; }
        [Display(Name = "Modified Date")]
        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? modified_date { get; set; }
        [Display(Name = "Multimedia")]
        public _enBinary multimedia { get; set; }
        [Display(Name = "No Of Pages")]
        [RegularExpression("^\\d{1,9}?$", ErrorMessage = "Enter Correct no of pages.")]
        public string no_of_pages { get; set; }
        [Display(Name = "Price")]
        [RegularExpression("^\\d{1,9}(\\.\\d{1,2})?$", ErrorMessage = "Enter Correct Price.")]
        public string price { get; set; }
        [Display(Name = "Series")]
        public string series { get; set; }
        [Display(Name = "Series")]
        [Required]
        public int series_id { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Shortcut")]
        [StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string shortcut { get; set; }
        public string sizeondisk { get; set; }
        [Display(Name = "Subject")]
        public string subject { get; set; }
        [Display(Name = "Subject")]
        [Required]
        public int subject_id { get; set; }
        [Display(Name = "Suppleymentary")]
        public _enBinary suppleymentary { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Book Title")]
        [Required]
        [StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string title { get; set; }
        [Display(Name = "Class / Section")]
        public string _class { get; set; }
        public string _Key { get; set; }
        [Display(Name = "Print")]
        public _enBinary _print { get; set; }
        public int ChapterID { get; set; }
        public int TopicID { get; set; }
    }
}
