using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Sql;
using System.Threading.Tasks;
using PrachiIndia.Web.Areas.Model;
using PrachiIndia.Sql.CustomRepositories;

namespace PrachiIndia.Portal.Areas.TestHour.Controllers
{
    [Authorize]
    public class TopicMastersController : Controller
    {
        private dbPrachiIndia_PortalEntities db = (dbPrachiIndia_PortalEntities)Factory.FactoryRepository.GetInstance(RepositoryType.dbPrachiIndia_PortalEntities);
        //private dbPrachiIndia_PortalEntities db = new dbPrachiIndia_PortalEntities();
        // GET: TestHour/TopicMasters
        public ActionResult Index(long chapterId=0)
        {
            ViewBag.Id = chapterId;
            List<TopicMaster> lst;
            if (chapterId != 0)
            {
                lst = db.TopicMasters.Where(t => t.CHAPTERID == chapterId && t.STATUS == true).ToList();
            }
            else {
                lst = db.TopicMasters.ToList();
            }

            return View(lst);
        }

        // GET: TestHour/TopicMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicMaster topicMaster = db.TopicMasters.Find(id);
            if (topicMaster == null)
            {
                return HttpNotFound();
            }
            return View(topicMaster);
        }

        // GET: TestHour/TopicMasters/Create
        public ActionResult Create(long ChapterId)
        {
            var chapters = db.Chapters.FirstOrDefault(t => t.Id == ChapterId);
            if (chapters == null)
            {
                ViewBag.ClassID = new SelectList(db.MasterClasses.Where(x => x.Status == 1).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name");
                Task<IQueryable> T2 = Task<IQueryable>.Factory.StartNew(() => db.ChapterContents.Include(t => t.tblCataLog).Where(x => x.tblCataLog.EncriptionKey != null && x.tblCataLog.Ebook == 1).Select(y => new { Id = y.Id, Name = y.Title }));
                ViewBag.BookID = Enumerable.Empty<SelectListItem>();
                ViewBag.CHAPTERID = Enumerable.Empty<SelectListItem>();
                return View("CreateTopic");
            }

            var classname = db.MasterClasses.First(t => t.Id.ToString() == chapters.tblCataLog.ClassId).Title;
            chapters.Description = classname;
           
            ViewBag.Chapter = chapters;

            return View();

            //new SelectList(db.ChapterContents.Include(t => t.tblCataLog).Where(x => x.tblCataLog.EncriptionKey != null && x.tblCataLog.Ebook == 1).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name")
        }

        // POST: TestHour/TopicMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassID,BookID,CHAPTERID,TOPIC")] TopicMaster topicMaster)
        {
            if (ModelState.IsValid)
            {
                topicMaster.STATUS = true;
                db.TopicMasters.Add(topicMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.MasterClasses.Where(x => x.Status == 1).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name");
            ViewBag.CHAPTERID = new SelectList(db.ChapterContents, "Id", "Name", topicMaster.CHAPTERID);
            ViewBag.BookID = new SelectList(db.tblCataLogs, "Id", "Title", topicMaster.BookID);
            return View(topicMaster);
        }

        // GET: TestHour/TopicMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            ChapterRepository chapterRepository = (ChapterRepository)Factory.FactoryRepository.GetInstance(RepositoryType.ChapterRepository);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicMaster topicMaster = db.TopicMasters.Find(id);
            if (topicMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.MasterClasses.Where(x => x.Status == 1).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name", topicMaster.ClassID);
            ViewBag.CHAPTERID = new SelectList(chapterRepository.GetAll().Where(x => x.BookId == topicMaster.BookID).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name", topicMaster.CHAPTERID);
            //ViewBag.CHAPTERID = new SelectList(db.ChapterContents.Where(x=> x.BookId== topicMaster.BookID), "Id", "Name", topicMaster.CHAPTERID);
            ViewBag.BookID = new SelectList(db.tblCataLogs.Where(x => x.Id == topicMaster.BookID), "Id", "Title", topicMaster.BookID);
            return View(topicMaster);
        }

        // POST: TestHour/TopicMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BookID,CHAPTERID,TOPIC,CREATEDDATE,UPDATEDDATE,STATUS")] TopicMaster topicMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topicMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CHAPTERID = new SelectList(db.ChapterContents, "Id", "Name", topicMaster.CHAPTERID);
            ViewBag.BookID = new SelectList(db.tblCataLogs, "Id", "Title", topicMaster.BookID);
            return View(topicMaster);
        }

        // GET: TestHour/TopicMasters/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TopicMaster topicMaster = db.TopicMasters.Find(id);
        //    if (topicMaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(topicMaster);
        //}

        // POST: TestHour/TopicMasters/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            TopicMaster topicMaster = db.TopicMasters.Find(id);
            //db.TopicMasters.Remove(topicMaster);
            topicMaster.STATUS = false;
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult GetBookByClass(string ClassID)
        {
            CatalogRepository bookRepository = (CatalogRepository)Factory.FactoryRepository.GetInstance(RepositoryType.CatalogRepository);
            return Json(new SelectList(bookRepository.GetAll().Where(x => x.ClassId == ClassID && x.Status == 1).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name"), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult GetChapterByBook(Int64 BookId)
        {
            ChapterRepository chapterRepository = (ChapterRepository)Factory.FactoryRepository.GetInstance(RepositoryType.ChapterRepository);
            return Json(new SelectList(chapterRepository.GetAll().Where(x => x.BookId == BookId && x.Status == 1).Select(y => new { Id = y.Id, Name = y.Title }), "Id", "Name"), JsonRequestBehavior.DenyGet);
        }
    }
}
