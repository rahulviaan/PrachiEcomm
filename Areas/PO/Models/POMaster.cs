using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Sql;
using System.Web.Mvc;
using System.Configuration;
using System.Globalization;
using MailServiece;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Areas.PO.Models
{
    public class POMaster
    {
        dbPrachiIndia_PortalEntities context;
        public string RequestID { get; set; }
        public int BoardID { get; set; }
        public int SubjectID { get; set; }
        public int SeriesID { get; set; }
        public int ClassID { get; set; }
        public List<int> ClassIDs { get; set; }
        public List<POMaster> POMasterDetails { get; set; }

        public void GeneratePO(POMaster objPoMaster)
        {
            string message = string.Empty;
            message = "<table style='width:800px; margin:0 auto; border: solid 1px #eaeaea;' cellspacing='0' cellpadding='0'><tr><td colspan='5' style='border: solid 1px #feefef; padding:10px;background-color: #f3f9eb;'><b>Subject Details</b></td></tr><tr><th style='border: solid 1px #feefef; padding:10px;'>S.No.</th><th style='border:solid 1px #feefef; padding:10px;'>Board</th><th style='border:solid 1px #feefef; padding:10px;'>Subject</th><th style='border:solid 1px #feefef; padding:10px;'>Series</th><th style='border:solid 1px #feefef; padding:10px;'>Class</th></tr>";
            context = new dbPrachiIndia_PortalEntities();
            for (int num = 0; num < objPoMaster.POMasterDetails.Count; num++)
            {
                //if (objPoMaster.POMasterDetails[num].ClassIDs != null && objPoMaster.POMasterDetails[num].SubjectID != null && AssistanceObj.SubjectDetail[num].Series != null)
                //{
                    string classes = string.Empty;
                    foreach (int classID in objPoMaster.POMasterDetails[num].ClassIDs)
                    {
                        classes = classes + ", " + Utility.classesByClassID(Convert.ToString(classID));

                    }
                    classes = classes.Remove(classes.IndexOf(","), 1);
                    long? BoardID = Convert.ToInt32(objPoMaster.POMasterDetails[num].BoardID);
                    long? subjectID = Convert.ToInt32(objPoMaster.POMasterDetails[num].SubjectID);
                    long? SeriesID = Convert.ToInt32(objPoMaster.POMasterDetails[num].SeriesID);

                    var Board = context.MasterBoards.Where(s => s.Id == BoardID).Select(x => new { Subject = x.Title }).ToList().First().Subject.ToString();
                    var Subject = context.MasterSubjects.Where(s => s.Id == subjectID).Select(x => new { Subject = x.Title }).ToList().First().Subject.ToString();
                    var Series = context.MasterSeries.Where(s => s.Id == SeriesID).Select(x => new { Series = x.Title }).ToList().First().Series.ToString();

                    message = message + "<tr>" +
                    "<td style='border:solid 1px #feefef; padding:10px;'>" + (num + 1) + "</td>" +
                    "<td style='border:solid 1px #feefef; padding:10px;'>" + Board + "</td>" +
                    "<td style='border:solid 1px #feefef; padding:10px;'>" + Subject + "</td>" +
                    "<td style='border:solid 1px #feefef; padding:10px;'>" + Series + "</td>" +
                    "<td style='border:solid 1px #feefef; padding:10px;'>" + classes + "</td>" +
                    "</tr>";
                //}

            }
            message = message + "</td></tr></table></table> ";

            DataTable dt = GetDataTable(objPoMaster);
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_GeneratePO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RequestID", objPoMaster.RequestID);
            cmd.Parameters.AddWithValue("@Message", message);
            cmd.Parameters.AddWithValue("@TYPE_eBookPOOrder", dt);
            con.Open();
            string NewRequestID=  Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            if (!string.IsNullOrEmpty(NewRequestID))
            {
                var MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString(CultureInfo.InvariantCulture);
                var Ebook= context.EbookOrders.FirstOrDefault(x => x.RequestID == objPoMaster.RequestID);
                var User= context.AspNetUsers.FirstOrDefault(x => x.Email == Ebook.SchoolEmail);
                Assistance AssistanceObj = new Assistance();
                string OTP = PrachiIndia.Portal.Framework.MessageSent.GeneratetOTP(5);
                AssistanceObj.insertOtp(User.Id, OTP, Ebook.SchoolMobile, NewRequestID);
                PrachiIndia.Portal.Framework.MessageSent.SendSMS(Ebook.SchoolMobile, "Thank you for placing your order, your PO NO : " + NewRequestID + ", your verification code is :" + OTP);
                var encriptedUserId = User.Id;
                var requestContext = HttpContext.Current.Request.RequestContext;
                var callbackUrl = new UrlHelper(requestContext).Action("VerifyDetails", "UserVerification", new { Id = encriptedUserId, RequestID = NewRequestID, Area = "CPanel" }, protocol: HttpContext.Current.Request.Url.Scheme);
                var emailVerifiedMessage = "Dear " + Ebook.SchoolName + ", <br/> <p>Thank you for placing you order, <br/><br/>PO NO# :" + RequestID + ".</p> <br/> Please click <a href=\"" + callbackUrl + "\">here</a> to verify your order. <br/><br/>Your Verification Code is : <b>" + OTP + "</b><br /><br /><p> Below are the details of your order<p> <br />";
                string message2 = emailVerifiedMessage + message;
                string message3 = "Dear, <br/>Order has been generated for " + Ebook.SchoolName + "<br/><br/> PO NO# :" + RequestID + "<br/><br/> Below are the details of the order" + message;
                //Portal.Framework.Utility.SendMail("E-Book Order", message2, Ebook.SchoolEmail, MailFrom, null);
                //Portal.Framework.Utility.SendMail("E-Book Order", message3, Ebook.SEmail, MailFrom, Ebook.MEmail);

                Mail.SendMail(AssistanceObj.School_Email, "E-Book Order", message2);
                Mail.SendMail(AssistanceObj.SEmail, AssistanceObj.MEmail, "E-Book Order", message3);
            }
        }

        public DataTable GetDataTable(POMaster POMasterObject)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Board");
            dt.Columns.Add("Class");
            dt.Columns.Add("Subject");
            dt.Columns.Add("Series");

            foreach (var data in POMasterObject.POMasterDetails)
            {
                foreach (var classid in data.ClassIDs)
                {
                    DataRow dr = dt.NewRow();
                    dr["Board"] = data.BoardID;
                    dr["Class"] = classid;
                    dr["Subject"] = data.SubjectID;
                    dr["Series"] = data.SeriesID;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

    }


}