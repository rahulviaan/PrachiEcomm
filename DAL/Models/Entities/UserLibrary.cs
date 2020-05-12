using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class UserLibrary
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public long BookId { get; set; }
        public string EpubPath { get; set; }
        public string EpubName { get; set; }
        public string EncriptionKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool EpubDownloaded { get; set; }
        public bool BundleUploaded { get; set; }
    }
}
