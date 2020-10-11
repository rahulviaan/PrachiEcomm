using DAL.Models.Entities;
using DAL.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Repository
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly ReadEdgeCoreContext context;
        public LibraryRepository(ReadEdgeCoreContext readEdgeCoreContext)
        {
            context = readEdgeCoreContext;
        }
        public async Task Add(UserLibrary userLibrary)
        {
            await context.AddAsync(userLibrary);
            await context.SaveChangesAsync();
        }

        public async Task AddChapter(Chapter chapter)
        {
            await context.AddAsync(chapter);
            await context.SaveChangesAsync();
        }

        public async Task AddChapterContent(ChapterContent chapterContent)
        {
            await context.AddAsync(chapterContent);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            UserLibrary userLibrary = context.UserLibrary.Find(Id);
            if (userLibrary != null)
            {
                context.UserLibrary.Remove(userLibrary);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteChapter(int Id)
        {
            Chapter chapter = context.Chapter.Find(Id);
            if (chapter != null)
            {
                context.Chapter.Remove(chapter);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteChapterContent(int Id)
        {
            ChapterContent chapterContent = context.ChapterContent.Find(Id);
            if (chapterContent != null)
            {
                context.ChapterContent.Remove(chapterContent);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserLibrary>> GetAllLibrary()
        {
            return await context.UserLibrary.ToListAsync();
        }

        public async Task GetChapter(int Id)
        {
            await context.Chapter.FindAsync(Id);
        }

        public async Task<ChapterContent> GetChapterContent(long Id)
        {
            return await context.ChapterContent.FindAsync(Id);
        }


        public async Task<IEnumerable<ChapterContent>> GetChapterContents()
        {
            return  await context.ChapterContent.ToListAsync();
        }

        public async Task<IEnumerable<Chapter>> GetChapters()
        {
            return await context.Chapter.ToListAsync();
        }


        public async Task<UserLibrary> GetLibrary(string Id)
        {
           return await context.UserLibrary.FindAsync(Id);
        }


        public async Task Update(UserLibrary userLibrary)
        {
            var userLibraries = context.UserLibrary.Attach(userLibrary);
            userLibraries.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateChapter(Chapter chapter)
        {
            var chapters = context.Chapter.Attach(chapter);
            chapters.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task UpdateChapterContent(ChapterContent chapterContent)
        {
            var chapterContents = context.ChapterContent.Attach(chapterContent);
            chapterContents.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<ClassModel> GetClass(string Id)
        {
            return await context.ClassModel.FindAsync(Id);
        }

        public async Task<IEnumerable<ClassModel>> GetClass()
        {
            try
            {
                return  context.ClassModel.OrderBy(t => t.OredrNo).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BookModel> GetBooks(long Id)
        {
            return await context.BookModel.FindAsync(Id);
        }

        public async Task<IEnumerable<BookModel>> GetBooks()
        {
            return  context.BookModel.ToList();
        }

        public  async Task<IEnumerable<long>> GetSubjectByClass(long Id)
        {
            var result = await context.SubjectClass.ToListAsync();
            return  result.Where(x => x.ClassId == Id).Select(y => y.SubjectId);
        }

        public async Task<SubjectModel> GetSubjects(string Id)
        {
            return await context.SubjectModel.FindAsync(Id);
        }

        public async Task<IEnumerable<SubjectModel>> GetSubjects()
        {
            return  context.SubjectModel.OrderBy(t => t.OredrNo);
        }

        #region BookMarks
        public void  AddBookMarks(UserBookMark userBookMark)
        {
             context.UserBookMark.Add(userBookMark);
             context.SaveChanges();
        }

        public async Task DeleteBookMarks(long Id)
        {
            try
            {
                UserBookMark userBookMark = context.UserBookMark.Find(Id);
                if (userBookMark != null)
                {
                context.UserBookMark.Remove(userBookMark);
                await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async  Task<IEnumerable<UserBookMark>> GetBookMarks()
        {
           return await context.UserBookMark.ToListAsync();
        }

        public void AddNotes(UserNote userNote)
        {
            try
            {
              context.UserNote.Add(userNote);
              context.SaveChanges();
            }
            catch (Exception ex) {

            }
        }

        public async Task<IEnumerable<UserNote>> GetNotes()
        {
            return await context.UserNote.ToListAsync();
        }

        public async Task RemoveNotes(long Id)
        {
            try
            {
                UserNote userNote = context.UserNote.Find(Id);
                if (userNote != null)
                {
                    context.UserNote.Remove(userNote);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
