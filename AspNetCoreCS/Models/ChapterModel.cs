using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class ChapterModel
    {
        public string Id { get; set; }
        public long BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public int ChapterIndex { get; set; }
        public List<ChapterContentModel> ChapterContents { get; set; }
    }
}
