using DAL;
using PrachiIndia.Portal.Areas.Readedge_BusLogic;
using PrachiIndia.Web.Areas.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PrachiIndia.Portal.Models;
namespace PrachiIndia.Portal.BAL
{
    public class SeriesBal : IRepositry
    {
        private SqlCommand com;

        private DBUtility dbutil;

        private string query = "";

        private List<objSeries> lstObj;

        private objSeries obj;

        public IEnumerable<object> GetAll()
        {
            return this.GetAll(IsValid.All, "");
        }

        public List<objSeries> GetAll(IsValid act = IsValid.All, string subject_Id = "")
        {
            if (act == IsValid.All)
            {
                if (subject_Id == "")
                {
                    this.query = "SELECT  ms.[Id] ,ms.Subject_Id,ms._image ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from tbl_Books mss where mss.Series_Id=ms.id)TotalBooks FROM [mst_Series]  ms order by ms.title asc";
                }
                else
                {
                    this.query = "SELECT  ms.[Id] ,ms.Subject_Id,ms._image ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from tbl_Books mss where mss.Series_Id=ms.id)TotalBooks FROM [mst_Series]  ms where ms.Subject_Id=" + subject_Id + " order by ms.title asc";
                }
            }
            else if (subject_Id == "")
            {
                this.query = "SELECT  ms.[Id] ,ms.Subject_Id,ms._image ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from tbl_Books mss where mss.Series_Id=ms.id)TotalBooks FROM [mst_Series]  ms where   ms.[is_active]=" + (int)act + " order by ms.title asc";
            }
            else
            {
                this.query = string.Concat(new object[]
                {
                    "SELECT  ms.[Id] ,ms.Subject_Id,ms._image ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date] ,ms.[modified_date] ,ms.[is_active]  ,(select count(1) from tbl_Books mss where mss.Series_Id=ms.id)TotalBooks FROM [mst_Series]  ms where ms.Subject_Id=",
                    subject_Id,
                    " and   ms.[is_active]=",
                    (int)act,
                    " order by ms.title asc"
                });
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objSeries>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objSeries
                {
                    Id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"]),
                    Description = Common_Static.ToSafeString(dataRow["Description"]),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"]),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    _image = Common_Static.ToSafeString(dataRow["_image"]),
                    Subject_Id = Common_Static.ToSafeString(dataRow["Subject_Id"].ToString()),
                    TotalBooks = Common_Static.ToSafeInt(dataRow["TotalBooks"].ToString())
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public IEnumerable<objSeries> GetAllSeriesSubject(IsValid act = IsValid.All)
        {
            if (act == IsValid.All)
            {
                this.query = "select m1.Id,m1.Title +'('+m2.title+')' title from MasterSeries m1 left join MasterSubject m2 on m1.SubjectId=m2.id   order by m2.title,m1.title asc";
            }
            else
            {
                this.query = string.Concat(new object[]
                {
                    "select m1.Id,m1.Title +'('+m2.title+')' title from MasterSeries m1 left join MasterSubject m2 on m1.SubjectId=m2.id where m1.status=",
                    (int)act,
                    " and m2.status=",
                    (int)act,
                    "order by m2.title,m1.title asc"
                });
            }
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            this.lstObj = new List<objSeries>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objSeries
                {
                    Id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"])
                };
                this.lstObj.Add(this.obj);
            }
            return this.lstObj;
        }

        public object Get(int id)
        {
            this.query = "SELECT  ms.[Id] ,ms.Subject_Id,ms._image ,ms.[title],ms.[Description],ms.[shortcut],ms.[created_date],ms.[modified_date] ,ms.[is_active]  ,(select count(1) from tbl_Books mss where mss.Series_Id=ms.id)TotalBooks ,mss.Title Subject FROM [mst_Series] ms left join mst_subject mss on ms.Subject_Id=mss.Id    where ms.id=" + id.ToString();
            this.dbutil = new DBUtility();
            DataTable dataTable = this.dbutil.getDataTable(this.query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.obj = new objSeries
                {
                    Id = Common_Static.ToSafeInt(dataRow["id"].ToString()),
                    title = Common_Static.ToSafeString(dataRow["title"]),
                    Description = Common_Static.ToSafeString(dataRow["Description"]),
                    shortcut = Common_Static.ToSafeString(dataRow["shortcut"]),
                    created_date = Common_Static.ToSafeDate(dataRow["created_date"].ToString()),
                    modified_date = Common_Static.ToSafeDate(dataRow["modified_date"].ToString()),
                    is_active = (IsValid)Common_Static.ToSafeInt(dataRow["is_active"].ToString()),
                    _image = Common_Static.ToSafeString(dataRow["_image"]),
                    Subject_Id = Common_Static.ToSafeString(dataRow["Subject_Id"].ToString()),
                    TotalBooks = Common_Static.ToSafeInt(dataRow["TotalBooks"].ToString()),
                    Subject = Common_Static.ToSafeString(dataRow["Subject"].ToString())
                };
            }
            return this.obj;
        }
        public int Add(object item)
        {
            Common_Static.CleanObjet(item);
            this.obj = new objSeries();
            this.obj = (objSeries)item;
            int num = 0;
            if (item != null)
            {
                this.obj.shortcut = this.obj.title.Trim().Replace('&', ' ');
                this.obj.shortcut = this.obj.shortcut.Trim().Replace(' ', '_');
                this.obj.shortcut = this.obj.shortcut.Trim().Replace("_-_", "_");
                this.obj = (objSeries)item;
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
                        this.query = "INSERT INTO [mst_Series] ([subject_id],[title],[shortcut],[created_date],[_image] ,[Description],[modified_date],[is_active])   ";
                        object obj = this.query;
                        this.query = string.Concat(new object[]
                        {
                            obj,
                            " VALUES(",
                            this.obj.Subject_Id,
                            ",'",
                            this.obj.title.Trim(),
                            "','",
                            this.obj.shortcut.Trim(),
                            "',getdate(),'",
                            this.obj._image,
                            "','",
                            this.obj.Description.Trim(),
                            "',getdate(),",
                            (int)this.obj.is_active,
                            ")"
                        });
                        this.com = new SqlCommand();
                        this.com.CommandText = this.query;
                        this.dbutil = new DBUtility();
                        num = this.dbutil.Execute(this.com);
                        if (num > 0)
                        {
                            this.query = "select id from [mst_Series] where  title='" + this.obj.title + "'  ";
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

        public bool CheckDuplicate(objSeries obj)
        {
            int num;
            int.TryParse(obj.Id.ToString(), out num);
            Common_Static.CleanObjet(obj);
            string sQL = string.Concat(new object[]
            {
                "SELECT count(1) from mst_Series where    (title='",
                obj.title,
                "' and subject_id=",
                obj.Subject_Id,
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
            this.query = string.Concat(new object[]
            {
                "update mst_Series set is_active=",
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
                this.obj = (objSeries)item;
                if (this.obj._image != "")
                {
                    this.query = string.Concat(new string[]
                    {
                        "update mst_Series set _image='",
                        this.obj._image,
                        "', subject_id=",
                        this.obj.Subject_Id,
                        ", title='",
                        this.obj.title,
                        "',Description='",
                        this.obj.Description,
                        "',shortcut='",
                        this.obj.shortcut,
                        "',modified_date=getdate() where id=",
                        this.obj.Id.ToString()
                    });
                }
                else
                {
                    this.query = string.Concat(new string[]
                    {
                        "update mst_Series set  subject_id=",
                        this.obj.Subject_Id,
                        " ,title='",
                        this.obj.title,
                        "',Description='",
                        this.obj.Description,
                        "',shortcut='",
                        this.obj.shortcut,
                        "',modified_date=getdate() where id=",
                        this.obj.Id.ToString()
                    });
                }
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
                this.obj = (objSeries)item;
                string commandText = string.Concat(new object[]
                {
                    "update mst_Series set is_active=",
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