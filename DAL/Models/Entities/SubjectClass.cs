using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class SubjectClass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
    }
}
