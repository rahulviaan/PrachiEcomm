using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace DAL.Models.Entities
{
    public class SessionMgt
    {
         [DatabaseGenerated(DatabaseGeneratedOption.None)]
         
         public int Id { get; set; }
         public string Name { get; set; }
         public string Val { get; set; }
    }
}
