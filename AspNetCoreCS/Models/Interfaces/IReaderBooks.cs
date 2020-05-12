using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface IReaderBooks
    {
        string Author { get; set; }
        string BoardId { get; set; }
        string BookId { get; set; }
        string BookPath { get; set; }
        string ClassId { get; set; }
        string CoverImage { get; set; }
        string CoverPage { get; set; }
        string dccreator { get; set; }
        string dcdate { get; set; }
        string dcidentifier { get; set; }
        string dclanguage { get; set; }
        string dcpublisher { get; set; }
        string Description { get; set; }
        string DirPath { get; set; }
        decimal? Discount { get; set; }
        decimal DiscountedPrice { get; set; }
        bool Downloaded { get; set; }
        int DownloadPercentage { get; set; }
        string DownloadTooltip { get; set; }
        DateTime dtmAdd { get; set; }
        DateTime dtmCreate { get; set; }
        DateTime dtmUpdate { get; set; }
        int? Ebook { get; set; }
        string Edition { get; set; }
        string EncriptionKey { get; set; }
        string EpubPath { get; set; }
        string EPubPath { get; set; }
        string Id { get; set; }
        int IdOrder { get; set; }
        string IdServer { get; set; }
        string Image { get; set; }
        IList<ReaderBookIndex> Indexes { get; set; }
        string IndexTitle { get; set; }
        string IpAddress { get; set; }
        string ISBN { get; set; }
        string isSize { get; set; }
        string LastPage { get; set; }
        int? MultiMedia { get; set; }
        string NewImage { get; set; }
        string OrderId { get; set; }
        IList<ReaderBookPage> Pages { get; set; }
        string Path { get; set; }
        decimal Price { get; set; }
        string SelectImage { get; set; }
        long? SeriesId { get; set; }
        string ShowPrice { get; set; }
        int? Solutions { get; set; }
        long? SubjectId { get; set; }
        string Title { get; set; }
        string ToolTip { get; set; }
        string UserId { get; set; }
        dynamic Visible { get; set; }
        dynamic VisibleDiscount { get; set; }
        dynamic VisibleDownload { get; set; }
        dynamic VisibleInProcee { get; set; }
        dynamic VisiblePurchase { get; set; }
        dynamic VisibleRead { get; set; }
    }
}
