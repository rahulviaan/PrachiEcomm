using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class NewArivalBook
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public bool Stats { get; set; }
    }
    public class BookSpecfication
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
        public bool Stats { get; set; }
    }
}