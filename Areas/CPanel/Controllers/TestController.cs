//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Script.Serialization;
//using System.Web.Security;
//using PrachiIndia.Sql.CustomRepositories;
//using PrachiIndia.Sql.Repositories;
//using PrachiIndia.Web.Areas.Model;
//using System.Threading.Tasks;
//using System.IO;
//using PrachiIndia.Portal.Helpers;
//using Microsoft.AspNet.Identity;
//using System.Configuration;
//using System.Globalization;
//using PrachiIndia.Sql;
//using Microsoft.Ajax.Utilities;
//using PrachiIndia.Portal.Areas.CPanel.Models;
//using System.Text;
//using System.Net.Http;
//using PrachiIndia.Portal.Models;

//namespace PrachiIndia.Portal.Areas.CPanel.Controllers
//{
//    [Authorize(Roles = "Admin,SuperAdmin")]
//    public class TestController : Controller
//    {
//        // GET: CPanel/Admin


//        #region Here All Order work start here For Addmin by deepak 
//        public ActionResult Index()
//        {
//            string ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
//            var orderProductRepository = new OrderProductRepository();
//            var itemRepository = new CatalogRepository();
//            var OrderRepository = new OrderRepository();
//            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
//            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
//            var result = query.DistinctBy(x => x.Id).ToList();
//            var items = (from item in result
//                         select new OrderTrackVm
//                         {
//                             OrderId = item.Order.Id.ToString() ?? "",
//                             TransactionId = item.Order.TransactionId,
//                             date = item.Order.CreatedDate.ToString(),
//                             price = item.Order.Amount ?? 0,
//                             status = item.Order.Status != 1 ? false : true,
//                             Dispatchby = item.Order.dispatchby,
//                             AWBNO = item.Order.AWBNO,
//                             IsRecive = item.Order.IsRecive ?? false,
//                             IsProduct = item.Order.IsInventory.ToString() ?? "false",
//                             InventoryMessage = item.Order.InventoryMessage
//                         }).DistinctBy(x => x.OrderId).ToList();
//            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//            return View(items);
//        }
//        [HttpPost]
//        public ActionResult Index(OrderTrackVm model)
//        {
//            var orderProductRepository = new OrderProductRepository();
//            var itemRepository = new CatalogRepository();
//            var OrderRepository = new OrderRepository();
//            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
//            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
//            if (model.fromdate != null && model.todate != null)
//            {
//                query = query.Where(t => t.CreatedDate >= model.fromdate && t.CreatedDate <= model.todate);
//            }
//            else if (model.fromdate != null)
//            {
//                query = query.Where(t => t.Order.CreatedDate >= model.fromdate);
//            }
//            else if (model.todate != null)
//            {
//                query = query.Where(t => t.Order.CreatedDate <= model.todate);
//            }

//            var result = query.DistinctBy(x => x.Id).ToList();
//            var items = (from item in result
//                         select new OrderTrackVm
//                         {
//                             OrderId = item.Order.Id.ToString() ?? "",
//                             TransactionId = item.Order.TransactionId,
//                             date = item.Order.CreatedDate.ToString(),
//                             price = item.Order.Amount ?? 0,
//                             status = item.Order.Status != 1 ? false : true,
//                             Dispatchby = item.Order.dispatchby,
//                             AWBNO = item.Order.AWBNO,
//                             IsRecive = item.Order.IsRecive ?? false
//                         }).DistinctBy(x => x.OrderId).ToList();
//            return View(items);
//        }

//        public ActionResult OrderDetail(int? OrderId)
//        {
//            string ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
//            var orderProductRepository = new OrderProductRepository();
//            var orderReposetory = new OrderRepository();
//            var itemRepository = new CatalogRepository();
//            var AspNetUser = new AspNetUserRepository();
//            AspNetUser objuser = new Sql.AspNetUser();
//            var customer = objuser;
//            var Userid = orderReposetory.GetAll().Where(x => x.Id == OrderId).ToList();
//            foreach (var item in Userid)
//            {
//                customer = new AspNetUserRepository().GetUser(item.UserId);
//            }
//            var CountryRepo = new CountryRepositories();
//            var StateRepo = new StateRepositories();
//            var CityRepo = new CityRepositories();
//            var Cid = Convert.ToInt32(customer.Country != null ? customer.Country : "0");
//            var Sid = Convert.ToInt32(customer.State != null ? customer.State : "0");
//            var CiId = Convert.ToInt32(customer.City != null ? customer.City : "0");
//            var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
//            var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
//            var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
//            var customers = new AspNetUser
//            {
//                FirstName = customer.FirstName,
//                Email = customer.Email,
//                Address = customer.Address,
//                Country = countryid,
//                City = CityId,
//                State = StateId,
//                PinCode = customer.PinCode,
//            };

//            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
//            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
//            var result = query.Where(x => x.OrderId == OrderId).ToList();
//            var items = (from item in result

//                         select new OrderTrackVm
//                         {
//                             TransactionId = item.Order.TransactionId,
//                             Title = item.Title,
//                             OrderId = item.OrderId.ToString(),
//                             date = item.CreatedDate.ToString(),
//                             Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
//                             price = item.Price ?? 0,
//                             Quantity = item.Quantity ?? 0,
//                             Discount = item.Discount ?? 0,
//                             status = item.Order.Status != 1 ? false : true,
//                             classs = item.tblCataLog.ClassId,
//                             Board = item.tblCataLog.BoardId,
//                             BookType = item.BookType.ToString(),

