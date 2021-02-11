using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using PrachiIndia.Portal.Helpers;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using PrachiIndia.Portal.Framework;
using System.Web.Mail;
using System.Net.Mail;
using System.Net;
using MailServiece;
using PrachiIndia.Portal.Models;
namespace PrachiIndia.Portal.Controllers
{
    public class HomeController : Controller
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

       
        public async Task<ActionResult> Index()
        {
             //var pass = Encryption.DecryptCommon("GAN4GtNZTm4HpQF+c+zjyw==");

            //var con = new SqlConnection(ConfigurationManager.ConnectionStrings["_ConSqlSever"].ToString());
            //var query = "select * from [dbo].[TempBooks]";
            //var cmd = new SqlCommand(query, con);
            //var da = new SqlDataAdapter(cmd);
            //var dt = new DataTable();
            //da.Fill(dt);
            //var context = new dbPrachiIndia_PortalEntities();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    var pr = Convert.ToDecimal(Convert.ToString(dr["Price"]));
            //    var tbl = new tblCataLog
            //    {
            //        Author = Convert.ToString(dr["Author"]),
            //        Title = Convert.ToString(dr["Title"]),
            //        ClassId = Convert.ToString(dr["Class"]),
            //        SubjectId = Convert.ToInt64(Convert.ToString(dr["Subject"])),
            //        SeriesId = Convert.ToInt64(Convert.ToString(dr["Series"])),
            //        BoardId = Convert.ToString(dr["Board"]),
            //        ISBN = Convert.ToString(dr["ISBN"]),
            //        Price = pr,
            //        PrintPrice = pr,
            //        EbookPrice = pr,
            //        Tax = 1
            //    };
            //    context.tblCataLogs.Add(tbl);
            //    context.SaveChanges();
            //}
            //var control = new Microsoft.Securities.ApplicationPool.AdministrationManager();
            //control.ResetApplicationPool();
            return View();
        }
        //Except contect all are static page now.
        public ActionResult About(string id = "")
        {
            ViewBag.QS = id;
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Publications()
        {
            return View();
        }
        public ActionResult TeacherasSistance()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        public ActionResult NewsEvents()
        {
            return View();
        }
        //contct now  dynamic and contect detail from db
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var CompanyRepo = new CompanyAddressReposetory();
            IQueryable<tblCompanyAddress> Query = CompanyRepo.GetAll();
            var Result = Query.ToList();
            return View(Result);
        }
        //[HttpPost]
        //public JsonResult ContactUs(string message)
        //{
        //    var subject = "Mail From Prachiindia.com";
        //    var mailTo = "rahul9179@gmail.com";
        //   // var status = PrachIndia.api.Framework.Utility.SendMail(subject, message, mailTo, "");
        //    var result = 0;//status > 0 ? "success" : "";
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult TermsofUse()
        {
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult DigitizationServices()
        {
            return View();
        }
        public ActionResult PrintingServices()
        {
            return View();
        }
        public ActionResult ConversionServices()
        {
            return View();
        }
        public ActionResult BrowseCatalogue()
        {
            return View();
        }
        public ActionResult DigitalRightManagment()
        {
            return View();
        }
        public ActionResult Career()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }
        public ActionResult NewArrivals()
        {
            return View();
        }
        public ActionResult EbookServices()
        {
            return View();
        }
        public ActionResult K12Publishing()
        {
            return View();
        }
        public ActionResult eBookSeries()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult FutureTrack()
        {
            return View();
        }
        public ActionResult HaryanaBoard()
        {
            return View();
        }
        public ActionResult UPBoard()
        {
            return View();
        }

