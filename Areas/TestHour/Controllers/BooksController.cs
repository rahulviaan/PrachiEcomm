using Microsoft.Ajax.Utilities;
using PrachiIndia.Portal.BAL;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PrachiIndia.Portal.Areas.TestHour.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        // GET: TestHour/Books
        public async Task<ActionResult> Index()
        {
            Books books = (Books)Factory.Factory.GetInstance(RepositoryType.Books);
            MasterSubjectRepository masterSubjectRepository = (MasterSubjectRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSubjectrepository);
            MasterSeriesRepositories masterSeriesRepository = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            MasterBoardRepository masterBoardRepository = (MasterBoardRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterBoardRepository);
            MasterClassRepository masterClassReposiroty = (MasterClassRepository)Factory.FactoryRepository.GetInstance(RepositoryType.MasterClassRepository);
            ViewBag.Boards = new SelectList(await Task.Run(() => (masterBoardRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
            ViewBag.Class = new SelectList(await Task.Run(() => (masterClassReposiroty.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
            ViewBag.Subjects = new SelectList(await Task.Run(() => (masterSubjectRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");
            ViewBag.Series = new SelectList(await Task.Run(() => (masterSeriesRepository.GetAll().Where(x => x.Status == 1).Select(x => new { Id = x.Id, Name = x.Title })).ToList()), "Id", "Name");

            Session.Remove("BookImage");
            await Task.WhenAll();
            return View(books);
        }
        [HttpPost]
        public PartialViewResult GetBooks(int idSubject = 0, int idSeries = 0, string idBoard = "", string idClass = "", string Title = "")
        {
            CatalogRepository catalogRepository = (CatalogRepository)Factory.FactoryRepository.GetInstance(RepositoryType.CatalogRepository);
            IQueryable<tblCataLog> Query = catalogRepository.GetAll().Where(x => x.Ebook == 1 && x.EncriptionKey != null);

            if (idSubject != 0 && idSeries != 0 && idBoard != "" && idClass != "" && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard && t.ClassId == idClass && t.Title == Title);
            }
            else if (idSubject != 0 && idSeries != 0 && idBoard != "" && idClass != "")
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard && t.ClassId == idClass);
            }
            else if (idSubject != 0 && idSeries != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard);
            }
            else if (idSubject != 0 && idSeries != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries && t.BoardId == idBoard);
            }
            else if (idClass != "" && idSubject != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSubject.Id == idSubject && t.BoardId == idBoard);
            }
            else if (idClass != "" && idSeries != 0 && idBoard != "")
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSery.Id == idSeries && t.BoardId == idBoard);
            }
            else if (idSubject != 0 && idSeries != 0)
            {
                Query = Query.Where(t => t.MasterSubject.Id == idSubject && t.MasterSery.Id == idSeries);
            }

            else if (idBoard != "" && idClass != "")
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.ClassId == idClass);
            }

            else if (idBoard != "" && idSubject != 0)
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.MasterSubject.Id == idSubject);
            }
            else if (idBoard != "" && idSeries != 0)
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.MasterSery.Id == idSeries);
            }
            else if (idBoard != "" && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.BoardId == idBoard && t.Title == Title);
            }
            else if (idClass != "" && idSubject != 0)
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSery.Id == idSubject);
            }
            else if (idClass != "" && idSeries != 0)
            {
                Query = Query.Where(t => t.ClassId == idClass && t.MasterSery.Id == idSeries);
            }
            else if (idClass != "" && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.ClassId == idClass && t.Title == Title);
            }
            else if (idSubject != 0 && idSeries != 0)
            {
                Query = Query.Where(t => t.MasterSery.Id == idSubject && t.MasterSery.Id == idSeries);
            }
            else if (idSubject != 0 && !string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.MasterSery.Id == idSubject && t.Title == Title);
            }
            else if (!string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.Title == Title);
            }
            else if (!string.IsNullOrWhiteSpace(Title))
            {
                Query = Query.Where(t => t.Title == Title);
            }
            else if (idClass != "")
            {
                Query = Query.Where(t => t.ClassId == idClass);
            }
            else if (idBoard != "")
            {
                Query = Query.Where(t => t.BoardId == idClass);
            }
            else if (idSeries != 0)
            {
                Query = Query.Where(t => t.MasterSery.Id == idSeries);
            }
            else if (idSubject != 0)
            {
                Query = Query.Where(t => t.MasterSery.Id == idSubject);
            }
            var lstBooks = Query.ToList();
            if (lstBooks != null)
            {
                try
                {
                    var result = from c in lstBooks
                                 orderby c.Title descending
                                 select new tblCataLog
                                 {
                                     Id = c.Id,
                                     Title = c.Title,
                                     Author = c.Author,
                                     Image = c.Image,
                                     ISBN = c.ISBN,
                                     CreateDate = c.dtmAdd,
                                     Edition = c.Edition,
                                     Solutions = c.Solutions,
                                     Multimedia = c.MultiMedia,
                                     Worksheet = c.Worksheet,
                                     Ebook = c.Ebook,
                                     LessonPlan = c.LessonPlan,
                                     Class = Utility.classesByClassID(c.ClassId),
                                     Board = Utility.BoardsByClassID(c.BoardId),
                                     Status = c.Status,
                                     Series = Utility.SeriesByID(c.SeriesId),
                                     SeriesId = c.SeriesId
                                 };
                    return PartialView("_Book", result);
                }
                catch (Exception ex)
                {

                }
            }
            return null;
        }

        public ContentResult GetSeries(int idSubject)
        {
            List<SeriesModel> lst = new List<SeriesModel>();
            MasterSeriesRepositories MasterSeriesRepositories = (MasterSeriesRepositories)Factory.FactoryRepository.GetInstance(RepositoryType.MasterSeriesRepositories);
            var lstBooks = MasterSeriesRepositories.GetAll().Where(t => t.SubjectId == idSubject && t.Status == 1);
            if (lstBooks != null)
            {
                try
                {
                    var result = from c in lstBooks
                                 orderby c.Title ascending
                                 select new
                                 {
                                     Id = c.Id,
                                     Title = c.Title,
                                 };
                    var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                    return new ContentResult()
                    {
                        Content = serializer.Serialize(result),
                        ContentType = "application/json",
                    };
                }
                catch (Exception ex)
                {

                }
            }
            return null;
        }

        #region ManageBook
        public ActionResult ManageBooks()
        {
            //SeriesBal bl = new SeriesBal();
            //IEnumerable<SelectListItem> series = (from m in bl.GetAllSeriesSubject(IsValid.Active) select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.title, Value = m.Id.ToString() });

            return View();
        }

        #endregion
        [HttpPost]
        public JsonResult getSeries(string term)
        {
            term = "1";
            SeriesBal bl = new SeriesBal();
            IEnumerable<SelectListItem> series = (from m in bl.GetAllSeriesSubject(IsValid.Active) select m).AsEnumerable().Where(x => x.title.Contains(term)).Select(m => new SelectListItem() { Text = m.title, Value = m.Id.ToString() });
            SelectList lst = new SelectList(series, "Value", "Text");
            return Json(lst);
        }
        public JsonResult getBooksJson(string series_id, string idBooks)
        {
            BookBal bls = new BookBal();
            IEnumerable<objBook> lsto;
            if (idBooks != "" && idBooks != null)
            {
                lsto = bls.GetAllBySeries(series_id, IsValid.All, idBooks);
            }
            else
                lsto = bls.GetAllBySeries(series_id, IsValid.All);

            var result = from c in lsto
                         orderby c.title ascending
                         select new { c.Id, c.title, c.audio, c.image_path, c.img_extension, c.is_active, c.isbn, c.sizeondisk };
            return Json(result);
        }
        public ActionResult ManageChapters(int id)
        {
            BookBal bl = new BookBal();
            objBook objBook = (objBook)bl.Get(id);
            objBookChapter objBookChapter = new objBookChapter();
            objBookChapter.book = objBook;
            objBookChapter.idSeries = objBook.series_id;
            objBookChapter.idSubject = objBook.subject_id;
            objBookChapter.idBook = objBook.Id;
            return View(objBookChapter);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageChapters(objBookChapter oc)
        {
            objBookChapter ocn = new objBookChapter();
            ocn = oc;
            BookChapterBAL blc = new BookChapterBAL();
            int i = blc.Add(oc);

            BookBal bl = new BookBal();
            objBook objBook = (objBook)bl.Get(oc.idBook);
            objBookChapter objBookChapter = new objBookChapter();
            objBookChapter.book = objBook;
            objBookChapter.idSeries = objBook.series_id;
            objBookChapter.idSubject = objBook.subject_id;
            objBookChapter.idBook = objBook.Id;
            return View(objBookChapter);
        }

        public ActionResult SelectPartialView(string id, string edid)
        {

            if (Request.IsAjaxRequest())
            {

                if (id == "_viewSubject")
                {
                    SubjectBal bl = new SubjectBal();
                    objSubject data = new objSubject();
                    data = (objSubject)bl.Get(Convert.ToInt32(edid));
                    return View("" + id + "", data);
                }
                else if (id == "_editSubject")
                {
                    SubjectBal bl = new SubjectBal();
                    objSubject data = new objSubject();
                    data = (objSubject)bl.Get(Convert.ToInt32(edid));
                    return View("_Subject", data);

                }
                else if (id == "_viewSeries")
                {
                    SeriesBal bl = new SeriesBal();
                    objSeries data = new objSeries();
                    data = (objSeries)bl.Get(Convert.ToInt32(edid));
                    return View("" + id + "", data);
                }
                else if (id == "_editSeries")
                {
                    SeriesBal bl = new SeriesBal();
                    objSeries data = new objSeries();
                    data = (objSeries)bl.Get(Convert.ToInt32(edid));
                    return View("_Series", data);

                }
                else if (id == "_viewBooks")
                {
                    BookBal bl = new BookBal();
                    objBook data = new objBook();
                    data = (objBook)bl.Get(Convert.ToInt32(edid));
                    return View("" + id + "", data);
                }
                else if (id == "_viewBooksQuestion")
                {
                    BookBal bl = new BookBal();
                    objBook data = new objBook();
                    data = (objBook)bl.Get(Convert.ToInt32(edid));
                    return View("" + id + "", data);
                }
                else if (id == "_editBooks")
                {
                    BookBal bl = new BookBal();
                    objBook data = new objBook();
                    data = (objBook)bl.Get(Convert.ToInt32(edid));
                    return View("_Books", data);

                }
                //else if (id == "_viewReader")
                //{
                //    BAL.Bus_Logic.ReaderBAL bl = new BAL.Bus_Logic.ReaderBAL();
                //    BAL.Models.objReader data = new BAL.Models.objReader();
                //    data = (BAL.Models.objReader)bl.Get(Convert.ToInt32(edid));
                //    return View("_Reader", data);

                //}
                //else if (id == "_UpdateReader")
                //{
                //    BAL.Bus_Logic.ReaderBAL bl = new BAL.Bus_Logic.ReaderBAL();
                //    BAL.Models.objReader data = new BAL.Models.objReader();
                //    data = (BAL.Models.objReader)bl.Get(Convert.ToInt32(edid));
                //    return View("_AddReader", data);

                //}

                //else if (id == "_GenBookCoupon")
                //{
                //    BAL.Bus_Logic.Account Account = new BAL.Bus_Logic.Account();
                //    BAL.Models.RegisterModel data = new BAL.Models.RegisterModel();
                //    data = (BAL.Models.RegisterModel)Account.Get(Convert.ToInt32(edid));
                //    return View("_GenBookCoupon", data);

                //}
                //else if (id == "_POCoupon")
                //{
                //    BAL.Bus_Logic.CouponBal blc = new BAL.Bus_Logic.CouponBal();

                //    IEnumerable<BAL.Models.objCoupon> data = blc.GetAll(BAL.Common_Static.ToSafeInt(edid));
                //    return View("_POCoupon", data);

                //}
                //else if (id == "_PO")
                //{
                //    BAL.Bus_Logic.POBAL bl = new BAL.Bus_Logic.POBAL();
                //    BAL.Models.objPO data = bl.Get(BAL.Common_Static.ToSafeInt(edid));
                //    return View("_PO", data);

                //}
                //else if (id == "_Userbookassign")
                //{
                //    BAL.Bus_Logic.BookBal bal = new BAL.Bus_Logic.BookBal();

                //    return View("_Userbookassign", bal.GetAll(BAL.Models.IsValid.Active));

                //}

                //else if (id == "_editCRM")
                //{
                //    BAL.Bus_Logic.DeliveryBal bl = new BAL.Bus_Logic.DeliveryBal();
                //    BAL.Models.objDelivery data = new BAL.Models.objDelivery();
                //    data = (BAL.Models.objDelivery)bl.Get(Convert.ToInt32(edid));
                //    return View("_CRM", data);

                //}
                //else if (id == "_editASTRO")
                //{
                //    BAL.Bus_Logic.balAstrlogy bl = new BAL.Bus_Logic.balAstrlogy();
                //    BAL.Models.tbl_As123 data = new BAL.Models.tbl_As123();
                //    data = (BAL.Models.tbl_As123)bl.Get(edid);
                //    return View("_ASTRO", data);

                //}
                else if (id == "_AddLP")
                {
                    BookChapterLPBAL bl = new BookChapterLPBAL();
                    objBookChapterLP data = new objBookChapterLP();
                    data = bl.Get(Convert.ToInt32(edid));
                    return View("_AddLP", data);
                }
                //else if (id == "_viewBP")
                //{
                //    BAL.Bus_Logic.BookPlusBAL bl = new BAL.Bus_Logic.BookPlusBAL();
                //    BAL.Models.objBookPlus data = new BAL.Models.objBookPlus();
                //    data = bl.Get(Convert.ToInt32(edid));
                //    return View("_viewBP", data);
                //}
                //else if (id == "_viewQuestion")
                //{
                //    BAL.Bus_Logic.QuestionsBAL bl = new BAL.Bus_Logic.QuestionsBAL();
                //    BAL.Models.objQuestions data = new BAL.Models.objQuestions();
                //    data = (BAL.Models.objQuestions)bl.Get(Convert.ToInt32(edid));
                //    return View("_viewQuestion", data);
                //}

                //if (id == "_AddQuestionCategory" || id == "_editQuestionCategory")
                //{
                //    BAL.Bus_Logic.QuestionCategoryBAL bl = new BAL.Bus_Logic.QuestionCategoryBAL();
                //    BAL.Models.objQuestionCategory data = new BAL.Models.objQuestionCategory();
                //    BAL.Models.objQuestionCategory os = new BAL.Models.objQuestionCategory();
                //    data = (BAL.Models.objQuestionCategory)bl.Get(edid);
                //    data = data == null ? os : data;
                //    return View("_AddQuestionCategory", data);
                //}
                //else if (id == "_viewQuestionCategory")
                //{
                //    BAL.Bus_Logic.QuestionCategoryBAL bl = new BAL.Bus_Logic.QuestionCategoryBAL();
                //    BAL.Models.objQuestionCategory data = new BAL.Models.objQuestionCategory();
                //    data = (BAL.Models.objQuestionCategory)bl.Get(edid);
                //    return View("_viewQuestionCategory", data);
                //}

                else
                    return View("" + id + "");
            }
            else
                return null;

        }

        #region "questions"
        public ActionResult ManageAllQuestion(int chid)
        {

            objBookChapter objBookChapter = new objBookChapter();
            objQuestions oq = new objQuestions();

            BookChapterBAL bl = new BookChapterBAL();
            objBookChapter = (objBookChapter)bl.Get(chid);


            BookBal blb = new BookBal();
            objBook objBook = (objBook)blb.Get(objBookChapter.idBook);

            oq.idSeries = objBookChapter.idSeries;
            oq.idSubject = objBookChapter.idSubject;
            oq.idBook = objBookChapter.idBook;
            oq.idChapter = objBookChapter.id;

            oq.Subject = objBook.subject;
            oq.Series = objBook.series;
            oq.Book = objBook.title;
            return View(oq);

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ManageAllQuestion(FormCollection collection)
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            objQuestions oq = new objQuestions();
            oq.QimgPriority = false;
            oq.AnsimgPriority = false;
            

            objQueOptions op = null;
            List<objQueOptions> lstop = new List<objQueOptions>();
            HttpPostedFileBase postedFile = null;
            int files = Request.Files.Count;
            foreach (var key in collection.AllKeys)
            {
                var value = collection[key];
                if (key.Contains("ID"))
                {
                    value = collection[key];
                    oq.ID = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("idSubject"))
                {
                    value = collection[key];
                    oq.idSubject = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("idSeries"))
                {
                    value = collection[key];
                    oq.idSeries = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("idBook"))
                {
                    value = collection[key];
                    oq.idBook = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("idChapter"))
                {
                    value = collection[key];
                    oq.idChapter = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("idQuestionType"))
                {
                    value = collection[key];
                    oq.idQuestionType = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("Category"))
                {
                    value = collection[key];
                    oq.Category = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("Topic"))
                {
                    value = collection[key];
                    oq.Topic = Common_Static.ToSafeInt(value.Trim());
                }
                else if (key.Contains("isHot"))
                {
                    value = collection[key];
                    oq.isHot = value == "true" ? true : false;
                }
                else if (key.Contains("txtHead"))
                {
                    value = collection[key];

                    oq.isHeader = value.Trim();
                }

                if (key=="QImgPriority")
                {
                    value = collection["QImgPriority"];
                    oq.QimgPriority = value=="on"?true:false;
                }
                if (key=="AnsImgPriority")
                {
                    value = collection["AnsImgPriority"];
                    oq.AnsimgPriority = value == "on" ? true : false;
                }
                else if (key.Contains("isQuestion"))
                {
                    value = collection[key];

                    oq.isQuestion = value.Trim();
                    if (collection.AllKeys.Contains("txtAns"))
                    {
                        value = collection["txtAns"];
                        oq.isAns = value.Trim();
                    }

                    
                    // optImg
                    string oid = collection.Get("optImg");
                    string oAnsid = collection.Get("AnsImg");
                    if (!oid.IsNullOrWhiteSpace())
                    {
                        var qid = Convert.ToInt64(oid);
                        var question = context.tbl_Questions.Where(x => x.ID == qid).ToList();
                        if (question != null)
                        {
                            oq.isImage = question.FirstOrDefault().isImage;
                            oq.isExtension = question.FirstOrDefault().isExtension;
                        }
                    }
                    if (!oAnsid.IsNullOrWhiteSpace())
                    {
                        var qid = Convert.ToInt64(oAnsid);
                        var question = context.tbl_Questions.Where(x => x.ID == qid).ToList();
                        if (question != null)
                        {
                            oq.isAnsImage = question.FirstOrDefault().AnsImage;
                            oq.AnsExtension = question.FirstOrDefault().AnsExtension;
                        }
                    }

                    if (Request.Files.Count > 0 && Request.Files["file_Question"] != null && Request.Files["file_Question"].ContentLength > 0)
                    {
                        postedFile = Request.Files["file_Question"];
                        oq.isImage = Common_Static.ConvertToBytes(postedFile);
                        oq.isExtension = postedFile.ContentType;
                    }

                    if (Request.Files.Count > 0 && Request.Files["file_Ans"] != null && Request.Files["file_Ans"].ContentLength > 0)
                    {
                        postedFile = Request.Files["file_Ans"];
                        oq.isAnsImage = Common_Static.ConvertToBytes(postedFile);
                        oq.AnsExtension = postedFile.ContentType;
                    }
                    
                }
                
                else if (key.Contains("txtOption_"))
                {
                    string num = key.Replace("txtOption_", "");
                    value = collection[key];
                    op = new objQueOptions();

                    op.AnsimgPriority = false;
                    op.QimgPriority = false;

                    //if (collection.AllKeys.Contains("QImgPriority"))
                    //{
                    //    var val = collection["QImgPriority"];
                    //    oq.QimgPriority = val == "on" ? true : false;
                    //}
                    //if (collection.AllKeys.Contains("AnsImgPriority"))
                    //{
                    //    var val = collection["AnsImgPriority"];
                    //    oq.QimgPriority = val == "on" ? true : false;
                    //}

                    if (collection.AllKeys.Contains("QImgPriority_" + num))
                    {
                        var val = collection["QImgPriority_" + num];
                        op.QimgPriority = val == "on" ? true : false;
                    }
                    if (collection.AllKeys.Contains("AnsImgPriority_" + num))
                    {
                        var val = collection["AnsImgPriority_" + num];
                        op.AnsimgPriority = val == "on" ? true : false;
                    }

                   
                    if (value.Trim() != "" || 1==1 || (Request.Files.Count > 0 && Request.Files["fileOptChild_" + num.Trim() + ""] != null))
                    {
                        if(value!="")
                        op.isOption = value.Trim();
                        if (collection.AllKeys.Contains("txtOptionChild_" + num.Trim() + ""))
                        {
                            value = collection["txtOptionChild_" + num.Trim() + ""];
                            op.isChildOption = value.Trim();

                        }
                        if (collection.AllKeys.Contains("txtAns_" + num.Trim() + ""))
                        {
                            value = collection["txtAns_" + num.Trim() + ""];
                            op.isAns = value.Trim();
                        }

                        string oid = collection.Get("optImg_" + num.Trim());
                        string oAnsid = collection.Get("optAnsImg_" + num.Trim());
                        if (!oid.IsNullOrWhiteSpace() && oid != "undefined")
                        {
                            var optid = Convert.ToInt64(oid);
                            var option = context.tbl_Question_Options.Where(x => x.ID == optid).ToList();
                            if (option != null)
                            {
                                if (option.FirstOrDefault() != null)
                                {
                                    op.isImage = option.FirstOrDefault().isImage;

                                    op.isExtension = option.FirstOrDefault().isExtension;
                                }
                            }
                        }
                        if (!oAnsid.IsNullOrWhiteSpace() && oAnsid != "undefined")
                        {
                            var optid = Convert.ToInt64(oAnsid);
                            var option = context.tbl_Question_Options.Where(x => x.ID == optid).ToList();
                            if (option != null)
                            {
                                op.AnsImage = option.FirstOrDefault().AnsImage;
                                op.AnsExtension = option.FirstOrDefault().AnsExtension;
                            }
                        }

                        if (Request.Files.Count > 0 && Request.Files["fileOptChild_" + num.Trim() + ""] != null && Request.Files["fileOptChild_" + num.Trim() + ""].ContentLength > 0)
                        {
                            postedFile = Request.Files["fileOptChild_" + num.Trim() + ""];
                            op.isImage = Common_Static.ConvertToBytes(postedFile);
                            op.isExtension = postedFile.ContentType;
                        }

                        if (Request.Files.Count > 0 && Request.Files["fileOptChildAns_" + num.Trim() + ""] != null && Request.Files["fileOptChildAns_" + num.Trim() + ""].ContentLength > 0)
                        {
                            postedFile = Request.Files["fileOptChildAns_" + num.Trim() + ""];
                            op.AnsImage = Common_Static.ConvertToBytes(postedFile);
                            op.AnsExtension = postedFile.ContentType;
                        }

                   
                    }
                    lstop.Add(op);
                    lstop.RemoveAll(x => x.isImage == null && x.AnsImage==null && x.isAns==null && x.isOption==null);
                }
            }

            oq.lstQueOptions = lstop;
            QuestionsBAL qb = new QuestionsBAL();
           int i = qb.Add(oq);
            objBookChapter objBookChapter = new objBookChapter();

            BookChapterBAL bl = new BookChapterBAL();
            objBookChapter = (objBookChapter)bl.Get(oq.idChapter);
            BookBal blb = new BookBal();
            objBook objBook = (objBook)blb.Get(objBookChapter.idBook);
            objQuestions oqr = new objQuestions();

            oqr.idSeries = objBookChapter.idSeries;
            oqr.idSubject = objBookChapter.idSubject;
            oqr.idBook = objBookChapter.idBook;
            oqr.idChapter = objBookChapter.id;

            oqr.Subject = objBook.subject;
            oqr.Series = objBook.series;
            oqr.Book = objBook.title;

            return View(oqr);
        }
        public JsonResult getQuestion(string Id)
        {

            int i = Common_Static.ToSafeInt(Id);
            QuestionsBAL bl = new QuestionsBAL();
            objQuestions objQuestions = new objQuestions();
            objQuestions = bl.Get(i);


            return new JsonResult{Data= objQuestions,MaxJsonLength=int.MaxValue, JsonRequestBehavior=JsonRequestBehavior.AllowGet};
        }

        public JsonResult BatchUpdateQuestion(string idQuestion, int idQuestionCategory)
        {
            int i = 0;
            QuestionsBAL bl = new QuestionsBAL();
            i = bl.BatchUpdate(idQuestion, idQuestionCategory);
            return Json(new { data = i });
        }

        public JsonResult BatchUpdateStatusQuestion(string idQuestion)
        {
            int i = 0;
            QuestionsBAL bl = new QuestionsBAL();
            i = bl.BatchUpdateStatus(idQuestion, IsValid.Inactive);
            return Json(new { data = i });

        }
        #endregion

        public ActionResult ManageBooksQuestion(string id)
        {
            int idBook;
            int.TryParse(id, out idBook);
            BookBal bl = new BookBal();
            objBook obj = (objBook)(bl.Get(idBook));
            //obj.TopicID = 0;
            //obj.ChapterID = 0;
            return View(obj);
        }
        [HttpPost]
        public ActionResult ManageBooksQuestion(objBook obj)
        {
            BookBal bl = new BookBal();
            objBook objBook = new objBook();
            objBook = (objBook)bl.Get(obj.Id);
            objBook.ChapterID = obj.ChapterID;
            objBook.Id = obj.Id;
            objBook.book_id = obj.book_id;
            objBook.TopicID = obj.TopicID;
            objBook.ChapterID = obj.ChapterID;
            //objBook.ca = obj.ChapterID;
            QuestionsBAL blq = new QuestionsBAL();
            var lstQuestion = blq.BookQuestion(obj.Id, obj.isChapter, IsValid.Active, obj.book_id, obj.TopicID, obj.ChapterID);
            ViewBag.QuestionCount = lstQuestion.Count();
            ViewBag.results = lstQuestion.GroupBy(p => p.idQuestionType,
                     (key, g) => new { idQuestionType = key, lstObj = g.ToList() });
            return View(lstQuestion);
        }

        public PartialViewResult GetOptedQuestion(int Id, string ischapetr, int bookid, int TopicId, int Chaperid)
        {
            QuestionsBAL blq = new QuestionsBAL();
            if (ischapetr == "")
            {
                ischapetr = null;
            }
            var lstQuestion = blq.BookQuestion(Id, ischapetr, IsValid.All, bookid, TopicId, Chaperid);
            ViewBag.QuestionCount = lstQuestion.Count();
            ViewBag.results = lstQuestion.GroupBy(p => p.idQuestionType,
                     (key, g) => new objQuestions { idQuestionType = key, lstQuestions = g.ToList() }).ToList();
            BookChapterBAL blc = new BookChapterBAL();
            ViewBag.lstChpater = blc.GetAllByBook(Convert.ToString(Id));
            return PartialView("~/Areas/TestHour/Views/Shared/_ManageBooksQuestion.cshtml", lstQuestion);
        }

        [HttpPost]
        public ActionResult GetChapters(string BookID)
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            List<SelectListItem> Chapterlist = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(BookID))
            {
                Int64 Bookid = Convert.ToInt64(BookID);
                List<Chapter> districts = context.Chapters.Where(x => x.BookId == Bookid).ToList();
                districts.ForEach(x =>
                {
                    Chapterlist.Add(new SelectListItem { Text = x.Title, Value = x.Id.ToString() });
                });
            }
            return Json(Chapterlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTopic(string ChapterID)
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            List<SelectListItem> Chapterlist = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(ChapterID))
            {
                Int64 Chapterid = Convert.ToInt64(ChapterID);
                List<TopicMaster> districts = context.TopicMasters.Where(x => x.CHAPTERID == Chapterid).ToList();
                districts.ForEach(x =>
                {
                    Chapterlist.Add(new SelectListItem { Text = x.TOPIC, Value = x.ID.ToString() });
                });
            }
            return Json(Chapterlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuestionType()
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            List<SelectListItem> Typelist = new List<SelectListItem>();

            List<QuestionType> questionType = context.QuestionTypes.ToList();
            questionType.ForEach(x =>
             {
                 Typelist.Add(new SelectListItem { Text = x.TYPE, Value = x.ID.ToString() });
             });

            return Json(Typelist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateQuestions(string QID, string CategoryID, string ChapterId, string TopicID, string TypeID)
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            List<SelectListItem> Chapterlist = new List<SelectListItem>();

            Int64 quid = Convert.ToInt64(QID);
            tbl_Questions questionobj = context.tbl_Questions.Where(x => x.ID == quid).FirstOrDefault();
            if (questionobj != null)
            {
                questionobj.idQuestionType = Convert.ToInt32(CategoryID);
                questionobj.idChapter = Convert.ToInt32(ChapterId);
                questionobj.Topic = Convert.ToInt32(TopicID);
                questionobj.Category = Convert.ToInt32(TypeID);
                context.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult AddTopic(int Id,string bookid)
        //{
        //    using (var context = new dbPrachiIndia_PortalEntities())
        //    {
        //        TopicMaster  objTopicMaster = new TopicMaster();
        //        var  TopicList = context.TopicMasters.Where(x => x.CHAPTERID == Id && x.STATUS == true).ToList();
        //        var model = new Tuple<List<TopicMaster>, int,string>(TopicList, Id,bookid);
        //        return PartialView("~/Areas/TestHour/Views/Shared/_TopicPartial.cshtml", model);
        //    }
        //}
        //public JsonResult Topic(int Id,string Topic,string bookid)
        //{
        //    using (var context = new dbPrachiIndia_PortalEntities())
        //    {
        //        TopicMaster objTopicMaster = new TopicMaster();

        //        objTopicMaster.TOPIC = Topic;
        //        objTopicMaster.CHAPTERID = Convert.ToInt64(Id);               
        //        objTopicMaster.BookID = Convert.ToInt64(bookid);
        //        context.TopicMasters.Add(objTopicMaster);
        //        context.SaveChanges();
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //}


        public ActionResult textHtml(string id)
        {

            int idBook;
            int.TryParse(id, out idBook);
            BookBal bl = new BookBal();
            objBook obj = (objBook)(bl.Get(idBook));
            //obj.TopicID = 0;
            //obj.ChapterID = 0;
            return View(obj);
        }

        public ActionResult GetOptedQuestionTest(int Id, string ischapetr, int bookid, int TopicId, int Chaperid)
        {
            QuestionsBAL blq = new QuestionsBAL();
            if (ischapetr == "")
            {
                ischapetr = null;
            }
            var lstQuestion = blq.BookQuestion(Id, ischapetr, IsValid.All, bookid, TopicId, Chaperid);
            ViewBag.QuestionCount = lstQuestion.Count();
            ViewBag.results = lstQuestion.GroupBy(p => p.idQuestionType,
                     (key, g) => new objQuestions { idQuestionType = key, lstQuestions = g.ToList() }).ToList();
            BookChapterBAL blc = new BookChapterBAL();
            ViewBag.lstChpater = blc.GetAllByBook(Convert.ToString(Id));

            QuestionCategoryBAL qcb = new QuestionCategoryBAL();
            var lstQC = qcb.GetAll(IsValid.All);
            string html = string.Empty;
            if (lstQuestion.Count() > 0)
            {
                var count = 1;
                foreach (var ct in (List<objQuestions>)ViewBag.results)
                {
                    var cat = (from x in lstQC where x.ID == ct.idQuestionType select x).First();
                    html = html + "<tr>";
                    html = html + "<td colspan = '2'>";
                    html = html + "<h4>" + cat.isTitle + " </h4>";
                    html = html + "<table style = 'margin:5px;' width = '98%' cellpadding = '0' cellspacing = '0' border='0'>";

                    foreach (var qu in ct.lstQuestions)
                    {
                        var item = (from x in (List<objBookChapter>)ViewBag.lstChpater where x.id == qu.idChapter select x).First();
                        html = html + "<tr>";
                        html = html + "</tr>";
                        html = html + "<tr>";

                        html = html + "<td valign='top'>Q.NO " + count + "</td><td colspan = '3'>";
                        html = html + "<h4 style='margin-top:0'>" + qu.isQuestion;

                        if (qu.isImage.LongLength > 0)
                        {
                            html = html + "<br/> <img style = 'max-width:200px' src = 'data:" + qu.isExtension + ";base64," + System.Convert.ToBase64String(qu.isImage) + "'/>";
                        }
                        html = html + "</h4>";
                        html = html + "<ul class='ultp' style='list-style-type: decimal-leading-zero; margin-left: 30px'>";
                        if (cat.ID == 7)
                        {
                            var optList = new List<string>();
                            var ansList = new List<string>();
                            foreach (var opt in qu.lstQueOptions)
                            {
                                var optHtml = "<b>" + opt.isOption + "</b>";
                                optList.Add(optHtml);

                                var ansHtml = "<br/>" + opt.isAns;
                                if (opt.isImage.LongLength > 0)
                                {
                                    ansHtml = ansHtml + "<br/><img style = 'max -width:200px' src = 'data:" + opt.isExtension + ";base64," + System.Convert.ToBase64String(opt.isImage) + "'/>";
                                }
                                ansList.Add(ansHtml);
                            }
                            //var ColumnA = List.Randomize(optList).ToList();
                            var ColumnA = optList;
                            var ColumnB = List.Randomize(ansList).ToList();
                            for (int i = 0; i < ColumnA.Count; i++)
                            {
                                html = html + "<li style = 'line -height: 20px'>";
                                html = html + "<div style='float:left;width:49%;'>";
                                html = html + ColumnA[i];
                                html = html + "</div><div style='float:left;width:49%;'>";
                                html = html + ColumnB[i];
                                html = html + "</div><div clear:both; height:1px;></div></li>";
                            }
                        }
                        else
                        {
                            foreach (var opt in qu.lstQueOptions)
                            {
                                html = html + "<li style = 'line -height: 20px'>";
                                html = html + "<b>" + opt.isOption + "</b>";
                                if (opt.isChildOption != "")
                                {
                                    html = html + "<br/><text> ChildOption:-</text>";
                                }
                                if (opt.isImage.LongLength > 0)
                                {
                                    html = html + "<br/><img style = 'max -width:200px' src = 'data:" + opt.isExtension + ";base64," + System.Convert.ToBase64String(opt.isImage) + "'/>";
                                }
                                if (!string.IsNullOrEmpty(opt.isAns))
                                {
                                    html = html + "<br/>Ans: " + opt.isAns;
                                }

                                html = html + "</li>";
                            };
                        }

                        html = html + "</ul></td></tr>";
                        count = count + 1;
                    }
                    html = html + "</td></tr>";
                    html = html + "</table>";
                }

            }
            else
            {
                html = html + "<tr><td colspan = '2'><div class='ndata'>No Data found................</div></td></tr>";
            }

            return Content(html);
        }
        //private List<E> ShuffleList<E>(List<E> inputList)
        //{
        //    List<E> randomList = new List<E>();

        //    Random r = new Random();
        //    int randomIndex = 0;
        //    while (inputList.Count > 0)
        //    {
        //        randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
        //        randomList.Add(inputList[randomIndex]); //add it to the new, random list
        //        inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        //    }

        //    return randomList; //return the new random list
        //}


        public JsonResult DeleteQuestion(int qid, int bid)
        {

            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            var question = context.tbl_Questions.Include("tbl_Question_Options").First(x => x.ID == qid);

            context.tbl_Questions.Remove(question);
            context.SaveChanges();

            return Json("Success",JsonRequestBehavior.AllowGet);
        }
    }

    public static class List
    {
        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy<T, int>((item) => rnd.Next());
        }
    }
}