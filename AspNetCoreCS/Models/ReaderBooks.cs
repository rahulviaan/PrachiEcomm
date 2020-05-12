using ReadEdgeCore.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class ReaderBooks : IReaderBooks
    {

        public string IdServer { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string BoardId { get; set; }
        public string ClassId { get; set; }
        public long? SubjectId { get; set; }
        public long? SeriesId { get; set; }
        public string CoverPage { get; set; }
        public string LastPage { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
        public string BookPath { get; set; }
        public string EpubPath { get; set; }
        public string NewImage { get; set; }
        public string SelectImage { get; set; }
        public DateTime dtmCreate { get; set; }
        public DateTime dtmUpdate { get; set; }
        //  public IsValid Status { get; set; }
        public string IndexTitle { get; set; }

        public string dccreator { get; set; }
        public string dcpublisher { get; set; }
        public string dcidentifier { get; set; }
        public string isSize { get; set; }
        public string dclanguage { get; set; }
        public string dcdate { get; set; }
        public bool Downloaded { get; set; }
        public int DownloadPercentage { get; set; }
        public dynamic Visible { get; set; }
        public string DownloadTooltip { get; set; }
        public string BookId { get; set; }
        public string EPubPath { get; set; }
        public string CoverImage { get; set; }
        public string EncriptionKey { get; set; }

        public dynamic VisiblePurchase { get; set; }
        public dynamic VisibleInProcee { get; set; }
        public dynamic VisibleDownload { get; set; }
        public dynamic VisibleDiscount { get; set; }
        public dynamic VisibleRead { get; set; }
        public string ToolTip { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ShowPrice { get; set; }

        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public decimal? Discount { get; set; }

        public int? Ebook { get; set; }
        public int? MultiMedia { get; set; }
        public int? Solutions { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime dtmAdd { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public int IdOrder { get; set; }
        public string OrderId { get; set; }
        // public BookStatus BookStatus { get; set; }
        public IList<ReaderBookPage> Pages { get; set; }
        public IList<ReaderBookIndex> Indexes { get; set; }
    }


    public class ReaderBookPage
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Title { get; set; }
        public bool BookMarked { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
    }
    public class ReaderBookIndex
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
        public IList<ReaderBookIndex> Indexes { get; set; }

    }
    public class objRecent
    {
        public string book { get; set; }
        public string date { get; set; }
        public int page { get; set; }
        //public SortLibary sortlibary { get; set; }
        //public LibraryView libraryview { get; set; }
    }

    public class objBookMark
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string dtmCreate { get; set; }


    }
    public class objNote
    {
        public int currentindex { get; set; }
        public string BookStaticsPath { get; set; }
        public string key { get; set; }
        public string Title { get; set; }
        public string selectedtext { get; set; }
        public string notetext { get; set; }
        public string dtmUpdate { get; set; }


    }
    public class objSearch
    {

        public int Index { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
    }
    public class UserReaderBookItem
    {
        public string dccreator { get; set; }
        public string dcpublisher { get; set; }
        public string dcidentifier { get; set; }
        public string dclanguage { get; set; }
        public string dcdate { get; set; }
        public DateTime dtmCreate { get; set; }
        public string ClassId { get; set; }
        public long? SubjectId { get; set; }
        public long? SeriesId { get; set; }
        public bool Downloaded { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public string EPubPath { get; set; }
        public string EncriptionKey { get; set; }
        public string BookPath { get; set; }
        public string ReaderId { get; set; }
        public string Title { get; set; }
        public string CoverImage { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Creator { get; set; }
        public int DownloadCount { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string PubIdentifier { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Status { get; set; }
        public dynamic Visible { get; set; }
        public string DownloadTooltip { get; set; }
        public int DownloadPercentage { get; set; }
        public string EpubPath { get; set; }
        public string CoverPage { get; set; }
        public string NewImage { get; set; }
        public string SelectImage { get; set; }
        public string Path { get; set; }
        public string DirPath { get; set; }
        public IList<ReaderBookIndex> Indexes { get; set; }
        public string IndexTitle { get; set; }
        public string LastPage { get; set; }
        public IList<ReaderBookPage> Pages { get; set; }

    }
    public class KeyValue
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}

