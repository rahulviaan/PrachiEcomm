
using DAL.Models.Entities;
using GleamTech.AspNet.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReadEdgeCore.Models;
using ReadEdgeCore.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReadEdgeCore.Utilities;
using GleamTech.DocumentUltimateExamples.AspNetCoreCS.Models;
using static ReadEdgeCore.Models.ReaderBookService;

namespace ReadEdgeCore.Controllers
{
    public class UserSubscriptionController : Controller
    {
        private readonly IUser _user;
        private readonly ILibrary _library;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IReaderBooks _readerBooks;
        private readonly IEbookReader _ebookReader;
        private readonly IConfiguration _iConfig;
        private readonly ErrorLog _errorLog;
        private readonly Subscription _subscription; 
        private readonly UserSubscription _userSubscription;
        private readonly RemotePost _remotePost;
        private readonly SubscriptionMail _subscriptionMail;
        private readonly SubscriptionPayment  _subscriptionPayment;
        private  AspNetUsers  _aspNetUsers;




        public UserSubscriptionController(IUser user, ILibrary library, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IReaderBooks readerBooks, IEbookReader ebookReader, IConfiguration iConfig, ErrorLog errorLog, Subscription subscription, UserSubscription userSubscription, RemotePost remotePost, SubscriptionMail subscriptionMail, SubscriptionPayment subscriptionPayment, AspNetUsers aspNetUsers)
        {
            _user = user;
            _library = library;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _ebookReader = ebookReader;
            _readerBooks = readerBooks;
            _iConfig = iConfig;
            _errorLog = errorLog;
            _userSubscription = userSubscription;
            _remotePost = remotePost;
            _subscriptionMail = subscriptionMail;
            _subscription = subscription;
            _subscriptionPayment = subscriptionPayment;
            _aspNetUsers = aspNetUsers;
        }
    
        // GET: UserSubscription
        //[Route("Subscription")]
        public ActionResult Index()
        {
            var IsVerified = _httpContextAccessor.HttpContext.Session.GetString("IsVerified");
            var otp = _httpContextAccessor.HttpContext.Session.GetString("OTP");
            ViewBag.AlreadyUser = true;
            if (otp == null || otp == "")
            {
                ViewBag.AlreadyUser = false;
            }
            if (IsVerified == "YES")
            {
                ViewBag.AlreadyUser = true;
            }
       
            return View();
        }
         
