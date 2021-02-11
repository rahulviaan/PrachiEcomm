using Ionic.Zip;
using Microsoft.AspNet.Identity;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Areas.Sales.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Areas.Sales.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales/Sales
        [Authorize]
        [HttpGet]
        public ActionResult Index(string serach = "")
        {
            var username = User.Identity.GetUserName().ToLower();
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var scholIds = new List<string>();
            if (string.IsNullOrWhiteSpace(serach))
            {
                scholIds = (from item in context.EbookOrders.Where(t => t.SEmail.ToLower() == username).ToList()
                            select item.UserID).ToList();
            }
            else
            {
                scholIds = (from item in context.EbookOrders.Where(t => t.SEmail.ToLower() == username && t.SchoolName.ToLower().Contains(serach)).ToList()
                            select item.UserID).ToList();
            }
            var results = (from asp in context.AspNetUsers.Where(t => scholIds.Contains(t.Id)).ToList()
                           select new SalesModel
                           {
                               DeviceCount = asp.DeviceCount ?? 0,
                               Id = asp.Id,
                               Email = asp.Email,
                               Mobile = asp.PhoneNumber,
                               Name = asp.FirstName,
                               Area = string.Format("{0}, {1}, {2}", asp.Address, asp.City, asp.State)
                           }).OrderBy(t => t.Name).ToList();
            return View(results);
        }

        public ActionResult ManageSchool(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return RedirectToAction("Index");
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var aspnetUser = context.AspNetUsers.FirstOrDefault(t => t.Id == Id);
            if (aspnetUser == null)
                return RedirectToAction("Index");
            var appUser = new SalesModel
            {
                Email = aspnetUser.Email,
                EmailConfirmed = aspnetUser.EmailConfirmed,
                Name = aspnetUser.FirstName,
                Id = aspnetUser.Id,
                Mobile = aspnetUser.PhoneNumber,
                MobileConfirmed = aspnetUser.PhoneNumberConfirmed,
                DeviceCount = (long)(aspnetUser.DeviceCount ?? 0)
            };
            if (appUser.DeviceCount <= 0)
            {
                var ebook = context.EbookOrders.FirstOrDefault(t => t.UserID == Id && t.NoOfSystem > 0);
                appUser.DeviceCount = ebook != null ? (long)ebook.NoOfSystem : 0;
            }
            return View(appUser);
        }
        [HttpPost]
        public ActionResult ManageSchool(SalesModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();

                var aspnetUser = context.AspNetUsers.FirstOrDefault(t => t.Id == model.Id);
                if (aspnetUser == null)
                {
                    ModelState.AddModelError("Name", "School not found");
                    return View(model);
                }
                var newemailObj = context.AspNetUsers.FirstOrDefault(t => t.UserName.ToLower() == model.Email.ToLower());
                if (newemailObj != null && newemailObj.Id != aspnetUser.Id)
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(model);
                }
                var newmobileObj = context.AspNetUsers.FirstOrDefault(t => t.PhoneNumber.ToLower() == model.Mobile.ToLower());
                if (newmobileObj != null && newmobileObj.Id != aspnetUser.Id)
                {
                    ModelState.AddModelError("Mobile", "Mobile already registered.");
                    return View(model);
                }
                if (!model.EmailConfirmed)
                {
                    aspnetUser.UserName = model.Email;
                    aspnetUser.Email = model.Email;
                }
                if (!model.MobileConfirmed)
                {
                    aspnetUser.PhoneNumber = model.Mobile;
                }
                aspnetUser.FirstName = model.Name;
                aspnetUser.DeviceCount = (int)model.DeviceCount;

                var ebook = context.EbookOrders.First(t => t.UserID == aspnetUser.Id && t.DefaultPassword != null);
                ebook.SchoolEmail = model.Email;
                ebook.SchoolMobile = model.Mobile;
                ebook.SchoolName = model.Name;
                context.EbookOrders.Attach(ebook);
                context.Entry(ebook).State = System.Data.Entity.EntityState.Modified;
                //context.SaveChanges();

                var readedgelogins = context.ReadEdgeLogins.FirstOrDefault(x => x.Userid == model.Id);

                readedgelogins.AllowedSystems = Convert.ToInt32(model.DeviceCount);
                readedgelogins.CurrentLogins = 0;
                readedgelogins.LoginAllowed = true;
                context.Entry(readedgelogins).State = System.Data.Entity.EntityState.Modified;


                //context.SaveChanges();

                var readedgeuserlogininfo = context.ReadEdgeUserLoginInfoes.Where(x => x.Userid == model.Id);

                context.ReadEdgeUserLoginInfoes.RemoveRange(readedgeuserlogininfo);

                context.AspNetUsers.Attach(aspnetUser);
                context.Entry(aspnetUser).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex.Message.ToString());
                return View(model);
            }

        }
        public ActionResult SchoolDetail(string Id)
        {
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var schoolName = context.EbookOrders.FirstOrDefault(t => t.UserID == Id);
            ViewBag.SchoolName = schoolName;
            var aspnetUser = context.AspNetUsers.FirstOrDefault(t => t.Id == Id);
            if (aspnetUser == null)
                return View("Index");

            ViewBag.User = aspnetUser;
            var ebookOrders = context.EbookOrders.Where(t => t.UserID == Id).OrderByDescending(t => t.OrderDate).ToList();
            return View(ebookOrders);
        }
        public FileResult DownloadFile(string RequestID, string UserType = "")
        {
            var userId = User.Identity.GetUserId();
            //  var schoolEmail = (from item in context.EbookOrders.Where(t => t.SEmail.ToLower() == username).ToList()
            // select item.UserID).ToList()

            var EncConfiguration = new EbookHelper().CreateConfiguration(userId, RequestID, UserType);
            string path = Server.MapPath("~/ModuleFiles/TempFiles/" + RequestID + "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var filename = "PO_Encrypted.xml";
            var fullPath = path + filename;

            var writer = new StreamWriter(fullPath);
            writer.Write(EncConfiguration);
            writer.Close();
            var zipFile = new ZipFile();
            zipFile.AddFile(fullPath, userId);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AppendHeader("content-disposition", "attachment; filename=" + userId + ".zip");
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
        public ActionResult EbookOrder(string Id)
        {
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var orderSubject = context.EbookOrderSubjects.Where(t => t.RequestID == Id).ToList();
            var boardId = (from board in orderSubject
                           select board.Board.ToString()).Distinct().ToList();
            var classId = (from board in orderSubject
                           select board.Class.ToString()).Distinct().ToList();
            var subjectId = (from board in orderSubject
                             select board.Subject).Distinct().ToList();
            var seriesId = (from board in orderSubject
                            select board.Series).Distinct().ToList();
            var query = context.tblCataLogs.Where(t => subjectId.Contains((int)t.SubjectId)).Where(t => seriesId.Contains((int)t.SeriesId));
            var books = query.Where(t => boardId.Contains(t.BoardId)).Where(t => classId.Contains(t.ClassId)).ToList();
            return View(books);
        }

        



    }
}