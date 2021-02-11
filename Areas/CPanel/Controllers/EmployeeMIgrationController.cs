using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using PrachiIndia.Portal.Areas.CPanel.Models;
using PrachiIndia.Sql.CustomRepositories;

namespace PrachiIndia.Portal.Areas.CPanel.Controllers
{
    public class EmployeeMIgrationController : Controller
    {
        // GET: CPanel/BookMigration

        // GET: /Migration/EmployeeMigration/
        Book_Migration bookMigration;
        DataTable dt = new DataTable();
        StringBuilder sbMessage = new StringBuilder();
        DataTable dtBinding = new DataTable();
        internal static string fldFilePath;
        internal static string fldFileName;
        string ImageBaseUrl = ConfigurationManager.AppSettings["ImageUrl"];
        public static int EmployeeType { get; set; }
        public ActionResult EmployeeMigration()
        {
            return View();
        }
        /*
            NAME    : RAHUL SRIVASTAVA
         *  DATE    : 26/03/2018
         *  PURPOSE : UPLOADING AND VALIDATING THE FILE 
         */
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file_Uploader,string Type)
        {
            if (file_Uploader != null)
            {
                string fileName = string.Empty;
                string destinationPath = string.Empty;
                string UploadedFileName = string.Empty;
                string message = string.Empty;
                string FileInDirectory = string.Empty;
                EmployeeType = Convert.ToInt32(Type);
                fileName = file_Uploader.FileName.ToLower();

                if ((fileName != "employeemigration.xls" || fileName == "employeemigration.xlsx") && (fileName != "employeemigration.xlsx" || fileName == "employeemigration.xls"))
                {
                    ViewBag.Message = "<div style='color:red'>Invalid file uploaded , You have to upload EmployeeMigration.xls / EmployeeMigration.xlsx</div>";
                    return View("EmployeeMigration");
                }
                destinationPath = Server.MapPath("~/Areas/Migration/upload/Employee" + fileName);
                if (!System.IO.Directory.Exists(Server.MapPath("~/Areas/Migration/upload/Employee")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Areas/Migration/upload/Employee"));
                }

                //Rahul srivastava
                //Deleting files from the folder
                Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/upload/Employee")), System.IO.File.Delete);

                if (fileName == "")
                    message = "Please Upload File ";
                else if (file_Uploader.ContentLength == 0)
                    message = "Please Upload Valid File ";

                if (message != "")
                {
                    ViewBag.Message = message;
                    return View("EnquiryMigration");

                }
                string[] a = fileName.Split('.');
                if (a[1].ToLower() != "xls" && a[1].ToLower() != "xlsx")
                    message = "Please Upload Valid File (.xls,.xlsx)";

                if (message != "")
                {
                    ViewBag.Message = message;
                    return View("EmployeeMigration");
                }


                file_Uploader.SaveAs(destinationPath);

                //if (Session["dtForMigration"] == null)
                //{
                FileInfo fi = new FileInfo(fileName);
                string ext = fi.Extension;
                string Connstr = string.Empty;
                if (ext == ".xls")
                {
                    Connstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + destinationPath + ";Extended Properties='Excel 8.0 xml;HDR=YES;'";

                }

                else if (ext == ".xlsx")
                {
                    Connstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + destinationPath + ";Extended Properties='Excel 12.0 xml;HDR=YES;'";
                }
                OleDbConnection oConn = new OleDbConnection(Connstr);
                OleDbDataAdapter oDa = new OleDbDataAdapter("Select * from [Sheet1$]", oConn);
                oDa.Fill(dt);
                oConn.Close();
                // Session["dtForMigration"] = dt;
                dt = RemoveBlankRows(dt);

                Session.Remove("dtForMigration_Employee");
                Session["dtForMigration_Employee"] = dt;
                if (dt.Rows.Count <= 0)
                {
                    ViewBag.Message = "File does not have any record please upload valid file";
                    Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/upload")), System.IO.File.Delete);
                    return View("EmployeeMigration");
                }
                if (ViewBag.Message == null)
                {
                    if (Validate(dt))
                    {
                        fldFilePath = destinationPath;
                        fldFileName = fileName;
                        ViewBag.Message = "<h4>File uploaded and validated successfully,Please click the save button to save the data</h4>";

                        return View("Save");
                    }
                    else
                    {
                        ViewBag.Message = TempData["Message"];
                        return View("EmployeeMigration");
                    }
                }


            }
            ViewBag.Message = "Please choose the file first to upload !";
            return View("EmployeeMigration");
        }


        public ActionResult Save()
        {
            return View();

        }
        [HttpPost]
        [ActionName("Save")]
        public ActionResult Save_Data(string Type)
        {
            try
            {
                if (SaveData())
                {
                    return View("Save");
                }

            }
            catch (Exception ex)
            {
                sbMessage.Append(ex.Message);
            }
            return View();
        }

        public ActionResult DownloadFile()
        {
            try
            {
                return File(new FileStream(Server.MapPath("~/Areas/CPanel/Migration/Download/Book.xlsx"), FileMode.Open), "application/octetstream", "Book.xlsx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable RemoveBlankRows(DataTable dt)
        {
            DataTable dtTable = new DataTable();
            string valuesarr = string.Empty;

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                valuesarr = "";
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                    valuesarr = valuesarr + dt.Rows[i][j].ToString();
                if (string.IsNullOrEmpty(valuesarr))
                {
                    dt.Rows[i].Delete();
                    i--;
                    dt.AcceptChanges();
                    valuesarr = "";
                }
            }
            dtTable = dt;
            return dtTable;
        }

        private Boolean Validate(DataTable dt)
        {
            try
            {
                int cnt = 0;
                //Validate NULL values:
                //Checking Board Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["UserName"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter UserName at row " + RowNo + "<br>");
                    }
                }

                //Checking Class Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Email"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Email at row " + RowNo + "<br>");
                    }
                }

                //Checking Subject Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Address"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Address at row " + RowNo + "<br>");
                    }
                }

                //Checking Series Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["FirstName"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter FirstName at row " + RowNo + "<br>");
                    }
                }


                //Checking Title Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Country"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Country at row " + RowNo + "<br>");
                    }
                }


                //Checking Author Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["State"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter State at row " + RowNo + "<br>");
                    }
                }


                //Checking ISBN Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["City"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter City at row " + RowNo + "<br>");
                    }
                }

                //Checking EDITION Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PhoneNumber"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter PhoneNumber at row " + RowNo + "<br>");
                    }
                }


                if (cnt == 8)
                    sbMessage.Append("File does not have any mandatory record.Please upload valid file.");
                if (sbMessage.ToString() != "")
                {
                    //enquiryMigration = new EnquiryMigration();
                    TempData["Message"] = "<ul style ='list-style:square;'>" + sbMessage.ToString() + "</ul>";

                    Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/EmployeeMIgration/upload")), System.IO.File.Delete);

                }
                else
                {
                    bookMigration = new Book_Migration();
                    bookMigration.Message = sbMessage.ToString();

                    sbMessage.Append(bookMigration.Message);
                    bookMigration.ReturnValue = true;

                }

                return bookMigration.ReturnValue;
            }
            catch (Exception ex)
            {

                sbMessage.Append(ex.Message);
                return false;
            }

        }

        private Boolean SaveData()
        {
            try
            {
                dt = (DataTable)Session["dtForMigration_Employee"];
                // save newly created datatable to database 
                Employee_Migration objEmployee_Migration = new Employee_Migration();
                int output = objEmployee_Migration.MigrateEmployee(dt, EmployeeType);
                string result = string.Empty;
                if (output == 1)
                {
                    result = "Data Uploaded successfully";

                }
                ViewBag.Message = sbMessage.Append(result);
                ViewBag.Flag = "true";
                ViewBag.UploadFlag = "true";
                return true;
            }
            catch (Exception ex)
            {
                sbMessage.Append(ex.Message);
                return false;
            }
        }
    }
    public enum EmployeeType
    {
        Sales,
        Maeketing
    }
}