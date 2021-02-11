using PrachiIndia.Portal.Helpers;
using PrachiIndia.Portal.Models;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace PrachiIndia.Portal.Framework
{
    public static class BookType
    {
        public const int PBook = 1;
        public const int EBook = 2;
        public const int Both = 3;
    }
    public static class PGType
    {
        public static string Domestic = "Domestic Card";
        public static string International = "International Card";

    }
    public class EbookHelper
    {
        public string CreateConfiguration(string userId, string RequestID, string UserType = "",string ConfigurationType="")
        {
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var schoolId = context.EbookOrders.First(t => t.RequestID.ToLower() == RequestID.ToLower()).UserID;
            Stopwatch swconfig = new Stopwatch();
            swconfig.Start();
            var results = GetConfiguration(userId, RequestID, UserType, schoolId, ConfigurationType);
            swconfig.Stop();
            var config = swconfig.Elapsed;
            var xmlResult = Utility.ConvertToXML(results);
            // var EncXMl = Encryption.EncryptCommon(xmlResult, schoolId);

           var EncXMl = Encryption.EncryptCommon(xmlResult);
            return EncXMl;
        }

        public UserConfiguration GetConfiguration(string userId, string RequestID, string UserType = "", string schoolId = "", string ConfigurationType = "")
        {
            var moduleFilePath = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            var imagePath = string.Format("{0}ModuleFiles/Items/", moduleFilePath);
            var epubPath = string.Format("{0}ModuleFiles/Epub/", moduleFilePath);
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            Stopwatch swuserid = new Stopwatch();
            swuserid.Start();
            if (UserType != "")
            {
                userId = context.USP_GetUsreIDBySaleMarketing(UserType, RequestID).FirstOrDefault();
            }
            swuserid.Stop();
            var userid= swuserid.Elapsed;

            Stopwatch swconfigurebook = new Stopwatch();
            swconfigurebook.Start();

            List<long?> bookids = context.USP_GetConfiguredBooks(RequestID).ToList();

            swconfigurebook.Stop();
            var getconfiguredbook = swconfigurebook.Elapsed;
            Stopwatch switems = new Stopwatch();
            switems.Start();
            var items = context.UserLibraries.Where(t => t.UserId == userId && bookids.Contains(t.BookId)).ToList();
            switems.Stop();
            var itemss = switems.Elapsed;
            var results = new List<UserLibraryModel>();
            if (results == null)
                return null;
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
                    Result = Portal.Models.ResponseStatus.Succeeded.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    //Chapters = GetChapters(item.BookId, context)
                };
                results.Add(reader);
            }

            var bookIds = (from item in results
                           select item.BookId).ToList();
            var userData = context.AspNetUsers.First(t => t.Id == schoolId);
            var usermodel = new UserLogin
            {
                AccessToken = "N/A",
                Expires = DateTime.Now.AddDays(365),
                Id = Guid.NewGuid().ToString(),
                Issued = DateTime.Now,
                ExpiresIn = "365",
                Name = userData.FirstName,
                Password = userData.PasswordHash,
                UserId = userData.Id,
                TokenType = "N/A",
                UserName = userData.UserName,
                UserRoles = Roles.School,
            };
            var aspNetUser = new AspNetUserModel
            {
                AboutMe = userData.AboutMe,
                Address = userData.Address,
                City = userData.City,
                CompleteName = userData.FirstName + " " + userData.LastName,
                Country = userData.Country,
                Designation = userData.Designation,
                DlNo = userData.DlNo,
                Email = userData.Email,
                FirstName = userData.FirstName,
                Id = userData.Id,
                LastName = userData.LastName,
                Organization = userData.Organization,
                PANId = userData.PANId,
                PhoneNumber = userData.PhoneNumber,
                PinCode = userData.PinCode,
                Profession = userData.Profession,
                ProfileImage = userData.ProfileImage,
                State = userData.State,
                Status = userData.Status,
                UserName = userData.UserName,
                Industry = userData.Industry,
                IsVerified = userData.IsVerified,
                PassportNo = userData.PassportNo,
                Remark = userData.Remark,
            };
            Stopwatch swuserconfig = new Stopwatch();
            swuserconfig.Start();

            UserConfiguration UserConfiguration = new UserConfiguration();
            if (ConfigurationType.ToLower() == "testhour")
            {
                UserConfiguration = new UserConfiguration
                {
                    Libraries = results,
                    Chapters = GetChapters(bookIds, context),
                    Questions = GetQuestions(bookIds, context),
                    Answers = GetAnswers(),
                    UserLogin = usermodel,
                    AspNetUser = aspNetUser,
                   // ChapterContents = GetChapterContents(bookIds, context),
                    Topics = GetTopics()
                };
            }
            else {
                UserConfiguration = new UserConfiguration
                {
                    Libraries = results,
                    Chapters = GetChapters(bookIds, context),
                    ChapterContents = GetChapterContents(bookIds, context),
                    UserLogin = usermodel,
                    AspNetUser = aspNetUser,
                };
            }
            swuserconfig.Stop();
            var userconfig= swuserconfig.Elapsed;
            return UserConfiguration;
        }

        List<ChapterModel> GetChapters(List<long> bookIds, PrachiIndia.Sql.dbPrachiIndia_PortalEntities context)
        {
            return (from item in context.Chapters.Where(t => bookIds.Contains(t.BookId ?? 0) && t.Status == 1).OrderBy(t => t.ChapterIndex).ToList()
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
                        //ChapterContents = GetChapterContents(item.Id, context)
                    }).ToList();
        }
        //public List<ChapterContentModel> GetChapterContents(long chapterId, PrachiIndia.Sql.dbPrachiIndia_PortalEntities context)
        //{
        //    return (from item in context.ChapterContents.Where(t => t.ChapterId == chapterId && t.Status == 1).OrderBy(t => t.OrderId)
        //            select new ChapterContentModel
        //            {
        //                BookId = item.BookId ?? 0,
        //                ChapterId = item.ChapterId ?? 0,
        //                ContentType = item.ContentType ?? 0,
        //                Desc = item.Desc,
        //                Id = item.Id,
        //                Name = item.Name,
        //                OrderId = item.OrderId ?? 0,
        //                Status = item.Status ?? 0,
        //                Title = item.Title,
        //                Type = item.Type ?? 0
        //            }).ToList();
        //}
        public List<ChapterContentModel> GetChapterContents(List<long> bookIds, PrachiIndia.Sql.dbPrachiIndia_PortalEntities context)
        {
            var contents = new List<ChapterContentModel>();
            //foreach (var bookId in bookIds)
            //{
            //var chapterIds = (from item in context.Chapters.Where(t => bookId == t.BookId && t.Status == 1).OrderBy(t => t.ChapterIndex).ToList()
            //                  select item.Id).ToList();

            //var chptersContents = (from item in context.ChapterContents.Where(t => bookId == t.BookId && t.Status == 1 && chapterIds.Contains(t.ChapterId ?? 0)).OrderBy(t => t.OrderId)
            //                       select new ChapterContentModel
            //                       {
            //                           BookId = item.BookId ?? 0,
            //                           ChapterId = item.ChapterId ?? 0,
            //                           ContentType = item.ContentType ?? 0,
            //                           Desc = item.Desc,
            //                           Id = item.Id,
            //                           Name = item.Name,
            //                           OrderId = item.OrderId ?? 0,
            //                           Status = item.Status ?? 0,
            //                           Title = item.Title,
            //                           Type = item.Type ?? 0
            //                       }).ToList();
            //contents.AddRange(chptersContents);
            var configurationManager = Convert.ToString(ConfigurationManager.ConnectionStrings["DefaultConnection"]);
           var dt= GetDataTable(bookIds);
            using (SqlConnection conn=new SqlConnection(configurationManager))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("USP_GETChapterContentByBookID", conn);
                //SqlCommand cmd = new SqlCommand("USP_GETChapterContentByBookID", conn);
                dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddWithValue("@BookIds", dt);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);
                var result= dataset.Tables[0].AsEnumerable()
                                               .Select(dataRow => new ChapterContentModel
                                               {
                                                   BookId = Convert.ToInt32(dataRow["BookId"]),
                                                   ChapterId= Convert.ToInt32(dataRow["ChapterId"]),
                                                   ContentType = Convert.ToInt32(dataRow["ContentType"]),
                                                   Desc = Convert.ToString(dataRow["Desc"]),
                                                   Id = Convert.ToInt32(dataRow["Id"]),
                                                   Name = Convert.ToString(dataRow["Name"]),
                                                   OrderId = Convert.ToInt32(dataRow["OrderId"]),
                                                   Status = Convert.ToInt32(dataRow["Status"]),
                                                   Title = Convert.ToString(dataRow["Title"]),
                                                   Type = Convert.ToInt32(dataRow["Type"]),
                                               }).ToList();
           
            //}
            return result;
            }
        }



        public List<Questions> GetQuestions(List<long> bookIds, PrachiIndia.Sql.dbPrachiIndia_PortalEntities context)
        {
            var contents = new List<ChapterContentModel>();
            var configurationManager = Convert.ToString(ConfigurationManager.ConnectionStrings["DefaultConnection"]);
            var dt = GetDataTable(bookIds);
       
                using (SqlConnection conn = new SqlConnection(configurationManager))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter("USP_QuestionAndAnswer", conn);
                    //SqlCommand cmd = new SqlCommand("USP_GETChapterContentByBookID", conn);
                    dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@BookIds", dt);
                    DataSet dataset = new DataSet();
                    dataAdapter.Fill(dataset);
                    var result = dataset.Tables[0].AsEnumerable()
                                                   .Select(dataRow => new Questions
                                                   {
                                                       BookId = Convert.ToInt32(dataRow["IdBook"]),
                                                       ChapterId = Convert.ToInt32(dataRow["IdChapter"]),
                                                       Id = Convert.ToInt32(dataRow["Id"]),
                                                       CategoryId = Convert.ToInt32(dataRow["idQuestionType"]),
                                                       Question = Convert.ToString(dataRow["isQuestion"]),
                                                       Answer = Convert.ToString(dataRow["isAns"]),
                                                       Image = Convert.ToString(dataRow["isImage"]),
                                                       Extension = Convert.ToString(dataRow["isExtension"]),
                                                       AnsImage = Convert.ToString(dataRow["AnsImage"]),
                                                       AnsExtension = Convert.ToString(dataRow["AnsExtension"]),
                                                       Status = Convert.ToBoolean(dataRow["isStatus"]),
                                                       UpdatedDate = Convert.ToDateTime(dataRow["dtmUpdate"]),
                                                       Topic = Convert.ToInt32(dataRow["Topic"]),
                                                       Type = Convert.ToInt32(dataRow["Category"]),

                                                   }).ToList();
                    HttpContext.Current.Session.Remove("AnswerResult");
                    HttpContext.Current.Session.Remove("Topic");
                HttpContext.Current.Session["AnswerResult"] = dataset.Tables[1];
                HttpContext.Current.Session["Topic"] = dataset.Tables[2];

                return result;
                }
           

        }


        public List<Answers> GetAnswers()
        {
            List<Answers> Answers=new List<Answers>();
            //if (HttpContext.Current.Session["QuestionResult"] != null)
            //{
            //    questions = (List<Questions>)HttpContext.Current.Session["QuestionResult"];
            //    bookIds= questions.Select(x => x.Id).ToList();
            //}
            //var dt = GetDataTable(bookIds);
            //using (SqlConnection conn = new SqlConnection(configurationManager))
            //{
            //    SqlDataAdapter dataAdapter = new SqlDataAdapter("USP_GETAnswerByQuestion", conn);
            //    //SqlCommand cmd = new SqlCommand("USP_GETChapterContentByBookID", conn);
            //    dataAdapter.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //    dataAdapter.SelectCommand.Parameters.AddWithValue("@BookIds", dt);
            //    DataSet dataset = new DataSet();
            //    dataAdapter.Fill(dataset);
            //    var result = dataset.Tables[0].AsEnumerable()
            //                                   .Select(dataRow => new Answers
            //                                   {
            //                                       QuestionId = Convert.ToInt32(dataRow["IdQuestion"]),
            //                                       Option = Convert.ToString(dataRow["isOption"]),
            //                                       Id = Convert.ToInt32(dataRow["Id"]),
            //                                       Ans= Convert.ToString(dataRow["isAns"]),
            //                                       Image= Convert.ToString(dataRow["isImage"]),
            //                                       Extension= Convert.ToString(dataRow["isExtension"]),
            //                                       Status= Convert.ToBoolean(dataRow["isStatus"]),
            //                                       UpdatedDate= Convert.ToDateTime(dataRow["dtmUpdate"]),
            //                                       AnsExtension = Convert.ToString(dataRow["AnsExtension"]),
            //                                       AnsImage = Convert.ToString(dataRow["AnsImage"]),
            //                                       ChildOption = Convert.ToString(dataRow["isChildOption"])
            //                                   }).ToList();


            //    return result;
            //}
            if (HttpContext.Current.Session["AnswerResult"] != null)
            {
                DataTable dtAnswer = (DataTable)HttpContext.Current.Session["AnswerResult"];

                Answers=dtAnswer.AsEnumerable().Select(dataRow => new Answers
                {
                    QuestionId = Convert.ToInt32(dataRow["IdQuestion"]),
                    Option = Convert.ToString(dataRow["isOption"]),
                    Id = Convert.ToInt32(dataRow["Id"]),
                    Ans = Convert.ToString(dataRow["isAns"]),
                    Image = Convert.ToString(dataRow["isImage"]),
                    Extension = Convert.ToString(dataRow["isExtension"]),
                    Status = Convert.ToBoolean(dataRow["isStatus"]),
                    UpdatedDate = Convert.ToDateTime(dataRow["dtmUpdate"]),
                    AnsExtension = Convert.ToString(dataRow["AnsExtension"]),
                    AnsImage = Convert.ToString(dataRow["AnsImage"]),
                    ChildOption = Convert.ToString(dataRow["isChildOption"])
                }).ToList();
            }
            return Answers;
        }

        public List<Topics> GetTopics()
        {
            List<Topics> Topics = new List<Topics>();
            if (HttpContext.Current.Session["Topic"] != null)
            {
                DataTable dtTopic = (DataTable)HttpContext.Current.Session["Topic"];

                dtTopic.AsEnumerable().Select(dataRow => new Topics
                {
                    Id = Convert.ToInt32(dataRow["Id"]),
                    BookId= Convert.ToInt32(dataRow["BookID"]),
                    ChapterId= Convert.ToInt32(dataRow["ChapterID"]),
                    CreatedDate = Convert.ToDateTime(dataRow["UpdatedDate"]),
                    Topic = Convert.ToString(dataRow["Topic"])
                }).ToList();
            }
            return Topics;

        }
        public string CreateConfiguration(string userId, long OrderId)
        {
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var results = GetConfiguration(userId, OrderId);
            var xmlResult = Utility.ConvertToXML(results);
            // var EncXMl = Encryption.EncryptCommon(xmlResult, schoolId);
            var EncXMl = Encryption.EncryptCommon(xmlResult);
            return EncXMl;
        }
        public UserConfiguration GetConfiguration(string userId, long OrderId)
        {
            var moduleFilePath = ConfigurationManager.AppSettings["SiteUrl"].ToString();
            var imagePath = string.Format("{0}ModuleFiles/Items/", moduleFilePath);
            var epubPath = string.Format("{0}ModuleFiles/Epub/", moduleFilePath);
            var context = new PrachiIndia.Sql.dbPrachiIndia_PortalEntities();
            var books = context.OrderProducts.Where(t => t.OrderId == OrderId && t.BookType == Framework.BookType.EBook).ToList();
            List<long?> bookids = (from item in books
                                   select item.ItemId).Distinct().ToList();

            var items = context.UserLibraries.Where(t => t.UserId == userId && bookids.Contains(t.BookId)).ToList();
            var results = new List<UserLibraryModel>();
            if (results == null)
                return null;
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
                    Result = Portal.Models.ResponseStatus.Succeeded.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    //  Chapters = GetChapters(item.BookId, context)
                };
                results.Add(reader);
            }

            var bookIds = (from item in results
                           select item.BookId).ToList();
            var userData = context.AspNetUsers.First(t => t.Id == userId);
            var usermodel = new UserLogin
            {
                AccessToken = "N/A",
                Expires = DateTime.Now.AddDays(365),
                Id = Guid.NewGuid().ToString(),
                Issued = DateTime.Now,
                ExpiresIn = "365",
                Name = userData.FirstName,
                Password = userData.PasswordHash,
                UserId = userData.Id,
                TokenType = "N/A",
                UserName = userData.UserName,
                UserRoles = Roles.User,
            };
            var aspNetUser = new AspNetUserModel
            {
                AboutMe = userData.AboutMe,
                Address = userData.Address,
                City = userData.City,
                CompleteName = userData.FirstName + " " + userData.LastName,
                Country = userData.Country,
                Designation = userData.Designation,
                DlNo = userData.DlNo,
                Email = userData.Email,
                FirstName = userData.FirstName,
                Id = userData.Id,
                LastName = userData.LastName,
                Organization = userData.Organization,
                PANId = userData.PANId,
                PhoneNumber = userData.PhoneNumber,
                PinCode = userData.PinCode,
                Profession = userData.Profession,
                ProfileImage = userData.ProfileImage,
                State = userData.State,
                Status = userData.Status,
                UserName = userData.UserName,
                Industry = userData.Industry,
                IsVerified = userData.IsVerified,
                PassportNo = userData.PassportNo,
                Remark = userData.Remark,
            };
            var UserConfiguration = new UserConfiguration
            {
                Libraries = results,
                Chapters = GetChapters(bookIds, context),
                ChapterContents = GetChapterContents(bookIds, context),
                UserLogin = usermodel,
                AspNetUser = aspNetUser,
            };
            return UserConfiguration;
        }
        public DataTable GetDataTable(List<long> bookids)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("BookIds"));
            for (int num = 0; num < bookids.Count; num++)
            {
                foreach (long bookid in bookids)
                {
                    DataRow dataRow = dt.NewRow();
                    dataRow["BookIds"] = bookid;
                    dt.Rows.Add(dataRow);
                }
            }
            return dt;
        }
    }

    public class UserConfiguration
    {
        public UserConfiguration()
        {
            Libraries = new List<UserLibraryModel>();
            Chapters = new List<ChapterModel>();
            ChapterContents = new List<ChapterContentModel>();
        }
        public List<UserLibraryModel> Libraries { get; set; }
        public List<ChapterModel> Chapters { get; set; }
        public List<ChapterContentModel> ChapterContents { get; set; }
        public UserLogin UserLogin { get; set; }
        public AspNetUserModel AspNetUser { get; set; }
        public List<Questions> Questions { get; set; }
        public List<Answers> Answers { get; set; }
        public List<Topics> Topics { get; set; }

    }
  
}