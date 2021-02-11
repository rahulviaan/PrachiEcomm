using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Net;
using System.Configuration;
using System.Net.Mail;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using PrachiIndia.Sql;
//using PrachiIndia.Sql.CustomRepositories;

namespace PrachiIndia.Portal.Framework
{

    public static class PrachiService
    {
        private static string[] ones = {
    "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
    "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
};

        private static string[] tens = { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        private static string[] thous = { "hundred", "thousand", "million", "billion", "trillion", "quadrillion" };

        public static string ToWords(decimal number)
        {
            if (number < 0)
                return "negative " + ToWords(Math.Abs(number));

            int intPortion = (int)number;
            int decPortion = (int)((number - intPortion) * (decimal)100);

            return string.Format("{0} only", ToWords(intPortion), ToWords(decPortion));
        }

        public static string ToWords(int number, string appendScale = "")
        {
            string numString = "";
            if (number < 100)
            {
                if (number < 20)
                    numString = ones[number];
                else
                {
                    numString = tens[number / 10];
                    if ((number % 10) > 0)
                        numString += "-" + ones[number % 10];
                }
            }
            else
            {
                int pow = 0;
                string powStr = "";

                if (number < 1000) // number is between 100 and 1000
                {
                    pow = 100;
                    powStr = thous[0];
                }
                else // find the scale of the number
                {
                    int log = (int)Math.Log(number, 1000);
                    pow = (int)Math.Pow(1000, log);
                    powStr = thous[log];
                }

                numString = string.Format("{0} {1}", ToWords(number / pow, powStr), ToWords(number % pow)).Trim();
            }

            return string.Format("{0} {1}", numString, appendScale).Trim();
        }

        public static InvoiceData GenerateInvoice(long orderId, string userId)
        {
            var invoiceCopy = new InvoiceData();
            var context = new dbPrachiIndia_PortalEntities();
            var user = context.AspNetUsers.First(t => t.Id == userId);
            var order = context.Orders.FirstOrDefault(t => t.Id == orderId);
            if (order == null || order.Id == 0)
                return invoiceCopy;


            var message = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/HtmlTemplate/Invoice.html"));
            var state = context.States.FirstOrDefault(t => t.StaeteId == user.StateId || t.StateName.ToLower().Contains(user.State.ToLower()));
            var products = context.OrderProducts.Where(t => t.OrderId == order.Id).ToList();
            var address = string.Format("{0}, {1}, {2}, {3}-{4}", user.Address, user.City, user.State, user.Country, user.PinCode);
            message = message.Replace("{{Name}}", order.Name).Replace("{{Email}}", order.Email).Replace("{{Mobile}}", order.Phone);
            message = message.Replace("{{Address}}", address).Replace("{{StateName}}", user.State).Replace("{{StateCode}}", state == null ? "N/A" : state.StateCode);
            message = message.Replace("{{InvoiceNo}}", order.TransactionId).Replace("{{InvoiceDate}}", order.UpdatedDate.Value.ToString("dd-MMM-yyyy"));
            var productDesc = "<table style='width: 100%; font-weight: normal; font-size: 11px; line-height:17px; text-align: left;' cellspacing='0' cellpadding='0'><thead><tr>";
            //Table Heading

            var isDelhiState = state != null && state.StateCode == "07" ? true : false;
            if (isDelhiState)
            {
                //State Delhi Include CGST/SGST
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='35%'>Product</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101; border-right:none;' width='20%'>HSN/SAC Code</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Type</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Qty</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Unit</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Discount</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Taxable value</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>CGST</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>SGST</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;' width='5%'>Total</th>";
            }
            else
            {
                //Other State Include IGST
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='35%'>Product</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101; border-right:none;' width='20%'>HSN/SAC Code</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Type</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Qty</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Unit</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Discount</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Taxable value</th>";
                productDesc = productDesc + "<th colspan='2' style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>IGST</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;' width='5%'>Total</th>";
            }
            productDesc = productDesc + "</tr></thead> <tbody>";

            //table boady
            var totalPrice = 0M;
            var gross = 0M;
            var totalUnitPrice = 0M;
            var totalDiscount = 0M;
            var totalSubtotal = 0M;
            var cgstTotal = 0M;
            var sgstTotal = 0M;
            var igstTotal = 0M;
            foreach (var product in products)
            {
                var item = context.tblCataLogs.First(t => t.Id == product.ItemId);
                var classMaster = context.MasterClasses.First(t => t.Id.ToString() == item.ClassId);
                product.Title = string.Format("{0} <br /><strong>Class:</strong> {1}", product.Title, classMaster.Title);
                var taxMaster = context.GSTTaxLists.FirstOrDefault(t => t.Id == item.Tax);
                var hsnCode = "N/A";
                if (taxMaster != null)
                {
                    hsnCode = taxMaster.Description;// string.Format("Book | HSN : {0} | TAX : {1}", "49011010", "0%");
                }
                var unitPrice = product.Price ?? 0;
                var qty = product.Quantity ?? 1;
                var discount = product.Discount ?? 0;
                var subtotal = (unitPrice * qty) - discount;

                var cgst = taxMaster.Rate > 0 ? (subtotal * (taxMaster.Rate) / 2) / 100 : 0M;
                var sgst = taxMaster.Rate > 0 ? (subtotal * (taxMaster.Rate) / 2) / 100 : 0M;
                var igst = (subtotal * (taxMaster.Rate)) / 100;




                totalUnitPrice = totalUnitPrice + unitPrice;
                totalDiscount = totalDiscount + discount;
                totalSubtotal = totalSubtotal + subtotal;
                if (isDelhiState)
                {


                    gross = subtotal + cgst + sgst;
                    cgstTotal = cgstTotal + cgst;
                    sgstTotal = sgstTotal + sgst;
                }
                else
                {
                    gross = subtotal + igst;
                    igstTotal = igstTotal + igst;
                }
                totalPrice = totalPrice + gross;
                var bookType = string.Empty;
                if (product.BookType == BookType.Both)
                    bookType = "Print Book & E-Book";
                else if (product.BookType == BookType.EBook)
                    bookType = "E-Book";
                else
                    bookType = "Print Book";
                if (isDelhiState)
                {
                    productDesc = productDesc + "<tr><td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + product.Title + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + hsnCode + "</td>";
                    productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;' width='5%'>" + bookType + "</th>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + qty + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", unitPrice) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", discount) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", subtotal) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", cgst) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", sgst) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><p style='margin:0;'>Rs. " + String.Format("{0:0.00}", gross) + "</p></td></tr>";
                }
                else
                {
                    productDesc = productDesc + "<tr><td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + product.Title + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + hsnCode + "</td>";
                    productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;' width='5%'>" + bookType + "</th>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + qty + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", unitPrice) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", discount) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", subtotal) + "</td>";
                    productDesc = productDesc + "<td colspan='2' style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", igst) + "</td>";
                    productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><p style='margin:0;'>Rs. " + String.Format("{0:0.00}", gross) + "</p></td></tr>";
                }
            }

            productDesc = productDesc + "</tbody><tfoot>";
            productDesc = productDesc + " <tr><td colspan='4' style='font-size:12px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Total:</strong></td>";
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'></td>";
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", totalDiscount) + "</td>";
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", totalSubtotal) + "</strong></td>";

            var totalInwords = ToWords(totalPrice).Replace("zero", "");

            if (isDelhiState)
            {
                productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", cgstTotal) + "</td>";
                productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", sgstTotal) + "</td>";
            }
            else
            {
                productDesc = productDesc + "<td colspan='2' style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", igstTotal) + "</td>";
            }
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", totalPrice) + "</strong></td></tr>";

