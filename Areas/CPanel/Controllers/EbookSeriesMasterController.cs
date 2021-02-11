using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Portal.Areas.CPanel.Models;
using PrachiIndia.Sql;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    public class EbookSeriesMasterController : Controller
    {
        //EbookSeries
        // GET: CPanel/EbookSeriesMaster
        dbPrachiIndia_PortalEntities context;
        public ActionResult Index()
        {
            context = new dbPrachiIndia_PortalEntities();
            var ebookseries = context.Ebookseries.Where(y=> y.Status==true)
                                                    .GroupBy(x => new { x.Subject, x.OrderID, x.Series, x.Board, x.Audio, x.LessonPlan, x.WorkSheet, x.WorkSheetSolution, x.BookSolution, x.TestHour, x.TestHourSolution, x.SingleStar, x.DoubleStar, x.Id })
                                                    .OrderBy(g => g.Key.OrderID)
                                                    .Select(g => new EbookSeries
                                                    {
                                                        Id = g.Key.Id,
                                                        Series = g.Key.Series,
                                                        Board = g.Key.Board,
                                                        Subject = g.Key.Subject,
                                                        Audio = g.Key.Audio,
                                                        LessonPlan = g.Key.LessonPlan,
                                                        WorkSheet = g.Key.WorkSheet,
                                                        WorkSheetSolution = g.Key.WorkSheetSolution,
                                                        BookSolution = g.Key.BookSolution,
                                                        TestHour = g.Key.TestHour,
                                                        TestHourSolution = g.Key.TestHourSolution,
                                                        SingleStar = g.Key.SingleStar,
                                                        DoubleStar = g.Key.DoubleStar
                                                    }).ToList();


            ViewBag.Message = "Your contact page.";
            //var Subjects = context.MasterSubjects.Where(x => x.Status == 1).ToList();
            //ViewBag.Subjects = new SelectList(Subjects, "Id", "Title");
            return View(ebookseries);
        }

        [HttpPost]
        public JsonResult Index(EbookSeries model)
        {
            try
            {
                using (context = new dbPrachiIndia_PortalEntities())
                {
                    model.Board=string.Join("/", model.Boards);
                    if (model.Id > 0)
                    {

                        mst_Question_Category questionCatagory = context.mst_Question_Category.SingleOrDefault(x => x.ID == model.Id && x.Status == 1);
                        //questionCatagory.Title = model.Title;
                        //questionCatagory.Remarks = model.Remarks;
                        Ebooksery ebookseries = context.Ebookseries.SingleOrDefault(x=> x.Id==model.Id && x.Status==true);
                        ebookseries.Status = true;
                        ebookseries.LessonPlan = model.LessonPlan;
                        ebookseries.WorkSheet = model.WorkSheet;
                        ebookseries.BookSolution = model.BookSolution;
                        ebookseries.WorkSheetSolution = model.WorkSheetSolution;
                        ebookseries.Audio = model.Audio;
                        ebookseries.TestHour = model.TestHour;
                        ebookseries.TestHourSolution = model.TestHourSolution;
                        ebookseries.SingleStar = model.SingleStar;
                        ebookseries.DoubleStar = model.DoubleStar;
                        ebookseries.Board = model.Board;
                        context.Entry(ebookseries).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                    else
                    {
                        //Insert
                        mst_Question_Category questionCatagory = new mst_Question_Category();
                        //questionCatagory.Title = model.Title;
                        //questionCatagory.Remarks = model.Remarks;
                        //questionCatagory.Status = model.Status;
                        context.mst_Question_Category.Add(questionCatagory);
                        context.SaveChanges();

                    }
                    //System.Threading.Thread.Sleep(2000);
                    return Json(true, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult AddEditebookSeries(int Id)
        {
            using (context = new dbPrachiIndia_PortalEntities())
            {
                context = new dbPrachiIndia_PortalEntities();
                EbookSeries ebookseries = new EbookSeries();
                var Boards = context.MasterBoards.Where(x => x.Status == 1).Select(y=> new {Title=y.Title.Trim()}).ToList();
                ViewBag.Boards = new SelectList(Boards, "Title", "Title");
                if (Id > 0)
                {
                    ebookseries = context.Ebookseries.Where(x => x.Id == Id)
                                                        .Select(y => new EbookSeries
                                                        {
                                                            Id = y.Id,
                                                            Series = y.Series,
                                                            Board = y.Board,
                                                            Subject = y.Subject,
                                                            Audio = y.Audio,
                                                            LessonPlan = y.LessonPlan,
                                                            WorkSheet = y.WorkSheet,
                                                            WorkSheetSolution = y.WorkSheetSolution,
                                                            BookSolution = y.BookSolution,
                                                            TestHour = y.TestHour,
                                                            TestHourSolution = y.TestHourSolution,
                                                            SingleStar = y.SingleStar,
                                                            DoubleStar = y.DoubleStar,
                                                        }).FirstOrDefault();
                    ebookseries.Boards= ebookseries.Board.Split('/').ToList();
                }
                var Subjects = context.MasterSubjects.Where(x => x.Status == 1).ToList();
                ViewBag.Subjects = new SelectList(Subjects, "Title", "Title");
                return PartialView("~/Areas/CPanel/Views/Shared/ebookSeriesAddEdit.cshtml", ebookseries);
            }
        }

        public ActionResult GetSearchRecord(string SearchText)
        {

            using (context = new dbPrachiIndia_PortalEntities())
            {
                List<EbookSeries> list;
                if (!String.IsNullOrWhiteSpace(SearchText))
                {


                    list = context.Ebookseries
                                             .Where(x => x.Series.Contains(SearchText))
                                             .GroupBy(x => new { x.Subject, x.OrderID, x.Series, x.Board, x.Audio, x.LessonPlan, x.WorkSheet, x.WorkSheetSolution, x.BookSolution, x.TestHour, x.TestHourSolution, x.SingleStar, x.DoubleStar })
                                             .OrderBy(g => g.Key.OrderID)
                                             .Select(g => new EbookSeries
                                             {
                                                 Series = g.Key.Series,
                                                 Board = g.Key.Board,
                                                 Subject = g.Key.Subject,
                                                 Audio = g.Key.Audio,
                                                 LessonPlan = g.Key.LessonPlan,
                                                 WorkSheet = g.Key.WorkSheet,
                                                 WorkSheetSolution = g.Key.WorkSheetSolution,
                                                 BookSolution = g.Key.BookSolution,
                                                 TestHour = g.Key.TestHour,
                                                 TestHourSolution = g.Key.TestHourSolution,
                                                 SingleStar = g.Key.SingleStar,
                                                 DoubleStar = g.Key.DoubleStar
                                             }).ToList();

                }
                else
                {
                    list = context.Ebookseries
                                    .Where(x => x.Series.Contains(SearchText))
                                    .GroupBy(x => new { x.Subject, x.OrderID, x.Series, x.Board, x.Audio, x.LessonPlan, x.WorkSheet, x.WorkSheetSolution, x.BookSolution, x.TestHour, x.TestHourSolution, x.SingleStar, x.DoubleStar })
                                    .OrderBy(g => g.Key.OrderID)
                                    .Select(g => new EbookSeries
                                    {
                                        Series = g.Key.Series,
                                        Board = g.Key.Board,
                                        Subject = g.Key.Subject,
                                        Audio = g.Key.Audio,
                                        LessonPlan = g.Key.LessonPlan,
                                        WorkSheet = g.Key.WorkSheet,
                                        WorkSheetSolution = g.Key.WorkSheetSolution,
                                        BookSolution = g.Key.BookSolution,
                                        TestHour = g.Key.TestHour,
                                        TestHourSolution = g.Key.TestHourSolution,
                                        SingleStar = g.Key.SingleStar,
                                        DoubleStar = g.Key.DoubleStar
                                    }).ToList();
                }
                return PartialView("~/Areas/CPanel/Views/Shared/_ebookSeries.cshtml", list);

            }
        }

        [HttpPost]
        public JsonResult Delete_eBookSeries(int ID)
        {
            using (context = new dbPrachiIndia_PortalEntities())
            {
                // var result = context.mst_Question_Category.FirstOrDefault(x => x.ID.Equals(ID));            
                var ebookseries = context.Ebookseries.FirstOrDefault(x => x.Id == ID);
                ebookseries.Status = false;
                context.Entry(ebookseries).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult PartialResult()
        {
            context = new dbPrachiIndia_PortalEntities();
            var ebookseries = context.Ebookseries.Where(y => y.Status == true)
                                                    .GroupBy(x => new { x.Subject, x.OrderID, x.Series, x.Board, x.Audio, x.LessonPlan, x.WorkSheet, x.WorkSheetSolution, x.BookSolution, x.TestHour, x.TestHourSolution, x.SingleStar, x.DoubleStar, x.Id })
                                                    .OrderBy(g => g.Key.OrderID)
                                                    .Select(g => new EbookSeries
                                                    {
                                                        Id = g.Key.Id,
                                                        Series = g.Key.Series,
                                                        Board = g.Key.Board,
                                                        Subject = g.Key.Subject,
                                                        Audio = g.Key.Audio,
                                                        LessonPlan = g.Key.LessonPlan,
                                                        WorkSheet = g.Key.WorkSheet,
                                                        WorkSheetSolution = g.Key.WorkSheetSolution,
                                                        BookSolution = g.Key.BookSolution,
                                                        TestHour = g.Key.TestHour,
                                                        TestHourSolution = g.Key.TestHourSolution,
                                                        SingleStar = g.Key.SingleStar,
                                                        DoubleStar = g.Key.DoubleStar
                                                    }).ToList();

           return  PartialView("~/Areas/CPanel/Views/Shared/_ebookSeries.cshtml", ebookseries);
        }
    }
}