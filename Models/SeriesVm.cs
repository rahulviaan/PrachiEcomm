using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.Models
{
    public class SeriesVm
    {
        public long Id { get; set; }
        public Nullable<long> SubjectId { get; set; }
        public string Title { get; set; }
        public Nullable<int> OredrNo { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> dtmAdd { get; set; }
        public Nullable<System.DateTime> dtmUpdate { get; set; }
        public Nullable<System.DateTime> dtmDelete { get; set; }
        public Nullable<int> Status { get; set; }
        public string SubjectName { get; set; }
    }

    public class SeriesVModel
    {
        public long Id { get; set; }
        public long SubjectId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string SubjectName { get; set; }
        public List<PrachiIndia.Sql.tblCataLog> Items { get; set; }
    }
}