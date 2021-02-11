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
    public class QuestionMigrationController : Controller
    {
        // GET: CPanel/QuestionMigration
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
            int pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
            return View(resultset.OrderBy(x => x.Series).OrderBy(t => t.Subject).ToPagedList(pageNumber, pageSize));
            //return View(results.OrderBy(t => t.Classes).OrderBy(t => t.Board));
        }
        public ActionResult Details(long Id)
        {
            Session.Remove("ReadEdgeBookID");
            Session["ReadEdgeBookID"] = Id;
            var results = new ReadEdgeBookModel();
            var readEdgeConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            var query = "select Id,subject_id,series_id,board_Id,class_id,Title,no_of_pages,is_size,isbn,edition,isEncKey from [dbPortal1].[dbo].[tbl_books] where Id=" + Id;
            var cmd = new SqlCommand(query, readEdgeConnection);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            // var queryChapter = "Select '0' as Id ,'---Select---' as isTitle Union select id,isTitle from [dbPortal1].[dbo].[tbl_books_chapters]where idBook=" + Id+ " and id not in(select distinct idChapter from tbl_Questions)";
            var queryChapter = "Select '0' as Id ,'---Select---' as isTitle Union select id,isTitle from [dbPortal1].[dbo].[tbl_books_chapters]where idBook=" + Id;
            var cmdChapter = new SqlCommand(queryChapter, readEdgeConnection);
            var daChapter = new SqlDataAdapter(cmdChapter);
            var dtChapter = new DataTable();
            daChapter.Fill(dtChapter);


            // ViewBag.TestHourChapter = dtChapter.AsEnumerable().Select(x=> new Chapter});

            // List<SelectListItem> list = new List<SelectListItem>();
            ViewBag.TestHourChapter = (from DataRow row in dtChapter.Rows
                                           // let x= new dbPrachiIndia_PortalEntities().tbl_Questions.Where(x=>x.idChapter)
                                       select new SelectListItem()
                                       {
                                           Text = row["id"].ToString(),
                                           Value = row["isTitle"].ToString()
                                       }).ToList();
            ViewBag.TestHourChapterCount = dtChapter.Rows.Count;

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
            var query = "select title from [dbPortal1].[dbo].[mst_Subject] where Id=" + Id;
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
            var query = "select title from [dbPortal1].[dbo].[mst_Series] where Id=" + Id;
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
            var query = "select title from [dbPortal1].[dbo].[mst_Class] where Id=" + Id;
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
            var query = "select title from [dbPortal1].[dbo].[mst_Board] where Id=" + Id;
            var cmd = new SqlCommand(query, con);
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["title"]);

            return "";
        }

        public PartialViewResult GetAllChapters(ReadEdgeBookModel obj)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                partialClass objpartialClass = new partialClass();
                var subject = Convert.ToInt64(obj.Subject);
                var Series = Convert.ToInt64(obj.Series);
                var MigratedChapter = context.tbl_Questions.Select(x => x.idChapter).ToList().Distinct();
                Session.Remove("PrachiBookID");
                var bid = context.tblCataLogs.Where(x => x.BoardId == obj.Board && x.ClassId == obj.Classes && x.SubjectId == subject && x.SeriesId == Series).Select(x => new { Bookid = x.Id, }).ToList();
                Session["PrachiBookID"] = bid[0].Bookid;
                if (bid.Count > 0)
                {
                    var bookid = Convert.ToInt64(bid[0].Bookid);

                    var results = context.Chapters.Where(x => x.BookId == bookid && x.Status == 1 && !MigratedChapter.Contains(x.Id)).Select(x => new ReadEdgeBookModel { BookID = x.Id, BookName = x.Title, Id = x.BookId ?? 0 }).ToList();
                    objpartialClass.lstReadEdgeBookModel = results.Where(t => t.Id == bookid).ToList();
                }
                else
                {
                    objpartialClass.lstReadEdgeBookModel = new List<ReadEdgeBookModel> { new ReadEdgeBookModel { BookID = 0, BookName = "--Select--" } };
                }

                // List<Student> model = db.Students.ToList();
                return PartialView("_ChapterMigrate", objpartialClass);
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
        public PartialViewResult Migrate(partialClass objList)
        {
            using (dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities())
            {
                var parameter1 = new SqlParameter("@PrachiChapterID", SqlDbType.Int);
                var parameter2 = new SqlParameter("@TestHourChapterID", SqlDbType.Int);
                parameter1.Value = Convert.ToString(objList.PrachiChapterID);
                parameter2.Value = Convert.ToString(objList.TestHourChapterID);
                context.Database.ExecuteSqlCommand("EXEC USP_MIGRATE_FROM_TESTHOUR @PrachiChapterID,@TestHourChapterID", parameter1, parameter2);

                var MigratedChapter = context.tbl_Questions.Select(x => x.idChapter).ToList().Distinct();
                partialClass objpartialClass = new partialClass();
                if (Session["PrachiBookID"] != null)
                {
                    var bookid = Convert.ToInt64(Session["PrachiBookID"]);
                    objpartialClass.lstReadEdgeBookModel = context.Chapters.Where(x => x.BookId == bookid && !MigratedChapter.Contains(x.Id)).Select(x => new ReadEdgeBookModel { BookID = x.Id, BookName = x.Title }).ToList();
                }
                else
                {
                    objpartialClass.lstReadEdgeBookModel = new List<ReadEdgeBookModel> { new ReadEdgeBookModel { BookID = 0, BookName = "--Select--" } };
                }

                // List<Student> model = db.Students.ToList();
                return PartialView("_ChapterMigrate", objpartialClass);
            }
        }
    }

    public class MigratedChapter
    {
        public long Chapterid { get; set; }
    }
}


