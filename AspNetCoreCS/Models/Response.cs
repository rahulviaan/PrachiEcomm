using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadEdgeCore.Models
{
    public class Response
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string ProductInfo { get; set; }
        public string AWBNO { get; set; }
        public string DispatchedBy { get; set; }
        public List<ProductItem> Products { get; set; }
        public string OrderId { get; set; }
        //public Sql.AspNetUser AspNetUser { get; set; }
        public string dtmCreate { get; set; }
    }
    public class ProductItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public long ItemId { get; set; }
        public string Flag { get; set; }
        public string Image { get; set; }
        public string EbookPrice { get; set; }
        public string BookType { get; set; }
        public string Classname { get; set; }
        public string BoardName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SolutionPrice { get; set; }
        public bool IsSolution { get; set;}
        public decimal TaxRate { get; set; }

    }


    public class PostBackParam
    {
        public int postBackParamId { get; set; }
        public string mihpayid { get; set; }
        public int paymentId { get; set; }
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string additionalCharges { get; set; }
        public string addedon { get; set; }
        public long createdOn { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string hash { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string cardToken { get; set; }
        public string offer_key { get; set; }
        public string offer_type { get; set; }
        public string offer_availed { get; set; }
        public string pg_ref_no { get; set; }
        public string offer_failure_reason { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string cardhash { get; set; }
        public string card_type { get; set; }
        public string card_merchant_param { get; set; }
        public string version { get; set; }
        public string postUrl { get; set; }
        public bool calledStatus { get; set; }
        public string additional_param { get; set; }
        public string amount_split { get; set; }
        public string discount { get; set; }
        public string net_amount_debit { get; set; }
        public string fetchAPI { get; set; }
        public string paisa_mecode { get; set; }
        public string meCode { get; set; }
        public string payuMoneyId { get; set; }
        public string encryptedPaymentId { get; set; }
        public string id { get; set; }
        public string surl { get; set; }
        public string furl { get; set; }
        public string baseUrl { get; set; }
        public int retryCount { get; set; }
        public string pg_TYPE { get; set; }
    }
    public class Result
    {
        public string merchantTransactionId { get; set; }
        public PostBackParam postBackParam { get; set; }
    }
    public class PaymentModel
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<Result> result { get; set; }
        public string errorCode { get; set; }
        public string responseCode { get; set; }
    }
}