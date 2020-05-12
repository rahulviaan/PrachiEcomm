using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class UserConfiguration
    {
        public UserConfiguration()
        {
            Libraries = new List<UserLibraryModel>();
            Chapters = new List<ChapterModel>();
            ChapterContents = new List<ChapterContentModel>();
            UserLogin = new UserLoginModel();
            AspNetUser = new AspNetUserModel();
        }
        public List<UserLibraryModel> Libraries { get; set; }
        public List<ChapterModel> Chapters { get; set; }
        public List<ChapterContentModel> ChapterContents { get; set; }
        public UserLoginModel UserLogin { get; set; }
        public AspNetUserModel AspNetUser { get; set; }

    }
}
