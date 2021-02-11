using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Controllers
{
    public class ExternalPaymentController : Controller
    {
        // GET: ExternalPayment
        public ActionResult PayNow()
        {
            var CountryRepo = new CountryRepositories();
            var CountryList = CountryRepo.GetAll().ToList();
            ViewBag.Country = new SelectList(CountryList, "countryId", "Name");
            return View();
        }
    }
}