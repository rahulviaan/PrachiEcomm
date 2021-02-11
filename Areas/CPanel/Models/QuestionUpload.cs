using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Areas.CPanel.Models
{
    public class QuestionUpload
    {
        
        public bool ReturnValue { get; set; }
        public string Message { get; set; }

        public int UploadQuestion(DataTable dt,string Type="1")
        {
            string ProcedureName = "USP_UploadQuestion";
            string TypeName = "dbo.TYPE_UploadeQuestion";
            string Param = "@TYPE_UploadeQuestion";
            //USP_UploadQuestionWithOption
            if (Type != "1" && Type != "2")
            {
                ProcedureName = "USP_UploadQuestionWithOption";
                TypeName = "dbo.TYPE_UploadeQuestionWithOption";
                Param = "@TYPE_UploadeQuestionWithOption";
            }
                var parameter1 = new SqlParameter(Param, SqlDbType.Structured);
            parameter1.Value = dt;
            parameter1.TypeName = TypeName;
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var cmd = new SqlCommand(ProcedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(parameter1);
            con.Open();
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return result;

        }
    }
}