using System.Collections.Generic;
using System.Data;
using PrachiIndia.Web.Areas.Model;
using System;

namespace PrachiIndia.Portal.Areas.Readedge_BusLogic
{
    public class blBook
    {
        public List<Books> GetBook(int subject_id, int series_id = 0, string Title = "")
        {
            var lstBooks = new List<Books>();
            string query = "select [id],[title] ,isEncKey from tbl_books ";
            query += " WHERE subject_id =" + subject_id + " AND series_id =" + series_id + " ";
            if (!string.IsNullOrWhiteSpace(Title) && Title != "Other" && Title != "-- Select --")
            {
                query += " AND title ='" + Title + "'  order by title asc";
            }
            var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
            DataTable dt = dbutil.getDataTable(query);
            foreach (DataRow r in dt.Rows)
            {
                Books obj = new Books
                {
                    idServer = Common_Static.ToSafeInt(r["Id"].ToString()),
                    Title = Common_Static.ToSafeString(r["title"].ToString()),
                    EncriptionKey = Common_Static.ToSafeString(r["isEncKey"])
                };
                lstBooks.Add(obj);
            }
            return lstBooks;
        }
        public List<Books> GetAll(string value = "")
        {
            var lstBooks = new List<Books>();
            string query = "select id,is_size ,isEncKey from tbl_books  WHERE  is_size != 'N/A' AND  is_size != '' ";
            // string query = "select id, is_size, isEncKey from tbl_books WHERE ID IN ("+ value + ")";
            var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
            DataTable dt = dbutil.getDataTable(query);
            foreach (DataRow r in dt.Rows)
            {
                Books obj = new Books
                {
                    Id = Common_Static.ToSafeInt(r["Id"].ToString()),
                    is_size = Common_Static.ToSafeString(r["is_size"].ToString()),
                    EncriptionKey = Common_Static.ToSafeString(r["isEncKey"])
                };
                lstBooks.Add(obj);
            }
            return lstBooks;
        }
        public Books Get(int Id)
        {
            var book = new Books();
            try
            {

                string query = "SELECT  [id],[subject_id],[series_id],[board_id],[class_id],[title],[price],[description],[image_path]"
                + " ,[img_extension],[no_of_pages],[is_size],[author],[isbn],[edition],[shortcut],[ebook],[suppleymentary],[_print],[audio] "
                + " ,[multimedia],[created_date],[modified_date],[is_active],[isEncKey],[isVersion] FROM [tbl_books] Where Id=" + Id + " ";
                var dbutil = new DAL.DBUtility(Common_Static.ReadedgeConnectionString);
                DataTable dt = dbutil.getDataTable(query);
                foreach (DataRow r in dt.Rows)
                {
                    book = new Books
                    {
                        Id = Common_Static.ToSafeInt(r["Id"].ToString()),
                        SubjectId = Common_Static.ToSafeInt(r["subject_id"].ToString()),
                        SeriesId = Common_Static.ToSafeInt(r["series_id"].ToString()),
                        Board = Common_Static.ToSafeString(r["board_id"].ToString()).Trim() == "" ? "0" : Common_Static.ToSafeString(r["board_id"].ToString()),
                        Class = Common_Static.ToSafeString(r["class_id"].ToString()).Trim() == "" ? "0" : Common_Static.ToSafeString(r["class_id"].ToString()),
                        Title = Common_Static.ToSafeString(r["title"].ToString()),
                        Price = Common_Static.ToSafeDecimal(r["price"].ToString()),
                        Description = Common_Static.ToSafeString(r["description"].ToString()),
                        Image = Common_Static.ToSafeString(r["image_path"].ToString()),
                        Author = Common_Static.ToSafeString(r["author"].ToString()),
                        ISBN = Common_Static.ToSafeString(r["isbn"].ToString()),
                        Edition = Common_Static.ToSafeString(r["edition"].ToString()),
                        ShortCut = Common_Static.ToSafeString(r["shortcut"].ToString()),
                        EncriptionKey = Common_Static.ToSafeString(r["isEncKey"]),

                    };
                }
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                book = null;
            } 
            return book;
        }
    }
}