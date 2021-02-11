using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PrachiIndia.Sql;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Controllers
{
    public class SubscriptionsController : Controller
    {
        // GET: Subscription
        //subscription plan 
        public ActionResult Index()
        {
            var subscription = new SubscriptionRepository();
            var subscriptionplan = subscription.GetAll().ToList();
            filedropdwon();
            return View(subscriptionplan);
        }
        //from here we post it for payment for subscription use
        public async Task<ActionResult> _EditAdress(int ids = 0)
        {

            long SubId = 0;
            var Sub = 0;
            Subscription model = new Subscription();
            if (ids == 0)
            {
                if (TempData["SuscriptionId"].ToString() != null)
                {
                    var SubSId = TempData["SuscriptionId"];
                    SubId = Convert.ToInt64(SubSId);
                }
            }
            else
            {
                SubId = ids;
            }
            var user = new AspNetUserRepository();
            var userdetail = user.GetUser(User.Identity.GetUserId());
            var subscriptionrepo = new SubscriptionRepository();
            var AllSubscription = subscriptionrepo.GetAll().ToList();
            var SubscriptionList = subscriptionrepo.GetById(SubId);
            var transubscription = new SubscriptionTrnRepostory();
            var trnsubscription = new Subscription
            {
                mstSubscriptionId = SubscriptionList.SubscriptionI,
                UserId = userdetail.Id,
                Class = SubscriptionList.ClassId,
                Board = SubscriptionList.BoardId,
                Amount = SubscriptionList.Amount,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                SubscriptionType = SubscriptionList.SubscriptionType
            };
            var results = transubscription.CreateAsync(trnsubscription);
            #region Post To Payment
            if (results.SubscriptionId > 0)
            {
                var merchantKey = ConfigurationManager.AppSettings["Key"].ToString(CultureInfo.InvariantCulture);
                var merchantSalt = ConfigurationManager.AppSettings["Salt"].ToString(CultureInfo.InvariantCulture);
                var productname = SubscriptionList.PlanName;
                var orderRequest = new Order
                {
                    Amount = results.Amount,
                    Email = userdetail.Email,
                    Name = userdetail.FirstName,
                    Phone = userdetail.PhoneNumber,
                    ProductInfo = productname,
                    TransactionId = PayUMoney.GenerateTxnId(),
                    UserId = userdetail.Id,
                    IsSubscription = true
                };
                var hashString = merchantKey + "|" + orderRequest.TransactionId + "|" + orderRequest.Amount + "|" + orderRequest.ProductInfo + "|" + orderRequest.Name + "|" + orderRequest.Email + "|||||||||||" + merchantSalt;
                var hash = PayUMoney.Generatehash512(hashString);
                orderRequest.RequestLog = hashString;
                orderRequest.RequestHash = hash;
                orderRequest.Status = PayUMoney.PaymentStatus.Initiated.GetHashCode();
                orderRequest.CreatedDate = DateTime.UtcNow;
                orderRequest.UpdatedDate = DateTime.UtcNow;
                var order = new OrderRepository().CreateAsync(orderRequest);//await
                var OrderId = new OrderRepository().GetAll().Where(x => x.RequestHash == hash).Select(x => x.Id).ToList();
                Int32 id = 0;
                foreach (var a in OrderId)
                {
                    id = Convert.ToInt32(a);
                }
                var myremotepost = new RemotePost();
                myremotepost.Url = ConfigurationManager.AppSettings["paymentgatewayUrl"].ToString(CultureInfo.InvariantCulture);
                myremotepost.Add("key", merchantKey);
                myremotepost.Add("txnid", orderRequest.TransactionId);
                myremotepost.Add("amount", orderRequest.Amount.ToString());
                myremotepost.Add("productinfo", orderRequest.ProductInfo);
                myremotepost.Add("firstname", orderRequest.Name);
                myremotepost.Add("phone", orderRequest.Phone);
                myremotepost.Add("email", orderRequest.Email);
                myremotepost.Add("surl", ConfigurationManager.AppSettings["returnUrls"].ToString(CultureInfo.InvariantCulture));
                myremotepost.Add("furl", ConfigurationManager.AppSettings["returnUrls"].ToString(CultureInfo.InvariantCulture));
                //comment due to here we use pay u biz so in this service provider not required
                myremotepost.Add("hash", hash);
                myremotepost.Post();
            }
            #endregion
            filedropdwon();
            return View("Index", AllSubscription);
        }
        //after subscription transaction it will redirect it to payment stattus page.
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
                    if (string.IsNullOrWhiteSpace(paymentRowResponse))
                        paymentRowResponse = form[i];
                    else
                        paymentRowResponse = paymentRowResponse + "|" + form[i];
                }
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                var transactionId = form["txnid"];
                var OrderRepository = new OrderRepository();
                var orderResponse = OrderRepository.SearchFor(t => t.TransactionId == transactionId).FirstOrDefault();
                if (orderResponse == null)
                    return null;
                var status = form["status"];
                var sUBSCRIPTIONiD = form["subId"];
                orderResponse.AddtionalCharge = form["additionalCharges "];
                orderResponse.Error = form["Error"];
                orderResponse.PayUMoneyId = form["payuMoneyId"];
                orderResponse.PGType = form["PG_TYPE"];
                orderResponse.ResponseHas = form["hash"];
                orderResponse.RequestLog = paymentRowResponse;
                orderResponse.TransactionId = form["txnid"];
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
                    //}
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
                    Email = customer.Email,
                    Address = customer.Address,
                    Country = countryid,
                    City = CityId,
                    State = StateId,
                    PinCode = customer.PinCode,
                };
                if (customer != null)
                {
                    responseObject.CustomerName = customer.FirstName;
                    responseObject.TransactionId = orderResponse.TransactionId;
                    responseObject.TotalAmount = Convert.ToDecimal(orderResponse.Amount);
                    var products = new List<ProductItem>();
                    var product = new ProductItem
                    {
                        Price = String.Format("{0:0.00}", orderResponse.Amount),
                        Quantity = 1,
                        Title = orderResponse.ProductInfo,
                        Flag = status.ToLower()
                    };
                    products.Add(product);
                    responseObject.Products = products;
                    ViewBag.customer = customers;
                    ViewData["customer"] = customers;
                    Oredermail(customers, responseObject);
                }
            }
            catch (Exception ex)
            { }
            return View(responseObject);
        }
        //after subscription transaction mail suit from sysytem to user  with mail template using MailTemplate method.
        public void Oredermail(AspNetUser customer, Response responseObject)
        {
            var toMail = ConfigurationManager.AppSettings["MailTo"].ToString(CultureInfo.InvariantCulture);
            var subject = "Mail from Website";
            var message = MailTemplate(customer, responseObject);
            MailServiece.Mail.SendMail(customer.Email, "", toMail, subject, message);//  SendMail(subject, message, customer.Email, toMail);
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
            textMessage += "<img src='http://prachiindia.com/img/logos/logo.png' width='200' alt='logo' />";
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
        #region country state and city master 
        private void filedropdwon()
        {
            var list = GetAllCountry();
            var State = GetSates();
            var City = GetCities();
            ViewData["list"] = list;
            ViewData["State"] = State;
            ViewData["City"] = City;
        }
        private object GetAllCountry()
        {
            var CountryRepo = new CountryRepositories();
            var CountryList = CountryRepo.GetAll().ToList();
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();

            foreach (var item in CountryList)
            {
                list.Add(new System.Web.Mvc.SelectListItem { Text = item.Name, Value = item.CountryId.ToString() });
            }
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
        #endregion
    }
}