            if (isDelhiState)
            {
                productDesc = productDesc + "<tr><td colspan='5' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Total invoice value (In Figure)</strong><br /><strong>Total invoice value (In Words)</strong><strong>Amount of Tax subject to Reverse Charges</strong></td>";
                productDesc = productDesc + "<td colspan='6' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", totalPrice) + "</strong><br /><strong><span style='text-transform: capitalize;'>" + totalInwords + "</sapn></strong></td></tr>";
            }
            else
            {
                productDesc = productDesc + "<tr><td colspan='5' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><strong>Total invoice value (In Figure)</strong><br /><strong>Total invoice value (In Words)</strong><br /><strong>Amount of Tax subject to Reverse Charges in Rs. </strong></td>";
                productDesc = productDesc + "<td colspan='5' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", totalPrice) + "</strong><br /><strong><span style='text-transform: capitalize;'>" + totalInwords + "</sapn></strong></td></tr>";
            }
            //fotterbody

            productDesc = productDesc + "</tfoot></table>";

            message = message.Replace("{{ProductDetails}}", productDesc);

            invoiceCopy.MailTemplate = message;
            invoiceCopy.Email = order.Email;
            invoiceCopy.Subject = string.Format("Invoice-{0}", order.TransactionId);
            return invoiceCopy;
        }

        public static InvoiceData GenerateInvoiceForSubscription(string transactionID, string userId)
        {
            var invoiceCopy = new InvoiceData();
            var context = new dbPrachiIndia_PortalEntities();
            var user = context.AspNetUsers.First(t => t.Id == userId);
            var orderPayment = context.SubscriptionPayments.FirstOrDefault(t => t.TransactionId  == transactionID);

            var message = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/HtmlTemplate/SubscriptionInvoiceHtml.html"));
            var state = context.States.FirstOrDefault(t => t.StaeteId == user.StateId || t.StateName.ToLower().Contains(user.State.ToLower()));
            var products = context.SubscriptionMasters.FirstOrDefault(t => t.Id == orderPayment.SubscriptionId);
            var address = string.Format("{0}, {1}, {2}, {3}-{4}", user.Address, user.City, user.State, user.Country, user.PinCode);
            message = message.Replace("{{Name}}", orderPayment.Name).Replace("{{Email}}", orderPayment.Email).Replace("{{Mobile}}", orderPayment.Phone);
            message = message.Replace("{{Address}}", address).Replace("{{StateName}}", user.State).Replace("{{StateCode}}", state == null ? "N/A" : state.StateCode);
            message = message.Replace("{{InvoiceNo}}", orderPayment.TransactionId).Replace("{{InvoiceDate}}", orderPayment.UpdatedDate.Value.ToString("dd-MMM-yyyy"));
            var productDesc = "<table style='width: 100%; font-weight: normal; font-size: 11px; line-height:17px; text-align: left;' cellspacing='0' cellpadding='0'><thead><tr>";
            //Table Heading

            var isDelhiState = state != null && state.StateCode == "07" ? true : false;
            if (isDelhiState)
            {
                //State Delhi Include CGST/SGST
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='35%'>Subscription Type</th>";
                
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Taxable value</th>";
               
            }
            else
            {
                //Other State Include IGST
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='35%'>Subscription Type</th>";
                //productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101; border-right:none;' width='20%'>HSN/SAC Code</th>";
                //productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Type</th>";
                //productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Qty</th>";
                //productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Unit</th>";
                //productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='5%'>Discount</th>";
                productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>Taxable value</th>";
                //productDesc = productDesc + "<th colspan='2' style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;' width='10%'>IGST</th>";
                //productDesc = productDesc + "<th style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;' width='5%'>Total</th>";
            }
            productDesc = productDesc + "</tr></thead> <tbody>";


                productDesc = productDesc + "<tr><td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>" + products.SubscriptionType + "</td>";

                productDesc = productDesc + "<td style='vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><p style='margin:0;'>Rs. " + String.Format("{0:0.00}", orderPayment.Amount) + "</p></td></tr>";
   

            productDesc = productDesc + "</tbody><tfoot>";
            productDesc = productDesc + " <tr><td colspan='4' style='font-size:12px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Total:</strong></td>";
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'></td>";
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'>Rs. " + String.Format("{0:0.00}", orderPayment.Amount) + "</td>";
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", orderPayment.Amount) + "</strong></td>";

            var totalInwords = ToWords(orderPayment.Amount??0).Replace("zero", "");

          
            productDesc = productDesc + "<td style='font-size:12px;vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", orderPayment.Amount) + "</strong></td></tr>";

            if (isDelhiState)
            {
                productDesc = productDesc + "<tr><td colspan='5' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Total invoice value (In Figure)</strong><br /><strong>Total invoice value (In Words)</strong><strong>Amount of Tax subject to Reverse Charges</strong></td>";
                productDesc = productDesc + "<td colspan='6' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-right:none;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", orderPayment.Amount) + "</strong><br /><strong><span style='text-transform: capitalize;'>" + totalInwords + "</sapn></strong></td></tr>";
            }
            else
            {
                productDesc = productDesc + "<tr><td colspan='5' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><strong>Total invoice value (In Figure)</strong><br /><strong>Total invoice value (In Words)</strong><br /><strong>Amount of Tax subject to Reverse Charges in Rs. </strong></td>";
                productDesc = productDesc + "<td colspan='5' style='font-size:11px; vertical-align:top;padding:2px 5px;border:solid 1px #010101;border-top:none;'><strong>Rs. " + String.Format("{0:0.00}", orderPayment.Amount) + "</strong><br /><strong><span style='text-transform: capitalize;'>" + totalInwords + "</sapn></strong></td></tr>";
            }
            //fotterbody

            productDesc = productDesc + "</tfoot></table>";

            message = message.Replace("{{ProductDetails}}", productDesc);

            invoiceCopy.MailTemplate = message;
            invoiceCopy.Email = orderPayment.Email;
            invoiceCopy.Subject = string.Format("Invoice-{0}", orderPayment.TransactionId);
            return invoiceCopy;
        }
    }
    public static class MessageSent
    {
        public static int SendSMS(string mobileNumber, string message)
        {

            var Url = "http://www.kit19.com/ComposeSMS.aspx?username=prachi88225&password=73780&sender=PIPLIN&to=" + mobileNumber + "&message=" + message + "&priority=1&dnd=1&unicode=0";
            try
            {
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(Url);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = 0;
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();
                //Close the response
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {
                // MessageBox.Show(ex.Message.ToString());
            }

            return 0;
        }
        public static string GeneratetOTP(int length)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B", "C", "D", "E" };
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

    }

    public class InvoiceData
    {
        public string MailTemplate { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}
