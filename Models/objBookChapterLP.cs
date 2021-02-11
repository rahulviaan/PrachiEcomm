using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Models
{
    public class objBookChapterLP
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

        public int idChapter
        {
            get;
            set;
        }

        [Display(Name = "Chapter Name"), Required]
        public string isChName
        {
            get;
            set;
        }

        [Display(Name = "Chapter Name"), Required]
        public string imgChName
        {
            get;
            set;
        }

        [Display(Name = "No. of Periods"), Required]
        public string isPeriods
        {
            get;
            set;
        }

        [Display(Name = "Objectives"), Required]
        public string isObjective
        {
            get;
            set;
        }

        [Display(Name = "Learning Outcome"), Required]
        public string isLearOutcome
        {
            get;
            set;
        }

        [Display(Name = "Teaching Methodology"), Required]
        public string isTeaMethod
        {
            get;
            set;
        }

        [Display(Name = " Suggestive FA Activity"), Required]
        public string isSuggFAAct
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
    }
}