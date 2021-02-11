using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrachiIndia.Portal.Models
{
    public class Filters
    {
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public List<string> Board { get; set; }
        public List<string> Class { get; set; }
        public List<string> subject { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string Short { get; set; }
        public long id { get; set; }
        public int parentId { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Series { get; set; }
    }
    public class Short : Filters
    {
        public string Price { get; set; }
        public string Author { get; set; }
    }

    public class MyEbookLibrary
    {
        public long Id { get; set; }
        public string Board { get; set; }
        public string Classes { get; set; }
        public string Subect { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }

}
