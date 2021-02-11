using PagedList;
using PrachiIndia.Portal.Areas.CPanel.Models;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Portal;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class DashboardController : Controller
    {
        // GET: CPanel/Dashboard
        public ActionResult Index()
        {
            AspNetUserRepository User = (AspNetUserRepository)Factory.FactoryRepository.GetInstance(RepositoryType.AspNetUserRepository);
            OrderRepository Order = (OrderRepository)Factory.FactoryRepository.GetInstance(RepositoryType.OrderRepository);

            ViewBag.UserCount = User.GetAll().Where(x => !x.AspNetRoles.Any(r => r.Name == "Admin")).Count();
            ViewBag.OrdersCount = Order.GetAll().Count();
           
            return View();
        }
        public ActionResult ManageUser(int? page)
        {
            var pageindex = page ?? 1;
            int pageSize = 25;
            var states = (from item in new StateRepositories().Get().ToList()
                          select new SelectListItem
                          {
                              Text = item.StateName,
                              Value = item.StaeteId.ToString()
                          }).ToList();
            states.Insert(0, new SelectListItem { Text = "Please select", Value = "0" });
            ViewBag.Countries = states;
         
            var context = new dbPrachiIndia_PortalEntities();
            var xx = context.AspNetUserClaims.First();
            
            //var results=from user in context.AspNetUsers
            //            join xx in context.AspNetRoles
            //            join xxx in context.AspNetUserRoles
            var userList = context.AspNetUsers.OrderBy(t => t.FirstName).ToPagedList(pageindex, pageSize);

            return View(userList);
        }
        public ActionResult ManageOrder()
        {
            var states = (from item in new StateRepositories().Get().ToList()
                          select new SelectListItem
                          {
                              Text = item.StateName,
                              Value = item.StaeteId.ToString()
                          }).ToList();
            states.Insert(0, new SelectListItem { Text = "Please select", Value = "0" });
            ViewBag.Countries = states;
            return View();
        }

        public ActionResult CheckPaymentStatus(long Id)
        {
            const string WEBSERVICE_URL = "https://www.payumoney.com/payment/op/getPaymentResponse?";
            try
            {
                string myParameters = "merchantKey=W49jcI&merchantTransactionIds=PIPL-18-227";
                var url = WEBSERVICE_URL + myParameters;
                var webRequest = System.Net.WebRequest.Create(url);

                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.Timeout = 20000;
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Authorization", "cd9V9T6gY+OjTy8n1Uip2uHLfQSqCak9WruwKf1Tv7Q=");
                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //string URI = "https://test.payumoney.com/payment/payment/chkMerchantTxnStatus?";
            //string myParameters = "merchantKey=W49jcI&merchantTransactionIds=PIPL-18-227";
            //using (WebClient wc = new WebClient())
            //{
            //    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    wc.Headers.Add("Authorization", "cd9V9T6gY+OjTy8n1Uip2uHLfQSqCak9WruwKf1Tv7Q=");
            //    HtmlResult += wc.UploadString(URI, myParameters);
            //    var jss = new JavaScriptSerializer();
            //    var dict = jss.Deserialize<Dictionary<dynamic, dynamic>>(HtmlResult);
            //    Console.WriteLine(dict["message"]);
            //}
            return View();
        }
        public JsonResult GetOrders(string stateId = "0", string cityId = "0")
        {
            var orderrepo = new OrderRepository();
            var Rsults = new List<OrderDashboardVm>();

            var query = new OrderRepository().GetAll().OrderByDescending(t => t.Id).AsQueryable();
            if (stateId != "0")
            {
                query = query.Where(t => t.AspNetUser.State == stateId);
            }
            if (cityId != "0")
            {
                query = query.Where(t => t.AspNetUser.City == cityId);
            }

            foreach (var items in query.ToList())
            {
                if (items.IsSubscription != true)
                {
                    var apporder = new OrderDashboardVm
                    {

                        Id = items.Id.ToString(),
                        Name = items.Name,
                        Email = items.Email,
                        Phone = items.Phone,
                        UserId = items.UserId,
                        Amount = items.Amount.ToString(),
                        Status = items.Status.ToString(),
                        TransactionId = items.TransactionId,
                        CreatedDate = items.CreatedDate.ToString(),
                        // AspNetUser=items.AspNetUser
                    };
                    Rsults.Add(apporder);
                }

            }
            return Json(Rsults);
        }
        public JsonResult GetCities(string stateId)
        {
            var cities = new CityRepositories().Get().ToList().Where(t => t.StateId == Utility.ToSafeInt(stateId));
            return Json(cities);
        }

        public JsonResult GetUsers(string stateId = "0", string cityId = "0")
        {
            var results = new List<ApplicationUser>();
            var cities = new CityRepositories().Get().ToList();
            var states = new StateRepositories().Get().ToList();
            var countries = new CountryRepositories().Get().ToList();
            var userList = new AspNetUserRepository().GetAll().ToList();
            if (stateId != "0" && cityId != "0")
            {
                userList = userList.Where(t => t.City == cityId && t.State == stateId).ToList();
            }
            else if (stateId != "0")
            {
                userList = userList.Where(t => t.State == stateId).ToList();
            }

            foreach (var item in userList)
            {
                var roles = (from role in item.AspNetRoles
                             select role.Name).ToList();
                if (!roles.Contains(Portal.Models.Roles.Admin))
                {
                    var appUser = new ApplicationUser
                    {
                        AboutMe = item.AboutMe,
                        Address = item.Address,
                        City = item.City,
                        CompleteName = string.Format("{0} {1} {2}", item.FirstName, item.MiddleName, item.LastName),
                        Country = item.Country,
                        Designation = item.Designation,
                        DlNo = item.DlNo,
                        Email = item.Email,
                        dtmDob = item.dtmDob,
                        Organization = item.Organization,
                        PhoneNumber = item.PhoneNumber,
                        State = item.State,
                    };
                    results.Add(appUser);
                }
            }

            return Json(results);
        }
        public ActionResult GSTList()
        {
            var context = new dbPrachiIndia_PortalEntities();
            var taxlist = (from item in context.GSTTaxLists.ToList()
                           select new GSTModel
                           {
                               Description = item.Description,
                               HSNCode = item.HSNCode,
                               Rate = item.Rate,
                               Status = item.Status,
                               Id = item.Id
                           }).ToList();

            return View(taxlist);
        }
        public ActionResult ManageGST(long Id = 0)
        {
            var model = new GSTModel();
            var context = new dbPrachiIndia_PortalEntities();
            var gstModel = context.GSTTaxLists.FirstOrDefault(t => t.Id == Id);
            if (gstModel != null)
            {
                model.Id = gstModel.Id;
                model.Description = gstModel.Description;
                model.HSNCode = gstModel.HSNCode;
                model.Rate = gstModel.Rate;
                model.Status = gstModel.Status;
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult ManageGST(GSTModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var context = new dbPrachiIndia_PortalEntities();
            var gstModel = context.GSTTaxLists.FirstOrDefault(t => t.Id == model.Id);
            if (gstModel == null)
            {
                ModelState.AddModelError("HSNCode", "Record not exist for update");
                return View(model);
            }
            if (gstModel.Id > 0)
            {
                gstModel.Rate = model.Rate;
                gstModel.HSNCode = model.HSNCode;
                gstModel.Description = model.Description;
                gstModel.Status = model.Status;

                context.GSTTaxLists.Attach(gstModel);
                context.Entry(gstModel).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                gstModel = new GSTTaxList
                {
                    Rate = model.Rate,
                    HSNCode = model.HSNCode,
                    Description = model.Description,
                    Status = model.Status
                };
                context.GSTTaxLists.Add(gstModel);
                context.SaveChanges();
            }
            return RedirectToAction("GSTList");
        }
    }
}