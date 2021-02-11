using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Models
{
    public class objQuestionCategory
    {
        public int ID
        {
            get;
            set;
        }

        [Display(Name = "Title"), Required, StringLength(500, ErrorMessage = "The {0} not be exceed 500 char.")]
        public string isTitle
        {
            get;
            set;
        }

        public IsValid isStatus
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

        [DataType(DataType.MultilineText), Display(Name = "Remarks"), DisplayFormat(NullDisplayText = "N/A"), StringLength(500, ErrorMessage = "The {0} not be exceed 500 char.")]
        public string isRemarks
        {
            get;
            set;
        }
    }
}