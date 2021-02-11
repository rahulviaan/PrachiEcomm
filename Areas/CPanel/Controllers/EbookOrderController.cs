using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Configuration;
using PrachiIndia.Portal.Areas.CPanel.Models;
using Microsoft.AspNet.Identity;
using PrachiIndia.Portal.Helpers;
using Microsoft.AspNet.Identity.Owin;
using PrachiIndia.Portal.Framework;
using System.Globalization;
using MailServiece;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Portal.Areas.CPanel.Models.ViewModels;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class EbookOrderController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: CPanel/EbookOrder
        int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        public ActionResult ViewEbookOrder(int? page)
        {
            var context = new dbPrachiIndia_PortalEntities();
            if (Session["objEbookorder"] != null && page != null)
            {
                var objEbookorder = (PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder)Session["objEbookorder"];
                DateTime todate = Convert.ToDateTime(objEbookorder.todate).AddDays(1);
                var listOrder = (from item in context.EbookOrders.Where(x=> x.RequestID== "EBR192074").ToList()
                                 let aspnetUser = context.AspNetUsers.First(t => t.Id == item.UserID)
                                 select new PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder
                                 {
                                     CreatedDate = item.OrderDate ?? DateTime.Now,
                                     EmailAddres = aspnetUser.Email,
                                     EmailConfirmed = item.EmaiVerified ?? false,
                                     Id = item.ID,
                                     Mobile = aspnetUser.PhoneNumber,
                                     MobileConfirmed = item.MobileVerified ?? false,
                                     RequestId = item.RequestID,
                                     SchoolName = item.SchoolName,
                                     UserId = item.UserID,
                                     Status = item.Status ?? false,
                                     State = item.State.Trim().ToLower(),
                                     City = item.City.Trim().ToLower(),
                                 });
                IEnumerable<PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder> result;
                result = listOrder;

                if (objEbookorder.fromdate != null)
                {
                    DateTime fromdate = Convert.ToDateTime(objEbookorder.fromdate);
                    result = result.Where(x => x.CreatedDate > fromdate);
                }

                if (objEbookorder.todate != null)
                {
                    DateTime fromdate = Convert.ToDateTime(objEbookorder.fromdate);
                    result = result.Where(x => x.CreatedDate < todate);
                }
                if (!string.IsNullOrEmpty(objEbookorder.SearchParameter))
                {
                    result = result.Where(x => x.RequestId == objEbookorder.SearchParameter || x.SchoolName.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower() || x.EmailAddres.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower() || x.Mobile == objEbookorder.SearchParameter || x.State.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower() || x.City.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower()).OrderByDescending(t => t.CreatedDate);
                }

                if (result != null)
                {
                    ViewBag.TotalOrder = result.Count();
                }
                else
                {
                    ViewBag.TotalOrder = 0;
                }
                int pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
                return View(result.ToPagedList(pageNumber, PageSize));
            }
            else
            {
                var listOrder = (from item in context.EbookOrders.Where(x => x.RequestID == "EBR192074").ToList()
                                 let aspnetUser = context.AspNetUsers.First(t => t.Id == item.UserID)
                                 select new PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder
                                 {
                                     CreatedDate = item.OrderDate ?? DateTime.Now,
                                     EmailAddres = aspnetUser.Email,
                                     EmailConfirmed = item.EmaiVerified ?? false,
                                     Id = item.ID,
                                     Mobile = aspnetUser.PhoneNumber,
                                     MobileConfirmed = item.MobileVerified ?? false,
                                     RequestId = item.RequestID,
                                     SchoolName = item.SchoolName,
                                     UserId = item.UserID,
                                     Status = item.Status ?? false,
                                     State = item.State.Trim().ToLower()
                                 }).OrderByDescending(t => t.CreatedDate).ToList();
                if (listOrder != null)
                {
                    ViewBag.TotalOrder = listOrder.Count();
                }
                else
                {
                    ViewBag.TotalOrder = 0;
                }
                int pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
                return View(listOrder.ToPagedList(pageNumber, PageSize));
            }

        }

        [HttpPost]
        public ActionResult ViewEbookOrder(PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder objEbookorder)
        {
            Session.Remove("objEbookorder");
            DateTime todate = Convert.ToDateTime(objEbookorder.todate).AddDays(1);
            var context = new dbPrachiIndia_PortalEntities();
            var listOrder = (from item in context.EbookOrders.ToList()
                             let aspnetUser = context.AspNetUsers.First(t => t.Id == item.UserID)
                             select new PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder
                             {
                                 CreatedDate = item.OrderDate ?? DateTime.Now,
                                 EmailAddres = aspnetUser.Email,
                                 EmailConfirmed = item.EmaiVerified ?? false,
                                 Id = item.ID,
                                 Mobile = aspnetUser.PhoneNumber,
                                 MobileConfirmed = item.MobileVerified ?? false,
                                 RequestId = item.RequestID,
                                 SchoolName = item.SchoolName,
                                 UserId = item.UserID,
                                 Status = item.Status ?? false,
                                 City = item.City,
                                 State = item.State.Trim().ToLower()
                             });
            IEnumerable<PrachiIndia.Portal.Areas.CPanel.Models.EbookOrder> result;
            result = listOrder;

            if (objEbookorder.fromdate != null)
            {
                DateTime fromdate = Convert.ToDateTime(objEbookorder.fromdate);
                result = result.Where(x => x.CreatedDate > fromdate);
            }

            if (objEbookorder.todate != null)
            {
                DateTime fromdate = Convert.ToDateTime(objEbookorder.fromdate);
                result = result.Where(x => x.CreatedDate < todate);
            }
            if (!string.IsNullOrEmpty(objEbookorder.SearchParameter))
            {
                result = result.Where(x => x.RequestId == objEbookorder.SearchParameter || x.SchoolName.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower() || x.EmailAddres.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower() || x.Mobile == objEbookorder.SearchParameter || x.State.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower() || x.City.Trim().ToLower() == objEbookorder.SearchParameter.Trim().ToLower()).OrderByDescending(t => t.CreatedDate);
            }

            if (result != null)
            {
                ViewBag.TotalOrder = result.Count();
            }
            else
            {
                ViewBag.TotalOrder = 0;
            }
            int pageNumber = 1;

            Session["objEbookorder"] = objEbookorder;
            return View(result.ToPagedList(pageNumber, PageSize));

        }

        public ActionResult VieworderForm(string RequestID)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                List<Sql.EbookOrder> lst = context.EbookOrders.Where(x => x.RequestID == RequestID).ToList();
                return View("VieworderForm", "", lst);
            }
        }

        //public PartialViewResult VerifyOrder(string RequestID)
        //{
        //    using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
        //    {
        //        List<EbookOrder> lst = context.EbookOrders.ToList();
        //        return PartialView("_ViewEbookOrder",lst);
        //    }
        //}
        [HttpPost]
        public JsonResult VerifyOrder(string RequestID)
        {
            var context = new dbPrachiIndia_PortalEntities();

            var Ebook = context.EbookOrders.FirstOrDefault(x => x.RequestID == RequestID);
            if (Ebook != null)
            {
                var objEbookOrderDetails = new EbookOrderDetails();

                int count = context.EbookOrders.Where(x => x.SchoolEmail == Ebook.SchoolEmail).Count();

                var aspNetuser = context.AspNetUsers.FirstOrDefault(t => t.Id == Ebook.UserID);
                if (aspNetuser != null)
                {
                    aspNetuser.DeviceCount = Ebook.NoOfSystem ?? 3;
                    context.Entry(aspNetuser).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    objEbookOrderDetails.UserID = aspNetuser.UserName;
                    objEbookOrderDetails.Password = context.EbookOrders.FirstOrDefault(x => x.RequestID == RequestID).DefaultPassword;
                    var courseBooks = context.EbookOrderSubjects.Where(t => t.RequestID == RequestID).ToList();
                    foreach (var courseBook in courseBooks)
                    {
                        var cl = courseBook.Class ?? 0;
                        var classId = Convert.ToString(cl);

                        var Board = courseBook.Board ?? 0;
                        var BoardId = Convert.ToString(Board);
                        var catalogs = context.tblCataLogs.Where(t => t.BoardId == BoardId && t.SubjectId == courseBook.Subject && t.SeriesId == courseBook.Series && t.ClassId == classId && t.Status == 1).ToList();
                        if (catalogs != null)
                        {
                            foreach (var catalog in catalogs)
                            {
                                var library = new UserLibrary
                                {
                                    BookId = catalog.Id,
                                    CreatedDate = DateTime.Now,
                                    EncriptionKey = catalog.EncriptionKey,
                                    EPubName = catalog.Ebookname,
                                    Id = Guid.NewGuid().ToString(),
                                    PublishDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now,
                                    UserId = aspNetuser.Id
                                };
                                context.UserLibraries.Add(library);
                                context.SaveChanges();
                            }
                        }
                    }
                    var mailfrom = ConfigurationManager.AppSettings["mailfrom"].ToString(CultureInfo.InvariantCulture);
                    var subject = "Prachi [india] welcome mail";
                    var message = GetMessage(objEbookOrderDetails, count, RequestID);
                    string message3 = "Dear Member, <br/>Your Order has been confirmed for " + Ebook.SchoolName + " against PO No# :" + RequestID + " has been approved</p>";
                    //Portal.Framework.Utility.SendMail(subject, message, Ebook.SchoolEmail, mailfrom, null);
                    //Portal.Framework.Utility.SendMail(subject, message3, Ebook.SEmail, mailfrom, Ebook.MEmail);

                    Mail.SendMail(Ebook.SchoolEmail, subject, message);
                    Mail.SendMail(Ebook.SEmail, Ebook.MEmail, subject, message3);

                    objEbookOrderDetails.VerifyOrder(RequestID, User.Identity.GetUserId());
                }
                return Json("true");
            }
            return Json("false");
        }

        private string GetMessage(EbookOrderDetails objEbookOrderDetails, int count, string RequestID)
        {
            string message = string.Empty;

            message = "<table style = 'width: 800px; margin: 0 auto; border: solid 1px #d7d6c9; font-family: 'arial', sans-serif; font-weight: normal; font-size: 14px;' cellspacing = '0' cellpadding = '0'>" +
                     "<tbody><tr><td><p><img src = 'https://preview.ibb.co/e4Onaw/logo.png' style = 'width: 140px; float: right; padding: 14px 0;'></p></td></tr><tr><td style = 'padding: 20px; color: #000000;'>" +
                      "<p> Dear User" + objEbookOrderDetails.UserName + ",</p>" +
                      "<p> Greetings from Prachi Publication !</p><br />" +
                      "<p> Your E-book order against PO NO# :" + RequestID + " has been approved and soon will be dispatched to you</p>";
            if (count == 1)
            {
                message = message + "<p style = 'margin: 0;' ><b> User ID </b> : " + objEbookOrderDetails.UserID + "</ p>" +
                "<p style = 'margin: 0;' ><b> Password </b> : " + objEbookOrderDetails.Password + "</p>";
            }

            message = message +
              "</td></tr><tr><td style = 'padding: 20px; color: #000000;'><p style = 'width:100%; float:left;'>Thanks & Regards <br/>" +
              "<img src = 'https://preview.ibb.co/e4Onaw/logo.png' style = 'width: 120px; float: left; padding: 10px 0 0 0;'></p>" +
              "<p style = 'width:100%; float:left;margin:0;'> 309 / 10, ALLIED HOUSE, INDER LOK, DELHI - 110035 <br /> 011 - 47320666(8 Lines) <br /> 011 - 43852438, 47320680 <br/> www.prachiindia.com <br/> info@prachiindia.com </p>" +
              "</td></tr> </tbody> </table>";
            return message;
        }

        [HttpPost]
        public JsonResult ResendverificationMail(string RequestID)
        {
            EbookOrderDetails objEbookOrderDetails = new EbookOrderDetails();
            objEbookOrderDetails.ResendConfirmationMail(RequestID);
            return Json(true);
        }


        public ActionResult Sticker_Preview(string RequestID)
        {
            var EbookOrderSubjectRepositories = new EbookOrderSubjectRepository();
            var SeriesList = EbookOrderSubjectRepositories.GetByID(x => x.RequestID == RequestID).Select(x => new { Series = x.Series }).ToList().Distinct();

            var MasterSeriesRepository = new MasterSeriesRepositories();
            //var DVDType = MasterSeriesRepository.GetAll().Where(x => SeriesList.Contains(y=>y.Id== x.Id)).ToList();

            var DVDReposirtoy = new DVDRepository();
            //var DVDM= DVDReposirtoy.GetAll().Where(x => DVDType.Contains(y => y.DvdType == x.Id)).ToList();
            var EbookOrderRepository = new EbookOrderRepositry();
            var EbookOrder = EbookOrderRepository.GetByID(x => x.RequestID == RequestID).FirstOrDefault();

            StickerVM stickerVM = new StickerVM();
            stickerVM.lstDVD = new List<DVDMaster>();
            stickerVM.SchoolName = EbookOrder.SchoolName;
            stickerVM.PONumber = EbookOrder.RequestID;
            stickerVM.Email = EbookOrder.SchoolEmail;
            stickerVM.MObileNo = EbookOrder.SchoolMobile;
            stickerVM.Address = EbookOrder.SchoolAddress;
            var list = new List<string>();
            foreach (var item in SeriesList)
            {
                var DVDType = MasterSeriesRepository.GetByID(x => item.Series == x.Id).FirstOrDefault();
                var DVDMaster = DVDReposirtoy.GetByID(x => DVDType.DvdType == x.Id).ToList();
                foreach (var dvd in DVDMaster)
                {
                    DVDMaster dVDMaster = new DVDMaster();
                    dVDMaster.Name = dvd.Name;
                    stickerVM.lstDVD.Add(dVDMaster);
                    if (!list.Contains(dvd.Name))
                    {
                        list.Add(dvd.Name);
                    }
                }
            }
            var dvdTitle = string.Empty;
            var dvdids = list.Distinct().ToList();
            foreach (var title in dvdids)
            {
                if (string.IsNullOrWhiteSpace(dvdTitle))
                    dvdTitle = title;
                else
                    dvdTitle = dvdTitle + " ,  " + title;
            }
            stickerVM.Dvds = dvdTitle;
            return View("Sticker_Preview", stickerVM);
        }
    }
}