using DAL;
using PrachiIndia.Portal.Models;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.BAL
{
    public class QuestionsBAL
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objQuestions> lstObj;

        private objQuestions obj;

        public List<objQuestions> GetAllByBook(int idBook, IsValid act = IsValid.All)
        {
            this.query = "SELECT   [ID] ,[idSubject] ,[idSeries] ,[idBook] ,[idChapter] ,[idQuestionType] ,[isHot] ,[isHeader]  ,[isQuestion] ,[isAns] ,[isImage] ,[isExtension] ,[isStatus] ,[dtmCreate]  ,[dtmUpdate]  FROM  [tbl_Questions] where [idBook]=" + idBook + " ";
            if (act != IsValid.All)
            {
                this.query = this.query + " and isStatus=" + (int)act;
            }
            this.query += " order by ID desc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestions();
                this.obj.ID = Common_Static.ToSafeInt(dataRow["id"].ToString());
                this.obj.idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString());
                this.obj.idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString());
                this.obj.idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString());
                this.obj.idChapter = Common_Static.ToSafeInt(dataRow["idChapter"].ToString());
                this.obj.idQuestionType = Common_Static.ToSafeInt(dataRow["idQuestionType"].ToString());
                this.obj.isHot = (Common_Static.ToSafeInt(dataRow["isHot"].ToString()) != 0);
                this.obj.isHeader = Common_Static.ToSafeString(dataRow["isHeader"].ToString());
                this.obj.isQuestion = Common_Static.ToSafeString(dataRow["isQuestion"].ToString());
                this.obj.isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString());
                this.obj.isImage = Common_Static.ToSafeByte(dataRow["isImage"]);
                this.obj.isExtension = Common_Static.ToSafeString(dataRow["isExtension"]);
                this.obj.isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString());
                this.obj.dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString());
                this.obj.dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString());
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objQuestions> CreateQuestionFile(string idBook)
        {
            this.query = "SELECT   [ID]  , [isImage] ,[isExtension]  FROM  [tbl_Questions] where  coalesce(isExtension,'')!=''  and idBook=" + idBook + " ";
            this.query += " order by ID desc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestions();
                this.obj.ID = Common_Static.ToSafeInt(dataRow["id"].ToString());
                this.obj.isImage = Common_Static.ToSafeByte(dataRow["isImage"]);
                this.obj.isExtension = Common_Static.ToSafeString(dataRow["isExtension"]);
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objQuestions> CreateQuestionOtionFile(string idBook)
        {
            this.query = "select  [ID]  , [isImage] ,[isExtension]  from tbl_Question_Options  where    coalesce(isExtension,'')!=''   and idQuestion in (SELECT  [ID]  FROM  [tbl_Questions] where    idBook=" + idBook + " )";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestions();
                this.obj.ID = Common_Static.ToSafeInt(dataRow["id"].ToString());
                this.obj.isImage = Common_Static.ToSafeByte(dataRow["isImage"]);
                this.obj.isExtension = Common_Static.ToSafeString(dataRow["isExtension"]);
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objQuestions> GetAll(int idChapter, IsValid act = IsValid.All)
        {
            this.query = "SELECT   TQ.[ID] ,[idSubject] ,[idSeries] ,[idBook] ,[idChapter] ,[idQuestionType] ,[isHot] ,[isHeader]  ,[isQuestion] ,[isAns] ,[isImage] ,[isExtension] ,[isStatus] ,[dtmCreate]  ,[dtmUpdate] ,ISNULL(TM.TOPIC,'N/A') as Topic   FROM  [tbl_Questions] TQ Left join TopicMaster TM on TM.ID=TQ.Topic where [idChapter]=" + idChapter + " ";
            if (act != IsValid.All)
            {
                this.query = this.query + " and isStatus=" + (int)act;
            }
            this.query += " order by TQ.ID desc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestions();
                this.obj.ID = Common_Static.ToSafeInt(dataRow["id"].ToString());
                this.obj.idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString());
                this.obj.idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString());
                this.obj.idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString());
                this.obj.idChapter = Common_Static.ToSafeInt(dataRow["idChapter"].ToString());
                this.obj.idQuestionType = Common_Static.ToSafeInt(dataRow["idQuestionType"].ToString());
                this.obj.isHot = (Common_Static.ToSafeInt(dataRow["isHot"].ToString()) != 0);
                this.obj.isHeader = Common_Static.ToSafeString(dataRow["isHeader"].ToString());
                this.obj.isQuestion = Common_Static.ToSafeString(dataRow["isQuestion"].ToString());
                this.obj.isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString());
                this.obj.isImage = Common_Static.ToSafeByte(dataRow["isImage"]);
                this.obj.isExtension = Common_Static.ToSafeString(dataRow["isExtension"]);
                this.obj.isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString());
                this.obj.dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString());
                this.obj.TopicDesc = Convert.ToString(dataRow["Topic"]);
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objQuestions> BookQuestion(int idBook, string isChapters = "", IsValid act = IsValid.All, int idQuestionType = 0, int Topic = 0, int Chapter = 0)
        {
            string text = "SELECT   [ID] ,[idSubject] ,[idSeries] ,[idBook] ,[idChapter] ,[idQuestionType] ,[isHot] ,[isHeader]  ,[isQuestion] ,[isAns] ,[isImage] ,[isExtension] ,[isStatus] ,[dtmCreate]  ,[dtmUpdate]  FROM  [tbl_Questions]   ";
            List<objQueOptions> bookOption;
            if (isChapters != null && isChapters != "")
            {
                string[] array = isChapters.Split(new char[]
                {
                    ','
                });
                ArrayList arrayList = new ArrayList();
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text2 = array2[i];
                    string[] array3 = text2.Split(new char[]
                    {
                        '-'
                    });
                    arrayList.Add(Common_Static.ToSafeInt(array3[0].Trim()));
                    arrayList.Add(Common_Static.ToSafeInt(array3[1].Trim()));
                }
                arrayList.Sort();
                int num = (int)arrayList[0];
                arrayList.Reverse();
                int num2 = (int)arrayList[0];
                object obj = text;
                text = string.Concat(new object[]
                {
                    obj,
                    " where idChapter in (select id from  chapters  where Chapterindex between ",
                    num,
                    " and ",
                    num2,
                    " and [idBook]=",
                    idBook,
                    ")"
                });
                bookOption = this.GetBookOption(idBook, num, num2);
            }
            else
            {
                //object obj2 = text;
                if (idBook != 0)
                {
                    text = string.Concat(new object[]
                    {
                    text,
                    " where [idBook]=",
                    idBook,
                    });
                }

                if (Topic != 0)
                {
                    text = string.Concat(new object[]
                    {
                    text,
                    " and Topic=",
                    Topic
                    });
                }
                if (Chapter != 0)
                {
                    text = string.Concat(new object[]
                 {
                    text,
                    " and idChapter=",
                    Chapter
                 });
                }
                bookOption = this.GetBookOption(idBook, 0, 0);
            }
            if (idQuestionType != 0)
            {
                text = text + " and idQuestionType=" + idQuestionType;
            }
            if (act != IsValid.All)
            {
                text = text + " and isStatus=" + (int)act;
            }
            text += " order by idQuestionType,idChapter asc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(text);
            this.lstObj = new List<objQuestions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestions();
                this.obj.ID = Common_Static.ToSafeInt(dataRow["id"].ToString());
                this.obj.idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString());
                this.obj.idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString());
                this.obj.idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString());
                this.obj.idChapter = Common_Static.ToSafeInt(dataRow["idChapter"].ToString());
                this.obj.idQuestionType = Common_Static.ToSafeInt(dataRow["idQuestionType"].ToString());
                this.obj.isHot = (Common_Static.ToSafeInt(dataRow["isHot"].ToString()) != 0);
                this.obj.isHeader = Common_Static.ToSafeString(dataRow["isHeader"].ToString());
                this.obj.isQuestion = Common_Static.ToSafeString(dataRow["isQuestion"].ToString());
                this.obj.isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString());
                this.obj.isImage = Common_Static.ToSafeByte(dataRow["isImage"]);
                this.obj.isExtension = Common_Static.ToSafeString(dataRow["isExtension"]);
                this.obj.isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString());
                this.obj.dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString());
                this.obj.dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString());
                IEnumerable<objQueOptions> enumerable = from item in bookOption
                                                        where item.idQuestion == this.obj.ID
                                                        select item;
                List<objQueOptions> list = new List<objQueOptions>();
                foreach (objQueOptions current in enumerable)
                {
                    list.Add(current);
                }
                this.obj.lstQueOptions = list;
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objBookCategoryQuestions> BookCategoryQuestions(int idBook = 0)
        {
            string text = "select (Select title from tbl_books where id=idBook)isBook, (Select isTitle from mst_Question_Category where id=idQuestionType)isCategory, count(1) isTotalQuestion from tbl_Questions group by idSubject, idSeries, idBook, idQuestionType , isSTatus  having isSTatus=" + 1;
            if (idBook > 0)
            {
                text = text + "   and  idBook=" + idBook;
            }
            text += "  order by isBook  ";
            List<objBookCategoryQuestions> list = new List<objBookCategoryQuestions>();
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(text);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                list.Add(new objBookCategoryQuestions
                {
                    isBook = Common_Static.ToSafeString(dataRow["isBook"].ToString()),
                    isCategory = Common_Static.ToSafeString(dataRow["isCategory"].ToString()),
                    isTotalQuestion = Common_Static.ToSafeInt(dataRow["isTotalQuestion"].ToString())
                });
            }
            return list;
        }

        public objQuestions Get(int id)
        {
            this.query = "SELECT   [ID] ,[idSubject] ,[idSeries] ,[idBook] ,[idChapter] ,[idQuestionType] ,[isHot] ,[isHeader]  ,[isQuestion] ,[isAns] ,[isImage] ,[isExtension] ,[isStatus] ,[dtmCreate]  ,[dtmUpdate],Category,Topic,[QstImgPriority],[AnsImgPriority],[AnsImage],[AnsExtension]  FROM  [tbl_Questions] where [id]=" + id + " ";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                this.obj = new objQuestions
                {
                    ID = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString()),
                    idChapter = Common_Static.ToSafeInt(dataRow["idChapter"].ToString()),
                    idQuestionType = Common_Static.ToSafeInt(dataRow["idQuestionType"].ToString()),
                    isHot = (Common_Static.ToSafeInt(dataRow["isHot"].ToString()) != 0),
                    isHeader = Common_Static.ToSafeString(dataRow["isHeader"].ToString()),
                    isQuestion = Common_Static.ToSafeString(dataRow["isQuestion"].ToString()),
                    isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString()),
                    isImage = Common_Static.ToSafeByte(dataRow["isImage"]),
                    isAnsImage = Common_Static.ToSafeByte(dataRow["AnsImage"]),
                    isExtension = Common_Static.ToSafeString(dataRow["isExtension"]),
                    AnsExtension = Common_Static.ToSafeString(dataRow["AnsExtension"]),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString()),
                    Category = Convert.ToInt32(dataRow["Category"]),
                    Topic = Convert.ToInt32(dataRow["Topic"]),
                    QimgPriority = Convert.ToBoolean(dataRow["QstImgPriority"]),
                    AnsimgPriority = Convert.ToBoolean(dataRow["AnsImgPriority"]),
                    lstQueOptions = this.GetQuestionOptions(Common_Static.ToSafeInt(dataRow["id"])),
                };
                if (this.obj.isImage.Length > 0)
                {
                    this.obj.ImageBase64String = System.Convert.ToBase64String(this.obj.isImage);
                }
                if (this.obj.isAnsImage.Length > 0)
                {
                    this.obj.AnsImageBase64String = System.Convert.ToBase64String(this.obj.isAnsImage);
                }
            }
            return this.obj;
        }

        public List<objQueOptions> GetQuestionOptions(int idQuestion = 0)
        {
            this.query = "SELECT   [ID] ,[idQuestion] ,[isOption]  ,[isChildOption] ,[isAns] ,[isImage]  ,[isExtension] ,[dtmCreate] ,[dtmUpdate]  ,[isStatus],[QstImgPriority],[AnsImgPriority],[AnsImage],[AnsExtension] FROM [tbl_Question_Options] where idQuestion=" + idQuestion + " order by [ID] asc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            List<objQueOptions> list = new List<objQueOptions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                objQueOptions item = new objQueOptions
                {
                    ID = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idQuestion = Common_Static.ToSafeInt(dataRow["idQuestion"].ToString()),
                    isOption = Common_Static.ToSafeString(dataRow["isOption"].ToString()),
                    isChildOption = Common_Static.ToSafeString(dataRow["isChildOption"].ToString()),
                    isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString()),
                    isImage = Common_Static.ToSafeByte(dataRow["isImage"]),
                    isExtension = Common_Static.ToSafeString(dataRow["isExtension"]),
                    AnsImage = Common_Static.ToSafeByte(dataRow["AnsImage"]),
                    AnsExtension = Common_Static.ToSafeString(dataRow["AnsExtension"]),
                    QimgPriority = Convert.ToBoolean(dataRow["QstImgPriority"]),
                    AnsimgPriority = Convert.ToBoolean(dataRow["AnsImgPriority"]),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString())
                };

                //item.ImageBase64String = System.Convert.ToBase64String(item.AnsImage);
                if (item.isImage.Length>0)
                {
                    item.ImageBase64String = System.Convert.ToBase64String(item.isImage);
                }

                if (item.AnsImage.Length > 0)
                {
                    item.AnsImageBase64String = System.Convert.ToBase64String(item.AnsImage);
                }
                list.Add(item);
            }
            return list;
        }

        public objQuestions GetE(int id)
        {
            this.query = "SELECT   [ID] ,[idSubject] ,[idSeries] ,[idBook] ,[idChapter] ,[idQuestionType] ,[isHot] ,[isHeader]  ,[isQuestion] ,[isAns] ,[isImage] , [isStatus] ,[dtmCreate]  ,[dtmUpdate],Category  FROM  [tbl_Questions] where [id]=" + id + " ";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                this.obj = new objQuestions
                {
                    ID = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString()),
                    idChapter = Common_Static.ToSafeInt(dataRow["idChapter"].ToString()),
                    idQuestionType = Common_Static.ToSafeInt(dataRow["idQuestionType"].ToString()),
                    isHot = (Common_Static.ToSafeInt(dataRow["isHot"].ToString()) != 0),
                    isHeader = Common_Static.ToSafeString(dataRow["isHeader"].ToString()),
                    isQuestion = Common_Static.ToSafeString(dataRow["isQuestion"].ToString()),
                    isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString()),
                    Category = Convert.ToInt32(dataRow["Category"]),
                    lstQueOptions = this.GetQuestionOptionsE(Common_Static.ToSafeInt(dataRow["id"].ToString()))
                };
            }
            return this.obj;
        }

        public List<objQueOptions> GetQuestionOptionsE(int idQuestion = 0)
        {
            this.query = "SELECT   [ID] ,[idQuestion] ,[isOption]  ,[isChildOption] ,[isAns] , [dtmCreate] ,[dtmUpdate]  ,[isStatus] FROM [tbl_Question_Options] where idQuestion=" + idQuestion + " order by [ID] asc";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            List<objQueOptions> list = new List<objQueOptions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                objQueOptions item = new objQueOptions
                {
                    ID = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idQuestion = Common_Static.ToSafeInt(dataRow["idQuestion"].ToString()),
                    isOption = Common_Static.ToSafeString(dataRow["isOption"].ToString()),
                    isChildOption = Common_Static.ToSafeString(dataRow["isChildOption"].ToString()),
                    isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString())
                };
                list.Add(item);
            }
            return list;
        }

        public List<objQueOptions> GetBookOption(int idBook = 0, int isChapterMin = 0, int isChapterMax = 0)
        {
            this.query = "SELECT   [ID] ,[idQuestion] ,[isOption]  ,[isChildOption] ,[isAns] ,[isImage]  ,[isExtension] ,[dtmCreate] ,[dtmUpdate]  ,[isStatus] FROM [tbl_Question_Options] where idQuestion in ";
            if (isChapterMin > 0 && isChapterMax > 0)
            {
                object obj = this.query;
                this.query = string.Concat(new object[]
                {
                    obj,
                    " (select id from    [tbl_Questions] where idChapter in(select   id from    tbl_books_chapters  where isChapter between ",
                    isChapterMin,
                    " and ",
                    isChapterMax,
                    " and idBook= ",
                    idBook,
                    ")) order by [ID] asc"
                });
            }
            else
            {
                object obj2 = this.query;
                this.query = string.Concat(new object[]
                {
                    obj2,
                    " (select ID from   [tbl_Questions] where idBook=",
                    idBook,
                    ") order by [ID] asc"
                });
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            List<objQueOptions> list = new List<objQueOptions>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                objQueOptions item = new objQueOptions
                {
                    ID = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idQuestion = Common_Static.ToSafeInt(dataRow["idQuestion"].ToString()),
                    isOption = Common_Static.ToSafeString(dataRow["isOption"].ToString()),
                    isChildOption = Common_Static.ToSafeString(dataRow["isChildOption"].ToString()),
                    isAns = Common_Static.ToSafeString(dataRow["isAns"].ToString()),
                    isImage = Common_Static.ToSafeByte(dataRow["isImage"]),
                    isExtension = Common_Static.ToSafeString(dataRow["isExtension"]),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString())
                };
                list.Add(item);
            }
            return list;
        }

        public int Add(objQuestions item)
        {
            Common_Static.CleanObjet(item);
            this.obj = new objQuestions();
            this.obj = item;
            int result = 0;
            if (item != null)
            {
                this.obj.isQuestion = this.obj.isQuestion.Trim();
                this.obj = item;
                this.obj.isStatus = IsValid.Active;
                if (this.CheckDuplicate(this.obj))
                {
                    this.com = new SqlCommand();
                    this.com.CommandText = "[USP_Insert_tbl_Questions]";
                    this.com.CommandType = CommandType.StoredProcedure;
                    this.com.Parameters.AddWithValue("@ID", this.obj.ID);
                    this.com.Parameters.AddWithValue("@idSubject", this.obj.idSubject);
                    this.com.Parameters.AddWithValue("@idSeries", this.obj.idSeries);
                    this.com.Parameters.AddWithValue("@idBook", this.obj.idBook);
                    this.com.Parameters.AddWithValue("@idChapter", this.obj.idChapter);
                    this.com.Parameters.AddWithValue("@idQuestionType", this.obj.idQuestionType);
                    this.com.Parameters.AddWithValue("@isHot", this.obj.isHot ? 1 : 0);
                    this.com.Parameters.AddWithValue("@isHeader", this.obj.isHeader);
                    this.com.Parameters.AddWithValue("@isQuestion", this.obj.isQuestion);
                    this.com.Parameters.AddWithValue("@isAns", this.obj.isAns);
                    this.com.Parameters.AddWithValue("@isImage", this.obj.isImage);
                    this.com.Parameters.AddWithValue("@isExtension", this.obj.isExtension);
                    this.com.Parameters.AddWithValue("@Category", item.Category); 
                    this.com.Parameters.AddWithValue("@AnsImage", item.isAnsImage);
                    this.com.Parameters.AddWithValue("@AnsExtension", item.AnsExtension);
                    this.com.Parameters.AddWithValue("@Topic", item.Topic);
                    this.com.Parameters.AddWithValue("@QimgPriority", item.QimgPriority);
                    this.com.Parameters.AddWithValue("@AnsimgPriority", item.AnsimgPriority);
                    //this.com.Parameters.AddWithValue("@AnsExtension", null);
                    this.com.Parameters.AddWithValue("@isStatus", (int)this.obj.isStatus);
                    SqlParameter sqlParameter = new SqlParameter();
                    sqlParameter.ParameterName = "@idOP";
                    sqlParameter.DbType = DbType.Int64;
                    sqlParameter.Direction = ParameterDirection.Output;
                    this.com.Parameters.Add(sqlParameter);
                    this.dbutil = new DBUtility();
                    result = this.dbutil.Execute(this.com);
                    int num = Common_Static.ToSafeInt(this.com.Parameters["@idOP"].Value.ToString());
                    if (num > 0)
                    {
                        this.obj.ID = num;
                        this.AddOptions(this.obj);
                        result = this.obj.ID;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public int UpdateImage(objQuestions item)
        {
            Common_Static.CleanObjet(item);
            this.obj = new objQuestions();
            this.obj = item;
            int result;
            if (item != null)
            {
                this.com = new SqlCommand();
                this.com.CommandText = "[sp_UpdateImage_tbl_Questions]";
                this.com.CommandType = CommandType.StoredProcedure;
                this.com.Parameters.AddWithValue("@ID", this.obj.ID);
                this.com.Parameters.AddWithValue("@isImage", this.obj.isImage);
                this.com.Parameters.AddWithValue("@isExtension", this.obj.isExtension);
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@idOP";
                sqlParameter.DbType = DbType.Int64;
                sqlParameter.Direction = ParameterDirection.Output;
                this.com.Parameters.Add(sqlParameter);
                this.dbutil = new DBUtility();
                result = this.dbutil.Execute(this.com);
                Common_Static.ToSafeInt(this.com.Parameters["@idOP"].Value.ToString());
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public int UpdateOptionImage(objQueOptions op)
        {
            this.com = new SqlCommand();
            this.com.CommandText = "[sp_UpdateImage_tbl_Questions_Options]";
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.Parameters.AddWithValue("@ID", op.ID);
            this.com.Parameters.AddWithValue("@isImage", op.isImage);
            this.com.Parameters.AddWithValue("@isExtension", op.isExtension);
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@idOP";
            sqlParameter.DbType = DbType.Int64;
            sqlParameter.Direction = ParameterDirection.Output;
            this.com.Parameters.Add(sqlParameter);
            this.dbutil = new DBUtility();
            int result = this.dbutil.Execute(this.com);
            int num = Common_Static.ToSafeInt(this.com.Parameters["@idOP"].Value.ToString());
            if (num > 0)
            {
                result = num;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public int AddOptions(objQuestions oq)
        {
            int result = 0;
            this.query = "DELETE FROM [tbl_Question_Options]  WHERE  [idQuestion]=" + oq.ID;
            this.com = new SqlCommand();
            this.com.CommandText = this.query;
            this.dbutil.Execute(this.com);
            oq.isStatus = IsValid.Active;
            if (oq.lstQueOptions != null && oq.lstQueOptions.Count > 0)
            {
                using (List<objQueOptions>.Enumerator enumerator = oq.lstQueOptions.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        objQueOptions current = enumerator.Current;
                        this.com = new SqlCommand();
                        this.com.CommandText = "[sp_Insert_tbl_Questions_Options]";
                        this.com.CommandType = CommandType.StoredProcedure;
                        this.com.Parameters.AddWithValue("@idQuestion", oq.ID);
                        this.com.Parameters.AddWithValue("@isOption", current.isOption);
                        this.com.Parameters.AddWithValue("@isChildOption", current.isChildOption);
                        this.com.Parameters.AddWithValue("@isAns", current.isAns);
                        this.com.Parameters.AddWithValue("@isImage", current.isImage);
                        this.com.Parameters.AddWithValue("@isExtension", current.isExtension);
                        this.com.Parameters.AddWithValue("@OptAnsImagePriority", current.AnsimgPriority);
                        this.com.Parameters.AddWithValue("@AnsImage", current.AnsImage);
                        this.com.Parameters.AddWithValue("@AnsExtension", current.AnsExtension);
                        this.com.Parameters.AddWithValue("@OptQImagePriority", current.QimgPriority);
                        this.com.Parameters.AddWithValue("@isStatus", (int)current.isStatus);
                        SqlParameter sqlParameter = new SqlParameter();
                        sqlParameter.ParameterName = "@idOP";
                        sqlParameter.DbType = DbType.Int64;
                        sqlParameter.Direction = ParameterDirection.Output;
                        this.com.Parameters.Add(sqlParameter);
                        this.dbutil = new DBUtility();
                        result = this.dbutil.Execute(this.com);
                        int num = Common_Static.ToSafeInt(this.com.Parameters["@idOP"].Value.ToString());
                        if (num > 0)
                        {
                            result = num;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    return result;
                }
            }
            result = 0;
            return result;
        }

        public bool CheckDuplicate(objQuestions obj)
        {
            /*just for bypassing*/
            return true;

            int num;
            int.TryParse(obj.ID.ToString(), out num);
            this.query = string.Concat(new object[]
            {
                "SELECT count(1) from tbl_Questions where idChapter=",
                obj.idChapter,
                " and  isQuestion='",
                obj.isQuestion,
                "'  and id <>",
                num,
                " "
            });
            this.dbutil = new DBUtility();
            DataTable dataTable = new DataTable();
            dataTable = this.dbutil.getDataTable(this.query);
            return Convert.ToInt32(dataTable.Rows[0][0]) <= 0;
        }

        public bool Update(objQuestions item)
        {
            if (item != null)
            {
                this.obj = item;
                this.query = "UPDATE  [tbl_Questions]   SET [idSubject] =@idSubject   ,[idSeries] =@idSeries ,[idBook] =@idBook ,[idChapter] =@idChapter ,[idQuestionType] =@idQuestionType ,[isHot] =@isHot ,[isHeader]=@isHeader ,[isQuestion]=@isQuestion ,[isAns]=@isAns ,[isImage]=@isImage ,[isExtension]=@isExtension ,[dtmUpdate] =getdate() WHERE  [ID]=@ID";
                this.com = new SqlCommand(this.query);
                this.com.Parameters.AddWithValue("@ID", this.obj.ID);
                this.com.Parameters.AddWithValue("@idSubject", this.obj.idSubject);
                this.com.Parameters.AddWithValue("@idSeries", this.obj.idSeries);
                this.com.Parameters.AddWithValue("@idBook", this.obj.idBook);
                this.com.Parameters.AddWithValue("@idChapter", this.obj.idChapter);
                this.com.Parameters.AddWithValue("@idQuestionType", this.obj.idQuestionType);
                this.com.Parameters.AddWithValue("@isHot", this.obj.isHot ? 1 : 0);
                this.com.Parameters.AddWithValue("@isHeader", this.obj.isHeader);
                this.com.Parameters.AddWithValue("@isQuestion", this.obj.isQuestion);
                this.com.Parameters.AddWithValue("@isAns", this.obj.isAns);
                this.com.Parameters.AddWithValue("@isImage", this.obj.isImage);
                this.com.Parameters.AddWithValue("@isExtension", this.obj.isExtension);
                this.dbutil = new DBUtility();
                int num = this.dbutil.Execute(this.com);
                return num > 0;
            }
            return false;
        }

        public int BatchUpdate(string idQuestion, int idQuestionCategory)
        {
            int result = 0;
            try
            {
                this.query = string.Concat(new object[]
                {
                    "UPDATE  [tbl_Questions]  set  [idQuestionType] =",
                    idQuestionCategory,
                    " ,[dtmUpdate] =DATEADD(minute, 330,GetUTCdate()) WHERE  [ID] in (",
                    idQuestion,
                    ")"
                });
                this.com = new SqlCommand(this.query);
                this.dbutil = new DBUtility();
                result = this.dbutil.Execute(this.com);
                return result;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public int BatchUpdateStatus(string idQuestion, IsValid status)
        {
            int result = 0;
            try
            {
                this.query = string.Concat(new object[]
                {
                    "UPDATE  [tbl_Questions]  set  [isStatus] =",
                    (int)status,
                    " ,[dtmUpdate] =DATEADD(minute, 330,GetUTCdate()) WHERE  [ID] in (",
                    idQuestion,
                    ")"
                });
                this.com = new SqlCommand(this.query);
                this.dbutil = new DBUtility();
                result = this.dbutil.Execute(this.com);
                return result;
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public int UpdateStatus(int id)
        {
            string commandText = "update [tbl_Questions] set isStatus= case isStatus when 1 then 0 when 0 then 1 end,dtmUpdate=getdate() where id=" + id;
            this.com = new SqlCommand();
            this.com.CommandText = commandText;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com);
        }

        public int Delete(int id)
        {
            string commandText = "Delete from tbl_Question_Options where idQuestion=" + id;
            this.com = new SqlCommand();
            this.com.CommandText = commandText;
            this.dbutil = new DBUtility();
            int num = this.dbutil.Execute(this.com);
            commandText = "Delete from tbl_Questions where id=" + id;
            this.com = new SqlCommand();
            this.com.CommandText = commandText;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com);
        }
    }
}