using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Interfaces
{
    public interface ILibraryRepository
    {
        #region Class
        Task<ClassModel> GetClass(string Id);
        Task<IEnumerable<ClassModel>> GetClass();
        #endregion
        #region Books
        Task<BookModel> GetBooks(Int64 Id);
        Task<IEnumerable<BookModel>> GetBooks();
        #endregion

        #region Subjects
        Task<SubjectModel> GetSubjects(string Id);
        Task<IEnumerable<SubjectModel>> GetSubjects();
        Task<IEnumerable<long>> GetSubjectByClass(long Id);
        #endregion

        #region Library
        Task<UserLibrary> GetLibrary(string Id);
        Task<IEnumerable<UserLibrary>> GetAllLibrary();
        Task Add(UserLibrary userLibrary);
        Task Update(UserLibrary userLibrary);
        Task Delete(int Id);
        #endregion

        #region Chapters
        Task GetChapter(int Id);
        Task<IEnumerable<Chapter>> GetChapters();
        Task AddChapter(Chapter chapter);
        Task UpdateChapter(Chapter chapter);
        Task DeleteChapter(int Id);
        #endregion

        #region ChapterContents
        Task<ChapterContent> GetChapterContent(long Id);
        Task<IEnumerable<ChapterContent>> GetChapterContents();
        Task AddChapterContent(ChapterContent chapter);
        Task UpdateChapterContent(ChapterContent chapter);
        Task DeleteChapterContent(int Id);
        #endregion

        #region BookMarks
        Task<IEnumerable<UserBookMark>> GetBookMarks();
        void AddBookMarks(UserBookMark userBookMark);
        Task DeleteBookMarks(long Id);
        #endregion

        #region Notes
        Task<IEnumerable<UserNote>> GetNotes();
        void AddNotes(UserNote userNote);
        Task RemoveNotes(long Id);
        #endregion

        List<ClassSubjects> GetSubjectByClassType(int Classtype);

    }
}
