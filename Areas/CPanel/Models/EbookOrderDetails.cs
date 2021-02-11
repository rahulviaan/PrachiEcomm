using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Sql;
using PrachiIndia.Portal.Framework;
using System.Web.Mvc;
using System.Configuration;
using MailServiece;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class EbookOrder
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string SchoolName { get; set; }
        public string EmailAddres { get; set; }
        public string Mobile { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool MobileConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public bool Status { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string SearchParameter { get; set; }
    }

    public class EbookOrderDetails
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public void VerifyOrder(string RequestID, string ApproverID)
        {

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_VerifyOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RequestID", RequestID);
            cmd.Parameters.AddWithValue("@ApproverID", ApproverID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static void CheckAndCreateRoles()
        {
            var roleManager = new IdentityRoleManager();
            if (!roleManager.RoleExists(Roles.User))
            {
                roleManager.CreateRole(Roles.User);
            }
            if (!roleManager.RoleExists(Roles.Teacher))
            {
                roleManager.CreateRole(Roles.Teacher);
            }
            if (!roleManager.RoleExists(Roles.School))
            {
                roleManager.CreateRole(Roles.School);
            }
            if (!roleManager.RoleExists(Roles.Admin))
            {
                roleManager.CreateRole(Roles.Admin);
            }
            if (!roleManager.RoleExists(Roles.SuperAdmin))
            {
                roleManager.CreateRole(Roles.SuperAdmin);
            }
            if (!roleManager.RoleExists(Roles.Sales))
            {
                roleManager.CreateRole(Roles.Sales);
            }
            if (!roleManager.RoleExists(Roles.Marketing))
            {
                roleManager.CreateRole(Roles.Marketing);
            }
        }

        public void ResendConfirmationMail(string RequestID)
        {
            var MailFrom = Convert.ToString(ConfigurationManager.AppSettings["MailFrom"]);
            Assistance AssistanceObj = new Assistance();
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            var ebook = context.EbookOrders.FirstOrDefault(y => y.RequestID == RequestID);
            string OTP = MessageSent.GeneratetOTP(5);
            AssistanceObj.insertOtp(ebook.UserID, OTP, ebook.SchoolMobile, RequestID);
            MessageSent.SendSMS(ebook.SchoolMobile, "Thank you for placing your order, your PO NO : " + RequestID + ", your verification code is " + OTP);
            var encriptedUserId = ebook.UserID;
            var requestContext = HttpContext.Current.Request.RequestContext;
            var callbackUrl = new UrlHelper(requestContext).Action("VerifyDetails", "UserVerification", new { Id = encriptedUserId, RequestID = RequestID, Area = "CPanel" }, protocol: HttpContext.Current.Request.Url.Scheme);
            var emailVerifiedMessage = "Dear " + ebook.SchoolName + ", <br/> &nbsp;&nbsp;&nbsp;<p>Thank you for placing you order.</p> <br/> Please click <a href=\"" + callbackUrl + "\">here</a> to verify your order. <br/><br/>Your Verification Code is :  <b>" + OTP + "</b><br /><br /><br />";

            //var x = Portal.Framework.Utility.SendMail("e book Verification", emailVerifiedMessage, ebook.SchoolEmail,MailFrom,null);
             Mail.SendMail(ebook.SchoolEmail, "e book Verification", emailVerifiedMessage);
            
        }

    }
}