        public ActionResult payment(string Type,string Name="",string Email="",string ContactNumber="")
        {

            //var aspNetUserRepository = new AspNetUserRepository();
            try
            {


                var subscriptionMaster = _subscription.FindSubscriptionTypeById(Convert.ToInt32(Type));


                //AspNetUser users = new AspNetUser();
                //var userId = User.Identity.GetUserId();
                //var user = new AspNetUserRepository().GetUser(userId);

                //if (string.IsNullOrEmpty(ContactNumber))
                //{

                //    var user = _subscription.GetUserByEmail(subscriptionPayments.Phone);
                //}
                
                var orderRequest = new SubscriptionPayment
                {
                    Amount = subscriptionMaster.Amount ?? 0,
                    DiscountPer = 0,
                    TotalAmount = subscriptionMaster.Amount ?? 0,
                    Email = Email,
                    Name = Name,
                    Phone = ContactNumber,
                    SubscriptionId = subscriptionMaster.Id,
                    TransactionId = _subscription.GenerateSubTxnId(),
                    //UserId = user.Id
                };

                var merchantKey = _iConfig.GetValue<string>("AppSettings:Key");
                var merchantSalt = _iConfig.GetValue<string>("AppSettings:Salt");
                var IsLive = Convert.ToBoolean(_iConfig.GetValue<string>("AppSettings:IsLive"));
                var serviceProvider = "payu_paisa";
                //var productInfo = "subscription";

                var firstname = orderRequest.Name.Length > 18 ? orderRequest.Name.Substring(0, 18) : orderRequest.Name;
                var hashString = merchantKey + "|" + orderRequest.TransactionId + "|" + orderRequest.Amount + "|" + subscriptionMaster.SubscriptionType + "|" + firstname + "|" + orderRequest.Email + "|||||||||||" + merchantSalt;

                // var hashString = merchantKey + "|" + orderRequest.TransactionId + "|" + orderRequest.Amount + "| '' |" + firstname + "|" + orderRequest.Email + "|||||||||||" + merchantSalt;
                var hash = PayUMoney.Generatehash512(hashString);
                orderRequest.RequestLog = hashString;
                orderRequest.RequestHash = hash;
                orderRequest.Status = PayUMoney.PaymentStatus.Initiated.GetHashCode();
                orderRequest.CreatedDate = DateTime.UtcNow.ToLocalTime();
                orderRequest.UpdatedDate = DateTime.UtcNow.ToLocalTime();

                _subscription.CreateSubscriptionPayment(orderRequest);


                _remotePost.Url = _iConfig.GetValue<string>("AppSettings:paymentgatewayUrl");
                _remotePost.Add("key", merchantKey);
                _remotePost.Add("txnid", orderRequest.TransactionId);
                _remotePost.Add("amount", orderRequest.Amount.ToString());
                _remotePost.Add("firstname", Name);
                _remotePost.Add("phone", orderRequest.Phone);
                _remotePost.Add("email", orderRequest.Email);
                _remotePost.Add("productinfo", subscriptionMaster.SubscriptionType);
                _remotePost.Add("surl", _iConfig.GetValue<string>("AppSettings:subreturnUrl"));
                _remotePost.Add("furl", _iConfig.GetValue<string>("AppSettings:subreturnUrl"));
          

                TempData["SubscriptionId"] = subscriptionMaster.Id.ToString();
                TempData["Email"] = Email;
                //comment due to here we use pay u biz so in this service provider not required

                //if (!Request.IsLocal)
                //    myremotepost.Add("service_provider", serviceProvider);
                if (IsLive)
                {
                    _remotePost.Add("service_provider", serviceProvider);
                }
                _remotePost.Add("hash", hash);
                _remotePost.Post();

            }
            catch (Exception ex)
            {

            }

            return View();

        }

