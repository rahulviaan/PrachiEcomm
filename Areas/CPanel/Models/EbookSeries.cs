using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class EbookSeries
    {
        public int Id { get; set; }
        public string Series { get; set; }
        public string Subject { get; set; }
        public string Board { get; set; }
        public bool Audio { get; set; }
        public bool LessonPlan { get; set; }
        public bool WorkSheet { get; set; }
        public bool WorkSheetSolution { get; set; }
        public bool BookSolution { get; set; }
        public bool TestHour { get; set; }
        public bool Status { get; set; }
        public bool SingleStar { get; set; }
        public bool DoubleStar { get; set; }
        public bool TestHourSolution { get; set; }
        public List<string> Boards { get; set; }

    }
}