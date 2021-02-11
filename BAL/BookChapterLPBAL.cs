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
    public class BookChapterLPBAL
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objBookChapterLP> lstObj;

        private objBookChapterLP obj;

        public IEnumerable<objBookChapterLP> GetAll(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "SELECT [id],[idChapter],[idBook],[idSeries],[idSubject],[isChName],[isPeriods],[isObjective],[isLearOutcome],[isTeaMethod]  ,[isSuggFAAct],[isStatus],[isType],[dtmCreate],[dtmUpdate] FROM  [tbl_books_chapters_lp]  order by id asc";
            }
            else
            {
                this.query = "SELECT [id],[idChapter],[idBook],[idSeries],[idSubject],[isChName],[isPeriods],[isObjective],[isLearOutcome],[isTeaMethod]  ,[isSuggFAAct],[isStatus],[isType],[dtmCreate],[dtmUpdate] FROM  [tbl_books_chapters_lp]  isStatus=" + (int)act + " order by id asc";
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objBookChapterLP>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objBookChapterLP
                {
                    id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    idChapter = Common_Static.ToSafeInt(dataRow["idChapter"].ToString()),
                    idBook = Common_Static.ToSafeInt(dataRow["idBook"].ToString()),
                    idSeries = Common_Static.ToSafeInt(dataRow["idSeries"].ToString()),
                    idSubject = Common_Static.ToSafeInt(dataRow["idSubject"].ToString()),
                    isChName = Common_Static.ToSafeString(dataRow["isChName"].ToString()),
                    isPeriods = Common_Static.ToSafeString(dataRow["isPeriods"].ToString()),
                    isObjective = Common_Static.ToSafeString(dataRow["isObjective"].ToString()),
                    isLearOutcome = Common_Static.ToSafeString(dataRow["isLearOutcome"].ToString()),
                    isTeaMethod = Common_Static.ToSafeString(dataRow["isTeaMethod"].ToString()),
                    isSuggFAAct = Common_Static.ToSafeString(dataRow["isSuggFAAct"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString()),
                    isType = (TextType)Common_Static.ToSafeInt(dataRow["isFromPage"].ToString()),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public objBookChapterLP Get(int id)
        {
            BookChapterBAL bookChapterBAL = new BookChapterBAL();
            objBookChapter objBookChapter = (objBookChapter)bookChapterBAL.Get(id);
            this.query = "SELECT [id],[idChapter],[idBook],[idSeries],[idSubject],[isChName],[isPeriods],[isObjective],[isLearOutcome],[isTeaMethod]  ,[isSuggFAAct],[isStatus],[isType],[dtmCreate],[dtmUpdate] FROM  [tbl_books_chapters_lp] where id=" + id;
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.obj = new objBookChapterLP();
            this.obj.idChapter = objBookChapter.id;
            this.obj.idBook = objBookChapter.idBook;
            this.obj.idSeries = objBookChapter.idSeries;
            this.obj.idSubject = objBookChapter.idSubject;
            this.obj.isChName = objBookChapter.isTitle;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj.id = Common_Static.ToSafeInt(dataRow["id"].ToString());
                this.obj.isPeriods = Common_Static.ToSafeString(dataRow["isPeriods"].ToString());
                this.obj.isObjective = Common_Static.ToSafeString(dataRow["isObjective"].ToString());
                this.obj.isLearOutcome = Common_Static.ToSafeString(dataRow["isLearOutcome"].ToString());
                this.obj.isTeaMethod = Common_Static.ToSafeString(dataRow["isTeaMethod"].ToString());
                this.obj.isSuggFAAct = Common_Static.ToSafeString(dataRow["isSuggFAAct"].ToString());
                this.obj.isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString());
                this.obj.isType = (TextType)Common_Static.ToSafeInt(dataRow["isFromPage"].ToString());
                this.obj.dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString());
            }
            return this.obj;
        }

        public int Add(object item)
        {
            Common_Static.CleanObjet(item);
            this.obj = new objBookChapterLP();
            this.obj = (objBookChapterLP)item;
            int result = 0;
            if (item != null)
            {
                this.obj.isChName = this.obj.isChName.Trim();
                this.obj.isPeriods = this.obj.isPeriods.Trim();
                this.obj.isObjective = this.obj.isObjective.Trim();
                this.obj.isLearOutcome = this.obj.isLearOutcome.Trim();
                this.obj.isTeaMethod = this.obj.isTeaMethod.Trim();
                this.obj.isSuggFAAct = this.obj.isSuggFAAct.Trim();
                this.obj = (objBookChapterLP)item;
                this.obj.isStatus = IsValid.Active;
                if (this.CheckDuplicate(this.obj))
                {
                    if (this.obj.id > 0)
                    {
                        if (this.Update(this.obj))
                        {
                            result = this.obj.id;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    else
                    {
                        this.query = string.Concat(new object[]
                        {
                            "INSERT INTO  [tbl_books_chapters_lp] ([idChapter],[idBook],[idSeries],[idSubject],[isChName],[isPeriods],[isObjective],[isLearOutcome], [isTeaMethod],[isSuggFAAct],[isStatus],[isType],[dtmCreate],[dtmUpdate])  VALUES(",
                            this.obj.idChapter,
                            ",",
                            this.obj.idBook,
                            ",",
                            this.obj.idSeries,
                            ",",
                            this.obj.idSubject,
                            ",'",
                            this.obj.isChName,
                            "','",
                            this.obj.isPeriods,
                            "', '",
                            this.obj.isObjective,
                            "','",
                            this.obj.isLearOutcome,
                            "', '",
                            this.obj.isTeaMethod,
                            "','",
                            this.obj.isSuggFAAct,
                            "',",
                            (int)this.obj.isStatus,
                            ",",
                            (int)this.obj.isType,
                            ",getdate(),getdate()) "
                        });
                        this.com = new SqlCommand();
                        this.com.CommandText = this.query;
                        this.dbutil = new DBUtility();
                        result = this.dbutil.Execute(this.com);
                    }
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public bool CheckDuplicate(objBookChapterLP obj)
        {
            int num;
            int.TryParse(obj.id.ToString(), out num);
            Common_Static.CleanObjet(obj);
            string sQL = string.Concat(new object[]
            {
                "SELECT count(1) from tbl_books_chapters_lp where   ( [idChapter]=",
                obj.idChapter,
                " and [idBook]=",
                obj.idBook,
                " and  [idSeries]=",
                obj.idSeries,
                " and [idSubject]=",
                obj.idSubject,
                " and [isChName]='",
                obj.isChName,
                "'  and [isPeriods] ='",
                obj.isPeriods,
                "' and  [isObjective]='",
                obj.isObjective,
                "' and [isLearOutcome]='",
                obj.isLearOutcome,
                "'  and [isTeaMethod]='",
                obj.isTeaMethod,
                "' and [isSuggFAAct] ='",
                obj.isSuggFAAct,
                "') and   id <>",
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
                Common_Static.CleanObjet(item);
                this.obj = (objBookChapterLP)item;
                this.query = string.Concat(new object[]
                {
                    "UPDATE  [tbl_books_chapters_lp]  [idChapter] = ",
                    this.obj.idChapter,
                    " ,[idBook] = ",
                    this.obj.idBook,
                    " ,[idSeries] = ",
                    this.obj.idSeries,
                    " ,[idSubject] = ",
                    this.obj.idSubject,
                    " ,[isChName] = ",
                    this.obj.isChName,
                    "' ,[isPeriods] = ",
                    this.obj.isPeriods,
                    "' ,[isObjective] = ",
                    this.obj.isObjective,
                    "' ,[isLearOutcome] = ",
                    this.obj.isLearOutcome,
                    "' ,[isTeaMethod] = ",
                    this.obj.isTeaMethod,
                    "' ,[isSuggFAAct] = ",
                    this.obj.isSuggFAAct,
                    "' ,[isType] = ",
                    (int)this.obj.isType,
                    " ,[dtmUpdate] = GETDATE() WHERE id= ",
                    this.obj.id
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
            string commandText = "update tbl_books_chapters_lp set isStatus= case isStatus when 1 then 0 when 0 then 1 end,dtmUpdate=getdate() where id=" + id;
            this.com = new SqlCommand();
            this.com.CommandText = commandText;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com);
        }
    }
}