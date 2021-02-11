using Microsoft.AspNet.Identity;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Controllers
{
    public class UserSubscriptionController : Controller
    {
        // GET: UserSubscription
        [Route("Subscription")]
        public ActionResult Index()
        {
            return View();
        }
         
        public ActionResult payment(string Type)
        {

            var aspNetUserRepository = new AspNetUserRepository();
            try
            {

                var subscriptionMasterRepository = new SubscriptionMasterRepository();
                var subscriptionOrderPaymentRepository = new SubscriptionOrderPaymentRepository();
                var subscriptionMaster = subscriptionMasterRepository.FindByIdAsync(Convert.ToInt32(Type));

                //subscriptionMaster.
                AspNetUser users = new AspNetUser();
                var userId = User.Identity.GetUserId();
                var user = new AspNetUserRepository().GetUser(userId);


                var orderRequest = new Sql.SubscriptionPayment
                {
                    Amount = subscriptionMaster.Amount,
                    DiscountPer = 0,
                    TotalAmount = subscriptionMaster.Amount,
                    Email = user.Email,
                    Name = user.FirstName,
                    Phone = user.PhoneNumber,
                    SubscriptionId = subscriptionMaster.Id,
                    TransactionId = PayUMoney.GenerateSubTxnId(),
                    UserId = user.Id
                };
               
                

                var merchantKey = ConfigurationManager.AppSettings["Key"].ToString(CultureInfo.InvariantCulture);
                var merchantSalt = ConfigurationManager.AppSettings["Salt"].ToString(CultureInfo.InvariantCulture);
                var IsLive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLive"].ToString(CultureInfo.InvariantCulture));
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

                subscriptionOrderPaymentRepository.CreateAsync(orderRequest);

                var myremotepost = new RemotePost();
                myremotepost.Url = ConfigurationManager.AppSettings["paymentgatewayUrl"].ToString(CultureInfo.InvariantCulture);
                myremotepost.Add("key", merchantKey);
                myremotepost.Add("txnid", orderRequest.TransactionId);
                myremotepost.Add("amount", orderRequest.Amount.ToString());
                myremotepost.Add("firstname", user.FirstName);
                myremotepost.Add("phone", orderRequest.Phone); 
                myremotepost.Add("email", orderRequest.Email);
                myremotepost.Add("productinfo",subscriptionMaster.SubscriptionType);
                myremotepost.Add("surl", ConfigurationManager.AppSettings["subreturnUrl"].ToString(CultureInfo.InvariantCulture));
                myremotepost.Add("furl", ConfigurationManager.AppSettings["subreturnUrl"].ToString(CultureInfo.InvariantCulture));
                //comment due to here we use pay u biz so in this service provider not required

                //if (!Request.IsLocal)
                //    myremotepost.Add("service_provider", serviceProvider);
                if (IsLive)
                {
                    myremotepost.Add("service_provider", serviceProvider);
                }
                myremotepost.Add("hash", hash);
                myremotepost.Post();

            }
            catch (Exception ex)
            {

            }

            return View();

        }

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
                var SubscriptionOrderPaymentRepository = new SubscriptionOrderPaymentRepository();
                var orderResponse = SubscriptionOrderPaymentRepository.Search(t => t.TransactionId == transactionId).FirstOrDefault();
                //if (orderResponse == null)
                //    return RedirectToAction("Cart", "Catalogue");
                var status = form["status"];
                orderResponse.AddtionalCharge = form["additionalCharges"];
                orderResponse.Error = form["Error"];
                orderResponse.PayUMoneyId = form["payuMoneyId"];
                orderResponse.PGType = form["PG_TYPE"];
                orderResponse.ResponseHas = form["hash"];
                orderResponse.ResponseLog = paymentRowResponse;
                orderResponse.TransactionId = transactionId;
                //orderResponse.OrderId = form["mihpayid"];//Merchant Identification Number
                ViewBag.Message = "";
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
                    //ViewBag.Message = "Your payment is successful";


                }
                else
                {
                    responseObject.Status = "FAILURE";
                    orderResponse.Error = orderResponse.Error + "(Hash value did not matched)";
                    orderResponse.Status = PayUMoney.PaymentStatus.Failure.GetHashCode();
                    ViewBag.Message = "Your payment is failed";
                    return View("PaymentStatusFailed");
                }
                orderResponse.UpdatedDate = DateTime.UtcNow.ToLocalTime();
                SubscriptionOrderPaymentRepository.UpdateAsync(orderResponse);
                if (status.ToLower() == "success")
                {
                    var userSubsrciption = new UserSubscription();
                    var userSubsrciptionRepository = new UserSubscriptionRepository();
                    var subscriptionMasterRepository = new SubscriptionMasterRepository();

                    var subscriptionMaster = subscriptionMasterRepository.FindByIdAsync(orderResponse.SubscriptionId ?? 0);
                    userSubsrciption.UserId = orderResponse.UserId;
                    userSubsrciption.StartDate = orderResponse.CreatedDate;
                    userSubsrciption.EndDate = orderResponse.CreatedDate.Value.AddDays(subscriptionMaster.DaysValidity??0);
                    userSubsrciption.Active = true;
                    userSubsrciption.TransactionId = orderResponse.TransactionId;
                    userSubsrciption.TypeId = orderResponse.SubscriptionId;
                    userSubsrciptionRepository.CreateAsync(userSubsrciption);

                    var subscriptionMail = new SubscriptionMail();
                    var userId = User.Identity.GetUserId();
                    var user = new AspNetUserRepository().GetUser(userId);
                    subscriptionMail.Name = orderResponse.Name;
                    subscriptionMail.TransactionId = orderResponse.TransactionId;
                    subscriptionMail.Amount = orderResponse.Amount;
                    subscriptionMail.UserName = user.UserName;
                    subscriptionMail.UpdatedDate = orderResponse.UpdatedDate.Value.ToLocalTime();
                    subscriptionMail.Password = Encryption.DecryptCommon(user.PasswordHash);
                   var smsText= "Congratulations " + orderResponse.Name + ", for successfully making a purchase of Rs." + orderResponse.Amount + " on Prachi’s Website with,Order ID: "+ orderResponse.TransactionId + " on,Order Date: "+ subscriptionMail.UpdatedDate + " Please check your email for more related information.Thanks - Prachi India";

                    MessageSent.SendSMS(orderResponse.Phone, smsText);
                    var body = RenderRazorViewToString("MailTemplate", subscriptionMail);
                    //var invoiceCopy = PrachiService.GenerateInvoiceForSubscription(orderResponse.TransactionId, orderResponse.UserId);
                    MailServiece.Mail.SendMail(orderResponse.Email, "THANK YOU FOR YOUR PURCHASE!!", body);

                  

                }

                    // var userId = User.Identity.GetUserId();
                }
            catch (Exception ex)
            {

            }
            return View();
        }


        public ActionResult Test()
        {
            var subscriptionOrderPaymentRepository = new SubscriptionPayment();
            var html = RenderRazorViewToString("MailTemplate", subscriptionOrderPaymentRepository);
            return View("MailTemplate");
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }


}
