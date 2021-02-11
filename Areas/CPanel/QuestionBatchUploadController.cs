using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrachiIndia.Portal.Helpers;
using System.Text;
using PrachiIndia.Portal.Areas.CPanel.Models;

namespace PrachiIndia.Portal.Areas.CPanel
{
    [Authorize]
    public class QuestionBatchUploadController : Controller
    {
        DataTable dt = new DataTable();
        StringBuilder sbMessage = new StringBuilder();
        DataTable dtBinding = new DataTable();
        QuestionUpload objQuestionUpload;
        internal static string fldFilePath;
        internal static string fldFileName;
        internal static string Type;
        // GET: CPanel/QuestionBatchUpload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save() {
            return View();
        }

        [HttpPost]
        [ActionName("Save")]
        public ActionResult Save_Data()
        {
                if (SaveData(Type))
                {
                    return View("Save");
                }
            return View();
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file_Uploader, string QType)
        {
            Type = QType;
            if (file_Uploader != null)
            {
                string fileName = string.Empty;
                string destinationPath = string.Empty;
                string UploadedFileName = string.Empty;
                string message = string.Empty;
                string FileInDirectory = string.Empty;
                //EmployeeType = Convert.ToInt32(Type);
                fileName = file_Uploader.FileName.ToLower();

                if ((fileName != "question.xls" || fileName == "question.xlsx") && (fileName != "question.xlsx" || fileName == "question.xls"))
                {
                    ViewBag.Message = "<div style='color:red'>Invalid file uploaded , You have to upload question.xls / question.xlsx</div>";
                    return View("Index");
                }

                if (!System.IO.Directory.Exists(Server.MapPath("~/Areas/Migration/Question/Upload")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Areas/Migration/Question/Upload"));
                }
                destinationPath = Server.MapPath("~/Areas/Migration/Question/Upload" + fileName);
 

                //Rahul srivastava
                //Deleting files from the folder
                Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/Question/Upload")), System.IO.File.Delete);

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
                dt = Utility.RemoveBlankRows(dt);

                Session.Remove("dtForMigration_question");
                Session["dtForMigration_question"] = dt;
                if (dt.Rows.Count <= 0)
                {
                    ViewBag.Message = "File does not have any record please upload valid file";
                    Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/Question/Upload")), System.IO.File.Delete);
                    return View("index");
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
                        return View("index");
                    }
                }


            }
            ViewBag.Message = "Please choose the file first to upload !";
            return View("Index");
        }

        public ActionResult DownloadFile()
        {
            try
            {
                return File(new FileStream(Server.MapPath("~/Areas/Migration/Question/Upload/question.xlsx"), FileMode.Open), "application/octetstream", "question.xlsx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean Validate(DataTable dt)
        {
            try
            {
                objQuestionUpload = new QuestionUpload();
                int cnt = 0;

                //Validate NULL values:
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["BookID"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter BookID at row " + RowNo + "<br>");
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ChapterID"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter ChapterID at row " + RowNo + "<br>");
                    }
                }

             
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TopicID"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter TopicID at row " + RowNo + "<br>");
                    }
                }

             
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["CategoryID"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter CategoryID at row " + RowNo + "<br>");
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["TypeID"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter TypeID at row " + RowNo + "<br>");
                    }
                }
            
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Question"].ToString() == "")
                    {
                        int RowNo = i + 2;
                        if (cnt != cnt + 1)
                            cnt = cnt + 1;
                        sbMessage.Append("<li>Please Enter Question at row " + RowNo + "<br>");
                    }
                }


                if (cnt == 7)
                    sbMessage.Append("File does not have any mandatory record.Please upload valid file.");
                if (sbMessage.ToString() != "")
                {
                    TempData["Message"] = "<ul style ='list-style:square;'>" + sbMessage.ToString() + "</ul>";

                    Array.ForEach(Directory.GetFiles(Server.MapPath("~/Areas/Migration/Question/Upload")), System.IO.File.Delete);
                }
                else
                {
                    
                    objQuestionUpload.Message = sbMessage.ToString();

                    sbMessage.Append(objQuestionUpload.Message);
                    objQuestionUpload.ReturnValue = true;

                }

                return objQuestionUpload.ReturnValue;
            }
            catch (Exception ex)
            {

                sbMessage.Append(ex.Message);
                return false;
            }

        }

        private Boolean SaveData(string Type)
        {
        
                dt = (DataTable)Session["dtForMigration_question"];
                // save newly created datatable to database 
                objQuestionUpload= new QuestionUpload();
                int output = objQuestionUpload.UploadQuestion(dt, Type);
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
    }
}