//                         }).ToList();
//            ViewBag.Customer = customers;
//            return View(items);
//        }
//        #endregion
//        public static class Booktypes
//        {
//            public static string Ebook = "Ebook";
//            public static string Pbook = "Pbook";
//        }
//        [ChildActionOnly]
//        public PartialViewResult _Dispatch(OrderTrackVm model)
//        {
//            return PartialView(model);
//        }
//        [ChildActionOnly]
//        public PartialViewResult _ReciveOrderTrack(OrderTrackVm model)
//        {
//            return PartialView(model);
//        }
//        public PartialViewResult _CheckProductInventory(OrderTrackVm model)
//        {
//            return PartialView(model);
//        }
//        public ActionResult UpdateInventoryStatus(OrderTrackVm model)
//        {
//            var OrderRepository = new OrderRepository();
//            var OrdertrackRepository = new OrderTrackRepositories();
//            var item = new Order
//            {
//                Id = Convert.ToInt32(model.OrderId),
//                IsInventory = model.IsProduct == "1" ? true : false,
//                InventoryMessage = model.InventoryMessage,
//            };
//            var result = OrderRepository.UpdateInventoryStatus(item);
//            return RedirectToAction("index");
//        }
//        public ActionResult UpdateOrderTrack()
//        {
//            string ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
//            var Id = Convert.ToInt64(Request.QueryString["OrderId"]);
//            var orderProductRepository = new OrderProductRepository();
//            var orderReposetory = new OrderRepository();
//            var itemRepository = new CatalogRepository();
//            var AspNetUser = new AspNetUserRepository();
//            AspNetUser objuser = new Sql.AspNetUser();
//            var customer = objuser;
//            var Userid = orderReposetory.GetAll().Where(x => x.Id == Id).ToList();
//            foreach (var item in Userid)
//            {
//                customer = new AspNetUserRepository().GetUser(item.UserId);
//            }
//            var CountryRepo = new CountryRepositories();
//            var StateRepo = new StateRepositories();
//            var CityRepo = new CityRepositories();
//            var Cid = Convert.ToInt32(customer.Country != null ? customer.Country : "0");
//            var Sid = Convert.ToInt32(customer.State != null ? customer.State : "0");
//            var CiId = Convert.ToInt32(customer.City != null ? customer.City : "0");
//            var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
//            var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
//            var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
//            var customers = new AspNetUser
//            {
//                FirstName = customer.FirstName,
//                Email = customer.Email,
//                Address = customer.Address,
//                Country = countryid,
//                City = CityId,
//                State = StateId,
//                PinCode = customer.PinCode,
//            };
//            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
//            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
//            var result = query.Where(x => x.OrderId == Id).ToList();
//            var items = (from item in result
//                         select new OrderTrackUpdateVm
//                         {
//                             TransactionId = item.Order.TransactionId,
//                             Title = item.Title,
//                             OrderId = item.OrderId.ToString(),
//                             date = item.CreatedDate.ToString(),
//                             Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
//                             price = string.Format("{0:0.00}", item.Price),
//                             Quantity = item.Quantity.ToString(),
//                             Discount = string.Format("{0:0.00}", item.Discount),
//                             status = item.Order.Status != 1 ? false : true,
//                             BookType = item.BookType.ToString(),
//                             AWBNO = item.Order.AWBNO,
//                             Dispatchby = item.Order.dispatchby
//                         }).ToList();
//            ViewBag.Customer = customers;
//            return View(items);
//        }
//        #region Order Track Work Start Here By Deepak
//        [HttpPost]
//        public ActionResult UpdateOrderTrack(OrderTrackUpdateVm model)
//        {
//            var OrderRepository = new OrderRepository();
//            var OrdertrackRepository = new OrderTrackRepositories();
//            var item = new Order
//            {
//                Id = Convert.ToInt32(model.OrderId),
//                OrderReciveBy = model.OrderRecivedBy,
//                IsRecive = true,
//                ReciveDate = model.ReciveDate,
//            };
//            var result = OrderRepository.UpdateOrderRecive(item);
//            return RedirectToAction("index");
//        }
//        #endregion
//        #region Disptach Order From User Side To user Start Here by Deepak
//        [HttpPost]
//        public ActionResult dispatchOrder(OrderTrackVm model)
//        {

//            var OrderId = Convert.ToInt64(model.OrderId);
//            var orderProductRepository = new OrderProductRepository();
//            var orderReposetory = new OrderRepository();
//            var itemRepository = new CatalogRepository();
//            var AspNetUser = new AspNetUserRepository();
//            AspNetUser objuser = new Sql.AspNetUser();
//            var customer = objuser;
//            var Userid = orderReposetory.GetAll().Where(x => x.Id == OrderId).ToList();
//            foreach (var item in Userid)
//            {
//                customer = new AspNetUserRepository().GetUser(item.UserId);
//            }
//            var CountryRepo = new CountryRepositories();
//            var StateRepo = new StateRepositories();
//            var CityRepo = new CityRepositories();
//            OrderTrackRepositories trackrepository = new OrderTrackRepositories();
//            var Cid = Convert.ToInt32(customer.Country != null ? customer.Country : "0");
//            var Sid = Convert.ToInt32(customer.State != null ? customer.State : "0");
//            var CiId = Convert.ToInt32(customer.City != null ? customer.City : "0");
//            var countryid = CountryRepo.FindByItemId(Cid) != null ? CountryRepo.FindByItemId(Cid).Name : "India";
//            var StateId = StateRepo.FindByItemId(Sid) != null ? StateRepo.FindByItemId(Sid).StateName : "";
//            var CityId = CityRepo.FindByItemId(CiId) != null ? CityRepo.FindByItemId(CiId).CityName : "";
//            var customers = new AspNetUser
//            {
//                FirstName = customer.FirstName,
//                Email = customer.Email,
//                Address = customer.Address,
//                Country = countryid,
//                City = CityId,
//                State = StateId,
//                PinCode = customer.PinCode,
//                PhoneNumber = customer.PhoneNumber,
//                Id = customer.Id
//            };
//            var orders = new Order
//            {
//                Id = Convert.ToInt64(model.OrderId),
//                AWBNO = model.AWBNO,
//                dispatchby = model.DispatchedBy
//            };
//            var OrderUdate = orderReposetory.Update(orders);
//            IQueryable<tblCataLog> query1 = itemRepository.GetAll();
//            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
//            if (OrderUdate)
//            {
//                var result = query.Where(x => x.OrderId == OrderId).ToList();
//                var objordertrack = (from itemss in result

//                                     select new OrderTrack
//                                     {
//                                         TransactionId = itemss.Order.TransactionId,
//                                         OrderId = Convert.ToInt32(itemss.OrderId),
//                                         Amount = itemss.Order.Amount,
//                                         Status = "Dispatched",
//                                         UserBillingAddress = customers.FirstName + ',' + customers.Address + ',' + customers.Email + ',' + customers.PhoneNumber + ',' + customers.Country + ',' + customers.State + ',' + customers.City + ',' + customers.PinCode,
//                                         UserId = itemss.Order.AspNetUser.Id,//item.Order.UserId,
//                                         ItemDestination = null,
//                                     }).ToList().DistinctBy(t => t.OrderId);
//                var status = trackrepository.Create(objordertrack);
//                var response = (from itemss in result
//                                select new DispatchOrderResponse
//                                {
//                                    TransactionId = itemss.Order.TransactionId,
//                                    Title = itemss.Title,
//                                    Price = String.Format("{0:0.00}", itemss.Price),
//                                    Quantity = Convert.ToInt32(itemss.Quantity),
//                                    TotalAmount = String.Format("{0:0.00}", itemss.Order.Amount),
//                                    Discount = Convert.ToDecimal(itemss.Discount),
//                                    AWBNO = model.AWBNO,
//                                    DispatchedBy = model.DispatchedBy,
//                                }).ToList();

