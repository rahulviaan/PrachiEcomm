using System.Collections.Generic;
using System.Web;
using System.Data;
using PrachiIndia.Web.Areas.Model;
namespace PrachiIndia.Portal.Areas.Readedge_BusLogic
{
    public class blBoard
    {
        public List<MasterBoards> GetAll()
        {
            var lstMasterBoards = new List<MasterBoards>();
            string query = "SELECT  [Id],[title] FROM mst_Board WHERE is_active=1";           
            var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
            DataTable dt = dbutil.getDataTable(query);
            foreach (DataRow r in dt.Rows)
            {
                var obj = new MasterBoards
                {
                    IdServer = Common_Static.ToSafeInt(r["Id"].ToString()),
                    Title = Common_Static.ToSafeString(r["title"].ToString()),
                   
                };
                lstMasterBoards.Add(obj);
            }
            return lstMasterBoards;
        }

    }
}