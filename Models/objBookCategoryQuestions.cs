using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PrachiIndia.Portal.Models
{
    public class objBookCategoryQuestions
    {
        public int Id
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

        public string isBook
        {
            get;
            set;
        }

        public string isCategory
        {
            get;
            set;
        }

        public int isTotalQuestion
        {
            get;
            set;
        }
    }
}