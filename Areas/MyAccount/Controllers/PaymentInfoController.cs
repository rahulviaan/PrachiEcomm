using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Areas.MyAccount.Controllers
{
    public class PaymentInfoController : Controller
    {
        // GET: MyAccount/PaymentInfo
        public ActionResult Index(int page = 0)
        {
            int itemsPerPage = 20;
            page = (page == 0) ? 1 : page;
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();

            var orders = context.Orders.OrderByDescending(t => t.Id).ToPagedList(page, itemsPerPage);

            return View(orders);
        }
        public ActionResult PaymentDetails(long Id, bool update = false)
        {
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var order = context.Orders.FirstOrDefault(t => t.Id == Id);
            if (order == null)
                return RedirectToAction("Index");
            #region VerifyPayment Status
            if (update)
            {
                CheckPayUStatus(order.TransactionId, order.Id, context);
            }
            #endregion

            var aspNetuser = context.AspNetUsers.First(t => t.Id == order.UserId);
            order = context.Orders.First(t => t.Id == Id);
            var customerInfo = new Models.ContactUs
            {
                ContactNo = aspNetuser.PhoneNumber,
                Email = aspNetuser.Email,
                Name = order.Name,
                Message = string.Format("{0}, {1}, {2}, {3}-{4}", aspNetuser.Address, aspNetuser.City, aspNetuser.State, aspNetuser.Country, aspNetuser.PinCode)
            };
            order.IsRecive = update;
            ViewBag.CustomerInformation = customerInfo;
            return View(order);
        }


        public ActionResult VerifyPayment(string paymentId)
        {
            var result = string.Empty;
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            if (!string.IsNullOrWhiteSpace(paymentId))
            {
                var order = context.Orders.FirstOrDefault(t => t.OrderId.ToLower() == paymentId || t.PayUMoneyId.ToLower() == paymentId);
                if (order != null)
                {
                    var orderId = order.TransactionId;
                    // xxxx(orderId);
                    //CheckPayUStatus(orderId, order.Id, context);
                }
                else
                {

                }
            }
            return View();
        }

        void CheckPayUStatus(string orderId, long Id, PrachiIndia.Sql.dbPrachiIndia_PortalEntities context)
        {
            var result = string.Empty;
            try
            {
                const string WEBSERVICE_URL = "https://www.payumoney.com/payment/op/getPaymentResponse?";
                string myParameters = "merchantKey=W49jcI&merchantTransactionIds=" + orderId;
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
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            if (!string.IsNullOrWhiteSpace(result))
            {
                var paymentModel = JsonConvert.DeserializeObject<Models.PaymentModel>(result);
                if (paymentModel != null && paymentModel.result != null && paymentModel.result.Any())
                {
                    foreach (var item in paymentModel.result)
                    {
                        if (item.postBackParam != null)
                        {
                            var order = context.Orders.First(t => t.Id == Id);
                            if (item.postBackParam.status.ToLower() == "success")
                            {
                                order.Status = 1;
                            }
                            else
                            {
                                order.Status = 2;
                            }

                            order.PayUMoneyId = item.postBackParam.payuMoneyId;
                            order.PGType = item.postBackParam.mode;
                            order.Error = string.Format("{0} ({1})", item.postBackParam.error, item.postBackParam.error_Message);
                            order.RequestLog = result;
                            order.ResponseHas = string.Format("Manual|{0}", item.postBackParam.hash);
                            order.UpdatedDate = DateTime.Now;
                            order.OrderId = item.postBackParam.mihpayid;
                            context.Orders.Attach(order);
                            context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
                else if (paymentModel.status == -1)
                {
                    var order = context.Orders.First(t => t.Id == Id);
                    order.Status = 2;
                    order.Error = string.Format("{0} ({1})", "", paymentModel.message);
                    order.UpdatedDate = DateTime.Now;
                    context.Orders.Attach(order);
                    context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
            }
        }

        void xxxx(string orderId)
        {
            var result = string.Empty;
            const string WEBSERVICE_URL = "https://payumoney.com/payment/payment/chkMerchantTxnStatus?";
            string myParameters = "merchantKey=W49jcI&merchantTransactionIds=" + orderId;
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
                        result = sr.ReadToEnd();
                    }
                }
            }
            var x = result;
        }
    }
}