using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PrachiIndia.Sql;

namespace PrachiIndia.Portal.Models
{
    public class Assistance
    {
        #region School Details
        public string SchoolName { get; set; }
        public string School_Landline { get; set; }
        public string School_Email { get; set; }
        public string Principal_Name { get; set; }
        public string School_Address { get; set; }
        public string Board { get; set; }
        public string CityDesc { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Strength { get; set; }
        public string ITIncharge { get; set; }
        public int NoOfSystem { get; set; }
        public string School_Mobile { get; set; }
        public string Type { get; set; }
        #endregion

        #region Contact Details
        public string RequestedBy { get; set; }
        public string Designation { get; set; }
        public string MMobileNo { get; set; }
        public string Landline { get; set; }
        public string Email { get; set; }
        public string RequiredFor { get; set; }
        public List<string> Required_Subjects { get; set; }
        public string Description { get; set; }
        public string MArea { get; set; }
        public string MName { get; set; }
        public string SName { get; set; }
        public string SArea { get; set; }
        public string SMobileNo { get; set; }
        public string SEmail { get; set; }
        public string MEmail { get; set; }

        public string Message { get; set; }
        public string UserID { get; set; }
        public string DefaultPassword { get; set; }
        public List<SubjectDetails> SubjectDetail { get; set; }
        #endregion


        public string CreateOrder(Assistance obj, DataTable dt)
        {
            string landline = obj.School_Landline ?? "";
            //using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            //{
            var parameter1 = new SqlParameter("@TYPE_eBookOrder", SqlDbType.Structured);
            parameter1.Value = dt;
            parameter1.TypeName = "dbo.TYPE_eBookOrder";

            var parameter2 = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            parameter2.Value = Convert.ToString(obj.SchoolName);

            var parameter3 = new SqlParameter("@SchoolAddress", SqlDbType.VarChar);
            parameter3.Value = Convert.ToString(obj.School_Address);

            var parameter4 = new SqlParameter("@SchoolPhone", SqlDbType.VarChar);
            parameter4.Value = Convert.ToString(landline);

            var parameter5 = new SqlParameter("@SchoolMobile", SqlDbType.VarChar);
            parameter5.Value = Convert.ToString(obj.School_Mobile);

            var parameter6 = new SqlParameter("@SchoolEmail", SqlDbType.VarChar);
            parameter6.Value = Convert.ToString(obj.School_Email);

            var parameter8 = new SqlParameter("@NoOfSystem", SqlDbType.Int);
            parameter8.Value = Convert.ToString(obj.NoOfSystem);

            var parameter9 = new SqlParameter("@Principal", SqlDbType.VarChar);
            parameter9.Value = Convert.ToString(obj.Principal_Name);

            var parameter10 = new SqlParameter("@City", SqlDbType.VarChar);
            parameter10.Value = Convert.ToString(obj.City);

            var parameter11 = new SqlParameter("@State", SqlDbType.VarChar);
            parameter11.Value = Convert.ToString(obj.State);

            //var parameter14 = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            //parameter14.Value = Convert.ToString(obj.SchoolName);

            var parameter12 = new SqlParameter("@Country", SqlDbType.VarChar);
            parameter12.Value = Convert.ToString(obj.Country);

            var parameter13 = new SqlParameter("@ITIncharge", SqlDbType.VarChar);
            parameter13.Value = Convert.ToString(obj.ITIncharge);

            //var parameter17 = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            //parameter17.Value = Convert.ToString(obj.SchoolName);

            var parameter14 = new SqlParameter("@RType", SqlDbType.VarChar);
            parameter14.Value = Convert.ToString(obj.Type);

            var parameter15 = new SqlParameter("@MName", SqlDbType.VarChar);
            parameter15.Value = Convert.ToString(obj.MName);

            var parameter16 = new SqlParameter("@MMobile", SqlDbType.VarChar);
            parameter16.Value = Convert.ToString(obj.MMobileNo);

            var parameter17 = new SqlParameter("@MArea", SqlDbType.VarChar);
            parameter17.Value = Convert.ToString(obj.MArea);

            var parameter18 = new SqlParameter("@MEmail", SqlDbType.VarChar);
            parameter18.Value = Convert.ToString(obj.MEmail);

            var parameter19 = new SqlParameter("@SName", SqlDbType.VarChar);
            parameter19.Value = Convert.ToString(obj.SName);

            var parameter20 = new SqlParameter("@SMobile", SqlDbType.VarChar);
            parameter20.Value = Convert.ToString(obj.SMobileNo);

            var parameter21 = new SqlParameter("@SArea", SqlDbType.VarChar);
            parameter21.Value = Convert.ToString(obj.SArea);

            var parameter22 = new SqlParameter("@SEmail", SqlDbType.VarChar);
            parameter22.Value = Convert.ToString(obj.SEmail);

            var parameter23 = new SqlParameter("@Message", SqlDbType.VarChar);
            parameter23.Value = Convert.ToString(obj.Message);

            var parameter24 = new SqlParameter("@UserID", SqlDbType.VarChar);
            parameter24.Value = Convert.ToString(obj.UserID ?? "");

            var parameter25 = new SqlParameter("@Strength", SqlDbType.Int);
            parameter25.Value = Convert.ToString(obj.Strength);

            var parameter26 = new SqlParameter("@DefaultPassword", SqlDbType.NVarChar);
            parameter26.Value = Convert.ToString(obj.DefaultPassword);


            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_eBookOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(parameter1);
            cmd.Parameters.Add(parameter2);
            cmd.Parameters.Add(parameter3);
            cmd.Parameters.Add(parameter4);
            cmd.Parameters.Add(parameter5);
            cmd.Parameters.Add(parameter6);
            cmd.Parameters.Add(parameter8);
            cmd.Parameters.Add(parameter9);
            cmd.Parameters.Add(parameter10);
            cmd.Parameters.Add(parameter11);
            cmd.Parameters.Add(parameter12);
            cmd.Parameters.Add(parameter13);
            cmd.Parameters.Add(parameter14);
            cmd.Parameters.Add(parameter15);
            cmd.Parameters.Add(parameter16);
            cmd.Parameters.Add(parameter17);
            cmd.Parameters.Add(parameter18);
            cmd.Parameters.Add(parameter19);
            cmd.Parameters.Add(parameter20);
            cmd.Parameters.Add(parameter21);
            cmd.Parameters.Add(parameter22);
            cmd.Parameters.Add(parameter23);
            cmd.Parameters.Add(parameter24);
            cmd.Parameters.Add(parameter25);
            cmd.Parameters.Add(parameter26);
            
            con.Open();
            string RequestID= Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return RequestID;
        }

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

        public void insertOtp(string UserID,string OTP,string MobileNo,string RequestID)
        {

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_InsertOTP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@OTP", OTP);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("@RequestID", RequestID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void resendtOtp(string UserID, string OTP, string MobileNo,string RequestID)
        {

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_InsertOTP_resend", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@OTP", OTP);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("@RequestID", RequestID);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public string VerifyOtp(string UserID, string OTP,string RequestID)
        {

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_VerifyOTP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID); 
            cmd.Parameters.AddWithValue("@OTP", OTP);
            cmd.Parameters.AddWithValue("@RequestID", RequestID);
            cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30);
            cmd.Parameters["@Status"].Direction = ParameterDirection.Output;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            string Status = cmd.Parameters["@Status"].Value.ToString();
            return Status;
        }

    }
}