using DAL;
using PrachiIndia.Portal.Models;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.BAL
{
    public class BookBal: IRepositry
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objBook> lstObj;

        private objBook obj;

        public IEnumerable<object> GetAll()
        {
            return this.GetAll(IsValid.All);
        }

        public IEnumerable<objBook> GetAllSC(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "select [id],[is_size],[subject_id],[series_id],[board_id],[class_id],[title] ,[price],[description],[image_path],[img_extension],[no_of_pages],[author],[isbn],[edition],[shortcut],[ebook],[suppleymentary],[_print],[audio],[multimedia],[created_date],[modified_date],[is_active] from tbl_books  where coalesce(image_path,'')='' or coalesce(shortcut,'')='' or coalesce(is_size,'N/A')='N/A' order by title asc";
            }
            else
            {
                this.query = "select [id],[is_size],[subject_id],[series_id],[board_id],[class_id],[title] ,[price],[description],[image_path],[img_extension],[no_of_pages],[author],[isbn],[edition],[shortcut],[ebook],[suppleymentary],[_print],[audio],[multimedia],[created_date],[modified_date],[is_active] from tbl_books where is_active=" + (int)act + "  and ( coalesce(image_path,'')='' or coalesce(shortcut,'')='' or coalesce(is_size,'N/A')='N/A')   order by title asc";
            }
            this.lstObj = new List<objBook>();
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            HttpContext arg_59_0 = HttpContext.Current;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBook
                {
                    Id = Common_Static.ToSafeInt(dataRow["Id"].ToString()),
                    subject_id = Common_Static.ToSafeInt(dataRow["subject_id"].ToString()),
                    series_id = Common_Static.ToSafeInt(dataRow["series_id"].ToString()),
                    board_id = Common_Static.ToSafeString(dataRow["board_id"].ToString()),
                    class_id = Common_Static.ToSafeString(dataRow["class_id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"].ToString()),
                    price = Common_Static.ToSafeString(dataRow["price"].ToString()),
                    description = Common_Static.ToSafeString(dataRow["description"].ToString()),
                    image_path = Common_Static.ToSafeString(dataRow["image_path"].ToString()),
                    img_extension = (contentType)Common_Static.ToSafeInt(dataRow["img_extension"].ToString()),
                    no_of_pages = Common_Static.ToSafeString(dataRow["no_of_pages"].ToString()),
                    author = Common_Static.ToSafeString(dataRow["author"].ToString()),
                    isbn = Common_Static.ToSafeString(dataRow["isbn"].ToString()),
                    edition = Common_Static.ToSafeString(dataRow["edition"].ToString()),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"].ToString()),
                    ebook = (bool)(dataRow["ebook"]),
                    suppleymentary = (_enBinary)Common_Static.ToSafeInt(dataRow["suppleymentary"].ToString()),
                    _print = (_enBinary)Common_Static.ToSafeInt(dataRow["_print"].ToString()),
                    audio = (bool)(dataRow["audio"]),
                    multimedia = (_enBinary)Common_Static.ToSafeInt(dataRow["multimedia"].ToString()),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    sizeondisk = Common_Static.ToSafeString(dataRow["is_size"])
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objBook> GetAll(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "select [id],[is_size],[subject_id],[series_id],[board_id],[class_id],[title] ,[price],[description],[image_path],[img_extension],[no_of_pages],[author],[isbn],[edition],[shortcut],[ebook],[suppleymentary],[_print],[audio],[multimedia],[created_date],[modified_date],[is_active] from tbl_books     order by title asc";
            }
            else
            {
                this.query = "select [id],[is_size],[subject_id],[series_id],[board_id],[class_id],[title] ,[price],[description],[image_path],[img_extension],[no_of_pages],[author],[isbn],[edition],[shortcut],[ebook],[suppleymentary],[_print],[audio],[multimedia],[created_date],[modified_date],[is_active] from tbl_books where is_active=" + (int)act + "    order by title asc";
            }
            this.lstObj = new List<objBook>();
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            HttpContext arg_59_0 = HttpContext.Current;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBook
                {
                    Id = Common_Static.ToSafeInt(dataRow["Id"].ToString()),
                    subject_id = Common_Static.ToSafeInt(dataRow["subject_id"].ToString()),
                    series_id = Common_Static.ToSafeInt(dataRow["series_id"].ToString()),
                    board_id = Common_Static.ToSafeString(dataRow["board_id"].ToString()),
                    class_id = Common_Static.ToSafeString(dataRow["class_id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"].ToString()),
                    price = Common_Static.ToSafeString(dataRow["price"].ToString()),
                    description = Common_Static.ToSafeString(dataRow["description"].ToString()),
                    image_path = Common_Static.ToSafeString(dataRow["image_path"].ToString()),
                    img_extension = (contentType)(dataRow["img_extension"]),
                    no_of_pages = Common_Static.ToSafeString(dataRow["no_of_pages"].ToString()),
                    author = Common_Static.ToSafeString(dataRow["author"].ToString()),
                    isbn = Common_Static.ToSafeString(dataRow["isbn"].ToString()),
                    edition = Common_Static.ToSafeString(dataRow["edition"].ToString()),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"].ToString()),
                    ebook = (bool)(dataRow["ebook"]),
                    suppleymentary = (_enBinary)Common_Static.ToSafeInt(dataRow["suppleymentary"].ToString()),
                    _print = (_enBinary)Common_Static.ToSafeInt(dataRow["_print"].ToString()),
                    audio = (bool)(dataRow["audio"]),
                    multimedia = (_enBinary)Common_Static.ToSafeInt(dataRow["multimedia"].ToString()),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    sizeondisk = Common_Static.ToSafeString(dataRow["is_size"])
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objBook> GetAllBooksHaveQuestion(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "select [id],IsNULL([issize],'')as issize,[subjectid],[seriesid],[boardid],[classid],  title +' ['+(select Title from  masterclass where id= tblcatalog.Classid and Status=1)+']' [title]   ,[price],ISNULL([description],'') as description,ISNULL([image],'') as image, ISNULL([PageCount],0) as PageCount,[author],ISNULL([isbn],'') as isbn,ISNULL([edition],'') as edition,ISNULL([ebook],'') as ebook,ISNULL([audio],'') as audio,ISNULL([multimedia],'') as multimedia, ISNULL([dtmAdd],'') as dtmAdd,ISNULL([dtmUpdate],'') as dtmUpdate,[status] from tblcatalog  where id in(select distinct idBook from  tbl_Questions )";
            }
            else
            {
                this.query = "select [id],IsNULL([issize],'')as issize,[subjectid],[seriesid],[boardid],[classid],  title +' ['+(select Title from  masterclass where id= tblcatalog.Classid and Status=1)  +']' [title]   ,[price],ISNULL([description],'') as description,ISNULL([image],'') as image, ISNULL([PageCount],0) as PageCount,[author],ISNULL([isbn],'') as isbn,ISNULL([edition],'') as edition,ISNULL([ebook],'') as ebook,ISNULL([audio],'') as audio,ISNULL([multimedia],'') as multimedia, ISNULL([dtmAdd],'') as dtmAdd,ISNULL([dtmUpdate],'') as dtmUpdate,[status] from tblcatalog  where id in(select distinct idBook from  tbl_Questions ) and   status= " + (int)act;
            }
            this.query += " order by title asc";
            this.lstObj = new List<objBook>();
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBook
                {
                    Id = Common_Static.ToSafeInt(dataRow["Id"].ToString()),
                    subject_id = Common_Static.ToSafeInt(dataRow["subjectid"].ToString()),
                    series_id = Common_Static.ToSafeInt(dataRow["seriesid"].ToString()),
                    board_id = Common_Static.ToSafeString(dataRow["boardid"].ToString()),
                    class_id = Common_Static.ToSafeString(dataRow["classid"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"].ToString()),
                    price = Common_Static.ToSafeString(dataRow["price"].ToString()),
                    description = Common_Static.ToSafeString(dataRow["description"].ToString()),
                    image_path = Common_Static.ToSafeString(dataRow["image"].ToString()),
                    //img_extension = (contentType)Common_Static.ToSafeInt(dataRow["img_extension"].ToString()),
                    no_of_pages = Common_Static.ToSafeString(dataRow["PageCount"].ToString()),
                    author = Common_Static.ToSafeString(dataRow["author"].ToString()),
                    isbn = Common_Static.ToSafeString(dataRow["isbn"].ToString()),
                    edition = Common_Static.ToSafeString(dataRow["edition"].ToString()),
                    shortcut = Common_Static.ToSafeString(dataRow["image"].ToString()),
                    ebook = Convert.ToBoolean((dataRow["ebook"])),
                    //suppleymentary = (_enBinary)Common_Static.ToSafeInt(dataRow["suppleymentary"].ToString()),
                    //_print = (_enBinary)Common_Static.ToSafeInt(dataRow["_print"].ToString()),
                    audio = Convert.ToBoolean((dataRow["audio"])),
                    multimedia = (_enBinary)Common_Static.ToSafeInt(dataRow["multimedia"].ToString()),
                    created_date = Common_Static.ToSafeDate(dataRow["dtmAdd"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["status"].ToString()),
                    sizeondisk = Common_Static.ToSafeString(dataRow["issize"])
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objBook> GetAllBySeries(string series_id = "-1", IsValid act = IsValid.All, string idBooks = "")
        {
            if (series_id != "")
            {
                this.query = "select [isSize], [id],coalesce([subjectid],0)[subject_id],coalesce([seriesid],0)[series_id],coalesce([boardid],'')[board_id],coalesce([classid],'')[class_id],tb1.title +'-'+(Select Title from  [dbo].[MasterClass] where id=tb1.classid)[title] ,coalesce([price],0)[price],coalesce([description],'')[description], coalesce([image],'')[image_path],coalesce([PageCount],0)[no_of_pages], coalesce([author],'')[author], coalesce([isbn],'')[isbn],coalesce([edition],'')[edition],coalesce([Title],'')[shortcut],coalesce([ebook],'')[ebook],coalesce([audio],'')[audio],coalesce([multimedia],'')[multimedia], coalesce([dtmAdd],'')[created_date], coalesce([dtmupdate],'')[modified_date],coalesce([status],0)[is_active] ";
                this.query = "select coalesce(tb1.EncriptionKey,'') isEncKey,coalesce(tb1.[isSize],'N/A') is_size, tb1.[id],coalesce(tb1.[subjectid],0)[subject_id],coalesce(tb1.[seriesid],0)[series_id],coalesce(tb1.[boardid],'')[board_id],   coalesce(tb1.[classid],'')[class_id],tb1.title +'-'+(Select Title from  [dbo].[MasterClass] where id=tb1.classid)[title] ,coalesce(tb1.[price],0)[price], coalesce(tb1.[image],'')[image_path],coalesce(tb1.[PageCount],0)[no_of_pages],  coalesce(tb1.[author],'') [author], coalesce(tb1.[isbn],'') [isbn],coalesce(tb1.[edition],'') [edition],coalesce(tb1.[Title],'')[shortcut], coalesce(tb1.[ebook],'') [ebook],coalesce(tb1.[audio],'')[audio], coalesce(tb1.[multimedia],'') [multimedia], coalesce(tb1.[dtmAdd],'') [created_date], coalesce(tb1.[dtmupdate],'')[modified_date], coalesce(tb1.[status],0) [is_active], coalesce(ms.title,'')subject, coalesce(mss.title,'')series from tblCataLog tb1 left join MasterSubject ms on tb1.subjectid=ms.id left join MasterSeries mss on tb1.seriesid=mss.id";
                if (series_id == "-1")
                {
                    this.query += "   where 1=1  ";
                }
                else
                {
                    this.query = this.query + " where tb1.seriesid=" + series_id + "  ";
                }
                if (act != IsValid.All)
                {
                    object obj = this.query;
                    this.query = string.Concat(new object[]
                    {
                        obj,
                        " and tb1.status=",
                        (int)act,
                        " "
                    });
                }
                if (idBooks != "")
                {
                    this.query = this.query + " and tb1.id not in " + idBooks;
                }
                this.query += " order by tb1.title asc";
                this.dbutil = new DBUtility();
                DataTable dataTable = this.dbutil.getDataTable(this.query);
                this.lstObj = new List<objBook>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    this.obj = new objBook
                    {
                        Id = Common_Static.ToSafeInt(dataRow["Id"].ToString()),
                        subject = Common_Static.ToSafeString(dataRow["subject"].ToString()),
                        series = Common_Static.ToSafeString(dataRow["series"].ToString()),
                        isEncKey = Common_Static.ToSafeString(dataRow["isEncKey"].ToString()),
                        subject_id = Common_Static.ToSafeInt(dataRow["subject_id"].ToString()),
                        series_id = Common_Static.ToSafeInt(dataRow["series_id"].ToString()),
                        board_id = Common_Static.ToSafeString(dataRow["board_id"].ToString()),
                        class_id = Common_Static.ToSafeString(dataRow["class_id"].ToString()),
                        title = Common_Static.ToSafeString(dataRow["title"].ToString()),
                        price = Common_Static.ToSafeString(dataRow["price"].ToString()),
                        description = Common_Static.ToSafeString(dataRow["title"].ToString()),
                        image_path = Common_Static.ToSafeString(dataRow["image_path"].ToString()),
                        //img_extension = (contentType)Common_Static.ToSafeInt(dataRow["img_extension"]),
                        no_of_pages = Common_Static.ToSafeString(dataRow["no_of_pages"].ToString()),
                        author = Common_Static.ToSafeString(dataRow["author"].ToString()),
                        isbn = Common_Static.ToSafeString(dataRow["isbn"].ToString()),
                        edition = Common_Static.ToSafeString(dataRow["edition"].ToString()),
                        shortcut = Common_Static.ToSafeString(dataRow["shortcut"].ToString()),
                        ebook = Convert.ToBoolean(dataRow["ebook"]),
                        //suppleymentary = (_enBinary)Common_Static.ToSafeInt(dataRow["suppleymentary"].ToString()),
                        //_print = (_enBinary)Common_Static.ToSafeInt(dataRow["_print"].ToString()),
                        audio = Convert.ToBoolean(dataRow["audio"]),
                        multimedia = (_enBinary)Common_Static.ToSafeInt(dataRow["multimedia"].ToString()),
                        created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                        modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                        is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                        sizeondisk = Common_Static.ToSafeString(dataRow["is_size"])
                    };
                    this.lstObj.Add(this.obj);
                }
                return this.lstObj;
            }
            this.lstObj = this.GetAll(IsValid.Active).ToList<objBook>();
            return this.lstObj;
        }

        public IEnumerable<objBook> GetAllBySubjectClass(string idSubject, string idClass)
        {
            this.query = "select [is_size], [id],coalesce([subject_id],0)[subject_id],coalesce([series_id],0)[series_id],coalesce([board_id],'')[board_id],  coalesce([class_id],'')[class_id],coalesce([title],'')[title] ,coalesce([price],0)[price],coalesce([description],'')[description], coalesce([image_path],'')[image_path], coalesce([img_extension],'')[img_extension],coalesce([no_of_pages],0)[no_of_pages], coalesce([author],'')[author], coalesce([isbn],'')[isbn],coalesce([edition],'')[edition],coalesce([shortcut],'')[shortcut],coalesce([ebook],'')[ebook],  coalesce([suppleymentary],'')[suppleymentary],coalesce([_print],'')[_print],coalesce([audio],'')[audio],coalesce([multimedia],'')[multimedia], coalesce([created_date],'')[created_date], coalesce([modified_date],'')[modified_date],coalesce([is_active],0)[is_active] ";
            this.query = string.Concat(new string[]
            {
                "select coalesce(tb1.[is_size],'N/A') is_size, tb1.[id],coalesce(tb1.[subject_id],0)[subject_id],coalesce(tb1.[series_id],0)[series_id],coalesce(tb1.[board_id],'')[board_id],   coalesce(tb1.[class_id],'')[class_id],coalesce(tb1.[title],'')[title] ,coalesce(tb1.[price],0)[price],coalesce(tb1.[description],'')[description], coalesce(tb1.[image_path],'')[image_path], coalesce(tb1.[img_extension],'')[img_extension],coalesce(tb1.[no_of_pages],0)[no_of_pages],  coalesce(tb1.[author],'') [author], coalesce(tb1.[isbn],'') [isbn],coalesce(tb1.[edition],'') [edition],coalesce(tb1.[shortcut],'')[shortcut], coalesce(tb1.[ebook],'') [ebook],  coalesce(tb1.[suppleymentary],'') [suppleymentary],coalesce(tb1.[_print],'')[_print],coalesce(tb1.[audio],'')[audio], coalesce(tb1.[multimedia],'') [multimedia], coalesce(tb1.[created_date],'') [created_date], coalesce(tb1.[modified_date],'')[modified_date], coalesce(tb1.[is_active],0) [is_active], coalesce(ms.title,'')subject, coalesce(mss.title,'')series from tbl_books tb1 left join mst_Subject ms on tb1.subject_id=ms.id left join mst_Series mss on tb1.series_id=mss.id where tb1.subject_id=",
                idSubject,
                " and class_id like'%",
                idClass,
                "%'order by tb1.title asc"
            });
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objBook>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string[] array = Common_Static.ToSafeString(dataRow["class_id"].ToString()).Split(new char[]
                {
                    ','
                });
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    if (idClass.Trim() == text.Trim())
                    {
                        this.obj = new objBook
                        {
                            Id = Common_Static.ToSafeInt(dataRow["Id"].ToString()),
                            subject = Common_Static.ToSafeString(dataRow["subject"].ToString()),
                            series = Common_Static.ToSafeString(dataRow["series"].ToString()),
                            subject_id = Common_Static.ToSafeInt(dataRow["subject_id"].ToString()),
                            series_id = Common_Static.ToSafeInt(dataRow["series_id"].ToString()),
                            board_id = Common_Static.ToSafeString(dataRow["board_id"].ToString()),
                            class_id = Common_Static.ToSafeString(dataRow["class_id"].ToString()),
                            title = Common_Static.ToSafeString(dataRow["title"].ToString()),
                            price = Common_Static.ToSafeString(dataRow["price"].ToString()),
                            description = Common_Static.ToSafeString(dataRow["description"].ToString()),
                            image_path = Common_Static.ToSafeString(dataRow["image_path"].ToString()),
                            img_extension = (contentType)Common_Static.ToSafeInt(dataRow["img_extension"].ToString()),
                            no_of_pages = Common_Static.ToSafeString(dataRow["no_of_pages"].ToString()),
                            author = Common_Static.ToSafeString(dataRow["author"].ToString()),
                            isbn = Common_Static.ToSafeString(dataRow["isbn"].ToString()),
                            edition = Common_Static.ToSafeString(dataRow["edition"].ToString()),
                            shortcut = Common_Static.ToSafeString(dataRow["shortcut"].ToString()),
                            ebook = (bool)(dataRow["ebook"]),
                            suppleymentary = (_enBinary)Common_Static.ToSafeInt(dataRow["suppleymentary"].ToString()),
                            _print = (_enBinary)Common_Static.ToSafeInt(dataRow["_print"].ToString()),
                            audio = (bool)(dataRow["audio"]),
                            multimedia = (_enBinary)Common_Static.ToSafeInt(dataRow["multimedia"].ToString()),
                            created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                            modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                            is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                            sizeondisk = Common_Static.ToSafeString(dataRow["is_size"])
                        };
                        this.lstObj.Add(this.obj);
                        break;
                    }
                }
            }
            return this.lstObj;
        }
        public IEnumerable<objBook> GetAllBookHaveImageAsQuestion()
        {
            this.query = " select distinct idBook,(select title from tbl_Books where id=idBook) title from tbl_Questions where coalesce(isExtension,'')!='' order by title asc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objBook>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBook
                {
                    Id = Common_Static.ToSafeInt(dataRow["idBook"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }
        public object Get(int id)
        {
            this.query = " select mst1.title subject, mst2.title series,coalesce(tb1.EncriptionKey,'')isEncKey,tb1.[id],coalesce(tb1.[subjectid],0) [subject_id],coalesce(tb1.[seriesid],0) [series_id], ";
            this.query += " coalesce(tb1.[boardid],'')[board_id],coalesce(tb1.[classid],'')[class_id],coalesce(tb1.[title],'')[title] , ";
            this.query += " coalesce(tb1.[issize],'N/A')[is_size],  ";
            this.query += " coalesce(tb1.[price],0)[price],coalesce(tb1.[description],'')[description],coalesce(tb1.[image],'')[image_path],  ";
            this.query += " coalesce(tb1.[PageCount],0)[no_of_pages], coalesce(tb1.[author],'')[author], ";
            this.query += " coalesce(tb1.[isbn],'')[isbn],coalesce(tb1.[edition],'')[edition],coalesce(tb1.[Title],'')[shortcut], ";
            this.query += " coalesce(tb1.[ebook],'')[ebook],coalesce(tb1.[audio],'') ";
            this.query += " [audio],coalesce(tb1.[multimedia],'')[multimedia],coalesce(tb1.[dtmadd],'')[created_date], coalesce(tb1.[dtmUpdate],'')[modified_date], ";
            this.query += " coalesce(tb1.[status],0)[is_active]  ";
            this.query += " from tblCataLog tb1 ";
            this.query += " left join  mastersubject mst1 ";
            this.query += " on tb1.[subjectid]=mst1.id ";
            this.query += " left join  masterseries mst2 ";
            this.query += " on tb1.[seriesid]=mst2.id ";
            this.query = this.query + " where  tb1.id=" + id.ToString();
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            // HttpContext arg_168_0 = HttpContext.Current;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBook
                {
                    Id = Common_Static.ToSafeInt(dataRow["Id"].ToString()),
                    subject_id = Common_Static.ToSafeInt(dataRow["subject_id"].ToString()),
                    series_id = Common_Static.ToSafeInt(dataRow["series_id"].ToString()),
                    subject = Common_Static.ToSafeString(dataRow["subject"].ToString()),
                    series = Common_Static.ToSafeString(dataRow["series"].ToString()),
                    board_id = ((Common_Static.ToSafeString(dataRow["board_id"].ToString()).Trim() == "") ? "0" : Common_Static.ToSafeString(dataRow["board_id"].ToString())),
                    class_id = ((Common_Static.ToSafeString(dataRow["class_id"].ToString()).Trim() == "") ? "0" : Common_Static.ToSafeString(dataRow["class_id"].ToString())),
                    title = Common_Static.ToSafeString(dataRow["title"].ToString()),
                    price = Common_Static.ToSafeString(dataRow["price"].ToString()),
                    description = Common_Static.ToSafeString(dataRow["description"].ToString()),
                    image_path = Common_Static.ToSafeString(dataRow["image_path"].ToString()),
                    //img_extension = (contentType)Common_Static.ToSafeInt(dataRow["img_extension"].ToString()),
                    no_of_pages = Common_Static.ToSafeString(dataRow["no_of_pages"].ToString()),
                    author = Common_Static.ToSafeString(dataRow["author"].ToString()),
                    isbn = Common_Static.ToSafeString(dataRow["isbn"].ToString()),
                    edition = Common_Static.ToSafeString(dataRow["edition"].ToString()),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"].ToString()),
                    ebook = Convert.ToBoolean((dataRow["ebook"])),
                    //suppleymentary = (_enBinary)Common_Static.ToSafeInt(dataRow["suppleymentary"].ToString()),
                    //_print = (_enBinary)Common_Static.ToSafeInt(dataRow["_print"].ToString()),
                    audio = Convert.ToBoolean((dataRow["audio"])),
                    multimedia = (_enBinary)Common_Static.ToSafeInt(dataRow["multimedia"].ToString()),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    isEncKey = Common_Static.ToSafeString(dataRow["isEncKey"].ToString()),
                    sizeondisk = Common_Static.ToSafeString(dataRow["is_size"]),
                };
            }
            this.obj = ((this.obj == null) ? new objBook() : this.obj);
            this.query = "  select id,title from MasterClass where id in(" + this.obj.class_id + ") order by title asc";
            DataTable dataTable2 = this.dbutil.getDataTable(this.query);
            foreach (DataRow dataRow2 in dataTable2.Rows)
            {
                objBook obj = this.obj;
                obj._class = obj._class + Common_Static.ToSafeString(dataRow2["title"].ToString()) + ",";
            }
            if (dataTable2.Rows.Count > 0)
            {
                this.obj._class = this.obj._class.Substring(0, this.obj._class.Length - 1);
            }
            this.query = "select id,title from MasterBoard where id in(" + this.obj.board_id + ") order by title asc";
            DataTable dataTable3 = this.dbutil.getDataTable(this.query);
            foreach (DataRow dataRow3 in dataTable3.Rows)
            {
                objBook obj = this.obj;
                obj.board = obj.board + Common_Static.ToSafeString(dataRow3["title"].ToString()) + ",";
            }
            if (this.obj.board != null && this.obj.board.Length > 0)
            {
                this.obj.board = this.obj.board.Substring(0, this.obj.board.Length - 1);
            }
            return this.obj;
        }

        public int Add(object item)
        {
            Common_Static.CleanObjet(item);
            this.obj = new objBook();
            this.obj = (objBook)item;
            int num = 0;
            if (item != null)
            {
                this.obj.shortcut = this.obj.title.Trim().Replace('&', ' ');
                this.obj.shortcut = this.obj.shortcut.Trim().Replace(' ', '_');
                this.obj.shortcut = this.obj.shortcut.Trim().Replace("_-_", "_");
                this.obj = (objBook)item;
                this.obj.is_active = IsValid.Active;
                if (this.CheckDuplicate(this.obj))
                {
                    if (this.obj.image_path.Trim() == "")
                    {
                        this.obj.image_path = this.obj.title;
                    }
                    if (this.obj.shortcut.Trim() == "")
                    {
                        this.obj.shortcut = this.obj.title;
                    }
                    if (this.obj.Id > 0)
                    {
                        if (this.Update(this.obj))
                        {
                            num = this.obj.Id;
                        }
                        else
                        {
                            num = 0;
                        }
                    }
                    else
                    {
                        this.query = string.Concat(new object[]
                        {
                            "INSERT INTO [tbl_books] ([subject_id],[series_id],[board_id],[class_id],[title] ,[price],[description],[image_path],[img_extension],[no_of_pages],[author],[isbn],[edition],[ebook],[suppleymentary],[_print],[audio],[multimedia]) VALUES(",
                            this.obj.subject_id,
                            ",",
                            this.obj.series_id,
                            ",'",
                            this.obj.board_id,
                            "','",
                            this.obj.class_id,
                            "','",
                            this.obj.title,
                            "' ,",
                            this.obj.price,
                            ",'",
                            this.obj.description,
                            "','",
                            this.obj.image_path,
                            "','",
                            this.obj.img_extension,
                            "',",
                            this.obj.no_of_pages,
                            ",'",
                            this.obj.author,
                            "','",
                            this.obj.isbn,
                            "','",
                            this.obj.edition,
                            "',",
                            this.obj.ebook,
                            ",",
                            (int)this.obj.suppleymentary,
                            ",",
                            (int)this.obj._print,
                            ",",
                            this.obj.audio,
                            ",",
                            (int)this.obj.multimedia,
                            ")"
                        });
                        this.com = new SqlCommand();
                        this.com.CommandText = this.query;
                        this.dbutil = new DBUtility();
                        num = this.dbutil.Execute(this.com);
                        if (num > 0)
                        {
                            this.query = string.Concat(new string[]
                            {
                                "select id from [tbl_books] where  [isbn]='",
                                this.obj.isbn,
                                "' and title='",
                                this.obj.title,
                                "'"
                            });
                            DataTable dataTable = new DataTable();
                            dataTable = this.dbutil.getDataTable(this.query);
                            num = Convert.ToInt32(dataTable.Rows[0]["id"].ToString());
                        }
                        else
                        {
                            num = 0;
                        }
                    }
                }
            }
            else
            {
                num = 0;
            }
            return num;
        }

        public bool CheckDuplicate(objBook obj)
        {
            int num;
            int.TryParse(obj.Id.ToString(), out num);
            Common_Static.CleanObjet(obj);
            string sQL = string.Concat(new object[]
            {
                "SELECT count(1) from tbl_books where    (title='",
                obj.title,
                "' and series_id=",
                obj.series_id,
                ") and  id <>",
                num
            });
            this.dbutil = new DBUtility();
            DataTable dataTable = new DataTable();
            dataTable = this.dbutil.getDataTable(sQL);
            return Convert.ToInt32(dataTable.Rows[0][0]) <= 0;
        }

        public bool Remove(int id)
        {
            this.query = "delete from  [tbl_books] where id=" + id.ToString();
            this.com = new SqlCommand();
            this.com.CommandText = this.query;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com) != 0;
        }

        public bool Update(object item)
        {
            if (item != null)
            {
                this.obj = (objBook)item;
                this.query = string.Concat(new string[]
                {
                    "update [tbl_books] set [subject_id]=",
                    this.obj.subject_id.ToString(),
                    ",[series_id]=",
                    this.obj.series_id.ToString(),
                    ", "
                });
                string text = this.query;
                this.query = string.Concat(new string[]
                {
                    text,
                    "  [board_id]='",
                    this.obj.board_id,
                    "',[class_id]='",
                    this.obj.class_id,
                    "',[title]='",
                    this.obj.title,
                    "' ,[price]=",
                    this.obj.price,
                    ",[description]='",
                    this.obj.description,
                    "', "
                });
                string text2 = this.query;
                this.query = string.Concat(new string[]
                {
                    text2,
                    "  [no_of_pages]=",
                    this.obj.no_of_pages,
                    ",[author]='",
                    this.obj.author,
                    "',[isbn]='",
                    this.obj.isbn,
                    "',[edition]='",
                    this.obj.edition,
                    "' , "
                });
                object obj = this.query;
                this.query = string.Concat(new object[]
                {
                    obj,
                    " [ebook]=",this.obj.ebook,
                    ",[suppleymentary]=",
                    (int)this.obj.suppleymentary,
                    ",[_print]=",
                    (int)this.obj._print,
                    ",[audio]=",
                    this.obj.audio,
                    ",[multimedia]=",
                    (int)this.obj.multimedia,
                    ",[modified_date]=getdate(),[is_active]=",
                    (int)this.obj.is_active,
                    " where id=",
                    this.obj.Id
                });
                this.com = new SqlCommand();
                this.com.CommandText = this.query;
                this.dbutil = new DBUtility();
                this.dbutil.Execute(this.com);
                return true;
            }
            return false;
        }

        public int UpdateSize()
        {
            int result = 0;
            try
            {
                IEnumerable<objBook> all = this.GetAll(IsValid.All);
                HttpContext current = HttpContext.Current;
                string text = "(0,";
                this.query = "UPDATE tbl_books  SET is_size = CASE id ";
                foreach (objBook current2 in all)
                {
                    string text2 = Common_Static.SizeOnDisk(current.Server.MapPath("~/library/" + current2.shortcut + ".epub"));
                    text2 = ((text2.Trim() == "") ? "N/A" : text2.Trim());
                    object obj = this.query;
                    this.query = string.Concat(new object[]
                    {
                        obj,
                        " WHEN ",
                        current2.Id,
                        " THEN '",
                        text2,
                        "' "
                    });
                    text = text + current2.Id + ",";
                }
                text += "-1)";
                this.query += " ELSE 'N/A'";
                this.query = this.query + "END WHERE id IN" + text + " ;";
                this.com = new SqlCommand();
                this.com.CommandText = this.query;
                this.dbutil = new DBUtility();
                result = this.dbutil.Execute(this.com);
            }
            catch
            {
                result = -1;
            }
            return result;
        }

        public bool Update(Dictionary<int, string> dict)
        {
            if (dict != null)
            {
                using (Dictionary<int, string>.Enumerator enumerator = dict.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<int, string> current = enumerator.Current;
                        this.query = string.Concat(new string[]
                        {
                            "update [tbl_books] set image_path='",
                            current.Value,
                            "', [shortcut]='",
                            current.Value,
                            "'  "
                        });
                        this.query = this.query + "  where id=" + current.Key;
                        this.com = new SqlCommand();
                        this.com.CommandText = this.query;
                        this.dbutil = new DBUtility();
                        this.dbutil.Execute(this.com);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool UpdateStatus(object item)
        {
            if (item != null)
            {
                objBook objBook = (objBook)item;
                string commandText = string.Concat(new object[]
                {
                    "update tbl_books set is_active=",
                    (int)objBook.is_active,
                    ",modified_date=getdate() where id=",
                    objBook.Id.ToString()
                });
                this.com = new SqlCommand();
                this.com.CommandText = commandText;
                this.dbutil = new DBUtility();
                this.dbutil.Execute(this.com);
                return true;
            }
            return false;
        }
    }
}