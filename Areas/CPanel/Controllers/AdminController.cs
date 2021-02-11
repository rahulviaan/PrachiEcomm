using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using PrachiIndia.Sql.CustomRepositories;
//using PrachiIndia.Web.Areas.Model;
using System.Threading.Tasks;
using PrachiIndia.Portal.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Globalization;
using PrachiIndia.Sql;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System.IO;
using PrachiIndia.Portal.Areas.CPanel.Models;
using System.Data;
using System.Data.Entity;
using PrachiIndia.Portal.Factory;
using System.Data.Entity.Core.Objects;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{

    [Authorize(Roles = "Admin,SuperAdmin")]
    //[Authorize]
    [ValidateInput(false)]
    public class AdminController : Controller
    {
        // GET: CPanel/Admin
        [AllowAnonymous]
        public async Task Test()
        {
            // string value = "10247,10248,10249,10250,10251,10254,10255,10256,10257,10259,10260,10261,10262,10263,10264,10266,10267,10268,10285,10286,10287,10288,10289,10290,10291,10292";
            var lstBook = new Readedge_BusLogic.blBook().GetAll();
            var objBook = new tblCataLog();

            foreach (var item in lstBook)
            {
                objBook = new tblCataLog
                {
                    idServer = item.Id,
                    isSize = item.is_size,
                    EncriptionKey = item.EncriptionKey,
                };
                new CatalogRepository().UpdateByIdServer(objBook);
            }
        }

        string ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
        #region Here All Order work start here For Addmin by deepak 
        public ActionResult Index()
        {
            var orderProductRepository = new OrderProductRepository();
            var itemRepository = new CatalogRepository();
            var OrderRepository = new OrderRepository();
            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
            var fromdatess = DateTime.Now.AddDays(-7);
            var fromdates = fromdatess.Date;

            var todatess = DateTime.Now;
            var todates = todatess.Date.AddDays(1);
            query = query.Where(x => x.CreatedDate >= fromdates && x.CreatedDate <= todates);
            var result = query.OrderByDescending(t => t.Id).DistinctBy(x => x.Id).ToList();
            var items = (from item in result
                         select new OrderTrackVm
                         {
                             OrderId = item.Order.Id.ToString() ?? "",
                             TransactionId = item.Order.TransactionId,
                             date = item.Order.UpdatedDate.ToString(),
                             price = item.Order.Amount ?? 0,
                             status = item.Order.Status != 1 ? false : true,
                             Dispatchby = item.Order.dispatchby,
                             AWBNO = item.Order.AWBNO,
                             IsRecive = item.Order.IsRecive ?? false,
                             IsProduct = item.Order.IsInventory.ToString() ?? "false",
                             InventoryMessage = item.Order.InventoryMessage,
                             fromdate = DateTime.Now.AddDays(-7),
                             todate = DateTime.Now
                         }).DistinctBy(x => x.OrderId).ToList();
            // var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            if (items.Count == 0)
            {
                items = new List<OrderTrackVm>
                        {
                           new OrderTrackVm
                           {
                            fromdate = DateTime.Now.AddDays(-7),
                            todate = DateTime.Now
                           }
                        };
            }

            return View(items);
        }
        [HttpPost]
        public ActionResult Index(OrderTrackVm model)
        {
            var orderProductRepository = new OrderProductRepository();
            var itemRepository = new CatalogRepository();
            var OrderRepository = new OrderRepository();
            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
            //Added by Rahul Srivastava date time issue.
            model.todate = model.todate.AddDays(1);
            if (model.fromdate != null && model.todate != null)
            {
                query = query.Where(t => t.UpdatedDate >= model.fromdate && t.UpdatedDate <= model.todate);
            }
            else if (model.fromdate != null)
            {
                query = query.Where(t => t.Order.CreatedDate >= model.fromdate);
            }
            else if (model.todate != null)
            {
                query = query.Where(t => t.Order.CreatedDate <= model.todate);
            }
            if (model.TranId == "1")
            {
                query = query.Where(t => t.Order.Status == 0);
            }
            else if (model.TranId == "2")
            {
                query = query.Where(t => t.Order.Status == 1);
            }
            var result = query.OrderByDescending(t => t.Id).DistinctBy(x => x.Id).ToList();
            var items = (from item in result
                         select new OrderTrackVm
                         {
                             OrderId = item.Order.Id.ToString() ?? "",
                             TransactionId = item.Order.TransactionId,
                             date = item.Order.UpdatedDate.ToString(),
                             price = item.Order.Amount ?? 0,
                             status = item.Order.Status != 1 ? false : true,
                             Dispatchby = item.Order.dispatchby,
                             AWBNO = item.Order.AWBNO,
                             IsRecive = item.Order.IsRecive ?? false,
                             IsProduct = item.Order.IsInventory.ToString() ?? "false",
                             InventoryMessage = item.Order.InventoryMessage,
                             fromdate = model.fromdate,
                             todate = model.todate
                         }).DistinctBy(x => x.OrderId).ToList();
            if (items.Count == 0)
            {
                items = new List<OrderTrackVm>
                        {
                           new OrderTrackVm
                           {
                            fromdate = DateTime.Now.AddDays(-7),
                            todate = DateTime.Now
                           }
                        };
            }

            return View(items);
        }

        public ActionResult OrderDetail(int OrderId)
        {
            var orderProductRepository = new OrderProductRepository();
            var orderReposetory = new OrderRepository();
            var order = orderReposetory.FindByIdAsync(OrderId);
            if (order == null || string.IsNullOrWhiteSpace(order.UserId))
            {
                return RedirectToAction("Index");
            }
            var customer = new AspNetUserRepository().GetUser(order.UserId);
            var context = new dbPrachiIndia_PortalEntities();

            var results = orderProductRepository.SearchFor(t => t.OrderId == OrderId).ToList();
            var items = new List<OrderTrackVm>();
            foreach (var result in results)
            {
                var tax = context.GSTTaxLists.FirstOrDefault(t => t.Id == result.TaxId);
                var description = tax == null ? string.Empty : tax.Description;
                var item = new OrderTrackVm
                {
                    TransactionId = result.Order.TransactionId,
                    Title = result.Title,
                    OrderId = result.OrderId.ToString(),
                    date = result.UpdatedDate.ToString(),
                    Image = ImageUrl + ((result.tblCataLog.Image != null) ? result.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
                    price = result.Price ?? 0,
                    Quantity = result.Quantity ?? 0,
                    Discount = result.Discount ?? 0,
                    status = result.Order.Status != 1 ? false : true,
                    classs = classes(result.ItemId),
                    Board = GetBoards(result.ItemId),
                    Book = result.BookType ?? 0,
                    TaxPrice = result.Tax,
                    TotalAmount = result.TotalAmount ?? 0,
                    Remark = description
                };
                items.Add(item);
            }
            ViewBag.Customer = customer;
            return View(items);
        }

        public string classes(long? itemid)
        {

            //var Catalogue = itemRepository.GetAll();
            // int ClassID = Convert.ToInt32(Classid);
            var context = new dbPrachiIndia_PortalEntities();
            var ClassID = context.tblCataLogs.Where(x => x.Id == itemid).Select(y => new { ClassID = y.ClassId }).ToList().First();
            int CID = Convert.ToInt32(ClassID.ClassID);
            var Class = context.MasterClasses.Where(x => x.Id == CID).Select(x => new { Class = x.Title }).ToList().First();
            return Class.Class;
        }

        public string GetBoards(long? itemid)
        {
            //int BoardID = Convert.ToInt32(Boarid);

            //int BoardID = Convert.ToInt32(Boarid);
            var context = new dbPrachiIndia_PortalEntities();
            var BoardID = context.tblCataLogs.Where(x => x.Id == itemid).Select(y => new { BoardID = y.BoardId }).ToList().First();
            int BID = Convert.ToInt32(BoardID.BoardID);
            var Board = context.MasterBoards.Where(x => x.Id == BID).Select(x => new { Board = x.Title }).ToList().First();
            return Board.Board;
        }




        #endregion

        //partial page for dispatch popup
        [ChildActionOnly]
        public PartialViewResult _Dispatch(OrderTrackVm model)
        {
            return PartialView(model);
        }
        //partial page for ReciveOrderTrack popup
        [ChildActionOnly]
        public PartialViewResult _ReciveOrderTrack(OrderTrackVm model)
        {
            return PartialView(model);
        }
        //partial page for CheckProductInventory popup
        public PartialViewResult _CheckProductInventory(OrderTrackVm model)
        {
            return PartialView(model);
        }
        //here we check inventory manualy and update to system item is there or not 
        public ActionResult UpdateInventoryStatus(OrderTrackVm model)
        {
            var OrderRepository = new OrderRepository();
            var OrdertrackRepository = new OrderTrackRepositories();
            var item = new Order
            {
                Id = Convert.ToInt32(model.OrderId),
                IsInventory = model.IsProduct == "1" ? true : false,
                InventoryMessage = model.InventoryMessage,
            };
            var result = OrderRepository.UpdateInventoryStatus(item);
            return RedirectToAction("index");
        }
        //this method use for reflect all changes at order page .
        public ActionResult UpdateOrderTrack()
        {
            var Id = Convert.ToInt64(Request.QueryString["OrderId"]);
            var orderProductRepository = new OrderProductRepository();
            var orderReposetory = new OrderRepository();
            var itemRepository = new CatalogRepository();
            var AspNetUser = new AspNetUserRepository();
            AspNetUser objuser = new Sql.AspNetUser();
            var customer = objuser;
            var Userid = orderReposetory.GetAll().Where(x => x.Id == Id).ToList();
            foreach (var item in Userid)
            {
                customer = new AspNetUserRepository().GetUser(item.UserId);
            }
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            var Cid = Convert.ToInt32(customer.Country != null ? customer.Country : "0");
            var Sid = Convert.ToInt32(customer.State != null ? customer.State : "0");
            var CiId = Convert.ToInt32(customer.City != null ? customer.City : "0");
            var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
            var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
            var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
            var customers = new AspNetUser
            {
                FirstName = customer.FirstName,
                Email = customer.Email,
                Address = customer.Address,
                Country = countryid,
                City = CityId,
                State = StateId,
                PinCode = customer.PinCode,
            };
            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
            var result = query.Where(x => x.OrderId == Id).ToList();
            var items = (from item in result
                         select new OrderTrackUpdateVm
                         {
                             TransactionId = item.Order.TransactionId,
                             Title = item.Title,
                             OrderId = item.OrderId.ToString(),
                             date = item.CreatedDate.ToString(),
                             Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
                             price = string.Format("{0:0.00}", item.Price),
                             Quantity = item.Quantity.ToString(),
                             Discount = string.Format("{0:0.00}", item.Discount),
                             status = item.Order.Status != 1 ? false : true,
                             BookType = item.BookType.ToString(),
                             AWBNO = item.Order.AWBNO,
                             Dispatchby = item.Order.dispatchby
                         }).ToList();
            ViewBag.Customer = customers;
            return View(items);
        }
        #region Order Track Work Start Here By Deepak
        [HttpPost]
        public ActionResult UpdateOrderTrack(OrderTrackUpdateVm model)
        {
            var OrderRepository = new OrderRepository();
            var OrdertrackRepository = new OrderTrackRepositories();
            var item = new Order
            {
                Id = Convert.ToInt32(model.OrderId),
                OrderReciveBy = model.OrderRecivedBy,
                IsRecive = true,
                ReciveDate = model.ReciveDate,
            };
            var result = OrderRepository.UpdateOrderRecive(item);
            return RedirectToAction("index");
        }
        #endregion
        #region Disptach Order From User Side To user Start Here by Deepak
        [HttpPost]
        public ActionResult dispatchOrder(OrderTrackVm model)
        {

            var OrderId = Convert.ToInt64(model.OrderId);
            var orderProductRepository = new OrderProductRepository();
            var orderReposetory = new OrderRepository();
            var itemRepository = new CatalogRepository();
            var AspNetUser = new AspNetUserRepository();
            AspNetUser objuser = new Sql.AspNetUser();
            var customer = objuser;
            var Userid = orderReposetory.GetAll().Where(x => x.Id == OrderId).ToList();
            foreach (var item in Userid)
            {
                customer = new AspNetUserRepository().GetUser(item.UserId);
            }
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            OrderTrackRepositories trackrepository = new OrderTrackRepositories();
            //var Cid = Convert.ToInt32(customer.Country != null ? customer.Country : "0");
            //var Sid = Convert.ToInt32(customer.State != null ? customer.State : "0");
            //var CiId = Convert.ToInt32(customer.City != null ? customer.City : "0");
            //var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
            //var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
            //var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
            var customers = new AspNetUser
            {
                FirstName = customer.FirstName,
                Email = customer.Email,
                Address = customer.Address,
                Country = customer.Country,
                City = customer.City,
                State = customer.State,
                PinCode = customer.PinCode,
                PhoneNumber = customer.PhoneNumber,
                Id = customer.Id
            };
            customers.Email = "rahul9179@gmail.com";
            var orders = new Order
            {
                Id = Convert.ToInt64(model.OrderId),
                AWBNO = model.AWBNO,
                dispatchby = model.DispatchedBy
            };
            var OrderUdate = orderReposetory.Update(orders);
            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
            if (OrderUdate)
            {
                var result = query.Where(x => x.OrderId == OrderId).ToList();
                var objordertrack = (from itemss in result

                                     select new OrderTrack
                                     {
                                         TransactionId = itemss.Order.TransactionId,
                                         OrderId = Convert.ToInt32(itemss.OrderId),
                                         Amount = itemss.Order.Amount,
                                         Status = "Dispatched",
                                         UserBillingAddress = customers.FirstName + ',' + customers.Address + ',' + customers.Email + ',' + customers.PhoneNumber + ',' + customers.Country + ',' + customers.State + ',' + customers.City + ',' + customers.PinCode,
                                         UserId = itemss.Order.AspNetUser.Id,//item.Order.UserId,
                                         ItemDestination = null,
                                     }).ToList().DistinctBy(t => t.OrderId);
                var status = trackrepository.Create(objordertrack);
                var response = (from itemss in result
                                select new Models.DispatchOrderResponse
                                {
                                    TransactionId = itemss.Order.TransactionId,
                                    Title = itemss.Title,
                                    Price = String.Format("{0:0.00}", itemss.Price),
                                    Quantity = Convert.ToInt32(itemss.Quantity),
                                    TotalAmount = String.Format("{0:0.00}", itemss.Order.Amount),
                                    Discount = Convert.ToDecimal(itemss.Discount),
                                    AWBNO = model.AWBNO,
                                    DispatchedBy = model.DispatchedBy,
                                }).ToList();

                Oredermail(customers, response);
            }
            return RedirectToAction("index");
        }
        #endregion
        #region Send Email To User After Dipatch Order by Deepak:12:01:2016
        public void Oredermail(AspNetUser customer, List<Models.DispatchOrderResponse> responseObject)
        {
            var toMail = ConfigurationManager.AppSettings["MailTo"].ToString(CultureInfo.InvariantCulture);
            var subject = "";
            foreach (var item in responseObject)
            {
                subject = "Shipment of items in order " + item.TransactionId + "  by PrachiIndia.com";
                break;
            }
            var message = MailTemplate(customer, responseObject);
            MailServiece.Mail.SendMail(customer.Email, "", toMail, subject, message);
            //  var x = Portal.Framework.Utility.SendMail(subject, message, customer.Email, toMail);
        }
        static string MailTemplate(AspNetUser customer, List<Models.DispatchOrderResponse> responseObject)
        {
            var textMessage = string.Empty;
            textMessage += "<table border='0' cellpadding='0' cellspacing='0' width='720' align='center' style='font-family: verdana; font-size: 12px; line-height: 18px; border: solid 1px #f1f1f1;' >";
            textMessage += "<tr>";
            textMessage += " <td style='padding: 10px; width: 700px;'>";
            textMessage += "<table border='0' cellpadding='0' cellspacing='0' width='100%'>";
            textMessage += "  <tr>";
            textMessage += "  <td align='left'>";

            foreach (var item in responseObject)
            {
                textMessage += "   <h2>your order has been placed " + item.DispatchedBy + "</h2>";
                textMessage += "  <h4>Order No. " + item.TransactionId + "  <br />Date :   " + DateTime.Now.ToString("dd-MM-yyyy") + " <br/>The shipment was sent through: " + item.DispatchedBy + " <br/> Shipment Tracking ID:" + item.AWBNO + " </h4>";
                break;
            }
            textMessage += "</td>";
            textMessage += " <td align='right'>";
            textMessage += "<img src='http://prachiindia.com/img/logos/logo.png' width='200' alt='logo' />";
            textMessage += "   </td>";
            textMessage += "</tr>";
            textMessage += " <tr>";
            textMessage += "<td align='left' colspan='2'>";
            textMessage += " <h4 style='text-decoration:underline;'>Billing/Shipping Address</h4>";
            textMessage += "<p>Name: " + customer.FirstName + "<br/>Address: " + customer.Address + "<br /> City: " + customer.City + "<br /> State: " + customer.State + "<br /> Country: " + customer.Country + "<br /> Pincode: " + customer.PinCode + "<br/>Mobile: " + customer.PhoneNumber;
            textMessage += " </p>";
            textMessage += " </td>";
            textMessage += " </tr>";
            textMessage += "</table><br/><br/>";
            textMessage += " <table cellpadding='0' cellspacing='0' width='100%' style='border: solid 1px #010101;'>";
            textMessage += "  <thead>";
            textMessage += "  <tr>";

            textMessage += "  <th align='left' style='padding: 4px;'>Name</th>";
            textMessage += "  <th align='left' style='padding: 4px;'>MRP Price</th>";
            textMessage += "  <th align='left' style='padding: 4px;'>Quantity</th>";
            textMessage += "  <th align='left' style='padding: 4px;'>Discount</th>";
            textMessage += "  <th align='right' style='padding: 4px;'>Subtotal</th>";
            textMessage += "  </tr>";
            textMessage += "  </thead>";
            textMessage += " <tbody>";
            if (responseObject != null && responseObject.Any())
            {
                decimal sum = 0;
                decimal totalDiscount = 0;
                foreach (var itemcart in responseObject)
                {
                    var discount = itemcart.Discount * itemcart.Quantity;
                    var price = Convert.ToDecimal(itemcart.Price) * itemcart.Quantity;
                    var subTotal = price - discount;
                    totalDiscount = totalDiscount + discount;
                    sum = sum + subTotal;
                    textMessage += "  <tr>";
                    textMessage += "  <td style='padding: 4px;' > " + itemcart.Title + "</td>";
                    textMessage += " <td style='padding: 4px;'  > Rs " + itemcart.Price + "</td>";
                    textMessage += "  <td style='padding: 4px;' > " + itemcart.Quantity + "</td>";
                    textMessage += " <td style='padding: 4px;'  > Rs " + discount + "</td>";
                    textMessage += "  <td style='padding: 4px;' align='right' > Rs " + subTotal + "</td>";
                    textMessage += " </tr>";

                }
                textMessage += "   <tr>";
                textMessage += "   <td style='padding: 4px;' colspan='3' align='right'>Total Discount:</td>";
                textMessage += "    <td style='padding: 4px;' align='right'> Rs " + totalDiscount + "</td>";
                textMessage += "  </tr>";
                textMessage += "   <tr>";
                textMessage += "   <td style='padding: 4px;' colspan='3' align='right'>Total Price:</td>";
                textMessage += "    <td style='padding: 4px;' align='right'> Rs " + sum + "</td>";
                textMessage += "  </tr>";

            }
            textMessage += "  </tbody>";
            textMessage += "  </table><br/><br/><br/>";
            textMessage += " <table border='0' cellpadding='0' cellspacing='0' width='100%'>";
            textMessage += "  <tr>";
            textMessage += " <td align='left'>";
            textMessage += " <p>Company: <a href='http://prachiindia.com/'>Prach [India] Pvt. Ltd.</a><br />";
            textMessage += "  Website: <a href='http://prachiindia.com/'>www.prachiindia.com</a><br />";
            textMessage += " Email: <a href='mailto:contact@goclabs.com'>info@prachiindia.com</a><br />";
            textMessage += " Address: 309/10, ALLIED HOUSE,<br />";
            textMessage += "  INDER LOK,DELHI-110035<br />";
            textMessage += "  Contact no: +91-11-47320666(8 Lines)";
            textMessage += "  </p>";
            textMessage += " </td>";
            textMessage += "  </tr>";
            textMessage += " </table>";
            textMessage += "  </td>";
            textMessage += "   </tr>";
            textMessage += " </table>";
            return textMessage;
        }
        #endregion
        #region "Comman"

        [AllowAnonymous]
        public async Task<ActionResult> SelectPartialView(string uc, int id, int idSubject = 0, int idSeries = 0)
        {
            if (uc == "ucSubject")
            {
                var objSubject = new Web.Areas.Model.MasterSubject();
                if (id > 0)
                {
                    var SubjectRepository = new MasterSubjectRepository();
                    var result = SubjectRepository.FindByIdAsync(id);

                    if (result != null)
                    {
                        objSubject = new Web.Areas.Model.MasterSubject
                        {
                            Id = result.Id,
                            Title = result.Title,
                            Description = result.Description,
                            OredrNo = result.OredrNo,
                            Image = result.Image,
                            IdServer = Common_Static.ToSafeInt(result.idServer),
                            IdSubject = Common_Static.ToSafeInt(result.idServer)
                        };

                    }
                }
                return View("ucSubject", objSubject);
            }
            else if (uc == "ucSeries")
            {
                var objSubject = new SeriesModel();
                if (id > 0)
                {
                    var SeriesRepository = new MasterSeriesRepositories();
                    var result = SeriesRepository.FindByIdAsync(id);

                    if (result != null)
                    {
                        objSubject = new SeriesModel
                        {
                            Id = result.Id,
                            Title = result.Title,
                            // SubjectId = result.SubjectId,
                            Description = result.Description,
                            // OredrNo = result.OredrNo,
                            Image = result.Image,
                            // IdServer = Common_Static.ToSafeInt(result.idServer),
                        };

                    }
                }
                return View("ucSeries", objSubject);
            }
            else if (uc == "ucClass")
            {
                var objClass = new MasterClasses();
                if (id > 0)
                {
                    var SeriesRepository = new MasterClassRepository();
                    var result = SeriesRepository.FindByIdAsync(id);

                    if (result != null)
                    {
                        objClass = new MasterClasses
                        {
                            Id = result.Id,
                            Title = result.Title,
                            Description = result.Description,
                            OredrNo = result.OredrNo,
                            Image = result.Image,
                            IdServer = Common_Static.ToSafeInt(result.idServer),
                        };

                    }
                }
                return View("ucClass", objClass);
            }
            else if (uc == "ucBoards")
            {
                var objBoards = new MasterBoards();
                if (id > 0)
                {
                    var SeriesRepository = new MasterBoardRepository();
                    var result = SeriesRepository.FindByIdAsync(id);

                    if (result != null)
                    {
                        objBoards = new MasterBoards
                        {
                            Id = result.Id,
                            Title = result.Title,
                            Description = result.Description,
                            OredrNo = result.OredrNo,
                            Image = result.Image,
                            IdServer = Common_Static.ToSafeInt(result.idServer),

                        };

                    }
                }
                return View("ucBoards", objBoards);
            }
            else if (uc == "ucBooks")
            {

                MasterSubjectRepository MasterSubjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSubjectrepository);
                MasterSeriesRepositories MasterSeriesRepositories = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
                MasterClassRepository MasterClassRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
                MasterBoardRepository MasterBoardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
                CatalogRepository CatalogRepository = (CatalogRepository)Factory.FactoryRepository.GetInstance(RepositoryType.CatalogRepository);
                GSTTaxListRepository gSTTaxListRepository = (GSTTaxListRepository)Factory.FactoryRepository.GetInstance(RepositoryType.GSTTaxListRepository);
                Task<tblCataLog> T = Task.Run(() => (CatalogRepository.FindByIdAsync(id)));
                ViewBag.lstSubject = new SelectList(await Task.Run(() => (MasterSubjectRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
                ViewBag.lstSeries = new SelectList(await Task.Run(() => (MasterSeriesRepositories.GetAll().Where(x => x.SubjectId == idSubject && x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
                // Title FindByIdAsync
                ViewBag.lstTitle = new List<SelectListItem>() { new SelectListItem { Text = "-- Select --", Value = "" } };
                //class
                ViewBag.lstCalss = new SelectList(await Task.Run(() => (MasterClassRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");

                //Board
                ViewBag.lstBoard = new SelectList(await Task.Run(() => (MasterBoardRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
                ViewBag.Tax = new SelectList(await Task.Run(() => (gSTTaxListRepository.GetAll().Where(x => x.Status == true).Select(x => new { Id = x.Id, Name = x.Description })).ToList()), "Id", "Name");
                var objBooks = new Books();
                if (id > 0)
                {
                    tblCataLog result = await T;
                    if (result != null)
                    {
                        objBooks = new Books()
                        {
                            Id = result.Id,
                            SubjectId = result.SubjectId ?? 0,
                            SeriesId = result.SeriesId ?? 0,
                            Title = result.Title,
                            Author = result.Author,
                            ISBN = result.ISBN,
                            Edition = result.Edition,
                            Price = result.Price,
                            Discount = result.Discount,
                            Ebook = result.Ebook,
                            MultiMedia = result.MultiMedia,
                            Solutions = result.Solutions,
                            Image = result.Image,
                            Description = result.Description,
                            dtmAdd = result.dtmAdd,
                            dtmUpdate = result.dtmUpdate,
                            dtmDelete = result.dtmDelete,
                            Status = result.Status,
                            IpAddress = result.IpAddress,
                            orderno = result.OrderNo ?? 0,
                            TaxId = result.Tax
                        };
                    }
                    else
                    {
                        objBooks.SubjectId = idSubject;
                        objBooks.SeriesId = idSeries;
                    }
                }
                else
                {
                    objBooks.SubjectId = idSubject;
                    objBooks.SeriesId = idSeries;
                }
                return View("ucBooks", objBooks);
            }
            return View("Index", id);
        }

        [AllowAnonymous]
        public async Task<ActionResult> SelectPartialView_Update(string uc, int id, int idSubject = 0, int idSeries = 0)
        {
            Books objBooks = (Books)Factory.Factory.GetInstance(RepositoryType.Books);
            if (id > 0)
            {
                MasterSubjectRepository MasterSubjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSubjectrepository);
                MasterSeriesRepositories MasterSeriesRepositories = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
                MasterClassRepository MasterClassRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
                MasterBoardRepository MasterBoardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
                CatalogRepository CatalogRepository = (CatalogRepository)Factory.FactoryRepository.GetInstance(RepositoryType.CatalogRepository);
                GSTTaxListRepository gSTTaxListRepository = (GSTTaxListRepository)Factory.FactoryRepository.GetInstance(RepositoryType.GSTTaxListRepository);
                Task<tblCataLog> T = Task.Run(() => (CatalogRepository.FindByIdAsync(id)));

                var result = CatalogRepository.FindByIdAsync(id);
                ViewBag.lstSubject = new SelectList(await Task.Run(() => (MasterSubjectRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
                ViewBag.lstSeries = new SelectList(await Task.Run(() => (MasterSeriesRepositories.GetAll().Where(x => x.SubjectId == result.SubjectId && x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name", result.SeriesId);
                // Title FindByIdAsync
                ViewBag.lstTitle = new List<SelectListItem>() { new SelectListItem { Text = "-- Select --", Value = "" } };
                //Board
                ViewBag.lstBoard = new SelectList(await Task.Run(() => (MasterBoardRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name", result.BoardId);
                //class
                ViewBag.lstCalss = new SelectList(await Task.Run(() => (MasterClassRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name", result.ClassId);

                ViewBag.Tax = new SelectList(await Task.Run(() => (gSTTaxListRepository.GetAll().Where(x => x.Status == true).Select(x => new { Id = x.Id, Name = x.Description })).ToList()), "Id", "Name");

                if (result != null)
                {
                    objBooks = new Books()
                    {
                        Id = result.Id,
                        SubjectId = result.SubjectId ?? 0,
                        SeriesId = result.SeriesId ?? 0,
                        Title = result.Title,
                        Author = result.Author,
                        ISBN = result.ISBN,
                        Edition = result.Edition,
                        TaxId = result.Tax,
                        Price = result.Price,
                        Discount = result.Discount,
                        Ebook = result.Ebook,
                        MultiMedia = result.MultiMedia,
                        Solutions = result.Solutions,
                        Image = result.Image,
                        Description = result.Description,
                        dtmAdd = result.dtmAdd,
                        dtmUpdate = result.dtmUpdate,
                        dtmDelete = result.dtmDelete,
                        Status = result.Status,
                        IpAddress = result.IpAddress,
                        orderno = result.OrderNo ?? 0,
                        EbookPrice = Convert.ToInt64(result.EbookPrice),
                        PrintPrice = result.PrintPrice,
                        PageCount = result.PageCount,
                        EbookSize_MB_ = Convert.ToInt32(result.EbookSize_MB_),
                        Board = result.BoardId,
                        Class = result.ClassId
                    };
                }
                else
                {
                    objBooks.SubjectId = idSubject;
                    objBooks.SeriesId = idSeries;
                }
            }
            else
            {
                objBooks.SubjectId = idSubject;
                objBooks.SeriesId = idSeries;
            }
            return View("UpdateBooks", objBooks);

        }


        private Books getPrachiMainPortal_EncriptionKey(long? subjectId, long? seriesId, string title)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase uploadedFile, string ItemId)
        {
            if (uploadedFile != null && uploadedFile.ContentLength > 0)
            {
                HttpPostedFileBase postedFile = null;
                string fn = uploadedFile.FileName;
                string ext = "";
                //string rootpath = Server.MapPath("~/Images/TempFiles");
                string rootpath = Server.MapPath("~/ModuleFiles/Items");
                byte[] FileByteArray = new byte[uploadedFile.ContentLength];
                /// ModuleFiles / Items / 32320478french - 2015 - (new) - activitybook - 2.png
                uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);
                postedFile = uploadedFile;
                // ext = Path.GetExtension(postedFile.FileName);
                System.IO.File.Delete(rootpath + "/" + fn);
                if (!string.IsNullOrEmpty(fn))
                {
                    PrachiIndia.Portal.Helpers.Utility.Upload(rootpath, postedFile, fn, ext);

                    dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
                    int itemID = Convert.ToInt32(ItemId);
                    var catalogueObj = context.tblCataLogs.Where(x => x.Id == itemID).FirstOrDefault<tblCataLog>();
                    catalogueObj.Image = fn;
                    context.Entry(catalogueObj).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    PrachiIndia.Portal.Helpers.Utility.LoadCatalogue();
                    return Json(new
                    {
                        statusCode = 200,
                        status = 1,
                        file = fn + ext
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new
                    {
                        statusCode = 400,
                        status = 0,
                        file = uploadedFile.FileName
                    }, JsonRequestBehavior.AllowGet);

                }


            }
            return Json(new
            {
                statusCode = 400,
                file = uploadedFile.FileName
            }, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> UpoladImageFile(int Id, string Folder, string FileName)
        {
            long id = 0;
            if (Folder == "Subject")
            {
                var dbSubject = new MasterSubjectRepository().GetByIdAsync(Id);
                var item = new PrachiIndia.Sql.MasterSubject
                {
                    IpAddress = Common_Static.IPAddress(),
                    dtmAdd = dbSubject.dtmAdd,
                    Title = dbSubject.Title,
                    Description = dbSubject.Description,
                    OredrNo = dbSubject.OredrNo,
                    dtmUpdate = dbSubject.dtmUpdate,
                    Status = dbSubject.Status,
                    Id = dbSubject.Id,
                    Image = FileName,


                };
                var Subject = new MasterSubjectRepository().UpdateAsync(item);
                id = Subject.Id;
            }
            else if (Folder == "Series")
            {

                var dbSeries = new MasterSeriesRepositories().GetByIdAsync(Id);
                var item = new PrachiIndia.Sql.MasterSery
                {
                    IpAddress = Common_Static.IPAddress(),
                    dtmAdd = dbSeries.dtmAdd,
                    Title = dbSeries.Title,
                    Description = dbSeries.Description,
                    SubjectId = dbSeries.SubjectId,
                    OredrNo = dbSeries.OredrNo,
                    dtmUpdate = DateTime.UtcNow,
                    UserId = User.Identity.GetUserId(),
                    Status = dbSeries.Status,
                    Id = dbSeries.Id,
                    Image = FileName,
                };
                var Seriest = new MasterSeriesRepositories().UpdateAsync(item);
                id = Seriest.Id;

            }
            else if (Folder == "Class")
            {

                var dbClass = new MasterClassRepository().GetByIdAsync(Id);
                var item = new PrachiIndia.Sql.MasterClass
                {
                    IpAddress = Common_Static.IPAddress(),
                    dtmAdd = dbClass.dtmAdd,
                    Title = dbClass.Title,
                    Description = dbClass.Description,
                    OredrNo = dbClass.OredrNo,
                    dtmUpdate = DateTime.UtcNow,
                    UserId = User.Identity.GetUserId(),
                    Status = dbClass.Status,
                    Id = dbClass.Id,
                    Image = FileName,
                };
                var Seriest = new MasterClassRepository().UpdateAsync(item);
                id = Seriest.Id;

            }
            if (Folder == "Boards")
            {
                var dbSubject = new MasterBoardRepository().GetByIdAsync(Id);
                var item = new PrachiIndia.Sql.MasterBoard
                {
                    IpAddress = Common_Static.IPAddress(),
                    dtmAdd = dbSubject.dtmAdd,
                    Title = dbSubject.Title,
                    Description = dbSubject.Description,
                    OredrNo = dbSubject.OredrNo,
                    dtmUpdate = dbSubject.dtmUpdate,
                    Status = dbSubject.Status,
                    Id = dbSubject.Id,
                    Image = FileName,
                };
                var Subject = new MasterBoardRepository().UpdateAsync(item);
                id = Subject.Id;
            }
            if (Folder == "Books")
            {
                var dbBooks = new CatalogRepository().GetByIdAsync(Id);
                var item = new PrachiIndia.Sql.tblCataLog
                {
                    Id = dbBooks.Id,
                    SubjectId = dbBooks.SubjectId,
                    SeriesId = dbBooks.SeriesId,
                    Title = dbBooks.Title,
                    Author = dbBooks.Author,
                    ISBN = dbBooks.ISBN,
                    Edition = dbBooks.Edition,
                    Price = dbBooks.Price,
                    Discount = dbBooks.Discount,
                    Ebook = dbBooks.Ebook,
                    MultiMedia = dbBooks.MultiMedia,
                    Solutions = dbBooks.Solutions,
                    Image = FileName,
                    Description = dbBooks.Description,
                    dtmAdd = dbBooks.dtmAdd,
                    dtmUpdate = dbBooks.dtmUpdate,
                    dtmDelete = dbBooks.dtmDelete,
                    Status = dbBooks.Status,
                    IpAddress = dbBooks.IpAddress,
                    OrderNo = dbBooks.OrderNo


                };
                var Subject = new CatalogRepository().UpdateAsync(item);
                id = Subject.Id;
            }
            if (!string.IsNullOrEmpty(FileName))
            {
                string sourcePath = Server.MapPath("~/Images/TempFiles/" + FileName);
                string destinationPath = Server.MapPath("~/Images/" + Folder + "/" + FileName);
                MoveTempFileFromMainFolder(sourcePath, destinationPath);
            }
            return Json(id);
        }

        private void MoveTempFileFromMainFolder(string sourcePath, string destinationPath)
        {
            try
            {
                if (System.IO.File.Exists(sourcePath))
                {
                    if (System.IO.File.Exists(destinationPath))
                        System.IO.File.Delete(destinationPath);
                    System.IO.File.Move(sourcePath, destinationPath);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        public ActionResult Logout()
        {
            try
            {
                Session.RemoveAll();
                TempData.Remove("portfolio");
                FormsAuthentication.SignOut();

                return RedirectToAction("Login", "Account");
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #region "Subject"
        [OutputCache(Duration = 45)]
        public ActionResult Subject(string Message = "")
        {
            MasterSubjectRepository subjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.SubjectRepositories);
            @ViewBag.Message = Message;
            return View(subjectRepository.GetAll().OrderBy(t => t.OredrNo).ToList());
        }
        public ActionResult ManageSubject(int id = 0)
        {
            SubjectModel subjectobj = (SubjectModel)Factory.Factory.GetInstance(RepositoryType.SubjectModel);
            MasterSubjectRepository subjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.SubjectRepositories);
            var model = subjectRepository.GetById(id);
            if (model != null)
            {
                subjectobj.Id = model.Id;
                subjectobj.Description = model.Description;
                subjectobj.Title = model.Title;
                subjectobj.Image = model.Image;
                subjectobj.Status = model.Status == 1 ? true : false;
            }
            return View(subjectobj);
        }
        [HttpPost]
        public ActionResult ManageSubject(SubjectModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var allowedExtensions = new[] {
            ".Jpg",".png", ".jpeg"
        };
            string myfile = string.Empty;
            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                {
                    ModelState.AddModelError("Image", "Please choose only image fil.");
                    return View(model);
                }
                else
                {

                    string name = Path.GetFileNameWithoutExtension(file.FileName); //getting file name without extension  
                    myfile = name + "_" + Guid.NewGuid().ToString().Substring(0, 8) + ext; //appending the name with id  
                                                                                           // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/ModuleFiles/Subjects/"), myfile);
                    file.SaveAs(path);
                }
            }
            MasterSubjectRepository subjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.SubjectRepositories);
            if (!string.IsNullOrWhiteSpace(myfile) && !string.IsNullOrWhiteSpace(model.Image))
            {
                var path = Path.Combine(Server.MapPath("~/ModuleFiles/Subjects/"), model.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            if (string.IsNullOrWhiteSpace(myfile))
            {
                myfile = model.Image;
            }

            if (model.Id > 0)
            {
                var subject = subjectRepository.GetByIdAsync(model.Id);
                subject.IpAddress = Common_Static.IPAddress();
                subject.dtmUpdate = DateTime.UtcNow;
                subject.Title = model.Title;
                subject.Description = model.Description;
                subject.Status = model.Status == true ? 1 : 0;
                subject.Image = myfile;
                subjectRepository.UpdateAsync(subject);
            }
            else
            {
                var subject = new Sql.MasterSubject
                {
                    Description = model.Description,
                    dtmUpdate = DateTime.Now,
                    dtmAdd = DateTime.Now,
                    Image = myfile,
                    IpAddress = Common_Static.IPAddress(),
                    OredrNo = subjectRepository.GetAll().Count() + 1,
                    Status = model.Status == true ? 1 : 0,
                    Title = model.Title
                };
                subjectRepository.CreateAsync(subject);
            }
            return RedirectToAction("Subject");
        }
        [HttpPost]
        public JsonResult DeleteSubject(int SubjectID, string Flag)
        {
            var subjectRepository = new MasterSubjectRepository();
            var subject = subjectRepository.GetByIdAsync(SubjectID);
            subject.IpAddress = Common_Static.IPAddress();
            subject.dtmUpdate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                subject.Status = 0;
            }
            else
            {
                subject.Status = 1;
            }
            subjectRepository.UpdateAsync(subject);
            return Json(Flag, JsonRequestBehavior.DenyGet);
            //return RedirectToAction("Subject", new { Message = Flag });
        }
        public async Task<JsonResult> UpdateStatus(int Id, string type, IsPublished act = IsPublished.All)
        {
            var result = 0;
            var board = new MasterSubjectRepository();
            Sql.MasterSubject bord = new Sql.MasterSubject();
            var SubjectModel = board.GetByIdAsync(Id);
            SubjectModel.dtmUpdate = DateTime.UtcNow;
            if (SubjectModel.Status == IsPublished.Yes.GetHashCode())
            {
                SubjectModel.Status = Convert.ToInt16(IsPublished.No);
            }
            else
            {
                SubjectModel.Status = Convert.ToInt16(IsPublished.Yes);
            }
            result = board.UpdateAsyncStatus(SubjectModel);
            return Json(result);
        }
        #endregion
        #region "Series"
        [HttpGet]
        public async Task<ActionResult> Series(int subject = 0, string Message = "")
        {
            dynamic series;
            MasterSubjectRepository MasterSubjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSubjectrepository);
            ViewBag.Subject = new SelectList(await Task.Run(() => (MasterSubjectRepository.GetAll().Where(x => x.Status == 1).Select(x => new { SubjectId = x.Id, Name = x.Title }).ToList())), "SubjectId", "Name");
            MasterSeriesRepositories seriesRepositories = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            DVDRepository dVDRepository = (DVDRepository)Factory.FactoryRepository.GetInstance(RepositoryType.DVDMasterReposiroty);
            if (subject <= 0)
                series = seriesRepositories.GetAll().Where(x => x.Status == 1).OrderBy(t => t.OredrNo).ToList();
            else
                series = seriesRepositories.SearchFor(t => t.SubjectId == subject).OrderBy(t => t.OredrNo).ToList();
            await Task.WhenAll();

            @ViewBag.Message = Message;
            return View(series);
        }
        public async Task<ActionResult> ManageSeries(int id = 0)
        {
            MasterSubjectRepository MasterSubjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSubjectrepository);
            TempData["Subject"] = new SelectList(await Task.Run(() => (MasterSubjectRepository.GetAll().Where(x => x.Status == 1).Select(x => new { SubjectId = x.Id, Name = x.Title }).ToList())), "SubjectId", "Name");
            TempData.Keep("Subject");
            DVDRepository dVDRepository = (DVDRepository)Factory.FactoryRepository.GetInstance(RepositoryType.DVDMasterReposiroty);
            TempData["DVDMaster"] = new SelectList(await Task.Run(() => (dVDRepository.GetAll().Where(x => x.Status == true).Select(x => new { Id = x.Id, Name = x.Name }).ToList())), "Id", "Name");
            TempData.Keep("DVDMaster");
            SeriesModel series = (SeriesModel)Factory.Factory.GetInstance(RepositoryType.MasterSery);
            MasterSeriesRepositories masterSeriesRepositories = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            var model = masterSeriesRepositories.GetById(id);
            if (model != null)
            {
                series.Id = model.Id;
                series.Title = model.Title;
                series.Description = model.Description;
                series.Status = model.Status == 1 ? true : false;
                series.Image = model.Image;
                series.SubjectId = model.SubjectId ?? -1;
                series.DVDType = model.DvdType ?? 0;
            }
            await Task.WhenAll();
            return View(series);
        }
        [HttpPost]
        public ActionResult ManageSeries(SeriesModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            MasterSeriesRepositories seriesRepository = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            var allowedExtensions = new[] {
            ".png", ".jpg", ".jpeg"
        };
            string myfile = string.Empty;
            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                {
                    ModelState.AddModelError("Image", "Please choose only image fil.");
                    return View(model);
                }
                else
                {
                    string name = Path.GetFileNameWithoutExtension(file.FileName); //getting file name without extension  
                    myfile = name + "_" + Guid.NewGuid().ToString().Substring(0, 8) + ext; //appending the name with id                                                                                             // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/ModuleFiles/Series/"), myfile);
                    file.SaveAs(path);
                }
            }

            if (!string.IsNullOrWhiteSpace(myfile) && !string.IsNullOrWhiteSpace(model.Image))
            {
                var path = Path.Combine(Server.MapPath("~/ModuleFiles/Series/"), model.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            if (string.IsNullOrWhiteSpace(myfile))
            {
                myfile = model.Image;
            }
            if (model.Id > 0)
            {
                var series = seriesRepository.GetByIdAsync(model.Id);
                series.IpAddress = Common_Static.IPAddress();
                series.dtmUpdate = DateTime.UtcNow;
                series.Title = model.Title;
                series.Description = model.Description;
                series.Status = model.Status == true ? 1 : 0;
                series.Image = myfile;
                series.DvdType = model.DVDType;

                seriesRepository.UpdateAsync(series);
            }
            else
            {
                var masterSery = new Sql.MasterSery
                {
                    Description = model.Description,
                    dtmUpdate = DateTime.Now,
                    dtmAdd = DateTime.Now,
                    Image = myfile,
                    IpAddress = Common_Static.IPAddress(),
                    OredrNo = seriesRepository.GetAll().Count() + 1,
                    Status = model.Status == true ? 1 : 0,
                    Title = model.Title,
                    SubjectId = model.SubjectId,

                };
                seriesRepository.CreateAsync(masterSery);
            }
            return RedirectToAction("Series", "Admin", new { subject = model.SubjectId, Message = "" });
        }
        [HttpPost]
        public JsonResult DeleteSeries(int SeriesID, string Flag)
        {
            MasterSeriesRepositories seriesRepository = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            var series = seriesRepository.GetByIdAsync(SeriesID);
            series.IpAddress = Common_Static.IPAddress();
            series.dtmUpdate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                series.Status = 0;
            }
            else
            {
                series.Status = 1;
            }
            seriesRepository.UpdateAsync(series);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult UpdateStatus(int SeriesID, string Flag)
        {
            var seriesRepository = new MasterSeriesRepositories();
            var series = seriesRepository.GetByIdAsync(SeriesID);
            series.IpAddress = Common_Static.IPAddress();
            series.dtmUpdate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                series.ShowAtHome = false;
            }
            else
            {
                series.ShowAtHome = true;
            }
            seriesRepository.UpdateAsync(series);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }
        public async Task<JsonResult> UpdateSeriesStatus(int Id, string type, IsPublished act = IsPublished.All)
        {
            var result = 0;
            MasterSeriesRepositories seriesRepository = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            var SubjectModel = seriesRepository.GetByIdAsync(Id);
            SubjectModel.dtmUpdate = DateTime.UtcNow;
            if (SubjectModel.Status == IsPublished.Yes.GetHashCode())
            {
                SubjectModel.Status = Convert.ToInt16(IsPublished.No);
            }
            else
            {
                SubjectModel.Status = Convert.ToInt16(IsPublished.Yes);
            }
            result = seriesRepository.UpdateAsyncStatus(SubjectModel);
            return Json(result);
        }
        #endregion
        #region " Class "
        public ActionResult Class(string Message = "")
        {
            MasterClassRepository classRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
            @ViewBag.Message = Message;
            return View(classRepository.GetAll().OrderBy(t => t.OredrNo).ToList());
        }
        public ActionResult ManageClass(int id = 0)
        {
            MasterClassRepository classRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
            ClassModel classModel = (ClassModel)Factory.Factory.GetInstance(RepositoryType.ClassModel);
            var model = classRepository.GetById(id);
            if (model != null)
            {
                classModel.Id = model.Id;
                classModel.Description = model.Description;
                classModel.Title = model.Title;
                classModel.Image = model.Image;
                classModel.Status = model.Status == 1 ? true : false;
            }
            return View(classModel);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ManageClass(ClassModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            MasterClassRepository classRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
            if (model.Id > 0)
            {
                var classes = classRepository.GetByIdAsync(model.Id);
                classes.IpAddress = Common_Static.IPAddress();
                classes.dtmUpdate = DateTime.UtcNow;
                classes.Title = model.Title;
                classes.Description = model.Description;
                classes.Status = model.Status == true ? 1 : 0;
                classRepository.UpdateAsync(classes);
            }
            else
            {
                var classes = new Sql.MasterClass
                {
                    Description = model.Description,
                    dtmUpdate = DateTime.Now,
                    dtmAdd = DateTime.Now,
                    IpAddress = Common_Static.IPAddress(),
                    OredrNo = classRepository.GetAll().Count() + 1,
                    Status = model.Status == true ? 1 : 0,
                    Title = model.Title
                };
                classRepository.CreateAsync(classes);
            }
            return RedirectToAction("Class");
        }
        public ActionResult DeleteClass(int ClassID, string Flag)
        {
            var classRepository = new MasterClassRepository();
            var classes = classRepository.GetByIdAsync(ClassID);
            classes.IpAddress = Common_Static.IPAddress();
            classes.dtmUpdate = DateTime.UtcNow;

            if (Flag.ToLower() == "false")
            {
                classes.Status = 0;
            }
            else
            {
                classes.Status = 1;
            }
            classRepository.UpdateAsync(classes);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> CreateMasterClass(MasterClasses Class)
        {
            MasterClass masterClass = (MasterClass)Factory.Factory.GetInstance(RepositoryType.MasterClass);
            var ObjClass = new MasterClasses();
            try
            {
                if (Class.Id == 0)
                {
                    masterClass.Title = Class.Title;
                    masterClass.Description = Class.Description;
                    masterClass.OredrNo = Class.OredrNo;
                    masterClass.Status = (int)IsPublished.Yes;
                    masterClass.dtmAdd = DateTime.UtcNow;
                    masterClass.IpAddress = Common_Static.IPAddress();
                    masterClass.dtmUpdate = DateTime.UtcNow;
                    masterClass = new MasterClassRepository().CreateAsync(masterClass);
                    ObjClass.Id = masterClass.Id;
                    ObjClass.IdServer = Common_Static.ToSafeInt(masterClass.idServer);
                }
                else
                {
                    MasterClassRepository classRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
                    var dbClass = classRepository.GetByIdAsync(Class.Id);

                    var item = new PrachiIndia.Sql.MasterClass
                    {
                        IpAddress = Common_Static.IPAddress(),
                        dtmAdd = dbClass.dtmAdd,
                        Title = Class.Title,
                        Description = Class.Description,
                        Image = dbClass.Image,
                        OredrNo = Class.OredrNo,
                        dtmUpdate = DateTime.UtcNow,
                        Status = dbClass.Status,
                        Id = dbClass.Id
                    };
                    var reult = classRepository.UpdateAsync(item);
                    ObjClass.Id = reult.Id;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return Json(ObjClass);
        }

        public async Task<JsonResult> UpdateStatusClass(int Id, string type, IsPublished act = IsPublished.All)
        {
            var result = 0;
            MasterClassRepository classRepository = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
            MasterClass masterClass = (MasterClass)Factory.Factory.GetInstance(RepositoryType.MasterClass);
            var ClassModel = classRepository.GetByIdAsync(Id);
            ClassModel.dtmUpdate = DateTime.UtcNow;
            if (ClassModel.Status == IsPublished.Yes.GetHashCode())
            {
                masterClass.Status = Convert.ToInt16(IsPublished.No);
            }
            else
            {
                masterClass.Status = Convert.ToInt16(IsPublished.Yes);
            }
            result = classRepository.UpdateAsyncStatus(ClassModel);
            return Json(result);
        }
        #endregion
        #region "Boards"
        public ActionResult Boards(string Message)
        {
            MasterBoardRepository boardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
            @ViewBag.Message = Message;
            return View(boardRepository.GetAll().ToList());
        }
        public ActionResult ManageBoards(int id = 0)
        {
            BoardModel boardResult = (BoardModel)Factory.Factory.GetInstance(RepositoryType.BoardModel);
            MasterBoardRepository boardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
            var model = boardRepository.GetById(id);
            if (model != null)
            {
                boardResult.Id = model.Id;
                boardResult.Description = model.Description;
                boardResult.Title = model.Title;
                boardResult.Image = model.Image;
                boardResult.Status = model.Status == 1 ? true : false;
            }
            return View(boardResult);
        }
        [HttpPost]
        public ActionResult ManageBoards(BoardModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            MasterBoardRepository boardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
            if (model.Id > 0)
            {
                var boardResult = boardRepository.GetByIdAsync(model.Id);
                boardResult.IpAddress = Common_Static.IPAddress();
                boardResult.dtmUpdate = DateTime.UtcNow;
                boardResult.Title = model.Title;
                boardResult.Description = model.Description;
                boardResult.Status = model.Status == true ? 1 : 0;
                boardRepository.UpdateAsync(boardResult);
            }
            else
            {
                var boardItem = new Sql.MasterBoard
                {
                    Description = model.Description,
                    dtmUpdate = DateTime.Now,
                    dtmAdd = DateTime.Now,
                    IpAddress = Common_Static.IPAddress(),
                    OredrNo = boardRepository.GetAll().Count() + 1,
                    Status = model.Status == true ? 1 : 0,
                    Title = model.Title
                };
                boardRepository.CreateAsync(boardItem);
            }
            return RedirectToAction("Boards");
        }

        public ActionResult DeleteBoard(int BoardID, string Flag)
        {
            MasterBoardRepository boardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
            var board = boardRepository.GetByIdAsync(BoardID);
            board.IpAddress = Common_Static.IPAddress();
            board.dtmUpdate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                board.Status = 0;
            }
            else
            {
                board.Status = 1;
            }
            boardRepository.UpdateAsync(board);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<JsonResult> CreateMasterBoards(MasterBoards Boards)
        {
            var SqlBoards = new PrachiIndia.Sql.MasterBoard();
            var ObjBoards = new MasterBoards();
            try
            {
                if (Boards.Id == 0)
                {
                    SqlBoards.Title = Boards.Title;
                    SqlBoards.Description = Boards.Description;
                    SqlBoards.OredrNo = Boards.OredrNo;
                    SqlBoards.Status = (int)IsPublished.Yes;
                    SqlBoards.dtmAdd = DateTime.UtcNow;
                    SqlBoards.IpAddress = Common_Static.IPAddress();
                    SqlBoards.dtmUpdate = DateTime.UtcNow;
                    SqlBoards = new MasterBoardRepository().CreateAsync(SqlBoards);
                    SqlBoards.idServer = Boards.IdServer;
                    Boards.Id = SqlBoards.Id;

                }
                else
                {
                    var dbBoards = new MasterBoardRepository().GetByIdAsync(Boards.Id);

                    var item = new PrachiIndia.Sql.MasterBoard
                    {
                        IpAddress = Common_Static.IPAddress(),
                        dtmAdd = dbBoards.dtmAdd,
                        Title = Boards.Title,
                        Description = Boards.Description,
                        OredrNo = Boards.OredrNo,
                        dtmUpdate = DateTime.UtcNow,
                        Status = dbBoards.Status,
                        Image = dbBoards.Image,
                        // idServer = getIDserver(dbBoards.idServer, Boards.IdServer),
                        Id = dbBoards.Id,

                    };
                    SqlBoards = new MasterBoardRepository().UpdateAsync(item);

                    Boards.Id = SqlBoards.Id;
                }

            }
            catch (Exception ex)
            {
                ObjBoards = new MasterBoards();
            }
            return Json(Boards);
        }

        public async Task<JsonResult> UpdateStatusBoards(int Id, string type, IsPublished act = IsPublished.All)
        {
            var result = 0;
            var Board = new MasterBoardRepository();
            Sql.MasterBoard bord = new Sql.MasterBoard();
            var BoardModel = Board.GetByIdAsync(Id);
            BoardModel.dtmUpdate = DateTime.UtcNow;
            if (BoardModel.Status == IsPublished.Yes.GetHashCode())
            {
                BoardModel.Status = Convert.ToInt16(IsPublished.No);
            }
            else
            {
                BoardModel.Status = Convert.ToInt16(IsPublished.Yes);
            }
            result = Board.UpdateAsyncStatus(BoardModel);
            return Json(result);
        }
        #endregion
        #region "Books"
        public ActionResult BookList(long subject = 0, long series = 0)
        {
            ViewBag.Subjects = subject;
            ViewBag.Series = series = 0;

            Session.Remove("BookImage");
            // @ViewBag.Message = Message;
            var seriesItems = GetSeries(subject);
            seriesItems.Insert(0, new SelectListItem { Text = "All Series", Value = "0" });
            ViewBag.AllSeries = seriesItems;
            var ctalogRepository = new CatalogRepository();
            //var lstBooks = MasterSubjectRepository.GetByID(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries).ToList();
            var Query = ctalogRepository.GetAll();

            if (subject > 0 && series > 0)
            {
                Query = Query.Where(t => t.MasterSubject.Id == subject && t.MasterSery.Id == subject);
            }
            else if (subject > 0 && series <= 0)
            {
                Query = Query.Where(t => t.MasterSubject.Id == subject);
            }

            var lstBooks = Query.OrderBy(t => t.SubjectId).OrderBy(t => t.Title).ToList();

            var results = (from c in lstBooks
                           orderby c.Title descending
                           select new Books
                           {

                               Id = c.Id,
                               Title = c.Title,

                               Author = c.Author,
                               Image = c.Image,
                               ISBN = c.ISBN,
                               Edition = c.Edition,
                               Class = PrachiIndia.Portal.Helpers.Utility.classesByClassID(c.ClassId),
                               Board = PrachiIndia.Portal.Helpers.Utility.BoardsByClassID(c.BoardId),
                               Status = c.Status,

                           }).ToList();
            return View(results);
        }
        //public ActionResult ManageBooks(long Id = 0)
        //{
        //    var book = new Books();
        //    book.Classes = new List<GroupValues>();
        //    var dbContext = new MasterClassRepository();
        //    var allClass = dbContext.GetAll().ToList();
        //    var results = (from m in allClass select m).AsEnumerable().Select(m => new GroupValues { Id = m.Id.ToString(), Name = m.Title, Selected = false });
        //    if (results != null && results.Any())
        //        book.Classes.AddRange(results);


        //    return View(book);
        //}
        [HttpPost]
        public ActionResult ManageBooks(Books book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            return View("BookList");
        }
        public async Task<ActionResult> Books(string Message = "")
        {
            MasterSubjectRepository masterSubjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSubjectrepository);
            MasterSeriesRepositories masterSeriesRepository = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            MasterBoardRepository masterBoardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
            MasterClassRepository masterClassReposiroty = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
            ViewBag.Boards = new SelectList(await Task.Run(() => (masterBoardRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
            ViewBag.Class = new SelectList(await Task.Run(() => (masterClassReposiroty.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
            ViewBag.Subjects = new SelectList(await Task.Run(() => (masterSubjectRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
            ViewBag.Series = new SelectList(await Task.Run(() => (masterSeriesRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");

            Session.Remove("BookImage");
            @ViewBag.Message = Message;
            await Task.WhenAll();
            return View();
        }
        [HttpPost]
        public PartialViewResult GetAllBooks(int idSubject = 0, int idSeries = 0, string idBoard = "", string idClass = "", string Title = "")
        {
            CatalogRepository catalogRepository = (CatalogRepository)Factory.FactoryRepository.GetInstance(RepositoryType.CatalogRepository);
            IQueryable<tblCataLog> Query = catalogRepository.GetAll();
            if (idSubject != 0 && idSeries != 0 && idBoard != "" && idClass != "" && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard && t.ClassId == idClass && t.Title == Title);
            }
            else if (idSubject != 0 && idSeries != 0 && idBoard != "" && idClass != "")
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard && t.ClassId == idClass);
            }
            else if (idSubject != 0 && idSeries != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard);
            }
            else if (idSubject != 0 && idSeries != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard);
            }
            else if (idClass != "" && idSubject != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSubject.Id == idSubject && t.BoardId == idBoard);
            }
            else if (idClass != "" && idSeries != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSery.Id == idSeries && t.BoardId == idBoard);
            }
            else if (idSubject != 0 && idSeries != 0)
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries);
            }
            else if (idBoard != "" && idClass != "")
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.ClassId == idClass);
            }

            else if (idBoard != "" && idSubject != 0)
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.MasterSubject.Id == idSubject);
            }
            else if (idBoard != "" && idSeries != 0)
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.MasterSery.Id == idSeries);
            }
            else if (idBoard != "" && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.Title == Title);
            }
            else if (idClass != "" && idSubject != 0)
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSery.Id == idSubject);
            }
            else if (idClass != "" && idSeries != 0)
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSery.Id == idSeries);
            }
            else if (idClass != "" && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.ClassId == idClass && t.Title == Title);
            }
            else if (idSubject != 0 && idSeries != 0)
            {
                Query = Query.Where(t => t.MasterSery.Id == idSubject && t.MasterSery.Id == idSeries);
            }
            else if (idSubject != 0 && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.MasterSery.Id == idSubject && t.Title == Title);
            }
            else if (!string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.Title == Title);
            }
            else if (!string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.Title == Title);
            }
            else if (idClass != "")
            {
                Query = Query.Where(t => t.ClassId == idClass);
            }
            else if (idBoard != "")
            {
                Query = Query.Where(t => t.BoardId == idClass);
            }
            else if (idSeries != 0)
            {
                Query = Query.Where(t => t.MasterSery.Id == idSeries);
            }
            else if (idSubject != 0)
            {
                Query = Query.Where(t => t.MasterSery.Id == idSubject);
            }
            var lstBooks = Query.ToList();
            if (lstBooks != null)
            {
                try
                {
                    var result = from c in lstBooks
                                 orderby c.Title descending
                                 select new tblCataLog
                                 {
                                     Id = c.Id,
                                     Title = c.Title,
                                     Author = c.Author,
                                     Image = c.Image,
                                     ISBN = c.ISBN,
                                     CreateDate = c.dtmAdd,
                                     Edition = c.Edition,
                                     Solutions = c.Solutions,
                                     Multimedia = c.MultiMedia,
                                     Worksheet = c.Worksheet,
                                     Ebook = c.Ebook,
                                     LessonPlan = c.LessonPlan,
                                     Class = Utility.classesByClassID(c.ClassId),
                                     Board = Utility.BoardsByClassID(c.BoardId),
                                     Status = c.Status,
                                     Series = Utility.SeriesByID(c.SeriesId),
                                     SeriesId = c.SeriesId
                                 };
                    return PartialView("_Book", result);
                }
                catch (Exception ex)
                {

                }
            }
            return null;
        }
        List<SelectListItem> GetSeries(long idSubject)
        {
            var lst = new List<SeriesModel>();
            var masterSeriesRepositories = new MasterSeriesRepositories();
            var lstBooks = masterSeriesRepositories.GetAll().Where(t => t.SubjectId == idSubject && t.Status == 1);
            var results = (from c in lstBooks
                           orderby c.Title ascending
                           select new SelectListItem
                           {
                               Value = c.Id.ToString(),
                               Text = c.Title,

                           }).ToList();
            return results;
        }
        public ContentResult getSeries(int idSubject)
        {

            List<SeriesModel> lst = new List<SeriesModel>();
            if (Request.IsAjaxRequest())
            {
                var MasterSubjectRepository = new MasterSeriesRepositories();
                var lstBooks = MasterSubjectRepository.GetAll().Where(t => t.SubjectId == idSubject && t.Status == 1);

                if (lstBooks != null)
                {
                    try
                    {
                        var result = from c in lstBooks
                                     orderby c.Title ascending
                                     select new
                                     {
                                         Id = c.Id,
                                         Title = c.Title,

                                     };
                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                        return new ContentResult()
                        {
                            Content = serializer.Serialize(result),
                            ContentType = "application/json",
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return null;
        }
        [HttpPost]
        public JsonResult DeleteBooks(int BookID, string Flag)
        {
            var bookRepository = new CatalogRepository();
            var book = bookRepository.GetByIdAsync(BookID);
            book.IpAddress = Common_Static.IPAddress();
            book.dtmUpdate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                book.Status = 0;
            }
            else
            {
                book.Status = 1;
            }
            bookRepository.UpdateAsync(book);
            Utility.LoadCatalogue();
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult CreateMasterBooks(Books Books)
        {
            dbPrachiIndia_PortalEntities context = (dbPrachiIndia_PortalEntities)Factory.FactoryRepository.GetInstance(RepositoryType.dbPrachiIndia_PortalEntities);
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            var SqlBooks = new PrachiIndia.Sql.tblCataLog();
            Books ObjBooks = (Books)Factory.Factory.GetInstance(RepositoryType.Books);
            try
            {
                //Task T1;
                if (Books.Id == 0)
                {
                    if (Books.CatalogClass.Count > 0)
                    {
                        var HttpPostedFile = (HttpPostedFile)Session["BookImage"];
                        if (HttpPostedFile != null)
                        {
                            var path = Path.Combine(Server.MapPath("~/ModuleFiles/Items"), HttpPostedFile.FileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            HttpPostedFile.SaveAs(path);
                        }
                        foreach (CatalogClass PostedClass in Books.CatalogClass)
                        {
                            foreach (var boardItem in Books.Boards)
                            {
                                SqlBooks.SubjectId = Books.SubjectId;
                                SqlBooks.SeriesId = Books.SeriesId;
                                SqlBooks.Title = Books.Title;
                                SqlBooks.Author = Books.Author;
                                SqlBooks.ISBN = Books.ISBN;
                                SqlBooks.Edition = Books.Edition;
                                SqlBooks.Discount = Books.Discount;
                                SqlBooks.Price = Books.Price;
                                SqlBooks.Ebook = Books.Ebook == null ? 0 : Books.Ebook;
                                SqlBooks.MultiMedia = Books.MultiMedia == null ? 0 : Books.MultiMedia;
                                SqlBooks.Solutions = Books.Solutions == null ? 0 : Books.Solutions;
                                SqlBooks.Description = Books.Description == null ? "" : Books.Description;
                                SqlBooks.idServer = Books.idServer;
                                SqlBooks.EncriptionKey = Books.EncriptionKey;
                                SqlBooks.Status = (int)IsPublished.Yes;
                                SqlBooks.dtmAdd = DateTime.UtcNow;
                                SqlBooks.IpAddress = Common_Static.IPAddress();
                                SqlBooks.dtmUpdate = DateTime.UtcNow;
                                SqlBooks.dtmDelete = DateTime.UtcNow;
                                SqlBooks.BoardId = boardItem;
                                SqlBooks.ClassId = Convert.ToString(PostedClass.ClassId);
                                SqlBooks.EbookPrice = Books.EbookPrice;
                                SqlBooks.PrintPrice = Books.PrintPrice;
                                SqlBooks.PageCount = Books.PageCount;
                                SqlBooks.Colour = (int?)Books.Colour;
                                SqlBooks.EbookSize_MB_ = Books.EbookSize_MB_;
                                SqlBooks.LessonPlan = Books.LessonPlan;
                                SqlBooks.TestPaper = Books.TestPaper;
                                SqlBooks.TestPaperSolution = Books.TestPaperSolution;
                                SqlBooks.Published = Books.Published;
                                SqlBooks.PublishedBy = Books.PublishedBy;
                                SqlBooks.PublishedDate = Books.PublishedDate;
                                SqlBooks.Image = HttpPostedFile == null ? "Cover_image.png" : HttpPostedFile.FileName;
                                SqlBooks.TestPaper = Books.TestPaper;
                                SqlBooks.PrintPrice = Books.PrintPrice;
                                SqlBooks.Tax = Books.TaxId;
                                SqlBooks.isSize = Books.is_size;
                                SqlBooks.PublishedDate = DateTime.UtcNow;
                                context.tblCataLogs.Add(SqlBooks);
                                context.SaveChanges();
                            }

                        }
                    }
                    Utility.LoadCatalogue();
                  //  T1 = Task.Run(() => Utility.LoadCatalogue());
                    SqlBooks.Id = SqlBooks.Id;
                    var output = JsonConvert.SerializeObject(SqlBooks,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

                    var result = Json(output, JsonRequestBehavior.AllowGet);
                    result.MaxJsonLength = int.MaxValue;
                   // await T1;
                    return result;
                }
                else
                {
                    var Currentbook = context.tblCataLogs.Where(x => x.Id == Books.Id).FirstOrDefault<tblCataLog>();
                   // var BID = Currentbook.BoardId;
                   // var CID = Convert.ToInt64(Currentbook.ClassId);
                    //if (Books.CatalogClass == null)
                    //{
                    //    Books.CatalogClass = new List<CatalogClass>();
                    //    Books.CatalogClass.Add(new CatalogClass { ClassId = Convert.ToInt64(CID) });
                    //}
                    //foreach (CatalogClass PostedClass in Books.CatalogClass)
                    //{
                    //    foreach (var boardItem in Books.Boards)
                    //    {
                            Currentbook.ISBN = Books.ISBN;
                            Currentbook.Edition = Books.Edition;
                            Currentbook.Ebook = Books.Ebook == null ? 0 : Books.Ebook;
                            Currentbook.MultiMedia = Books.MultiMedia == null ? 0 : Books.MultiMedia;
                            Currentbook.Solutions = Books.Solutions == null ? 0 : Books.Solutions;
                            Currentbook.Title = Books.Title;
                            Currentbook.Author = Books.Author;
                            Currentbook.idServer = Books.idServer;
                            //   Currentbook.EncriptionKey = Books.EncriptionKey;
                            Currentbook.Description = Books.Description == null ? "" : Books.Description;
                            Currentbook.Status = (int)IsPublished.Yes;
                            Currentbook.dtmAdd = DateTime.UtcNow;
                            Currentbook.IpAddress = Common_Static.IPAddress();
                            Currentbook.dtmUpdate = DateTime.UtcNow;
                            Currentbook.dtmDelete = DateTime.UtcNow;
                            Currentbook.EbookPrice = Books.EbookPrice; 
                            Currentbook.Price = Books.Price;
                            Currentbook.Edition = Books.Edition; 
                            Currentbook.EbookPrice = Books.EbookPrice;
                            Currentbook.Discount = Books.Discount;
                            Currentbook.PrintPrice = Books.PrintPrice;
                            Currentbook.Price = Books.Price;
                            Currentbook.ISBN = Books.ISBN;
                            Currentbook.Tax = Books.TaxId;
                            Currentbook.PageCount = Books.PageCount;
                    //Currentbook.BoardId = boardItem;
                    //Currentbook.ClassId = Convert.ToString(PostedClass.ClassId);
                    //if (!Books.Boards.Contains(BID))
                    //{
                    context.Entry(Currentbook).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    //}
                    //else if (PostedClass.ClassId != CID)
                    //{
                    //    context.tblCataLogs.Add(Currentbook);
                    //    context.SaveChanges();
                    //}
                    //else
                    //{

                    //}

                    Currentbook.Id = Books.Id;
                    //    }
                    //}
                    //T1 = Task.Run(() => Utility.LoadCatalogue());
                    Utility.LoadCatalogue();
                    var output = JsonConvert.SerializeObject(SqlBooks,
                                    Formatting.None,
                                    new JsonSerializerSettings()
                                    {
                                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                    });
                    var result = Json(output, JsonRequestBehavior.AllowGet);
                    result.MaxJsonLength = int.MaxValue;
                    //await T1;
                    return result;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
           // return Json(ObjBooks);
        }
        [HttpPost]
        public void PostImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                Session.Remove("BookImage");
                Session["BookImage"] = System.Web.HttpContext.Current.Request.Files["PostedImage"];
                HttpPostedFile PostedImage = (HttpPostedFile)Session["BookImage"];
            }
        }
        public async Task<JsonResult> UpdateStatusBooks(int Id, string type, IsPublished act = IsPublished.All)
        {
            var result = 0;
            var Books = new CatalogRepository();
            Sql.tblCataLog bord = new Sql.tblCataLog();
            var BoardModel = Books.GetByIdAsync(Id);
            BoardModel.dtmUpdate = DateTime.UtcNow;
            if (BoardModel.Status == IsPublished.Yes.GetHashCode())
            {
                BoardModel.Status = Convert.ToInt16(IsPublished.No);
            }
            else
            {
                BoardModel.Status = Convert.ToInt16(IsPublished.Yes);
            }
            result = Books.UpdateAsyncStatus(BoardModel);
            return Json(result);
        }
        public ActionResult BookDetails(int id = 0, string Message = "")
        {
            var bookItem = new CatalogRepository().FindByIdAsync(id);
            var result = new PrachiIndia.Portal.Areas.CPanel.Models.BookModel();
            if (bookItem != null)
            {
                result.Class = Utility.classesByClassID(bookItem.ClassId);
                result.Author = bookItem.Author;
                result.Title = bookItem.Title;
                result.ISBN = bookItem.ISBN;
                result.Id = bookItem.Id;
                result.Price = bookItem.Price ?? 0;
                result.Subject = bookItem.MasterSubject.Title;
                result.Series = bookItem.MasterSery.Title;
                result.Chapters = (from item in new ChapterRepository().Search(t => t.BookId == result.Id).Where(x => x.Status == 1).OrderBy(t => t.ChapterIndex)
                                   select new Models.ChapterModel
                                   {
                                       BookId = item.BookId ?? 0,
                                       Descreption = item.Description,
                                       FromPage = item.FromPage ?? 0,
                                       Id = item.Id,
                                       Title = item.Title,
                                       ToPage = item.ToPage ?? 0,
                                       OrderNo = item.ChapterIndex ?? 0,
                                       Status = item.Status
                                   }).ToList();

            }
            @ViewBag.Message = Message;
            return View(result);
        }
        #endregion
        #region Manage Book Content
        public ActionResult ManageChapter(long bookId, long chapterId = 0)
        {
            var chapterRepository = new ChapterRepository();
            var chapter = chapterRepository.GetByIdAsync(chapterId);
            var result = new Models.ChapterModel();
            var bookItem = new CatalogRepository().FindByIdAsync((int)bookId);
            var classes = Helpers.Utility.classesByClassID(bookItem.ClassId);
            ViewBag.BookName = string.Format("{0} - {1}", bookItem.Title, classes);

            result.BookId = bookId;
            if (chapter != null)
            {
                result.Id = chapter.Id;
                result.OrderNo = chapter.ChapterIndex ?? 0;
                result.Title = chapter.Title;
                result.ToPage = chapter.ToPage ?? 0;
                result.FromPage = chapter.FromPage ?? 0;
                result.BookId = chapter.BookId ?? 0;
                result.Descreption = chapter.Description;
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult ManageChapter(Models.ChapterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string myfile = string.Empty;

            var chapterRepository = new ChapterRepository();

            if (model.Id > 0)
            {
                var subject = chapterRepository.GetByIdAsync(model.Id);
                subject.BookId = model.BookId;
                subject.ChapterIndex = (short)model.OrderNo;
                subject.Description = model.Descreption;
                subject.FromPage = (short)model.FromPage;
                subject.Id = model.Id;
                subject.Title = model.Title;
                subject.ToPage = (short)model.ToPage;
                subject.Updateddate = DateTime.Now;
                // subject.Status = model.Status == true ? 1 : 0;
                chapterRepository.UpdateAsync(subject);
            }
            else
            {
                var subject = new Sql.Chapter
                {
                    Description = model.Descreption,
                    Title = model.Title,
                    BookId = model.BookId,
                    ChapterIndex = (short)model.OrderNo,
                    CreatedDate = DateTime.Now,
                    FromPage = (short)model.FromPage,
                    Id = model.Id,
                    ToPage = (short)model.ToPage,
                    Updateddate = DateTime.Now,
                    Status = 1

                };
                chapterRepository.CreateAsync(subject);
            }
            return RedirectToAction("BookDetails/" + model.BookId);
        }

        public ActionResult DeleteChapter(int ChapterID, string Flag)
        {
            var chapterRepository = new ChapterRepository();
            var Chapter = chapterRepository.GetByIdAsync(ChapterID);
            Chapter.Updateddate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                Chapter.Status = 0;
            }
            else
            {
                Chapter.Status = 1;
            }
            chapterRepository.UpdateAsync(Chapter);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ContentList(long bookId = 0, long Id = 0, string Message = "")
        {
            ViewBag.Chapter = new ChapterRepository().FindByIdAsync(Id);
            var contents = new ContentRepository().Search(t => t.ChapterId == Id && t.BookId == bookId).Where(x => x.Status == 1);
            //if (contents.ElementType == 2)
            //{
            //    subject.MathInput = model.MathInput;
            //}
            var bookItem = new CatalogRepository().FindByIdAsync((int)bookId);
            ViewBag.BookId = bookId;
            var classes = Utility.classesByClassID(bookItem.ClassId);
            ViewBag.BookName = string.Format("{0} - {1}", bookItem.Title, classes);
            @ViewBag.Message = Message;
            return View(contents);
        }
        public ActionResult ManageContents(long ChapterId, long ContentId = 0)
        {
            var chapter = new ChapterRepository().FindByIdAsync(ChapterId);
            ViewBag.ChapterType = EnumHelper.ToSelectLists<ChapterType>();
            var content = new ContentRepository().FindByIdAsync(ContentId);
            if (content != null)
            {
                var item = new Models.ChapterContentModel
                {
                    BookId = content.BookId ?? 0,
                    ChapterId = content.ChapterId ?? 0,
                    ContentType = content.ContentType ?? 0,
                    Descreption = content.Desc,
                    FileName = content.Name,
                    Id = content.Id,
                    OrderNo = content.OrderId ?? 0,
                    Title = content.Title,
                    Type = (int)content.Type,

                };
                return View(item);
            }
            var model = new Models.ChapterContentModel();
            model.BookId = chapter.BookId ?? 0;
            model.ChapterId = chapter.Id;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageContents(Models.ChapterContentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string myfile = string.Empty;

            var chapterRepository = new ChapterContentRepository();

            if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
            {

                var fileextension = System.IO.Path.GetExtension(model.PostedFile.FileName);
                if (fileextension == ".html" || fileextension == ".htm")
                {
                    BinaryReader b = new BinaryReader(model.PostedFile.InputStream);
                    byte[] binData = b.ReadBytes(model.PostedFile.ContentLength);
                    model.Descreption = System.Text.Encoding.UTF8.GetString(binData);
                }
                else
                {
                    myfile = Guid.NewGuid().ToString().Substring(0, 10) + model.PostedFile.FileName;
                    string _FileName = Path.GetFileName(model.PostedFile.FileName);
                    string BasePath = Server.MapPath("~/ModuleFiles/Chapters/");
                    string _path = string.Empty;
                    if (model.Type == ChapterType.Lesson_Plan.GetHashCode())
                    {
                        _path = Path.Combine(BasePath + "LessonPlan\\", myfile);
                    }
                    else if (model.Type == ChapterType.Workheet.GetHashCode())
                    {
                        _path = Path.Combine(BasePath + "Worksheets\\", myfile);
                    }
                    else if (model.Type == ChapterType.Audio_Video.GetHashCode())
                    {
                        _path = Path.Combine(BasePath + "Audio\\", myfile);
                    }
                    else if (model.Type == ChapterType.Solutions.GetHashCode())
                    {
                        _path = Path.Combine(BasePath + "Solutions\\", myfile);
                    }
                    model.PostedFile.SaveAs(_path);
                }
            }
            if (model.Id > 0)
            {
                var subject = chapterRepository.GetByIdAsync(model.Id);
                subject.BookId = model.BookId;
                subject.ChapterId = model.ChapterId;
                subject.OrderId = (short)model.OrderNo;
                if (model.ContentType == 2)
                {
                    subject.Desc = model.MathInput;
                }
                else {
                    subject.Desc = model.Descreption;
                }
                subject.Type = model.Type;
                subject.Id = model.Id;
                subject.Name = model.ContentType != contentType.ImageHtml.GetHashCode() ? string.IsNullOrWhiteSpace(myfile) ? subject.Name : myfile : string.Empty;
                subject.Title = model.Title;
                subject.CreatedDate = DateTime.Now;
                subject.Status = 1;
                subject.ContentType = Convert.ToInt16(model.ContentType);
                //subject.Updateddate = DateTime.Now;
                // subject.Status = model.Status == true ? 1 : 0;
                chapterRepository.UpdateAsync(subject);
            }
            else
            {
                var subject = new Sql.ChapterContent
                {
                    BookId = model.BookId,
                    ChapterId = model.ChapterId,
                    OrderId = (short)model.OrderNo,
                    Desc = model.Descreption,
                    Type = model.Type,
                    Id = model.Id,
                    Name = myfile,
                    Title = model.Title,
                    CreatedDate = DateTime.Now,
                    Status = 1,
                    ContentType = Convert.ToInt16(model.ContentType),

                };
                chapterRepository.CreateAsync(subject);
            }
            return RedirectToAction("ContentList", new { bookId = model.BookId, Id = model.ChapterId, Message = "" });

        }

        public ActionResult DeleteContent(int ContentID, int BookId, string Flag)
        {
            var contentRepository = new ContentRepository();
            var Content = contentRepository.GetByIdAsync(ContentID);
            Content.UpdatedDate = DateTime.UtcNow;
            if (Flag.ToLower() == "false")
            {
                Content.Status = 0;
            }
            else
            {
                Content.Status = 1;
            }
            contentRepository.UpdateAsync(Content);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }
        #endregion
        #region Start Book Update With Order Display Using Catalouge
        public ActionResult BookOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BookOrder2()
        {
            return View();
        }


        public ContentResult getBookTitle(int idSubject, int SeriesId = 0, string title = "")
        {

            List<MasterClasses> lst = new List<MasterClasses>();
            if (Request.IsAjaxRequest())
            {
                var blBook = new Readedge_BusLogic.blBook();
                var lstClass = blBook.GetBook(idSubject, SeriesId, title);

                if (lstClass != null)
                {
                    try
                    {
                        var result = from c in lstClass
                                     orderby c.Title ascending
                                     select new
                                     {
                                         c.idServer,
                                         c.Title,
                                         c.EncriptionKey
                                     };
                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                        return new ContentResult()
                        {
                            Content = serializer.Serialize(result),
                            ContentType = "application/json",
                        };
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return null;
        }
        #endregion
        #region Manage Lession Plan
        public ActionResult Lessions(long id)
        {
            return View();
        }
        #endregion

        public ActionResult Addcity()
        {
            return View();
        }
        public JsonResult GetAllcountry()
        {
            var Country = new CountryRepositories().Get().ToList().Select(x => new { Name = x.Name, CountryId = x.CountryId });
            return Json(Country, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetState(string countryId)
        {
            var State = new StateRepositories().Get().ToList().Where(t => t.CountryId == Utility.ToSafeInt(countryId));
            return Json(State);
        }


        public ActionResult CityMaster()
        {
            CityRepositories cityrepository = (CityRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CityRepository);
            return View(cityrepository.GetAll().OrderByDescending(x => x.CityId).ToList());
        }
        public ActionResult CityMasterEdit(int CityId)
        {
            CityRepositories cityrepository = (CityRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CityRepository);
            StateRepositories staterepository = (StateRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.StateRepositories);
            var city = cityrepository.FindByIdAsync(CityId);
            ViewBag.State = new SelectList(staterepository.GetAll().Select(x => new { StateID = x.StaeteId, StateName = x.StateName }).ToList(), "StateID", "StateName", city.StateId);
            return View(cityrepository.FindByIdAsync(CityId));

        }
        [HttpPost]
        public ActionResult CityMasterEdit(City cityobj)
        {
            if (ModelState.IsValid)
            {
                CityRepositories cityrepository = (CityRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CityRepository);
                cityrepository.Edit(cityobj);
                return RedirectToAction("CityMaster");
            }
            return View();

        }

        [HttpPost]
        public JsonResult AddCityName(int StateId, string CityName)
        {
            if (ModelState.IsValid)
            {
                CityRepositories city = (CityRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CityRepository);
                City cityobj = (City)Factory.Factory.GetInstance(RepositoryType.City);
                cityobj.CityName = CityName;
                cityobj.StateId = StateId;
                cityobj.IsActive = true;
                city.CreateAsync(cityobj);
            }
            return Json(true, JsonRequestBehavior.DenyGet);
        }

        public ActionResult StateMaster()
        {
            CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
            ViewBag.Country = new SelectList(countryrepository.GetAll().Select(x => new { CountryID = x.CountryId, Name = x.Name }).ToList(), "CountryID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult StateMaster(State stateObj)
        {
            if (ModelState.IsValid)
            {
                StateRepositories stateRepositories = (StateRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.StateRepositories);
                stateObj.IsActive = true;
                stateRepositories.CreateAsync(stateObj);
                return RedirectToAction("StateMasterList");
            }
            return View();
        }
        public ActionResult StateMasterList(State stateObj)
        {
            StateRepositories stateRepositories = (StateRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.StateRepositories);
            return View(stateRepositories.GetAll().OrderBy(x => x.StateCode).ToList());
        }
        public ActionResult StateMasterEdit(int StateId)
        {
            StateRepositories stateRepositories = (StateRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.StateRepositories);
            CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
            var state = stateRepositories.FindByIdAsync(StateId);
            ViewBag.Country = new SelectList(countryrepository.GetAll().Select(x => new { CountryID = x.CountryId, Name = x.Name }).ToList(), "CountryID", "Name", state.CountryId);
            return View(state);

        }
        [HttpPost]
        public ActionResult StateMasterEdit(State stateobj)
        {
            if (ModelState.IsValid)
            {
                StateRepositories stateRepositories = (StateRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.StateRepositories);
                stateRepositories.EditAsync(stateobj);
                return RedirectToAction("StateMasterList");
            }
            return View();

        }
        public ActionResult CountryMasterList()
        {
            CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
            return View(countryrepository.GetAll().OrderBy(x => x.Name).ToList());
        }
        public ActionResult CountryMaster()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CountryMaster(Country Countryobj)
        {
            if (ModelState.IsValid)
            {
                CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
                countryrepository.CreateAsync(Countryobj);
                return RedirectToAction("CountryMasterList");
            }
            return View();
        }

        public ActionResult CountryMasterEdit(int Countryid)
        {
            CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
            return View(countryrepository.FindByIdAsync(Countryid));
        }
        [HttpPost]
        public ActionResult CountryMasterEdit(Country countryobj)
        {
            if (ModelState.IsValid)
            {
                CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
                countryrepository.EditAsync(countryobj);
                return RedirectToAction("CountryMasterList");
            }
            return View();

        }

        [HttpPost]
        public JsonResult ChangeCountryStatus(int CountryID, string Flag)
        {
            CountryRepositories countryrepository = (CountryRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.CountryRepository);
            var country = countryrepository.GetByIdAsync(CountryID);
            if (Flag.ToLower() == "false")
            {
                country.Status = "1"; //true
            }
            else
            {
                country.Status = "0";  //false
            }
            countryrepository.UpdateAsync(country);
            return Json(Flag, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SyncReadEdge(int contentId, int ChapterId, int BookId, int Type, int ContentType)
        {
            string result = string.Empty;

            try
            {
                var chapterContentRepo = new ChapterContentRepository();
                result = chapterContentRepo.SyncReadEdgeChapter(BookId, ChapterId, Type, ContentType, contentId,User.Identity.GetUserId());

                var chapterContent = chapterContentRepo.GetByIdAsync(contentId);


                if (result.ToLower() == "true")
                {

                    string _FileName = chapterContent.Name;

                    string BasePath = Server.MapPath("~/ModuleFiles/Chapters/");
                    string _SourcePath = string.Empty;
                    if (chapterContent.Type == ChapterType.Lesson_Plan.GetHashCode())
                    {
                        _SourcePath = Path.Combine(BasePath + "LessonPlan\\", _FileName);
                    }
                    else if (chapterContent.Type == ChapterType.Workheet.GetHashCode())
                    {
                        _SourcePath = Path.Combine(BasePath + "Worksheets\\", _FileName);
                    }
                    else if (chapterContent.Type == ChapterType.Audio_Video.GetHashCode())
                    {
                        _SourcePath = Path.Combine(BasePath + "Audio\\", _FileName);
                    }
                    else if (chapterContent.Type == ChapterType.Solutions.GetHashCode())
                    {
                        _SourcePath = Path.Combine(BasePath + "Solutions\\", _FileName);
                    }

                    if (System.IO.File.Exists(_SourcePath))
                    {
                        string _DestinationPath = ConfigurationManager.AppSettings["ReadEdgeApp"].ToString(CultureInfo.InvariantCulture);

                        var bookDetails = new CatalogRepository().GetByIdAsync(BookId);
                        string Bookclass = classes(BookId);


                        _DestinationPath = _DestinationPath + "//wwwroot//Books//Prachi - " + bookDetails.Title + " - " + Bookclass;

                        if (!Directory.Exists(_DestinationPath))
                        {
                            Directory.CreateDirectory(_DestinationPath);
                        }
                        _DestinationPath = Path.Combine(_DestinationPath + "\\", _FileName);
                        System.IO.File.Copy(_SourcePath, _DestinationPath, true);
                    }
                    else
                    {
                        ErrorLogger.CreateErrorLog("File not found for sync Path$" + _SourcePath, "Admin/SyncReadEdge");
                        result = "File not found for sync.";
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogger.CreateErrorLog(ex.Message, "Admin/SyncReadEdge");
                result = ex.Message;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult SyncChapterReadEdge(int ChapterId, int BookId)
        {
            string result = string.Empty;

            try
            {
                var chapterRepo = new ChapterContentRepository();
                result = chapterRepo.SyncReadEdgeChapter(BookId, ChapterId, User.Identity.GetUserId());

                var AllChapterRepo = chapterRepo.GetAll();

                var chapterContentRepo = AllChapterRepo.Where(x => x.BookId == BookId && x.ChapterId == ChapterId).ToList();
                if (result.ToLower() == "true")
                {
                    foreach (var chapterContent in chapterContentRepo)
                    {

                        string _FileName = chapterContent.Name;

                        string BasePath = Server.MapPath("~/ModuleFiles/Chapters/");
                        string _SourcePath = string.Empty;
                        if (chapterContent.Type == ChapterType.Lesson_Plan.GetHashCode())
                        {
                            _SourcePath = Path.Combine(BasePath + "LessonPlan\\", _FileName);
                        }
                        else if (chapterContent.Type == ChapterType.Workheet.GetHashCode())
                        {
                            _SourcePath = Path.Combine(BasePath + "Worksheets\\", _FileName);
                        }
                        else if (chapterContent.Type == ChapterType.Audio_Video.GetHashCode())
                        {
                            _SourcePath = Path.Combine(BasePath + "Audio\\", _FileName);
                        }
                        else if (chapterContent.Type == ChapterType.Solutions.GetHashCode())
                        {
                            _SourcePath = Path.Combine(BasePath + "Solutions\\", _FileName);
                        }

                        if (System.IO.File.Exists(_SourcePath))
                        {
                            string _DestinationPath = ConfigurationManager.AppSettings["ReadEdgeApp"].ToString(CultureInfo.InvariantCulture);
                            var bookDetails = new CatalogRepository().GetByIdAsync(BookId);
                            string Bookclass = classes(BookId);

                            _DestinationPath = _DestinationPath + "//wwwroot//Books//Prachi - " + bookDetails.Title + " - " + Bookclass;

                            if (!Directory.Exists(_DestinationPath))
                            {
                                Directory.CreateDirectory(_DestinationPath);
                            }
                            _DestinationPath = Path.Combine(_DestinationPath + "\\", _FileName);
                            System.IO.File.Copy(_SourcePath, _DestinationPath, true);
                        }
                        else
                        {
                            ErrorLogger.CreateErrorLog("File not found for sync Path$" + _SourcePath, "Admin/SyncChapterReadEdge");
                            // result = "File not found for sync.";
                        }

                    }
                }
                else
                {
                    result = "Fail !!! Sync Issue...Please try later or contatc administrator.";
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.CreateErrorLog(ex.Message, "Admin/SyncReadEdge");
                result = ex.Message;
            }
            return Json(result);
        }
    }
}