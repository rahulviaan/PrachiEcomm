using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Net;
using System.Configuration;
using PrachiIndia.Sql;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using PrachiIndia.Web.Areas.Model;

namespace PrachiIndia.Portal.Helpers
{
    public static class Encryption
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("b9eafac5-27f5-446a-b8fa-f1a9f9b3788e");
        private static string key = "beb65e7b-1daa-4f8c-868a-0cffb08dfa23";
        public static string DecryptStringDES(string cipherText, string _Key)
        {
            string sharedSecret = _Key;
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
            RijndaelManaged aesAlg = null;
            string plaintext = "";
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }
        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
            string outStr = null;                       // Encrypted string to return 
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data. 
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return outStr;
        }
        public static string StringAES(string plainText)
        {
            if (plainText == String.Empty)
            {
                return "";
            }
            if (plainText == null)
            {
                return "";
            }
            string sharedSecret = key;
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return 
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data. 

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return outStr;
        }
        public static string StringDES(string cipherText)
        {
            if (cipherText == String.Empty)
            {
                return "";
            }
            string sharedSecret = key;
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");
            RijndaelManaged aesAlg = null;
            string plaintext = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }
        public static string EncryptCommon(string textToEncrypt, string _key = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textToEncrypt))
                {
                    return "";
                }
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                key = _key == "" ? key : _key;
                rijndaelCipher.KeySize = 0x80; // 256bit key
                rijndaelCipher.BlockSize = 0x80;
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
                return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string DecryptCommon(string textToDecrypt, string _key = "")
        {
            if (string.IsNullOrWhiteSpace(textToDecrypt))
            {
                return "";
            }
            key = _key == "" ? key : _key;
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[0x10];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

    }
    public static class Utility
    {
        static string connectionstring = Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"]);
        public static List<tblCataLog> objtblCataLog;

        public static string MakeFileNameWebSafe(string fileNameIn)
        {
            string pattern = @"[^A-Za-z0-9. ]";
            string safeFilename = System.Text.RegularExpressions.Regex.Replace(fileNameIn, pattern, string.Empty);
            if (safeFilename.StartsWith(".")) safeFilename = "noname" + safeFilename;

            return safeFilename;
        }
        public static string GetQueryString(string param)
        {
            string QueryString = "";
            if (HttpContext.Current.Request.QueryString[param] != null)
            {
                QueryString = HttpContext.Current.Request.QueryString[param].ToString();
            }
            return QueryString;
        }

        static public string IPAddress()
        {
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public static string IsActive(this HtmlHelper html, string control, string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "active" : "";
        }
        public static string ValidateEmail(string Email)
        {

            string mailto = "";
            string[] tmpEmail = Email.Split(',');
            foreach (string ms in tmpEmail)
            {
                bool isEmail = Regex.IsMatch(ms, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail)
                {
                    mailto += ms + ",";
                }
            }
            mailto = mailto + "avashishta@gmail.com";
            return mailto;
        }
        public static string IsActive(this HtmlHelper html, string control, string action, string id)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "text-danger" : "";
        }
        public static string EncodeQueryStringId(string encodeMe)
        {

            var DecodedString = "";
            try
            {

                byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
                DecodedString = Convert.ToBase64String(encoded);
            }
            catch
            {
                DecodedString = "";
            }
            return DecodedString;


        }
        public static string SubDomain()
        {
            string subdomain = "";
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var DomainString = HttpContext.Current.Request.Url.Host.ToLower().Replace("yocustomer.com", "");
                if (DomainString.Contains("."))
                {
                    subdomain = HttpContext.Current.Request.Url.Host.Split(new[] { '.' })[0].ToLower();
                }
                else
                    subdomain = "prachi";
            }
            else
            {
                subdomain = HttpContext.Current.User.Identity.Name.Split('.')[1];
            }
            return subdomain;
        }
        public static string DecodeQueryStringId(string decodeMe)
        {


            var EncodedString = "";
            try
            {

                byte[] encoded = Convert.FromBase64String(decodeMe);
                EncodedString = System.Text.Encoding.UTF8.GetString(encoded);
            }
            catch
            {
                EncodedString = "";
            }
            return EncodedString;


        }
        public static int RandomNumber()
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
        public static string ToDateTime(DateTime dt)
        {


            return dt.ToString("MMM dd, yyyy");


        }
        public static string ToDateTimeDetails(DateTime? dt)
        {
            //string ap = string.Format("{0:t tt}", dt)=="P PM"?"PM":"AM";
            //String.Format("{0:dddd, MMMM d, yyyy}", dt);
            return String.Format("{0:f}", dt);


        }
        public static string ToDateTimeDisplay(DateTime? dt)
        {
            //string ap = string.Format("{0:t tt}", dt)=="P PM"?"PM":"AM";
            //String.Format("{0:dddd, MMMM d, yyyy}", dt);
            if (dt == null)
            {

                return "";
            }
            else
            {
                if (dt.ToString() == "01/01/1900 12:00:00 AM")
                {
                    return "";
                }
                else
                    return String.Format("{0:f}", dt);
            }


        }
        public static string ToDateTimeYYMMDD(DateTime dt)
        {


            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();


        }
        public static string ToDateTimeDDMMMYYYY(DateTime dt)
        {


            return dt.ToString("dd") + " " + dt.ToString("MMMM") + " " + dt.Year.ToString();


        }
        public static string ToDateTimeDDMMMYYYY(DateTime? dt)
        {
            DateTime dt1 = (DateTime)dt;

            return dt1.ToString("dd") + " " + dt1.ToString("MMM") + " " + dt1.Year.ToString();


        }

        public static string getFileName()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
        }


        public static bool Upload(string path, HttpPostedFileBase postedFileFront, string fn, string ext)
        {
            bool isSaved = false;
            if (postedFileFront != null)
            {
                postedFileFront.SaveAs(path + "\\" + fn);
                isSaved = true;
            }

            return isSaved;
        }
        public static bool Move(string src, string dest, string fn)
        {
            bool isMove = false;

            try
            {
                File.Copy(src + fn, dest + fn);
                File.Delete(src + fn);
                isMove = true;
            }
            catch
            {
                isMove = false;
            };

            return isMove;

        }
        public static bool Copy(string src, string dest)
        {
            bool isCopy = false;

            try
            {
                File.Copy(src, dest);
                isCopy = true;
            }
            catch
            {
                isCopy = false;
            };

            return isCopy;

        }
        public static bool Delete(string path)
        {
            bool isDeleted = false;
            try
            {

                if (File.Exists(path))
                {

                    File.Delete(path);
                    //File.Delete(path.Replace(".", "_th."));
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
            return (obj ?? string.Empty).ToString().Trim();
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

        public static byte[] ToSafeByteFromString(string obj)
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


        private static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        #endregion


        public static IEnumerable<SelectListItem> ToSelectLists<TEnum>()
        {
            var myEnumDescriptions = from TEnum n in Enum.GetValues(typeof(TEnum))
                                     select new SelectListItem
                                     {
                                         Text = GetEnumDescription(n),
                                         Value = n.GetHashCode().ToString()
                                     };
            return myEnumDescriptions;
        }

        public static string classesByClassID(string ClassID)
        {
            var context = new dbPrachiIndia_PortalEntities();
            int CID = Convert.ToInt32(ClassID);
            var Class = context.MasterClasses.Where(x => x.Id == CID).Select(x => new { Class = x.Title }).ToList().First();
            return Class.Class;
        }

        public static string BoardsByClassID(string BoardID)
        {
            var context = new dbPrachiIndia_PortalEntities();
            int BID = Convert.ToInt32(BoardID);
            var Board = context.MasterBoards.Where(x => x.Id == BID).Select(x => new { Board = x.Title }).ToList().First();
            return Board.Board;
        }
        public static string SeriesByID(long? SeriesID)
        {
            dbPrachiIndia_PortalEntities context=(dbPrachiIndia_PortalEntities)Factory.FactoryRepository.GetInstance(RepositoryType.dbPrachiIndia_PortalEntities);
            int SID = Convert.ToInt32(SeriesID);
            var Series = context.MasterSeries.Where(x => x.Id == SID).Select(x => new { Series = x.Title }).ToList().FirstOrDefault();
            return Series.Series;
        }

        public static void LoadCatalogue()
        {
            dbPrachiIndia_PortalEntities context = new dbPrachiIndia_PortalEntities();
            objtblCataLog = context.tblCataLogs.ToList<tblCataLog>();
            objtblCataLog = objtblCataLog.Where(t => t.Status == 1).OrderBy(x => Convert.ToInt32(x.ClassId)).ToList();
        }
        public static string ConvertToXML(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }
        public static T ConvertFromXML<T>(string xmlText)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (StringReader stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {

                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }



        #region "Validation"







        public static DateTime ToSafeDate1(object obj)
        {
            if (obj == null || obj == "" || obj == System.DBNull.Value)
                return Convert.ToDateTime("1/1/1900");
            else
                return Convert.ToDateTime(obj);
        }
        #endregion






        public static DataTable GetUserByRole(string Type)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                var sda = new SqlDataAdapter("USP_GetUserByRole", con);
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sda.SelectCommand.Parameters.AddWithValue("@EmpType", Type);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
        }
        public static string IsEmailExists(string EmailID)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand("USP_CheckEmailExist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailID", EmailID);
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 30);
                cmd.Parameters["@Status"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return cmd.Parameters["@Status"].Value.ToString();
            }
        }

        public static string ChapterbyTitle(int TitleID)
        {
            var context = new dbPrachiIndia_PortalEntities();
            int Tid = Convert.ToInt32(TitleID);
            var ChapterID = context.TopicMasters.Where(x => x.ID == Tid).ToList().First();
            if (ChapterID != null)
            {
                var chid = Convert.ToInt64(ChapterID.CHAPTERID);
                var Chapters = context.Chapters.Where(x => x.Id == chid).ToList().First();
                return Chapters.Title;
            }
            return "";
        }

        public static DataTable RemoveBlankRows(DataTable dt)
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
    }

}