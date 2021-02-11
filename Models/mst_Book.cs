using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class mst_Book
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int SeriesId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string BoardId { get; set; }
        public string ClassId { get; set; }
        public int Ebook { get; set; }
        public int MultiMedia { get; set; }
        public int Solutions { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> dtmAdd { get; set; }
        public Nullable<System.DateTime> dtmUpdate { get; set; }
        public Nullable<System.DateTime> dtmDelete { get; set; }
        public Nullable<int> Status { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public int OrderNo { get; set; }
        public int idServer { get; set; }
        public string EncriptionKey { get; set; }
        public string isSize { get; set; }


    }


    public class Master
    {
        public List<mst_Board> mstBoard { get; set; }    
        public List<mst_Book> mstBook { get; set; }
        public List<mst_Class> mstClass { get; set; }
        public List<mst_Subject> mstSubject { get; set; }
        public List<mst_Series> mstSeries { get; set; }
    }
}