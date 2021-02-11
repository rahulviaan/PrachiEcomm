using Microsoft.AspNet.Identity;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Sql;
using PrachiIndia.Sql.CustomRepositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PrachiIndia.Web.Areas.Model;
using System.Globalization;
using PrachiIndia.Portal.Helpers;

namespace PrachiIndia.Portal.Controllers
{
    [Authorize]
    public class ReadersController : ApiController
    {
        public int MaxCount = 5;
        #region "User Details"  

        [HttpGet]
        [Route("api/GetUser/{idUser}")]
        public async Task<Models.ApplicationUser> GetUser(string idUser)
        {
            var AspUser = new Models.ApplicationUser();
            var CountryRepo = new CountryRepositories();
            var StateRepo = new StateRepositories();
            var CityRepo = new CityRepositories();

            var AspUserRepo = new AspNetUserRepository();
            var result = AspUserRepo.GetByIdAsync(idUser);

            var Country = CountryRepo.FindByItemId(Utility.ToSafeInt(result.Country)) != null ? CountryRepo.FindByItemId(Utility.ToSafeInt(result.Country)).Name : "India";
            var State = StateRepo.FindByItemId(Utility.ToSafeInt(result.State)) != null ? StateRepo.FindByItemId(Utility.ToSafeInt(result.State)).StateName : "";
            var City = CityRepo.FindByItemId(Utility.ToSafeInt(result.City)) != null ? CityRepo.FindByItemId(Utility.ToSafeInt(result.City)).CityName : "";

            AspUser = new Models.ApplicationUser()
            {
                Id = result.Id,
                FirstName = result.Id,
                MiddleName = result.Id,
                LastName = result.Id,
                CompleteName = result.Id,
                dtmAdd = result.dtmAdd,
                dtmUpdate = result.dtmUpdate,
                dtmDelete = result.dtmDelete,
                Status = Utility.ToSafeInt(result.Status),
                dtmDob = result.dtmDob,
                AboutMe = result.AboutMe,
                ProfileImage = result.ProfileImage,
                City = City,
                State = State,
                Country = Country,
                CountryId = Utility.ToSafeInt(result.Country),
                StateId = Utility.ToSafeInt(result.State),
                CityId = Utility.ToSafeInt(result.City),
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
                PasswordHash = result.PasswordHash,
                SecurityStamp = result.SecurityStamp,
                PhoneNumber = result.PhoneNumber,
                UserName = result.UserName,

            };
            return AspUser;
        }


        [HttpGet]
        [Route("api/ForgetPssword/{EmailID}")]
        public async Task<Models.ApplicationUser> ForgetPssword(string Email)
        {
            var AspUser = new Models.ApplicationUser();
            var AspUserRepo = new AspNetUserRepository();
            var result = AspUserRepo.GetByEmailAsync(Email);
            AspUser = new Models.ApplicationUser()
            {
                PasswordHash = result.PasswordHash,
                UserName = result.UserName,
            };
            return AspUser;
        }
        #endregion

        [Route("api/GetMasterData/{act}")]
        public Models.Master GetMasterData(MasterSynch act = MasterSynch.All)
        {
            Models.Master master = new Models.Master();
            var lstBook = new List<Models.mst_Book>();
            var lstBoard = new List<Models.mst_Board>();
            var lstClass = new List<Models.mst_Class>();
            var lstSubject = new List<Models.mst_Subject>();
            var lstSeries = new List<Models.mst_Series>();

            var readerRepository = new CatalogRepository();
            var classRepository = new MasterClassRepository();
            var boardRepository = new MasterBoardRepository();
            var seriesRepository = new MasterSeriesRepositories();
            var subjectRepository = new MasterSubjectRepository();

            if (act == MasterSynch.Class)
            {

                var listClass = classRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item in listClass)
                {
                    var objClass = new Models.mst_Class
                    {
                        Description = item.Description,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = item.Image,
                        IpAddress = item.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item.OredrNo),
                        Status = (int)item.Status,
                        Title = item.Title,
                        idServer = Common_Static.ToSafeInt(item.idServer)
                    };
                    lstClass.Add(objClass);

                }

            }

