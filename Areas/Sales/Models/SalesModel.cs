using PrachiIndia.Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.Sales.Models
{
    public class SalesModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Area { get; set; }
        [Range(1, 50)]
        [Display(Name = "No of Systems")]
        public long DeviceCount { get; set; }
        public bool EmailConfirmed { get; internal set; }
        public bool MobileConfirmed { get; internal set; }
    }

    public class BookSample
    {
        #region School Details
        public string SchoolName { get; set; }
        public string School_Landline { get; set; }
        public string School_Email { get; set; }
        public string SchoolCode { get; set; }
        public int TeacherStrength { get; set; }
        public string Principal_Name { get; set; }
        public string School_Address { get; set; }
        public string Board { get; set; }
        public string CityDesc { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string BookIncharge { get; set; }
        public string School_Mobile { get; set; }
        #endregion

        #region Contact Details
        public string RequestedBy { get; set; }
        public string Designation { get; set; }
        public string HMobileNo { get; set; }
        public string Landline { get; set; }
        public string Email { get; set; }
        public string RequiredFor { get; set; }
        public List<string> Required_Subjects { get; set; }
        public string Description { get; set; }
        public string HArea { get; set; }
        public string HName { get; set; }
        public string HEmail { get; set; }
        public string SName { get; set; }
        public string SArea { get; set; }
        public string SMobileNo { get; set; }
        public string SEmail { get; set; }
        public string TransportType { get; set; }
        public string PickupLocation { get; set; }
        public string DeliveryMode { get; set; }
        public string DepotStation { get; set; }


        public string Message { get; set; }
        public string UserID { get; set; }


        public List<SampleSubjectDetails> SampleSubjectDetails { get; set; }
        #endregion

        public string CreateSampleRequest(BookSample obj, DataTable dt)
        {
            var parameter1 = new SqlParameter("@TYPE_Sample", SqlDbType.Structured);
            parameter1.Value = dt;
            parameter1.TypeName = "dbo.TYPE_Sample";
            
            var parameter14 = new SqlParameter("@HName", SqlDbType.VarChar);
            parameter14.Value = Convert.ToString(obj.HName);

            var parameter15 = new SqlParameter("@HMobile", SqlDbType.VarChar);
            parameter15.Value = Convert.ToString(obj.HMobileNo);

            var parameter16 = new SqlParameter("@HArea", SqlDbType.VarChar);
            parameter16.Value = Convert.ToString(obj.HArea);

            var parameter17 = new SqlParameter("@HEmail", SqlDbType.VarChar);
            parameter17.Value = Convert.ToString(obj.HEmail);

            var parameter18 = new SqlParameter("@SName", SqlDbType.VarChar);
            parameter18.Value = Convert.ToString(obj.SName);

            var parameter19 = new SqlParameter("@SMobile", SqlDbType.VarChar);
            parameter19.Value = Convert.ToString(obj.SMobileNo);

            var parameter20 = new SqlParameter("@SArea", SqlDbType.VarChar);
            parameter20.Value = Convert.ToString(obj.SArea);

            var parameter21 = new SqlParameter("@SEmail", SqlDbType.VarChar);
            parameter21.Value = Convert.ToString(obj.SEmail);

            var parameter22 = new SqlParameter("@Message", SqlDbType.VarChar);
            parameter22.Value = Convert.ToString(obj.Message);

            var parameter23 = new SqlParameter("@UserID", SqlDbType.VarChar);
            parameter23.Value = Convert.ToString(obj.UserID ?? "");


            var parameter25 = new SqlParameter("@TransportType", SqlDbType.VarChar);
            parameter25.Value = Convert.ToString(obj.TransportType ?? "");

            var parameter26 = new SqlParameter("@PickupLocation", SqlDbType.VarChar);
            parameter26.Value = Convert.ToString(obj.PickupLocation ?? "");

            var parameter27 = new SqlParameter("@DeliveryMode", SqlDbType.VarChar);
            parameter27.Value = Convert.ToString(obj.DeliveryMode ?? "");

            var parameter28 = new SqlParameter("@DepotStation", SqlDbType.VarChar);
            parameter28.Value = Convert.ToString(obj.DepotStation ?? "");

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_SampleRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(parameter1);
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
    
            cmd.Parameters.Add(parameter25);
            cmd.Parameters.Add(parameter26);
            cmd.Parameters.Add(parameter27);
            cmd.Parameters.Add(parameter28);
            

            con.Open();
            string RequestID = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return RequestID;
        }


        public string CreateSampleDistribution(BookSample obj, DataTable dt)
        {
            string landline = obj.School_Landline ?? "";
            //using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            //{
            var parameter1 = new SqlParameter("@TYPE_Sample", SqlDbType.Structured);
            parameter1.Value = dt;
            parameter1.TypeName = "dbo.TYPE_Sample";

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

            var parameter7 = new SqlParameter("@TeacherStrength", SqlDbType.Int);
            parameter7.Value = Convert.ToString(obj.TeacherStrength);

            var parameter8 = new SqlParameter("@Principal", SqlDbType.VarChar);
            parameter8.Value = Convert.ToString(obj.Principal_Name);

            var parameter9 = new SqlParameter("@City", SqlDbType.VarChar);
            parameter9.Value = Convert.ToString(obj.City);

            var parameter10 = new SqlParameter("@State", SqlDbType.VarChar);
            parameter10.Value = Convert.ToString(obj.State);

            //var parameter14 = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            //parameter14.Value = Convert.ToString(obj.SchoolName);

            var parameter11 = new SqlParameter("@Country", SqlDbType.VarChar);
            parameter11.Value = Convert.ToString(obj.Country);

            var parameter12 = new SqlParameter("@BookIncharge", SqlDbType.VarChar);
            parameter12.Value = Convert.ToString(obj.BookIncharge);

            var parameter13 = new SqlParameter("@SchoolCode", SqlDbType.VarChar);
            parameter13.Value = Convert.ToString(obj.SchoolCode);

            var parameter14 = new SqlParameter("@HName", SqlDbType.VarChar);
            parameter14.Value = Convert.ToString(obj.HName);

            var parameter15 = new SqlParameter("@HMobile", SqlDbType.VarChar);
            parameter15.Value = Convert.ToString(obj.HMobileNo);

            var parameter16 = new SqlParameter("@HArea", SqlDbType.VarChar);
            parameter16.Value = Convert.ToString(obj.HArea);

            var parameter17 = new SqlParameter("@HEmail", SqlDbType.VarChar);
            parameter17.Value = Convert.ToString(obj.HEmail);

            var parameter18 = new SqlParameter("@SName", SqlDbType.VarChar);
            parameter18.Value = Convert.ToString(obj.SName);

            var parameter19 = new SqlParameter("@SMobile", SqlDbType.VarChar);
            parameter19.Value = Convert.ToString(obj.SMobileNo);

            var parameter20 = new SqlParameter("@SArea", SqlDbType.VarChar);
            parameter20.Value = Convert.ToString(obj.SArea);

            var parameter21 = new SqlParameter("@SEmail", SqlDbType.VarChar);
            parameter21.Value = Convert.ToString(obj.SEmail);

            var parameter23 = new SqlParameter("@UserID", SqlDbType.VarChar);
            parameter23.Value = Convert.ToString(obj.UserID ?? "");

            var parameter24 = new SqlParameter("@Board", SqlDbType.VarChar);
            parameter24.Value = Convert.ToString(obj.Board ?? "");

           
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_SampleDistribution", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(parameter1);
            cmd.Parameters.Add(parameter2);
            cmd.Parameters.Add(parameter3);
            cmd.Parameters.Add(parameter4);
            cmd.Parameters.Add(parameter5);
            cmd.Parameters.Add(parameter6);
            cmd.Parameters.Add(parameter7);
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
            cmd.Parameters.Add(parameter23);
            cmd.Parameters.Add(parameter24);
            con.Open();
            string RequestID = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return RequestID;
        }
    }
}


















