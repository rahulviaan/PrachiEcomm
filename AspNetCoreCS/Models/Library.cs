using DAL.Models.Entities;
using ReadEdgeCore.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.Interfaces;
using Ionic.Zip;
using ReadEdgeCore.Utilities;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ReadEdgeCore.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using ReadEdgeCore.Models.ViewModel;

namespace ReadEdgeCore.Models
{
    public class Library : ILibrary
    {
        private readonly IHubContext<BroadcastHub> _hubContext;
        private ILibraryRepository _ILibraryRepository;
        private IBundleConfiguration _IbundleConfiguration;
        private IUser _user;

        //public static double percentage = 0;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private SessionMgt sessionMgt=new SessionMgt();
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private IPrachiuser _prachiUser { get; set; }
        public Library(ILibraryRepository ILibraryRepository, IUser user, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IHubContext<BroadcastHub> context,IBundleConfiguration bundleConfiguration, IPrachiuser prachiuser)
        {
            _ILibraryRepository = ILibraryRepository;
            _user= user;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = context;
            _IbundleConfiguration = bundleConfiguration;
            _prachiUser = prachiuser;
            //_sessionMgt = sessionMgt;
        }
        public async Task Add(UserLibrary userLibrary)
        {
           await _ILibraryRepository.Add(userLibrary);
        }

        public async Task AddChapter(Chapter chapter)
        {
            await _ILibraryRepository.AddChapter(chapter);
        }

        public async Task AddChapterContent(ChapterContent chapterContent)
        {
           await _ILibraryRepository.AddChapterContent(chapterContent);
        }

        public async Task Delete(int Id)
        {
            await _ILibraryRepository.Delete(Id);
        }

        public async Task DeleteChapter(int Id)
        {
            await _ILibraryRepository.DeleteChapter(Id);
        }

        public async Task DeleteChapterContent(int Id)
        {
            await _ILibraryRepository.DeleteChapterContent(Id);
        }

        public async Task<IEnumerable<UserLibrary>> GetAllLibrary()
        {
           return await _ILibraryRepository.GetAllLibrary();
        }

        public async Task GetChapter(int Id)
        {
            await _ILibraryRepository.GetChapter(Id);
        }

        public async Task<ChapterContent> GetChapterContent(long Id)
        {
            return await _ILibraryRepository.GetChapterContent(Id);
        }

        public async Task<IEnumerable<ChapterContent>> GetChapterContents()
        {
             return await _ILibraryRepository.GetChapterContents();
        }

        public async Task<IEnumerable<Chapter>> GetChapters()
        {
            return await _ILibraryRepository.GetChapters();
        }

        public async Task<UserLibrary> GetLibrary(string Id)
        {
            return await _ILibraryRepository.GetLibrary(Id);
        }

        public async Task Update(UserLibrary userLibrary)
        {
            await _ILibraryRepository.Update(userLibrary);
        }

        public async Task UpdateChapter(Chapter chapter)
        {
            await _ILibraryRepository.UpdateChapter(chapter);
        }

        public async Task UpdateChapterContent(ChapterContent chapterContent)
        {
            await _ILibraryRepository.UpdateChapterContent(chapterContent);
        }


