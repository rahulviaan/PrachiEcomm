using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PrachiIndia.Web.Areas.Model;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Models
{
    public class objQuestions
    {
        public int ID
        {
            get;
            set;
        }

        public int idSubject
        {
            get;
            set;
        }

        public int idSeries
        {
            get;
            set;
        }

        public int idChapter
        {
            get;
            set;
        }

        [Display(Name = "Book"), Required]
        public int idBook
        {
            get;
            set;
        }

        [Display(Name = "Type"), Required]
        public int idQuestionType
        {
            get;
            set;
        }

        [Display(Name = "Hot"), Required]
        public bool isHot
        {
            get;
            set;
        }

        public string isHeader
        {
            get;
            set;
        }

        [Display(Name = "Question"), Required]
        public string isQuestion
        {
            get;
            set;
        }

        [Display(Name = "Answer")]
        public string isAns
        {
            get;
            set;
        }

        [Display(Name = "Image")]
        public byte[] isImage
        {
            get;
            set;
        }
        public byte[] isAnsImage
        {
            get;
            set;
        }
        public string ImageBase64String { get; set; }
        public string AnsImageBase64String { get; set; }
        public string isExtension
        {
            get;
            set;
        }
        public string AnsExtension
        {
            get;
            set;
        }

        public bool QimgPriority { get; set; }
        public bool AnsimgPriority { get; set; }

        public IsValid isStatus
        {
            get;
            set;
        }

        public string Book
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Series
        {
            get;
            set;
        }

        public string Chapter
        {
            get;
            set;
        }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dtmCreate
        {
            get;
            set;
        }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dtmUpdate
        {
            get;
            set;
        }

        public List<objQueOptions> lstQueOptions
        {
            get;
            set;
        }
        public List<objQuestions> lstQuestions { get; set; }

        public int Category
        {
            get;
            set;
        }
        public int Topic
        {
            get;
            set;
        }

        public string TopicDesc
        {
            get;
            set;
        }
    }
}