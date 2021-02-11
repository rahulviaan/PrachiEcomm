using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Models
{
    public class objSeries
    {
        public int Id
        {
            get;
            set;
        }

        [Display(Name = "Subject"), Required]
        public string Subject_Id
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? created_date
        {
            get;
            set;
        }

        [DisplayFormat(NullDisplayText = "N/A", DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? modified_date
        {
            get;
            set;
        }

        [Display(Name = "Title"), Required, StringLength(200, ErrorMessage = "The {0} not be exceed 200 char.")]
        public string title
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText), DisplayFormat(NullDisplayText = "N/A"), StringLength(400, ErrorMessage = "The {0} not be exceed 500 char.")]
        public string Description
        {
            get;
            set;
        }

        public string shortcut
        {
            get;
            set;
        }

        public IsValid is_active
        {
            get;
            set;
        }

        [Display(Name = "Image")]
        public string _image
        {
            get;
            set;
        }

        public int TotalBooks
        {
            get;
            set;
        }
    }
}