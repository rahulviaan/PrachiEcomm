using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Portal.Areas.CPanel.Models;
using PrachiIndia.Sql;

namespace PrachiIndia.Portal.Areas.TestHour.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        // GET: TestHour/Qeustion
        dbPrachiIndia_PortalEntities context;
        public ActionResult Index(long bookId, long chapterId = 0)
        {
               context = new dbPrachiIndia_PortalEntities();
      
                var chapterRepository = new ChapterRepository();
                var chapter = chapterRepository.GetByIdAsync(chapterId);
                var result = new ChapterModel();
                var bookItem = new CatalogRepository().FindByIdAsync((int)bookId);
                var classes = Helpers.Utility.classesByClassID(bookItem.ClassId);
                ViewBag.Topic=new SelectList(context.TopicMasters.Where(x => x.STATUS == true).ToList(), "ID","TOPIC");
                ViewBag.QuesionType=new SelectList(context.QuestionTypes.Where(x => x.STATUS == true).ToList(), "ID","TYPE");
                ViewBag.QuesionCatagory=new SelectList(context.mst_Question_Category.Where(x => x.Status == 1).ToList(), "ID","Title");
                ViewBag.BookName = string.Format("{0} - {1}", bookItem.Title, classes);

                result.BookId = bookId;
                if (chapter != null)
                {
                    result.Id = chapter.Id;
                    result.OrderNo = chapter.ChapterIndex ?? 0;
                    result.Title = chapter.Title;
                    result.ToPage = chapter.ToPage ?? 0;
                    result.FromPage = chapter.FromPage ?? 0;
                    result.BookId = chapter.BookId ?? 0;
                    result.Descreption = chapter.Description;
                }
                return View(result);

               
        }
    }
}