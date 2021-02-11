using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PrachiIndia.Portal.Areas.TestHour.Models
{
    public class QuestionVM
    {

        public Int64 Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Remarks { get; set; }
        public Nullable<int> Status { get; set; }

    }
}