//                Oredermail(customers, response);
//            }
//            return RedirectToAction("index");
//        }
//        #endregion
//        #region Send Email To User After Dipatch Order by Deepak:12:01:2016
//        public void Oredermail(AspNetUser customer, List<DispatchOrderResponse> responseObject)
//        {
//            var toMail = ConfigurationManager.AppSettings["MailTo"].ToString(CultureInfo.InvariantCulture);
//            var subject = "";
//            foreach (var item in responseObject)
//            {
//                subject = "Shipment of items in order " + item.TransactionId + "  by PrachiIndia.com";
//                break;
//            }
//            var message = MailTemplate(customer, responseObject);
//            MailServiece.Mail.SendMail(customer.Email, "", toMail, subject, message);
//            // var x = Portal.Framework.Utility.SendMail(subject, message, customer.Email, toMail);
//        }
//        static string MailTemplate(AspNetUser customer, List<DispatchOrderResponse> responseObject)
//        {
//            var textMessage = string.Empty;
//            textMessage += "<table border='0' cellpadding='0' cellspacing='0' width='720' align='center' style='font-family: verdana; font-size: 12px; line-height: 18px; border: solid 1px #f1f1f1;' >";
//            textMessage += "<tr>";
//            textMessage += " <td style='padding: 10px; width: 700px;'>";
//            textMessage += "<table border='0' cellpadding='0' cellspacing='0' width='100%'>";
//            textMessage += "  <tr>";
//            textMessage += "  <td align='left'>";

//            foreach (var item in responseObject)
//            {
//                textMessage += "   <h2>your order has been placed " + item.DispatchedBy + "</h2>";
//                textMessage += "  <h4>Order No. " + item.TransactionId + "  <br />Date :   " + DateTime.Now.ToString("dd-MM-yyyy") + " <br/>The shipment was sent through: " + item.DispatchedBy + " <br/> Shipment Tracking ID:" + item.AWBNO + " </h4>";
//                break;
//            }
//            textMessage += "</td>";
//            textMessage += " <td align='right'>";
//            textMessage += "<img src='http://prachiindia.com/img/logos/logo.png' width='200' alt='logo' />";
//            textMessage += "   </td>";
//            textMessage += "</tr>";
//            textMessage += " <tr>";
//            textMessage += "<td align='left' colspan='2'>";
//            textMessage += " <h4 style='text-decoration:underline;'>Billing/Shipping Address</h4>";
//            textMessage += "<p>Name: " + customer.FirstName + "<br/>Address: " + customer.Address + "<br /> City: " + customer.City + "<br /> State: " + customer.State + "<br /> Country: " + customer.Country + "<br /> Pincode: " + customer.PinCode + "<br/>Mobile: " + customer.PhoneNumber;
//            textMessage += " </p>";
//            textMessage += " </td>";
//            textMessage += " </tr>";
//            textMessage += "</table><br/><br/>";
//            textMessage += " <table cellpadding='0' cellspacing='0' width='100%' style='border: solid 1px #010101;'>";
//            textMessage += "  <thead>";
//            textMessage += "  <tr>";

//            textMessage += "  <th align='left' style='padding: 4px;'>Name</th>";
//            textMessage += "  <th align='left' style='padding: 4px;'>MRP Price</th>";
//            textMessage += "  <th align='left' style='padding: 4px;'>Quantity</th>";
//            textMessage += "  <th align='left' style='padding: 4px;'>Discount</th>";
//            textMessage += "  <th align='right' style='padding: 4px;'>Subtotal</th>";
//            textMessage += "  </tr>";
//            textMessage += "  </thead>";
//            textMessage += " <tbody>";
//            if (responseObject != null && responseObject.Any())
//            {
//                decimal sum = 0;
//                decimal totalDiscount = 0;
//                foreach (var itemcart in responseObject)
//                {
//                    var discount = itemcart.Discount * itemcart.Quantity;
//                    var price = Convert.ToDecimal(itemcart.Price) * itemcart.Quantity;
//                    var subTotal = price - discount;
//                    totalDiscount = totalDiscount + discount;
//                    sum = sum + subTotal;
//                    textMessage += "  <tr>";
//                    textMessage += "  <td style='padding: 4px;' > " + itemcart.Title + "</td>";
//                    textMessage += " <td style='padding: 4px;'  > Rs " + itemcart.Price + "</td>";
//                    textMessage += "  <td style='padding: 4px;' > " + itemcart.Quantity + "</td>";
//                    textMessage += " <td style='padding: 4px;'  > Rs " + discount + "</td>";
//                    textMessage += "  <td style='padding: 4px;' align='right' > Rs " + subTotal + "</td>";
//                    textMessage += " </tr>";

//                }
//                textMessage += "   <tr>";
//                textMessage += "   <td style='padding: 4px;' colspan='3' align='right'>Total Discount:</td>";
//                textMessage += "    <td style='padding: 4px;' align='right'> Rs " + totalDiscount + "</td>";
//                textMessage += "  </tr>";
//                textMessage += "   <tr>";
//                textMessage += "   <td style='padding: 4px;' colspan='3' align='right'>Total Price:</td>";
//                textMessage += "    <td style='padding: 4px;' align='right'> Rs " + sum + "</td>";
//                textMessage += "  </tr>";

//            }
//            textMessage += "  </tbody>";
//            textMessage += "  </table><br/><br/><br/>";
//            textMessage += " <table border='0' cellpadding='0' cellspacing='0' width='100%'>";
//            textMessage += "  <tr>";
//            textMessage += " <td align='left'>";
//            textMessage += " <p>Company: <a href='http://prachiindia.com/'>Prach [India] Pvt. Ltd.</a><br />";
//            textMessage += "  Website: <a href='http://prachiindia.com/'>www.prachiindia.com</a><br />";
//            textMessage += " Email: <a href='mailto:contact@goclabs.com'>info@prachiindia.com</a><br />";
//            textMessage += " Address: 309/10, ALLIED HOUSE,<br />";
//            textMessage += "  INDER LOK,DELHI-110035<br />";
//            textMessage += "  Contact no: +91-11-47320666(8 Lines)";
//            textMessage += "  </p>";
//            textMessage += " </td>";
//            textMessage += "  </tr>";
//            textMessage += " </table>";
//            textMessage += "  </td>";
//            textMessage += "   </tr>";
//            textMessage += " </table>";
//            return textMessage;
//        }
//        #endregion
//        #region "Comman"

//        [AllowAnonymous]
//        public async Task<ActionResult> SelectPartialView(string uc, int id)
//        {

//            if (Request.IsAjaxRequest())
//            {
//                if (uc == "ucSubject")
//                {
//                    var objSubject = new Sql.MasterSubject();
//                    if (id > 0)
//                    {
//                        var SubjectRepository = new MasterSubjectRepository();
//                        var result = SubjectRepository.FindByIdAsync(id);

