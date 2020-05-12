using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class ChapterContentModel
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public long ChapterId { get; set; }
        public long Type { get; set; }
        public string Name { get; set; }
        public int ContentType { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Status { get; set; }
        public long OrderId { get; set; }
    }
}
