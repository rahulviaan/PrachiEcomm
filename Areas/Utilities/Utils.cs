using System;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using PrachiIndia.Portal.Framework;
using PrachiIndia.Portal.Helpers;

namespace PrachiIndia.Portal.Areas.Utilities
{
    public static class Utils
    {
        #region Method:EscapeIllegalCharacter
        public static string EscapeIllegalCharacter(string strValue)
        {
            return strValue.Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("<br />", "\n");
        }
        #endregion

        #region Method:ReplaceIllegalCharacter
        public static string ReplaceIllegalCharacter(string strValue)
        {
            return strValue.Replace("&amp;", "&").Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">").Replace("<br />", "\n").Replace("&rsquo;", "'");
        }
        #endregion


    }

    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ///        
    }

    public class DBAccess
    {
        public SqlConnection moSqlConnection;
        public SqlCommand moSqlCommand;
        public SqlDataReader moSqlDataReader;
        public SqlTransaction moSqlTransaction;
        public SqlDataAdapter moSqlDataAdapter;
        public DataSet moDataSet;

        public DBAccess(String fsConnectionString)
        {
            moSqlConnection = new SqlConnection(fsConnectionString);
        }

        //Open Connection
        public void openConnection()
        {
            if (moSqlConnection.State == ConnectionState.Closed)
                moSqlConnection.Open();
            if (moSqlCommand != null && moSqlCommand.Transaction == null)
                moSqlCommand.Transaction = beginTransaction();
        }

        public void executeMyReader()
        {
            openConnection();
            moSqlDataReader = moSqlCommand.ExecuteReader();
        }

        public bool isRead()
        {
            return moSqlDataReader.Read();
        }

        public bool isNull(object foValue)
        {
            return (foValue == DBNull.Value || foValue == null);
        }

        public object getValue(String fsFieldName)
        {
            return moSqlDataReader[fsFieldName];
        }

        public object getValue(int fiFieldIndex)
        {
            return moSqlDataReader[fiFieldIndex];
        }

        public object getParameter(String fsParameter)
        {
            return moSqlCommand.Parameters[fsParameter].Value;
        }

        public object getParameter(int fiParameterIndex)
        {
            return moSqlCommand.Parameters[fiParameterIndex].Value;
        }

        public int executeMyQuery()
        {
            try
            {
                openConnection();
                int temp = Utility.ToSafeInt(Convert.ToString(moSqlCommand.ExecuteNonQuery()));
                closeConnection();
                return temp;
            }
            catch (Exception ex)
            {
                rollBackTransaction();
                throw ex;
            }
        }

        public object executeMyScalar()
        {
            try
            {
                openConnection();
                object temp = moSqlCommand.ExecuteScalar();
                closeConnection();
                return temp;
            }
            catch (Exception ex)
            {
                rollBackTransaction();
                throw ex;
            }
        }

        public DataSet getDataSet()
        {
            moSqlDataAdapter = new SqlDataAdapter(moSqlCommand);
            moDataSet = new DataSet();
            moDataSet.Clear();
            moSqlDataAdapter.Fill(moDataSet);
            moSqlDataAdapter.Dispose();
            return moDataSet;
        }

        public DataTable getDataTable()
        {
            try
            {
                moSqlDataAdapter = new SqlDataAdapter(moSqlCommand);
                moDataSet = new DataSet();
                moDataSet.Clear();
                moSqlDataAdapter.Fill(moDataSet);
                moSqlDataAdapter.Dispose();
                DataTable temp = moDataSet.Tables[0];
                closeConnection();
                return temp;
            }
            catch { return null; }
        }

        // This function initializes the SqlCommand object
        public void execute(String fsSQL, SQLType foSQLType)
        {
            moSqlCommand = new SqlCommand(fsSQL, moSqlConnection, moSqlTransaction);
            if (foSQLType == SQLType.IS_PROC)
                moSqlCommand.CommandType = CommandType.StoredProcedure;
        }

        // Length is an additional security
        public void addParam(String fsParameterName, SqlDbType foSqlDbType, object foValue, SqlDirection foSqlDirection, int fiLength)
        {
            SqlParameter loSqlParameter = new SqlParameter();
            loSqlParameter.ParameterName = fsParameterName;
            loSqlParameter.SqlDbType = foSqlDbType;
            loSqlParameter.Size = fiLength;
            if (foSqlDirection == SqlDirection.IN)
                loSqlParameter.Direction = ParameterDirection.Input;
            else if (foSqlDirection == SqlDirection.OUT)
                loSqlParameter.Direction = ParameterDirection.Output;
            loSqlParameter.Value = foValue;
            moSqlCommand.Parameters.Add(loSqlParameter);
        }

        public void addParam(String fsParameterName, SqlDbType foSqlDbType, object foValue, SqlDirection foSqlDirection)
        {
            SqlParameter loSqlParameter = new SqlParameter();
            loSqlParameter.ParameterName = fsParameterName;
            loSqlParameter.SqlDbType = foSqlDbType;
            if (foSqlDirection == SqlDirection.IN)
                loSqlParameter.Direction = ParameterDirection.Input;
            else if (foSqlDirection == SqlDirection.OUT)
                loSqlParameter.Direction = ParameterDirection.Output;
            loSqlParameter.Value = foValue;
            moSqlCommand.Parameters.Add(loSqlParameter);
        }

        public void addParam(String fsParameterName, SqlDbType foSqlDbType, SqlDirection foSqlDirection)
        {

            SqlParameter loSqlParameter = new SqlParameter();
            loSqlParameter.ParameterName = fsParameterName;
            loSqlParameter.SqlDbType = foSqlDbType;
            loSqlParameter.Size = 10;

            if (foSqlDirection == SqlDirection.IN)
                loSqlParameter.Direction = ParameterDirection.Input;
            else if (foSqlDirection == SqlDirection.OUT)
                loSqlParameter.Direction = ParameterDirection.Output;

            moSqlCommand.Parameters.Add(loSqlParameter);
        }

        public void addParam(String fsParameterName, SqlDbType foSqlDbType, SqlDirection foSqlDirection, int fiLength)
        {
            SqlParameter loSqlParameter = new SqlParameter();
            loSqlParameter.ParameterName = fsParameterName;
            loSqlParameter.SqlDbType = foSqlDbType;
            loSqlParameter.Size = 10;
            if (foSqlDirection == SqlDirection.IN)
                loSqlParameter.Direction = ParameterDirection.Input;
            else if (foSqlDirection == SqlDirection.OUT)
            {
                loSqlParameter.Direction = ParameterDirection.Output;
                loSqlParameter.Size = fiLength;
            }
            moSqlCommand.Parameters.Add(loSqlParameter);
        }

        // This function is used to Add a Parameter
        public void addParam(String fsParameterName, object foValue, SqlDirection foSqlDirection)
        {
            SqlParameter loSqlParameter = moSqlCommand.Parameters.AddWithValue(fsParameterName, foValue);
            if (foSqlDirection == SqlDirection.IN)
                loSqlParameter.Direction = ParameterDirection.Input;
            else if (foSqlDirection == SqlDirection.OUT)
                loSqlParameter.Direction = ParameterDirection.Output;
        }

        // This function is used to Add a Parameter
        public void addParam(String fsParameterName, object foValue)
        {
            addParam(fsParameterName, foValue, SqlDirection.IN);
        }

        // This function is used to Add a Parameter
        public void addParam(SqlParameter foSqlParameter)
        {
            moSqlCommand.Parameters.Add(foSqlParameter);
        }

        public void closeConnection()
        {
            commitTransaction();
            if (moSqlConnection.State == ConnectionState.Open)
                moSqlConnection.Close();
        }

        public void releasedReader()
        {
            if (moSqlDataReader != null)
                moSqlDataReader.Dispose();
            closeConnection();
        }

        public void releasedCommand()
        {
            if (moSqlCommand != null)
            {
                moSqlCommand.Dispose();
                moSqlCommand = null;
            }
            closeConnection();
        }

        public enum SQLType
        {
            IS_QUERY = 1,
            IS_PROC = 2
        }

        public enum SqlDirection
        {
            IN = 1,
            OUT = 2
        }

        public SqlTransaction beginTransaction()
        {
            try
            {
                if (moSqlTransaction == null)
                    moSqlTransaction = moSqlConnection.BeginTransaction();
            }
            catch (Exception feException)
            { }
            return moSqlTransaction;
        }

        public void commitTransaction()
        {
            try
            {
                if (moSqlTransaction != null)
                    moSqlTransaction.Commit();
            }
            catch (Exception feException)
            { }
        }

        public void rollBackTransaction()
        {
            try
            {
                if (moSqlTransaction != null)
                    moSqlTransaction.Rollback();
            }
            catch (Exception feException)
            { }
        }

        /// <summary>Advances the data reader to the next result, when reading the results of batch SQL statements.</summary>
        /// <returns>true if there are more rows; otherwise, false.</returns>
        public bool NextResult()
        {
            return moSqlDataReader.NextResult();
        }

        // Function to check whether a column exists in the Result Set
        public bool HasColumn(string fsColumnName)
        {
            return moSqlDataReader.GetSchemaTable().Select("ColumnName='" + fsColumnName + "'").Length > 0;
        }


    }

    public static class HtmlTagsRemoval
    {

        public static string GetCleanText(string Content, int SecurityLevel)
        {
            string CleanContent = string.Empty;
            if (SecurityLevel == 1)
            {
                CleanContent = CleanScriptTags(Content);
            }
            else if (SecurityLevel == 2)
            {
                CleanContent = CleanScriptTags(Content);
                CleanContent = RemoveMaliciousHtmlTags(CleanContent);
            }
            else if (SecurityLevel == 3)
            {
                CleanContent = CleanScriptTags(Content);
                CleanContent = RemoveMaliciousHtmlTags(CleanContent);
                CleanContent = CleanEmbedTag(CleanContent);
            }
            else if (SecurityLevel == 4)
            {
                CleanContent = CleanScriptTags(Content);
                CleanContent = RemoveMaliciousHtmlTags(CleanContent);
                CleanContent = CleanEmbedTag(CleanContent);
                CleanContent = CleanImgTag(CleanContent);
            }
            else if (SecurityLevel == 5)
            {
                CleanContent = CleanScriptTags(Content);
                CleanContent = RemoveMaliciousHtmlTags(CleanContent);
                CleanContent = CleanEmbedTag(CleanContent);
                CleanContent = CleanImgTag(CleanContent);
                CleanContent = CleanAllOtherTags(CleanContent);
            }


            return CleanContent;
        }

        private static string RemoveMaliciousHtmlTags(string Content)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Regex.Replace(Content,
                           @"(<applet>).*(</applet>)", string.Empty,
                           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                        @"(<body>).*(</body>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                Content = Regex.Replace(Content,
                       @"(<frame>).*(</frame>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                     @"(<frameset>).*(</frameset>)", string.Empty,
                       System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
             @"(<html>).*(</html>)", string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
              @"(<iframe>).*(</iframe>)", string.Empty,
              System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
               @"(<style>).*(</style>)", string.Empty,
           System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                      @"(<layer>).*(</layer>)", string.Empty,
                  System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                             @"(<link>).*(</link>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                                      @"(<ilayer>).*(</ilayer>)", string.Empty,
                                  System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                                            @"(<meta>).*(</meta>)", string.Empty,
                                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                                                    @"(<object>).*(</object>)", string.Empty,
                                                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                Content = Regex.Replace(Content,
                 @"(<meta>).*(</meta>)", string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                return Content;
            }
            else return string.Empty;
        }

        private static string CleanEmbedTag(string Content)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Regex.Replace(Content,
                                     @"(<embed>).*(</embed>)", string.Empty,
                                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                return Content;
            }
            else return string.Empty;
        }

        public static string CleanQueryString(string Content)
        {
            Content = GetCleanText(Content, 5);
            Content = Content.Replace("--", "");
            Content = Content.Replace(";", "");
            return Content;
        }

        public static string CleanQueryString(string Content, string DataType)
        {

            if (DataType == "int")
            {
                int number;
                bool result = Int32.TryParse(Content, out number);
                if (result)
                {
                    return CleanQueryString(number.ToString());
                }
                else
                {
                    return null;
                }

            }
            else if (DataType == "string")
            {
                return CleanQueryString(Content);
            }
            return null;
        }

        private static string CleanImgTag(string Content)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Regex.Replace(Content,
                            @"(<img>).*(</img>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                Content = Regex.Replace(Content,
                            @"(<img).*(</img>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                Content = Regex.Replace(Content,
                            @"(<img).*(/>)", string.Empty,
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                return Content;
            }
            else return string.Empty;
        }

        private static string CleanScriptTags(string Content)
        {
            try
            {
                string sMyScreenPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
                string[] values = sMyScreenPath.Split('/');
                string filename = values.Last();
                Match match = Regex.Match(Content, @"(<script).*(</script>)");
                if (match.Length > 0)
                {
                    //try
                    //{
                    //    SendMailToAdminForCleanTagsError(Content, filename);
                    //    LogIntoDatabase(filename);
                    //}

                    //catch { }
                }

                Content = Regex.Replace(Content,
                               @"(<script>).*(</script>)", string.Empty,
                               System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                Content = Regex.Replace(Content,
                               @"(<script).*(</script>)", string.Empty,
                               System.Text.RegularExpressions.RegexOptions.IgnoreCase);


                return Content;


            }
            catch { return string.Empty; }

        }

        private static string CleanAllOtherTags(string Content)
        {

            //  Content = Regex.Replace(Content, @"(<).*(>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(Content))
            {
                Content = Regex.Replace(Content, @"<.*?>", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                return Content;
            }
            else
                return string.Empty;
        }

        public static string ValidValueToAdd(string txtToAdd)
        {
            return Utilities.HtmlTagsRemoval.GetCleanText(txtToAdd.Replace("\n", string.Empty), 5);
        }

        public static string ValidValueToAdd(string txtToAdd, int MaxLength)
        {
            string valToadd = ValidValueToAdd(txtToAdd);
            if (MaxLength > 0)
                valToadd = valToadd.Length > MaxLength ? valToadd.Substring(0, MaxLength - 1) : valToadd;
            return valToadd;
        }
    }

   

    public class ImageSize
    {
        public ImageSize()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void ResizeAndSaveImage(HttpPostedFile fu, int Height, int Width, string FilePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(fu.InputStream);
            int ImageHeight, ImageWidth;

            ImageHeight = (int)image.PhysicalDimension.Height;
            ImageWidth = (int)image.PhysicalDimension.Width;
            if (ImageHeight <= Height && ImageWidth <= Width)
            {
                image.Save(FilePath, image.RawFormat);
                image.Dispose();
            }
            else
            {
                System.Drawing.Size NewSize = Image_Resize(Height, Width, ImageHeight, ImageWidth);

                System.Drawing.Bitmap img = (System.Drawing.Bitmap)image;
                var imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;

                Bitmap imgOutput = new Bitmap(img, NewSize.Width, NewSize.Height);
                Graphics myresizer = default(Graphics);
                myresizer = Graphics.FromImage(imgOutput);
                myresizer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                myresizer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                myresizer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                myresizer.DrawImage(img, 0, 0, NewSize.Width, NewSize.Height);
                try
                {
                    imgOutput.Save(FilePath, imageFormat);
                }
                catch { }
                myresizer.Dispose();
                imgOutput.Dispose();
                img.Dispose();
            }
        }

        public static Bitmap ResizeImage(HttpPostedFile fu, int height, int width)
        {
            var image = Image.FromStream(fu.InputStream);

            var imageHeight = (int)image.PhysicalDimension.Height;
            var imageWidth = (int)image.PhysicalDimension.Width;
            if (imageHeight <= height && imageWidth <= width)
            {
                var img = (Bitmap)image;
                return img;
            }
            else
            {
                var newSize = Image_Resize(height, width, imageHeight, imageWidth);

                var img = (Bitmap)image;

                var imgOutput = new Bitmap(img, newSize.Width, newSize.Height);
                var myresizer = Graphics.FromImage(imgOutput);
                myresizer.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                myresizer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                myresizer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                myresizer.DrawImage(img, 0, 0, newSize.Width, newSize.Height);
                myresizer.Dispose();
                img.Dispose();
                return imgOutput;
            }
        }

        public static Size Image_Resize(int container_height, int container_width, int src_height, int src_width)
        {
            int result_height = 0, result_width = 0;
            Size resultant_size = new Size();
            float src_ratio, trg_ratio, src_ratio_pos;

            src_ratio = (float)src_width / src_height;
            src_ratio_pos = (float)src_height / src_width;
            trg_ratio = (float)container_width / container_height;

            if (container_height > src_height && container_width > src_width)
            {
                result_height = src_height;
                result_width = src_width;
                //No resize required as the Source image is smaller then the target container
            }
            else if (container_height == 0 || container_width == 0)      //If the src height or width is '0' then only resize the non-zero dimension of the src_image. 
            {
                if (container_height == 0)
                {
                    result_height = src_height;
                    result_width = container_width;
                }
                else
                {
                    result_height = container_height;
                    result_width = src_width;
                }
            }
            else
            {
                if (src_ratio < trg_ratio)
                {
                    result_height = container_height;
                    result_width = (int)(result_height * src_ratio);
                }
                else
                {
                    result_width = container_width;
                    result_height = (int)(result_width * src_ratio_pos);
                }
            }
            resultant_size.Height = result_height;
            resultant_size.Width = result_width;
            return resultant_size;
        }

        public static Bitmap ResizeBitmap(Bitmap originalBitmap, int requiredHeight, int requiredWidth)
        {
            Size heightWidthRequiredDimensions;

            // Pass dimensions to worker method depending on image type required
            heightWidthRequiredDimensions = WorkDimensions(originalBitmap.Height, originalBitmap.Width, requiredHeight, requiredWidth);


            Bitmap resizedBitmap = new Bitmap(heightWidthRequiredDimensions.Width,
                                               heightWidthRequiredDimensions.Height);

            const float resolution = 72;

            resizedBitmap.SetResolution(resolution, resolution);

            Graphics graphic = Graphics.FromImage((Image)resizedBitmap);

            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

            graphic.Dispose();
            originalBitmap.Dispose();
            //resizedBitmap.Dispose(); // Still in use


            return resizedBitmap;
        }

        public static Bitmap CropImage(int Height, int Width, System.Drawing.Image original, string filename)
        {
            if (original.Height <= Height && original.Width <= Width)
            {
                System.Drawing.Bitmap img = (System.Drawing.Bitmap)original;

                return img;
            }

            else
            {
                System.Drawing.Size NewSize = ImageSize.Image_Resize(Height, Width, original.Height, original.Width);
                System.Drawing.Bitmap img = (System.Drawing.Bitmap)original;
                var imageFormat = img.RawFormat;

                Bitmap imgOutput = new Bitmap(img, NewSize.Width, NewSize.Height);
                Graphics myresizer = default(Graphics);
                myresizer = Graphics.FromImage(imgOutput);
                myresizer.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                myresizer.DrawImage(img, 0, 0, NewSize.Width, NewSize.Height);
                return imgOutput;

            }
        }

        private static Size WorkDimensions(int originalHeight, int originalWidth, int requiredHeight, int requiredWidth)
        {
            int imgHeight = 0;
            int imgWidth = 0;

            imgWidth = requiredHeight;
            imgHeight = requiredWidth;


            int requiredHeightLocal = originalHeight;
            int requiredWidthLocal = originalWidth;

            double ratio = 0;

            // Check height first
            // If original height exceeds maximum, get new height and work ratio.
            if (originalHeight > imgHeight)
            {
                ratio = double.Parse(((double)imgHeight / (double)originalHeight).ToString());
                requiredHeightLocal = imgHeight;
                requiredWidthLocal = (int)((decimal)originalWidth * (decimal)ratio);
            }

            // Check width second. It will most likely have been sized down enough
            // in the previous if statement. If not, change both dimensions here by width.
            // If new width exceeds maximum, get new width and height ratio.
            if (requiredWidthLocal >= imgWidth)
            {
                ratio = double.Parse(((double)imgWidth / (double)originalWidth).ToString());
                requiredWidthLocal = imgWidth;
                requiredHeightLocal = (int)((double)originalHeight * (double)ratio);
            }

            Size heightWidthDimensionArr = new Size();
            heightWidthDimensionArr.Height = requiredHeightLocal;
            heightWidthDimensionArr.Width = requiredWidthLocal;

            return heightWidthDimensionArr;
        }
    }


    public class SerialNo
    {
        static DBAccess dbAccess = new DBAccess(string.Empty);
        #region Without Cat
        public void AddSno(string ConStr, string TableName, string ColumnName, string TargetSno)
        {
            if (Convert.ToInt32(TargetSno) > 0)
            {
                string strGetData = " update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + ColumnName + ">0";
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void RemoveSno(string ConStr, string TableName, string ColumnName, string TargetSno)
        {
            if (Convert.ToInt32(TargetSno) > 0)
            {
                string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + TargetSno.ToString() + " And " + ColumnName + ">0";
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void MoveSrNo(string ConStr, string TableName, string ColumnName, int TargetSno, int SourceSno)
        {
            if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
            {
                string strGetData = string.Empty;
                if (SourceSno < TargetSno)
                    strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString() + " And " + ColumnName + ">0";
                else if (SourceSno > TargetSno)
                    strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString() + " And " + ColumnName + ">0";
                if (!string.IsNullOrEmpty(strGetData))
                {
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }
        }
        #endregion
        public void AddSnoCatSpecific(string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID)
        {
            if (Convert.ToInt32(TargetSno) > 0)
            {
                string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0";
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void RemoveSnoCatSpecific(string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID)
        {
            if (Convert.ToInt32(TargetSno) > 0)
            {
                string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + categoryColumnName + " = '" + CategoryID + "' and " + ColumnName + " > " + TargetSno.ToString() + " And " + ColumnName + ">0";
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void MoveSnoSameCategory(string ConStr, string TableName, string ColumnName, string TargetSno, string SourceSno, string categoryColumnName, string CategoryID)
        {
            if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
            {
                string strGetData = string.Empty;
                if (Convert.ToInt32(SourceSno) < Convert.ToInt32(TargetSno))
                    strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + " > 0";
                else if (Convert.ToInt32(SourceSno) > Convert.ToInt32(TargetSno))
                    strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + " > 0";
                if (!string.IsNullOrEmpty(strGetData))
                {
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }
        }
        public void MoveSnoDiffCategory(string ConStr, string TableName, string ColumnName, string CategoryColumn, string NewTargetSno, string OldSourceSno, string OldCategory, string NewCategory)
        {
            if (Convert.ToInt32(NewTargetSno) > 0 && Convert.ToInt32(OldSourceSno) > 0)
            {
                string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + OldSourceSno + " AND " + CategoryColumn + " = '" + OldCategory.ToString().ToString() + "' And " + ColumnName + ">0";
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();

                string strNewCatData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + NewTargetSno + " AND " + CategoryColumn + " = '" + NewCategory.ToString().ToString() + "' And " + ColumnName + ">0";
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void AddSnoNoCat(string ConStr, string TableName, string ColumnName, string TargetSno)
        {
            if (Convert.ToInt32(TargetSno) > 0)
            {
                string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString();
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void AddSno(string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID)
        {
            if (Convert.ToInt32(TargetSno) > 0)
            {
                string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + categoryColumnName + " = " + CategoryID + "And " + ColumnName + " > 0";
                if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                    dbAccess.moSqlConnection.Close();
                dbAccess.moSqlConnection.ConnectionString = ConStr;
                dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                dbAccess.executeMyQuery();
                dbAccess.releasedCommand();
            }
        }
        public void MoveSnoWithoutCategory(string ConStr, string TableName, string ColumnName, string TargetSno, string SourceSno)
        {
            if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
            {
                string strGetData = string.Empty;
                if (Utility.ToSafeInt(SourceSno) < Utility.ToSafeInt(TargetSno))
                    strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString();
                else if (Convert.ToInt32(SourceSno) > Convert.ToInt32(TargetSno))
                    strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString();
                if (!string.IsNullOrEmpty(strGetData))
                {
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }
        }
        public class SerialNoModuleInstanceBased
        {
            public void AddSno(string ConStr, string TableName, string ColumnName, string TargetSno)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + ColumnName + ">0";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void RemoveSno(string ConStr, string TableName, string ColumnName, string TargetSno)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + TargetSno.ToString() + " And " + ColumnName + ">0";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void MoveSrNo(string ConStr, string TableName, string ColumnName, int TargetSno, int SourceSno)
            {
                if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
                {
                    string strGetData = string.Empty;
                    if (SourceSno < TargetSno)
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString() + " And " + ColumnName + ">0";
                    else if (SourceSno > TargetSno)
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString() + " And " + ColumnName + ">0";
                    if (!string.IsNullOrEmpty(strGetData))
                    {
                        if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                            dbAccess.moSqlConnection.Close();
                        dbAccess.moSqlConnection.ConnectionString = ConStr;
                        dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                        dbAccess.executeMyQuery();
                        dbAccess.releasedCommand();
                    }
                }
            }

            public void AddSnoCatSpecific(string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID, int ModuleInstanceId)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void RemoveSnoCatSpecific(int ModuleInstanceId, string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + categoryColumnName + " = '" + CategoryID + "' and " + ColumnName + " > " + TargetSno.ToString() + " And " + ColumnName + ">0 AND fkModuleInstanceId =" + ModuleInstanceId + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void MoveSnoSameCategory(string ConStr, string TableName, string ColumnName, string TargetSno, string SourceSno, string categoryColumnName, string CategoryID, int ModuleInstanceId)
            {
                if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
                {
                    string strGetData = string.Empty;
                    if (Utility.ToSafeInt(SourceSno) < Utility.ToSafeInt(TargetSno))
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "";
                    else if (Utility.ToSafeInt(SourceSno) > Utility.ToSafeInt(TargetSno))
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "";
                    if (!string.IsNullOrEmpty(strGetData))
                    {
                        if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                            dbAccess.moSqlConnection.Close();
                        dbAccess.moSqlConnection.ConnectionString = ConStr;
                        dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                        dbAccess.executeMyQuery();
                        dbAccess.releasedCommand();
                    }
                }
            }

            public void MoveSnoDiffCategory(string ConStr, string TableName, string ColumnName, string CategoryColumn, string NewTargetSno, string OldSourceSno, string OldCategory, string NewCategory, int ModuleInstanceId)
            {
                if (Convert.ToInt32(NewTargetSno) > 0 && Convert.ToInt32(OldSourceSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + OldSourceSno + " AND " + CategoryColumn + " = '" + OldCategory.ToString().ToString() + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                    string strNewCatData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + NewTargetSno + " AND " + CategoryColumn + " = '" + NewCategory.ToString().ToString() + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }
        }
        public class SerialNoModuleInstanceSectionBased
        {
            public void AddSno(string ConStr, string TableName, string ColumnName, string TargetSno)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + ColumnName + ">0";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void RemoveSno(string ConStr, string TableName, string ColumnName, string TargetSno)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + TargetSno.ToString() + " And " + ColumnName + ">0";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void MoveSrNo(string ConStr, string TableName, string ColumnName, int TargetSno, int SourceSno)
            {
                if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
                {
                    string strGetData = string.Empty;
                    if (SourceSno < TargetSno)
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString() + " And " + ColumnName + ">0";
                    else if (SourceSno > TargetSno)
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString() + " And " + ColumnName + ">0";
                    if (!string.IsNullOrEmpty(strGetData))
                    {
                        if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                            dbAccess.moSqlConnection.Close();
                        dbAccess.moSqlConnection.ConnectionString = ConStr;
                        dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                        dbAccess.executeMyQuery();
                        dbAccess.releasedCommand();
                    }
                }
            }

            public void AddSnoCatSpecific(string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID, int ModuleInstanceId, int fkCategoryID, string CategoryColumnName)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + " AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void RemoveSnoCatSpecific(int ModuleInstanceId, string ConStr, string TableName, string ColumnName, string TargetSno, string categoryColumnName, string CategoryID, int fkCategoryID, string CategoryColumnName)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + categoryColumnName + " = '" + CategoryID + "' and " + ColumnName + " > " + TargetSno.ToString() + " And " + ColumnName + ">0 AND fkModuleInstanceId =" + ModuleInstanceId + "  AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void MoveSnoSameCategory(string ConStr, string TableName, string ColumnName, string TargetSno, string SourceSno, string categoryColumnName, string CategoryID, int ModuleInstanceId, int fkCategoryID, string CategoryColumnName)
            {
                if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
                {
                    string strGetData = string.Empty;
                    if (Utility.ToSafeInt(SourceSno) < Utility.ToSafeInt(TargetSno))
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + SourceSno.ToString() + " AND " + ColumnName + " <= " + TargetSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + " AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    else if (Utility.ToSafeInt(SourceSno) > Utility.ToSafeInt(TargetSno))
                        strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + TargetSno.ToString() + " AND " + ColumnName + " < " + SourceSno.ToString() + " And " + categoryColumnName + " = '" + CategoryID + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "  AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (!string.IsNullOrEmpty(strGetData))
                    {
                        if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                            dbAccess.moSqlConnection.Close();
                        dbAccess.moSqlConnection.ConnectionString = ConStr;
                        dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                        dbAccess.executeMyQuery();
                        dbAccess.releasedCommand();
                    }
                }
            }

            public void MoveSnoDiffCategory(string ConStr, string TableName, string ColumnName, string CategoryColumn, string NewTargetSno, string OldSourceSno, string OldCategory, string NewCategory, int ModuleInstanceId, string Section, int fkCategoryID, string CategoryColumnName)
            {
                if (Convert.ToInt32(NewTargetSno) > 0 && Convert.ToInt32(OldSourceSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " - 1 where " + ColumnName + " > " + OldSourceSno + " AND " + CategoryColumn + " = '" + OldCategory.ToString().ToString() + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + " AND Section = '" + Section + "'  AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                    string strNewCatData = "update " + TableName + " set " + ColumnName + "=" + ColumnName + " + 1 where " + ColumnName + " >= " + NewTargetSno + " AND " + CategoryColumn + " = '" + NewCategory.ToString().ToString() + "' And " + ColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "  AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            // ******************

            public void RemoveSrNoCatAndSectionSpecific(int ModuleInstanceId, string ConStr, string TableName, string SrNoColumnName, string TargetSno, string SectionColumnName, string Section, string CategoryColumnName, int fkCategoryID)
            {
                if (Convert.ToInt32(TargetSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + SrNoColumnName + "=" + SrNoColumnName + " - 1 where " + SectionColumnName + " = '" + Section + "' and " + SrNoColumnName + " > " + TargetSno.ToString() + " And " + SrNoColumnName + ">0 AND fkModuleInstanceId =" + ModuleInstanceId + "  AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public void MoveSnoSameCatAndSameSection(string ConStr, string TableName, string SrNoColumnName, string TargetSno, string SourceSno, string SectionColumnName, string Section, int ModuleInstanceId, int fkCategoryID, string CategoryColumnName)
            {
                if (Convert.ToInt32(TargetSno) > 0 && Convert.ToInt32(SourceSno) > 0)
                {
                    string strGetData = string.Empty;
                    if (Utility.ToSafeInt(SourceSno) < Utility.ToSafeInt(TargetSno))
                        strGetData = "update " + TableName + " set " + SrNoColumnName + "=" + SrNoColumnName + " - 1 where " + SrNoColumnName + " > " + SourceSno.ToString() + " AND " + SrNoColumnName + " <= " + TargetSno.ToString() + " And " + SectionColumnName + " = '" + Section + "' And " + SrNoColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + " AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    else if (Utility.ToSafeInt(SourceSno) > Utility.ToSafeInt(TargetSno))
                        strGetData = "update " + TableName + " set " + SrNoColumnName + "=" + SrNoColumnName + " + 1 where " + SrNoColumnName + " >= " + TargetSno.ToString() + " AND " + SrNoColumnName + " < " + SourceSno.ToString() + " And " + SectionColumnName + " = '" + Section + "' And " + SrNoColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + "  AND " + CategoryColumnName + " = " + fkCategoryID + "";
                    if (!string.IsNullOrEmpty(strGetData))
                    {
                        if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                            dbAccess.moSqlConnection.Close();
                        dbAccess.moSqlConnection.ConnectionString = ConStr;
                        dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                        dbAccess.executeMyQuery();
                        dbAccess.releasedCommand();
                    }
                }
            }

            public void RemoveSrNoWithCatWithSection(string ConStr, string TableName, string SrColumnName, string SectionColumnName, string NewTargetSno, string OldSourceSno, string OldSection, string NewSection, int ModuleInstanceId, int OldCategoryId, int NewCategoryId, string CategoryColumnName)
            {
                if (Convert.ToInt32(NewTargetSno) > 0 && Convert.ToInt32(OldSourceSno) > 0)
                {
                    string strGetData = "update " + TableName + " set " + SrColumnName + "=" + SrColumnName + " + 1 where " + SrColumnName + " > " + OldSourceSno + " AND " + SectionColumnName + " = '" + OldSection.ToString().ToString() + "' And " + SrColumnName + ">0 AND fkModuleInstanceId = " + ModuleInstanceId + " AND " + CategoryColumnName + " = " + OldCategoryId + "";
                    if (dbAccess.moSqlConnection.State == ConnectionState.Open)
                        dbAccess.moSqlConnection.Close();
                    dbAccess.moSqlConnection.ConnectionString = ConStr;
                    dbAccess.execute(strGetData, DBAccess.SQLType.IS_QUERY);
                    dbAccess.executeMyQuery();
                    dbAccess.releasedCommand();
                }
            }

            public int GetMaxSrNo(string ConnectionString, int ModuleInstanceId, string ModuleInsatnceIdColumnName, string TableName, string SrNoColumnName, string SectionColumnName, string section, string CatColumnName, int CatID)
            {
                int x;
                try
                {
                    SqlConnection Conn = new SqlConnection(ConnectionString);
                    string query = "select max(" + SrNoColumnName + ") from " + TableName + " where " + SectionColumnName + "='" + section + "' AND " + CatColumnName + " = " + CatID + "  AND " + ModuleInsatnceIdColumnName + " = " + ModuleInstanceId + " and " + SrNoColumnName + " > 0'";

                    SqlDataAdapter DA = new SqlDataAdapter(query, Conn);

                    DataSet DS = new DataSet();

                    DA.Fill(DS);

                    x = ((int)(DS.Tables[0].Rows[0][0])) + 1;
                    if (x < 1)
                    {
                        x = 1;
                    }
                }
                catch
                {
                    x = 1;
                }

                return x;
            }
        }
    }

  

}