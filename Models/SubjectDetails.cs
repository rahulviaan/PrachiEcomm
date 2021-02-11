using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class SubjectDetails
    {
        public string  Board { get; set; }
        public string  Subject { get; set; }
        public string  Series { get; set; }
        public List<string> Classes { get; set; }
    }

    public class SampleSubjectDetails
    {
        public string Board { get; set; }
        public string Subject { get; set; }
        public string Series { get; set; }
        public int Quantity { get; set; }
        public List<string> Classes { get; set; }
    }
}