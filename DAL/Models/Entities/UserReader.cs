using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public partial class UserReader
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ReaderKey { get; set; }
        public string DescripTion { get; set; }
        public string MachineKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int DeviceType { get; set; }
        public int ReaderStatus { get; set; }
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public int MasterVesrion { get; set; }
        public int BookVersion { get; set; }
        public int LibraryVersion { get; set; }
        public DateTime SynchDate { get; set; }
    }
}
