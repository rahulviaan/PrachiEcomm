using PrachiIndia.Portal.Areas.PO.Models;
using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Portal;
using PrachiIndia.Portal.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using System.Globalization;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Sql.CustomRepositories;
using MailServiece;
using PrachiIndia.Portal.Areas.CPanel.Models;
using PrachiIndia.Portal.Models;
using System.Threading.Tasks;
using PrachiIndia.Portal.Areas.Sales.Models;
using System.Data.Entity;

namespace PrachiIndia.Portal.Areas.PO.Controllers
{
    [RouteArea("PO", AreaPrefix = "")]
    [RoutePrefix("Teacher")]
    public class POController : Controller
    {
        static dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
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
        // GET: PO/PO
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult EbookOrder()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreatePO(POMaster objPOMaster)
        {
            try
            {
                objPOMaster.GeneratePO(objPOMaster);
                return Json(true, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.DenyGet);
            }
        }

        public ActionResult ViewOrder(string RequestID)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                string Order = context.EbookOrders.FirstOrDefault(x => x.RequestID == RequestID).Message;
                ViewBag.Message = Order;
                return View();
            }
        }

        [HttpPost]
        public JsonResult AssistanceMail(Assistance AssistanceObj)
        {
            string RequiredFor = "RequiredFor";
            string[] cityDesc = AssistanceObj.CityDesc.Split(new string[] { "||" }, StringSplitOptions.None);
            AssistanceObj.City = cityDesc[0];
            AssistanceObj.State = cityDesc[1];
            AssistanceObj.Country = cityDesc[2];
            AssistanceObj.Type = "";
            string Heading = "e-book Order Form";
            var user = new ApplicationUser
            {
                UserName = AssistanceObj.School_Email.ToLower(),
                Email = AssistanceObj.School_Email.ToLower(),
                Address = AssistanceObj.School_Address,
                FirstName = AssistanceObj.SchoolName,
                Country = AssistanceObj.Country,
                State = AssistanceObj.State,
                City = AssistanceObj.City,
                PinCode = "",
                PhoneNumber = AssistanceObj.School_Mobile,
                idServer = 0,
                dtmAdd = DateTime.Now,
                ProfileImage = "/UserProfileImage/Noimage.jpg"
            };
            var Password = MessageSent.GeneratetOTP(6);
            AssistanceObj.DefaultPassword = Password;
            var result = UserManager.Create(user, Password);

            if (result.Succeeded)
            {
                Areas.CPanel.Models.EbookOrderDetails.CheckAndCreateRoles();
                var aspNetuser = UserManager.FindByEmail(user.UserName);
                if (aspNetuser != null)
                {
                    UserManager.AddToRoleAsync(aspNetuser.Id, Roles.School);
                    AssistanceObj.UserID = aspNetuser.Id;
                    //var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString(CultureInfo.InvariantCulture);
                    var subject = Heading;

                    //Getting Datatable 
                    DataTable dt = GetDataTable(AssistanceObj);
                    var message = GetMessage(AssistanceObj, RequiredFor, Heading, cityDesc);
                    string RequestID = AssistanceObj.CreateOrder(AssistanceObj, dt);
                    if (!string.IsNullOrEmpty(RequestID))
                    {
                        VerifyOrder(RequestID);
                        MessageSent.SendSMS(AssistanceObj.School_Mobile, "Thank you for placing your order, your PO NO : " + RequestID);
                        var encriptedUserId = aspNetuser.Id;
                        // var callbackUrl = Url.Action("VerifyDetails", "UserVerification", new { Id = encriptedUserId, RequestID = RequestID, Area = "CPanel" }, protocol: Request.Url.Scheme);
                        var emailVerifiedMessage = "Dear " + AssistanceObj.SchoolName + ", <br/> <p>Thank you for placing you order, <br/><br/>PO NO :" + RequestID + ".</p> <br /><br/>To access your purchased ebooks, please go to <a href=/'http://readedge.mielib.com/'>http://readedge.mielib.com</a> and enter the below mentioned credentials.Your Credentials are <p style = 'margin: 0;'><b> User ID </b> : " + user.UserName + " </p><p style = 'margin: 0;' ><b> Password </b> : " + AssistanceObj.DefaultPassword + "</p> <br /><p>If you face any trouble in accessing your account, please write us on marketing@prachigroup.com or call our support number +91-9142626262.</p><br/>Regards<br/>Team Digital Support<br/>Prachi Group<br/><br/>Below are the details of your order<br/>";
                        string message2 = emailVerifiedMessage + message;
                        Mail.SendMail(AssistanceObj.School_Email, AssistanceObj.SEmail, AssistanceObj.MEmail, subject, message2);
                    }
                }

                else
                {
                    return Json("Message sending failed", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("user already exists", JsonRequestBehavior.AllowGet);
            }

            return Json("Message sent", JsonRequestBehavior.AllowGet);
        }

        private static string GetMessage(Assistance AssistanceObj, string RequiredFor, string Heading, string[] cityDesc)
        {
            Int64 boardid = Convert.ToInt64(AssistanceObj.Board);
            var Board = context.MasterBoards.FirstOrDefault(x => x.Id == boardid).Title;
            var itemRepository = new CatalogRepository();
            IQueryable<tblCataLog> query = itemRepository.GetAll();
            //var result = query.Where(s => s.SubjectId == SubjectID).Select(x => new { SeriesID = x.Id, Series = x.Title }).ToList();

            string message = string.Empty;
            if (Heading == "e-book Order Form")
            {
                message = "<table style='width:800px; margin:0 auto; border: solid 4px #164e71; background-color:#ffffff;' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td colspan = '4' style ='text-align:center; background-color:#123754; color:#ffffff;'><h2 style ='color:#ffffff;'> " + Heading + " </h2></td>" +
                                      "</tr>" +
                                      "<tr>" +
                                      "<td colspan = '4' style = 'background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'> School Details </h3></td>" +
                                      "</tr>" +
                                      "<tr>" +
                                      "<td width ='20%' style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td>" +
                                       "<td width ='5%' style='border: solid 1px #feefef; padding:10px;'>:-</td>" +
                                           "<td width ='75%' style='border: solid 1px #feefef; padding:10px;' colspan = '2'>" + AssistanceObj.SchoolName.ToUpper() + "</td>" +
                                       "</tr>" +

                                          "<tr>" +
                                          "<td style='border: solid 1px #feefef; padding:10px;'><b>Principal</b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.Principal_Name.ToUpper() + "</td>" +
                                          "</tr>" +
                                           "<tr>" +
                                           "<td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td> <td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.School_Email + " </td>" +
                                            "</tr>" +
                                            "<tr>" +
                                            "<td style='border: solid 1px #feefef; padding:10px;'><b> Phone </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.School_Landline + "</td>" +
                                            "</tr>" +
                                            "<td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.School_Mobile + "</td>" +
                                            "</tr>" +
                                            "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Address </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + (AssistanceObj.School_Address ?? "NA").ToUpper() + "</td>" +
                                            "</tr>" +
                                              "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> IT Incharge </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + (AssistanceObj.ITIncharge ?? "NA").ToUpper() + "</td>" +
                                               "</tr><tr> <td style='border: solid 1px #feefef; padding:10px;'><b> City </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.City.ToUpper() + "</td>" +
                                                "</tr>" +
                                                "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Satate </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.State.ToUpper() + "</td>" +
                                                "</tr>" +
                                                 "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Coutry </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.Country.ToUpper() + "</td>" +
                                                "</tr>" +
                                                  "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Strength </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.Strength + "</td>" +
                                                "</tr>" +
                                                  "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> No of System </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.NoOfSystem + "</td>" +
                                                "</tr>" +
                                                  "<tr><td style='border: solid 1px #feefef; padding:10px;'><b>Board</b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + Board + "</td>" +
                                                "</tr>" +
                                                 "<tr><td colspan ='4' style ='background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'> Representative Details </h3></td>" +
                                                  "</tr>" +
                                                  "<tr><td colspan='4'><table width='100%' style='width:100%; padding:10px 20px;' cellspacing='0' cellpadding='0'><tr><td colspan='4' style='background-color:#e9edeb; color:#000000; padding:7px 10px;'><h3 style='margin:0px;'> Sales </h3></td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.SName.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.SMobileNo.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Area </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.SArea.ToUpper() + "</td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.SEmail + "</td></tr><tr><td colspan='4' style='background-color:#e9edeb; color:#000000; padding:7px 10px;'><h3 style='margin:0px;'> Marketing </h3></td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.MName.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.MMobileNo.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Area </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + (AssistanceObj.MArea ?? "NA").ToUpper() + "</td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + (AssistanceObj.MEmail ?? "NA") + "</td></tr></table></td>" +
                                                  "</tr>" +
                                                  "<tr>" +
                                                         "<tr>" +
                                                         "<td colspan='4' style='border: solid 1px #feefef;'>" +
                                                         "<table style='width:100%; margin:0 auto;' cellspacing='0' cellpadding='0'>" +
                                                         "<tr>" +
                                                         "<td colspan='5' style ='background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'>Subject Details</h3></td>" +
                                                         "</tr>" +
                                                         "<tr>" +
                                                         "<td width='10%' style='border:solid 1px #feefef; padding:10px;'><b>S.No.</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Subject</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Series</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Class</b></td>" +
                                                         "</tr>";

                string message1 = string.Empty;
                if (AssistanceObj.SubjectDetail.Count > 0)
                {
                    //string message1 = string.Empty;
                    var MasterSubject = new MasterSubjectRepository();
                    IQueryable<MasterSubject> querySubject = MasterSubject.GetAll();

                    var MasterSeries = new MasterSeriesRepositories();
                    IQueryable<MasterSery> querySeries = MasterSeries.GetAll();

                    for (int num = 0; num < AssistanceObj.SubjectDetail.Count; num++)
                    {
                        if (AssistanceObj.SubjectDetail[num].Classes != null && AssistanceObj.SubjectDetail[num].Subject != null && AssistanceObj.SubjectDetail[num].Series != null)
                        {
                            string classes = string.Empty;
                            foreach (string classID in AssistanceObj.SubjectDetail[num].Classes)
                            {
                                classes = classes + ", " + PrachiIndia.Portal.Helpers.Utility.classesByClassID(classID);

                            }
                            classes = classes.Remove(classes.IndexOf(","), 1);
                            long? subjectID = Convert.ToInt32(AssistanceObj.SubjectDetail[num].Subject);
                            long? SeriesID = Convert.ToInt32(AssistanceObj.SubjectDetail[num].Series);
                            var Subject = querySubject.Where(s => s.Id == subjectID).Select(x => new { Subject = x.Title }).ToList().First().Subject.ToString();
                            var Series = querySeries.Where(s => s.Id == SeriesID).Select(x => new { Series = x.Title }).ToList().First().Series.ToString();

                            message1 = message1 +
                            "<tr>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + (num + 1) + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Subject + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Series + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + classes + "</td>" +
                            "</tr>";
                        }

                    }
                }
                else
                {
                    message1 = "<tr>" +
                        "<td style='border:solid 1px #feefef; padding:10px;' colspan='4'>No Subject Selected</td>" +
                        "</tr>";

                }
                message = message + message1 + "</td></tr></table></table> ";
            }
            else
            {

                message = "<table style='width:800px; margin:0 auto; border: solid 4px #164e71; background-color:#ffffff;' cellspacing='0' cellpadding='0'>" +
                                       "<tr>" +
                                         "<td colspan = '3' style ='text-align:center; background-color:#123754; color:#ffffff;'><h2 style ='color:#ffffff;'> " + Heading + " </h2></td>" +
                                       "</tr>" +
                                       "<tr>" +
                                       "<td colspan = '3' style = 'background-color:#c7fee1; color:#000000; padding:10px;'><h3> School Details </h3></td>" +
                                       "</tr>" +
                                       "<tr>" +
                                       "<td width ='20%' style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td>" +
                                        "<td width ='5%' style='border: solid 1px #feefef; padding:10px;'>:-</td>" +
                                            "<td width ='75%' style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.SchoolName.ToUpper() + "</td>" +
                                        "</tr>" +
                                          "<tr>" +
                                          "<td style='border: solid 1px #feefef; padding:10px;'><b> Landline </b></td>" +
                                           "<td style='border: solid 1px #feefef; padding:10px;'>:-</td> <td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.School_Landline + "</td>" +
                                          "</tr>" +
                                           "<tr>" +
                                           "<td style='border: solid 1px #feefef; padding:10px;'><b>Principal</b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.Principal_Name.ToUpper() + "</td>" +
                                           "</tr>" +
                                            "<tr>" +
                                            "<td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td> <td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.School_Email + " </td>" +
                                             "</tr>" +
                                             "<tr>" +
                                             "<td style='border: solid 1px #feefef; padding:10px;'><b> Address </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.School_Address.ToUpper() + "</td>" +
                                              "</tr>" +
                                               "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Board </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.Board.ToUpper() + " </td>" +
                                                "</tr><tr> <td style='border: solid 1px #feefef; padding:10px;'><b> City </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + cityDesc[0].ToUpper() + "</td>" +
                                                 "</tr>" +
                                                 "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Satate </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + cityDesc[1].ToUpper() + "</td>" +
                                                 "</tr>" +
                                                  "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Coutry </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'> " + cityDesc[2].ToUpper() + "</td>" +
                                                 "</tr>" +
                                                  "<tr><td colspan ='3' style ='background-color:#c7fee1; color:#000000; padding:10px;'><h3> Contact Details </h3></td>" +
                                                   "</tr>" +
                                                   "<tr>" +
                                                    "<td style='border: solid 1px #feefef; padding:10px;'><b> Requested By </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'> " + AssistanceObj.RequestedBy.ToUpper() + " </td>" +
                                                     "</tr>" +
                                                      "<tr>" +
                                                        "<td style='border: solid 1px #feefef; padding:10px;'><b> Designation </b></td> <td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.Designation.ToUpper() + "</td>" +
                                                      "</tr >" +
                                                       "<tr>" +
                                                       "<td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.SMobileNo + " </td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                        "<td style='border: solid 1px #feefef; padding:10px;'><b> Landline </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.Landline + "</td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                        "<td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.Email + "</td>" +
                                                        "</tr>" +
                                                         "<tr>" +
                                                         "<td style='border: solid 1px #feefef; padding:10px;'><b>" + RequiredFor + "</b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.RequiredFor + "</td>" +
                                                         "</tr>" +
                                                          "<tr>" +
                                                          "<td style='border: solid 1px #feefef; padding:10px;'><b> Description </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td style='border: solid 1px #feefef; padding:10px;'>" + AssistanceObj.Description.ToUpper() + "</td>" +
                                                          "</tr>" +
                                                          "</table> ";

            }
            AssistanceObj.Message = message;

            return message;
        }

        /*
        DEVELOPER NAME : RAHUL SRIVASTAVA
        DATE           : 16/10/2017
        PURPOSE        : GETTING CITY AUTOCOMPLETE LIST

     */
        public JsonResult AutoCompleteCity(string term)
        {
            CityRepositories objCityRepositories = new CityRepositories();

            var result = objCityRepositories.AutoCompleteCity(term);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllBoard()
        {
            var result = context.USP_EbookBoard().Select(x => new { BoardID = x.Id, Board = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSubject(int BoardID)
        {
            var result = context.USP_EbookSubject(BoardID).Select(x => new { SubjectID = x.id, Subject = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSubjectSample(int BoardID)
        {
            var result = context.USP_BookSubject(BoardID).Select(x => new { SubjectID = x.id, Subject = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSeriesSample(int SubjectID, int BoardID)
        {
            var result = context.USP_bookSeries(SubjectID, BoardID).Select(x => new { SeriesID = x.Id, Series = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllClassesSample(int SeriesID, int BoardID, int subject)
        {
            var result = context.USP_bookClasses(subject, BoardID, SeriesID).Select(x => new { ClassID = x.Id, Class = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllSeries(int SubjectID, int BoardID)
        {
            var result = context.USP_EbookSeries(SubjectID, BoardID).Select(x => new { SeriesID = x.Id, Series = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllClasses(int SeriesID, int BoardID, int subject)
        {
            var result = context.USP_EbookClasses(subject, BoardID, SeriesID).Select(x => new { ClassID = x.Id, Class = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public DataTable GetDataTable(Assistance AssistanceObj)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("Board"));
            dt.Columns.Add(new DataColumn("Class"));
            dt.Columns.Add(new DataColumn("Subject"));
            dt.Columns.Add(new DataColumn("Series"));
            for (int num = 0; num < AssistanceObj.SubjectDetail.Count; num++)
            {
                if (AssistanceObj.SubjectDetail[num].Classes != null && AssistanceObj.SubjectDetail[num].Subject != null && AssistanceObj.SubjectDetail[num].Series != null)
                {
                    foreach (string classID in AssistanceObj.SubjectDetail[num].Classes)
                    {
                        DataRow dataRow = dt.NewRow();
                        //dataRow["Board"] = AssistanceObj.SubjectDetail[num].Board;
                        dataRow["Class"] = classID;
                        dataRow["Subject"] = AssistanceObj.SubjectDetail[num].Subject;
                        dataRow["Series"] = AssistanceObj.SubjectDetail[num].Series;

                        dt.Rows.Add(dataRow);
                    }
                }
            }
            return dt;
        }




        public JsonResult GetSalesEmployee(string Type)
        {

            var result = Utility.GetUserByRole(Type).AsEnumerable();
            var User = (from DataRow dr in result
                        select new
                        {
                            UserID = Convert.ToString(dr["Id"]),
                            UserName = Convert.ToString(dr["Name"])
                        }).OrderBy(x => x.UserName).ToList();

            return Json(User, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetEmpDetail(string Type, string UserName)
        {

            var result = Utility.GetUserByRole(Type).AsEnumerable();
            var User = (from DataRow dr in result
                        select new
                        {
                            UserID = Convert.ToString(dr["Id"]),
                            UserName = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            Area = Convert.ToString(dr["City"]),
                            MobileNo = Convert.ToString(dr["PhoneNumber"]),
                        }).Where(x => x.UserName == UserName).OrderBy(x => x.UserName).ToList();

            return Json(User, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckUserByEmail(string Email)
        {
            string status = Utility.IsEmailExists(Email);

            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckUserByMobile(string Mobile)
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            bool status = context.AspNetUsers.Any(x => x.PhoneNumber == Mobile);

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public void VerifyOrder(string RequestID)
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
                    var userId = User.Identity.GetUserId();
                    if (string.IsNullOrWhiteSpace(userId))
                        userId = "6052f464-fbb9-4b94-9b3a-1edd0e6aa084";
                    objEbookOrderDetails.VerifyOrder(RequestID, userId);
                }
            }
        }

        [Authorize]
        public ActionResult SampleRequestForm()
        {
            return View("SampleOrderForm");
        }


        [HttpPost]
        public JsonResult SampleRequestForm(BookSample bookSample)
        {
            string RequiredFor = "RequiredFor";
        
            string Heading = "Sampling Request Form";
            var subject = Heading;

            //Getting Datatable 
            DataTable dt = GetDataTableBookSample(bookSample);
            var message = GetSampleRequestMessage(bookSample, RequiredFor, Heading);
            string RequestID = bookSample.CreateSampleRequest(bookSample, dt);
            if (!string.IsNullOrEmpty(RequestID))
            {
                //VerifyOrder(RequestID);
                MessageSent.SendSMS(bookSample.SMobileNo, "Thank you for filing sampling request form , your RequestId is " + RequestID);
                // var callbackUrl = Url.Action("VerifyDetails", "UserVerification", new { Id = encriptedUserId, RequestID = RequestID, Area = "CPanel" }, protocol: Request.Url.Scheme);
                var emailVerifiedMessage = "Dear " + bookSample.SEmail + ", <br/> please write us on marketing@prachigroup.com or call our support number +91-9142626262.</p><br/>Regards<br/>Team Digital Support<br/>Prachi Group<br/><br/>Below are the details of samples<br/>";
                string message2 = emailVerifiedMessage + message;
                Mail.SendMail(bookSample.HEmail, bookSample.SEmail, subject, message2);
            }




            return Json("Message sent", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SampleDistributionForm()
        {
            return View();
        }


        [HttpPost]
        public JsonResult SampleDistributionForm(BookSample bookSample)
        {
            string RequiredFor = "RequiredFor";
            string[] cityDesc = bookSample.CityDesc.Split(new string[] { "||" }, StringSplitOptions.None);
            bookSample.City = cityDesc[0];
            bookSample.State = cityDesc[1];
            bookSample.Country = cityDesc[2];

            string Heading = "Sample Distribution Form";
            var subject = Heading;

            bookSample.HName = ConfigurationManager.AppSettings["HName"];
            bookSample.HMobileNo = ConfigurationManager.AppSettings["HMobile"];
            bookSample.HEmail = ConfigurationManager.AppSettings["HEmail"];
            bookSample.HArea = ConfigurationManager.AppSettings["HArea"];

            //Getting Datatable 
            DataTable dt = GetDataTableBookSample(bookSample);
            var message = GetSampleDistributionMessage(bookSample, RequiredFor, Heading, cityDesc);
            string RequestID = bookSample.CreateSampleDistribution(bookSample, dt);
            if (!string.IsNullOrEmpty(RequestID))
            {
                MessageSent.SendSMS(bookSample.SMobileNo, "Thank you for filing sampling distirbution form , your RequestId is " + RequestID);
                // var callbackUrl = Url.Action("VerifyDetails", "UserVerification", new { Id = encriptedUserId, RequestID = RequestID, Area = "CPanel" }, protocol: Request.Url.Scheme);
                var emailVerifiedMessage = "Dear " + bookSample.SEmail + ", <br/> please write us on marketing@prachigroup.com or call our support number +91-9142626262.</p><br/>Regards<br/>Team Digital Support<br/>Prachi Group<br/><br/>Below are the details of samples<br/>";
                string message2 = emailVerifiedMessage + message;
                Mail.SendMail(bookSample.HEmail, bookSample.SEmail, subject, message2);
            }




            return Json("Message sent", JsonRequestBehavior.AllowGet);
        }

        public DataTable GetDataTableBookSample(BookSample bookSample)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("Board"));
            dt.Columns.Add(new DataColumn("Class"));
            dt.Columns.Add(new DataColumn("Subject"));
            dt.Columns.Add(new DataColumn("Series"));
            dt.Columns.Add(new DataColumn("Quantity"));
            for (int num = 0; num < bookSample.SampleSubjectDetails.Count; num++)
            {
                if (bookSample.SampleSubjectDetails[num].Classes != null && bookSample.SampleSubjectDetails[num].Subject != null && bookSample.SampleSubjectDetails[num].Series != null)
                {
                    foreach (string classID in bookSample.SampleSubjectDetails[num].Classes)
                    {
                        DataRow dataRow = dt.NewRow();
                        //dataRow["Board"] = AssistanceObj.SubjectDetail[num].Board;
                        dataRow["Class"] = classID;
                        dataRow["Subject"] = bookSample.SampleSubjectDetails[num].Subject;
                        dataRow["Series"] = bookSample.SampleSubjectDetails[num].Series;
                        dataRow["Quantity"] = bookSample.SampleSubjectDetails[num].Quantity;
                        dt.Rows.Add(dataRow);
                    }
                }
            }
            return dt;
        }

        private static string GetSampleRequestMessage(BookSample bookSample, string RequiredFor, string Heading)
        {
           
            var itemRepository = new CatalogRepository();

            var transportType = string.Empty;
            if (bookSample.TransportType == "SelfPickup")
            {
                transportType = "SELF PICKUP";
            }
            else
            {
                transportType = "TO BE DELIVERED";
            }

            string message = string.Empty;
          
                message = "<table style='width:800px; margin:0 auto; border: solid 4px #164e71; background-color:#ffffff;' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td colspan = '4' style ='text-align:center; background-color:#123754; color:#ffffff;'><h2 style ='color:#ffffff;'> " + Heading + " </h2></td>" +
                                      "</tr>" +
                                      
                                                  "</tr>" +
                                                  "<tr><td colspan='4'><table width='100%' style='width:100%; padding:10px 20px;' cellspacing='0' cellpadding='0'><tr><td colspan='4' style='background-color:#e9edeb; color:#000000; padding:7px 10px;'><h3 style='margin:0px;'> Sales </h3></td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SName.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SMobileNo.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Area </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SArea.ToUpper() + "</td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SEmail + "</td></tr><tr><td colspan='4' style='background-color:#e9edeb; color:#000000; padding:7px 10px;'><h3 style='margin:0px;'> Head Office </h3></td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'> " + bookSample.HName.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.HMobileNo.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Area </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + (bookSample.HArea ?? "NA").ToUpper() + "</td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + (bookSample.HEmail ?? "NA") + "</td></tr></table></td>" +
                                                  "</tr>" +
                                                  "<tr>" +
                                                         "<tr>" +
                                                         "<td colspan='4' style='border: solid 1px #feefef;'>" +
                                                         "<table style='width:100%; margin:0 auto;' cellspacing='0' cellpadding='0'>" +
                                                         "<tr>" +
                                                         "<td colspan='5' style ='background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'>Subject Details</h3></td>" +
                                                         "</tr>" +
                                                         "<tr>" +
                                                         "<td width='10%' style='border:solid 1px #feefef; padding:10px;'><b>S.No.</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Subject</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Series</b></td>" +
                                                         "<td width='20%' style='border:solid 1px #feefef; padding:10px;'><b>Class</b></td>" +
                                                         "<td width='10%' style='border:solid 1px #feefef; padding:10px;'><b>Quantity</b></td>" +
                                                         "</tr>";




                string message1 = string.Empty;
                if (bookSample.SampleSubjectDetails.Count > 0)
                {
                    //string message1 = string.Empty;
                    var MasterSubject = new MasterSubjectRepository();
                    IQueryable<MasterSubject> querySubject = MasterSubject.GetAll();

                    var MasterSeries = new MasterSeriesRepositories();
                    IQueryable<MasterSery> querySeries = MasterSeries.GetAll();

                    for (int num = 0; num < bookSample.SampleSubjectDetails.Count; num++)
                    {
                        if (bookSample.SampleSubjectDetails[num].Classes != null && bookSample.SampleSubjectDetails[num].Subject != null && bookSample.SampleSubjectDetails[num].Series != null)
                        {
                            string classes = string.Empty;
                            foreach (string classID in bookSample.SampleSubjectDetails[num].Classes)
                            {
                                classes = classes + ", " + PrachiIndia.Portal.Helpers.Utility.classesByClassID(classID);

                            }
                            classes = classes.Remove(classes.IndexOf(","), 1);
                            long? subjectID = Convert.ToInt32(bookSample.SampleSubjectDetails[num].Subject);
                            long? SeriesID = Convert.ToInt32(bookSample.SampleSubjectDetails[num].Series);
                            long? Quantity = Convert.ToInt32(bookSample.SampleSubjectDetails[num].Quantity);
                            var Subject = querySubject.Where(s => s.Id == subjectID).Select(x => new { Subject = x.Title }).ToList().First().Subject.ToString();
                            var Series = querySeries.Where(s => s.Id == SeriesID).Select(x => new { Series = x.Title }).ToList().First().Series.ToString();

                            message1 = message1 +
                            "<tr>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + (num + 1) + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Subject + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Series + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + classes + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Quantity + "</td>" +
                            "</tr>";
                        }

                    }
                }
                else
                {
                    message1 = "<tr>" +
                        "<td style='border:solid 1px #feefef; padding:10px;' colspan='4'>No Subject Selected</td>" +
                        "</tr>";

                }

                message1 = message1 + "<tr><td colspan='5' style ='background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'>Transport Details</h3></td>" +
                                                       "</tr>" +
                                                       "<tr>" +
                                                       "<td colspan='5' style='border:solid 1px #feefef; padding:10px;'>" +
                                                       "<b>Transport Type : </b>" + transportType +
                                                       "</td>" +
                                                       "</tr>";



                if (bookSample.TransportType == "SelfPickup")
                {
                    message1 = message1 + "<tr>" +
                    "<td colspan='5' style='border:solid 1px #feefef; padding:10px;'>" +
                                                        "<b>Pickup Point Location : </b>" + bookSample.PickupLocation +
                                                        "</td>" +
                                                        "</tr>";
                }
                else
                {
                    message1 = message1 + "<tr>" +
                    "<td colspan='5' style='border:solid 1px #feefef; padding:10px;'>" +
                                                        "<b>Delivery Mode : </b>" + bookSample.DeliveryMode +
                                                        "</td>" +
                                                        "</tr>" +
                                                          "<tr>" +
                    "<td colspan='5' style='border:solid 1px #feefef; padding:10px;'>" +
                                                        "<b>Depot Station : </b>" + bookSample.DepotStation +
                                                        "</td>" +
                                                        "</tr>";
                }


                message = message + message1 + "</td></tr></table></table> ";
        
           
            bookSample.Message = message;

            return message;
        }

        private static string GetSampleDistributionMessage(BookSample bookSample, string RequiredFor, string Heading, string[] cityDesc)
        {
            Int64 boardid = Convert.ToInt64(bookSample.Board);
            var Board = context.MasterBoards.FirstOrDefault(x => x.Id == boardid).Title;
            var itemRepository = new CatalogRepository();

            string message = string.Empty;
       
                message = "<table style='width:800px; margin:0 auto; border: solid 4px #164e71; background-color:#ffffff;' cellspacing='0' cellpadding='0'>" +
                                      "<tr>" +
                                        "<td colspan = '4' style ='text-align:center; background-color:#123754; color:#ffffff;'><h2 style ='color:#ffffff;'> " + Heading + " </h2></td>" +
                                      "</tr>" +
                                      "<tr>" +
                                      "<td colspan = '4' style = 'background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'> School Details </h3></td>" +
                                      "</tr>" +
                                      "<tr>" +
                                      "<td width ='20%' style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td>" +
                                       "<td width ='5%' style='border: solid 1px #feefef; padding:10px;'>:-</td>" +
                                           "<td width ='75%' style='border: solid 1px #feefef; padding:10px;' colspan = '2'>" + bookSample.SchoolName.ToUpper() + "</td>" +
                                       "</tr>" +

                                          "<tr>" +
                                          "<td style='border: solid 1px #feefef; padding:10px;'><b>Principal</b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.Principal_Name.ToUpper() + "</td>" +
                                          "</tr>" +
                                           "<tr>" +
                                           "<td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td> <td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.School_Email + " </td>" +
                                            "</tr>" +
                                            "<tr>" +
                                            "<td style='border: solid 1px #feefef; padding:10px;'><b> Phone </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + bookSample.School_Landline + "</td>" +
                                            "</tr>" +
                                            "<td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + bookSample.School_Mobile + "</td>" +
                                            "</tr>" +
                                            "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Address </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + (bookSample.School_Address ?? "NA").ToUpper() + "</td>" +
                                            "</tr>" +
                                              "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Book Incharge </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + (bookSample.BookIncharge ?? "NA").ToUpper() + "</td>" +
                                               "</tr><tr> <td style='border: solid 1px #feefef; padding:10px;'><b> City </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.City.ToUpper() + "</td>" +
                                                "</tr>" +
                                                "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Satate </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.State.ToUpper() + "</td>" +
                                                "</tr>" +
                                                 "<tr><td style='border: solid 1px #feefef; padding:10px;'><b> Coutry </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + bookSample.Country.ToUpper() + "</td>" +
                                                "</tr>" +
                                                  "</tr>" +
                                                  "</tr>" +
                                                  "<tr><td style='border: solid 1px #feefef; padding:10px;'><b>Board</b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan = '2' style='border: solid 1px #feefef; padding:10px;'> " + Board + "</td>" +
                                                "</tr>" +
                                                 "<tr><td colspan ='4' style ='background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'> Representative Details </h3></td>" +
                                                  "</tr>" +
                                                  "<tr><td colspan='4'><table width='100%' style='width:100%; padding:10px 20px;' cellspacing='0' cellpadding='0'><tr><td colspan='4' style='background-color:#e9edeb; color:#000000; padding:7px 10px;'><h3 style='margin:0px;'> Sales </h3></td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SName.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SMobileNo.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Area </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SArea.ToUpper() + "</td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.SEmail + "</td></tr><tr><td colspan='4' style='background-color:#e9edeb; color:#000000; padding:7px 10px;'><h3 style='margin:0px;'> Head Office </h3></td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Name </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'> " + bookSample.HName.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Mobile </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + bookSample.HMobileNo.ToUpper() + " </td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Area </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + (bookSample.HArea ?? "NA").ToUpper() + "</td></tr><tr><td style='border: solid 1px #feefef; padding:10px;'><b> Email </b></td><td style='border: solid 1px #feefef; padding:10px;'>:-</td><td colspan='2' style='border: solid 1px #feefef; padding:10px;'>" + (bookSample.HEmail ?? "NA") + "</td></tr></table></td>" +
                                                  "</tr>" +
                                                  "<tr>" +
                                                         "<tr>" +
                                                         "<td colspan='4' style='border: solid 1px #feefef;'>" +
                                                         "<table style='width:100%; margin:0 auto;' cellspacing='0' cellpadding='0'>" +
                                                         "<tr>" +
                                                         "<td colspan='5' style ='background-color:#c7fee1; color:#000000; padding:5px 10px;'><h3 style='margin:0px;'>Subject Details</h3></td>" +
                                                         "</tr>" +
                                                         "<tr>" +
                                                         "<td width='10%' style='border:solid 1px #feefef; padding:10px;'><b>S.No.</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Subject</b></td>" +
                                                         "<td width='30%' style='border:solid 1px #feefef; padding:10px;'><b>Series</b></td>" +
                                                         "<td width='20%' style='border:solid 1px #feefef; padding:10px;'><b>Class</b></td>" +
                                                         "<td width='10%' style='border:solid 1px #feefef; padding:10px;'><b>Quantity</b></td>" +
                                                         "</tr>";




                string message1 = string.Empty;
                if (bookSample.SampleSubjectDetails.Count > 0)
                {
                    //string message1 = string.Empty;
                    var MasterSubject = new MasterSubjectRepository();
                    IQueryable<MasterSubject> querySubject = MasterSubject.GetAll();

                    var MasterSeries = new MasterSeriesRepositories();
                    IQueryable<MasterSery> querySeries = MasterSeries.GetAll();

                    for (int num = 0; num < bookSample.SampleSubjectDetails.Count; num++)
                    {
                        if (bookSample.SampleSubjectDetails[num].Classes != null && bookSample.SampleSubjectDetails[num].Subject != null && bookSample.SampleSubjectDetails[num].Series != null)
                        {
                            string classes = string.Empty;
                            foreach (string classID in bookSample.SampleSubjectDetails[num].Classes)
                            {
                                classes = classes + ", " + PrachiIndia.Portal.Helpers.Utility.classesByClassID(classID);

                            }
                            classes = classes.Remove(classes.IndexOf(","), 1);
                            long? subjectID = Convert.ToInt32(bookSample.SampleSubjectDetails[num].Subject);
                            long? SeriesID = Convert.ToInt32(bookSample.SampleSubjectDetails[num].Series);
                            long? Quantity = Convert.ToInt32(bookSample.SampleSubjectDetails[num].Quantity);
                            var Subject = querySubject.Where(s => s.Id == subjectID).Select(x => new { Subject = x.Title }).ToList().First().Subject.ToString();
                            var Series = querySeries.Where(s => s.Id == SeriesID).Select(x => new { Series = x.Title }).ToList().First().Series.ToString();

                            message1 = message1 +
                            "<tr>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + (num + 1) + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Subject + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Series + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + classes + "</td>" +
                            "<td style='border:solid 1px #feefef; padding:10px;'>" + Quantity + "</td>" +
                            "</tr>";
                        }

                    }
                }
                else
                {
                    message1 = "<tr>" +
                        "<td style='border:solid 1px #feefef; padding:10px;' colspan='4'>No Subject Selected</td>" +
                        "</tr>";

                }

              


                message = message + message1 + "</td></tr></table></table> ";
           
          
            bookSample.Message = message;

            return message;
        }
     



        [Route("Details")]
        public ActionResult TeacherRegistration() {
           
            SchoolTeacher schoolTeacher = new SchoolTeacher();
            var Classresult = context.USP_EbookClasses(0, 0, 0).Select(x => new { ClassID = x.Id, Class = x.Title }).ToList();
            schoolTeacher.Classes = new SelectList(Classresult, "ClassID", "Class").ToList();
            var result = context.USP_EbookSubject(0).Select(x => new { SubjectID = x.id, Subject = x.Title }).ToList();
            schoolTeacher.Subjects = new SelectList(result, "SubjectID", "Subject").ToList();

            return View(schoolTeacher);
        }
        [HttpPost]
        public ActionResult TeacherRegistration(SchoolTeacher schoolTeacher)
        {
            var Classresult = context.USP_EbookClasses(0, 0, 0).Select(x => new { ClassID = x.Id, Class = x.Title }).ToList();
            schoolTeacher.Classes = new SelectList(Classresult, "ClassID", "Class").ToList();
            var results = context.USP_EbookSubject(0).Select(x => new { SubjectID = x.id, Subject = x.Title }).ToList();
            schoolTeacher.Subjects = new SelectList(results, "SubjectID", "Subject").ToList();
            if (!ModelState.IsValid)
            {
                return View(schoolTeacher);
            }
          
            var user = new ApplicationUser
            {
                UserName = schoolTeacher.ContactNo,
                Email = (schoolTeacher.Email ?? "").ToLower(),
                Address = "",
                FirstName = schoolTeacher.Name,
                Country = "",
                State = "",
                City = "",
                PinCode = "",
                PhoneNumber = schoolTeacher.ContactNo,
                idServer = 0,
                dtmAdd = DateTime.Now,
                ProfileImage = "/UserProfileImage/Noimage.jpg"
            };
            var Password = schoolTeacher.ContactNo;
            var result = UserManager.Create(user, Password);
            ViewBag.Message = "";
            if (result.Succeeded)
            {
                //EbookOrderDetails.CheckAndCreateRoles();
                dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
                var aspNetuser = context.AspNetUsers.FirstOrDefault(x => x.PhoneNumber == schoolTeacher.ContactNo);
                // var aspNetuser = UserManager.FindByEmail(user.UserName);
                if (aspNetuser != null)
                {
                    UserManager.AddToRole(aspNetuser.Id, "Teacher");
                    var TextMessage = "Respected Teacher,%0aGreetings from Prachi Publications, In the wake of present pandemic situation we are giving you free access to our textbooks via e - book application named Readedge.%0aWhich you can access by filling your contact number as user id and password on the given link: Readedge.mielib.com";
                    MessageSent.SendSMS(schoolTeacher.ContactNo, TextMessage);
                    //ReadEdgeLogin readEdgeLogin = new ReadEdgeLogin();
                    //readEdgeLogin.AllowedSystems = 1;
                    //readEdgeLogin.Userid = aspNetuser.Id;
                    //readEdgeLogin.CurrentLogins = 0;
                    //readEdgeLogin.LoginAllowed = true;
                    //context.ReadEdgeLogins.Add(readEdgeLogin);


                    //TeacherDetail teacherDetail = new TeacherDetail();
                    //teacherDetail.ContactNo = schoolTeacher.ContactNo;
                    //teacherDetail.CreatedDate = DateTime.UtcNow.ToLocalTime();
                    //teacherDetail.Email = schoolTeacher.Email;
                    //teacherDetail.Name = schoolTeacher.Name;
                    //teacherDetail.SchoolName = schoolTeacher.SchoolName;

                    //context.TeacherDetails.Add(teacherDetail);
                    //context.SaveChanges();

                    schoolTeacher.TeacherId = aspNetuser.Id;
                    var Dt = GetDataTable(schoolTeacher);
                    schoolTeacher.TeacherRegistration(schoolTeacher, Dt);
                }
                ViewBag.Message = "Data saved Successfully!!";
            }

            return View(schoolTeacher);
        }

        public DataTable GetDataTable(SchoolTeacher schoolTeacher)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add(new DataColumn("Board"));
            dt.Columns.Add(new DataColumn("Teacherid"));
            dt.Columns.Add(new DataColumn("ClassId"));
            dt.Columns.Add(new DataColumn("SubjectId"));
                    foreach (var classID in schoolTeacher.ClassIds)
                    {
                            foreach (var SubjectID in schoolTeacher.SubjectIds)
                            {
                                DataRow dataRow = dt.NewRow();
                                //dataRow["Board"] = AssistanceObj.SubjectDetail[num].Board;
                                dataRow["Teacherid"] = schoolTeacher.TeacherId;
                                dataRow["ClassId"] = classID;
                                dataRow["SubjectId"] = SubjectID;
                                dt.Rows.Add(dataRow);
                            }
                    }     
            return dt;
        }
        [Route("updateTeacherPassword")]
        public void updateTeacherPassword() {

            var aspNetuser = context.AspNetUsers.Where(x => x.PasswordHash == "WzN0aBSZ+YIR93oqt40zMg==").ToList();

            foreach (var user in aspNetuser)
            {
                try
                {
                    user.PasswordHash = Encryption.EncryptCommon(user.UserName);
                    context.AspNetUsers.Add(user);
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();

                    //var TextMessage = "Respected Teacher,%0aGreetings from Prachi Publications, In the wake of present pandemic situation we are giving you free access to solutions of our Future Track 20-20 Sample Paper via an E-book application named Readedge.%0aWhich you can access by filling your already given contact number as User id and Password on the given link: Readedge.mielib.com";
                    //MessageSent.SendSMS(user.UserName, TextMessage);
                }
                catch (Exception ex)
                {

                }
            }
            Response.Write("Teachers password updted successfully....");
        }
        
    }
}