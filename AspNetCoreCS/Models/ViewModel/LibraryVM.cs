using DAL.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static DAL.Models.Enum;
using static ReadEdgeCore.Utilities.Enums;

namespace ReadEdgeCore.Models.ViewModel
{
    public class LibraryVM
    {

        public IFormFile ConfigurationFile { get; set; }
        public IFormFile BundleFile { get; set; }
        public IFormFile NoteFile { get; set; }
        public string Description { get; set; }
        public int PageNumber { get; set; }        
        public bool IsUserContent { get; set; }        
        public int ContentId { get; set; }
        public string UserId { get; set; }        
        public string ClassId { get; set; } 
        public Int64 BookId { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public string BookName { get; set; }
        public string Chapter { get; set; }
        public string ChapterContent { get; set; }
        public bool BundlePackage { get; set; }
        public bool IsInLibrary { get; set; }
        public bool IsBundleUploaded { get; set; }
        [Required(ErrorMessage = "Note is Required")]
        public int startindex { get; set; } = 1;
        public int endindex { get; set; } = 1;
        
        public BpType type { get; set; }
        public ContentTypes ContentType { get; set; }
        public int AllowedCBPages { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<ChapterContent> ChapterContents { get; set; }

    }
}
