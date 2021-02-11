using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class StateVM
    {
        public string StateName { get; set; }
        public string StateId { get; set; }
        public string countryId { get; set; }
        public static IQueryable<StateVM> GetAllState()
        {
            return new List<StateVM>
            {
                new StateVM { StateName = "delhi",     StateId = "1",countryId="1" },
                new StateVM { StateName = "Up",     StateId = "2" ,countryId="1"},
                new StateVM { StateName = "Rajasthan",     StateId = "3" ,countryId="1"},
                new StateVM { StateName = "Punjab", StateId = "4" ,countryId="1"},
                new StateVM { StateName = "Jharkhand", StateId = "5" ,countryId="1"},
            }.AsQueryable();
        }
    }
}