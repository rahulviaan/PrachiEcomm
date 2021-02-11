using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public partial class Category
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long ParentId { get; set; }
        public short Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public short? DisplayOrder { get; set; }
        public string IPInsertd { get; set; }
        public string IPUpdated { get; set; }
        public long? UserInserted { get; set; }
        public long? UserUpdated { get; set; }


    }

}