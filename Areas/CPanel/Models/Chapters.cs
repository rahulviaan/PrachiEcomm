using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrachiIndia.Web.Areas.Model
{


    public class BookChapters
    {
        internal string idClass;
        public int id { get; set; }
        public int idBook { get; set; }
        public int idSubject { get; set; }
        public int idSeries { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }
        public string isTitle { get; set; }
        public string isDesc { get; set; }
        public IsValid isStatus { get; set; }
        public TextType isType { get; set; }
        public int isFromPage { get; set; }
        public int isToPage { get; set; }
        public int isChapter { get; set; }

    }
    public class BookPlus
    {
        public long id { get; set; }
        public int idBook { get; set; }
        public int idSubject { get; set; }
        public int idSeries { get; set; }
        public int idChapter { get; set; }
        public ChapterType idType { get; set; }
        public string isName { get; set; }
        public contentType isContentType { get; set; }
        public string Chapter { get; set; }
        public string isTitle { get; set; }
        public string isDesc { get; set; }
        public IsValid isStatus { get; set; }
        public DateTime? dtmCreate { get; set; }
        public DateTime? dtmUpdate { get; set; }



    }
    public class UserAppReader
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ReaderKey { get; set; }
        public string DescripTion { get; set; }
        public string MachineKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DeviceType DeviceType { get; set; }
        public int ReaderStatus { get; set; }
        public int Status { get; set; }
        public string ErrorMessage { get; set; }
        public int MasterVesrion { get; set; }
        public int BookVersion { get; set; }
        public int LibraryVersion { get; set; }
        public DateTime SynchDate { get; set; }
    }

    public class eBookOrder
    {
        public string BookIdServer { get; set; }
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string ReaderId { get; set; }
        public string UserId { get; set; }
        public string ProductInfo { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public int BookType { get; set; }
        public Portal.Framework.PayUMoney.PaymentStatus Status { get; set; }
        public string TransactionId { get; set; }
        public string RequestLog { get; set; }
        public string RequestHash { get; set; }
        public string PGType { get; set; }
        public string PayUMoneyId { get; set; }
        public string AddtionalCharge { get; set; }
        public string ResponseLog { get; set; }
        public string ResponseHas { get; set; }
        public string DownloadPath { get; set; }
        public string EncryptionKey { get; set; }
        public string IdServer { get; set; }


    }
}