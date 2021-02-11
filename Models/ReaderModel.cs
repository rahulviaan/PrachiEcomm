using System;

namespace PrachiIndia.Portal.Models
{
    public enum ResponseStatus
    {
        Succeeded = 0,
        Failed = 1,
        Unknown = 2
    }
    public enum DeviceType
    {

        Window = 0,
        Android = 1,
        Apple = 2,
        Linux = 3,
        Other = 4,

    }
    public enum ReaderStatus
    {

        Old = 0,
        New = 1,
        Error = 2,         

    }
    
    public class BaseClass
    {
        public DateTime TimeStamp { get; set; }
        public string Result { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class UserReader : BaseClass
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ReaderKey { get; set; }
        public string Description { get; set; }
        public string MachineKey { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DeviceType DeviceType { get; set; }
        public ReaderStatus ReaderStatus { get; set; }

        public int MasterVesrion { get; set; }
        public int BookVersion { get; set; }
        public int LibraryVersion { get; set; }
        public DateTime SynchDate { get; set; }
    }

    public class UserReaderBook : BaseClass
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ReaderId { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string Creator { get; set; }
        public int? DownloadCount { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string PubIdentifier { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Status { get; set; }
        public string EPubPath { get; set; }
        public string EncriptionKey { get; set; }
    }

   
}