        public ActionResult PaymentStatus(IFormCollection form)
        {
            var responseObject = new Response();
            try
            {
                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                var paymentRowResponse = string.Empty;
                var txnid = string.Empty;
                //for (int i = 0; i < form.Count; i++)
                //{
                //    paymentRowResponse += form.Keys[i] + ": " + form[i] + " | ";
                //}
                foreach (var key in form.Keys)
                {
                    paymentRowResponse += key + ": " + form[key] + " | ";
                }
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                var transactionId = form["txnid"];
                //var SubscriptionOrderPaymentRepository = new SubscriptionOrderPaymentRepository();
               // var orderResponse = _subscription.Search(t => t.TransactionId == transactionId).ToList().FirstOrDefault();
                //if (orderResponse == null)
                //    return RedirectToAction("Cart", "Catalogue");
                var status = form["status"].ToString();
                _subscriptionPayment.AddtionalCharge = form["additionalCharges"];
                _subscriptionPayment.Error = form["Error"];
                _subscriptionPayment.PayUMoneyId = form["payuMoneyId"];
                _subscriptionPayment.PGType = form["PG_TYPE"];
                _subscriptionPayment.ResponseHas = form["hash"];
                _subscriptionPayment.ResponseLog = paymentRowResponse;
                _subscriptionPayment.TransactionId = transactionId;
                //_subscriptionPayment.OrderId = form["mihpayid"];//Merchant Identification Number
                ViewBag.Message = "";
                if (status.ToLower() == "success")
                {
                    #region payment check sum check
                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = _iConfig.GetValue<string>("AppSettings:Salt");
                    //merc_hash_string = ConfigurationManager.AppSettings["Salt"] + "|" + status;
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (form[merc_hash_var].ToString() != null ? form[merc_hash_var].ToString() : "");
                    }
                    merc_hash = PayUMoney.Generatehash512(merc_hash_string).ToLower();
                    #endregion
                    responseObject.Status = "SUCCESS";
                    _subscriptionPayment.Status = PayUMoney.PaymentStatus.Success.GetHashCode();
                    //ViewBag.Message = "Your payment is successful";


                }
                else
                {
                    responseObject.Status = "FAILURE";
                    _subscriptionPayment.Error = _subscriptionPayment.Error + "(Hash value did not matched)";
                    _subscriptionPayment.Status = PayUMoney.PaymentStatus.Failure.GetHashCode();
                    ViewBag.Message = "Your payment is failed";
                    return View("PaymentStatusFailed");
                }
                _subscriptionPayment.UpdatedDate = DateTime.UtcNow.ToLocalTime();
                _subscription.Edit(_subscriptionPayment);
                var subscriptionPayments = _subscription.GetSubscriptionPaymentByTransId(transactionId);
                if (status.ToLower() == "success")
                {
                    _aspNetUsers.FirstName = subscriptionPayments.Name;
                    _aspNetUsers.Email = subscriptionPayments.Email;
                    _aspNetUsers.UserName = subscriptionPayments.Phone;
                    _aspNetUsers.PasswordHash = Encryption.EncryptCommon(subscriptionPayments.Phone);
                    _aspNetUsers.ProfileImage = "/UserProfileImage/Noimage.jpg";
                    _aspNetUsers.PhoneNumber = subscriptionPayments.Phone;

                    /*Create User*/
                    _subscription.AddUser(_aspNetUsers, transactionId, subscriptionPayments.SubscriptionId);

                    //var userSubsrciption = new UserSubscription();
                    //var userSubsrciptionRepository = new UserSubscriptionRepository();
                    //  var subscriptionMasterRepository = new SubscriptionMasterRepository();
                    //var SubscriptionId = _httpContextAccessor.HttpContext.Session.GetInt32("SubscriptionId");
                    //var SubscriptionId = Convert.ToInt32(TempData["SubscriptionId"]);
                    //var Email = Convert.ToString(TempData["Email"]);
              
                    
                       //var subscriptionMaster = _subscription.FindSubscriptionTypeById(SubscriptionId);
                       var subscriptionMaster = _subscription.FindSubscriptionTypeById(subscriptionPayments.SubscriptionId);
                    //_userSubscription.UserId = _subscriptionPayment.UserId;
                    //_userSubscription.StartDate = DateTime.UtcNow.ToLocalTime();
                    //_userSubscription.EndDate = DateTime.UtcNow.ToLocalTime().AddDays(subscriptionMaster.DaysValidity ?? 0);
                    //_userSubscription.Active = true;
                    //_userSubscription.TransactionId = _subscriptionPayment.TransactionId;
                    //_userSubscription.TypeId = subscriptionPayments.SubscriptionId;
                    //_subscription.Create(_userSubscription, _aspNetUsers.UserName);

               
                    var user = _subscription.GetUserByEmail(subscriptionPayments.Phone);
                    _subscriptionMail.Name = subscriptionPayments.Name;
                    _subscriptionMail.TransactionId = subscriptionPayments.TransactionId;
                    _subscriptionMail.Amount = subscriptionPayments.Amount;
                    _subscriptionMail.UserName = user.UserName;
                    _subscriptionMail.UpdatedDate = subscriptionPayments.UpdatedDate.ToLocalTime();
                    _subscriptionMail.Password = Encryption.DecryptCommon(user.PasswordHash);
                    var smsText = "Congratulations " + subscriptionPayments.Name + ", for successfully making a purchase of Rs." + subscriptionPayments.Amount + " on Prachi’s Website with,Order ID: " + subscriptionPayments.TransactionId + " on,Order Date: " + subscriptionPayments.UpdatedDate + " Please check your email for more related information.Thanks - Prachi India";

                    Common.SendSMS(subscriptionPayments.Phone, smsText);
                    var body = RenderRazorViewToString("MailTemplate", _subscriptionMail);
                   // MailServiece.Mail.SendMail(subscriptionPayments.Email, "THANK YOU FOR YOUR PURCHASE!!", body);

                    Common.SendMail(subscriptionPayments.Email, "THANK YOU FOR YOUR PURCHASE!!", body);
                    ViewBag.Message = "1";
                    return View("index","UserSubscription");

                }

                // var userId = User.Identity.GetUserId();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "0";
            }
            return View();
        }


        public ActionResult Test()
        {
            //var subscriptionOrderPaymentRepository = new SubscriptionPayment();
            //var html = RenderRazorViewToString("MailTemplate", subscriptionOrderPaymentRepository);
            return View("MailTemplate");
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {

                return this.RenderViewToStringAsync(viewName, model).Result;
             
            }
 
        }
    }


}
