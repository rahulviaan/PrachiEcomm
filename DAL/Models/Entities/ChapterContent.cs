using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class ChapterContent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public long BookId { get; set; }
        public long ChapterId { get; set; }
        public long Type { get; set; }
        public string Name { get; set; }
        public int ContentType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long? OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Downloaded { get; set; }
    }
}
