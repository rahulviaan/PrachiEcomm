using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class CityVm
    {
        public string CityName { get; set; }
        public string CityId { get; set; }
        public string StateId { get; set; }
        public static IQueryable<CityVm> GetAllCity()
        {
            return new List<CityVm>
            {
                new CityVm { CityName = "ajmer",      CityId = "1", StateId="3" },
                new CityVm { CityName = "Kota",         CityId = "2" ,StateId="3"},
                new CityVm { CityName = "jaipur",     CityId = "3" ,StateId="3"},
                new CityVm { CityName = "Ambala",     CityId = "4" ,StateId="4"},
                new CityVm { CityName = "Jalandhar",  CityId = "5" ,StateId="4"},
            }.AsQueryable();
        }
    }
    
   
}