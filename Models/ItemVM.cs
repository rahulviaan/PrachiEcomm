using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Sql;
using System.ComponentModel.DataAnnotations;

namespace PrachiIndia.Portal.Models
{
    public class ItemVM : tblCataLog
    {
        public int count { get; set; }
        
        public int Class_ID { get; set; }
        [AllowHtml]
        public string SeriesDescription { get; set; }

        public string itemprice { get; set; }
        public string ebookitemprice { get; set; }
        public string Class { get; internal set; }
        public string Youtubelink { get; internal set; }
        //public decimal itemprice
        //{
        //    get;
        //    set;
        //}

    }
}