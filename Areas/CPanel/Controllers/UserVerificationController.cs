using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MailServiece;
using PrachiIndia.Portal.Models;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    public class UserVerificationController : Controller
    {
        // GET: CPanel/UserVerification
        dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
        public ActionResult VerifyDetails(string Id,string RequestID)
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                var UserId = Id;
                Session["UID"] = UserId;
                Session["RequestID"] = RequestID;
                var aspnetUser = context.AspNetUsers.FirstOrDefault(t => t.Id == UserId);
                if (aspnetUser != null)
                {
                    context.USP_VerifyEmail(UserId, RequestID);
                    return View();
                }
                else
                {
                    return View("UnregisteredLink");
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult VerifyMobile(string OTP)
        {
            if (Session["UID"] != null && Session["RequestID"]!=null)
            {
                Assistance obj = new Assistance();
                string status = obj.VerifyOtp(Convert.ToString(Session["UID"]), OTP, Convert.ToString(Session["RequestID"]));
                if (status == "1")
                {
                    Session.Remove("UID");
                    Session.Remove("RequestID");
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            return Json(true);

        }

        [HttpPost]
        public JsonResult ResendOTP()
        {
            if (Session["UID"] != null)
            {
                var RequestID= Convert.ToString(Session["RequestID"]); 
                var userid=Convert.ToString(Session["UID"]);
                Assistance objAssistance = new Assistance();
                 var ebookOrder = context.EbookOrders.FirstOrDefault(t => t.UserID == userid);
                string OTP = MessageSent.GeneratetOTP(5);
                objAssistance.resendtOtp(ebookOrder.UserID, OTP, ebookOrder.SchoolMobile, RequestID);
                MessageSent.SendSMS(ebookOrder.SchoolMobile, "Dear Customer, "+ OTP+" is your Verfication Code for completing your order.Please do not disclose.");
            }
            return Json("1");

        }

    }
}