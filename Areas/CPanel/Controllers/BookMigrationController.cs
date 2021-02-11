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
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BookMigrationController : Controller
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

        public ActionResult BookMigration()
        {
            return View();
        }
        /*
            NAME    : RAHUL SRIVASTAVA
         *  DATE    : 12/05/2016
         *  PURPOSE : UPLOADING AND VALIDATING THE FILE 
         */
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file_Uploader)
        {
            if (file_Uploader != null)
            {
                string fileName = string.Empty;
                string destinationPath = string.Empty;
                string UploadedFileName = string.Empty;
                string message = string.Empty;
                string FileInDirectory = string.Empty;
                fileName = file_Uploader.FileName.ToLower();

                if ((fileName != "book.xls" || fileName == "book.xlsx") && (fileName != "book.xlsx" || fileName == "book.xls"))
                {
                    ViewBag.Message = "<div style='color:red'>Invalid file uploaded , You have to upload book.xls / book.xlsx</div>";
                    return View("BookMigration");
                }
                destinationPath = Server.MapPath("~/Areas/Migration/upload/" + fileName);
                if (!System.IO.Directory.Exists(Server.MapPath("~/Areas/Migration/upload")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Areas/Migration/upload"));
                }

                //Rahul srivastava
                //Deleting files from the folder
                Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/upload")), System.IO.File.Delete);

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
                    return View("BookMigration");
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
                OleDbDataAdapter oDa = new OleDbDataAdapter("Select * from [Books$]", oConn);
                oDa.Fill(dt);
                oConn.Close();
                // Session["dtForMigration"] = dt;
                dt = RemoveBlankRows(dt);
       
                Session.Remove("dtForMigration");
                Session["dtForMigration"] = dt;
                if (dt.Rows.Count <= 0)
                {
                    ViewBag.Message = "File does not have any record please upload valid file";
                    Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/upload")), System.IO.File.Delete);
                    return View("BookMigration");
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
                        return View("BookMigration");
                    }
                }


            }
            ViewBag.Message = "Please choose the file first to upload !";
            return View("BookMigration");
        }


        public ActionResult Save()
        {
            return View();

        }
        [HttpPost]
        [ActionName("Save")]
        public ActionResult Save_Data()
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
                    if (dt.Rows[i]["BOARD"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Board at row " + RowNo+ "<br>");
                    }
                }

                //Checking Class Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["CLASS"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Class at row " + RowNo + "<br>");
                    }
                }

                //Checking Subject Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SUBJECT"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Subject at row " + RowNo + "<br>");
                    }
                }

                //Checking Series Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SERIES"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Series at row " + RowNo + "<br>");
                    }
                }


                //Checking Title Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TITLE"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Title at row " + RowNo + "<br>");
                    }
                }


                //Checking Author Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["AUTHOR"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Author at row " + RowNo + "<br>");
                    }
                }


                //Checking ISBN Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ISBN"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter ISBN at row " + RowNo + "<br>");
                    }
                }

                //Checking EDITION Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["EDITION"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Edition at row " + RowNo + "<br>");
                    }
                }

                //Checking PRICE Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["PRICE"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Price at row " + RowNo + "<br>");
                    }
                }

                //Checking E-BOOK Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["E-BOOK"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter E-Book at row " + RowNo + "<br>");
                    }
                }


                //Checking Multimedia Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["MULTIMEDIA"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Multimedai at row " + RowNo + "<br>");
                    }
                }


                //Checking Solution Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SOLUTIONS"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Solution at row " + RowNo + "<br>");
                    }
                }


                //Checking Image Null value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["IMAGE"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if(cnt != cnt + 1)
                        cnt = cnt+1;
                        sbMessage.Append("<li>Please Enter Image at row " + RowNo + "<br>");
                    }
                }

                int val = 0;
                bool IsNumeric = true;
                //Checking Discount value
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IsNumeric= int.TryParse(dt.Rows[i]["DISCOUNT"].ToString(), out val);
                    if (!IsNumeric)
                    {
                             int RowNo = i + 2;
                            sbMessage.Append("<li>Discount Should Be Numeric at row " + RowNo + "<br>");
                    
                    }
                }


                if (cnt == 14)
                    sbMessage.Append("File does not have any mandatory record.Please upload valid file.");
                if (sbMessage.ToString() != "")
                {
                    //enquiryMigration = new EnquiryMigration();
                    TempData["Message"] = "<ul style ='list-style:square;'>" + sbMessage.ToString() + "</ul>";

                    Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/upload")), System.IO.File.Delete);

                }
                else
                {
                    bookMigration = new Book_Migration();
                    bookMigration.Message = sbMessage.ToString();
                   // bookMigration.ReturnValue = ReturnValue;
                   // bookMigration = bookMigration.FileValidation(enquiryMigration, dtBinding);
                    sbMessage.Append(bookMigration.Message);
                    bookMigration.ReturnValue = true;

                }
                //ViewBag.Message = sbMessage.ToString();
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
                dt = (DataTable)Session["dtForMigration"];
                // save newly created datatable to database 
                CatalogRepository objCatalogRepository=new CatalogRepository();
               string result = objCatalogRepository.MigrteBook(dt);
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
}