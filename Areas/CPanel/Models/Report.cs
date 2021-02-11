using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PrachiIndia.Sql;
using System.Data.Common;
using PrachiIndia.Portal.Areas.Report.Models;
namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class Report
    {
        //public static List<USP_GET_RETAIL_ORDERS_Result> GetRetailOrder(string Type, string FromDate, string ToDate, string OrderNo,string UserID)
        //{

        //    var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        //    var cmd = new SqlCommand("USP_GET_RETAIL_ORDERS", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Type", Type);
        //    cmd.Parameters.AddWithValue("@FromDate", FromDate);
        //    cmd.Parameters.AddWithValue("@ToDate", ToDate);
        //    cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
        //    cmd.Parameters.AddWithValue("@UserID", UserID);
        //    con.Open();
        //    SqlDataReader sdr = cmd.ExecuteReader();
        //    List<USP_GET_RETAIL_ORDERS_Result> orderlist=new List<USP_GET_RETAIL_ORDERS_Result>();
        //    if (sdr.HasRows)
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Load(sdr);

        //        orderlist = (from DataRow row in dt.Rows

        //                     select new USP_GET_RETAIL_ORDERS_Result
        //                     {
        //                         Title = row["Title"].ToString(),
        //                         Class = row["Class"].ToString(),
        //                         Price = Convert.ToDecimal(row["Price"]),
        //                         CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
        //                         Quantity = Convert.ToInt32(row["Quantity"]),
        //                         TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
        //                     }).ToList();

        //    }
        //    else {
        //        orderlist.Add(new USP_GET_RETAIL_ORDERS_Result
        //        {
        //            Title = "",
        //            Class = "",
        //            Price = 0,
        //            CreatedDate = new DateTime(),
        //            Quantity = 0,
        //            TotalAmount = 0,
        //        });
        //    }
        //    con.Close();

        //    return orderlist;
        //}

        public static List<RetailOrderVM> GetRetailOrder(string Type, string FromDate, string ToDate, string OrderNo, string UserID)
        {

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_GET_RETAIL_ORDERS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);
            cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            List<RetailOrderVM> orderlist = new List<RetailOrderVM>();
            if (sdr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(sdr);

                orderlist = (from DataRow row in dt.Rows

                             select new RetailOrderVM
                             {
                                 Title = row["Title"].ToString(),
                                 classes = row["Class"].ToString(),
                                 Price = Convert.ToDecimal(row["Price"]),
                                 OrderDate = Convert.ToDateTime(row["CreatedDate"]),
                                 Quantity = Convert.ToInt32(row["Quantity"]),
                                 Email = Convert.ToString(row["Email"]),
                                 Name = Convert.ToString(row["Name"]),
                                 PhoneNo = Convert.ToString(row["Phone"]),
                                 TransactionID = Convert.ToString(row["TransactionId"]),
                             }).ToList();

            }
            else
            {
                orderlist.Add(new RetailOrderVM
                {
                    Title = "",
                    classes = "",
                    Price = 0,
                    OrderDate = new DateTime(),
                    Quantity = 0,
                    Email = "",
                    Name = "",
                    PhoneNo = "",
                    TransactionID = "",
                });
            }
            con.Close();

            return orderlist;
        }
    }
}