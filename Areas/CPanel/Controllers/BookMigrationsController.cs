using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;


namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BookMigrationsController : Controller
    {
        // GET: CPanel/BookMigrations
        public ActionResult Index(int? page)
        {
            if (System.Web.HttpContext.Current.Application["BookMigration"] == null)
            {
                var results = new List<ReadEdgeBookModel>();
                var readEdgeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["_ConSqlSever"].ToString());
                var query = "select Id,subject_id,series_id,board_Id,class_id,Title,no_of_pages,is_size,isbn,edition,isEncKey from [dbo].[tbl_books] order by subject_id,series_id";
                var cmd = new SqlCommand(query, readEdgeConnection);
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var classIds = Convert.ToString(dr["class_id"]);
                        var subjectId = dr.IsNull("subject_id") ? 0 : Convert.ToInt64(Convert.ToString(dr["subject_id"]));
                        var seriesId = dr.IsNull("series_id") ? 0 : Convert.ToInt64(Convert.ToString(dr["series_id"]));
                        var classList = classIds.Split(',');
                        var boardIds = Convert.ToString(dr["board_Id"]);
                        var boardList = boardIds.Split(',');
                        var classname = string.Empty;
                        foreach (var classId in classList)
                        {
                            if (string.IsNullOrWhiteSpace(classname))
                                classname = GetClass(readEdgeConnection, classId);
                            else
                                classname = classname + "," + GetClass(readEdgeConnection, classId);
                        }
                        var boardname = string.Empty;
                        foreach (var boardId in boardList)
                        {
                            if (string.IsNullOrWhiteSpace(boardname))
                                boardname = GetBoard(readEdgeConnection, boardId);
                            else
                                boardname = boardname + "," + GetBoard(readEdgeConnection, boardId);
                        }

                        var item = new ReadEdgeBookModel
                        {
                            Board = boardname,
                            BookName = Convert.ToString(dr["Title"]),
                            Classes = classname,
                            EncriptionKey = dr.IsNull("isEncKey") ? "" : Convert.ToString(dr["isEncKey"]),
                            Id = Convert.ToInt64(Convert.ToString(dr["Id"])),
                            ISBN = dr.IsNull("isbn") ? "" : Convert.ToString(dr["isbn"]),
                            PageCount = dr.IsNull("no_of_pages") ? "" : Convert.ToString(dr["no_of_pages"]),
                            PageSise = dr.IsNull("is_size") ? "" : Convert.ToString(dr["is_size"]),
                            Series = GetSeries(readEdgeConnection, seriesId),
                            Subject = GetSubject(readEdgeConnection, subjectId),
                            Edition = dr.IsNull("edition") ? "" : Convert.ToString(dr["edition"])
                        };
                        results.Add(item);
                    }
                }
                System.Web.HttpContext.Current.Application.Lock();
                System.Web.HttpContext.Current.Application["BookMigration"] = results;
                System.Web.HttpContext.Current.Application.UnLock();
            }
     
            List<ReadEdgeBookModel> resultset = (List<ReadEdgeBookModel>)System.Web.HttpContext.Current.Application["BookMigration"];
            int pageSize = 25;
            int pageNumber = page.HasValue ? Convert.ToInt32(page):1;
            return View(resultset.OrderBy(x=> x.Series).OrderBy(t => t.Subject).ToPagedList(pageNumber, pageSize));
            //return View(results.OrderBy(t => t.Classes).OrderBy(t => t.Board));
        }
        public ActionResult Details(long Id)
        { 
            Session.Remove("ReadEdgeBookID");
            Session["ReadEdgeBookID"] = Id;
            var results = new ReadEdgeBookModel();
            var readEdgeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["_ConSqlSever"].ToString());
            var query = "select Id,subject_id,series_id,board_Id,class_id,Title,no_of_pages,is_size,isbn,edition,isEncKey from [dbo].[tbl_books] where Id=" + Id;
            var cmd = new SqlCommand(query, readEdgeConnection);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                ViewBag.Board = context.MasterBoards.Select(m => new SelectListItem() { Text = m.Title.ToString(), Value = m.Id.ToString() }).ToList();
                ViewBag.Class = context.MasterClasses.Select(m => new SelectListItem() { Text = m.Title.ToString(), Value = m.Id.ToString() }).ToList();
                ViewBag.Subject = context.MasterSubjects.Select(m => new SelectListItem() { Text = m.Title.ToString(), Value = m.Id.ToString() }).ToList();
                ViewBag.Series = context.MasterSeries.Select(m => new SelectListItem() { Text = m.Title.ToString(), Value = m.Id.ToString() }).ToList();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                var classIds = Convert.ToString(dr["class_id"]);
                var subjectId = dr.IsNull("subject_id") ? 0 : Convert.ToInt64(Convert.ToString(dr["subject_id"]));
                var seriesId = dr.IsNull("series_id") ? 0 : Convert.ToInt64(Convert.ToString(dr["series_id"]));
                var classList = classIds.Split(',');
                var boardIds = Convert.ToString(dr["board_Id"]);
                var boardList = boardIds.Split(',');
                var classname = string.Empty;
                foreach (var classId in classList)
                {
                    if (string.IsNullOrWhiteSpace(classname))
                        classname = GetClass(readEdgeConnection, classId);
                    else
                        classname = classname + "," + GetClass(readEdgeConnection, classId);
                }
                var boardname = string.Empty;
                foreach (var boardId in boardList)
                {
                    if (string.IsNullOrWhiteSpace(boardname))
                        boardname = GetBoard(readEdgeConnection, boardId);
                    else
                        boardname = boardname + "," + GetBoard(readEdgeConnection, boardId);
                }
                results.Board = boardname;
                results.BookName = Convert.ToString(dr["Title"]);
                results.Classes = classname;
                results.EncriptionKey = dr.IsNull("isEncKey") ? "" : Convert.ToString(dr["isEncKey"]);
                results.Id = Convert.ToInt64(Convert.ToString(dr["Id"]));
                results.ISBN = dr.IsNull("isbn") ? "" : Convert.ToString(dr["isbn"]);
                results.PageCount = dr.IsNull("no_of_pages") ? "" : Convert.ToString(dr["no_of_pages"]);
                results.PageSise = dr.IsNull("is_size") ? "" : Convert.ToString(dr["is_size"]);
                results.Series = GetSeries(readEdgeConnection, seriesId);
                results.Subject = GetSubject(readEdgeConnection, subjectId);
                results.Edition = dr.IsNull("edition") ? "" : Convert.ToString(dr["edition"]);
            }
            return View(results);
        }

        string GetSubject(SqlConnection con, long Id)
        {
            var query = "select title from [dbo].[mst_Subject] where Id=" + Id;
            var cmd = new SqlCommand(query, con);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["title"]);
            return "";
        }
        string GetSeries(SqlConnection con, long Id)
        {
            var query = "select title from [dbo].[mst_Series] where Id=" + Id;
            var cmd = new SqlCommand(query, con);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["title"]);

            return "";
        }
        string GetClass(SqlConnection con, string Id)
        {
            var query = "select title from [dbo].[mst_Class] where Id=" + Id;
            var cmd = new SqlCommand(query, con);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["title"]);

            return "";
        }
        string GetBoard(SqlConnection con, string Id)
        {
            var query = "select title from [dbo].[mst_Board] where Id=" + Id;
            var cmd = new SqlCommand(query, con);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["title"]);

            return "";
        }

        public PartialViewResult GetAllBooks(ReadEdgeBookModel obj)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                partialClass objpartialClass = new partialClass();
                objpartialClass.lstReadEdgeBookModel = context.USP_GetBooks_PRACHI(obj.Board, obj.Classes, obj.Subject, obj.Series).Select(x=> new ReadEdgeBookModel {BookID=x.ID,BookName=x.Title}).ToList();

                // List<Student> model = db.Students.ToList();
                return PartialView("_BookMigrate", objpartialClass);
            }
        }
        public JsonResult GetAllSeries(int SubjectID)
        {
            var MasterSeriesRepositories = new PrachiIndia.Sql.CustomRepositories.MasterSeriesRepositories();
            IQueryable<MasterSery> query = MasterSeriesRepositories.GetAll();
            var result = query.Where(s => s.SubjectId == SubjectID).Select(x => new { SeriesID = x.Id, Series = x.Title }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
      

          
        }
        [HttpPost]
        public JsonResult Migrate(partialClass objList)
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("BOOK_ID");
            dt.Columns.Add(dc);
            foreach (var item in objList.lstReadEdgeBookModel)
            {
                if (item.Selected)
                {
                    DataRow dr = dt.NewRow();
                    dr["BOOK_ID"] = Convert.ToString(item.BookID);
                    dt.Rows.Add(dr);
                }
                 
            }
            dynamic showMessageString = string.Empty;
            try
            {
                CatalogRepository objCatalogRepository = new CatalogRepository();
                if (dt.Rows.Count > 0)
                {

                    if (Session["ReadEdgeBookID"] != null)
                    {
                        int result = objCatalogRepository.MigrteBook_Readedge(dt, Convert.ToInt32(Session["ReadEdgeBookID"]));
                        System.Threading.Thread.Sleep(2000);
                        if (result != 0)
                        {
                            showMessageString = 1;
                        }
                        else {
                            showMessageString = 2;
                        }
                      
                    }
                }
                else
                {
                    showMessageString = 3;
                }
            
                return Json(showMessageString);
            }
            catch(Exception ex) {
                showMessageString = 4;
                return Json(showMessageString);
            }
        }
    }

    public class ReadEdgeBookModel
    {
        public long Id { get; set; }
        public string Board { get; set; }
        public string Classes { get; set; }
        public string Subject { get; set; }
        public string Series { get; set; }
        public string BookName { get; set; }
        public string PageCount { get; set; }
        public string PageSise { get; set; }
        public string ISBN { get; set; }
        public string EncriptionKey { get; set; }
        public string Edition { get; set; }
        public long BookID { get; set; }
        public bool Selected { get; set;}

    }

    public class partialClass {
        public int PrachiChapterID {get;set;}
        public int TestHourChapterID {get;set; }
        public List<ReadEdgeBookModel> lstReadEdgeBookModel { get; set; }
    }
}