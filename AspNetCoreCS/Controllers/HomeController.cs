using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadEdgeCore.Models;
using ReadEdgeCore.Models.Interfaces;
using ReadEdgeCore.Models.ViewModel;
using ReadEdgeCore.Utilities;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using GleamTech.DocumentUltimateExamples.AspNetCoreCS.Filters;
using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using static GleamTech.DocumentUltimateExamples.AspNetCoreCS.Models.TestHOurModels;
using System.Net.Http.Headers;
using RestSharp;
using System.Net;

namespace ReadEdgeCore.Controllers
{
    #region Home

    #endregion
    public class HomeController : Controller
    {
        private readonly IUser _user;
        private readonly ILibrary _library;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private static IReaderBooks _readerBooks;
        private readonly IEbookReader _ebookReader;
        private readonly IConfiguration _iConfig;
        private readonly ErrorLog _errorLog;

        private Dictionary<string, string> Inputs = new Dictionary<string, string>();
        //public string Url = "http://202.140.136.39:82/Account/GoToTestEdge";
        public string Url = "http://testhour.mielib.com/Account/GoToTestEdge";
        //public string Method = "post";
        public string FormName = "TestHourForm";

        // private IHttpContextAccessor _HttpContextAccessor;
        // private ISession _session => _httpContextAccessor.HttpContext.Session;
        public HomeController(IUser user, ILibrary library, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IReaderBooks readerBooks, IEbookReader ebookReader, IConfiguration iConfig, ErrorLog errorLog)
        {
            _user = user;
            _library = library;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _ebookReader = ebookReader;
            _readerBooks = readerBooks;
            _iConfig = iConfig;
            _errorLog = errorLog;
        }
        public IActionResult Index()
        {
            try
            {

                //if (_user.GetAllUser().Result.Any())
                //{
                return RedirectToAction("Login", "Account");

                //}
                //return View("Configuration");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Home()
        {
    
            return View();
        }

        public async Task<IActionResult> Configuration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Configuration(LibraryVM userVM)
        {

            await _library.UploadConfig(userVM.ConfigurationFile);
            return Json("true");
        }

        public async Task<IActionResult> ReConfiguration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReConfiguration(LibraryVM userVM)
        {
            await _library.UploadConfig(userVM.ConfigurationFile);
            return Json("true");
        }

        public async Task<IActionResult> UploadBundle()
        {
            var classes = await _library.GetClass();
            List<ClassModel> classList = Factory.GetClassModelList();

            classList = classes.Where(x => x.Status == 1).OrderBy(y => y.OredrNo).ToList();
            classList.Insert(0, new ClassModel { Id = "0", Title = "Select" });
            ViewBag.ListOfClasses = classList;

            var books = await _library.GetBooks();
            List<BookModel> bookList = Factory.GetBookModelList();

            bookList = books.OrderBy(y => y.Title).ToList();
            bookList.Insert(0, new BookModel { Id = 0, Title = "Select" });
            ViewBag.LiostOfBooks = bookList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadBundlePackage(LibraryVM libraryVM)
        {
            libraryVM.BundlePackage = true;
            await _library.UploadBundle(libraryVM);
            return Json("true");
        }
        [HttpPost]
        public async Task<IActionResult> UploadBundle(LibraryVM libraryVM)
        {
            libraryVM.BundlePackage = false;
            await _library.UploadBundle(libraryVM);
            return Json("true");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                var exceptionObject = HttpContext.Features.Get<IExceptionHandlerFeature>();

                if (null != exceptionObject)
                {
                    var errorMessage = exceptionObject.Error.Message + exceptionObject.Error.StackTrace;
                    _errorLog.ErrorMsg = errorMessage;
                    _errorLog.CreatedDate = DateTime.UtcNow.ToLocalTime();
                    _library.LogError(_errorLog);
                }

                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            //  return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> GetBookByClass(Int32 ClassId)
        {
            var books = await _library.GetBooks();
            List<BookModel> bookList = Factory.GetBookModelList();
            var userLibrary = await _library.GetAllLibrary();
            var BookIds = userLibrary.Select(y => y.BookId).ToList();
            bookList = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.ClassId == ClassId).OrderBy(y => y.Title).ToList();
            bookList.Insert(0, new BookModel { Id = 0, Title = "Select" });

            return Json(bookList);
        }
        [ServiceFilter(typeof(AsyncActionFilter))]
        public async Task<ActionResult> Dashboard(int type = 1)
        {
            var classes = await _library.GetClass();
            List<ClassModel> classList = Factory.GetClassModelList();
            List<ClassSubjects> classSubjectsList = new List<ClassSubjects>();
            _httpContextAccessor.HttpContext.Session.SetInt32("ClassType", type);
            //if (type == 1)
            //{
            //    classList = classes.Where(x => x.Status == 1 && x.OredrNo < 15).OrderBy(y => y.OredrNo).ToList();

            //}
            //else
            //{
            //    classList = classes.Where(x => x.Status == 1 && x.OredrNo >= 15).OrderBy(y => y.OredrNo).ToList();
            //}
            classList = classes.Where(x => x.Status == 1).OrderBy(y => y.OredrNo).ToList();
            ViewBag.ClassList = classList;
            ViewBag.Subjects = _library.GetSubjectByClassType(type).ToList();
            var userLibrary = await _library.GetAllLibrary();


            List<long> BookIds = new List<long>();

            var Userbookids = _httpContextAccessor.HttpContext.Session.GetString("Userbookids") ?? "";
            var role = _httpContextAccessor.HttpContext.Session.GetString("Role") ?? "";
            //var TeacherclassIds = "16,17,18,19";
            //var classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();
            var lstuserbookidids = Userbookids.Split(',').ToList();
            if (Userbookids != "")
            {
                BookIds = lstuserbookidids.Select(x => long.Parse(x)).ToList();
            }

            else
            {
                BookIds = userLibrary.Select(y => y.BookId).ToList();
            }

            var books = await _library.GetBooks();
            ViewBag.SubjectId = 0;
            List<LibraryVM> libraryVMs = Factory.GetLibrayVMList();
            if (role.ToLower() == "teacher")
            {
                var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                var TeacherSubjectClasses = _library.GetTeacherSubjectClasses(Userid);
                var classidlist = TeacherSubjectClasses.Select(x => long.Parse(x.ClassId.ToString())).ToList();
                var subjectList = TeacherSubjectClasses.Select(x => long.Parse(x.SubjectId.ToString())).ToList();
                if (TeacherSubjectClasses.Count == 0)
                {
                    var TeacherclassIds = "16,17,18,19";
                    classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();
                }
                if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                {
                    if (TeacherSubjectClasses.Count != 0)
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && classidlist.Contains(x.ClassId) && subjectList.Contains(x.SubjectId) && x.ClassId < 16)
                   .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = true }).Take(50).ToList();


                    }
                    else {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && classidlist.Contains(x.ClassId) && x.ClassId < 16)
               .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = true }).Take(50).ToList();

                    }

                }
                else
                {
                    if (TeacherSubjectClasses.Count != 0)
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && classidlist.Contains(x.ClassId) && subjectList.Contains(x.SubjectId) && x.ClassId > 15)
.Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = true }).Take(50).ToList();
                    }
                    else
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && classidlist.Contains(x.ClassId) && x.ClassId > 15)
  .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = true }).Take(50).ToList();

                    }
                }

            }
            else
            {
                if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                {
                    libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.ClassId < 16)
                              .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = true }).Take(50).ToList();

                }
                else
                {
                    libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.ClassId > 15)
                           .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = true }).Take(50).ToList();

                }
            }

            return View(libraryVMs);

        }


        public async Task<ActionResult> SubjectDetails(long ClassId)
        {
            var subjectIds = await _library.GetSubjectByClass(ClassId);
            var subjectIdList = subjectIds.ToList();
            var subjects = await _library.GetSubjects();
            var reuslt = subjects.Where(x => (subjectIdList.Contains(Convert.ToInt64(x.Id)))).ToList();
            ViewBag.Subjects = reuslt;
            ViewBag.ClassID = ClassId;
            return View();
        }
        [ServiceFilter(typeof(AsyncActionFilter))]
        public async Task<IActionResult> MyLibrary(Int32 SubjectID = 0, Int32 ClassID = 0)
        {
            //Searching by Subject and class added 17.03.2020
            var SubjectList = await _library.GetSubjects();
            ViewBag.Subject = SubjectList;
            var ClassList = await _library.GetClass();
            if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
            {
                ViewBag.Class = ClassList.Where(x => x.Status == 1 && x.OredrNo < 15).ToList();
            }
            else
            {
                ViewBag.Class = ClassList.Where(x => x.Status == 1 && x.OredrNo >= 15).ToList();
            }


            var userLibrary = await _library.GetAllLibrary();
            List<long> BookIds = new List<long>();

            var Userbookids = _httpContextAccessor.HttpContext.Session.GetString("Userbookids") ?? "";
            var role = _httpContextAccessor.HttpContext.Session.GetString("Role") ?? "";
            //var TeacherclassIds = "16,17,18,19";
            //var classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();

            var lstuserbookidids = Userbookids.Split(',').ToList();
            if (Userbookids != "")
            {
                BookIds = lstuserbookidids.Select(x => long.Parse(x)).ToList();
            }

            else
            {
                BookIds = userLibrary.Select(y => y.BookId).ToList();
            }

            var books = await _library.GetBooks();
            var uploadedBundle = userLibrary.Where(x => x.BundleUploaded == true).Select(y => y.BookId);
            ViewBag.SubjectId = SubjectID;
            ViewBag.ClassID = ClassID;

            List<LibraryVM> libraryVMs = Factory.GetLibrayVMList();
            if (SubjectID != 0)
            {

                if (role.ToLower() == "teacher")
                {
                    var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    var TeacherSubjectClasses = _library.GetTeacherSubjectClasses(Userid);
                    var classidlist = TeacherSubjectClasses.Select(x => long.Parse(x.ClassId.ToString())).ToList();
                    var subjectList = TeacherSubjectClasses.Select(x => long.Parse(x.SubjectId.ToString())).ToList();
                    if (TeacherSubjectClasses.Count == 0)
                    {
                        var TeacherclassIds = "16,17,18,19";
                        classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();
                    }
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && ((classidlist.Contains(x.ClassId) || ClassID == 0) && subjectList.Contains(x.SubjectId) && x.ClassId < 16))
                            .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();

                        }
                        else {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && ((classidlist.Contains(x.ClassId) || ClassID == 0) && x.ClassId < 16))
                          .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();

                        }
                    }
                    else
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {

                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && ((classidlist.Contains(x.ClassId) || ClassID == 0) && subjectList.Contains(x.SubjectId) && x.ClassId > 15))
                         .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();

                        }
                        else {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && ((classidlist.Contains(x.ClassId) || ClassID == 0) && x.ClassId > 15))
.Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();

                        }
                    }

                }
                else
                {
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId < 16))
                      .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();
                    }
                    else
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId > 15))
                        .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();

                    }

                }
            }
            else
            {
                if (role.ToLower() == "teacher")
                {
                    var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    var TeacherSubjectClasses = _library.GetTeacherSubjectClasses(Userid);
                    var classidlist = TeacherSubjectClasses.Select(x => long.Parse(x.ClassId.ToString())).ToList();
                    var subjectList = TeacherSubjectClasses.Select(x => long.Parse(x.SubjectId.ToString())).ToList();
                    if (TeacherSubjectClasses.Count == 0)
                    {
                        var TeacherclassIds = "16,17,18,19";
                        classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();
                    }
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((classidlist.Contains(x.ClassId)) && subjectList.Contains(x.SubjectId) && x.ClassId < 16))
                            .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();

                        }

                        else {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((classidlist.Contains(x.ClassId)) && x.ClassId < 16))
.Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();

                        }
                    }
                    else
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((classidlist.Contains(x.ClassId)) && subjectList.Contains(x.SubjectId) && x.ClassId > 15))
                    .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();

                        }
                        else {
                            libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((classidlist.Contains(x.ClassId)) && x.ClassId > 15))
                    .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();

                        }

                    }


                }
                else
                {
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId < 16))
                         .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();

                    }
                    else
                    {
                        libraryVMs = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId > 15))
                       .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();

                    }

                }
            }
            return View(libraryVMs);

            //if (SubjectID != 0)
            //    libraryVMs = books.Where(x => x.Status == true && x.IsEbook == true && x.SubjectId == SubjectID && (x.ClassId == ClassID || ClassID == 0))
            //                   .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false, }).ToList();
            //else
            //    libraryVMs = books.Where(x => x.Status == true && x.IsEbook == true && (x.ClassId == ClassID || ClassID == 0))
            //        .Select(x => new LibraryVM { BookId = x.Id, Author = x.Author, Image = x.ImageName, BookName = x.Title, IsInLibrary = true, IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false }).ToList();
            //return View(libraryVMs);

        }
        public async Task<IActionResult> SearchMyLibrary(string parameter, Int32 SubjectID = 0, Int32 ClassID = 0)
        {
            var userLibrary = await _library.GetAllLibrary();
            //var BookIds = userLibrary.Select(y => y.BookId).ToList();
            List<long> BookIds = new List<long>();
            var Userbookids = _httpContextAccessor.HttpContext.Session.GetString("Userbookids") ?? "";

            var lstuserbookidids = Userbookids.Split(',').ToList();
            if (Userbookids != "")
            {
                BookIds = lstuserbookidids.Select(x => long.Parse(x)).ToList();
            }

            else
            {
                BookIds = userLibrary.Select(y => y.BookId).ToList();
            }

            var role = _httpContextAccessor.HttpContext.Session.GetString("Role") ?? "";
            //var TeacherclassIds = "16,17,18,19";
            //var classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();

            var books = await _library.GetBooks();
            var uploadedBundle = userLibrary.Where(x => x.BundleUploaded == true).Select(y => y.BookId);
            List<LibraryVM> libraryVMs = Factory.GetLibrayVMList();
            IEnumerable<BookModel> bookModels;
            if (!string.IsNullOrWhiteSpace(parameter) || !string.IsNullOrEmpty(parameter))
            {
                if (role.ToLower() == "teacher")
                {
                    var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    var TeacherSubjectClasses = _library.GetTeacherSubjectClasses(Userid);
                    var classidlist = TeacherSubjectClasses.Select(x => long.Parse(x.ClassId.ToString())).ToList();
                    var subjectList = TeacherSubjectClasses.Select(x => long.Parse(x.SubjectId.ToString())).ToList();

                    if (TeacherSubjectClasses.Count == 0)
                    {
                        var TeacherclassIds = "16,17,18,19";
                        classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();
                    }
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && subjectList.Contains(x.SubjectId) && x.ClassId < 16));
                        }
                        else {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && x.ClassId < 16));

                        }
                    }
                    else
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && subjectList.Contains(x.SubjectId) && x.ClassId > 15));
                        }
                        else {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && x.ClassId > 15));

                        }
                    }
                }
                else
                {
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId < 16));
                    }
                    else
                    {
                        bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId > 15));

                    }
                }
            }
            else
            {


                if (role.ToLower() == "teacher")
                {
                    var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");
                    var TeacherSubjectClasses = _library.GetTeacherSubjectClasses(Userid);
                    var classidlist = TeacherSubjectClasses.Select(x => long.Parse(x.ClassId.ToString())).ToList();
                    var subjectList = TeacherSubjectClasses.Select(x => long.Parse(x.SubjectId.ToString())).ToList();
                    if (TeacherSubjectClasses.Count == 0)
                    {
                        var TeacherclassIds = "16,17,18,19";
                        classidlist = TeacherclassIds.Split(',').ToList().Select(x => long.Parse(x)).ToList();
                    }
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        if (TeacherSubjectClasses.Count != 0)
                        {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && subjectList.Contains(x.SubjectId) && x.ClassId < 16));
                        }
                        else {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && x.ClassId < 16));

                        }
                    }
                    else
                    {
                        if (subjectList.Count != 0)
                        {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((classidlist.Contains(x.ClassId)) && subjectList.Contains(x.SubjectId) && x.ClassId > 15));
                        }
                        else {
                            bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((classidlist.Contains(x.ClassId)) && x.ClassId > 15));
                        }
                    }
                }
                else
                {
                    if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
                    {
                        bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId < 16));
                    }
                    else
                    {
                        bookModels = books.Where(x => BookIds.Contains(x.Id) && x.Status == true && x.IsEbook == true && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && ((x.ClassId == ClassID || ClassID == 0) && x.ClassId > 15));

                    }
                }
            }
            //if (!string.IsNullOrWhiteSpace(parameter) || !string.IsNullOrEmpty(parameter))
            //    bookModels = books.Where(x => x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()) && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && (x.ClassId == ClassID || ClassID == 0));
            //else
            //    bookModels = books.Where(x => x.Status == true && x.IsEbook == true && ((x.SubjectId == SubjectID) || (SubjectID == 0)) && (x.ClassId == ClassID || ClassID == 0));


            libraryVMs = bookModels.Select(x => new LibraryVM
            {
                BookId = x.Id,
                Author = x.Author,
                Image = x.ImageName,
                BookName = x.Title,
                IsInLibrary = true,
                IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false,
            }).ToList();

            return PartialView("_Library", libraryVMs);
        }

        [ServiceFilter(typeof(AsyncActionFilter))]
        public async Task<IActionResult> Library()
        {
            var userLibrary = await _library.GetAllLibrary();
            var BookIds = userLibrary.Select(y => y.BookId).ToList();
            var uploadedBundle = userLibrary.Where(x => x.BundleUploaded == true).Select(y => y.BookId);
            var books = await _library.GetBooks();
            List<LibraryVM> libraryVMs = Factory.GetLibrayVMList();

            if (_httpContextAccessor.HttpContext.Session.GetInt32("ClassType") == 1)
            {
                libraryVMs = books.Where(x => x.Status == true && x.IsEbook == true && x.ClassId < 16)
         .Select(x => new LibraryVM
         {
             BookId = x.Id,
             Author = x.Author,
             Image = x.ImageName,
             BookName = x.Title,
             IsInLibrary = BookIds.Contains(x.Id) ? true : false,
             IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false,
         }).ToList();
            }
            else
            {
                libraryVMs = books.Where(x => x.Status == true && x.IsEbook == true && x.ClassId > 15)
      .Select(x => new LibraryVM
      {
          BookId = x.Id,
          Author = x.Author,
          Image = x.ImageName,
          BookName = x.Title,
          IsInLibrary = BookIds.Contains(x.Id) ? true : false,
          IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false,
      }).ToList();
            }

            return View(libraryVMs);
        }
        public async Task<IActionResult> SearchLibrary(string parameter)
        {
            var userLibrary = await _library.GetAllLibrary();
            var BookIds = userLibrary.Select(y => y.BookId).ToList();
            var books = await _library.GetBooks();
            var uploadedBundle = userLibrary.Where(x => x.BundleUploaded == true).Select(y => y.BookId);
            List<LibraryVM> libraryVMs = Factory.GetLibrayVMList();
            IEnumerable<BookModel> bookModels;
            if (!string.IsNullOrWhiteSpace(parameter) || !string.IsNullOrEmpty(parameter))
                bookModels = books.Where(x => x.Status == true && x.IsEbook == true && x.Title.ToLower().Contains(parameter.ToLower()));
            else
                bookModels = books.Where(x => x.Status == true && x.IsEbook == true);
            libraryVMs = bookModels.Select(x => new LibraryVM
            {
                BookId = x.Id,
                Author = x.Author,
                Image = x.ImageName,
                BookName = x.Title,
                IsInLibrary = BookIds.Contains(x.Id) ? true : false,
                IsBundleUploaded = uploadedBundle.Contains(x.Id) ? true : false,
            }).ToList();

            return PartialView("_Library", libraryVMs);
        }
        public async Task<ActionResult> BookDetail(long bookid)
        {
            var IsVerified = _httpContextAccessor.HttpContext.Session.GetString("IsVerified");
            var otp = _httpContextAccessor.HttpContext.Session.GetString("OTP");
            ViewBag.AlreadyUser = true;
            if (otp == null || otp == "")
            {
                ViewBag.AlreadyUser = false;
            }
            if (IsVerified == "YES")
            {
                ViewBag.AlreadyUser = true;
            }
            var result = _library.GetAllLibrary().Result;
            var File = result.Where(x => x.BookId == bookid).FirstOrDefault();
            var book = await _library.GetBooks(bookid);

            //var isPdf = book.EpubName.Split("_pdf")[1];
            //if (isPdf != null)
            //    ViewBag.CourseBookType = 5;
            //else
            //    ViewBag.CourseBookType = 0;


            if (File.BundleUploaded && book.IsEbook)
            {
                book.IsEbook = true;
            }
            else
            {
                book.IsEbook = false;
            }
            if (File.BundleUploaded && book.IsLessonPlan)
            {
                book.IsLessonPlan = true;
            }
            else
            {
                book.IsLessonPlan = false;
            }
            if (File.BundleUploaded && book.IsMultiMedia)
            {
                book.IsMultiMedia = true;
            }
            else
            {
                book.IsMultiMedia = false;
            }

            if (File.BundleUploaded && book.IsWorkbook)
            {
                book.IsWorkbook = true;
            }
            else
            {
                book.IsWorkbook = false;
            }

            if (File.BundleUploaded && book.IsSolutions)
            {
                book.IsSolutions = true;
            }
            else
            {
                book.IsSolutions = false;
            }

            if (File.BundleUploaded && book.BEP)
            {
                book.BEP = true;
            }
            else
            {
                book.BEP = false;
            }
            if (File.BundleUploaded && book.CBSE)
            {
                book.CBSE = true;
            }
            else
            {
                book.CBSE = false;
            }
            if (File.BundleUploaded && book.MTP)
            {
                book.MTP = true;
            }
            else
            {
                book.MTP = false;
            }
            if (File.BundleUploaded && book.TPG)
            {
                book.TPG = true;
            }
            else
            {
                book.TPG = false;
            }
            if (File.BundleUploaded && book.ConceptMap)
            {
                book.ConceptMap = true;
            }
            else
            {
                book.ConceptMap = false;
            }
            var role = _httpContextAccessor.HttpContext.Session.GetString("Role") ?? "";
            if (role.ToLower() == "user")
            {
                book.IsLessonPlan = false;
                //book.IsSolutions = false;
                //book.IsWorkbook = false;
                //book.IsMultiMedia = false;
            }
            ViewBag.Book = book;
            ViewBag.UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName") ?? "";
            ViewBag.Password = _httpContextAccessor.HttpContext.Session.GetString("Password") ?? "";
            //var client = new HttpClient();
            //// var clie  client.BaseAddress = new Uri("http://localhost:60464/api/");nt = new HttpClient("http://testhour.mielib.com/api/masterdata");
            //client.BaseAddress = new Uri("http://testhour.mielib.com/api/masterdata");
            ////var request = new RestRequest(Method.GET);
            //client.DefaultRequestHeaders.Add("api_key", "6stSN88P7Yh+AQU8+0Dp4VH6v5i2l8xjTiFkE8HzB+j3pGEt+aUFE5Ied/O5U4nCUopJX7V1Om9ur/dQG9P2ObGcggumlKd31fYlp9yMfkGsYP2uo6dMucbNzVycPKuDAndnyPgkaJtS6Jr/K0Av93w/oS0K1MHwQNlYOi6Qi1+lXyhyPNIG4Ki5j9kf9PfrKY9H22E6ClNr5jfpDTKhuAp2tuqdyPSaOlR5jKYKsIY2Ys7ASP6HC+VFFps4/WtBD5/dqfg0th1NU06G8PcKFTHqpzZmH0YXXTBxA0Blyxhuv0dTn4b8Xp+vCtgPtwyQtSU78y/qss25MPF0Dwgmupcj1j5DzXmNkEjCwaCNOGQ=");
            ////client.AddHeader("api_key", "6stSN88P7Yh+AQU8+0Dp4VH6v5i2l8xjTiFkE8HzB+j3pGEt+aUFE5Ied/O5U4nCUopJX7V1Om9ur/dQG9P2ObGcggumlKd31fYlp9yMfkGsYP2uo6dMucbNzVycPKuDAndnyPgkaJtS6Jr/K0Av93w/oS0K1MHwQNlYOi6Qi1+lXyhyPNIG4Ki5j9kf9PfrKY9H22E6ClNr5jfpDTKhuAp2tuqdyPSaOlR5jKYKsIY2Ys7ASP6HC+VFFps4/WtBD5/dqfg0th1NU06G8PcKFTHqpzZmH0YXXTBxA0Blyxhuv0dTn4b8Xp+vCtgPtwyQtSU78y/qss25MPF0Dwgmupcj1j5DzXmNkEjCwaCNOGQ=");
            //// IRestResponse response = client.Execute(request);
            //var response = client.GetAsync(client.BaseAddress);
            ////var results = JsonConvert.DeserializeObject<Response<MasterDatatModel>>(response);
            ///

            //var client = new RestClient("http://testhour.mielib.com/api/masterdata");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("api_key", "6stSN88P7Yh+AQU8+0Dp4VH6v5i2l8xjTiFkE8HzB+j3pGEt+aUFE5Ied/O5U4nCUopJX7V1Om9ur/dQG9P2ObGcggumlKd31fYlp9yMfkGsYP2uo6dMucbNzVycPKuDAndnyPgkaJtS6Jr/K0Av93w/oS0K1MHwQNlYOi6Qi1+lXyhyPNIG4Ki5j9kf9PfrKY9H22E6ClNr5jfpDTKhuAp2tuqdyPSaOlR5jKYKsIY2Ys7ASP6HC+VFFps4/WtBD5/dqfg0th1NU06G8PcKFTHqpzZmH0YXXTBxA0Blyxhuv0dTn4b8Xp+vCtgPtwyQtSU78y/qss25MPF0Dwgmupcj1j5DzXmNkEjCwaCNOGQ=");
            //IRestResponse response = client.Execute(request);
            //var results = JsonConvert.DeserializeObject<Response<MasterDatatModel>>(response.Content)




            var classTitle = await _library.GetClass(Convert.ToString(book.ClassId));
            var subjectTitle = await _library.GetSubjects(Convert.ToString(book.SubjectId));
            var client = new HttpClient();

            //client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.Add("api_key", "6stSN88P7Yh+AQU8+0Dp4VH6v5i2l8xjTiFkE8HzB+j3pGEt+aUFE5Ied/O5U4nCUopJX7V1Om9ur/dQG9P2ObGcggumlKd31fYlp9yMfkGsYP2uo6dMucbNzVycPKuDAndnyPgkaJtS6Jr/K0Av93w/oS0K1MHwQNlYOi6Qi1+lXyhyPNIG4Ki5j9kf9PfrKY9H22E6ClNr5jfpDTKhuAp2tuqdyPSaOlR5jKYKsIY2Ys7ASP6HC+VFFps4/WtBD5/dqfg0th1NU06G8PcKFTHqpzZmH0YXXTBxA0Blyxhuv0dTn4b8Xp+vCtgPtwyQtSU78y/qss25MPF0Dwgmupcj1j5DzXmNkEjCwaCNOGQ=");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("bookname", book.Title);
            //client.DefaultRequestHeaders.Add("subjectname", book.SubjectId.ToString());
            //client.DefaultRequestHeaders.Add("isbnname", "");
            //client.DefaultRequestHeaders.Add("classname", book.ClassId.ToString());

            var parameters = new Dictionary<string, string> { { "bookname", book.Title }, { "subjectname", subjectTitle.Title.Trim() },{ "isbnname", book.Isbn }, { "classname", classTitle.Title.Trim() } };
            var encodedContent = new FormUrlEncodedContent(parameters);


            //var url = "http://202.140.136.39:82/api/MasterData/SearchBook";
            var url = "http://testhour.mielib.com/api/MasterData/SearchBook";
            //HttpResponseMessage response = await client.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            //var resp = await response.Content.ReadAsStringAsync();

            var response = await client.PostAsync(url, encodedContent);
            //var response = client.PostAsync(url, new StringContent(parameters)).GetAwaiter().GetResult();
            var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var results = JsonConvert.DeserializeObject<Response<SearchBookModel>>(content);
            //return content;

            //var Masterdata = JsonConvert.DeserializeObject<Response<MasterDatatModel>>(resp);
            //var TestHourBooks = Masterdata.Data.Books;

            //var TestHourbook = TestHourBooks.Where(x => x.Title==book.Title).FirstOrDefault();
           
            ViewBag.THBid = 0;
            if (results.Data != null)
            {
                ViewBag.THBid = results.Data.bookid;
            }
            //contributors.ForEach(Console.WriteLine);

            //var classTitle= await _library.GetClass(Convert.ToString(book.ClassId));
            //var subjectTitle= await _library.GetSubjects(Convert.ToString(book.SubjectId));
            //var client = new RestClient("http://202.140.136.39:82/api/MasterData/SearchBook");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("api_key", "6stSN88P7Yh+AQU8+0Dp4VH6v5i2l8xjTiFkE8HzB+j3pGEt+aUFE5Ied/O5U4nCUopJX7V1Om9ur/dQG9P2ObGcggumlKd31fYlp9yMfkGsYP2uo6dMucbNzVycPKuDAndnyPgkaJtS6Jr/K0Av93w/oS0K1MHwQNlYOi6Qi1+lXyhyPNIG4Ki5j9kf9PfrKY9H22E6ClNr5jfpDTKhuAp2tuqdyPSaOlR5jKYKsIY2Ys7ASP6HC+VFFps4/WtBD5/dqfg0th1NU06G8PcKFTHqpzZmH0YXXTBxA0Blyxhuv0dTn4b8Xp+vCtgPtwyQtSU78y/qss25MPF0Dwgmupcj1j5DzXmNkEjCwaCNOGQ=");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddParameter("bookname", book.Title.Trim());
            //request.AddParameter("subjectname", subjectTitle.Title.Trim());
            //request.AddParameter("isbnname", "");
            //request.AddParameter("classname", classTitle.Title.Trim());
            //IRestResponse response = client.Execute(request);
            //var results = JsonConvert.DeserializeObject<Response<SearchBookModel>>(response.Content);
            ////var TestHourbook = "";
            //ViewBag.THBid = 0;
            //if (results.Data != null)
            //{
            //    ViewBag.THBid = results.Data.bookid;
            //}
            return View();
        }

        public async Task<IActionResult> PdfViewer(long bookid)
        {

            var Chapters = await _library.GetChapters();
            List<Chapter> chapterList = Factory.GetChapterList();

            chapterList = Chapters.Where(x => x.BookId == bookid).OrderBy(y => y.Id).ToList();
            chapterList.Insert(0, new Chapter { Id = "0", Title = "Select" });
            ViewBag.Chapters = chapterList;


            var ChapterIds = chapterList.Select(x => Convert.ToInt64(x.Id)).ToList();
            var chapterContents = await _library.GetChapterContents();
            var reuslt = chapterContents.Where(x => (ChapterIds.Contains(x.ChapterId)) && x.BookId == bookid).ToList();
            ViewBag.ChapterContents = reuslt;

            return View();
        }

        public async Task<IActionResult> ViewContent(LibraryVM libraryVM)
        {
            if (libraryVM.type == Enums.BpType.None)
            {
                var result = _library.GetAllLibrary().Result;
                var File = result.Where(x => x.BookId == libraryVM.BookId).FirstOrDefault();
                Common.readerBooks = _ebookReader.GetReaderBooks("Animal_Tracks", null);
                ViewBag.PageList = new SelectList(Common.readerBooks.Pages, "Index", "Index");
                ViewBag.Ebook = _ebookReader.OpenEbook();
                ViewBag.Title = Common.readerBooks.Title;
                ViewBag.UserId = _user.GetAllUser().Result.FirstOrDefault().Id;
                ViewBag.Id = libraryVM.BookId;

                /* READEDGE_30012020A001 Start SiddharthSingh
                * Setting conditions of books
                */
                var role = _httpContextAccessor.HttpContext.Session.GetString("Role") ?? "";

                var book = await _library.GetBooks(libraryVM.BookId);

                if (File.BundleUploaded && book.IsEbook)
                {
                    book.IsEbook = true;
                }
                else
                {
                    book.IsEbook = false;
                }
                if (File.BundleUploaded && book.IsLessonPlan)
                {
                    book.IsLessonPlan = true;
                }
                else
                {
                    book.IsLessonPlan = false;
                }
                if (File.BundleUploaded && book.IsMultiMedia)
                {
                    book.IsMultiMedia = true;
                }
                else
                {
                    book.IsMultiMedia = false;
                }
                if (File.BundleUploaded && book.IsWorkbook)
                {
                    book.IsWorkbook = true;
                }
                else
                {
                    book.IsWorkbook = false;
                }
                if (File.BundleUploaded && book.IsSolutions)
                {
                    book.IsSolutions = true;
                }
                else
                {
                    book.IsSolutions = false;
                }
                // READEDGE_30012020A001 End

                if (role.ToLower() == "user")
                {
                    book.IsLessonPlan = false;
                    //book.IsSolutions = false;
                    //book.IsWorkbook = false;
                    //book.IsMultiMedia = false;
                }
                ViewBag.Book = book;
                libraryVM.startindex = _httpContextAccessor.HttpContext.Session.GetInt32("currentindex") ?? 0;
                libraryVM.endindex = Common.readerBooks.Pages.Count();
                return View("~/Views/Shared/_EBookViewer.cshtml", libraryVM);
            }
            else if (libraryVM.type == Enums.BpType.Solutions || libraryVM.type == Enums.BpType.Workheet || libraryVM.type == Enums.BpType.LessonPlan ||
                libraryVM.type == Enums.BpType.CBSE | libraryVM.type == Enums.BpType.BEP ||
                libraryVM.type == Enums.BpType.MTP || libraryVM.type == Enums.BpType.TPG ||
                libraryVM.type == Enums.BpType.ConceptMap)
            {
                ViewBag.LblChapter = "Chapter";
                ViewBag.LblContent = "Title";
                if (libraryVM.type == Enums.BpType.BEP)
                {
                    ViewBag.LblChapter = "Region";
                    ViewBag.LblContent = "Set";
                }
                return await DocumentData(libraryVM, "~/Views/Home/PdfViewer.cshtml");

            }
            else if (libraryVM.type == Enums.BpType.AudioVideo)
            {
                return await DocumentData(libraryVM, "~/Views/Shared/Multimedia.cshtml");
            }

            else if (libraryVM.type == Enums.BpType.EbookPdf)
            {
                return await DocumentData(libraryVM, "~/Views/Home/EbookPdfViewer .cshtml", true);
            }
            return View();

        }

        private async Task<IActionResult> DocumentData(LibraryVM libraryVM, string ViewName, bool EbookPdf = false)
        {
            try
            {
                //List<Chapter> chapterList = await ChapterList(libraryVM);
                //libraryVM.Chapters = chapterList.Where(x => x.BookId == libraryVM.BookId && x.Status == 1).ToList();

                //List<ChapterContent> reuslt = await ChapterContents(libraryVM, chapterList);
                //libraryVM.ChapterContents = reuslt.ToList();

                //// var Contentchapters = reuslt.Select(x => Convert.ToString(x.ChapterId)).ToList();
                //// ViewBag.Chapters = Chapters.Where(x => Contentchapters.Contains(x.Id)).ToList();

                //var library = await _library.GetAllLibrary();
                //ViewBag.BooName = library.FirstOrDefault(x => x.BookId == libraryVM.BookId).EpubName.Split('.')[0];
                //if (!EbookPdf)
                //    ViewBag.FileName = reuslt.FirstOrDefault().Name;
                //else
                //    ViewBag.FileName = ViewBag.BooName + ".pdf";
                //ViewBag.Type = Convert.ToInt32(libraryVM.type);

                //var result = _library.GetAllLibrary().Result;
                //var File = result.Where(x => x.BookId == libraryVM.BookId).FirstOrDefault();

                //var book = await _library.GetBooks(libraryVM.BookId);




                List<Chapter> chapterList = await ChapterList(libraryVM);
                // ViewBag.Chapters = chapterList.Where(x => x.BookId == libraryVM.BookId && x.Status == 1);
                var chptlist = chapterList.Where(x => x.BookId == libraryVM.BookId && x.Status == 1).ToList();


                //ViewBag.Chapters = new SelectList(chptlist, "Id", "Title");
                libraryVM.Chapters = chptlist;
                List<ChapterContent> reuslt = await ChapterContents(libraryVM, chapterList);

                var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");

                ViewBag.ChapterContents = new SelectList(reuslt, "Name", "Title");
                var Contentchapters = reuslt.Select(x => Convert.ToString(x.ChapterId)).ToList();

                var ddlChapters = chptlist.Where(x => Contentchapters.Contains(x.Id));


                if (string.IsNullOrEmpty(Userid))
                {

                    //ddlChapters = ddlChapters.Take(3).ToList();
                    /*Modified on 28122020 by Rahul*/
                    ddlChapters = ddlChapters.Where(x => x.IsAllowed == true).ToList();
                }
                ViewBag.Chapters = new SelectList(ddlChapters.ToList(), "Id", "Title");
                //ViewBag.ChapterContents = reuslt;
                libraryVM.ChapterContents = reuslt.ToList();
                var library = await _library.GetAllLibrary();
                ViewBag.BooName = library.FirstOrDefault(x => x.BookId == libraryVM.BookId).EpubName.Split('.')[0];
                if (!EbookPdf)
                    ViewBag.FileName = reuslt.FirstOrDefault().Name;
                else
                    ViewBag.FileName = ViewBag.BooName + ".pdf";
                ViewBag.Type = Convert.ToInt32(libraryVM.type);

                var result = _library.GetAllLibrary().Result;
                var File = result.Where(x => x.BookId == libraryVM.BookId).FirstOrDefault();

                var book = await _library.GetBooks(libraryVM.BookId);

                if (File.BundleUploaded && book.IsEbook)
                {
                    book.IsEbook = true;
                }
                else
                {
                    book.IsEbook = false;
                }
                if (File.BundleUploaded && book.IsLessonPlan)
                {
                    book.IsLessonPlan = true;
                }
                else
                {
                    book.IsLessonPlan = false;
                }
                if (File.BundleUploaded && book.IsMultiMedia)
                {
                    book.IsMultiMedia = true;
                }
                else
                {
                    book.IsMultiMedia = false;
                }

                if (File.BundleUploaded && book.IsWorkbook)
                {
                    book.IsWorkbook = true;
                }
                else
                {
                    book.IsWorkbook = false;
                }

                if (File.BundleUploaded && book.IsSolutions)
                {
                    book.IsSolutions = true;
                }
                else
                {
                    book.IsSolutions = false;
                }
                var role = _httpContextAccessor.HttpContext.Session.GetString("Role") ?? "";
                if (role.ToLower() == "user")
                {
                    book.IsLessonPlan = false;
                    //book.IsSolutions = false;
                    //book.IsWorkbook = false;
                    //book.IsMultiMedia = false;
                }
                ViewBag.Book = book;
                return View(ViewName, libraryVM);
            }
            catch (Exception ex)
            {
                return View(ViewName, libraryVM);
            }
        }

        //private async Task<IActionResult> EBookDocumentData(LibraryVM libraryVM, string ViewName)
        //{

        //    var library = await _library.GetAllLibrary();
        //    ViewBag.BooName = library.FirstOrDefault(x => x.BookId == libraryVM.BookId).EpubName.Split('.')[0];
        //    ViewBag.FileName = ViewBag.BooName + ".pdf";
        //    ViewBag.Type = Convert.ToInt32(libraryVM.type);

        //    var book = await _library.GetBooks(libraryVM.BookId);

        //    var File = result.Where(x => x.BookId == libraryVM.BookId).FirstOrDefault();
        //    ViewBag.Book = book;
        //    if (File.BundleUploaded && book.IsEbook)
        //    {
        //        book.IsEbook = true;
        //    }
        //    else
        //    {
        //        book.IsEbook = false;
        //    }
        //    if (File.BundleUploaded && book.IsLessonPlan)
        //    {
        //        book.IsLessonPlan = true;
        //    }
        //    else
        //    {
        //        book.IsLessonPlan = false;
        //    }
        //    if (File.BundleUploaded && book.IsMultiMedia)
        //    {
        //        book.IsMultiMedia = true;
        //    }
        //    else
        //    {
        //        book.IsMultiMedia = false;
        //    }

        //    if (File.BundleUploaded && book.IsWorkbook)
        //    {
        //        book.IsWorkbook = true;
        //    }
        //    else
        //    {
        //        book.IsWorkbook = false;
        //    }

        //    if (File.BundleUploaded && book.IsSolutions)
        //    {
        //        book.IsSolutions = true;
        //    }
        //    else
        //    {
        //        book.IsSolutions = false;
        //    }

        //    return View(ViewName, libraryVM);
        //}

    
        private async Task<List<ChapterContent>> ChapterContents(LibraryVM libraryVM, List<Chapter> chapterList)
        {
            var ChapterIds = chapterList.Select(x => Convert.ToInt64(x.Id)).ToList();
            var chapterContents = await _library.GetChapterContents();
            var type = Convert.ToInt64(libraryVM.type);
            var reuslt = chapterContents.Where(x => ChapterIds.Contains(x.ChapterId) && x.BookId == libraryVM.BookId && x.Type == type && x.Status == 1).ToList();
            return reuslt;
        }

        private async Task<List<Chapter>> ChapterList(LibraryVM libraryVM)
        {
            var Chapters = await _library.GetChapters();
            List<Chapter> chapterList = Factory.GetChapterList();

            chapterList = Chapters.Where(x => x.BookId == libraryVM.BookId).OrderBy(y => y.ChapterIndex).ToList();
            chapterList.Insert(0, new Chapter { Id = "0", Title = "Select" });
            return chapterList;
        }

        public IActionResult Loadpage(int PageId = 1)
        {
            var result = _ebookReader.LoadPage(PageId);
           // result= result.Replace("</html>", "<script src='https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js'></script><script type='text/javascript'>$(document).ready(function (){$(document).ready(function () {$('html, body').animate({scrollTop: $('#a6').offset().top}, 'slow');});});</script></html>");
            return Content(result);
        }
        public IActionResult Loadpagewihthtml(string hmtlpage)
        {
            var result = _ebookReader.LoadPageWithdivNaviagaion(hmtlpage);
            return Content(result);
        }

        public JsonResult IsAudio()
        {
            var isAudio = Convert.ToBoolean(_httpContextAccessor.HttpContext.Session.GetInt32("Audio"));
            return Json(isAudio);
        }
        public async Task<JsonResult> GetContenByChapter(Int32 Chapter, Int32 BookID, Int32 Type)
        {
            var chapterContents = await _library.GetChapterContents();
            var reuslt = chapterContents.Where(x => x.ChapterId == Chapter && x.BookId == BookID && x.Type == Type && x.Status == 1).ToList();
            List<ChapterContent> bookList = Factory.GetChapterContentlList();
            var Userid = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(Userid))
            {
                reuslt = reuslt.Take(3).ToList();
            }
            bookList = reuslt;
            bookList.Insert(0, new ChapterContent { Id = 0, Title = "Select" });

            return Json(bookList);
        }
        public PartialViewResult TestPartial(string pdfFIle = "", string Title = "", bool EbookPdf = false, int BookId = 0, string AllowedPage = "1", string Type = "")
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "books");
            string booklocation = _iConfig.GetValue<string>("AppSettings:BookLocation");
            string CourseBookLocation = _iConfig.GetValue<string>("AppSettings:CourseBookLocation");
            @ViewBag.Type = Type;
            if (EbookPdf)
            {
                //uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "PdfEbooks\\" + pdfFIle);
                var CourseBookPdf = CourseBookLocation + "\\" + pdfFIle;
                //ViewBag.file = Path.Combine(uploadsFolder, pdfFIle);


                //if (((BookId >= 942 && BookId <= 949)))
                //{
                //    CourseBookPdf = CourseBookLocation + "\\" + "TestDoc.pdf";
                //}
                ViewBag.file = @CourseBookPdf;
                //ViewBag.AllowedPages = AllowedPage;
            }
            else
            {
                //ViewBag.file = Path.Combine(uploadsFolder, Title + "\\" + pdfFIle);
                var ContentLocation = booklocation + "\\" + Title + "\\" + pdfFIle;
                ViewBag.file = @ContentLocation;
                // ViewBag.AllowedPages = AllowedPage;
            }
            ViewBag.AllowedPages = AllowedPage;
            ViewBag.EbookPdf = EbookPdf;
            ViewBag.BookId = BookId;
            // ViewBag.file = Path.Combine(uploadsFolder, Title + "\\" + pdfFIle);
            return PartialView("~/Views/Shared/_PdfViewer.cshtml");
        }

        public PartialViewResult VideoPartial(string pdfFIle = "", string Title = "")
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "books");
            //ViewBag.file = Path.Combine(uploadsFolder, Title + "\\" + pdfFIle);
            ViewBag.file = "/Books/" + Title + "/" + pdfFIle;
            return PartialView("~/Views/Shared/_Multimedia.cshtml");
        }

        #region BookMarks
        [HttpPost]
        public JsonResult AddBookMarks(UserBookMark userBookMark)
        {
            userBookMark.CreatedDate = DateTime.UtcNow.ToLocalTime();
            _library.AddBookMarks(userBookMark);
            return Json(true);
        }

        public JsonResult RemveBookMark(long BookMarkId)
        {
            _library.DeleteBookMarks(BookMarkId);
            return Json(true);
        }
        public JsonResult BookMarkValidity(UserBookMark userBookMark)
        {
            userBookMark.ContentType = DAL.Models.Enum.ContentTypes.Epub;
            var validity = _library.ValidBookMark(userBookMark);
            return Json(validity);
        }

        public JsonResult GetBookMarksByUserAndContentId(UserBookMark userBookMark)
        {
            userBookMark.UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var result = _library.GetBookMarksByUserAndContentId(userBookMark).Result.Select(x => new
            {
                Id = x.Id.ToString(),
                ContentId = x.ContentId.ToString(),
                UserId = x.UserId,
                PageNumber = x.PageNumber.ToString(),
                CreatedDate = x.CreatedDate.ToString()
            }).OrderBy(y => y.PageNumber).Where(x => x.UserId == userBookMark.UserId).ToList();
            return Json(result);
        }

        #endregion
        #region Notes
        [HttpPost]
        public JsonResult AddNotes(LibraryVM libraryVM)
        {
            UserNote userNote = Factory.GetUserNote();
            userNote.CreatedDate = DateTime.UtcNow.ToLocalTime();
            userNote.IsUserContent = libraryVM.IsUserContent;
            userNote.PageNumber = libraryVM.PageNumber;
            userNote.UserId = libraryVM.UserId;
            userNote.ContentType = libraryVM.ContentType;
            userNote.ContentId = libraryVM.ContentId;
            userNote.Description = libraryVM.Description;
            if (libraryVM.NoteFile != null)
            {
                userNote.FileName = libraryVM.NoteFile.FileName;
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Notes");
                string subPath = Path.Combine(uploadsFolder, userNote.UserId + "/");
                bool exists = System.IO.Directory.Exists(subPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(subPath);
                var filepath = subPath + libraryVM.NoteFile.FileName;
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                libraryVM.NoteFile.CopyToAsync(new FileStream(filepath, FileMode.Create));
            }
            _library.AddNotes(userNote);
            return Json(true);
        }
        public JsonResult RemoveNotes(long NoteId)
        {
            _library.RemoveNotes(NoteId);
            return Json(true);
        }
        public JsonResult GetNotesByUserAndContentId(UserNote userNote)
        {
            userNote.UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            var result = _library.GetNotesByUserAndContentId(userNote).Result.Select(x => new
            {
                Id = x.Id.ToString(),
                ContentId = x.ContentId.ToString(),
                UserId = x.UserId,
                PageNumber = x.PageNumber.ToString(),
                CreatedDate = x.CreatedDate.ToString(),
                Description = x.Description
            }).OrderBy(y => y.PageNumber).Where(x => x.UserId == userNote.UserId).ToList();
            return Json(result);
        }
        #endregion

        public JsonResult GetOTP(string ContactNo)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetString("ContactNo", ContactNo);
                var OTP = Common.GenerateRandomNo().ToString();
                //var Msg = "Your OTP for Readedge trial is " + OTP;
                var Msg = "Your%20OTP%20for%20Readedge%20trial%20is%20"+OTP+"%20Prachi%20India%20Pvt%20Ltd";
                          //"Your%20OTP%20for%20Readedge%20trial%20is%20"+OTP+"%20Prachi%20India%20Pvt%20Ltd"
                _httpContextAccessor.HttpContext.Session.SetString("OTP", OTP);
                _httpContextAccessor.HttpContext.Session.SetString("IsVerified", "NO");
                var MaskedNumber = ContactNo.Mask(2, 5, '*');
                Common.SendSMS(ContactNo, Msg);
                return Json(MaskedNumber);
            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }

        public async Task<JsonResult> VerifyOTP(string ContactNo, string Name, string Email, string OTP)
        {
            try
            {
                var otp = _httpContextAccessor.HttpContext.Session.GetString("OTP");
                if (otp != OTP)
                {
                    return Json("Invalid");
                }
                else
                {

                    var trialUsers = await _library.GetReadEdgeTrialUser();
                    var isUserAvailable = trialUsers.Any(x => x.ContactNo == ContactNo);
                    if (!isUserAvailable)
                    {
                        ReadEdgeTrialUsers readEdgeTrialUsers = new ReadEdgeTrialUsers();
                        readEdgeTrialUsers.ContactNo = ContactNo;
                        readEdgeTrialUsers.Name = Name;
                        readEdgeTrialUsers.Email = Email;
                        await _library.AddReadEdgeTrialUser(readEdgeTrialUsers);

                    }
                    _httpContextAccessor.HttpContext.Session.SetString("IsVerified", "YES");
                    return Json("Success");
                }

            }
            catch (Exception ex)
            {
                return Json("Failed");
            }
        }

        public void  LoginTestHour(string BookId)
        {
            var UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName");
            var Password = _httpContextAccessor.HttpContext.Session.GetString("Password");
            var FirstName = _httpContextAccessor.HttpContext.Session.GetString("FirstName");
            var LastName = _httpContextAccessor.HttpContext.Session.GetString("LastName");
            var Email = _httpContextAccessor.HttpContext.Session.GetString("Email");
            var PhoneNumber = _httpContextAccessor.HttpContext.Session.GetString("PhoneNumber");
            var strPostString = new StringBuilder();

            strPostString.Append("<html><head>");
            strPostString.Append("</head><body onload=\"document.form1.submit();\">");
            strPostString.Append("<form name=\"form1\" method=\"post\" action=\"" + Url + "\">");

                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "UserName", UserName));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "Password", Password));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "MobNo", PhoneNumber));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "Email", Email));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "FirstName", FirstName));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "LastName", LastName));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "MasterBookId", BookId));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "Gender", "0"));
                strPostString.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", "Api_Key", "6stSN88P7Yh+AQU8+0Dp4VH6v5i2l8xjTiFkE8HzB+j3pGEt+aUFE5Ied/O5U4nCUopJX7V1Om9ur/dQG9P2ObGcggumlKd31fYlp9yMfkGsYP2uo6dMucbNzVycPKuDAndnyPgkaJtS6Jr/K0Av93w/oS0K1MHwQNlYOi6Qi1+lXyhyPNIG4Ki5j9kf9PfrKY9H22E6ClNr5jfpDTKhuAp2tuqdyPSaOlR5jKYKsIY2Ys7ASP6HC+VFFps4/WtBD5/dqfg0th1NU06G8PcKFTHqpzZmH0YXXTBxA0Blyxhuv0dTn4b8Xp+vCtgPtwyQtSU78y/qss25MPF0Dwgmupcj1j5DzXmNkEjCwaCNOGQ="));

            strPostString.Append("</form>");
            strPostString.Append("</body></html>");

            Response.Clear();
            Response.WriteAsync(strPostString.ToString());

        }
    }


    public static class Extensions
    {

        public static string Mask(this string source, int start, int maskLength)
        {
            return source.Mask(start, maskLength, 'X');
        }

        public static string Mask(this string source, int start, int maskLength, char maskCharacter)
        {
            if (start > source.Length - 1)
            {
                throw new ArgumentException("Start position is greater than string length");
            }

            if (maskLength > source.Length)
            {
                throw new ArgumentException("Mask length is greater than string length");
            }

            if (start + maskLength > source.Length)
            {
                throw new ArgumentException("Start position and mask length imply more characters than are present");
            }

            string mask = new string(maskCharacter, maskLength);
            string unMaskStart = source.Substring(0, start);
            string unMaskEnd = source.Substring(start + maskLength, source.Length - (start + maskLength));

            return unMaskStart + mask + unMaskEnd;
        }


        public static string ToStringMask(this int source, int start, int maskLength)
        {
            return source.ToString().Mask(start, maskLength, 'X');
        }

        public static string ToStringMask(this int source, int start, int maskLength, char maskCharacter)
        {
            return source.ToString().Mask(start, maskLength, maskCharacter);
        }

    }


}