        public async Task  UploadConfig(IFormFile PostedFile)
        {
            //sessionMgt = GetSession("Progress");
            //percentage = sessionMgt.Val;
            //percentage= Convert.to(sessionMgt.Val)
               string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
                string filePath = Path.Combine(uploadsFolder, PostedFile.FileName);
                if ((File.Exists(filePath)))
                {
                    File.Delete(filePath);
                }
                PostedFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                //var filepath = uploadsFolder + "\\" + PostedFile.FileName;
                ZipFile zip = ZipFile.Read(filePath);
                zip.ExtractAll(Common.AppTempPath(), ExtractExistingFileAction.OverwriteSilently);
          
                var allfiles = Directory.GetFiles(Common.AppTempPath(), "PO_Encrypted.xml", SearchOption.AllDirectories);

            

            var configurationFile = allfiles.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(configurationFile))
            {
                var stramReader = new StreamReader(configurationFile);
                var encString = stramReader.ReadToEnd();
                var xmlString = Cryptography.DecryptCommon(encString, "");
                var configuration = Common.ConvertFromXML<UserConfiguration>(xmlString);
                
                // Check  if User already exists & configuration uploaded 
                var IsUserExist = await _user.GetUserLogin(configuration.UserLogin.UserName, configuration.UserLogin.Password);

                if (configuration != null && configuration.Libraries != null && configuration.Libraries.Any())
                {
                    if (_user.GetAllUser().Result.Any())
                    {
                        if (IsUserExist != null && IsUserExist.Name == configuration.AspNetUser.UserName) { }
                        else
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Failed!!! Wrong configuration....User not exists", 0);
                            Thread.CurrentThread.Abort();
                        }
                    }
                    else
                    {
                        var UserLogins = new UserLogin
                        {

                            AccessToken = configuration.UserLogin.AccessToken,
                            Expires = configuration.UserLogin.Expires,
                            ExpiresIn = configuration.UserLogin.ExpiresIn,
                            Id = configuration.UserLogin.Id,
                            Issued = configuration.UserLogin.Issued,
                            Name = configuration.UserLogin.Name,
                            Password = configuration.UserLogin.Password,
                            TokenType = configuration.UserLogin.TokenType,
                            UserId = configuration.UserLogin.UserId,
                            UserName = configuration.UserLogin.UserName,
                            UserRoles = configuration.UserLogin.UserRoles
                        };

                        await _user.AddUserLogin(UserLogins);
                        // percentage = Convert.ToString(Convert.ToInt32(percentage)+ Convert.ToInt32(Common.GetProgressPercenttage(1, 1, 5)));
                        Common.percentage = Common.GetProgressPercenttage(1, 1, 5);
                        //sessionMgt.Val = percentage;
                        //SetSession(sessionMgt);

                        //chat.SendMessage("Library", Convert.ToString(Common.percentage));

                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "User Logins", Convert.ToInt32(Common.percentage));
                        var userMaster = new UserMaster
                        {

                            AboutMe = configuration.AspNetUser.AboutMe,
                            Address = configuration.AspNetUser.Address,
                            UserName = configuration.AspNetUser.UserName,
                            CompleteName = configuration.AspNetUser.CompleteName,
                            Country = configuration.AspNetUser.Country,
                            Designation = configuration.AspNetUser.Designation,
                            DlNo = configuration.AspNetUser.DlNo,
                            Email = configuration.AspNetUser.Email,
                            FirstName = configuration.AspNetUser.FirstName,
                            Id = configuration.AspNetUser.Id,
                            Industry = configuration.AspNetUser.Industry,
                            IsVerified = configuration.AspNetUser.IsVerified,
                            Organization = configuration.AspNetUser.Organization,
                            Panid = configuration.AspNetUser.PANId,
                            PassportNo = configuration.AspNetUser.PassportNo,
                            PhoneNumber = configuration.AspNetUser.PhoneNumber,
                            PinCode = configuration.AspNetUser.PinCode,
                            Profession = configuration.AspNetUser.Profession,
                            ProfileImage = configuration.AspNetUser.ProfileImage,
                            Remark = configuration.AspNetUser.Remark,
                            State = configuration.AspNetUser.State,
                            Status = configuration.AspNetUser.Status,
                            LastName = configuration.AspNetUser.LastName
                        };
                        await _user.Add(userMaster);

                        //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(1, 1, 5)));
                        Common.percentage = Common.GetProgressPercenttage(1, 1, 5);
                        //sessionMgt.Val = percentage;
                        //SetSession(sessionMgt);
                        //chat.SendMessage("UserMaster", Convert.ToString(Common.percentage));
                        Thread.Sleep(2000);
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "User Master", Convert.ToInt32(Common.percentage));
                    }
                    int libcount = 1;
                    foreach (var library in configuration.Libraries)
                    {
                        Common.percentage = 0;
                        var UserLibrary = new UserLibrary
                        {
                            BookId = library.BookId,
                            CreatedDate = DateTime.Now,
                            EncriptionKey = library.EncriptionKey,
                            EpubName = library.EpubName,
                            EpubPath = library.EPubPath,
                            Id = library.Id,
                            UpdateDate = DateTime.Now,
                            UserId = library.UserId,
                            EpubDownloaded = false,
                            BundleUploaded = false
                        };
                        var lib = await GetLibrary(library.Id);
                        if (lib != null)
                        {
                            UserLibrary.EpubDownloaded = lib.EpubDownloaded;
                            UserLibrary.UpdateDate = DateTime.Now;
                        }
                        await Add(UserLibrary);
                        //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(libcount, configuration.Libraries.Count, 5)));
                        Common.percentage = Common.GetProgressPercenttage(libcount, configuration.Libraries.Count, 5);
                        //sessionMgt.Val = percentage;
                        //SetSession(sessionMgt);
                        // _hubContext.SendMessage("Library", Convert.ToString(Common.percentage));
                        Thread.Sleep(2000);
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Library", Convert.ToInt32(Common.percentage));
                        libcount = libcount + 1;
                    }
                    int Chaptercount = 1;
                    if (configuration.Chapters != null && configuration.Chapters.Any())
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapters", 0);
                        Thread.Sleep(3000);
                        foreach (var chapter in configuration.Chapters)
                        {
                            var chapt = new Chapter
                            {
                                BookId = chapter.BookId,
                                ChapterIndex = chapter.ChapterIndex,
                                CreatedDate = DateTime.Now,
                                Description = chapter.Description,
                                FromPage = chapter.FromPage,
                                Id = chapter.Id,
                                Status = chapter.Status,
                                Title = chapter.Title,
                                ToPage = chapter.ToPage,
                                UpdateDate = DateTime.Now
                            };
                            await AddChapter(chapt);
                            //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(Chaptercount, configuration.Chapters.Count, 5)));
                            Common.percentage = Common.GetProgressPercenttage(Chaptercount, configuration.Chapters.Count, 5);
                            //sessionMgt.Val = percentage;
                            //SetSession(sessionMgt);
                            //chat.SendMessage("Chapters", Convert.ToString(Common.percentage));
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapters", Convert.ToInt32(Common.percentage));
                            Chaptercount = Chaptercount + 1;
                        }

