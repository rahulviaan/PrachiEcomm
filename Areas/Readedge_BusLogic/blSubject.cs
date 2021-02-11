using System.Collections.Generic;
using System.Web;
using System.Data;
using PrachiIndia.Web.Areas.Model;
namespace PrachiIndia.Portal.Areas.Readedge_BusLogic
{
    public class blSubject 
    {
        public List<MasterSubject> GetAll()
        {
            var lstMasterSubject = new List<MasterSubject>();
            string query = "SELECT  [Id],[title] ,[is_active]  FROM mst_Subject WHERE is_active=1";           
            var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
            DataTable dt = dbutil.getDataTable(query);
            HttpContext ct = HttpContext.Current;
            foreach (DataRow r in dt.Rows)
            {
                MasterSubject obj = new MasterSubject
                {
                    IdServer = Common_Static.ToSafeInt(r["Id"].ToString()),
                    Title = Common_Static.ToSafeString(r["title"].ToString()),
                   
                };
                lstMasterSubject.Add(obj);
            }
            return lstMasterSubject;
        }

    }
}