using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class BookModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public long BoardId { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long SeriesId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public string Edition { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsEbook { get; set; }
        public bool IsMultiMedia { get; set; }
        public bool IsSolutions { get; set; }
        public bool IsWorkbook { get; set; }
        public bool IsVideo { get; set; }
        public bool IsLessonPlan { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string EpubPath { get; set; }
        public string EpubName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Status { get; set; }
        public int OrderNo { get; set; }
        public string Size { get; set; }
        public bool ImageDownloaded { get; set; }
        public bool EpubDownloaded { get; set; }
        public bool CBSE { get; set; }
        public bool MTP { get; set; }
        public bool BEP { get; set; }
        public bool TPG { get; set; }
        public bool ConceptMap { get; set; }
        public int AllowedCBPages { get; set; }
    }
}
