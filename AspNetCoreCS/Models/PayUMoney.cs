using DAL.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GleamTech.DocumentUltimateExamples.AspNetCoreCS.Models
{
    public class PayUMoney
    {
        private ISubscriptionOrderPaymentRepository _ISubscriptionOrderPaymentRepository;
     
      
        public PayUMoney(ISubscriptionOrderPaymentRepository subscriptionOrderPaymentRepository)
        {
            _ISubscriptionOrderPaymentRepository = subscriptionOrderPaymentRepository;
         
        }
        public enum PaymentStatus
        {

            Initiated = 0,
            Success = 1,
            Failure = 2
        }

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
        }


     
  

    }
    public class RemotePost
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RemotePost(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
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
            _httpContextAccessor.HttpContext.Response.Clear();
            _httpContextAccessor.HttpContext.Response.WriteAsync("<html><head>");
            sb.Append("<html><head>");
            _httpContextAccessor.HttpContext.Response.WriteAsync(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            sb.Append(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            _httpContextAccessor.HttpContext.Response.WriteAsync(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            sb.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            for (int i = 0; i < _inputs.Keys.Count; i++)
            {
                _httpContextAccessor.HttpContext.Response.WriteAsync(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", _inputs.Keys[i], _inputs[_inputs.Keys[i]]));
                sb.Append(string.Format(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", _inputs.Keys[i], _inputs[_inputs.Keys[i]])));
            }
            _httpContextAccessor.HttpContext.Response.WriteAsync("</form>");
            sb.Append("</form></body></html>");

            _httpContextAccessor.HttpContext.Response.WriteAsync("</body></html>");
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status200OK;

        }
    }
}