            else if (act == MasterSynch.Board)
            {
                var listBoard = boardRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item in listBoard)
                {
                    var objBoard = new Models.mst_Board
                    {
                        Description = item.Description,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = item.Image,
                        IpAddress = item.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item.OredrNo),
                        Status = (int)item.Status,
                        Title = item.Title,
                        idServer = Common_Static.ToSafeInt(item.idServer)

                    };
                    lstBoard.Add(objBoard);
                }
            }
            else if (act == MasterSynch.Subject)
            {
                var listSubject = subjectRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item in listSubject)
                {
                    var objSubject = new Models.mst_Subject
                    {
                        Description = item.Description,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = item.Image,
                        IpAddress = item.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item.OredrNo),
                        Status = (int)item.Status,
                        Title = item.Title,
                        idServer = Common_Static.ToSafeInt(item.idServer)
                    };
                    lstSubject.Add(objSubject);
                }
            }
            else if (act == MasterSynch.Series)
            {
                var listseries = seriesRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item1 in listseries)
                {
                    var objSeries = new Models.mst_Series
                    {
                        Description = item1.Description,
                        dtmAdd = item1.dtmAdd,
                        dtmDelete = item1.dtmDelete,
                        dtmUpdate = item1.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item1.Id),
                        Image = item1.Image,
                        IpAddress = item1.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item1.OredrNo),
                        Status = (int)item1.Status,
                        Title = item1.Title,
                        SubjectId = Common_Static.ToSafeInt(item1.SubjectId),
                        UserId = item1.UserId,
                        idServer = Common_Static.ToSafeInt(item1.idServer)
                    };
                    lstSeries.Add(objSeries);
                }
            }
            else if (act == MasterSynch.Book)
            {
                var moduleFilePath = ConfigurationManager.AppSettings["ModuleFiles"].ToString();
                var imagePath = string.Format("{0}Items/", moduleFilePath);
                var listBook = readerRepository.Search(t => t.Ebook == BookType.EBook.GetHashCode() &&
                (t.Status == (int)Status.Active) && (t.idServer > 0) &&
                !(string.IsNullOrEmpty(t.isSize) && !t.isSize.Contains("N/A"))).ToList();

                foreach (var item in listBook)
                {
                    var objBook = new Models.mst_Book
                    {
                        Author = item.Author,
                        BoardId = item.BoardId,
                        ClassId = item.ClassId,
                        Description = item.Description,
                        Discount = item.Discount,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Ebook = Common_Static.ToSafeInt(item.Ebook),
                        Edition = item.Edition,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = string.IsNullOrWhiteSpace(item.Image) ? "" : imagePath + item.Image,
                        IpAddress = item.IpAddress,
                        ISBN = item.ISBN,
                        MultiMedia = Common_Static.ToSafeInt(item.MultiMedia),
                        OrderNo = Common_Static.ToSafeInt(item.OrderNo),
                        Price = item.Price,
                        SeriesId = Common_Static.ToSafeInt(item.SeriesId),
                        Solutions = Common_Static.ToSafeInt(item.Solutions),
                        SubjectId = Common_Static.ToSafeInt(item.SubjectId),
                        Status = (int)item.Status,
                        Title = item.Title,
                        isSize = item.isSize,
                        idServer = Common_Static.ToSafeInt(item.idServer)
                    };
                    lstBook.Add(objBook);
                }
            }
            else if (act == MasterSynch.All)
            {
                #region "Board"
                var listBoard = boardRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item in listBoard)
                {
                    var objBoard = new Models.mst_Board
                    {
                        Description = item.Description,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = item.Image,
                        IpAddress = item.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item.OredrNo),
                        Status = (int)item.Status,
                        Title = item.Title,
                        idServer = Common_Static.ToSafeInt(item.idServer)

                    };
                    lstBoard.Add(objBoard);
                }
                #endregion
                #region "Class"
                var listClass = classRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item in listClass)
                {
                    var objClass = new Models.mst_Class
                    {
                        Description = item.Description,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = item.Image,
                        IpAddress = item.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item.OredrNo),
                        Status = (int)item.Status,
                        Title = item.Title,
                        idServer = Common_Static.ToSafeInt(item.idServer)
                    };
                    lstClass.Add(objClass);

                }
                #endregion
                #region "Book"
                var moduleFilePath = ConfigurationManager.AppSettings["ModuleFiles"].ToString();
                var imagePath = string.Format("{0}Items/", moduleFilePath);
                var listBook = readerRepository.Search(t => t.Ebook == (int)BookType.EBook &&
                (t.Status == (int)Status.Active) && (t.idServer > 0) &&
                !(string.IsNullOrEmpty(t.isSize) && !t.isSize.Contains("N/A"))).ToList();

                foreach (var item in listBook)
                {
                    var objBook = new Models.mst_Book
                    {
                        Author = item.Author,
                        BoardId = item.BoardId,
                        ClassId = item.ClassId,
                        Description = item.Description,
                        Discount = item.Discount,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Ebook = Common_Static.ToSafeInt(item.Ebook),
                        Edition = item.Edition,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = string.IsNullOrWhiteSpace(item.Image) ? "" : imagePath + item.Image,
                        IpAddress = item.IpAddress,
                        ISBN = item.ISBN,
                        MultiMedia = Common_Static.ToSafeInt(item.MultiMedia),
                        OrderNo = Common_Static.ToSafeInt(item.OrderNo),
                        Price = item.Price,
                        SeriesId = Common_Static.ToSafeInt(item.SeriesId),
                        Solutions = Common_Static.ToSafeInt(item.Solutions),
                        SubjectId = Common_Static.ToSafeInt(item.SubjectId),
                        Status = (int)item.Status,
                        Title = item.Title,
                        isSize = item.isSize,
                        idServer = Common_Static.ToSafeInt(item.idServer)
                    };
                    lstBook.Add(objBook);
                }
                #endregion

                #region "Subject"
                var listSubject = subjectRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item in listSubject)
                {
                    var objSubject = new Models.mst_Subject
                    {
                        Description = item.Description,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item.Id),
                        Image = item.Image,
                        IpAddress = item.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item.OredrNo),
                        Status = (int)item.Status,
                        Title = item.Title,
                        idServer = Common_Static.ToSafeInt(item.idServer)
                    };
                    lstSubject.Add(objSubject);
                }
                #endregion

                #region "Series"

                var listseries = seriesRepository.Search(t => t.Status == (int)Status.Active && t.idServer > 0);
                foreach (var item1 in listseries)
                {
                    var objSeries = new Models.mst_Series
                    {
                        Description = item1.Description,
                        dtmAdd = item1.dtmAdd,
                        dtmDelete = item1.dtmDelete,
                        dtmUpdate = item1.dtmUpdate,
                        Id = Common_Static.ToSafeInt(item1.Id),
                        Image = item1.Image,
                        IpAddress = item1.IpAddress,
                        OredrNo = Common_Static.ToSafeInt(item1.OredrNo),
                        Status = (int)item1.Status,
                        Title = item1.Title,
                        idServer = Common_Static.ToSafeInt(item1.idServer)
                    };
                    lstSeries.Add(objSeries);
                }
                #endregion              
            }
            master.mstBook = lstBook;
            master.mstBoard = lstBoard;
            master.mstClass = lstClass;
            master.mstSubject = lstSubject;
            master.mstSeries = lstSeries;
            return master;
        }

        #region "Used By Apps"
        [HttpPost]
        [Route("api/PostReader")]
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
        [Route("api/GetBoards")]
        public IEnumerable<MasterBoard> GetBoards()
        {
            try
            {
                return (from item in new MasterBoardRepository().GetAll().ToList().Where(t => t.Status == Status.Active.GetHashCode())
                        select new MasterBoard
                        {
                            Description = Utility.ToSafeString(item.Description),
                            dtmAdd = Utility.ToSafeDate(item.dtmAdd),
                            dtmDelete = Utility.ToSafeDate(item.dtmDelete),
                            dtmUpdate = Utility.ToSafeDate(item.dtmUpdate),
                            Id = Utility.ToSafeInt(item.Id),
                            Image = Utility.ToSafeString(item.Image),
                            IpAddress = Utility.ToSafeString(item.IpAddress),
                            OredrNo = Utility.ToSafeInt(item.OredrNo),
                            Status = Utility.ToSafeInt(item.Status),
                            Title = Utility.ToSafeString(item.Title),
                            idServer = Utility.ToSafeInt(item.idServer),
                        }).AsEnumerable();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("api/GetClasses")]
        public IEnumerable<MasterClass> GetClasses()
        {
            try
            {
                return (from item in new MasterClassRepository().GetAll().ToList().Where(t => t.Status == Status.Active.GetHashCode())
                        select new MasterClass
                        {

                            Description = Utility.ToSafeString(item.Description),
                            dtmAdd = Utility.ToSafeDate(item.dtmAdd),
                            dtmDelete = Utility.ToSafeDate(item.dtmDelete),
                            dtmUpdate = Utility.ToSafeDate(item.dtmUpdate),
                            Id = Utility.ToSafeInt(item.Id),
                            Image = Utility.ToSafeString(item.Image),
                            IpAddress = Utility.ToSafeString(item.IpAddress),
                            OredrNo = Utility.ToSafeInt(item.OredrNo),
                            Status = Utility.ToSafeInt(item.Status),
                            Title = Utility.ToSafeString(item.Title),
                            idServer = Utility.ToSafeInt(item.idServer),
                        }).AsEnumerable();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("api/GetSubjects")]
        public IEnumerable<Sql.MasterSubject> GetSubjects()
        {
            try
            {
                return (from item in new MasterSubjectRepository().GetAll().ToList().Where(t => t.Status == Status.Active.GetHashCode())
                        select new PrachiIndia.Sql.MasterSubject
                        {

                            Description = Utility.ToSafeString(item.Description),
                            dtmAdd = Utility.ToSafeDate(item.dtmAdd),
                            dtmDelete = Utility.ToSafeDate(item.dtmDelete),
                            dtmUpdate = Utility.ToSafeDate(item.dtmUpdate),
                            Id = Utility.ToSafeInt(item.Id),
                            Image = Utility.ToSafeString(item.Image),
                            IpAddress = Utility.ToSafeString(item.IpAddress),
                            OredrNo = Utility.ToSafeInt(item.OredrNo),
                            Status = Utility.ToSafeInt(item.Status),
                            Title = Utility.ToSafeString(item.Title),
                            idServer = Utility.ToSafeInt(item.idServer),
                        }).AsEnumerable();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("api/GetSeries")]
        public IEnumerable<MasterSery> GetSeries()
        {
            try
            {
                return (from item in new MasterSeriesRepositories().GetAll().ToList().Where(t => t.Status == Status.Active.GetHashCode())
                        select new MasterSery
                        {
                            Description = Utility.ToSafeString(item.Description),
                            dtmAdd = Utility.ToSafeDate(item.dtmAdd),
                            dtmDelete = Utility.ToSafeDate(item.dtmDelete),
                            dtmUpdate = Utility.ToSafeDate(item.dtmUpdate),
                            Id = Utility.ToSafeInt(item.Id),
                            Image = Utility.ToSafeString(item.Image),
                            IpAddress = Utility.ToSafeString(item.IpAddress),
                            OredrNo = Utility.ToSafeInt(item.OredrNo),
                            Status = Utility.ToSafeInt(item.Status),
                            Title = Utility.ToSafeString(item.Title),
                            idServer = Utility.ToSafeInt(item.idServer),
                            SubjectId = item.SubjectId,
                        }).AsEnumerable();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("api/GetBooks/{ReaderId}")]
        public IEnumerable<tblCataLog> GetBooks(string ReaderId)
        {
            try
            {
                //var id = User.Identity.GetUserId();
                // id = RequestContext.Principal.Identity.GetUserId();
                var catatlogRepository = new CatalogRepository();
                var listBook = catatlogRepository.Search(t => t.Ebook == (int)BookType.EBook &&
                    (t.Status == (int)Status.Active) && (t.idServer > 0) &&
                    !(string.IsNullOrEmpty(t.isSize) && !t.isSize.Contains("N/A"))).ToList();
                if (listBook == null || listBook.Count() == 0)
                    return null;
                var moduleFilePath = ConfigurationManager.AppSettings["ModuleFiles"].ToString();
                var imagePath = string.Format("{0}Items/", moduleFilePath);
                var books = GetBooks(listBook);
                return books;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        [Route("api/GetChapters/{ReaderId}")]
        public IEnumerable<BookChapters> GetChapters(string ReaderId)
        {
            try
            {
                var id = User.Identity.GetUserId();
                var readerRepository = new CatalogRepository();
                var listBook = readerRepository.Search(t => t.Ebook == (int)BookType.EBook &&
                    (t.Status == (int)Status.Active) && (t.idServer > 0) &&
                    !(string.IsNullOrEmpty(t.isSize) && !t.isSize.Contains("N/A"))).ToList();
                if (listBook == null || listBook.Count() == 0)
                    return null;
                var BookIds = string.Join(",", listBook.Select(r => r.idServer.ToString()));
                Areas.Readedge_BusLogic.blChapters blchapters = new Areas.Readedge_BusLogic.blChapters();
                var Chapters = blchapters.GetAll(BookIds);
                foreach (var item in Chapters)
                {
                    var book = listBook.FirstOrDefault(m => m.idServer == item.idBook);
                    item.idBook = (int)book.Id;
                    item.idSubject = (int)book.SubjectId;
                    item.idSeries = (int)book.SeriesId;
                    item.idClass = book.ClassId;
                }
                return Chapters;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        [Route("api/GetReader/{ReaderId}")]
        public UserAppReader GetReader(string ReaderId)
        {
            try
            {
                var id = User.Identity.GetUserId();
                var readerRepository = new ReaderRepository();
                var readers = readerRepository.Search(t => t.Id == ReaderId).ToList();
                if (readers == null || readers.Count() == 0)
                    return null;
                var reader = readers.FirstOrDefault();
                var objRader = new UserAppReader
                {
                    Id = Utility.ToSafeString(reader.Id),
                    BookVersion = Utility.ToSafeInt(reader.BookVersion),
                    CreatedDate = (DateTime)Utility.ToSafeDate(reader.CreatedDate),
                    DescripTion = Utility.ToSafeString(reader.Description),
                    DeviceType = (Web.Areas.Model.DeviceType)Utility.ToSafeInt(reader.DeviceType),
                    LibraryVersion = Utility.ToSafeInt(reader.LibraryVersion),
                    MachineKey = Utility.ToSafeString(reader.MachineKey),
                    MasterVesrion = Utility.ToSafeInt(reader.MasterVesrion),
                    ReaderKey = Utility.ToSafeString(reader.ReaderKey),
                    ReaderStatus = Utility.ToSafeInt(reader.Status),
                    SynchDate = (DateTime)Utility.ToSafeDate(reader.SynchDate),
                    UpdatedDate = (DateTime)Utility.ToSafeDate(reader.UpdatedDate),
                    UserId = Utility.ToSafeString(reader.UserId)
                };
                return objRader;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        [Route("api/GetBookPlus/{BookId}")]
        public IEnumerable<BookPlus> GetBookPlus(long BookId)
        {
            try
            {
                var id = User.Identity.GetUserId();
                var readerRepository = new CatalogRepository();
                var listBook = readerRepository.Search(t => t.idServer == BookId);

                if (listBook == null || listBook.Count() == 0)
                    return null;
                var BookIds = string.Join(",", listBook.Select(r => r.idServer.ToString()));
                Areas.Readedge_BusLogic.blBookPlus blBookPlus = new Areas.Readedge_BusLogic.blBookPlus();
                var bplist = blBookPlus.GetAll(BookIds);
                foreach (var item in bplist)
                {
                    var book = listBook.FirstOrDefault(m => m.idServer == item.idBook);
                    item.idBook = (int)book.Id;
                    item.idSubject = (int)book.SubjectId;
                    item.idSeries = (int)book.SeriesId;


                }
                return bplist;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return null;
            }
        }
        [Route("api/GetBookImages/{ReaderId}")]
        public string GetBookImages(string ReaderId)
        {
            string imgFolderPath = "";
            try
            {
                var id = User.Identity.GetUserId();
                // id = RequestContext.Principal.Identity.GetUserId();
                var readerRepository = new CatalogRepository();
                var listBook = readerRepository.Search(t => t.Ebook == (int)Framework.BookType.EBook &&
                    (t.Status == (int)Status.Active) && (t.idServer > 0) &&
                    !(string.IsNullOrEmpty(t.isSize) && !t.isSize.Contains("N/A"))).ToList();
                if (listBook == null || listBook.Count() == 0)
                    return "";
                var srcDir = HttpContext.Current.Server.MapPath("~/ModuleFiles/Items/");
                var ZipDir = srcDir + ReaderId;
                DirectoryInfo dir = new DirectoryInfo(ZipDir);
                if (dir.Exists)
                {
                    Utility.RecursiveDelete(dir);
                }
                dir.Create();
                var books = GetBooks(listBook);
                foreach (var book in books)
                {
                    if (!string.IsNullOrWhiteSpace(book.Image))
                    {
                        var srcFile = srcDir + book.Image;
                        var destFile = ZipDir + "/" + book.Image;
                        File.Copy(srcFile, destFile);
                    }
                }
                if (File.Exists(ZipDir + ".zip"))
                {
                    File.Delete(ZipDir + ".zip");
                }
                if (dir.Exists)
                {
                    System.IO.Compression.ZipFile.CreateFromDirectory(ZipDir, ZipDir + ".zip");
                }
                System.Threading.Thread.Sleep(100);
                Utility.RecursiveDelete(dir);
                var moduleFilePath = ConfigurationManager.AppSettings["ModuleFiles"].ToString();
                imgFolderPath = string.Format("{0}/{1}", moduleFilePath, ReaderId + ".zip");
                return imgFolderPath;
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
                return "";
            }
        }
        public IEnumerable<tblCataLog> GetBooks(IEnumerable<tblCataLog> listBook)
        {
            return (from item in listBook
                    select new tblCataLog
                    {
                        Author = item.Author,
                        BoardId = item.BoardId,
                        ClassId = item.ClassId,
                        Description = item.Description,
                        Discount = item.Discount,
                        dtmAdd = item.dtmAdd,
                        dtmDelete = item.dtmDelete,
                        dtmUpdate = item.dtmUpdate,
                        Ebook = item.Ebook,
                        Edition = item.Edition,
                        Id = item.Id,
                        Image = string.IsNullOrWhiteSpace(item.Image) ? "" : item.Image,
                        IpAddress = item.IpAddress,
                        ISBN = item.ISBN,
                        MultiMedia = item.MultiMedia,
                        OrderNo = item.OrderNo,
                        Price = item.Price,
                        SeriesId = item.SeriesId,
                        Solutions = item.Solutions,
                        SubjectId = item.SubjectId,
                        Status = item.Status,
                        Title = item.Title,
                        idServer = item.idServer,
                        isSize = item.isSize
                    });
        }

        [HttpPost]
        [Route("api/eBookOrder")]
        //Story: First check Reader against machinekey if exist return shame reader
        //if  not exist check readercount <5 creaet new reader and return new reader.
        public async Task<eBookOrder> eBookOrder([FromBody]eBookOrder order)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userReo = new AspNetUserRepository();
                var user = userReo.FindByIdAsync(order.UserId);

                var orderRequest = new Order
                {
                    ReaderId = order.ReaderId,
                    Amount = order.Price,
                    DiscountPer = order.Discount,
                    TotalAmount = order.TotalAmount,
                    Email = user.Email,
                    Name = user.FirstName,
                    Phone = user.PhoneNumber,
                    ProductInfo = order.ProductInfo,
                    TransactionId = Guid.NewGuid().ToString(),
                    UserId = order.UserId,

                };
                var merchantKey = ConfigurationManager.AppSettings["Key"].ToString(CultureInfo.InvariantCulture);
                var merchantSalt = ConfigurationManager.AppSettings["Salt"].ToString(CultureInfo.InvariantCulture);
                var serviceProvider = "payu_paisa";
                var payment = order.TotalAmount.ToString("N2");
                var hashString = merchantKey + "|" + orderRequest.TransactionId + "|" + payment + "|" + orderRequest.ProductInfo + "|" + orderRequest.Name + "|" + orderRequest.Email + "|||||||||||" + merchantSalt;
                var hash = PayUMoney.Generatehash512(hashString);
                orderRequest.RequestLog = hashString;
                orderRequest.RequestHash = hash;
                orderRequest.Status = PayUMoney.PaymentStatus.Initiated.GetHashCode();
                orderRequest.CreatedDate = DateTime.UtcNow;
                orderRequest.UpdatedDate = DateTime.UtcNow;
                var orderinsert = new OrderRepository().CreateAsync(orderRequest);//await
                if (orderinsert != null)
                {

                    var OrderId = Utility.ToSafeInt(new OrderRepository().GetAll().FirstOrDefault(x => x.RequestHash == hash).Id);
                    orderRequest.OrderId = "EB-" + OrderId.ToString().PadLeft(6, '0');
                    var resultupdate = new OrderRepository().UpdateAsync(orderRequest);
                    if (resultupdate != null)
                    {
                        order.BookIdServer = string.IsNullOrWhiteSpace(order.BookIdServer) ? "0" : order.BookIdServer;
                        var orderProduct = new OrderProduct
                        {
                            idServer = Utility.ToSafeInt(order.BookIdServer),
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            ItemId = order.BookId,
                            OrderId = OrderId,
                            Title = order.Title,
                            Quantity = order.Quantity,
                            Price = order.Price,
                            Discount = order.Discount,
                            TotalAmount = order.TotalAmount,
                            BookType = order.BookType.GetHashCode()
                        };
                        var result = new OrderProductRepository().CreateAsync(orderProduct);
                        if (result != null)
                        {
                            order.OrderId = orderRequest.OrderId;
                            order.Id = OrderId;
                        }
                        else
                        {
                            new OrderProductRepository().Delete(orderProduct);
                            new OrderRepository().Delete(orderRequest);
                        }
                    }
                    else
                    {
                        new OrderRepository().Delete(orderRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                var v1 = ex.Message;
            }
            return order;
        }
        [HttpGet]
        [Route("api/OrderStatus/{Id}")]
        public async Task<eBookOrder> OrderStatus(int Id)
        {
            var order = new eBookOrder();
            var Order = new OrderRepository().GetById(Id);
            var OrderItems = new OrderProductRepository().GetAll(Id).FirstOrDefault();
            var catalog = new CatalogRepository().FindByIdAsync((int)OrderItems.ItemId);
            var book = new PrachiIndia.Portal.Areas.Readedge_BusLogic.blBook().Get((int)catalog.idServer);

            try
            {
                order = new eBookOrder()
                {
                    AddtionalCharge = "",
                    BookId = Utility.ToSafeInt(OrderItems.ItemId),
                    BookIdServer = Utility.ToSafeString(OrderItems.idServer),
                    BookType = Utility.ToSafeInt(OrderItems.BookType),
                    CreatedDate = Utility.ToSafeDate1(Order.CreatedDate),
                    Discount = Utility.ToSafeInt(OrderItems.Discount),
                    Id = Utility.ToSafeInt(Order.Id),
                    OrderId = Order.OrderId,
                    PayUMoneyId = Utility.ToSafeString(Order.PayUMoneyId),
                    PGType = Utility.ToSafeString(Order.PGType),
                    Price = Utility.ToSafeDecimal(OrderItems.Price),
                    ProductInfo = Utility.ToSafeString(Order.ProductInfo),
                    Quantity = Utility.ToSafeInt(OrderItems.Quantity),
                    ReaderId = Utility.ToSafeString(Order.ReaderId),
                    RequestHash = Utility.ToSafeString(Order.RequestHash),
                    RequestLog = Utility.ToSafeString(Order.RequestHash),
                    ResponseHas = Utility.ToSafeString(Order.ResponseHas),
                    ResponseLog = Utility.ToSafeString(Order.ResponseLog),
                    Status = (Portal.Framework.PayUMoney.PaymentStatus)Utility.ToSafeInt(Order.Status),
                    Title = Utility.ToSafeString(Order.ReaderId),
                    TotalAmount = Utility.ToSafeDecimal(Order.TotalAmount),
                    TransactionId = Utility.ToSafeString(Order.TransactionId),
                    UpdatedDate = Utility.ToSafeDate1(Order.UpdatedDate),
                    UserId = Utility.ToSafeString(Order.UserId),
                    DownloadPath = "http://readedge.in/library/" + book.ShortCut + ".epub",
                    EncryptionKey = book.EncriptionKey
                };
                if (order.Status == PayUMoney.PaymentStatus.Success)
                {
                    var userLibrary = new Sql.UserLibrary
                    {
                        BookId = (long)OrderItems.ItemId,
                        //ReaderId = Order.ReaderId,
                        UserId = Order.UserId,
                        //CoverImage = book.Image,
                        CreatedDate = DateTime.UtcNow,
                        //Creator = Order.UserId,
                        //DiscountAmount = OrderItems.Price - Order.TotalAmount,
                        //DiscountPercentage = Order.DiscountPer,
                        // DownloadCount = 0,
                        //EPubPath = "http://readedge.in/library/" + book.ShortCut + ".epub",
                        Id = Guid.NewGuid().ToString(),
                        //Price = OrderItems.Price,
                        UpdatedDate = DateTime.UtcNow,
                        EncriptionKey = book.EncriptionKey,
                        //PubIdentifier = "",
                        PublishDate = DateTime.Now,
                        //Publisher = "Prachi India Pvt Ltd.",
                        // OrderId = Order.Id,
                        // Title = OrderItems.Title,
                        // Status = 1,
                    };
                    var v1 = new ReaderBookRepository().CreateAsync(userLibrary);
                    order.IdServer = v1.Id;
                }
            }
            catch (Exception ex)
            {

                order = null;
            }
            return order;
        }
        #endregion
        #region "Old API"
        [HttpGet]
        [Route("api/GetLibraries/{readerId}")]
        public List<Models.UserReaderBook> GetLibraries(string readerId)
        {
            var readerBookRepository = new ReaderBookRepository();
            var moduleFilePath = ConfigurationManager.AppSettings["ModuleFiles"].ToString();
            var imagePath = string.Format("{0}Items/", moduleFilePath);
            var epubPath = string.Format("{0}Epub/", moduleFilePath);
            var userId = User.Identity.GetUserId();
            var items = readerBookRepository.SearchFor(t => t.UserId == userId).ToList();
            var results = new List<Models.UserReaderBook>();
            if (items == null || !items.Any())
                return results;
            foreach (var item in items)
            {
                var reader = new Models.UserReaderBook
                {
                    BookId = item.BookId,
                    //CoverImage = string.IsNullOrWhiteSpace(item.CoverImage) ? "" : imagePath + item.CoverImage,
                    CreatedDate = item.CreatedDate,
                    //Creator = item.Creator,
                    //DiscountAmount = item.DiscountAmount,
                    //DiscountPercentage = item.DiscountPercentage,
                    //DownloadCount = item.DownloadCount,
                    ErrorMessage = "",
                    Id = item.Id,
                    //Language = item.Language,
                    //Price = item.Price,
                    //PubIdentifier = item.PubIdentifier,
                    PublishDate = item.PublishDate,
                    //Publisher = item.Publisher,
                    ReaderId = readerId,
                    Result = Models.ResponseStatus.Succeeded.ToString(),
                    //Status = item.Status,
                    TimeStamp = DateTime.UtcNow,
                    //Title = item.Title,
                    UpdatedDate = item.UpdatedDate,
                    UserId = item.UserId,
                    //EPubPath = string.IsNullOrWhiteSpace(item.EPubPath) ? "" : epubPath + item.EPubPath,
                    EncriptionKey = item.EncriptionKey
                };
                results.Add(reader);
            }
            return results;
        }
        [HttpGet]
        [Route("api/UpdateReaderLibrary/{readerId}/{bookId}/{status}")]
        public async Task<Models.UserReader> UpdateReaderLibrary(string readerId, long bookId, bool status = false)
        {

            var readerBookRepository = new ReaderBookRepository();
            var readerRepository = new ReaderRepository();
            var userReaderRepository = new UserReaderRepository();
            var userId = User.Identity.GetUserId();
            var items = readerBookRepository.SearchFor(t => t.UserId == userId && t.BookId == bookId);
            var reader = readerRepository.Search(t => t.UserId == userId && t.Id == readerId).FirstOrDefault();
            if (reader == null)
            {
                var result = new Models.UserReader
                {
                    ErrorMessage = "Reader does't exist.",
                    Id = readerId,
                    UserId = userId,
                    Result = Models.ResponseStatus.Failed.ToString()

                };
                return result;
            }
            try
            {
                var dbContext = new dbPrachiIndia_PortalEntities();

                if (status == false)
                {
                    foreach (var item in items)
                    {
                        var isRecordExist = dbContext.UserReaderLibraries.FirstOrDefault(t => t.UserId == userId && t.ReaderId == readerId && t.BookId == bookId);
                        if (isRecordExist == null)
                        {

                            var userLib = new UserReaderLibrary
                            {
                                BookId = bookId,
                                DounloadCount = 0,
                                LibraryId = item.Id,
                                ReaderId = reader.Id,
                                UserId = userId,
                                Id = Guid.NewGuid().ToString(),

                            };
                            userReaderRepository.CreateAsync(userLib);

                        }

                    }

                }
                else
                {
                    var userReaderLibrarys = dbContext.UserReaderLibraries.ToList();

                    foreach (var reade in userReaderLibrarys)
                    {
                        dbContext = new dbPrachiIndia_PortalEntities();
                        reade.DounloadCount = reade.DounloadCount + 1;
                        userReaderRepository.UpdateAsync(reade);
                    }
                    foreach (var item in items)
                    {
                        dbContext = new dbPrachiIndia_PortalEntities();
                        //item.DownloadCount = item.DownloadCount + 1;
                        readerBookRepository.UpdateAsync(item);

                    }
                }
                var result2 = new Models.UserReader
                {
                    ErrorMessage = "",
                    Id = readerId,
                    UserId = userId,
                    Result = Models.ResponseStatus.Succeeded.ToString()

                };
                return result2;
            }
            catch (Exception ex)
            {
                var result = new Models.UserReader
                {
                    ErrorMessage = "Reader does't exist.",
                    Id = readerId,
                    UserId = userId,
                    Result = Models.ResponseStatus.Unknown.ToString()

                };
                return result;
            }
        }


        [Route("api/GetEPub/{librayId}")]
        public async Task<HttpResponseMessage> GetEPub(string librayId)
        {
            var readerBookRepository = new ReaderBookRepository();
            var moduleFilePath = ConfigurationManager.AppSettings["ModuleUrls"].ToString();
            var epubPath = string.Format("{0}EPub/", moduleFilePath);
            var userId = User.Identity.GetUserId();
            var items = readerBookRepository.FindByIdAsync(librayId);
            HttpResponseMessage result = null;
            if (items != null)
            {
                var localFilePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ModuleFiles/EPub/"), items.EPubName);

                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = items.EPubName;

            }
            return result;
        }



        #endregion

        [AllowAnonymous]
        [Route("api/GetBooks")]
        public Models.UserReaderBook GetBooks()
        {
            var x = new Models.UserReaderBook
            {
                Creator = "Rahul",
                BookId = 1
            };
            return x;
        }

    }
}
