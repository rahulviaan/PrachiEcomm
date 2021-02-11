using PrachiIndia.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PrachiIndia.Portal.Helpers
{
    public static class ErrorLogger
    {
        public static void CreateErrorLog(string message, string eventname)
        {
            try
            {
                var log = new ErrorLog
                {
                    CreatedDate = DateTime.Now,
                    ErrorMessage = message,
                    EventName = eventname,
                    Id = Guid.NewGuid().ToString()
                };
                var context = new dbPrachiIndia_PortalEntities();
                context.ErrorLogs.Add(log);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var logFile = "ErrorLog_" + DateTime.Now.ToString("MM-dd-yyyy") + ".txt";
                var sb = new StringBuilder();
                sb.Append(ex.Message.ToString());
                File.AppendAllText(logFile, sb.ToString());
                sb.Clear();
            }
        }


    }
}