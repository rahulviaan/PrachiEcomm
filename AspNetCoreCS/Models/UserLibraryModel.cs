using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class UserLibraryModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public long BookId { get; set; }
        public string EPubPath { get; set; }
        public string EpubName { get; set; }
        public string EncriptionKey { get; set; }
        public List<ChapterModel> Chapters { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Result { get; set; }
        public string ErrorMessage { get; set; }
    }
}
