using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Models
{
    public class objBookChapter
    {
        public int id
        {
            get;
            set;
        }

        public int idBook
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

        public objBook book
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

        [Display(Name = "Chapter Title"), Required]
        public string isTitle
        {
            get;
            set;
        }

        [Display(Name = "ChapterTitle"), Required, AllowHtml]
        public string imgTitle
        {
            get;
            set;
        }

        [Display(Name = "Description")]
        public string isDesc
        {
            get;
            set;
        }

        public IsValid isStatus
        {
            get;
            set;
        }

        [Display(Name = "Content Type")]
        public TextType isType
        {
            get;
            set;
        }

        [Display(Name = "Page From"), Required]
        public int isFromPage
        {
            get;
            set;
        }

        [Display(Name = "Page To"), Required]
        public int isToPage
        {
            get;
            set;
        }

        [Display(Name = "Chapter Index"), Required]
        public int isChapter
        {
            get;
            set;
        }

        public int countbp
        {
            get;
            set;
        }

        public int lp
        {
            get;
            set;
        }

        public int ws
        {
            get;
            set;
        }

        public int sl
        {
            get;
            set;
        }

        public int mm
        {
            get;
            set;
        }

        public int qu
        {
            get;
            set;
        }
    }
}