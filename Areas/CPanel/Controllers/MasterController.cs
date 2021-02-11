using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class MasterController : Controller
    {
        // GET: CPanel/Master
        public ActionResult Index()
        {
            var results = new List<Sql.AspNetUser>();
            foreach (var item in new AspNetUserRepository().GetAll().ToList())
            {
                var roles = (from role in item.AspNetRoles
                             select role.Name).ToList();
                if (roles.Contains(Portal.Models.Roles.Sales) || roles.Contains(Portal.Models.Roles.Marketing))
                    results.Add(item);
            }
            return View(results);
        }

        #region new arrivals
        public ActionResult UpComming()
        {
            //var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            //var newArrivals = context.NewArrivals.Where(t => t.Status == true).ToList();
            //var bookIds = (from i in newArrivals
            //               select i.BookId).ToList();
            //var books = (from t in context.tblCataLogs.Where(t => t.Status == 1).ToList()
            //             where bookIds.Contains(t.Id)
            //             let newarr = newArrivals.First(x => x.BookId == t.Id)
            //             select new Models.NewArivalBook
            //             {
            //                 BookId = t.Id,
            //                 Title = t.Title,
            //                 CoverImage = t.Image,
            //                 Id = newarr.Id,
            //                 Stats = newarr.Status
            //             }).ToList();
            //return View(books);
            return View();

        }
        #endregion
    }
}