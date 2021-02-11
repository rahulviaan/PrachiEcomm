using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrachiIndia.Portal.Areas.PO.Models
{

    public class SchoolTeacher
    {
        [Required]
        public string Name { get; set; }
        public string TeacherId { get; set; }
        public string SchoolName { get; set; }
        public string Email { get; set; }
        [Required]
        public string ContactNo { get; set; }

        public List<SelectListItem> Classes { get; set; }
        [Required]
        public int[] ClassIds { get; set; }


        public List<SelectListItem> Subjects { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public int[] SubjectIds { get; set; }


        public void TeacherRegistration(SchoolTeacher obj, DataTable dt)
        {
            // string landline = obj.School_Landline ?? "";
            //using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            //{
            var parameter1 = new SqlParameter("@TYPE_eBookOrderTeacher", SqlDbType.Structured);
            parameter1.Value = dt;
            parameter1.TypeName = "dbo.TYPE_eBookOrderTeacher";

            var parameter2 = new SqlParameter("@Name", SqlDbType.VarChar);
            parameter2.Value = Convert.ToString(obj.Name);

            var parameter3 = new SqlParameter("@SchoolName", SqlDbType.VarChar);
            parameter3.Value = Convert.ToString(obj.SchoolName);

            var parameter4 = new SqlParameter("@Email", SqlDbType.VarChar);
            parameter4.Value = Convert.ToString(obj.Email);

            var parameter5 = new SqlParameter("@ContactNo", SqlDbType.VarChar);
            parameter5.Value = Convert.ToString(obj.ContactNo);

            var parameter6 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
            parameter6.Value = Convert.ToString(obj.CreatedDate);

            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand("USP_eBookOrderTeacher", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(parameter1);
            cmd.Parameters.Add(parameter2);
            cmd.Parameters.Add(parameter3);
            cmd.Parameters.Add(parameter4);
            cmd.Parameters.Add(parameter5);
            cmd.Parameters.Add(parameter6);
            con.Open();
            cmd.ExecuteScalar();
            con.Close();

        }
    }
}