//                        if (result != null)
//                        {
//                            objSubject = new Sql.MasterSubject
//                            {
//                                Id = result.Id,
//                                Title = result.Title,
//                                Description = result.Description,
//                                OredrNo = result.OredrNo,
//                                Image = result.Image,
//                            };

//                        }
//                    }
//                    return View("ucSubject", objSubject);
//                }
//                else if (uc == "ucSeries")
//                {
//                    var objSubject = new SeriesModel();
//                    if (id > 0)
//                    {
//                        var SeriesRepository = new MasterSeriesRepositories();
//                        var result = SeriesRepository.FindByIdAsync(id);

//                        if (result != null)
//                        {
//                            objSubject = new SeriesModel
//                            {
//                                Id = result.Id,
//                                Title = result.Title,
//                                //SubjectId = result.SubjectId,
//                                Description = result.Description,
//                                // OredrNo = result.OredrNo,
//                                Image = result.Image,
//                            };

//                        }
//                    }
//                    return View("ucSeries", objSubject);
//                }
//                else if (uc == "ucClass")
//                {
//                    var objClass = new MasterClasses();
//                    if (id > 0)
//                    {
//                        var SeriesRepository = new MasterClassRepository();
//                        var result = SeriesRepository.FindByIdAsync(id);

//                        if (result != null)
//                        {
//                            objClass = new MasterClasses
//                            {
//                                Id = result.Id,
//                                Title = result.Title,
//                                Description = result.Description,
//                                OredrNo = result.OredrNo,
//                                Image = result.Image,
//                            };

//                        }
//                    }
//                    return View("ucClass", objClass);
//                }
//                else if (uc == "ucBoards")
//                {
//                    var objClass = new MasterBoards();
//                    if (id > 0)
//                    {
//                        var SeriesRepository = new MasterBoardRepository();
//                        var result = SeriesRepository.FindByIdAsync(id);

//                        if (result != null)
//                        {
//                            objClass = new MasterBoards
//                            {
//                                Id = result.Id,
//                                Title = result.Title,
//                                Description = result.Description,
//                                OredrNo = result.OredrNo,
//                                Image = result.Image,
//                            };

//                        }
//                    }

//                }
//                else if (uc == "ucBooks")
//                {
//                    var objBooks = new Books();
//                    if (id > 0)
//                    {
//                        var SeriesRepository = new CatalogRepository();
//                        var result = SeriesRepository.FindByIdAsync(id);

//                        if (result != null)
//                        {
//                            objBooks = new Books()
//                            {
//                                Id = result.Id,
//                                SubjectId = result.SubjectId ?? 0,
//                                SeriesId = result.SeriesId ?? 0,
//                                Title = result.Title,
//                                Author = result.Author,
//                                ISBN = result.ISBN,
//                                Edition = result.Edition,
//                                Price = result.Price,
//                                Discount = result.Discount,
//                                Ebook = result.Ebook,
//                                MultiMedia = result.MultiMedia,
//                                Solutions = result.Solutions,
//                                Image = result.Image,
//                                Description = result.Description,
//                                dtmAdd = result.dtmAdd,
//                                dtmUpdate = result.dtmUpdate,
//                                dtmDelete = result.dtmDelete,
//                                Status = result.Status,
//                                IpAddress = result.IpAddress,
//                                orderno = result.OrderNo ?? 0
//                            };
//                        }
//                    }
//                    return View("ucBooks", objBooks);
//                }
//            }
//            return View("Index", id);
//        }


//        public JsonResult Upload(HttpPostedFileBase uploadedFile)
//        {
//            if (uploadedFile != null && uploadedFile.ContentLength > 0)
//            {
//                HttpPostedFileBase postedFile = null;
//                string fn = uploadedFile.FileName;
//                string ext = "";
//                string rootpath = Server.MapPath("~/Images/TempFiles");
//                byte[] FileByteArray = new byte[uploadedFile.ContentLength];
//                uploadedFile.InputStream.Read(FileByteArray, 0, uploadedFile.ContentLength);
//                postedFile = uploadedFile;
//                // ext = Path.GetExtension(postedFile.FileName);
//                System.IO.File.Delete(rootpath + "/" + fn + ext);
//                if (!string.IsNullOrEmpty(fn))
//                {
//                    Utility.Upload(rootpath, postedFile, fn, ext);

//                    return Json(new
//                    {
//                        statusCode = 200,
//                        status = 1,
//                        file = fn + ext
//                    }, JsonRequestBehavior.AllowGet);

//                }
//                else
//                {
//                    return Json(new
//                    {
//                        statusCode = 400,
//                        status = 0,
//                        file = uploadedFile.FileName
//                    }, JsonRequestBehavior.AllowGet);

//                }


//            }
//            return Json(new
//            {
//                statusCode = 400,
//                file = uploadedFile.FileName
//            }, JsonRequestBehavior.AllowGet);
//        }


//        public async Task<JsonResult> UpoladImageFile(int Id, string Folder, string FileName)
//        {
//            long id = 0;
//            if (Folder == "Subject")
//            {
//                var dbSubject = new MasterSubjectRepository().GetByIdAsync(Id);
//                var item = new PrachiIndia.Sql.MasterSubject
//                {
//                    IpAddress = Common_Static.IPAddress(),
//                    dtmAdd = dbSubject.dtmAdd,
//                    Title = dbSubject.Title,
//                    Description = dbSubject.Description,
//                    OredrNo = dbSubject.OredrNo,
//                    dtmUpdate = dbSubject.dtmUpdate,
//                    Status = dbSubject.Status,
//                    Id = dbSubject.Id,
//                    Image = FileName,


//                };
//                var Subject = new MasterSubjectRepository().UpdateAsync(item);
//                id = Subject.Id;
//            }
//            else if (Folder == "Series")
//            {

//                var dbSeries = new MasterSeriesRepositories().GetByIdAsync(Id);
//                var item = new PrachiIndia.Sql.MasterSery
//                {
//                    IpAddress = Common_Static.IPAddress(),
//                    dtmAdd = dbSeries.dtmAdd,
//                    Title = dbSeries.Title,
//                    Description = dbSeries.Description,
//                    SubjectId = dbSeries.SubjectId,
//                    OredrNo = dbSeries.OredrNo,
//                    dtmUpdate = DateTime.UtcNow,
//                    UserId = User.Identity.GetUserId(),
//                    Status = dbSeries.Status,
//                    Id = dbSeries.Id,
//                    Image = FileName,
//                };
//                var Seriest = new MasterSeriesRepositories().UpdateAsync(item);
//                id = Seriest.Id;

//            }
//            else if (Folder == "Class")
//            {

