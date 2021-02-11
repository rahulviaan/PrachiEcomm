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
    public class SubjectBal : IRepositry
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objSubject> lstObj;

        private objSubject obj;

        public IEnumerable<object> GetAll()
        {
            return this.GetAll(IsValid.All);
        }

        public IEnumerable<objSubject> GetAll(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "SELECT  ms.[Id] ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from mst_Series mss where mss.subject_id=ms.id)TotalSeries FROM [mst_Subject]  ms order by ms.title asc";
            }
            else
            {
                this.query = "SELECT  ms.[Id] ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from mst_Series mss where mss.subject_id=ms.id)TotalSeries FROM [mst_Subject] ms where  ms.[is_active]=" + (int)act + " order by ms.title asc";
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objSubject>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objSubject
                {
                    Id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"]),
                    Description = Common_Static.ToSafeString(dataRow["Description"]),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"]),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    TotalSeries = Common_Static.ToSafeInt(dataRow["TotalSeries"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public object Get(int id)
        {
            this.query = "SELECT  ms.[Id] ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from mst_Series mss where mss.subject_id=ms.id)TotalSeries FROM [mst_Subject]  ms  where ms.id=" + id.ToString() + " ";
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objSubject
                {
                    Id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"]),
                    Description = Common_Static.ToSafeString(dataRow["Description"]),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"]),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    TotalSeries = Common_Static.ToSafeInt(dataRow["TotalSeries"].ToString())
                };
            }
            return this.obj;
        }

        public int Add(object item)
        {
            Common_Static.CleanObjet(item);
            this.obj = new objSubject();
            this.obj = (objSubject)item;
            int num = 0;
            if (item != null)
            {
                this.obj.shortcut = this.obj.title.Trim().Replace('&', ' ');
                this.obj.shortcut = this.obj.shortcut.Trim().Replace(' ', '_');
                this.obj.shortcut = this.obj.shortcut.Trim().Replace("_-_", "_");
                this.obj = (objSubject)item;
                this.obj.is_active = IsValid.Active;
                if (this.CheckDuplicate(this.obj))
                {
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
                        this.query = "INSERT INTO [mst_Subject] ([title] ,[Description] ,[shortcut] ,[created_date] ,[modified_date] ,[is_active])   ";
                        object obj = this.query;
                        this.query = string.Concat(new object[]
                        {
                            obj,
                            " VALUES('",
                            this.obj.title.Trim(),
                            "','",
                            this.obj.Description.Trim(),
                            "','",
                            this.obj.shortcut.Trim(),
                            "',getdate(),getdate(),",
                            (int)this.obj.is_active,
                            ")"
                        });
                        this.com = new SqlCommand();
                        this.com.CommandText = this.query;
                        this.dbutil = new DBUtility();
                        num = this.dbutil.Execute(this.com);
                        if (num > 0)
                        {
                            this.query = "select id from [mst_Subject] where  title='" + this.obj.title + "'  ";
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

        public bool CheckDuplicate(objSubject obj)
        {
            int num;
            int.TryParse(obj.Id.ToString(), out num);
            Common_Static.CleanObjet(obj);
            string sQL = string.Concat(new object[]
            {
                "SELECT count(1) from mst_Subject where    title='",
                obj.title,
                "' and  id <>",
                num
            });
            this.dbutil = new DBUtility();
            DataTable dataTable = new DataTable();
            dataTable = this.dbutil.getDataTable(sQL);
            return Convert.ToInt32(dataTable.Rows[0][0]) <= 0;
        }

        public bool Remove(int id)
        {
            this.query = string.Concat(new object[]
            {
                "update mst_Subject set is_active=",
                4,
                ",modified_date=Date()  where id=",
                id.ToString()
            });
            this.com = new SqlCommand();
            this.com.CommandText = this.query;
            this.dbutil = new DBUtility();
            return this.dbutil.Execute(this.com) != 0;
        }

        public bool Update(object item)
        {
            if (item != null)
            {
                Common_Static.CleanObjet(item);
                this.obj = (objSubject)item;
                this.query = string.Concat(new string[]
                {
                    "update mst_Subject set title='",
                    this.obj.title,
                    "',Description='",
                    this.obj.Description,
                    "',shortcut='",
                    this.obj.shortcut,
                    "',modified_date=getdate() where id=",
                    this.obj.Id.ToString()
                });
                this.com = new SqlCommand();
                this.com.CommandText = this.query;
                this.dbutil = new DBUtility();
                this.dbutil.Execute(this.com);
                return true;
            }
            return false;
        }

        public bool UpdateStatus(object item)
        {
            if (item != null)
            {
                this.obj = (objSubject)item;
                string commandText = string.Concat(new object[]
                {
                    "update mst_Subject set is_active=",
                    (int)this.obj.is_active,
                    ",modified_date=getdate() where id=",
                    this.obj.Id.ToString()
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