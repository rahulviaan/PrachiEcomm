using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using PrachiIndia.Sql;
using System.Linq;
using System.Configuration;

namespace PrachiIndia.Portal.Framework
{

    public class PayUMoney
    {
        public enum PaymentStatus
        {

            Initiated = 0,
            Success = 1,
            Failure = 2
        }
        static string OrderIdPrefix = ConfigurationManager.AppSettings["OrderIdPrefix"]; 
        static string SubOrderIdPrefix = ConfigurationManager.AppSettings["SubOrderIdPrefix"]; 
        public static string Generatehash512(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }


            return hex;
            //var message = Encoding.UTF8.GetBytes(text);
            //var ue = new UnicodeEncoding();
            //var hashString = new SHA512Managed();
            //var hashValue = hashString.ComputeHash(message);
            //return hashValue.Aggregate("", (current, x) => current + String.Format("{0:x2}", x));
        }
        public static string GenerateTxnId()
        {

            //Random rnd = new Random();
            //string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            //string txnid1 = strHash.ToString().Substring(0, 20);
            //return txnid1;

            var context = new dbPrachiIndia_PortalEntities();
            lock (context.Orders)
            {
                string orderid = Convert.ToString(context.Orders.Max(x => x.Id) + 1);
                string TransactionID = OrderIdPrefix + DateTime.Now.Year.ToString().Substring(2,2) + "-" + orderid;
                return TransactionID;
            }
        }

        public static string GenerateSubTxnId()
        {

            //Random rnd = new Random();
            //string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            //string txnid1 = strHash.ToString().Substring(0, 20);
            //return txnid1;

            var context = new dbPrachiIndia_PortalEntities();
            lock (context.Orders)
            {
                string orderid = Convert.ToString(context.SubscriptionPayments.Max(x => x.Id) + 1);
                string TransactionID = SubOrderIdPrefix + DateTime.Now.Year.ToString().Substring(2, 2) + "-" + orderid;
                return TransactionID;
            }
        }
    }
    public class RemotePost
    {
        private readonly System.Collections.Specialized.NameValueCollection _inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            _inputs.Add(name, value);
        }
        public void Post()
        {
            StringBuilder sb = new StringBuilder(500);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("<html><head>");
            sb.Append("<html><head>");
            HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            sb.Append(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            sb.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            for (int i = 0; i < _inputs.Keys.Count; i++)
            {
                HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", _inputs.Keys[i], _inputs[_inputs.Keys[i]]));
                sb.Append(string.Format(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", _inputs.Keys[i], _inputs[_inputs.Keys[i]])));
            }
            HttpContext.Current.Response.Write("</form>");
            sb.Append("</form></body></html>");

            HttpContext.Current.Response.Write("</body></html>");
            HttpContext.Current.Response.End();
        }
    }
}