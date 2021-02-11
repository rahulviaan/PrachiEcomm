using Microsoft.AspNet.Identity;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SubscriptionController : Controller
    {
        // GET: CPanel/Subscription
        #region Subscription Work Start here 
        public ActionResult subscription()
        {
            mst_subscription model = new mst_subscription();
            fillDropDownList();
            return View(model);
        }
        [HttpPost]
        public ActionResult subscription(mst_subscription model)
        {
            SubscriptionRepository objsubscription = new SubscriptionRepository();
            var objsubscriptions = new mst_subscription
            {
                PlanName = model.PlanName,
                PlanTime = model.PlanTime,
                StartDate = model.StartDate,
                Amount = model.Amount,
                SubscriptionType = model.SubscriptionType,
                ClassId = model.ClassId,
                BoardId = model.BoardId,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = User.Identity.GetUserId(),
                UpdateOn = DateTime.UtcNow,
                UpdatedBy = User.Identity.GetUserId(),
            };
            var result = objsubscription.CreateAsync(objsubscriptions);
            ModelState.Clear();
            fillDropDownList();
            return View(model);
        }
        private void fillDropDownList()
        {
            var list = GetAllClass();
            var Board = GetAllBoard();
            var SubscriptionType = GetSubscriptionType();
            ViewData["list"] = list;
            ViewData["Board"] = Board;
            ViewData["Subscription"] = SubscriptionType;
        }

        [AllowAnonymous]
        private object GetAllClass()
        {
            var ClassRepository = new MasterClassRepository();
            var ClassList = ClassRepository.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            foreach (var item in ClassList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.Title, Value = item.Id.ToString() });
            }
            return list;
        }
        [AllowAnonymous]
        public object GetAllBoard()
        {
            var MasterBoardRepo = new MasterBoardRepository();
            var MasterBoard = MasterBoardRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in MasterBoard)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.Title, Value = item.Id.ToString() });
            }
            return list;
        }
        public object GetSubscriptionType()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            //list.Add(new System.Web.Mvc.SelectListItem { Text = "Subscription Type", Value = "0" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Year", Value = "1" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Month", Value = "2" });
            return list;
        }
        public object GetSubscriptionYear()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            //list.Add(new System.Web.Mvc.SelectListItem { Text = "Subscription year", Value = "0" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "one Year", Value = "1" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Two Year", Value = "2" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Three Year", Value = "3" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Four Year", Value = "4" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Five Year", Value = "5" });
            return list;
        }
        public object GetSubscriptionMonth()
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            //list.Add(new System.Web.Mvc.SelectListItem { Text = "Subscription year", Value = "0" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "one Month", Value = "1" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Two Month", Value = "2" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Three Month", Value = "3" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Four Month", Value = "4" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Five Month", Value = "5" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Six Month", Value = "6" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Seven Month", Value = "7" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Eight Month", Value = "8" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Nine Month", Value = "9" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Ten Month", Value = "10" });
            list.Add(new System.Web.Mvc.SelectListItem { Text = "Eleve Month", Value = "11" });
            return list;
        }
        #endregion
    }
}