//                var dbClass = new MasterClassRepository().GetByIdAsync(Id);
//                var item = new PrachiIndia.Sql.MasterClass
//                {
//                    IpAddress = Common_Static.IPAddress(),
//                    dtmAdd = dbClass.dtmAdd,
//                    Title = dbClass.Title,
//                    Description = dbClass.Description,
//                    OredrNo = dbClass.OredrNo,
//                    dtmUpdate = DateTime.UtcNow,
//                    UserId = User.Identity.GetUserId(),
//                    Status = dbClass.Status,
//                    Id = dbClass.Id,
//                    Image = FileName,
//                };
//                var Seriest = new MasterClassRepository().UpdateAsync(item);
//                id = Seriest.Id;

//            }
//            if (Folder == "Boards")
//            {
//                var dbSubject = new MasterBoardRepository().GetByIdAsync(Id);
//                var item = new PrachiIndia.Sql.MasterBoard
//                {
//                    IpAddress = Common_Static.IPAddress(),
//                    dtmAdd = dbSubject.dtmAdd,
//                    Title = dbSubject.Title,
//                    Description = dbSubject.Description,
//                    OredrNo = dbSubject.OredrNo,
//                    dtmUpdate = dbSubject.dtmUpdate,
//                    Status = dbSubject.Status,
//                    Id = dbSubject.Id,
//                    Image = FileName,
//                };
//                var Subject = new MasterBoardRepository().UpdateAsync(item);
//                id = Subject.Id;
//            }
//            if (Folder == "Books")
//            {
//                var dbBooks = new CatalogRepository().GetByIdAsync(Id);
//                var item = new PrachiIndia.Sql.tblCataLog
//                {
//                    Id = dbBooks.Id,
//                    SubjectId = dbBooks.SubjectId,
//                    SeriesId = dbBooks.SeriesId,
//                    Title = dbBooks.Title,
//                    Author = dbBooks.Author,
//                    ISBN = dbBooks.ISBN,
//                    Edition = dbBooks.Edition,
//                    Price = dbBooks.Price,
//                    Discount = dbBooks.Discount,
//                    Ebook = dbBooks.Ebook,
//                    MultiMedia = dbBooks.MultiMedia,
//                    Solutions = dbBooks.Solutions,
//                    Image = FileName,
//                    Description = dbBooks.Description,
//                    dtmAdd = dbBooks.dtmAdd,
//                    dtmUpdate = dbBooks.dtmUpdate,
//                    dtmDelete = dbBooks.dtmDelete,
//                    Status = dbBooks.Status,
//                    IpAddress = dbBooks.IpAddress,
//                    OrderNo = dbBooks.OrderNo


//                };
//                var Subject = new CatalogRepository().UpdateAsync(item);
//                id = Subject.Id;
//            }
//            if (!string.IsNullOrEmpty(FileName))
//            {
//                string sourcePath = Server.MapPath("~/Images/TempFiles/" + FileName);
//                string destinationPath = Server.MapPath("~/Images/" + Folder + "/" + FileName);
//                MoveTempFileFromMainFolder(sourcePath, destinationPath);
//            }
//            return Json(id);
//        }

//        private void MoveTempFileFromMainFolder(string sourcePath, string destinationPath)
//        {
//            try
//            {
//                if (System.IO.File.Exists(sourcePath))
//                {
//                    if (System.IO.File.Exists(destinationPath))
//                        System.IO.File.Delete(destinationPath);
//                    System.IO.File.Move(sourcePath, destinationPath);
//                }
//            }
//            catch (Exception ex)
//            {
//            }
//        }
//        #endregion

//        public ActionResult Logout()
//        {
//            try
//            {
//                Session.RemoveAll();
//                TempData.Remove("portfolio");
//                FormsAuthentication.SignOut();

//                return RedirectToAction("Login", "Account");
//            }
//            catch
//            {
//                return RedirectToAction("Login", "Account");
//            }
//        }
//        #region "Subject"
//        public ActionResult Subject()
//        {
//            return View();
//        }
//        public ContentResult GetAllSubject()
//        {

//            var lst = new List<Sql.MasterSubject>();
//            if (Request.IsAjaxRequest())
//            {
//                var MasterSubjectRepository = new MasterSubjectRepository();
//                var lstSubject = MasterSubjectRepository.GetAll();

//                if (lstSubject != null)
//                {
//                    try
//                    {
//                        var result = from c in lstSubject
//                                     orderby c.OredrNo ascending
//                                     select new
//                                     {
//                                         c.Id,
//                                         Title = c.Title,
//                                         CreateDate = c.dtmAdd,
//                                         Image = c.Image,
//                                         DisplayOrder = c.OredrNo,
//                                         Description = c.Description,
//                                         Status = c.Status
//                                     };
//                        //lst = (result as IEnumerable<MasterSubject>).Cast<MasterSubject>().ToList();
//                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//                        return new ContentResult()
//                        {
//                            Content = serializer.Serialize(result),
//                            ContentType = "application/json",
//                        };
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }
//            return null;
//        }

//        [HttpPost, ValidateInput(false)]
//        public async Task<JsonResult> CreateMasterSubject(Sql.MasterSubject subject)
//        {
//            var Sqlsubject = new PrachiIndia.Sql.MasterSubject();
//            var ObjSubject = new Sql.MasterSubject();
//            try
//            {
//                var masterSubjectRepository = new MasterSubjectRepository();
//                if (subject.Id == 0)
//                {
//                    Sqlsubject.Title = subject.Title;
//                    Sqlsubject.Description = subject.Description;
//                    Sqlsubject.OredrNo = subject.OredrNo;
//                    Sqlsubject.Status = (int)IsPublished.Yes;
//                    Sqlsubject.dtmAdd = DateTime.UtcNow;
//                    Sqlsubject.IpAddress = Common_Static.IPAddress();
//                    Sqlsubject.dtmUpdate = DateTime.UtcNow;
//                    Sqlsubject = masterSubjectRepository.CreateAsync(Sqlsubject);
//                    subject.Id = Sqlsubject.Id;
//                }
//                else
//                {
//                    var dbSubject = masterSubjectRepository.GetByIdAsync(subject.Id);

//                    var item = new PrachiIndia.Sql.MasterSubject
//                    {
//                        IpAddress = Common_Static.IPAddress(),
//                        dtmAdd = dbSubject.dtmAdd,
//                        Title = subject.Title,
//                        Description = subject.Description,
//                        OredrNo = subject.OredrNo,
//                        dtmUpdate = DateTime.UtcNow,
//                        Status = dbSubject.Status,
//                        Id = dbSubject.Id,

//                    };
//                    Sqlsubject = masterSubjectRepository.UpdateAsync(item);
//                    subject.Id = Sqlsubject.Id;
//                }

