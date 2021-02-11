using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CompanyAddressController : Controller
    {
        // GET: CPanel/CompanyAddress
        #region Company Address Work  
        public ActionResult Index()
        {
            var list = classlist();
            ViewData["classlist"] = list;
            return View();
        }
        [HttpGet]
        public JsonResult getCompanyAddress()
        {
            var allCompanies = new CompanyAddressReposetory();
            IQueryable<tblCompanyAddress> Query = allCompanies.GetAll();
            var item = Query.ToList();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return Json(new
            {
                aaData = item
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult _AddCompanyAddress(tblCompanyAddress model)
        {
            var CompanyAddress = new tblCompanyAddress
            {
                StateName = model.StateName,
                H_No = model.H_No,
                Address = model.Address,
                NearBy = model.NearBy,
                City = model.City,
                Pincode = model.Pincode,
                mobileno = model.mobileno,
                Email = model.Email,
                IsMainOffice = model.IsMainOffice == null ? false : model.IsMainOffice,
                Color = model.Color
            };
            var companyrepo = new CompanyAddressReposetory();
            var result = companyrepo.CreateAsync(CompanyAddress);
            return Redirect("Index");
        }
        public ActionResult EditCompanyAddress(tblCompanyAddress model)
        {
            var list = classlist();
            ViewData["classlist"] = list;
            PartialView("_EditCompanyAddress", model);
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return Json(new
            {

            }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        //public ActionResult TestView()
        //{
        //    var strData = new DAL.DBUtility("").ReturnString();
        //    ViewBag.Message = strData;
        //    return View();
        //}
       
        private object classlist()
        {
            var list = new List<SelectListItem>
              {
                new SelectListItem{ Text="locationadd1", Value = "locationadd1" },
                new SelectListItem{ Text="locationadd2", Value = "locationadd2" },
                new SelectListItem{ Text="locationadd3", Value = "locationadd3" },
                new SelectListItem{ Text="locationadd4", Value = "locationadd4" },
                new SelectListItem{ Text="locationadd5", Value = "locationadd5" },
                new SelectListItem{ Text="locationadd6", Value = "locationadd6" },
                new SelectListItem{ Text="locationadd7", Value = "locationadd7" },
                 new SelectListItem{ Text="locationadd8", Value = "locationadd8" },
               new SelectListItem{ Text="class", Value = "0", Selected = true }
               };
            return list;
        }
        #endregion
        #region Books Order Work
        public ActionResult Books()
        {
            var Subject = (from item in new MasterSubjectRepository().Get().OrderBy(t => t.OredrNo).ToList()
                           select new SelectListItem
                           {
                               Text = item.Title,
                               Value = item.Id.ToString()
                           }).ToList();
            Subject.Insert(0, new SelectListItem { Text = "Please select", Value = "0" });
            ViewBag.Subject = Subject;
            return View();
        }
       
        public JsonResult GetBooks(string SubjectId = "0", string SeriesId = "0")
        {
            var orderrepo = new tblCataLog();
            var repo = new CatalogRepository();
            var Rsults = new List<tblCataLog>();
            IQueryable<tblCataLog> query = repo.GetAll();
            var subId = Utility.ToSafeInt(SubjectId);
            var SerId= Utility.ToSafeInt(SeriesId);
            if (SubjectId != "0")
            {
                query = query.Where(t => t.SubjectId== subId).OrderBy(t=>t.MasterSubject.OredrNo);
            }
            if (SeriesId != "0")
            {
                query = query.Where(t => t.SeriesId == SerId).OrderBy(t=>t.MasterSery.OredrNo);
            }
            foreach (var items in query.ToList())
            {
                    var apporder = new tblCataLog
                    {
                        Id      = items.Id,
                        Title   = items.Title,
                        Author  = items.Author!=null?items.Author:"Inhouse Author",
                        ISBN    = items.ISBN,
                        BoardId = items.BoardId,
                        ClassId = items.ClassId,
                        Image   = items.Image
                    };
                    Rsults.Add(apporder);
            }
            return Json(Rsults);
        }
        public JsonResult GetSeries(string SubjectId)
        {
            MasterSeriesRepositories obj = new MasterSeriesRepositories();
            IQueryable<MasterSery> query = obj.GetAll();
            var Rsults = new List<MasterSery>();
            var subject = Convert.ToInt64(SubjectId);
            query = query.Where(t => t.SubjectId == subject);
            var Series = query.ToList();
            foreach (var items in Series)
            {
                var apporder = new MasterSery
                {
                    Id = items.Id,
                    Title = items.Title,
                };
                Rsults.Add(apporder);
            }
            return Json(Rsults);
        }

        #endregion
    }

}

