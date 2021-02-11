using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using PrachiIndia.Portal.Factory;
using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Web.Areas.Model;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Portal.Areas.CPanel.Models;
using PrachiIndia.Portal.Areas.Report.Models;

namespace PrachiIndia.Portal.Areas.Report.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report/Report
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult ViewReport()
        {
            ViewBag.Message = "Report";
            return View();
        }

        public ActionResult ViewMarketReport()
        {
            ViewBag.Message = "Report";
            return View();
        }

        [HttpPost]
        public ActionResult searchby(string txtsearch, DateTime? fromdate, DateTime? todate)
        {
            //LocalReport localReport = new LocalReport();
            //localReport.ReportPath = Server.MapPath("Rpt/ViewReport.rdlc");


            //var context = new dbPrachiIndia_PortalEntities();
            //var Result= context.usp_getSalesReport(txtsearch, fromdate, todate).ToList();

            //ReportDataSource reportDataSource = new ReportDataSource();
            //reportDataSource.Name = "DS_Sales";
            //reportDataSource.Value = Result;
            //localReport.DataSources.Add(reportDataSource);




            return View();
        }

        public ActionResult ViewBookOrderReport()
        {
            return View();
        }

        public ActionResult PO_Report(string PONUMBER)
        {
            ViewBag.PoNumber = PONUMBER;
            return View();
        }

        public ActionResult PO_Reportbygrp(string PONUMBER)
        {
            ViewBag.Po = PONUMBER;
            return View();
        }

        public ActionResult RetailBookOrder()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult UserEbookOrder()
        {
            return PartialView("_RetailEbookOrder");
        }
        [ChildActionOnly]
        public ActionResult UserEbookOrder_User()
        {
            return PartialView("_RetailEbookOrderUser");
        }
        [ChildActionOnly]
        public ActionResult UserPrintBookOrder()
        {
            return PartialView("_RetailPrintBookOrder");
        }
        [ChildActionOnly]
        public ActionResult UserPrintBookOrder_User()
        {
            return PartialView("_RetailPrintBookOrderUser");
        }
        [HttpPost]
        public ActionResult GetBookOrderReport(string Type, string FromDate, string ToDate, string OrderNo)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                var userID = User.Identity.GetUserId();
                //var result = context.USP_GET_RETAIL_ORDERS(Type, FromDate, ToDate, OrderNo, userID);
                var result = CPanel.Models.Report.GetRetailOrder(Type, FromDate, ToDate, OrderNo, userID);
                var results = from order in result
                              group order by order.TransactionID into g
                              select new RetailOrderVM
                              {
                                  TransactionID = g.Key,
                                  Email = g.FirstOrDefault().Email,
                                  Name = g.FirstOrDefault().Name,
                                  PhoneNo = g.FirstOrDefault().PhoneNo,
                                  OrderDate = g.FirstOrDefault().OrderDate,
                                  RetailOrderVMsList = g.ToList()
                              };
                ViewBag.Type = "Detailed_" + Type;
                return PartialView("_OrderReport", results.ToList());
            }

        }
        [HttpPost]
        public ActionResult GetBookOrderReport_User(string Type, string FromDate, string ToDate, string OrderNo)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                var userID = User.Identity.GetUserId();

                var result = CPanel.Models.Report.GetRetailOrder(Type, FromDate, ToDate, OrderNo, userID);
                var results = from order in result
                              group order by order.TransactionID into g
                              select new RetailOrderVM
                              {
                                  TransactionID = g.Key,
                                  Email = g.FirstOrDefault().Email,
                                  Name = g.FirstOrDefault().Name,
                                  PhoneNo = g.FirstOrDefault().PhoneNo,
                                  OrderDate = g.FirstOrDefault().OrderDate,
                              };
                ViewBag.Type = "User_" + Type;
                return PartialView("_OrderReportUser", results.ToList());
            }

        }

    }
}