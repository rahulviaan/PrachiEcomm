using System.Collections.Generic;
using System.Web;
using System.Data;
using PrachiIndia.Web.Areas.Model;
namespace PrachiIndia.Portal.Areas.Readedge_BusLogic
{
    public class blChapters
    {   public List<BookChapters> GetAll(string idBook)
        {
            var chapters = new List<BookChapters>();
             string query = "SELECT  [id],[idBook],[idSubject],[idSeries],[isTitle] ,[isDesc],[dtmCreate],[dtmUpdate],[isStatus]"
                + ",[isFromPage] ,[isToPage] ,[isType] ,[isChapter] FROM [tbl_books_chapters] where idBook in("+ idBook + ")";
          
            var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
            DataTable dt = dbutil.getDataTable(query);           
            foreach (DataRow r in dt.Rows)
            {
                var obj = new BookChapters
                {
                    id = Common_Static.ToSafeInt(r["id"].ToString()),
                    idBook = Common_Static.ToSafeInt(r["idBook"].ToString()),
                    idSubject = Common_Static.ToSafeInt(r["idSubject"].ToString()),
                    idSeries = Common_Static.ToSafeInt(r["idSeries"].ToString()),
                    isTitle = Common_Static.ToSafeString(r["isTitle"]),
                    isDesc = Common_Static.ToSafeString(r["isDesc"]),
                    dtmCreate = Common_Static.ToSafeDate(r["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(r["dtmUpdate"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(r["isStatus"].ToString()),
                    isType = (TextType)Common_Static.ToSafeInt(r["isType"].ToString()),
                    isFromPage = Common_Static.ToSafeInt(r["isFromPage"].ToString()),
                    isToPage = Common_Static.ToSafeInt(r["isToPage"].ToString()),
                    isChapter = Common_Static.ToSafeInt(r["isChapter"].ToString()),                   
                };
                chapters.Add(obj);
            }
            return chapters;
        }
    }
    public class blBookPlus
    {
        public List<BookPlus> GetAll(string idBook)
        {
            var bplist = new List<BookPlus>();
            string query = "SELECT [ID],[idBook],[idChapter],[idType],[isName],[isContentType]"
            + ",[isTitle],[isDesc],[dtmCreate],[dtmUpdate] ,[isStatus] FROM [tbl_books_plus] where idBook in(" + idBook + ")";

            var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
            DataTable dt = dbutil.getDataTable(query);
            foreach (DataRow r in dt.Rows)
            {
                var obj = new BookPlus
                {
                    id = Common_Static.ToSafeInt(r["ID"].ToString()),
                    idBook = Common_Static.ToSafeInt(r["idBook"].ToString()),
                    idChapter = Common_Static.ToSafeInt(r["idChapter"].ToString()),
                    idType = (ChapterType)Common_Static.ToSafeInt(r["idType"].ToString()),
                    isName = Common_Static.ToSafeString(r["isName"]),
                    isContentType = (contentType)Common_Static.ToSafeInt(r["isContentType"].ToString()),
                    isTitle = Common_Static.ToSafeString(r["isTitle"]),
                    isDesc = Common_Static.ToSafeString(r["isDesc"]),
                    dtmCreate = Common_Static.ToSafeDate(r["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(r["dtmUpdate"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(r["isStatus"].ToString()),
                };
                bplist.Add(obj);
            }
            return bplist;
        }
    }
}