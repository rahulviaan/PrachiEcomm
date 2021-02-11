using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Sql;
using PrachiIndia.Portal.Areas.TestHour.Models;
namespace PrachiIndia.Portal.Areas.TestHour.Controllers
{
    [Authorize]
    public class QuestionCatagoryController : Controller
    {
        dbPrachiIndia_PortalEntities context;
        // GET: TestHour/Question
        public ActionResult Index()
        {
            using (context = new dbPrachiIndia_PortalEntities())
            {
                ViewBag.QuestioCatagory = context.mst_Question_Category.Where(x => x.Status == 1).ToList();
                return View("~/Areas/TestHour/Views/QuestionCatagory/index.cshtml");
            }

        }

        [HttpPost]
        public ActionResult Index(QuestionVM model)
        {
            try
            {
                using (context = new dbPrachiIndia_PortalEntities())
                {
                    if (model.Id > 0)
                    {

                        mst_Question_Category questionCatagory = context.mst_Question_Category.SingleOrDefault(x => x.ID == model.Id && x.Status == 1);
                        questionCatagory.Title = model.Title;
                        questionCatagory.Remarks = model.Remarks;
                        questionCatagory.Status = 1;
                        context.Entry(questionCatagory).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                    }
                    else
                    {
                        //Insert
                        mst_Question_Category questionCatagory = new mst_Question_Category();
                        questionCatagory.Title = model.Title;
                        questionCatagory.Remarks = model.Remarks;
                        questionCatagory.Status = model.Status;
                        context.mst_Question_Category.Add(questionCatagory);
                        context.SaveChanges();

                    }
                    //System.Threading.Thread.Sleep(2000);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult AddEditQuestionCatagory(int Id)
        {
            using (context = new dbPrachiIndia_PortalEntities())
            {
                QuestionVM qCatagory = new QuestionVM();
                if (Id > 0)
                {
                    mst_Question_Category questionCatagory = context.mst_Question_Category.SingleOrDefault(x => x.ID == Id && x.Status == 1);
                    qCatagory.Title = questionCatagory.Title;
                    qCatagory.Remarks = questionCatagory.Remarks;
                    qCatagory.Status = questionCatagory.Status;
                }
                return PartialView("~/Areas/TestHour/Views/Shared/_QuestionCatagoryAddEdit.cshtml", qCatagory);
            }
        }

        public ActionResult GetSearchRecord(string SearchText)
        {

            using (context = new dbPrachiIndia_PortalEntities())
            {
                List<QuestionVM> list;
                if (!String.IsNullOrWhiteSpace(SearchText))
                {
                    list = context.mst_Question_Category.Where(x => x.Title.Contains(SearchText) || x.Remarks.Contains(SearchText)).Select(x => new QuestionVM { Title = x.Title, Remarks = x.Remarks }).ToList();
                }
                else
                {
                    list = context.mst_Question_Category.Select(x => new QuestionVM { Title = x.Title, Remarks = x.Remarks, Status = x.Status, Id = x.ID }).ToList();
                }
                return PartialView("~/Areas/TestHour/Views/Shared/_SearchPartial.cshtml", list);

            }
        }

        [HttpPost]
        public JsonResult DeleteQuestionCatagory(int ID)
        {

            using (context = new dbPrachiIndia_PortalEntities())
            {
                var result = context.mst_Question_Category.FirstOrDefault(x => x.ID.Equals(ID));
                result.Status = 0;
                context.Entry(result).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}