//            }
//            catch (Exception ex)
//            {
//                ObjSubject = new Sql.MasterSubject();
//            }
//            return Json(subject);
//        }

//        public async Task<JsonResult> UpdateStatus(int Id, string type, IsPublished act = IsPublished.All)
//        {
//            var result = 0;
//            var board = new MasterSubjectRepository();
//            Sql.MasterSubject bord = new Sql.MasterSubject();
//            var SubjectModel = board.GetByIdAsync(Id);
//            SubjectModel.dtmUpdate = DateTime.UtcNow;
//            if (SubjectModel.Status == IsPublished.Yes.GetHashCode())
//            {
//                SubjectModel.Status = Convert.ToInt16(IsPublished.No);
//            }
//            else
//            {
//                SubjectModel.Status = Convert.ToInt16(IsPublished.Yes);
//            }
//            result = board.UpdateAsyncStatus(SubjectModel);
//            return Json(result);
//        }
//        #endregion

//        #region "Series"
//        public ActionResult Series()
//        {
//            SeriesModel obj = new SeriesModel();
//            return View(obj);
//        }
//        public ContentResult GetAllSeries(int Id)
//        {

//            List<SeriesModel> lst = new List<SeriesModel>();
//            if (Request.IsAjaxRequest())
//            {
//                IQueryable<PrachiIndia.Sql.MasterSery> lstSubject = null;
//                //var lstSubject= new IQueryable<PrachiIndia.Sql.MasterSery>();
//                var MasterSubjectRepository = new MasterSeriesRepositories();
//                if (Id == 0)
//                {
//                    lstSubject = MasterSubjectRepository.GetAll();
//                }
//                else
//                {
//                    lstSubject = MasterSubjectRepository.GetAll().Where(t => t.SubjectId == Id);
//                }
//                if (lstSubject != null)
//                {
//                    try
//                    {
//                        var result = from c in lstSubject
//                                     orderby c.OredrNo ascending
//                                     select new
//                                     {
//                                         c.Id,
//                                         Title = c.Title,
//                                         CreateDate = c.dtmAdd,
//                                         Image = c.Image,
//                                         DisplayOrder = c.OredrNo,
//                                         Description = c.Description,
//                                         Status = c.Status
//                                     };
//                        //lst = (result as IEnumerable<MasterSubject>).Cast<MasterSubject>().ToList();
//                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//                        return new ContentResult()
//                        {
//                            Content = serializer.Serialize(result),
//                            ContentType = "application/json",
//                        };
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }
//            return null;
//        }

//        [HttpPost, ValidateInput(false)]
//        public async Task<JsonResult> CreateMasterSeries(SeriesModel Series)
//        {
//            var SqlSeriest = new PrachiIndia.Sql.MasterSery();
//            var ObjSeries = new SeriesModel();
//            try
//            {
//                if (Series.Id == 0)
//                {
//                    SqlSeriest.Title = Series.Title;
//                    SqlSeriest.Description = Series.Description;
//                    SqlSeriest.SubjectId = Series.SubjectId;
//                    SqlSeriest.OredrNo = Series.OredrNo;
//                    SqlSeriest.Status = (int)IsPublished.Yes;
//                    SqlSeriest.dtmAdd = DateTime.UtcNow;
//                    SqlSeriest.IpAddress = Common_Static.IPAddress();
//                    SqlSeriest.dtmUpdate = DateTime.UtcNow;
//                    SqlSeriest.UserId = User.Identity.GetUserId();
//                    SqlSeriest = new MasterSeriesRepositories().CreateAsync(SqlSeriest);
//                    ObjSeries.Id = SqlSeriest.Id;
//                }
//                else
//                {
//                    var dbSeries = new MasterSeriesRepositories().GetByIdAsync(Series.Id);

//                    var item = new PrachiIndia.Sql.MasterSery
//                    {
//                        IpAddress = Common_Static.IPAddress(),
//                        dtmAdd = dbSeries.dtmAdd,
//                        Title = Series.Title,
//                        Description = Series.Description,
//                        SubjectId = Series.SubjectId,
//                        OredrNo = Series.OredrNo,
//                        dtmUpdate = DateTime.UtcNow,
//                        Status = dbSeries.Status,
//                        UserId = User.Identity.GetUserId(),
//                        Id = dbSeries.Id,

//                    };
//                    SqlSeriest = new MasterSeriesRepositories().UpdateAsync(item);
//                    ObjSeries.Id = SqlSeriest.Id;
//                }

//            }
//            catch (Exception ex)
//            {
//                ObjSeries = new SeriesModel();
//            }
//            return Json(ObjSeries);
//        }

//        public async Task<JsonResult> UpdateSeriesStatus(int Id, string type, IsPublished act = IsPublished.All)
//        {
//            var result = 0;
//            var Series = new MasterSeriesRepositories();
//            // var Series = new Sql.MasterSery();
//            var SubjectModel = Series.GetByIdAsync(Id);
//            SubjectModel.dtmUpdate = DateTime.UtcNow;
//            if (SubjectModel.Status == IsPublished.Yes.GetHashCode())
//            {
//                SubjectModel.Status = Convert.ToInt16(IsPublished.No);
//            }
//            else
//            {
//                SubjectModel.Status = Convert.ToInt16(IsPublished.Yes);
//            }
//            result = Series.UpdateAsyncStatus(SubjectModel);
//            return Json(result);
//        }
//        #endregion


//        #region " Class "
//        public ActionResult Class()
//        {
//            return View();
//        }
//        public ContentResult GetAllClass()
//        {

//            List<MasterClasses> lst = new List<MasterClasses>();
//            if (Request.IsAjaxRequest())
//            {
//                var MasterClassRepository = new MasterClassRepository();
//                var lstClass = MasterClassRepository.GetAll();

//                if (lstClass != null)
//                {
//                    try
//                    {
//                        var result = from c in lstClass
//                                     orderby c.OredrNo ascending
//                                     select new
//                                     {
//                                         c.Id,
//                                         Title = c.Title,
//                                         CreateDate = c.dtmAdd,
//                                         Image = c.Image,
//                                         DisplayOrder = c.OredrNo,
//                                         Description = c.Description,
//                                         Status = c.Status
//                                     };
//                        //lst = (result as IEnumerable<MasterSubject>).Cast<MasterSubject>().ToList();
//                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//                        return new ContentResult()
//                        {
//                            Content = serializer.Serialize(result),
//                            ContentType = "application/json",
//                        };
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }
//            return null;
//        }

