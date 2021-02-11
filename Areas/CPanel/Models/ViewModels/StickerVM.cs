using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrachiIndia.Sql;

namespace PrachiIndia.Portal.Areas.CPanel.Models.ViewModels
{
    public class StickerVM : Sticker
    {
        public List<DVDMaster> lstDVD {get; set;}
    }
}