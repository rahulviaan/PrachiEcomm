using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using PrachiIndia.Portal.Helpers;
using System.Configuration;
using System.Globalization;
using Microsoft.Ajax.Utilities;
using PagedList;
using System.Data.SqlClient;
using System.Collections.Generic;
using PrachiIndia.Web.Areas.Model;
using PrachiIndia.Portal.Framework;
using System.IO;
using Ionic.Zip;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Controllers
{
    //[Authorize(Roles = "Admin,SuperAdmin")]
    [Authorize]
    public class CartHistoryController : Controller
    {
        string ImageUrl = ConfigurationManager.AppSettings["ImageUrl"].ToString(CultureInfo.InvariantCulture);
        //this method is use for Show User Order History
        public ActionResult UserCartHistory(string sortOrder, string CurrentSort, int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page ?? 1;

            ViewBag.CurrentSort = sortOrder;
            sortOrder = String.IsNullOrEmpty(sortOrder) ? "PaymentDate" : sortOrder;
            IPagedList<Order> orders = null;

            var orderRepository = new OrderRepository();
            var userId = User.Identity.GetUserId();
            var results = orderRepository.SearchFor(t => t.UserId == userId).OrderByDescending(t => t.Id).ToList();

            switch (sortOrder)
            {
                case "PaymentDate":
                    if (sortOrder.Equals(CurrentSort))
                        orders = orderRepository.SearchFor(t => t.UserId == userId).OrderByDescending
                                (m => m.UpdatedDate).ToPagedList(pageIndex, pageSize);
                    else
                        orders = orderRepository.SearchFor(t => t.UserId == userId).OrderByDescending
                                (m => m.UpdatedDate).ToPagedList(pageIndex, pageSize);
                    break;

                case "Default":
                    orders = orderRepository.SearchFor(t => t.UserId == userId).OrderByDescending
                                 (m => m.Id).ToPagedList(pageIndex, pageSize);
                    break;
            }


            //var items = (from item in results
            //select new OrderTrackVm
            //{
            //    OrderId = item.OrderId,
            //    TransactionId = item.TransactionId,
            //    Title = item.Name,
            //    date = item.UpdatedDate.Value.ToString("dd-MM-yyyy"),
            //    //  Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
            //    Image = ImageUrl + (("no_image.jpg")),//item.Image?? "no_image.jpg",
            //    price = String.Format("{0:0.00}", item.Amount),
            //    //Quantity = item.Q.ToString(),
            //    status = item.Status != 1 ? false : true,
            //    Dispatchby = item.dispatchby,
            //    AWBNO = item.AWBNO,
            //    IsRecive = Convert.ToBoolean(item.IsRecive),
            //    ReciveDate = item.ReciveDate.ToString(),
            //    OrderRecivedBy = item.OrderReciveBy,
            //    fromdate = DateTime.Now.AddDays(-7),
            //    todate = DateTime.Now
            //}).ToList();
            //var orderProductRepository = new OrderProductRepository();
            //var UserId = User.Identity.GetUserId();
            //var query = orderProductRepository.GetAll();
            //var result = query.Where(x => x.Order.UserId == UserId).ToList().DistinctBy(t => t.OrderId);
            //var items = (from item in result
            //             select new OrderTrackVm
            //             {
            //                 OrderId = item.OrderId.ToString() ?? "",
            //                 TransactionId = item.Order.TransactionId,
            //                 Title = item.Title,
            //                 date = item.CreatedDate.ToString(),
            //                 Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
            //                 price = String.Format("{0:0.00}", item.Order.Amount),
            //                 Quantity = item.Quantity.ToString(),
            //                 status = item.Order.Status != 1 ? false : true,
            //                 Dispatchby = item.Order.dispatchby,
            //                 AWBNO = item.Order.AWBNO,
            //                 IsRecive = Convert.ToBoolean(item.Order.IsRecive),
            //                 ReciveDate = item.Order.ReciveDate.ToString(),
            //                 OrderRecivedBy = item.Order.OrderReciveBy,
            //                 fromdate = DateTime.Now.AddDays(-7) ,
            //                 todate= DateTime.Now
            //             }).ToList();
            //var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };

            return View(orders);
        }
        //this method is use for Show User Order History with fillter which given there.
        [HttpPost]
        public ActionResult UserCartHistory(OrderTrackVm model)
        {
            var orderProductRepository = new OrderProductRepository();
            var UserId = User.Identity.GetUserId();
            IQueryable<OrderProduct> query = orderProductRepository.GetAll();
            query = query.Where(x => x.Order.UserId == UserId);
            model.todate = model.todate.AddDays(1);
            if (model.fromdate != null && model.todate != null)
            {
                query = query.Where(t => t.CreatedDate >= model.fromdate && t.CreatedDate <= model.todate);
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
            var result = query.ToList();
            var items = (from item in result
                         select new OrderTrackVm
                         {
                             OrderId = item.OrderId.ToString() ?? "",
                             TransactionId = item.Order.TransactionId,
                             Title = item.Title,
                             date = item.CreatedDate.ToString(),
                             Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
                             price = item.Order.Amount ?? 0,
                             Quantity = item.Quantity ?? 0,
                             status = item.Order.Status != 1 ? false : true,
                             Dispatchby = item.Order.dispatchby,
                             AWBNO = item.Order.AWBNO,
                             IsRecive = Convert.ToBoolean(item.Order.IsRecive),
                             ReciveDate = item.Order.ReciveDate.ToString(),
                             OrderRecivedBy = item.Order.OrderReciveBy
                         }).DistinctBy(x => x.OrderId).ToList();
            //Added by Rahul Srivastava for handling record filtering.
            items.Add(model);
            return View(items);
        }
        //This Method is use for show order detail
        public ActionResult OrderDetail(int orderId)
        {
            var orderProductRepository = new OrderProductRepository();
            var itemRepository = new OrderRepository();
            var UserId = User.Identity.GetUserId();
            var order = itemRepository.FindByIdAsync(orderId);
            var result = orderProductRepository.SearchFor(t => t.OrderId == orderId).ToList();
            //var result = query.Where(x => x.OrderId == orderId).ToList();
            var items = (from item in result

                         select new CartHistoryVM
                         {
                             TransactionId = item.Order.TransactionId,
                             Title = item.Title,
                             OrderId = item.OrderId.ToString(),
                             date = item.CreatedDate.ToString(),
                             Image = ImageUrl + ((item.tblCataLog.Image != null) ? item.tblCataLog.Image : ("no_image.jpg")),//item.Image?? "no_image.jpg",
                             price = item.Price ?? 0,
                             Quantity = item.Quantity ?? 0,
                             Discount = item.Discount ?? 0,
                             TotalPrice = item.TotalAmount ?? 0,
                             TaxPrice = item.Tax,
                             status = item.Order.Status != 1 ? false : true,
                             BookType = item.BookType.ToString()
                         }).ToList();
            ViewBag.Order = order;
            return View(items);
        }

        public ActionResult MyLibrary()
        {
            var results = new List<MyEbookLibrary>();
            var UserId = User.Identity.GetUserId();
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            var con = new SqlConnection(connectionString);
            var cmd = new SqlCommand("usp_MyLibrary", con);
            cmd.CommandType = CommandType.StoredProcedure;
            var userId = User.Identity.GetUserId();
            cmd.Parameters.Add(new SqlParameter("@userId", userId));
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var MyEbookLibrary = new MyEbookLibrary
                    {
                        Board = dr.IsNull("Board") ? string.Empty : Convert.ToString(dr["Board"]),
                        Classes = dr.IsNull("Class") ? string.Empty : Convert.ToString(dr["Class"]),
                        Id = dr.IsNull("Id") ? 0 : Convert.ToInt64(Convert.ToString(dr["Id"])),
                        Image = dr.IsNull("Image") ? string.Empty : Convert.ToString(dr["Image"]),
                        Subect = dr.IsNull("Subject") ? string.Empty : Convert.ToString(dr["Subject"]),
                        Title = dr.IsNull("Title") ? string.Empty : Convert.ToString(dr["Title"]),
                    };
                    results.Add(MyEbookLibrary);
                }
            }

            return View(results);
        }
        public ActionResult MyPO()
        {
            var userId = User.Identity.GetUserId();
            var list = new dbPrachiIndia_PortalEntities().EbookOrders.Where(t => t.UserID == userId).OrderByDescending(x => x.OrderDate).ToList();
            return View(list);
        }
        [OutputCache(Duration = 1800, VaryByParam = "RequestID;ConfigurationType")]
        public FileResult DownloadFile(string RequestID, string UserType = "",string ConfigurationType = "")
        {
            var userId = User.Identity.GetUserId();
            var EncConfiguration = new EbookHelper().CreateConfiguration(userId, RequestID, UserType, ConfigurationType);
            string path = Server.MapPath("~/ModuleFiles/TempFiles/" + RequestID + "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var filename = "PO_Encrypted.xml";
            var fullPath = path + filename;

            var writer = new StreamWriter(fullPath);
            writer.Write(EncConfiguration);
            writer.Close();
            var zipFile = new ZipFile();
            zipFile.AddFile(fullPath, RequestID);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("content-disposition", "attachment; filename=" + RequestID+"_"+ConfigurationType + ".zip");
            var outputStream = new MemoryStream();
            zipFile.Save(outputStream);
            outputStream.Position = 0;
            string fileType = "application/octet-stream";
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            return new FileStreamResult(outputStream, fileType);
        }
        public FileResult DownloadReatilFile(long OrderId)
        {
            var req = "PIPL-18-" + OrderId;
            var userId = User.Identity.GetUserId();
            var EncConfiguration = new EbookHelper().CreateConfiguration(userId, OrderId);
            string path = Server.MapPath("~/ModuleFiles/TempFiles/" + req + "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var filename = "PO_Encrypted.xml";
            var fullPath = path + filename;

            var writer = new StreamWriter(fullPath);
            writer.Write(EncConfiguration);
            writer.Close();
            var zipFile = new ZipFile();
            zipFile.AddFile(fullPath, req);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("content-disposition", "attachment; filename=" + req + ".zip");
            var outputStream = new MemoryStream();
            zipFile.Save(outputStream);
            outputStream.Position = 0;
            string fileType = "application/octet-stream";
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            return new FileStreamResult(outputStream, fileType);
        }
        //this method is use for Show User Subscription
        public ActionResult UserSubscriptionHistory()
        {
            var SubscriptionRepo = new SubscriptionTrnRepostory();
            var UserId = User.Identity.GetUserId();
            var result = SubscriptionRepo.GetAll().Where(t => t.UserId == UserId).ToList();
            var results = (from item in result
                           select new SubscriptionVm
                           {
                               PlanName = item.mst_subscription.PlanName,
                               PlanTime = item.mst_subscription.PlanTime.ToString(),
                               startdate = item.StartDate ?? DateTime.Now,
                               subscriptionType = item.mst_subscription.SubscriptionType,
                               Class = item.MasterClass.Title,
                               Board = item.MasterBoard.Title,
                               Amount = item.Amount,
                           }).ToList();
            return View(results);
        }
        public static class Booktypes
        {
            public static string Ebook = "Ebook";
            public static string Pbook = "Pbook";
            public const string Both = "Both";
        }

        public ActionResult PrintInvoice(long id)
        {

            var context = new dbPrachiIndia_PortalEntities();
            var userId = User.Identity.GetUserId();
            var invoiceCopy = PrachiService.GenerateInvoice(id, userId);


            MailServiece.Mail.SendMail(invoiceCopy.Email, invoiceCopy.Subject, invoiceCopy.MailTemplate);
            return RedirectToAction("OrderDetail", "CartHistory", new { orderId = id });

        }

        public static void SendMail(String ToEmail, String Subj, string Message, System.Net.Mail.Attachment attach)
        {
            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress("marketing@prachigroup.com", "no-reply@prachigroup.com");
            mailMessage.Subject = Subj;
            mailMessage.Body = Message;
            mailMessage.IsBodyHtml = true;

            string[] ToMuliId = ToEmail.Trim().Split(',');
            if (ToMuliId[0] != "")
            {
                foreach (string ToEMailId in ToMuliId)
                {
                    mailMessage.To.Add(new System.Net.Mail.MailAddress(ToEMailId));
                }
            }

            mailMessage.Attachments.Add(attach);

            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("marketing@prachigroup.com", "otp@1212")
            };

            try
            {
                smtp.Send(mailMessage);
                mailMessage.Dispose();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
    }
}