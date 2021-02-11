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
    public class QuestionCategoryBAL
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objQuestionCategory> lstObj;

        private objQuestionCategory obj;

        public int Add(objQuestionCategory obj)
        {
            Common_Static.CleanObjet(obj);
            int num = 0;
            if (obj != null)
            {
                obj.isStatus = IsValid.Active;
                if (this.CheckDuplicate(obj))
                {
                    if (obj.ID > 0)
                    {
                        if (this.Update(obj))
                        {
                            num = obj.ID;
                        }
                        else
                        {
                            num = 0;
                        }
                    }
                    else
                    {
                        this.query = "INSERT INTO [mst_Question_Category] ([isTitle],[isRemarks],[dtmCreate],[isStatus])   ";
                        object obj2 = this.query;
                        this.query = string.Concat(new object[]
                        {
                            obj2,
                            " VALUES('",
                            obj.isTitle.Trim(),
                            "','",
                            obj.isRemarks.Trim(),
                            "',getdate(),",
                            (int)obj.isStatus,
                            ") SELECT @@IDENTITY"
                        });
                        this.com = new SqlCommand();
                        this.com.CommandText = this.query;
                        this.dbutil = new DBUtility();
                        num = this.dbutil.ExecuteScalar(this.com);
                        if (num <= 0)
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

        public bool CheckDuplicate(objQuestionCategory obj)
        {
            int num;
            int.TryParse(obj.ID.ToString(), out num);
            Common_Static.CleanObjet(obj);
            string sQL = string.Concat(new object[]
            {
                "SELECT count(1) from mst_Question_Category where   isTitle='",
                obj.isTitle,
                "'   and  id <>",
                num
            });
            this.dbutil = new DBUtility();
            DataTable dataTable = new DataTable();
            dataTable = this.dbutil.getDataTable(sQL);
            return Convert.ToInt32(dataTable.Rows[0][0]) <= 0;
        }

        public bool Update(objQuestionCategory obj)
        {
            if (obj != null)
            {
                Common_Static.CleanObjet(obj);
                this.query = string.Concat(new string[]
                {
                    "update mst_Question_Category set   isTitle='",
                    obj.isTitle,
                    "',isRemarks='",
                    obj.isRemarks,
                    "' , dtmUpdate=getdate() where id=",
                    obj.ID.ToString()
                });
                this.com = new SqlCommand();
                this.com.CommandText = this.query;
                this.dbutil = new DBUtility();
                this.dbutil.Execute(this.com);
                return true;
            }
            return false;
        }

        public bool UpdateStatus(objQuestionCategory obj)
        {
            if (obj != null)
            {
                Common_Static.CleanObjet(obj);
                this.query = string.Concat(new object[]
                {
                    "update mst_Question_Category set isStatus=",
                    (int)obj.isStatus,
                    "  where id=",
                    obj.ID.ToString()
                });
                this.com = new SqlCommand();
                this.com.CommandText = this.query;
                this.dbutil = new DBUtility();
                this.dbutil.Execute(this.com);
                return true;
            }
            return false;
        }

        public List<objQuestionCategory> GetAll(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "SELECT   [ID],[Title],[Remarks],[dtmCreate],[dtmUpdate],[Status]  FROM  [mst_Question_Category] order by [Title] asc";
            }
            else
            {
                this.query = "SELECT   [ID],[Title],[Remarks],[dtmCreate],[dtmUpdate],[Status]  FROM  [mst_Question_Category] where Status=" + (int)act + " order by [Title] asc";
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestionCategory>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestionCategory
                {
                    ID = Common_Static.ToSafeInt(dataRow["ID"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["Title"]),
                    isRemarks = Common_Static.ToSafeString(dataRow["Remarks"]),
                    dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                    dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString()),
                    isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["Status"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }
        public List<objQuestionCategory> GetAllCategory()
        {
  
            this.query = "SELECT [ID],[Type],[Status]  FROM  [QuestionType] where Status=1 order by ID Desc";

            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestionCategory>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestionCategory
                {
                    ID = Common_Static.ToSafeInt(dataRow["ID"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["Type"]),
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public List<objQuestionCategory> GetTopic(int chapterid)
        {

            this.query = "SELECT [ID],[Topic],[Status]  FROM  [TopicMaster] where Status=1  and Chapterid= "+ chapterid + " order by ID Desc";

            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objQuestionCategory>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objQuestionCategory
                {
                    ID = Common_Static.ToSafeInt(dataRow["ID"].ToString()),
                    isTitle = Common_Static.ToSafeString(dataRow["Topic"]),
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }
        public objQuestionCategory Get(string id)
        {
            this.query = "SELECT   [ID],[isTitle],[isRemarks],[dtmCreate],[dtmUpdate],[isStatus]  FROM  [mst_Question_Category] where id=" + id + " ";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            //new CityBal();
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    this.obj = new objQuestionCategory
                    {
                        ID = Common_Static.ToSafeInt(dataRow["ID"].ToString()),
                        isTitle = Common_Static.ToSafeString(dataRow["isTitle"]),
                        isRemarks = Common_Static.ToSafeString(dataRow["isRemarks"]),
                        dtmCreate = Common_Static.ToSafeDate(dataRow["dtmCreate"].ToString()),
                        dtmUpdate = Common_Static.ToSafeDate(dataRow["dtmUpdate"].ToString()),
                        isStatus = (IsValid)Common_Static.ToSafeInt(dataRow["isStatus"].ToString())
                    };
                }
                else
                {
                    this.obj = null;
                }
            }
            else
            {
                this.obj = null;
            }
            return this.obj;
        }
    }
}