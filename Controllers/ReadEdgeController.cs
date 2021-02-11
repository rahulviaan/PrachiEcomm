using Microsoft.AspNet.Identity;
using PrachiIndia.Portal.Helpers;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PrachiIndia.Portal.Controllers
{
    [Authorize]
    [RoutePrefix("api/ReadEdge")]
    public class ReadEdgeController : ApiController
    {
        public int MaxCount = 3;
        [HttpGet]
        [Route("GetUser")]
        public Models.ApplicationUser GetUser()
        {
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();
            var AspUserRepo = new AspNetUserRepository();
            var userId = User.Identity.GetUserId();
            var result = AspUserRepo.GetByIdAsync(userId);
            //var Country = CountryRepo.FindByItemId(Utility.ToSafeInt(result.Country)) != null ? CountryRepo.FindByItemId(Utility.ToSafeInt(result.Country)).Name : "India";
            //var State = StateRepo.FindByItemId(Utility.ToSafeInt(result.State)) != null ? StateRepo.FindByItemId(Utility.ToSafeInt(result.State)).StateName : "";
            //var City = CityRepo.FindByItemId(Utility.ToSafeInt(result.City)) != null ? CityRepo.FindByItemId(Utility.ToSafeInt(result.City)).CityName : "";
            var aspUser = new Models.ApplicationUser
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                CompleteName = string.Format("{0} {1}", result.FirstName, result.LastName),
                dtmAdd = result.dtmAdd,
                dtmUpdate = result.dtmUpdate,
                dtmDelete = result.dtmDelete,
                Status = Utility.ToSafeInt(result.Status),
                dtmDob = result.dtmDob,
                AboutMe = result.AboutMe,
                ProfileImage = result.ProfileImage,
                City = result.City,
                State = result.State,
                Country = result.Country,
                CountryId = Utility.ToSafeInt(result.CountryId),
                StateId = Utility.ToSafeInt(result.StateId),
                CityId = Utility.ToSafeInt(result.CityId),
                PinCode = result.PinCode,
                Address = result.Address,
                Profession = result.Profession,
                Designation = result.Designation,
                Organization = result.Organization,
                Industry = result.Industry,
                PANId = result.PANId,
                PassportNo = result.PassportNo,
                Remark = result.Remark,
                Email = result.Email,
                SecurityStamp = result.SecurityStamp,
                PhoneNumber = result.PhoneNumber,
                UserName = result.UserName,

            };
            return aspUser;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetBoards")]
        public IEnumerable<BoardModel> GetBoards()
        {
            var items = (from item in new MasterBoardRepository().GetAll().Where(t => t.Status == (int)Status.Active).OrderBy(t => t.Id).ToList()
                         select new BoardModel
                         {
                             Description = item.Description,
                             Id = item.Id,
                             Title = item.Title,
                             Status = item.Status == (int)Status.Active ? true : false,
                             Image = item.Image,
                             OredrNo = item.OredrNo ?? 0
                         }).AsEnumerable();
            return items;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetClass")]
        public IEnumerable<ClassModel> GetClass()
        {
            var items = (from item in new MasterClassRepository().GetAll().Where(t => t.Status == (int)Status.Active).OrderBy(t => t.Id).ToList()
                         select new ClassModel
                         {
                             Description = item.Description,
                             Id = item.Id,
                             Title = item.Title,
                             Status = item.Status == (int)Status.Active ? true : false,
                             Image = item.Image,
                             OredrNo = item.OredrNo ?? 0
                         }).AsEnumerable();
            return items;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSubjectClass")]
        public IEnumerable<SubjectClass> GetSubjectClass()
        {
            var items = (from item in new dbPrachiIndia_PortalEntities().SubjectClasses.ToList()
                         select new SubjectClass
                         {
                             ClassId = item.ClassId,
                             Id = item.Id,
                             SubjectId = item.SubjectId
                         }).AsEnumerable();
            return items;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSubjects")]
        public IEnumerable<SubjectModel> GetSubjects()
        {
            var items = (from item in new MasterSubjectRepository().GetAll().Where(t => t.Status == (int)Status.Active).OrderBy(t => t.Id).ToList()
                         select new SubjectModel
                         {
                             Description = item.Description,
                             Id = item.Id,
                             Title = item.Title,
                             Status = item.Status == (int)Status.Active ? true : false,
                             Image = item.Image,
                             OredrNo = item.OredrNo ?? 0,
                         }).AsEnumerable();
            return items;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSeries")]
        public IEnumerable<SeriesModel> GetSeries()
        {
            var items = (from item in new MasterSeriesRepositories().GetAll().Where(t => t.Status == (int)Status.Active).OrderBy(t => t.Id).ToList()
                         select new SeriesModel
                         {
                             Description = item.Description,
                             Id = item.Id,
                             Title = item.Title,
                             Status = item.Status == (int)Status.Active ? true : false,
                             Image = item.Image,
                             OredrNo = item.OredrNo ?? 0,
                             SubjectId = item.SubjectId ?? 0
                         }).AsEnumerable();
            return items;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetBooks")]
        public IEnumerable<BookModel> GetBooks()
        {
            var sitePath = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            var books = new CatalogRepository().Search(t => t.Status == (int)Status.Active && t.Ebook == 1).OrderBy(t => t.Id).ToList();
            var items = (from item in books.Where(t => !string.IsNullOrWhiteSpace(t.Ebookname) && !string.IsNullOrWhiteSpace(t.EncriptionKey))
                         select new BookModel
                         {
                             Description = item.Description,
                             Id = item.Id,
                             Title = item.Title,
                             Status = item.Status == (int)Status.Active ? true : false,
                             OrderNo = item.OrderNo ?? 0,
                             SubjectId = item.SubjectId ?? 0,
                             Author = item.Author,
                             BoardId = Utility.ToSafeInt(item.BoardId),
                             ClassId = Utility.ToSafeInt(item.ClassId),
                             CreatedDate = item.dtmAdd ?? DateTime.Now,
                             Discount = item.Discount ?? 0,
                             Edition = item.Edition,
                             //EncriptionKey = item.EncriptionKey,
                             EpubPath = string.Format("{0}ModuleFiles/Epub", sitePath),
                             ImageName = item.Image,
                             ImagePath = string.Format("{0}ModuleFiles/Items", sitePath),
                             ISBN = item.ISBN,
                             IsEbook = item.Ebook == 1 ? true : false,
                             IsMultiMedia = item.Audio ?? false,
                             IsSolutions = item.Solutions == 1 ? true : false,
                             IsVideo = item.MultiMedia == 1 ? true : false,
                             IsWorkbook = item.Worksheet ?? false,
                             Price = item.Price ?? 0,
                             SeriesId = item.SeriesId ?? 0,
                             Size = item.isSize,
                             IsLessonPlan = item.LessonPlan ?? false,
                             UpdatedDate = item.dtmUpdate ?? DateTime.Now
                         }).AsEnumerable();
            return items;

        }
        #region Reader Service
        [HttpPost]
        [Route("PostReader")]
        //Story: First check Reader against machinekey if exist return shame reader
        //if  not exist check readercount <5 creaet new reader and return new reader.
        public Models.UserReader PostReader([FromBody]Sql.UserReader reader)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reader.MachineKey))
                {
                    var reader1 = new Models.UserReader
                    {
                        ErrorMessage = "Machine key not fetched.",
                        Result = Models.ResponseStatus.Failed.ToString(),
                        TimeStamp = DateTime.UtcNow,
                        ReaderStatus = Models.ReaderStatus.Error,
                    };
                    return reader1;
                }
                var userId = User.Identity.GetUserId();
                var readerRepository = new ReaderRepository();
                var item = new Models.UserReader();
                var userReader = readerRepository.SearchFor(t => t.MachineKey.ToLower() == reader.MachineKey.ToLower() && t.UserId == userId).FirstOrDefault();
                if (userReader != null)
                {
                    item.Id = userReader.Id;
                    item.UserId = userReader.UserId;
                    item.Status = userReader.Status;
                    item.CreatedDate = userReader.CreatedDate;
                    item.UpdatedDate = userReader.UpdatedDate;
                    item.ReaderKey = userReader.ReaderKey;
                    item.DeviceType = (Models.DeviceType)Common_Static.ToSafeInt(userReader.DeviceType.ToString());
                    item.ReaderStatus = Models.ReaderStatus.Old;
                    item.MasterVesrion = Common_Static.ToSafeInt(userReader.MasterVesrion);
                    item.LibraryVersion = Common_Static.ToSafeInt(userReader.LibraryVersion);
                    item.BookVersion = Common_Static.ToSafeInt(userReader.BookVersion);
                    item.SynchDate = (DateTime)userReader.SynchDate;
                    return item;
                }
                var readerCount = readerRepository.Search(t => t.UserId == userId).Count();
                var user = new AspNetUserRepository().GetByIdAsync(userId);
                if (user != null)
                {
                    MaxCount = user.DeviceCount ?? 1;
                }

                if (readerCount > MaxCount)
                {
                    item = new Models.UserReader
                    {
                        ErrorMessage = "The user limit has been exceeded.",
                        Result = Models.ResponseStatus.Failed.ToString(),
                        TimeStamp = DateTime.UtcNow,
                        ReaderStatus = Models.ReaderStatus.Error,
                        UserId = userId
                    };
                    return item;
                }
                else
                {
                    reader.Id = Guid.NewGuid().ToString();
                    reader.UserId = userId;
                    reader.Status = 1;
                    reader.CreatedDate = DateTime.UtcNow;
                    reader.UpdatedDate = DateTime.UtcNow;
                    reader.ReaderKey = Guid.NewGuid().ToString();
                    userReader = readerRepository.CreateAsync(reader);

                    item = new Models.UserReader
                    {
                        CreatedDate = userReader.CreatedDate,
                        Description = userReader.Description,
                        ErrorMessage = "",
                        Id = userReader.Id,
                        DeviceType = userReader.DeviceType == null ? Models.DeviceType.Window : (Models.DeviceType)userReader.DeviceType,
                        ReaderKey = userReader.ReaderKey,
                        Result = Models.ResponseStatus.Succeeded.ToString(),
                        Status = userReader.Status,
                        TimeStamp = DateTime.UtcNow,
                        UpdatedDate = userReader.UpdatedDate,
                        UserId = userReader.UserId,
                        MachineKey = userReader.MachineKey,
                        ReaderStatus = Models.ReaderStatus.New,
                        MasterVesrion = Common_Static.ToSafeInt(userReader.MasterVesrion),
                        LibraryVersion = Common_Static.ToSafeInt(userReader.LibraryVersion),
                        BookVersion = Common_Static.ToSafeInt(userReader.BookVersion),
                        SynchDate = (DateTime)userReader.SynchDate,
                    };
                    return item;
                }
            }
            catch (Exception ex)
            {

                var item = new Models.UserReader
                {
                    ErrorMessage = ex.Message,
                    Result = Models.ResponseStatus.Failed.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    ReaderStatus = Models.ReaderStatus.Error,
                };
                return item;
            }
        }
        [HttpGet]
        [Route("GetLibraries")]
        public List<UserLibraryModel> GetLibraries()
        {
            var readerBookRepository = new ReaderBookRepository();
            var moduleFilePath = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            var imagePath = string.Format("{0}ModuleFiles/Items/", moduleFilePath);
            var epubPath = string.Format("{0}ModuleFiles/Epub/", moduleFilePath);
            var userId = User.Identity.GetUserId();
            var items = readerBookRepository.SearchFor(t => t.UserId == userId).ToList();
            var results = new List<UserLibraryModel>();
            if (items == null || !items.Any())
                return results;
            foreach (var item in items)
            {
                var reader = new UserLibraryModel
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    UserId = item.UserId,
                    EPubPath = epubPath,
                    EncriptionKey = item.EncriptionKey,
                    EpubName = item.EPubName,
                    ErrorMessage = "",
                    Result = Models.ResponseStatus.Succeeded.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    Chapters = GetChapters(item.BookId)

                };
                results.Add(reader);
            }
            return results;
        }
        List<ChapterModel> GetChapters(long bookId)
        {
            var chapterRepository = new ChapterRepository();
            return (from item in chapterRepository.Search(t => t.BookId == bookId).OrderBy(t => t.ChapterIndex).ToList()
                    select new ChapterModel
                    {
                        BookId = item.BookId ?? 0,
                        ChapterIndex = item.ChapterIndex ?? 0,
                        Description = item.Description,
                        FromPage = item.FromPage ?? 0,
                        Id = item.Id,
                        Status = item.Status ?? 0,
                        Title = item.Title,
                        ToPage = item.ToPage ?? 0,
                        ChapterContents = GetChapterContents(item.Id)
                    }).ToList();

        }
        public List<ChapterContentModel> GetChapterContents(long chapterId)
        {
            return (from item in new ChapterContentRepository().Search(t => t.ChapterId == chapterId).ToList()
                    select new ChapterContentModel
                    {
                        BookId = item.BookId ?? 0,
                        ChapterId = item.ChapterId ?? 0,
                        ContentType = item.ContentType ?? 0,
                        Desc = item.Desc,
                        Id = item.Id,
                        Name = item.Name,
                        OrderId = item.OrderId ?? 0,
                        Status = item.Status ?? 0,
                        Title = item.Title,
                        Type = item.Type ?? 0
                    }).ToList();
        }
        [HttpGet]
        [Route("MyLibrary")]
        public List<UserLibraryModel> MyLibrary()
        {
            var readerBookRepository = new ReaderBookRepository();
            var moduleFilePath = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            var imagePath = string.Format("{0}ModuleFiles/Items/", moduleFilePath);
            var epubPath = string.Format("{0}ModuleFiles/Epub/", moduleFilePath);
            var userId = User.Identity.GetUserId();
            var items = readerBookRepository.SearchFor(t => t.UserId == userId).ToList();
            var results = new List<UserLibraryModel>();
            if (items == null || !items.Any())
                return results;
            foreach (var item in items)
            {
                var reader = new UserLibraryModel
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    UserId = item.UserId,
                    EPubPath = epubPath,
                    EncriptionKey = item.EncriptionKey,
                    EpubName = item.EPubName,
                    ErrorMessage = "",
                    Result = Models.ResponseStatus.Succeeded.ToString(),
                    TimeStamp = DateTime.UtcNow,
                };
                results.Add(reader);
            }
            return results;
        }
        [HttpPost]
        [Route("GetChapters")]
        public List<ChapterModel> GetChapters(List<long> bookIds)
        {
            var chapterRepository = new ChapterRepository();
            return (from item in chapterRepository.Search(t => bookIds.Contains(t.BookId ?? 0)).OrderBy(t => t.ChapterIndex).ToList()
                    select new ChapterModel
                    {
                        BookId = item.BookId ?? 0,
                        ChapterIndex = item.ChapterIndex ?? 0,
                        Description = item.Description,
                        FromPage = item.FromPage ?? 0,
                        Id = item.Id,
                        Status = item.Status ?? 0,
                        Title = item.Title,
                        ToPage = item.ToPage ?? 0,
                        // ChapterContents = GetChapterContents(item.Id)
                    }).ToList();
        }
        [HttpPost]
        [Route("GetContents")]
        public List<ChapterContentModel> GetContents(List<long> bookIds)
        {

            return (from item in new ChapterContentRepository().Search(t => bookIds.Contains(t.BookId ?? 0)).OrderBy(t => t.OrderId).ToList()
                    select new ChapterContentModel
                    {
                        BookId = item.BookId ?? 0,
                        ChapterId = item.ChapterId ?? 0,
                        ContentType = item.ContentType ?? 0,
                        Desc = item.Desc,
                        Id = item.Id,
                        Name = item.Name,
                        OrderId = item.OrderId ?? 0,
                        Status = item.Status ?? 0,
                        Title = item.Title,
                        Type = item.Type ?? 0
                    }).ToList();
        }
       
        #endregion

    }
}

