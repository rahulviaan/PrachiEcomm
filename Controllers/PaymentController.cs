using Microsoft.AspNet.Identity;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Portal.Framework;
using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using PrachiIndia.Sql;
using System.Threading.Tasks;
using PrachiIndia.Portal.Helpers;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using PrachiIndia.Web.Areas.Model;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PrachiIndia.Portal.Models;
//using PayPal.Api;

namespace PrachiIndia.Web.Controllers
{
    //[Authorize(Roles = "User,School")]
    [Authorize]
    public class PaymentController : Controller
    {
        //this will transfar you Checkout page where user ca go for payment
        [HttpGet]
        public ActionResult Index()
        {
            AspNetUser model = new AspNetUser();
            TblShipingAddress modelss = new TblShipingAddress();
            var AspnetRepository = new AspNetUserRepository();
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            // var ShippingRepo = new ShippingAddressRepository();
            //IQueryable<TblShipingAddress> querys = ShippingRepo.GetAll();
            IQueryable<AspNetUser> query = AspnetRepository.GetAll();
            var userId = User.Identity.GetUserId();
            // var listshipp = querys.Where(s => s.UserId == userId).ToList();
            var list = query.Where(s => s.Id == userId).ToList();
            // List<TblShipingAddress> listss = new List<TblShipingAddress>();
            //foreach (var item in listshipp)
            //{

            //    modelss.UserName = item.UserName;
            //    modelss.Email = item.Email;
            //    modelss.Address = item.Address;

            //    modelss.Countries = item.Countries;
            //    modelss.States = item.States;
            //    modelss.Cities = item.Cities;

            //    modelss.Country = item.Country;
            //    modelss.State = item.State;
            //    modelss.City = item.City;
            //    modelss.PinCode = item.PinCode;
            //    modelss.Phone = item.Phone;
            //    listss.Add(modelss);
            //}
            foreach (var models in list)
            {

                model.UserName = models.UserName;
                model.Email = models.Email;
                model.Address = models.Address;
                model.FirstName = models.FirstName;
                model.Country = models.Country;
                model.State = models.State;
                model.City = models.City;
                model.CountryId = models.CountryId;
                model.StateId = models.StateId;
                model.CityId = models.CityId;
                model.PinCode = models.PinCode;
                model.PhoneNumber = models.PhoneNumber;
                list[0].State = models.State;
                list[0].Country = models.Country;
                list[0].City = models.City;

            }
            //Changed By Rahul shipping address is taken from usermaster
            Session["Shipping"] = list;
            var lists = GetAllCountry();
            var state = GetSates();
            var City = GetCities();
            ViewData["State"] = state;
            ViewData["City"] = City;
            ViewData["list"] = lists;
            return View(model);
        }
        //this methode redirect u to payumony page where user put their card detail for payment
        [HttpPost]
        public async Task<ActionResult> Index(AspNetUser model, string paymentMethod)
        {
            paymentMethod = PGType.Domestic;
            Session["PaymentProcesses"] = true;
            var aspNetUserRepository = new AspNetUserRepository();
            try
            {
                if (model == null)
                {
                    return RedirectToAction("Cart", "Catalogue");
                }

                AspNetUser users = new AspNetUser();
                var userId = User.Identity.GetUserId();
                var user = new AspNetUserRepository().GetUser(userId);
                if (user != null)
                {
                    users.Id = user.Id;
                    users.FirstName = user.FirstName;
                    users.Email = user.Email;
                    users.PhoneNumber = user.PhoneNumber;
                    users.PinCode = user.PinCode;
                    users.Country = user.Country;
                    users.State = user.State;
                    users.City = user.City;
                    users.Address = user.Address;
                }
                //if (string.IsNullOrWhiteSpace(paymentMethod))
                //{
                //    var list = new List<Sql.AspNetUser>();
                //    list.Add(user);
                //    Session["Shipping"] = list;
                //    ViewData["State"] = GetSates();
                //    ViewData["City"] = GetCities();
                //    ViewData["list"] = GetAllCountry();
                //    ModelState.AddModelError("paymentMethod", "Please select your card type");
                //    return View(user);
                //}
                var cartRepository = new CartRepository();
                var carts = cartRepository.FindByCarts(userId).Where(t => t.IsWishList != true).ToList();
                var results = new List<OrderProduct>();
                var context = new dbPrachiIndia_PortalEntities();
                foreach (var cartItem in carts)
                {
                    var taxRate = context.GSTTaxLists.FirstOrDefault(t => t.Id == cartItem.tblCataLog.Tax);
                    var rate = taxRate == null ? 0 : taxRate.Rate;
                    var item = new OrderProduct
                    {
                        Quantity = cartItem.Quntity,
                        Title = cartItem.tblCataLog.Title,
                        ItemId = cartItem.ItemId,
                        Discount = Convert.ToInt32(cartItem.Discount),
                        BookType = cartItem.BookType,
                        CreatedDate = DateTime.Now,
                    };
                    var price = 0M;
                    if (cartItem.BookType == BookType.PBook & !cartItem.IsSolution ?? false)
                    {
                        price = cartItem.tblCataLog.PrintPrice ?? 0;
                    }
                    else if (cartItem.BookType == BookType.EBook & !cartItem.IsSolution ?? false)
                    {
                        price = cartItem.tblCataLog.EbookPrice ?? 0;
                    }
                    else if (cartItem.BookType == BookType.Both & !cartItem.IsSolution ?? false)
                    {
                        var Pprice = cartItem.tblCataLog.PrintPrice + cartItem.tblCataLog.EbookPrice;
                        price = Pprice ?? 0;
                    }
                    else if (cartItem.IsSolution ?? false)
                    {
                        price = cartItem.tblCataLog.SolutionPrice ?? 0;
                    }

                    var productPrice = price * cartItem.Quntity ?? 0;
                    item.Discount = (int)(cartItem.Discount * cartItem.Quntity ?? 0);

                    var subtotal = (productPrice - item.Discount) ?? 0;
                    item.Price = price;
                    item.Tax = (subtotal * rate) / 100;
                    item.TotalAmount = subtotal + item.Tax;
                    results.Add(item);

                }



                if (results == null && !results.Any())
                {
                    return RedirectToAction("Cart", "Catalogue");
                }
                var totalPrice = 0M;
                var name = string.Empty;
                foreach (var item in results)
                {
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = item.Title;
                    }
                    else
                    {
                        name = name + " " + item.Title;
                    }
                    //var discount = item.Discount * item.Quantity;
                    //var price = item.Price;

                    //var subtotal = price * item.Quantity;
                    totalPrice = totalPrice + item.TotalAmount ?? 0;

                }
                var productname = name;
                var orderRequest = new Sql.Order
                {
                    Amount = totalPrice,
                    DiscountPer = 0,
                    TotalAmount = totalPrice,
                    Email = user.Email,
                    Name = user.FirstName,
                    Phone = user.PhoneNumber,
                    ProductInfo = productname,
                    TransactionId = PayUMoney.GenerateTxnId(),
                    UserId = user.Id
                };
                if (PrachiIndia.Portal.Framework.PGType.International == paymentMethod)
                {
                    orderRequest.RequestLog = "N/A";
                    orderRequest.RequestHash = "N/A";
                    orderRequest.Status = PayUMoney.PaymentStatus.Initiated.GetHashCode();
                    orderRequest.CreatedDate = DateTime.UtcNow;
                    orderRequest.UpdatedDate = DateTime.UtcNow;
                    var order = new OrderRepository().CreateAsync(orderRequest);//await
                    var orderProductRepository = new OrderProductRepository();
                    var OrderProductss = new List<Sql.OrderProduct>();
                    foreach (var item in results)
                    {
                        if (item.ItemId > 0)
                        {
                            var orderProducts = new OrderProduct
                            {
                                CreatedDate = DateTime.UtcNow,
                                UpdatedDate = DateTime.UtcNow,
                                ItemId = item.ItemId,
                                OrderId = order.Id,
                                Title = item.Title,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                Discount = item.Discount,
                                BookType = item.BookType,
                                Tax = item.Tax,
                                TotalAmount = item.TotalAmount
                            };
                            OrderProductss.Add(orderProducts);
                        }
                    }
                    orderProductRepository.Create(OrderProductss);
                    Session["OrderId"] = order.Id;
                    return RedirectToAction("Index", "PaymentWithPayPal");
                }
                else
                {
                    #region Post To Payment
                    var merchantKey = ConfigurationManager.AppSettings["Key"].ToString(CultureInfo.InvariantCulture);
                    var merchantSalt = ConfigurationManager.AppSettings["Salt"].ToString(CultureInfo.InvariantCulture);
                    var IsLive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLive"].ToString(CultureInfo.InvariantCulture));
                    var serviceProvider = "payu_paisa";

                    var productInfo = orderRequest.ProductInfo.Length > 95 ? orderRequest.ProductInfo.Substring(0, 95) : orderRequest.ProductInfo;
                    var firstname = orderRequest.Name.Length > 18 ? orderRequest.Name.Substring(0, 18) : orderRequest.Name;
                    var hashString = merchantKey + "|" + orderRequest.TransactionId + "|" + orderRequest.Amount + "|" + productInfo + "|" + firstname + "|" + orderRequest.Email + "|||||||||||" + merchantSalt;
                    var hash = PayUMoney.Generatehash512(hashString);
                    orderRequest.RequestLog = hashString;
                    orderRequest.RequestHash = hash;
                    orderRequest.Status = PayUMoney.PaymentStatus.Initiated.GetHashCode();
                    orderRequest.CreatedDate = DateTime.UtcNow;
                    orderRequest.UpdatedDate = DateTime.UtcNow;
                    var order = new OrderRepository().CreateAsync(orderRequest);//await


                    var orderProductRepository = new OrderProductRepository();
                    var OrderProductss = new  List<Sql.OrderProduct>();
                    foreach (var item in results)
                    {
                        if (item.ItemId > 0)
                        {
                            var orderProducts = new OrderProduct
                            {
                                CreatedDate = DateTime.UtcNow,
                                UpdatedDate = DateTime.UtcNow,
                                ItemId = item.ItemId,
                                OrderId = order.Id,
                                Title = item.Title,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                Discount = item.Discount,
                                BookType = item.BookType,
                                Tax = item.Tax,
                                TotalAmount = item.TotalAmount
                            };
                            OrderProductss.Add(orderProducts);
                        }
                    }
                    orderProductRepository.Create(OrderProductss);

                    var myremotepost = new RemotePost();
                    myremotepost.Url = ConfigurationManager.AppSettings["paymentgatewayUrl"].ToString(CultureInfo.InvariantCulture);
                    myremotepost.Add("key", merchantKey);
                    myremotepost.Add("txnid", orderRequest.TransactionId);
                    myremotepost.Add("amount", orderRequest.Amount.ToString());
                    myremotepost.Add("productinfo", productInfo);
                    myremotepost.Add("firstname", firstname);
                    myremotepost.Add("phone", orderRequest.Phone);
                    myremotepost.Add("email", orderRequest.Email);
                    myremotepost.Add("surl", ConfigurationManager.AppSettings["returnUrl"].ToString(CultureInfo.InvariantCulture));
                    myremotepost.Add("furl", ConfigurationManager.AppSettings["returnUrl"].ToString(CultureInfo.InvariantCulture));
                    //comment due to here we use pay u biz so in this service provider not required

                    //if (!Request.IsLocal)
                    //    myremotepost.Add("service_provider", serviceProvider);
                    if (IsLive)
                    {
                        myremotepost.Add("service_provider", serviceProvider);
                    }
                    myremotepost.Add("hash", hash);
                    myremotepost.Post();
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            //var lists = GetAllCountry();
            //var state = GetSates();
            //var City = GetCities();
            //ViewData["State"] = state;
            //ViewData["City"] = City;
            //ViewData["list"] = lists;
            //
            return View();
        }
        //Order From App
        [AllowAnonymous]
        public ActionResult OrderPayment(int id)
        {

            var repoorder = new OrderRepository();
            var order = repoorder.GetById(id);
            if (order != null && (order.Status != PayUMoney.PaymentStatus.Success.GetHashCode()))
            {

                try
                {
                    var amount = (decimal)order.TotalAmount;
                    var merchantKey = ConfigurationManager.AppSettings["Key"].ToString(CultureInfo.InvariantCulture);
                    var merchantSalt = ConfigurationManager.AppSettings["Salt"].ToString(CultureInfo.InvariantCulture);
                    var serviceProvider = "payu_paisa";
                    var myremotepost = new RemotePost();
                    myremotepost.Url = ConfigurationManager.AppSettings["paymentgatewayUrl"].ToString(CultureInfo.InvariantCulture);
                    myremotepost.Add("key", merchantKey);
                    myremotepost.Add("txnid", order.TransactionId);
                    myremotepost.Add("amount", amount.ToString("N2"));
                    myremotepost.Add("productinfo", order.ProductInfo);
                    myremotepost.Add("firstname", order.Name);
                    myremotepost.Add("phone", order.Phone);
                    myremotepost.Add("email", order.Email);
                    myremotepost.Add("surl", ConfigurationManager.AppSettings["returnUrlApp"].ToString(CultureInfo.InvariantCulture));

                    myremotepost.Add("furl", ConfigurationManager.AppSettings["returnUrlApp"].ToString(CultureInfo.InvariantCulture));
                    //comment due to here we use pay u biz so in this service provider not required
                    myremotepost.Add("service_provider", serviceProvider);
                    myremotepost.Add("hash", order.RequestHash);
                    myremotepost.Post();
                }
                catch (Exception ex)
                {
                }
            }
            return View();
        }
        [HttpGet]
        public ContentResult _CartProduct()
        {
            var cartRepository = new CartRepository();
            var itemRepository = new CatalogRepository();
            var ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
            var carts = cartRepository.FindByCarts(User.Identity.GetUserId()).Where(t => t.IsWishList != true).ToList();//"c667741a-27d1-4438-8662-cc51a784e5d6").ToList();
            var results = (from cartItem in carts
                           select new ProductItem
                           {
                               Id = cartItem.Id,
                               Image = ImageUrl + cartItem.tblCataLog.Image,
                               Price = cartItem.tblCataLog.Price == null ? "0.00" : String.Format("{0:0.00}", cartItem.tblCataLog.Price),
                               EbookPrice = cartItem.tblCataLog.Price == null ? "0.00" : String.Format("{0:0.00}", cartItem.tblCataLog.Price),
                               Quantity = Convert.ToInt32(cartItem.Quntity),
                               Title = cartItem.tblCataLog.Title,
                               ItemId = cartItem.ItemId ?? 0,
                               Discount = cartItem.Discount ?? 0,
                               BookType = cartItem.BookType.ToString(),
                           }).ToList();
            ViewData["list"] = results;
            //return PartialView();
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
            return new ContentResult()
            {
                Content = serializer.Serialize(results),
                ContentType = "application/json",
            };
        }
        //this partial is use for edit user billing adress detail.
        [HttpPost]
        public ActionResult _EditAdress(AspNetUser model)
        {
            var aspNetUserRepository = new AspNetUserRepository();
            int result = 0;
            try
            {
                var country = new CountryRepositories().FindByItemId(model.CountryId ?? 0);
                dynamic state = null;
                dynamic city = null;
                if (country != null && country.CountryId == 86)
                {
                    state = new StateRepositories().FindByItemId(model.StateId ?? 0);
                    city = new CityRepositories().FindByItemId(model.CityId ?? 0);
                }
                var userId = User.Identity.GetUserId();
                // var aspnetUser = new AspNetUserRepository().FindByIdAsync(userId);
                var context = new dbPrachiIndia_PortalEntities();
                var aspnetUser = context.AspNetUsers.First(t => t.Id == userId);
                aspnetUser.FirstName = model.FirstName;
                aspnetUser.Email = model.Email;
                aspnetUser.PhoneNumber = model.PhoneNumber;
                aspnetUser.PinCode = model.PinCode;
                aspnetUser.Country = country != null ? country.Name : string.Empty;
                aspnetUser.State = state != null ? state.StateName : string.Empty;
                aspnetUser.City = city != null ? city.CityName : string.Empty;

                aspnetUser.CountryId = country != null ? country.CountryId : 0;
                aspnetUser.StateId = state != null ? state.StaeteId : 0;
                aspnetUser.CityId = city != null ? city.CityId : 0;

                if (city == null)
                {
                    aspnetUser.City = model.City;
                }
                if (state == null)
                {
                    aspnetUser.State = model.State;
                }


                aspnetUser.Address = model.Address;

                context.AspNetUsers.Attach(aspnetUser);
                context.Entry(aspnetUser).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                //aspNetUserRepository.UpdateAsync(aspnetUser);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");
        }
        #region country state city  work  
        private object GetAllCountry()
        {
            var CountryRepo = new CountryRepositories();
            var CountryList = CountryRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            foreach (var item in CountryList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.Name, Value = item.CountryId.ToString() });
            }
            list.Insert(0, new SelectListItem { Text = "Please select country", Value = "0" });
            return list;
        }
        [AllowAnonymous]
        public object GetSates()
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in StateList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.StateName, Value = item.StaeteId.ToString() });
            }
            return list;
        }

        [AllowAnonymous]
        public object GetCities()
        {
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in CityList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.CityName, Value = item.CityId.ToString() });
            }
            return list;
        }
        [AllowAnonymous]
        public JsonResult GetCity(string StateId)
        {
            var CityRepo = new CityRepositories();
            var CityList = CityRepo.GetAll().ToList();
            var district = from s in CityList.ToList()
                           where s.StateId == Convert.ToInt32(StateId)
                           select s;
            return Json(new SelectList(district.ToArray(), "CityId", "CityName"), JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult GetSate(string id)
        {
            var StateRepo = new StateRepositories();
            var StateList = StateRepo.GetAll().ToList();
            var district = from s in StateList.ToList()
                           where s.CountryId == Convert.ToInt32(id)
                           select s;
            return Json(new SelectList(district.ToArray(), "StaeteId", "StateName"), JsonRequestBehavior.AllowGet);
        }
        private void filedropdwon()
        {
            var list = GetAllCountry();
            ViewData["list"] = list;
        }
        #endregion
        //after payment payment status page wil come with user billing addresss and shipping address
        public async Task<ActionResult> PaymentStatus(FormCollection form)
        {
            var responseObject = new Response();
            try
            {
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                var paymentRowResponse = string.Empty;
                for (int i = 0; i < form.Count; i++)
                {
                    paymentRowResponse += form.AllKeys[i] + ": " + form[i] + " | ";
                }
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                var transactionId = form["txnid"];
                var OrderRepository = new OrderRepository();
                var orderResponse = OrderRepository.SearchFor(t => t.TransactionId == transactionId).FirstOrDefault();
                if (orderResponse == null)
                    return RedirectToAction("Cart", "Catalogue");
                var status = form["status"];
                orderResponse.AddtionalCharge = form["additionalCharges"];
                orderResponse.Error = form["Error"];
                orderResponse.PayUMoneyId = form["payuMoneyId"];
                orderResponse.PGType = form["PG_TYPE"];
                orderResponse.ResponseHas = form["hash"];
                orderResponse.ResponseLog = paymentRowResponse;
                orderResponse.TransactionId = transactionId;
                orderResponse.OrderId = form["mihpayid"];//Merchant Identification Number

                if (status.ToLower() == "success")
                {
                    #region payment check sum check
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = ConfigurationManager.AppSettings["Salt"] + "|" + status;
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (form[merc_hash_var] != null ? form[merc_hash_var] : "");
                    }
                    merc_hash = PayUMoney.Generatehash512(merc_hash_string).ToLower();
                    #endregion
                    responseObject.Status = "SUCCESS";
                    orderResponse.Status = PayUMoney.PaymentStatus.Success.GetHashCode();
                }
                else
                {
                    responseObject.Status = "FAILURE";
                    orderResponse.Error = orderResponse.Error + "(Hash value did not matched)";
                    orderResponse.Status = PayUMoney.PaymentStatus.Failure.GetHashCode();
                }
                orderResponse.UpdatedDate = DateTime.UtcNow;
                OrderRepository.UpdateAsync(orderResponse);
                var userId = User.Identity.GetUserId();
                var customer = new AspNetUserRepository().GetUser(userId);


                var customers = new AspNetUser
                {
                    Email = customer.Email,
                    Address = customer.Address,
                    Country = customer.Country,
                    City = customer.City,
                    State = customer.State,
                    PinCode = customer.PinCode,
                    PhoneNumber = customer.PhoneNumber
                };
                if (customer != null)
                {
                    responseObject.CustomerName = customer.FirstName;
                    responseObject.TransactionId = orderResponse.TransactionId;
                    responseObject.TotalAmount = Convert.ToDecimal(orderResponse.Amount);

                    var cartRepository = new CartRepository();
                    var products = new List<ProductItem>();
                    var userLibraries = new List<Sql.UserLibrary>();
                    var orderProducts = new OrderProductRepository().GetAll(orderResponse.Id).ToList();
                    foreach (var cartItem in orderProducts)
                    {
                        var product = new ProductItem
                        {
                            Price = cartItem.Price == null ? "0.00" : cartItem.Price.Value.ToString("0.00"),
                            Quantity = cartItem.Quantity ?? 0,
                            Title = cartItem.tblCataLog.Title,
                            Flag = status.ToLower(),
                            BookType = cartItem.BookType.ToString(),
                            Discount = cartItem.Discount ?? 0,
                            TaxRate = cartItem.Tax,
                            TotalAmount = cartItem.TotalAmount ?? 0,
                            IsSolution = Convert.ToBoolean (cartItem.tblCataLog.IsSolution)
                        };
                        products.Add(product);


                        ///
                        /// when be publish ebook then uncomment the code
                        /// 
                        if (cartItem.BookType == BookType.EBook.GetHashCode() || cartItem.BookType == BookType.Both.GetHashCode())
                        {
                            var userLibrary = new Sql.UserLibrary
                            {
                                BookId = cartItem.tblCataLog.Id,
                                CreatedDate = DateTime.UtcNow,
                                EPubName = cartItem.tblCataLog.Ebookname,
                                Id = Guid.NewGuid().ToString(),
                                UpdatedDate = DateTime.UtcNow,
                                UserId = userId,
                                EncriptionKey = cartItem.tblCataLog.EncriptionKey,
                                PublishDate = cartItem.tblCataLog.PublishedDate,

                            };
                            userLibraries.Add(userLibrary);
                            if (status.ToLower() == "success")
                            {
                                var context = new dbPrachiIndia_PortalEntities();
                                var carts = cartRepository.SearchFor(t => t.UserId == userId && t.IsWishList == false).ToList();
                                foreach (var cart in carts)
                                {
                                    cartRepository.DeleteAsync(cart);

                                    if (cart.IsSolution??false)
                                    {
                                        //var products = context.OrderProducts.Where(t => t.OrderId == order.Id).ToList();
                                        var usersolution = new tblUserDigitalBook();
                                        var book = context.tblDigitalBooks.Where(t => t.BookId == cartItem.ItemId).FirstOrDefault();
                                        if(book!= null) usersolution.DigitalBookId = book.Sno;
                                        else usersolution.DigitalBookId = cartItem.ItemId;
                                        usersolution.UserId = userId;
                                        usersolution.Status = true;
                                        usersolution.SubscriptionStartDate = DateTime.Now;
                                        usersolution.CreateDate = DateTime.Now;
                                        context.tblUserDigitalBooks.Add(usersolution);
                                        context.SaveChanges();
                                    }

                                }
                            }
                        }
                    }


                    if (userLibraries != null && userLibraries.Any())
                    {
                        new ReaderBookRepository().CreateAsync(userLibraries.ToList());
                    }
                    
                    responseObject.Products = products;
                    ViewBag.customer = customers;
                    ViewData["customer"] = customers;

                    if (status.ToLower() == "success")
                    {
                        var invoiceCopy = PrachiService.GenerateInvoice(orderResponse.Id, userId);
                        MailServiece.Mail.SendMail(invoiceCopy.Email, invoiceCopy.Subject, invoiceCopy.MailTemplate);
                        //Oredermail(customers, responseObject);
                    }
                }
            }
            catch (Exception ex)
            { Session.Remove("PaymentProcesses"); }
            Session.Remove("PaymentProcesses");
            return View(responseObject);
        }





        //after payment Email  will suit to user with payment invoice
        public void Oredermail(AspNetUser customer, Response responseObject)
        {

            var toMail = customer.Email;
            var BCC = ConfigurationManager.AppSettings["MailTo"].ToString(CultureInfo.InvariantCulture);
            var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString(CultureInfo.InvariantCulture);
            var subject = "Your order is confirmed";
            var message = MailTemplate(customer, responseObject);
            MailServiece.Mail.SendMail(customer.Email, "", toMail, subject, message);
            //var x = Portal.Framework.Utility.SendMail(subject, message, toMail, MailFrom, BCC);
        }
        static string MailTemplate(AspNetUser customer, Response response)
        {
            var textMessage = string.Empty;
            textMessage += "<table border='0' cellpadding='0' cellspacing='0' width='720' align='center' style='font-family: verdana; font-size: 12px; line-height: 18px; border: solid 1px #f1f1f1;' >";
            textMessage += "<tr>";
            textMessage += " <td style='padding: 10px; width: 700px;'>";
            textMessage += "<table border='0' cellpadding='0' cellspacing='0' width='100%'>";
            textMessage += "  <tr>";
            textMessage += "  <td align='left'>";
            textMessage += "   <h2>Thank you for your order.</h2>";
            if (response.Status == "FAILURE")
                textMessage += "<p style='font-size:14px;'>PAYMENT STATUS:<b style='color:#ae0910;'>" + response.Status + "</b></p>";
            else
                textMessage += "<p style='font-size:14px;'>PAYMENT STATUS:<b style='color:#16760B;'>" + response.Status + "</b></p>";
            textMessage += "  <h4>Order# " + response.TransactionId + "  <br />Date :   " + DateTime.Now.ToString("dd-MM-yyyy") + "</h4>";
            textMessage += "</td>";
            textMessage += " <td align='right'>";
            textMessage += "<img src='http://prachiindia.com/img/logo.png' width='194' alt='logo' />";
            textMessage += "   </td>";
            textMessage += "</tr>";
            textMessage += " <tr>";
            textMessage += "<td align='left' colspan='2'>";
            textMessage += " <h4 style='text-decoration:underline;'>Billing/Shipping Address</h4>";
            textMessage += "<p>Name: " + response.CustomerName + "<br/>Address: " + customer.Address + "<br /> City: " + customer.City + "<br /> State: " + customer.State + "<br /> Country: " + customer.Country + "<br /> Pincode: " + customer.PinCode + "<br/>Mobile: " + customer.PhoneNumber;
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
            if (response.Products != null && response.Products.Any())
            {
                decimal sum = 0;
                decimal totalDiscount = 0;
                foreach (var itemcart in response.Products)
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
    }
}