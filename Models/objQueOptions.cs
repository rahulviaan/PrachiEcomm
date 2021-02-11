using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Models
{
    public class objQueOptions
    {
        public int ID
        {
            get;
            set;
        }

        public int idQuestion
        {
            get;
            set;
        }

        [Display(Name = "Question"), Required]
        public string isOption
        {
            get;
            set;
        }

        [Display(Name = "Child Option"), Required]
        public string isChildOption
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
        public byte[] AnsImage
        {
            get;
            set;
        }
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

        public IsValid isStatus
        {
            get;
            set;
        }

        public string ImageBase64String { get; set; }
        public string AnsImageBase64String { get; set; }
    }
}