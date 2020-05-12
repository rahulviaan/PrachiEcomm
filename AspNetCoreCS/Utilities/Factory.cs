using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.Entities;
using ReadEdgeCore.Models.ViewModel;

namespace ReadEdgeCore.Utilities
{
    public static class Factory
    {
        public static List<ClassModel> GetClassModelList() {
            return new List<ClassModel>();
        }
        public static List<BookModel> GetBookModelList()
        {
            return new List<BookModel>();
        }
        public static List<ChapterContent> GetChapterContentlList()
        {
            return new List<ChapterContent>();
        }
        public static List<UserLibrary> GetUserLibraryList()
        {
            return new List<UserLibrary>();
        }
        public static LibraryVM GetLibraryVM()
        {
            return new LibraryVM();
        }
        public static List<LibraryVM> GetLibrayVMList()
        {
            return new List<LibraryVM>();
        }
        public static List<Chapter> GetChapterList()
        {
            //return new LibraryVM();
            return new List<Chapter>();
        }

        public static UserNote GetUserNote()
        {
            return new UserNote();
        }
    }
}