//        [HttpPost, ValidateInput(false)]
//        public async Task<JsonResult> CreateMasterClass(MasterClasses Class)
//        {
//            var SqlClass = new PrachiIndia.Sql.MasterClass();
//            var ObjClass = new MasterClasses();
//            try
//            {
//                if (Class.Id == 0)
//                {
//                    SqlClass.Title = Class.Title;
//                    SqlClass.Description = Class.Description;
//                    SqlClass.OredrNo = Class.OredrNo;
//                    SqlClass.Status = (int)IsPublished.Yes;
//                    SqlClass.dtmAdd = DateTime.UtcNow;
//                    SqlClass.IpAddress = Common_Static.IPAddress();
//                    SqlClass.dtmUpdate = DateTime.UtcNow;
//                    SqlClass = new MasterClassRepository().CreateAsync(SqlClass);
//                    ObjClass.Id = SqlClass.Id;
//                }
//                else
//                {
//                    var dbSubject = new MasterClassRepository().GetByIdAsync(Class.Id);

//                    var item = new PrachiIndia.Sql.MasterClass
//                    {
//                        IpAddress = Common_Static.IPAddress(),
//                        dtmAdd = dbSubject.dtmAdd,
//                        Title = Class.Title,
//                        Description = Class.Description,
//                        OredrNo = Class.OredrNo,
//                        dtmUpdate = DateTime.UtcNow,
//                        Status = dbSubject.Status,
//                        Id = dbSubject.Id,

//                    };
//                    var reult = new MasterClassRepository().UpdateAsync(item);
//                    ObjClass.Id = reult.Id;
//                }

//            }
//            catch (Exception ex)
//            {
//                ObjClass = new MasterClasses();
//            }
//            return Json(ObjClass);
//        }

//        public async Task<JsonResult> UpdateStatusClass(int Id, string type, IsPublished act = IsPublished.All)
//        {
//            var result = 0;
//            var board = new MasterClassRepository();
//            Sql.MasterClass bord = new Sql.MasterClass();
//            var ClassModel = board.GetByIdAsync(Id);
//            ClassModel.dtmUpdate = DateTime.UtcNow;
//            if (ClassModel.Status == IsPublished.Yes.GetHashCode())
//            {
//                ClassModel.Status = Convert.ToInt16(IsPublished.No);
//            }
//            else
//            {
//                ClassModel.Status = Convert.ToInt16(IsPublished.Yes);
//            }
//            result = board.UpdateAsyncStatus(ClassModel);
//            return Json(result);
//        }
//        #endregion

//        #region "Boards"
//        public ActionResult BoardManager()
//        {
//            var boards = new MasterSubjectRepository().GetAll().OrderBy(t => t.OredrNo);
//            return View(boards);
//        }
//        public ActionResult Boards()
//        {
//            return View();
//        }
//        public ContentResult GetAllBoards()
//        {

//            List<MasterBoards> lst = new List<MasterBoards>();
//            if (Request.IsAjaxRequest())
//            {
//                var MasterSubjectRepository = new MasterBoardRepository();
//                var lstSubject = MasterSubjectRepository.GetAll();

//                if (lstSubject != null)
//                {
//                    try
//                    {
//                        var result = from c in lstSubject
//                                     orderby c.OredrNo ascending
//                                     select new
//                                     {
//                                         c.Id,
//                                         Title = c.Title,
//                                         CreateDate = c.dtmAdd,
//                                         Image = c.Image,
//                                         DisplayOrder = c.OredrNo,
//                                         Description = c.Description,
//                                         Status = c.Status
//                                     };
//                        //lst = (result as IEnumerable<MasterSubject>).Cast<MasterSubject>().ToList();
//                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//                        return new ContentResult()
//                        {
//                            Content = serializer.Serialize(result),
//                            ContentType = "application/json",
//                        };
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }
//            return null;
//        }

//        [HttpPost, ValidateInput(false)]
//        public async Task<JsonResult> CreateMasterBoards(MasterBoards Boards)
//        {
//            var SqlBoards = new PrachiIndia.Sql.MasterBoard();
//            var ObjBoards = new MasterBoards();
//            try
//            {
//                if (Boards.Id == 0)
//                {
//                    SqlBoards.Title = Boards.Title;
//                    SqlBoards.Description = Boards.Description;
//                    SqlBoards.OredrNo = Boards.OredrNo;
//                    SqlBoards.Status = (int)IsPublished.Yes;
//                    SqlBoards.dtmAdd = DateTime.UtcNow;
//                    SqlBoards.IpAddress = Common_Static.IPAddress();
//                    SqlBoards.dtmUpdate = DateTime.UtcNow;
//                    SqlBoards = new MasterBoardRepository().CreateAsync(SqlBoards);
//                    Boards.Id = SqlBoards.Id;
//                }
//                else
//                {
//                    var dbBoards = new MasterBoardRepository().GetByIdAsync(Boards.Id);

//                    var item = new PrachiIndia.Sql.MasterBoard
//                    {
//                        IpAddress = Common_Static.IPAddress(),
//                        dtmAdd = dbBoards.dtmAdd,
//                        Title = Boards.Title,
//                        Description = Boards.Description,
//                        OredrNo = Boards.OredrNo,
//                        dtmUpdate = DateTime.UtcNow,
//                        Status = dbBoards.Status,
//                        Id = dbBoards.Id,

//                    };
//                    SqlBoards = new MasterBoardRepository().UpdateAsync(item);

//                    Boards.Id = SqlBoards.Id;
//                }

//            }
//            catch (Exception ex)
//            {
//                ObjBoards = new MasterBoards();
//            }
//            return Json(Boards);
//        }

//        public async Task<JsonResult> UpdateStatusBoards(int Id, string type, IsPublished act = IsPublished.All)
//        {
//            var result = 0;
//            var Board = new MasterBoardRepository();
//            Sql.MasterBoard bord = new Sql.MasterBoard();
//            var BoardModel = Board.GetByIdAsync(Id);
//            BoardModel.dtmUpdate = DateTime.UtcNow;
//            if (BoardModel.Status == IsPublished.Yes.GetHashCode())
//            {
//                BoardModel.Status = Convert.ToInt16(IsPublished.No);
//            }
//            else
//            {
//                BoardModel.Status = Convert.ToInt16(IsPublished.Yes);
//            }
//            result = Board.UpdateAsyncStatus(BoardModel);
//            return Json(result);
//        }
//        #endregion

//        #region "Books"
//        public ActionResult Books()
//        {
//            return View();
//        }
//        public ContentResult GetAllBooks(int idSubject, int idSeries)
//        {

//            List<Books> lst = new List<Books>();

//            if (Request.IsAjaxRequest())
//            {
//                var MasterSubjectRepository = new CatalogRepository();
//                //var lstBooks = MasterSubjectRepository.GetByID(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries).ToList();
//                IQueryable<tblCataLog> Query = MasterSubjectRepository.GetAll();
//                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries);
//                var lstBooks = Query.ToList();
//                if (lstBooks != null)
//                {
//                    try
//                    {
//                        var result = from c in lstBooks
//                                     orderby c.Title ascending
//                                     select new
//                                     {

