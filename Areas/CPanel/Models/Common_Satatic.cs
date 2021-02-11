using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Xml;
using System.Collections;
using System.Configuration;

namespace PrachiIndia.Web.Areas.Model
{

    public static class Utilities
    {
        public static string IsActive(this HtmlHelper html, string control, string action, string id = "")
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];
            var returnActive = false;
            if (id == "")
            {
                returnActive = control == routeControl && action == routeAction;
            }
            else
            {
                var id1 = (string)routeData.Values["id"];
                returnActive = (control == routeControl) && (action == routeAction) && (id1 == id);
            }


            return returnActive ? "active" : "";
        }
    }
    public static class QSEncDec
    {
        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }
        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
    }
    public static class Common_Static
    {
        public static List<SelectListItem> lstStatus()
        {
            var lstSec = new List<SelectListItem>()
            {

                new SelectListItem { Text = "Inactive", Value = "0" },
                new SelectListItem { Text = "Active", Value = "1" },
                new SelectListItem { Text = "All", Value = "2" },

            };
            return lstSec;
        }

        public static string IPAddress()
        {
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }
        public static byte[] ConvertToBytes(HttpPostedFileBase Image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(Image.InputStream);
            imageBytes = reader.ReadBytes((int)Image.ContentLength);
            return imageBytes;
        }

        public static object ToDateTimeDDMMYYYY(DateTime? dt)
        {
            return String.Format("{0:d MMM yyyy}", dt);
        }

        public static string PreparePOSTForm(string url, Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key + "\"  id=\"" + key.Key + "\" value=\"" + key.Value + "\">");
            }

            strForm.Append("</form>");

            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script lang='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");


            return strForm.ToString() + strScript.ToString();
        }
        public static string SizeOnDisk(string fn)
        {
            string result = "";
            try
            {

                string[] sizes = { "B", "KB", "MB", "GB" };


                double len = 0;
                if (fn != "")
                {
                    len = new System.IO.FileInfo(fn).Length;
                }

                int order = 0;
                while (len >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    len = len / 1024;
                }
                result = String.Format("{0:0.##} {1}", len, sizes[order]);
            }
            catch
            { }
            return result;
        }
        public static string GetColor(int i)
        {
            string cr;
            if (i == 0)
            {
                cr = "#D3D3D3";
            }
            else if (i == 1)
            {
                cr = "#FFFFE0";
            }

            else if (i == 2)
            {
                cr = "#FFB6C1";
            }
            else if (i == 3)
            {
                cr = "#20B2AA";
            }
            else if (i == 4)
            {
                cr = "#87CEFA";
            }
            else if (i == 5)
            {
                cr = "#F5DEB3";
            }
            else if (i == 6)
            {
                cr = "#61f7ee";
            }
            else if (i == 7)
            {
                cr = "#FFAAAA";
            }
            else
            {
                cr = "";
            }
            return cr;
        }

        public static int Number()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            return myRandomNo;
        }


        public static string Key()
        {
            string st1 = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            st1 = st1.Length > 6 ? st1.Substring(0, 6) : st1;
            return st1.ToUpper();
        }

        public static string UniqueKey()
        {
            string st1 = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            string st2 = DateTime.Now.ToString().GetHashCode().ToString("x");
            string st3 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int len = st1.Length > st2.Length ? st1.Length : st2.Length;
            len = len > st3.Length ? len : st3.Length;
            string _key = "";
            for (int i = 0; i < len; i++)
            {
                if (i <= st3.Length - 1)
                {
                    _key += st3[i].ToString();
                }
                if (i <= st2.Length - 1)
                {
                    _key += st2[i].ToString();
                }
                if (i <= st1.Length - 1)
                {
                    _key += st1[i].ToString();
                }
            }

            return _key;
        }

        public static string UniqueKey(string id)
        {
            string st1 = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            string st2 = DateTime.Now.ToString().GetHashCode().ToString("x");
            string st3 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int len = st1.Length > st2.Length ? st1.Length : st2.Length;
            len = len > st3.Length ? len : st3.Length;
            string _key = "";
            for (int i = 0; i < len; i++)
            {
                if (i <= st3.Length - 1)
                {
                    _key += st3[i].ToString();
                }
                if (i <= st2.Length - 1)
                {
                    _key += st2[i].ToString();
                }
                if (i <= st1.Length - 1)
                {
                    _key += st1[i].ToString();
                }
            }

            return _key + id;
        }



        public static string ToSafeSubString(string obj, int len)
        {
            string res = ToSafeString(obj);
            string ret = "";
            if (res.Length <= len)
            {
                ret = res;
            }
            else
            {
                ret = res.Substring(0, len);
            }
            return ret;
        }
        public static void CopyAllFilesFromMaster(string src, string dest)
        {
            foreach (string fle in Directory.EnumerateFiles(src, "*.*"))
            {
                try
                {
                    string[] pathArr = fle.Split('\\');
                    string fileName = pathArr.Last();
                    if (File.Exists(dest + "\\" + fileName))
                    {
                        File.Delete(dest + "\\" + fileName);
                    }
                    System.IO.File.Copy(fle, dest + "\\" + fileName);
                }
                catch { }

            }

        }
        public static void CleanObjet(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Type st = property.PropertyType;
                string st1 = property.Name;
                if (st.Name == "String")
                {
                    var ra = property.GetValue(obj, null);
                    ra = ra == "null" || ra == null ? "" : ra;
                    //ra = getCleanSQLParameters(ra.ToString());
                    if (property.SetMethod != null)
                        property.SetValue(obj, ra, null);
                }
            }
        }
        public static string getCleanSQLParameters(string strSQLParameter)
        {
            if (!string.IsNullOrEmpty(strSQLParameter))
            {
                strSQLParameter = (strSQLParameter).Trim();
                //   strSQLParameter = (strSQLParameter).Replace("'", "''");
                //        strSQLParameter=(strSQLParameter).Replace("","");
                strSQLParameter = (strSQLParameter).Replace(")", "");
                strSQLParameter = (strSQLParameter).Replace("(", "");
                //  strSQLParameter = (strSQLParameter).Replace(";", "");
                //   strSQLParameter = (strSQLParameter).Replace("-", "");
                strSQLParameter = (strSQLParameter).Replace("|", "");
                //   strSQLParameter = (strSQLParameter).Replace("<", "");
                //strSQLParameter = (strSQLParameter).Replace(">", "");
            }
            else
            {
                strSQLParameter = "";
            }
            return strSQLParameter;
        }
        public static string ToDateTime(DateTime dt)
        {


            return dt.ToString("MMM dd, yyyy");


        }
        public static string ToDateTimeDetails(DateTime? dt)
        {
            //string ap = string.Format("{0:t tt}", dt)=="P PM"?"PM":"AM";
            String.Format("{0:dddd, MMMM d, yyyy}", dt);
            //
            return String.Format("{0:f}", dt);


        }

        public static string ToDateTimeDisplay(DateTime? dt)
        {
            //string ap = string.Format("{0:t tt}", dt)=="P PM"?"PM":"AM";
            //String.Format("{0:dddd, MMMM d, yyyy}", dt);


            return String.Format("{0:ddd, MMM d, yyyy}", dt);


        }
        public static string ToDateTimeYYMMDD(DateTime dt)
        {


            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();


        }

        public static string ToDateTimeDDMMYYYY(DateTime dt)
        {


            return String.Format("{0:d MMM yyyy}", dt);


        }

        #region "Validation"

        public static bool ValidateDate(string stringDateValue)
        {
            try
            {
                System.Globalization.CultureInfo CultureInfoDateCulture = new System.Globalization.CultureInfo("en-US");
                DateTime d = DateTime.ParseExact(stringDateValue, "MM/dd/yyyy", CultureInfoDateCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsItNumber(string inputvalue)
        {
            //            IsItNumber("2"); will return true;
            //IsItNumber("A"); will return false;
            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }
        public static bool IsItDecimalNumber(string inputvalue)
        {

            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("^([0-9]*)(\\.[0-9]{2})?$");
            return !isnumber.IsMatch(inputvalue);
        }
        public static bool IsItAlphabate(string inputvalue)
        {

            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("^[a-zA-Z]*$");
            return !isnumber.IsMatch(inputvalue);
        }
        public static bool IsItAlphaNumeric(string inputvalue)
        {

            System.Text.RegularExpressions.Regex isnumber = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]*$");
            return !isnumber.IsMatch(inputvalue);
        }
        public static string ToSafeString(object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        public static string ToSafeURL(string url)
        {
            url = url.Replace("&", "and");
            url = url.Replace("*", "_");
            url = url.Replace("/", "_");
            return url;
        }
        public static int ISNULL(string str)
        {
            if (str == "")
                return 0;
            else
                return Convert.ToInt32(str);
        }
        public static int ToSafeInt(object obj)
        {
            if (obj == null || obj == "")
                return 0;
            else
                return ISNULL(Convert.ToString(obj));
        }

        public static byte[] ToSafeByte1(string obj)
        {
            byte[] array = new byte[0];
            if (obj == null || obj == "")
                return array;
            else
                return Encoding.UTF8.GetBytes(obj);
        }
        internal static byte[] ToSafeByte(object p)
        {
            byte[] array = new byte[0];
            if (p == null || p == System.DBNull.Value)
                return array;
            else
                return (byte[])p;
        }
        public static double ToSafeDouble(object obj)
        {
            if (obj == null || obj == "")
                return 0;
            else
                return Convert.ToDouble(obj);
        }
        public static decimal ToSafeDecimal(object obj)
        {
            if (obj == null || obj == "")
                return 0;
            else
                return Convert.ToDecimal(obj);
        }
        public static DateTime? ToSafeDate(object obj)
        {
            if (obj == null || obj == "" || obj == System.DBNull.Value)
                return Convert.ToDateTime("1/1/1900");
            else
                return Convert.ToDateTime(obj);
        }
        #endregion

        public static string getFileName()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
        }



        public static bool Upload(string path, HttpPostedFile postedFileFront, string fn, string ext)
        {
            bool isSaved = false;
            if (postedFileFront != null)
            {
                postedFileFront.SaveAs(path + "\\" + fn + ext);
                isSaved = true;
            }

            return isSaved;
        }

        public static bool Upload(string path, HttpPostedFileBase postedFileFront, string fn, string ext)
        {
            bool isSaved = false;
            if (postedFileFront != null)
            {
                postedFileFront.SaveAs(path + "\\" + fn + ext);
                isSaved = true;
            }

            return isSaved;
        }

        public static bool Delete(string path)
        {
            bool isDeleted = false;
            try
            {

                if (File.Exists(path))
                {

                    File.Delete(path);
                    File.Delete(path.Replace(".", "_th."));
                    isDeleted = true;
                }

            }
            catch { }

            return isDeleted;
        }


        public static bool supportedTypes(string ext)
        {
            string[] tp = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            return tp.Contains(ext.ToLower());

        }
        public static string UploadFileValidation(string ext)
        {
            string err = "";
            string[] validFileTypes = { "html", "htm", "png", "jpg", "jpeg", "mp4", "pdf" };
            bool isValidFile = false;
            for (int i = 0; i < validFileTypes.Length; i++)
            {
                if (ext == "." + validFileTypes[i])
                {
                    isValidFile = true;
                    break;
                }
            }
            if (!isValidFile)
            {

                err = "Invalid File. Please upload a File with extension " +
                               string.Join(",", validFileTypes);
            }

            else
            {
                err = "";
            }
            return err;
        }
        public static void ThumbnailLarge(string src, string dest, int NewWidth)
        {
            //  Fill.ThumbnailLarge(Server.MapPath("~/fileupload/DesignFile/"), Server.MapPath("~/fileupload/DesignFile/"), image, 150);                    

            try
            {
                if (!File.Exists(dest))
                {
                    int NewHeight;
                    System.Drawing.Image image = System.Drawing.Image.FromFile(src);
                    int width = image.Width;
                    double aspect = width / image.Height;
                    NewHeight = (int)(((double)NewWidth / (double)image.Width) * (double)image.Height);
                    System.Drawing.Image thumbnail = image.GetThumbnailImage(NewWidth, NewHeight, null, System.IntPtr.Zero);

                    thumbnail.Save(dest);
                    thumbnail.Dispose();
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }




        }


        public static string IsFileExist(string path, string file)
        {

            string isFile = "";
            try
            {
                if (System.IO.File.Exists(path + file))
                {
                    isFile = file;
                }
                else
                    isFile = "blank.jpg";

            }
            catch (Exception ex)
            {
                isFile = "blank.jpg";
            }

            return isFile;


        }



        public static string TimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public static string ApiUrl()
        {
            string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["WebApiURL"].ToString();
            return ApiUrl;
        }

        public static string ReadedgeConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            }

        }

        public static string ImgAccess()
        {
            string ImgAccess = System.Configuration.ConfigurationManager.AppSettings["ImgAccessType"].ToString();
            return ImgAccess;
        }
        public static string ImageeServerURL(string urlType)
        {
            string ImageeServerURL = "";
            if (urlType == "EversionOutside")
            {
                ImageeServerURL = System.Configuration.ConfigurationManager.AppSettings["ImageeServer"].ToString();
            }
            else
            {
                ImageeServerURL = System.Configuration.ConfigurationManager.AppSettings["ImageeServerLocal"].ToString();
            }
            return ImageeServerURL;
        }
        public static List<SelectListItem> lstPayStatus()
        {
            var lstSec = new List<SelectListItem>() {


                  new SelectListItem  { Text = "Success" ,Value="2" },
                 new SelectListItem  { Text = "Submit" ,Value="1" },
                 new SelectListItem  { Text = "All" ,Value="0" },
                };
            return lstSec;
        }
        public static List<SelectListItem> lstPayType()
        {
            var lstSec = new List<SelectListItem>() { 
                //new SelectListItem  { Text = "Online" ,Value="1" },
                new SelectListItem  { Text = "Check/DD" ,Value="2" },
                };
            return lstSec;
        }
        public static List<SelectListItem> lstVersionType()
        {
            var lstSec = new List<SelectListItem>()
            {


                new SelectListItem { Text = "E Version", Value = "0" },
                new SelectListItem { Text = "P Version", Value = "1" },
                new SelectListItem { Text = "All", Value = "-1" },
            };
            return lstSec;
        }



        public static List<SelectListItem> lstQueryType()
        {
            var lstSec = new List<SelectListItem>()
            {

                new SelectListItem { Text = "All", Value = "3" },
                new SelectListItem { Text = "Technical", Value = "1" },
                new SelectListItem { Text = "Operational", Value = "2" },

            };
            return lstSec;
        }

        public static List<SelectListItem> GetHelpDeskPriority()
        {
            var lstSec = new List<SelectListItem>()
            {
                new SelectListItem { Text = "All", Value = "3" },
                new SelectListItem { Text = "Normal", Value = "0" },
                new SelectListItem { Text = "Medium", Value = "1" },
                new SelectListItem { Text = "High", Value = "2" },

            };
            return lstSec;
        }

        public static List<SelectListItem> GetHelpDeskType()
        {
            var lstSec = new List<SelectListItem>()
            {
                  new SelectListItem { Text = "All", Value = "2" },
                new SelectListItem { Text = "P Version", Value = "1" },
                new SelectListItem { Text = "E Version", Value = "0" },
               new SelectListItem { Text = "E Version Hindi", Value = "3" },

            };
            return lstSec;
        }

        public static List<SelectListItem> GetComplainRegard()
        {
            var lstSec = new List<SelectListItem>()
            {
                 new SelectListItem { Text = "All", Value = "6" },
                new SelectListItem { Text = "Payment", Value = "1" },
                new SelectListItem { Text = "Downloading", Value = "2" },
                new SelectListItem { Text = "Reading News Paper", Value = "3" },
                new SelectListItem { Text = "P-Version Subscription", Value = "4" },
                new SelectListItem { Text = "E-Version Subscription", Value = "5" },

            };
            return lstSec;
        }


        public static string getLagugae(string isLanguage)
        {
            string retisLanguage = "English";
            switch (isLanguage)
            {
                case "en":
                    retisLanguage = "English";
                    break;
                case "hi":
                    retisLanguage = "Hindi";
                    break;
                case "ur":
                    retisLanguage = "Urdu";
                    break;
                default:
                    retisLanguage = "English";
                    break;
            }
            return retisLanguage;
        }

        public static List<SelectListItem> lstLagugae()
        {

            var lstSec = new List<SelectListItem>() { 
                    //new SelectListItem  { Text = "---Select Language" ,Value="0" },
                    new SelectListItem  { Text = "English" ,Value="en" },
                    new SelectListItem  { Text = "Hindi" ,Value="hi" },
                    new SelectListItem  { Text = "Urdu" ,Value="ur" },


                };
            return lstSec;
        }
        public static int getLanguageType()
        {
            var cookie = HttpContext.Current.Request.Cookies["Language"];
            var isType = 0;
            var lang = "English";
            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                if (cookie.Value == "en")
                {
                    lang = "English";
                }

                else
                {
                    lang = "हिन्दी";
                    isType = 1;
                }

            }
            return isType;
        }

        public static string getFolderPath(string isLang)
        {
            string FolderPath = "";
            if (isLang == "hi")
            {
                FolderPath = "~/Newspapers/Hindi";
            }
            else if (isLang == "en")
            {
                FolderPath = "~/Newspapers/English";
            }
            else if (isLang == "ur")
            {
                FolderPath = "~/Newspapers/Urdu";
            }
            return FolderPath;
        }

    }
    public class _Common
    {
        public string today { get; set; }
        public string weekly { get; set; }
        public string monthly { get; set; }
        public string total { get; set; }
    }

    public class clsCurrency
    {

        string Number;
        string deciml;
        string _number;
        string _deciml;
        string[] US = new string[1003];
        string[] SNu = new string[20];
        string[] SNt = new string[10];
        public clsCurrency()
        {
            SNu[0] = "";
            SNu[1] = "One";
            SNu[2] = "Two";
            SNu[3] = "Three";
            SNu[4] = "Four";
            SNu[5] = "Five";
            SNu[6] = "Six";
            SNu[7] = "Seven";
            SNu[8] = "Eight";
            SNu[9] = "Nine";
            SNu[10] = "Ten";
            SNu[11] = "Eleven";
            SNu[12] = "Twelve";
            SNu[13] = "Thirteen";
            SNu[14] = "Fourteen";
            SNu[15] = "Fifteen";
            SNu[16] = "Sixteen";
            SNu[17] = "Seventeen";
            SNu[18] = "Eighteen";
            SNu[19] = "Nineteen";
            SNt[2] = "Twenty";
            SNt[3] = "Thirty";
            SNt[4] = "Forty";
            SNt[5] = "Fifty";
            SNt[6] = "Sixty";
            SNt[7] = "Seventy";
            SNt[8] = "Eighty";
            SNt[9] = "Ninety";
            US[1] = "";
            US[2] = "Thousand";
            US[3] = "Million"; //"Million"
            US[4] = "Billion"; //"Billion"
            US[5] = "Trillion";
            US[6] = "Quadrillion";
            US[7] = "Quintillion";
            US[8] = "Sextillion";
            US[9] = "Septillion";
            US[10] = "Octillion";
        }

        public string ToConvertWord(double douValue)
        {
            string currency = " In Word(INR) :- ";// "Rupees "//;
            string _currency = " Paise Only";
            string stResult = "";
            if (douValue == 0)
            {
                stResult = "Null Value";

            }
            if (douValue < 0)
            {
                stResult = "Invalid Value";

            }
            string[] no = { "0", "0" };
            string[] temp = douValue.ToString().Split('.');
            if (temp.Length == 2)
            {
                no[0] = temp[0];
                no[1] = temp[1];
            }
            else
            {
                no[0] = temp[0];
            }
            if ((no[0] != null) && (Convert.ToInt32(no[1]) > 0))
            {
                Number = no[0];
                deciml = douValue.ToString("F2").Substring(douValue.ToString("F2").IndexOf('.') + 1);
                _number = (NameOfNumber(Number));
                _deciml = (NameOfNumber(deciml));
                stResult = currency + _number.Trim() + " and " + _deciml.Trim() + _currency;
            }
            if ((no[0] != null) && (Convert.ToInt32(no[1]) == 0))
            {
                Number = no[0];
                _number = (NameOfNumber(Number));
                stResult = currency + _number + "Only";
            }
            if ((Convert.ToDouble(no[0]) == 0) && (no[1] != null))
            {
                deciml = no[1];
                _deciml = (NameOfNumber(deciml));
                stResult = _deciml + _currency;
            }
            return stResult;
        }


        private string NameOfNumber(string Number)
        {
            string GroupName = "";
            string OutPut = "";

            if ((Number.Length % 3) != 0)
            {
                Number = Number.PadLeft((Number.Length + (3 - (Number.Length % 3))), '0');
            }
            string[] Array = new string[Number.Length / 3];
            Int16 Element = -1;
            Int32 DisplayCount = -1;
            bool LimitGroupsShowAll = false;
            int LimitGroups = 0;
            bool GroupToWords = true;
            for (Int16 Count = 0; Count <= Number.Length - 3; Count += 3)
            {
                Element += 1;
                Array[Element] = Number.Substring(Count, 3);

            }
            if (LimitGroups == 0)
            {
                LimitGroupsShowAll = true;
            }
            for (Int16 Count = 0; (Count <= ((Number.Length / 3) - 1)); Count++)
            {
                DisplayCount++;
                if (((DisplayCount < LimitGroups) || LimitGroupsShowAll))
                {
                    if (Array[Count] == "000") continue;
                    {
                        GroupName = US[((Number.Length / 3) - 1) - Count + 1];
                    }


                    if ((GroupToWords == true))
                    {
                        OutPut += Group(Array[Count]).TrimEnd(' ') + " " + GroupName + " ";

                    }
                    else
                    {
                        OutPut += Array[Count].TrimStart('0') + " " + GroupName;

                    }
                }

            }
            Array = null;
            return OutPut;

        }
        private string Group(string Argument)
        {
            string Hyphen = "";
            string OutPut = "";
            Int16 d1 = Convert.ToInt16(Argument.Substring(0, 1));
            Int16 d2 = Convert.ToInt16(Argument.Substring(1, 1));
            Int16 d3 = Convert.ToInt16(Argument.Substring(2, 1));
            if ((d1 >= 1))
            {
                OutPut += SNu[d1] + " Hundred ";
            }
            if ((double.Parse(Argument.Substring(1, 2)) < 20))
            {
                OutPut += SNu[Convert.ToInt16(Argument.Substring(1, 2))];
            }
            if ((double.Parse(Argument.Substring(1, 2)) >= 20))
            {
                if (Convert.ToInt16(Argument.Substring(2, 1)) == 0)
                {
                    Hyphen += " ";
                }
                else
                {
                    Hyphen += " ";
                }
                OutPut += SNt[d2] + Hyphen + SNu[d3];
            }
            return OutPut;
        }

    }
}
