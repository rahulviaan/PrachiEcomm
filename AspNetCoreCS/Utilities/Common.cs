using ReadEdgeCore.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadEdgeCore.Utilities
{
    public static class Common
    {
        static string RootFolder = "ReadEdgeCore";
        static string GroupFolder = "ReadEdage_2.0.0.0";
        public static double percentage = 0;
        public static double Bundlepercentage = 0;
        public static IReaderBooks readerBooks { get; set; }
        public static string AppTempPath()
        {
            var path = AppPath() + "Temp\\";
            return path;
        }
        public static string AppPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + RootFolder + "\\" + GroupFolder + "\\";
            return path;
        }
        public static string TempFilePath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "TempFilePath\\";
            return path;
        }
        public static T ConvertFromXML<T>(string xmlText)
        {
            if (String.IsNullOrWhiteSpace(xmlText)) return default(T);

            using (StringReader stringReader = new StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
        public static int ToSafeInt(object obj)
        {
            if (obj == null || obj == "")
                return 0;
            else
                return ISNULL(Convert.ToString(obj));
        }

        public static string ToSafeString(object obj)
        {
            return (obj ?? string.Empty).ToString().Trim();
        }

        public static double ToSafeDouble(object obj)
        {
            if (obj == null || obj == "")
                return 0;
            else
                return Convert.ToDouble(obj);
        }
        public static int ISNULL(string str)
        {
            if (str == "")
                return 0;
            else
                return Convert.ToInt32(str);
        }


        public static DateTime ToSafeDate(object obj)
        {
            if (obj == null || obj == "")
                return Convert.ToDateTime("1/1/1900");
            else
                return Convert.ToDateTime(obj);
        }
        public static double GetProgressPercenttage(double currnetFile,int NoOfFiles,int TotalProcess) {

            double percentage = ((currnetFile * 100 / NoOfFiles));
            return percentage;
        }
    }
}