        public ActionResult SeriesDetail(int id = 0)
        {
            var conext = new dbPrachiIndia_PortalEntities();
            var seriesModel = conext.MasterSeries.FirstOrDefault(t => t.Id == id && t.Status == 1);
            if (seriesModel == null)
                return RedirectToAction("Series");
            var subMaster = conext.MasterSubjects.FirstOrDefault(t => t.Id == seriesModel.SubjectId);

            var model = new PrachiIndia.Portal.Models.SeriesVModel
            {
                Description = seriesModel.Description,
                Id = seriesModel.Id,
                Image = seriesModel.Image,
                Title = seriesModel.Title,
                SubjectId = seriesModel.SubjectId ?? 0,
                SubjectName = subMaster == null ? "" : subMaster.Title
            };
            var userId = User.Identity.GetUserId();
            var otherThenIndia = false;

                var users = new dbPrachiIndia_PortalEntities().AspNetUsers.Where(t => t.Id == userId).FirstOrDefault();
                if (users != null)
                {
                     if(users.CountryId != 0 && users.CountryId != 86)
                            otherThenIndia = true;
                }
                ViewBag.Other = otherThenIndia;
                model.Items = new List<tblCataLog>();
                if (seriesModel.tblCataLogs != null && seriesModel.tblCataLogs.Any())
                {
                    foreach (var item in seriesModel.tblCataLogs.OrderBy(t => t.BoardId))
                    {
                        var classMaster = conext.MasterClasses.FirstOrDefault(t => t.Id.ToString() == item.ClassId);
                        var BoardMaster = conext.MasterBoards.FirstOrDefault(t => t.Id.ToString() == item.BoardId);
                        item.ClassId = classMaster == null ? "" : classMaster.Title;
                        item.BoardId = BoardMaster == null ? "" : BoardMaster.Title;
                        model.Items.Add(item);
                    }
                }
            
            
            return View(model);
        }
        public ActionResult Series(long Id = 0)
        {
            var conext = new dbPrachiIndia_PortalEntities();
            var subjects = conext.MasterSubjects.Where(t => t.Status == 1).OrderBy(t => t.OredrNo).ToList();
            dynamic seriesModels;
            seriesModels = conext.MasterSeries.Where(t => t.Status == 1 && t.SubjectId == Id && t.ShowAtHome == true).OrderBy(t => t.OredrNo).ToList();
            if (seriesModels == null || seriesModels.Count <= 0)
            {
                Id = subjects.First().Id;
                seriesModels = conext.MasterSeries.Where(t => t.Status == 1 && t.SubjectId == Id && t.ShowAtHome == true).OrderBy(t => t.OredrNo).ToList();
            }
            ViewBag.Subjects = subjects;
            return View(seriesModels);
        }
        public ActionResult Download()
        {
            return View();
        }
        public JsonResult ContactUs(ContactUs ContactUs)
        {
            var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString(CultureInfo.InvariantCulture);
            var subject = "Contact Us";
            var message = "<p>&nbsp;" + ContactUs.Message + "</p>" +
                            "<p> &nbsp;</p><p><span style = 'color: #333399;'><strong>" +
                            "&nbsp; Name: " + ContactUs.Name + "</strong></span></p><p><span style = 'color: #333399;'>" +
                            "<strong>  &nbsp; Contact No: " + ContactUs.ContactNo + " </strong></span></p>" +
                            "<p><span style = 'color: #333399;'><strong> &nbsp; " +
                            "E-mail Id : " + ContactUs.Email + " </strong></span></p>";
            // var x = Portal.Framework.Utility.SendMail(subject, message, MailFrom, MailFrom, null);

            Mail.SendMail(MailFrom, subject, message);
            //if (x > 0)
            //{
            var MailTo_Resoponse = ContactUs.Email;
            var subject_Resoponse = "Acknowledgement";
            var response = "<table border='0' cellspacing='0' cellpadding='0' align='center' style='border:1px solid #333;width:90%'>" +
                          "<tbody><tr><td style = 'padding:20px 20px 20px 30px; font-size: 20px;'> Thank you for getting in touch!</td></tr>" +
                          "<tr><td valign = 'top' style = 'text-align:left;font-family:Calibri,sans-serif;color:#000000;background:#f3efed; padding:10px 10px 10px 30px'>" +
                          "We appreciate you contacting us.One of our Prachi India Pvt Ltd colleagues will contact you shortly.</td></tr>" +
                          "<tr><td valign = 'top' style = 'text-align:left;font-family:Calibri,sans-serif;color:#000000;background:#f3efed; padding:10px 10px 10px 30px'>Please do not reply to this message.This email is an automated notification, which is unable to receive replies.If you have any questions or concerns that require an immediate response, please call us directly at one of the following address and numbers:</td></tr>" +
                          "<tr><td valign = 'top' style = 'text-align:left;font-family:Calibri,sans-serif;color:#000000;background:#f3efed; padding:10px 10px 10px 30px'>" +
                          "<b> Prachi[India] Pvt.Ltd.</b><br /> 309 / 10, ALLIED HOUSE, INDER LOK, DELHI - 110035 <br /> 011 - 47320666(8 Lines) <br /> 011 - 43852438, 47320680 <br /> www.prachiindia.com <br /> info@prachiindia.com</td></tr>" +
                          "<tr><td valign = 'top' style = 'text-align:left;font-family:Calibri,sans-serif;color:#000000;background:#f3efed; padding:10px 10px 10px 30px'><img src = 'https://ci5.googleusercontent.com/proxy/W6S58YMJmnzGZOfg9sI4wW5O3VqI-FiZRQeBs6sd8SUeMC5glCnguo-khYu-atEAgavBQHmHLtU2=s0-d-e1-ft#http://prachiindia.com/img/logo.png' alt = 'logo' class='CToWUd' style='width:172px; height:50px;'></td></tr></tbody></table>";
            //var y = Portal.Framework.Utility.SendMail(subject_Resoponse, response, MailTo_Resoponse, MailFrom, null);
            MailServiece.Mail.SendMail(MailTo_Resoponse, subject_Resoponse, response);
            Mail.SendMail(MailTo_Resoponse, subject_Resoponse, response);
            return Json("Message sent", JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json("Message sending failed", JsonRequestBehavior.AllowGet);
            //}
        }

        //public ActionResult Verify(string Id)
        //{
        //    if (!string.IsNullOrWhiteSpace(Id))
        //    {
        //        var UserId = Encryption.DecryptCommon(Id);
        //        var context = new dbPrachiIndia_PortalEntities();
        //        var aspnetUser = context.AspNetUsers.FirstOrDefault(t => t.Id == UserId);
        //        if (aspnetUser != null)
        //        {
        //            aspnetUser.EmailConfirmed = true;
        //            aspnetUser.dtmUpdate = DateTime.Now;
        //            context.AspNetUsers.Attach(aspnetUser);
        //            context.Entry(aspnetUser).State = System.Data.Entity.EntityState.Modified;
        //            context.SaveChanges();
        //            return View("Index");
        //        }
        //        else
        //        {
        //            return RedirectToAction("");
        //        }

        //    }

        //    return RedirectToAction("");
        //}
        public FileResult Setup(int SetupType = 0)
        {
            string SetupName = string.Empty;
            string fullName = string.Empty;
            if (SetupType == 1)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/Readedge_Setup.zip");
                SetupName = "Readedge Setup.zip";
            }
            else if (SetupType == 2)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/.NET Framework 4.5.zip");
                SetupName = ".NET Framework.zip";
            }
            else if (SetupType == 3)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/Win Vista 32bit.zip");
                SetupName = "Win Vista 32bit.zip";
            }
            else if (SetupType == 4)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/Win7 32 bit.zip");
                SetupName = "Win7 32bit.zip";
            }
            else if (SetupType == 5)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/Win7 64 bit.zip");
                SetupName = "Win7 64bit.zip";
            }
            else if (SetupType == 6)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/ReadEdge_Package.zip");
                SetupName = "Readedge Package.zip";
            }
            else if (SetupType == 7)
            {
                fullName = Server.MapPath("~/ModuleFiles/ReadEdge_Package/ReadEdge-Enterprise-Setup.zip");
                SetupName = "ReadEdge-Enterprise-Setup.zip";
            }

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, SetupName);
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public ActionResult WorkShop()
        {
            return View();
        }
        public ActionResult Excepction()
        {
            return View();
        }

        public ActionResult BanglaBoard()
        {
            return View();
        }
    }
}