//                                         Id = c.Id,
//                                         //SubjectId = c.SubjectId,
//                                         //SeriesId = c.SeriesId,
//                                         Title = c.Title,
//                                         Author = c.Author,
//                                         //ISBN = c.ISBN,
//                                         //Edition = c.Edition,
//                                         //Price = c.Price,
//                                         //Discount = c.Discount,
//                                         //Ebook = c.Ebook,
//                                         //MultiMedia = c.MultiMedia,
//                                         //Solutions = c.Solutions,                                         
//                                         //Description = c.Description,
//                                         CreateDate = c.dtmAdd,
//                                         //dtmUpdate = c.dtmUpdate,
//                                         //dtmDelete = c.dtmDelete,
//                                         Status = c.Status,
//                                         //IpAddress = c.IpAddress
//                                     };
//                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//                        return new ContentResult()
//                        {
//                            Content = serializer.Serialize(result),
//                            ContentType = "application/json",
//                        };
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }
//            return null;
//        }
//        public ContentResult getSeries(int idSubject)
//        {

//            List<SeriesModel> lst = new List<SeriesModel>();
//            if (Request.IsAjaxRequest())
//            {
//                var MasterSubjectRepository = new MasterSeriesRepositories();
//                var lstBooks = MasterSubjectRepository.GetAll().Where(t => t.SubjectId == idSubject);

//                if (lstBooks != null)
//                {
//                    try
//                    {
//                        var result = from c in lstBooks
//                                     orderby c.Title ascending
//                                     select new
//                                     {
//                                         Id = c.Id,
//                                         Title = c.Title,

//                                     };
//                        var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
//                        return new ContentResult()
//                        {
//                            Content = serializer.Serialize(result),
//                            ContentType = "application/json",
//                        };
//                    }
//                    catch (Exception ex)
//                    {

//                    }
//                }
//            }
//            return null;
//        }

//        public async Task<JsonResult> CreateMasterBooks(Books Books)
//        {
//            var SqlBooks = new PrachiIndia.Sql.tblCataLog();
//            var ObjBooks = new Books();
//            try
//            {
//                if (Books.Id == 0)
//                {
//                    SqlBooks.SubjectId = Books.SubjectId;
//                    SqlBooks.SeriesId = Books.SeriesId;
//                    SqlBooks.Title = Books.Title;
//                    SqlBooks.Author = Books.Author;
//                    SqlBooks.ISBN = Books.ISBN;
//                    SqlBooks.Edition = Books.Edition;
//                    SqlBooks.Discount = Books.Discount;
//                    SqlBooks.Ebook = Books.Ebook;
//                    SqlBooks.MultiMedia = Books.MultiMedia;
//                    SqlBooks.Solutions = Books.Solutions;
//                    SqlBooks.Description = Books.Description;
//                    SqlBooks.Status = (int)IsPublished.Yes;
//                    SqlBooks.dtmAdd = DateTime.UtcNow;
//                    SqlBooks.IpAddress = Common_Static.IPAddress();
//                    SqlBooks.dtmUpdate = DateTime.UtcNow;
//                    SqlBooks.dtmDelete = DateTime.UtcNow;

//                    SqlBooks = new CatalogRepository().CreateAsync(SqlBooks);
//                    Books.Id = SqlBooks.Id;
//                }
//                else
//                {
//                    var dbBooks = new CatalogRepository().GetByIdAsync(Books.Id);

//                    var item = new PrachiIndia.Sql.tblCataLog
//                    {
//                        SubjectId = Books.SubjectId,
//                        SeriesId = Books.SeriesId,
//                        Title = Books.Title,
//                        Author = Books.Author,
//                        ISBN = Books.ISBN,
//                        Edition = Books.Edition,
//                        Discount = Books.Discount,
//                        Ebook = Books.Ebook,
//                        MultiMedia = Books.MultiMedia,
//                        Solutions = Books.Solutions,
//                        Description = Books.Description,
//                        Status = (int)IsPublished.Yes,
//                        dtmAdd = DateTime.UtcNow,
//                        IpAddress = Common_Static.IPAddress(),
//                        dtmUpdate = DateTime.UtcNow,
//                        dtmDelete = DateTime.UtcNow,

//                    };
//                    SqlBooks = new CatalogRepository().UpdateAsync(item);

//                    Books.Id = SqlBooks.Id;
//                }

//            }
//            catch (Exception ex)
//            {
//                ObjBooks = new Books();
//            }
//            return Json(ObjBooks);
//        }

//        public async Task<JsonResult> UpdateStatusBooks(int Id, string type, IsPublished act = IsPublished.All)
//        {
//            var result = 0;
//            var Books = new CatalogRepository();
//            Sql.tblCataLog bord = new Sql.tblCataLog();
//            var BoardModel = Books.GetByIdAsync(Id);
//            BoardModel.dtmUpdate = DateTime.UtcNow;
//            if (BoardModel.Status == IsPublished.Yes.GetHashCode())
//            {
//                BoardModel.Status = Convert.ToInt16(IsPublished.No);
//            }
//            else
//            {
//                BoardModel.Status = Convert.ToInt16(IsPublished.Yes);
//            }
//            result = Books.UpdateAsyncStatus(BoardModel);
//            return Json(result);
//        }
//        #endregion
//        #region Start Book Update With Order Display Using Catalouge
//        public ActionResult BookOrder()
//        {
//            return View();
//        }
//        #endregion

//        public ActionResult TestView(HttpPostedFileBase files)
//        {
//            if (files != null && files.ContentLength > 0)
//            {
//                byte[] fileData = null;
//                var filename = "";
//                using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
//                {
//                    filename = Request.Files[0].FileName;
//                    fileData = binaryReader.ReadBytes(Request.Files[0].ContentLength);
//                }
//                string byteArrayString = Encoding.ASCII.GetString(fileData);
//                var model = new FileModel
//                {
//                    Image = byteArrayString,
//                    fileName = filename
//                };

//                using (var client = new HttpClient())
//                {

//                    client.BaseAddress = new Uri("http://satish.cloudindiatechnology.com/");
//                    var response = client.PostAsJsonAsync("api/values/UploadImages", model).Result;
//                    if (response.IsSuccessStatusCode)
//                    {
//                        Console.Write("Success");
//                    }
//                    else
//                        Console.Write("Error");
//                }

//            }

//            return View();
//        }
//    }

//    public class FileModel
//    {
//        public string contentType { get; set; }

//        public string Image { get; set; }

//        public string fileName { get; set; }

//    }
//}