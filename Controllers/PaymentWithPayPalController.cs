using PayPal.Api;
using PrachiIndia.Portal.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Portal.Models;
using Microsoft.AspNet.Identity;
using PrachiIndia.Sql;

namespace PrachiIndia.Portal.Controllers
{
    public class PaymentWithPayPalController : Controller
    {
        private PayPal.Api.Payment payment;
        // GET: PaymentWithPayPal
        public ActionResult Index(string Cancel = null)
        {
            var orderId = Session["OrderId"] != null ? ((long)Session["OrderId"]) : 0;
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var orderMaster = context.Orders.FirstOrDefault(t => t.Id == orderId);
            if (orderMaster == null)
                return RedirectToAction("Cart", "Catalogue");


            if (Cancel == "true")
            {
                return RedirectToAction("PaymentCancel", "PaymentWithPayPal");
            }
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PaymentWithPayPal/Index?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, orderMaster);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    Session["executedPayment"] = executedPayment;
                    //If executed payment failed then we will show payment failure message to user  
                    return RedirectToAction("PaymentSuccess", "PaymentWithPayPal");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("PaymentFailure", "PaymentWithPayPal");
            }
        }

        public ActionResult PaymentSuccess()
        {
            Session.Remove("OrderId");
            var responseObject = new Response();
            if (Session["executedPayment"] != null)
            {
                var executedPayment = (Payment)Session["executedPayment"];

                var trnsactions = executedPayment.transactions;
                var payment = trnsactions.First();
                var transactionId = payment.invoice_number;
                var amount = payment.amount.total;


                var OrderRepository = new OrderRepository();
                var orderResponse = OrderRepository.SearchFor(t => t.TransactionId == transactionId).FirstOrDefault();
                if (orderResponse == null)
                    return RedirectToAction("Cart", "Catalogue");

                //orderResponse.AddtionalCharge = form["additionalCharges"];
                //orderResponse.Error = form["Error"];
                orderResponse.PayUMoneyId = payment.related_resources[0].sale.id;
                orderResponse.PGType = "PayPal";
                //orderResponse.ResponseHas = form["hash"];
                //orderResponse.ResponseLog = paymentRowResponse;
                //orderResponse.TransactionId = transactionId;
                orderResponse.OrderId = payment.related_resources[0].sale.parent_payment;


                responseObject.Status = "SUCCESS";
                orderResponse.Status = PayUMoney.PaymentStatus.Success.GetHashCode();

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
                            Flag = "sccess",
                            BookType = cartItem.BookType.ToString(),
                            Discount = cartItem.Discount ?? 0,
                            TaxRate = cartItem.Tax,
                            TotalAmount = cartItem.TotalAmount ?? 0
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

                            var carts = cartRepository.SearchFor(t => t.UserId == userId && t.IsWishList == false).ToList();
                            foreach (var cart in carts)
                            {
                                cartRepository.DeleteAsync(cart);
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

                    var invoiceCopy = PrachiService.GenerateInvoice(orderResponse.Id, userId);
                    MailServiece.Mail.SendMail(invoiceCopy.Email, invoiceCopy.Subject, invoiceCopy.MailTemplate);
                    //Oredermail(customers, responseObject);

                }
                Session.Remove("executedPayment");
                return View(responseObject);
            }
            else
            {
                return RedirectToAction("Cart", "Catalogue");
            }

        }
        public ActionResult PaymentFailure()
        {
            var executedPayment = (Payment)Session["executedPayment"];
            var orderId = (long)Session["OrderId"];
            var OrderRepository = new OrderRepository();
            var orderResponse = OrderRepository.SearchFor(t => t.Id == orderId).FirstOrDefault();
            if (orderResponse == null)
                return RedirectToAction("Cart", "Catalogue");



            orderResponse.Status = PayUMoney.PaymentStatus.Failure.GetHashCode();
            orderResponse.UpdatedDate = DateTime.UtcNow;
            OrderRepository.UpdateAsync(orderResponse);

            Session.Remove("OrderId");
            Session.Remove("executedPayment");
            return View(orderResponse);
        }
        public ActionResult PaymentCancel()
        {
            Session.Remove("OrderId");
            Session.Remove("executedPayment");
            return View();
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, Sql.Order orderMaster)
        {
            var amt = orderMaster.TotalAmount ?? 0;
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = orderMaster.ProductInfo,
                currency = "INR",
                price = amt.ToString(),
                quantity = "1",
                sku = "SKU" + orderMaster.TransactionId
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = amt.ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "INR",
                total = amt.ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = orderMaster.ProductInfo,
                invoice_number = orderMaster.TransactionId, //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}