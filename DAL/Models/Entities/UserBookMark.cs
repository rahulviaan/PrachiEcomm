using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static DAL.Models.Enum;

namespace DAL.Models.Entities
{
    public partial class UserBookMark
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public ContentTypes ContentType { get; set; }
        public int ContentId { get; set; }
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsUserContent { get; set; }
    }
}
