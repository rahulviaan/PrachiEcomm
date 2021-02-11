using DAL;
using PrachiIndia.Portal.Models;
using PrachiIndia.Sql.CustomRepositories;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.BAL
{
    public class BookChapterBAL
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objBookChapter> lstObj;

        private objBookChapter obj;

        public List<objBookChapter> GetAll(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "SELECT tb1.[id],tb1.[isChapter],tb1.[idBook],tb1.[idSubject], tb1.[isType],tb1.[isFromPage],tb1.[isToPage],tb1.[idSeries],tb1.[isTitle], tb1.[isDesc],tb1.[dtmCreate],tb1.[dtmUpdate],tb1.[isStatus] , (select count(1) from tbl_books_plus tbp where tbp.idChapter=tb1.id)bp FROM  [tbl_books_chapters] tb1  order by tb1.isChapter asc";
            }
            else
            {
                this.query = "SELECT tb1.[id],tb1.[isChapter],tb1.[idBook],tb1.[idSubject], tb1.[isType],tb1.[isFromPage],tb1.[isToPage],tb1.[idSeries],tb1.[isTitle], tb1.[isDesc],tb1.[dtmCreate],tb1.[dtmUpdate],tb1.[isStatus] , (select count(1) from tbl_books_plus tbp where tbp.idChapter=tb1.id)bp FROM  [tbl_books_chapters] tb1 where     tb1.isStatus=" + (int)act + "  order by tb1.isChapter asc";
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objBookChapter>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBookChapter
                {
                    id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString()),
                    idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["isTitle"]),
                    isDesc = Common_Static.ToSafeString(dataRow["isDesc"]),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    isType = (TextType)Common_Static.ToSafeInt(dataRow["isType"].ToString()),
                    isFromPage = Common_Static.ToSafeInt(dataRow["isFromPage"].ToString()),
                    isToPage = Common_Static.ToSafeInt(dataRow["isToPage"].ToString()),
                    isChapter = Common_Static.ToSafeInt(dataRow["isChapter"].ToString()),
                    countbp = Common_Static.ToSafeInt(dataRow["bp"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objBookChapter> GetAll(string idBook, IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "SELECT tb1.[id],tb1.[Bookid],tb1.[FromPage],tb1.[ToPage],tb1.[Title], tb1.[Description],tb1.[CreatedDate],tb1.[Updateddate],tb1.[Status] , (select count(1) from ChapterContent tbp where tbp.id=tb1.id)bp FROM  [Chapters] tb1 where   tb1.bookID in (" + idBook + ")  order by tb1.id asc";
            }
            else
            {
                this.query = string.Concat(new object[]
                {
                    "SELECT tb1.[id],tb1.[Bookid],tb1.[FromPage],tb1.[ToPage],tb1.[Title], tb1.[Description],tb1.[CreatedDate],tb1.[Updateddate],tb1.[Status] , (select count(1) from ChapterContent tbp where tbp.id=tb1.id)bp FROM  [Chapters] tb1 where   tb1.bookID in(",
                    idBook,
                    ") and tb1.Status=",
                    (int)act,
                    "  order by tb1.id asc"
                });
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objBookChapter>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBookChapter
                {
                    id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["bookid"].ToString()),
                    //idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    //idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["Title"]),
                    isDesc = Common_Static.ToSafeString(dataRow["Description"]),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["CreatedDate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["Updateddate"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["Status"].ToString()),
                    //isType = (TextType)Common_Static.ToSafeInt(dataRow["isType"].ToString()),
                    isFromPage = Common_Static.ToSafeInt(dataRow["FromPage"].ToString()),
                    isToPage = Common_Static.ToSafeInt(dataRow["ToPage"].ToString()),
                    isChapter = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    countbp = Common_Static.ToSafeInt(dataRow["bp"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public List<objBookChapter> GetAllByBook(string idBook, IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = string.Concat(new object[]
                {
                    "select tc.*,  ( select count(1) from ChapterContent \twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id )bp,  ( \tselect count(1) LP from ChapterContent \twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    1,
                    " )LP,(\tselect count(1) WS from ChapterContent\twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    2,
                    "  )WS, ( \tselect count(1) mm from ChapterContent\twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    3,
                    "  )mm, (\tselect count(1) SL from ChapterContent\twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    4,
                    "  )SL, (\tselect count(1) from tbl_Questions where idBook=",
                    idBook,
                    " and idChapter=tc.id  )Qu from [Chapters] tc where bookid=",
                    idBook,
                    " order by id asc"
                });
            }
            else
            {
                this.query = string.Concat(new object[]
                {
                    "select tc.*,  ( select count(1) from ChapterContent \twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id )bp , ( \tselect count(1) LP from ChapterContent \twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    1,
                    " )LP,(\tselect count(1) WS from ChapterContent\twhere idbook=",
                    idBook,
                       " and Chapterid=tc.id  and type=",
                    2,
                    "  )WS, ( \tselect count(1) mm from ChapterContent\twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    3,
                    "  )mm, (\tselect count(1) SL from ChapterContent\twhere bookid=",
                    idBook,
                    " and Chapterid=tc.id  and type=",
                    4,
                    "  )SL, (\tselect count(1) from tbl_Questions where idBook=",
                    idBook,
                    " and idChapter=tc.id  )Qu from Chapters tc where bookid=",
                    idBook,
                    " and Status=",
                    (int)act,
                    " order by isChapter asc"
                });
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objBookChapter>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBookChapter
                {
                    id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["Bookid"].ToString()),
                    //idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    //idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["Title"]),
                    isDesc = Common_Static.ToSafeString(dataRow["Description"]),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["CreatedDate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["UpdatedDate"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["Status"].ToString()),
                    //isType = (TextType)Common_Static.ToSafeInt(dataRow["Type"].ToString()),
                    isFromPage = Common_Static.ToSafeInt(dataRow["FromPage"].ToString()),
                    isToPage = Common_Static.ToSafeInt(dataRow["ToPage"].ToString()),
                    isChapter = Common_Static.ToSafeInt(dataRow["Chapterindex"].ToString()),
                    countbp = Common_Static.ToSafeInt(dataRow["bp"].ToString()),
                    lp = Common_Static.ToSafeInt(dataRow["lp"].ToString()),
                    ws = Common_Static.ToSafeInt(dataRow["ws"].ToString()),
                    sl = Common_Static.ToSafeInt(dataRow["sl"].ToString()),
                    mm = Common_Static.ToSafeInt(dataRow["mm"].ToString()),
                    qu = Common_Static.ToSafeInt(dataRow["qu"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public object Get(int id)
        {
            this.query = "SELECT tb1.[id],tb1.[BookId],tb1.[FromPage],tb1.[ToPage],tb1.[Title], tb1.[Description],tb1.[CreatedDate],tb1.[Updateddate],tb1.[Status] , (select count(1) from ChapterContent tbp where tbp.Chapterid=tb1.id)bp FROM  [Chapters] tb1 where   tb1.id=" + id + " ";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBookChapter
                {
                    id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["Bookid"].ToString()),
                    //idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    //idSeries = Common_Static.ToSafeInt(dataRow["Series"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["Title"]),
                    isDesc = Common_Static.ToSafeString(dataRow["Description"]),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["CreatedDate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["Updateddate"].ToString()),
                    //isType = (TextType)Common_Static.ToSafeInt(dataRow["isType"].ToString()),
                    isFromPage = Common_Static.ToSafeInt(dataRow["FromPage"].ToString()),
                    isToPage = Common_Static.ToSafeInt(dataRow["ToPage"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["Status"].ToString()),
                    isChapter = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    countbp = Common_Static.ToSafeInt(dataRow["bp"].ToString())
                };
            }
            return this.obj;
        }

        public int Add(object item)
        {
            int returnValue = 0;
            var chapterRepository = new ChapterRepository();
            objBookChapter obj = (objBookChapter)item;

            if (obj.id > 0)
            {
                var subject = chapterRepository.GetByIdAsync(obj.id);
                subject.BookId = obj.idBook;
                subject.ChapterIndex = (short)obj.isChapter;
                subject.Description = obj.isDesc;
                subject.FromPage = (short)obj.isFromPage;
                subject.Id = obj.id;
                subject.Title = obj.isTitle;
                subject.ToPage = (short)obj.isToPage;
                subject.Updateddate = DateTime.Now;
                chapterRepository.UpdateAsync(subject);
                returnValue = 1;
            }
            else
            {
                var subject = new Sql.Chapter
                {
                    Description = obj.isDesc,
                    Title = obj.isTitle,
                    BookId = obj.idBook,
                    ChapterIndex = (short)obj.isChapter,
                    CreatedDate = DateTime.Now,
                    FromPage = (short)obj.isFromPage,
                    Id = obj.id,
                    ToPage = (short)obj.isToPage,
                    Updateddate = DateTime.Now,
                    Status = 1

                };
                chapterRepository.CreateAsync(subject);
                returnValue = 1;
            }
            return returnValue;
        }

        public bool CheckDuplicate(objBookChapter obj)
        {
            int num;
            int.TryParse(obj.id.ToString(), out num);
            Common_Static.CleanObjet(obj);
            string sQL = string.Concat(new object[]
            {
                "SELECT count(1) from tbl_books_chapters where  idBook=",
                obj.idBook,
                " and  isTitle='",
                obj.isTitle,
                "' and  id <>",
                num
            });
            this.dbutil = new DBUtility();
            DataTable dataTable = new DataTable();
            dataTable = this.dbutil.getDataTable(sQL);
            return Convert.ToInt32(dataTable.Rows[0][0]) <= 0;
        }

        public bool Update(object item)
        {
            if (item != null)
            {
                this.obj = (objBookChapter)item;
                this.query = string.Concat(new object[]
                {
                    "update tbl_books_chapters set isChapter=",
                    this.obj.isChapter,
                    ", isTitle='",
                    this.obj.isTitle,
                    "',isDesc='",
                    this.obj.isDesc,
                    "' , isType=",
                    (int)this.obj.isType,
                    ",isFromPage=",
                    this.obj.isFromPage,
                    ",isToPage=",
                    this.obj.isToPage,
                    ",  dtmUpdate=getdate() where id=",
                    this.obj.id.ToString()
                });
                this.com = new SqlCommand();
                this.com.CommandText = this.query;
                this.dbutil = new DBUtility();
                this.dbutil.Execute(this.com);
                return true;
            }
            return false;
        }

        public int UpdateStatus(int id)
        {
            string commandText = "update tbl_books_chapters set isStatus= case isStatus when 1 then 0 when 0 then 1 end,dtmUpdate=getdate() where id=" + id;
            this.com = new SqlCommand();
            this.com.CommandText = commandText;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com);
        }

        public int DeleteChapter(int id)
        {
            string commandText = "delete from tbl_books_chapters where id=" + id;
            this.com = new SqlCommand();
            this.com.CommandText = commandText;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com);
        }
    }
}