                        int ChapterContentcount = 1;
                        if (configuration.ChapterContents != null && configuration.ChapterContents.Any())
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapter Contents", 0);
                            Thread.Sleep(3000);
                            var i = 0;
                            foreach (var contents in configuration.ChapterContents)
                            {
                                i++;
                                var content = new ChapterContent
                                {
                                    BookId = contents.BookId,
                                    ChapterId = contents.ChapterId,
                                    ContentType = contents.ContentType,
                                    CreatedDate = DateTime.Now,
                                    Description = contents.Desc,
                                    Id = contents.Id,
                                    Name = contents.Name,
                                    OrderId = contents.OrderId,
                                    Status = contents.Status,
                                    Title = contents.Title,
                                    Type = contents.Type,
                                    UpdateDate = DateTime.Now
                                };
                                var cont = await GetChapterContent(content.Id);
                                if (cont != null)
                                {
                                    content.Downloaded = cont.Downloaded;
                                    content.UpdateDate = DateTime.Now;
                                }
                                await AddChapterContent(content);
                                //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(ChapterContentcount, configuration.ChapterContents.Count, 5)));
                                Common.percentage = Common.GetProgressPercenttage(ChapterContentcount, configuration.ChapterContents.Count, 5);
                                //sessionMgt.Val = percentage;
                                //SetSession(sessionMgt);
                                //chat.SendMessage("ChapterContent", Convert.ToString(Common.percentage));
                                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapter Contents", Convert.ToInt32(Common.percentage));
                                ChapterContentcount = ChapterContentcount + 1;

                            }

                        }


                    }
                }
            }
        }

        public async Task UploadReConfig(IFormFile PostedFile)
        {
            //sessionMgt = GetSession("Progress");
            //percentage = sessionMgt.Val;
            //percentage= Convert.to(sessionMgt.Val)
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
            string filePath = Path.Combine(uploadsFolder, PostedFile.FileName);
            if ((File.Exists(filePath)))
            {
                File.Delete(filePath);
            }
            PostedFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

            //var filepath = uploadsFolder + "\\" + PostedFile.FileName;
            ZipFile zip = ZipFile.Read(filePath);
            zip.ExtractAll(Common.AppTempPath(), ExtractExistingFileAction.OverwriteSilently);
            var allfiles = Directory.GetFiles(Common.AppTempPath(), "PO_Encrypted.xml", SearchOption.AllDirectories);

            var configurationFile = allfiles.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(configurationFile))
            {
                var stramReader = new StreamReader(configurationFile);
                var encString = stramReader.ReadToEnd();
                var xmlString = Cryptography.DecryptCommon(encString, "");
                var configuration = Common.ConvertFromXML<UserConfiguration>(xmlString);
                if (configuration != null && configuration.Libraries != null && configuration.Libraries.Any())
                {
                    int libcount = 1;
                    foreach (var library in configuration.Libraries)
                    {
                        Common.percentage = 0;
                        var UserLibrary = new UserLibrary
                        {
                            BookId = library.BookId,
                            CreatedDate = DateTime.Now,
                            EncriptionKey = library.EncriptionKey,
                            EpubName = library.EpubName,
                            EpubPath = library.EPubPath,
                            Id = library.Id,
                            UpdateDate = DateTime.Now,
                            UserId = library.UserId,
                            EpubDownloaded = false,
                            BundleUploaded = false
                        };
                        var lib = await GetLibrary(library.Id);
                        if (lib != null)
                        {
                            UserLibrary.EpubDownloaded = lib.EpubDownloaded;
                            UserLibrary.UpdateDate = DateTime.Now;
                        }
                        await Add(UserLibrary);
                        //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(libcount, configuration.Libraries.Count, 5)));
                        Common.percentage = Common.GetProgressPercenttage(libcount, configuration.Libraries.Count, 5);
                        //sessionMgt.Val = percentage;
                        //SetSession(sessionMgt);
                        // _hubContext.SendMessage("Library", Convert.ToString(Common.percentage));
                        Thread.Sleep(2000);
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Library", Convert.ToInt32(Common.percentage));
                        libcount = libcount + 1;
                    }
                    int Chaptercount = 1;
                    if (configuration.Chapters != null && configuration.Chapters.Any())
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapters", 0);
                        Thread.Sleep(3000);
                        foreach (var chapter in configuration.Chapters)
                        {
                            var chapt = new Chapter
                            {
                                BookId = chapter.BookId,
                                ChapterIndex = chapter.ChapterIndex,
                                CreatedDate = DateTime.Now,
                                Description = chapter.Description,
                                FromPage = chapter.FromPage,
                                Id = chapter.Id,
                                Status = chapter.Status,
                                Title = chapter.Title,
                                ToPage = chapter.ToPage,
                                UpdateDate = DateTime.Now
                            };
                            await AddChapter(chapt);
                            //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(Chaptercount, configuration.Chapters.Count, 5)));
                            Common.percentage = Common.GetProgressPercenttage(Chaptercount, configuration.Chapters.Count, 5);
                            //sessionMgt.Val = percentage;
                            //SetSession(sessionMgt);
                            //chat.SendMessage("Chapters", Convert.ToString(Common.percentage));
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapters", Convert.ToInt32(Common.percentage));
                            Chaptercount = Chaptercount + 1;
                        }

                        int ChapterContentcount = 1;
                        if (configuration.ChapterContents != null && configuration.ChapterContents.Any())
                        {
                            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapter Contents", 0);
                            Thread.Sleep(3000);
                            var i = 0;
                            foreach (var contents in configuration.ChapterContents)
                            {
                                i++;
                                var content = new ChapterContent
                                {
                                    BookId = contents.BookId,
                                    ChapterId = contents.ChapterId,
                                    ContentType = contents.ContentType,
                                    CreatedDate = DateTime.Now,
                                    Description = contents.Desc,
                                    Id = contents.Id,
                                    Name = contents.Name,
                                    OrderId = contents.OrderId,
                                    Status = contents.Status,
                                    Title = contents.Title,
                                    Type = contents.Type,
                                    UpdateDate = DateTime.Now
                                };
                                var cont = await GetChapterContent(content.Id);
                                if (cont != null)
                                {
                                    content.Downloaded = cont.Downloaded;
                                    content.UpdateDate = DateTime.Now;
                                }
                                await AddChapterContent(content);
                                //percentage = Convert.ToString(Convert.ToInt32(percentage) + Convert.ToInt32(Common.GetProgressPercenttage(ChapterContentcount, configuration.ChapterContents.Count, 5)));
                                Common.percentage = Common.GetProgressPercenttage(ChapterContentcount, configuration.ChapterContents.Count, 5);
                                //sessionMgt.Val = percentage;
                                //SetSession(sessionMgt);
                                //chat.SendMessage("ChapterContent", Convert.ToString(Common.percentage));
                                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Chapter Contents", Convert.ToInt32(Common.percentage));
                                ChapterContentcount = ChapterContentcount + 1;

                            }

                        }


                    }
                }
            }
        }

        public async Task UploadBundle(LibraryVM libraryVM)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "tempBundles");
            string filePath = string.Empty;
            if (libraryVM.BundlePackage)
            {
                filePath = Path.Combine(uploadsFolder, libraryVM.ConfigurationFile.FileName);
            }
            else
            {
                filePath = Path.Combine(uploadsFolder, libraryVM.BundleFile.FileName);
            }
            //if ((File.Exists(filePath)))
            //{
            //    File.Delete(uploadsFolder);
            //}
            //DirectoryInfo di = new DirectoryInfo(uploadsFolder);

            //foreach (FileInfo file in di.GetFiles())
            //{
            //    file.Delete();
            //}
            //foreach (DirectoryInfo dir in di.GetDirectories())
            //{
            //    dir.Delete(true);
            //}

            #region Copying...
            if (libraryVM.BundlePackage)
            {
                libraryVM.ConfigurationFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }
            else {
                libraryVM.BundleFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }

            for (int i = 1; i <=49; i++)
            {
                Thread.Sleep(3000);
                await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Bundle Initializing...", i * 2);
            }
            await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Bundle Initialized...", 100);
            Thread.Sleep(1000);
            for (int i = 1; i <= 49; i++)
            {
                Thread.Sleep(3000);
                await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Bundle Processing...", i * 2);
            }
           
            #endregion
            var zipFile = ZipFile.Read(filePath);
            zipFile.ExtractAll(uploadsFolder, ExtractExistingFileAction.OverwriteSilently);
            List<UserLibrary> libraries = Factory.GetUserLibraryList();
            if (libraryVM.BundlePackage)
            {
                libraries = _ILibraryRepository.GetAllLibrary().Result.Where(t => t.BundleUploaded != true).ToList();
                if (libraries != null && libraries.Any())
                {
                    foreach (var library in libraries)
                    {
                        var ext = library.EpubName.Split('.')[1];
                        ext = "." + ext;
                        var zipfile = library.EpubName.Replace(ext, ".zip");
                        //var searchFile = filePath + "\\" + zipfile;
                        var searchFile = filePath.Replace(".zip", "") + "\\" + zipfile;
                        //var searchFile = filePath;
                        var result = zipFile.Any(entry => entry.FileName.EndsWith(zipfile));
                        if (File.Exists(searchFile))
                        {
                            await BindAllBundle(searchFile, library.BookId, library.EpubName);
                            var message = library.EpubName + " uploaded successfully...";
                            await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", message, 100);
                        }

                    }
                }
            }
            else {
                libraries = _ILibraryRepository.GetAllLibrary().Result.Where(t => t.BundleUploaded != true && t.BookId== libraryVM.BookId).ToList();
                if (libraries != null && libraries.Any())
                {
                    foreach (var library in libraries)
                    {
                        var ext = library.EpubName.Split('.')[1];
                        ext = "." + ext;
                        var zipfile = library.EpubName.Replace(ext, ".zip");
                        var searchFile = uploadsFolder + "\\" + zipfile;
                        var result = zipFile.Any(entry => entry.FileName.EndsWith(zipfile));
                        if (File.Exists(searchFile))
                        {
                            await BindAllBundle(searchFile, library.BookId, library.EpubName);
                            var message = library.EpubName + " uploaded successfully...";
                            await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", message, 100);
                        }

                    }
                }
            }

            //if ((File.Exists(filePath)))
            //{
            //    File.Delete(uploadsFolder);
            //}
            Thread.Sleep(3000);
            await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "", 100);
        }


        public async Task BindAllBundle(string bookname, long bookId, string epubname)
        {
            //await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Bundle uploading initiated..", Common.percentage);
            string BookPath = Path.Combine(_hostingEnvironment.WebRootPath, "Books");
            var isConfigured =await _IbundleConfiguration.UploadBundles(bookname, BookPath, bookId, epubname);
            if (isConfigured)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Update library..", 85);
                //var library = SqlLiteHelper.GetLibrary(bookId, Constants.UserId);
                var library = _ILibraryRepository.GetAllLibrary().Result.FirstOrDefault(x=>x.BookId==bookId);
                library.BundleUploaded = isConfigured;
                //SqlLiteHelper.InsertOrReplace(library);
                await  _ILibraryRepository.Update(library);
                await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Content migration initiated..", 90);
                var directoryname = epubname.Split('.')[0];
                var epubDirectory = BookPath +"\\"+directoryname + "\\";
                string[] filePaths = Directory.GetFiles(epubDirectory);
                //var chapterContents = SqlLiteHelper.GetContentsByBook(bookId);
                var chapterContents = await _ILibraryRepository.GetChapterContents();
                chapterContents= chapterContents.Where(x => x.BookId == bookId);
                await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Content migration..", 95);
                var ChapterContentscount = 0;
                foreach (var chaptContent in chapterContents)
                {
                    var Filename = epubDirectory + chaptContent.Name;
                    if (filePaths.Contains(Filename))
                    {
                        if (!chaptContent.Downloaded)
                        {
                            chaptContent.Downloaded = true;
                            chaptContent.UpdateDate = DateTime.Now;
                            await _ILibraryRepository.UpdateChapterContent(chaptContent);                           
                        }
                    }
                    Common.Bundlepercentage = Common.GetProgressPercenttage(ChapterContentscount, chapterContents.Count(), 5);
                    await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Updating chapters...", Convert.ToInt32(Common.Bundlepercentage));
                    ChapterContentscount = ChapterContentscount + 1;
                }
                //await _hubContext.Clients.All.SendAsync("ReceiveMessageBundle", "Bundle uploaded successfully.", 100);
            }
        }

        public async  Task<ClassModel> GetClass(string Id)
        {
           return  await _ILibraryRepository.GetClass(Id);
        }

        public async Task<IEnumerable<ClassModel>> GetClass()
        {
            return await _ILibraryRepository.GetClass();
        }

        public async  Task<BookModel> GetBooks(Int64 Id)
        {
            return await _ILibraryRepository.GetBooks(Id);
        }

        public async  Task<IEnumerable<BookModel>> GetBooks()
        {
                return await _ILibraryRepository.GetBooks();
        }

        public async Task<SubjectModel> GetSubjects(string Id)
        {
            return await _ILibraryRepository.GetSubjects(Id);
        }

        public async Task<IEnumerable<SubjectModel>> GetSubjects()
        {
            return await _ILibraryRepository.GetSubjects();
        }

        public async Task<IEnumerable<long>> GetSubjectByClass(long Id)
        {
            return await _ILibraryRepository.GetSubjectByClass(Id);
        }

        #region BookMarks
        public void AddBookMarks(UserBookMark userBookMark)
        {
            userBookMark.UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            _ILibraryRepository.AddBookMarks(userBookMark);
        }

        public async Task DeleteBookMarks(long Id)
        {
            await _ILibraryRepository.DeleteBookMarks(Id);
        }

        public bool ValidBookMark(UserBookMark userBookMark)
        {
           var bookmarks= _ILibraryRepository.GetBookMarks();
          return bookmarks.Result.Any(x => x.UserId == userBookMark.UserId && x.ContentId == userBookMark.ContentId && x.PageNumber == userBookMark.PageNumber && x.IsUserContent == userBookMark.IsUserContent);
        }


        public async Task<IEnumerable<UserBookMark>> GetBookMarks()
        {
            return await _ILibraryRepository.GetBookMarks();
        }

        public async Task<IEnumerable<UserBookMark>> GetBookMarksByUserAndContentId(UserBookMark userBookMark)
        {
            var bookmarks = await _ILibraryRepository.GetBookMarks();
           return  bookmarks.Where(x => x.ContentId == userBookMark.ContentId && x.ContentType == userBookMark.ContentType && x.UserId == userBookMark.UserId).ToList(); 
        }


        #endregion


        #region Notes
        public void AddNotes(UserNote userNote)
        {
             userNote.UserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            _ILibraryRepository.AddNotes(userNote);
        }
        public async Task<IEnumerable<UserNote>> GetNotesByUserAndContentId(UserNote userNote)
        {
            var notes = await _ILibraryRepository.GetNotes();
            return notes.Where(x => x.ContentId == userNote.ContentId && x.ContentType == userNote.ContentType && x.UserId == userNote.UserId).ToList();
        }

        public async Task<IEnumerable<UserNote>> GetNotes()
        {

            return await _ILibraryRepository.GetNotes();
        }

        public async Task RemoveNotes(long Id)
        {
            await _ILibraryRepository.RemoveNotes(Id);
        }
        #endregion

        public async Task AddReadEdgeTrialUser(ReadEdgeTrialUsers readEdgeTrialUsers)
        {
            await _prachiUser.AddReadEdgeTrialUsers(readEdgeTrialUsers);
        }

        public async Task<IEnumerable<ReadEdgeTrialUsers>> GetReadEdgeTrialUser()
        {
            return await _prachiUser.GetReadEdgeTrialUsers();
        }

   

       public List<ClassSubjects> GetSubjectByClassType(int Classtype)
        {
            return _ILibraryRepository.GetSubjectByClassType(Classtype);
        }


    }
}
