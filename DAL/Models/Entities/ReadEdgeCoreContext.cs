using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Models.Entities
{
    public partial class ReadEdgeCoreContext : DbContext
    {
        public ReadEdgeCoreContext()
        {
        }

        public ReadEdgeCoreContext(DbContextOptions<ReadEdgeCoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BoardModel> BoardModel { get; set; }
        public virtual DbSet<BookModel> BookModel { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<ChapterContent> ChapterContent { get; set; }
        public virtual DbSet<ClassModel> ClassModel { get; set; }
        public virtual DbSet<SeriesModel> SeriesModel { get; set; }
        public virtual DbSet<SubjectClass> SubjectClass { get; set; }
        public virtual DbSet<SubjectModel> SubjectModel { get; set; }
        public virtual DbSet<UserLibrary> UserLibrary { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<UserMaster> UserMaster { get; set; }
        public virtual DbSet<UserReader> UserReader { get; set; }
        public virtual DbSet<UserBookMark> UserBookMark { get; set; }
        public virtual DbSet<UserNote> UserNote { get; set; }
        public virtual DbSet<SessionMgt> SessionMgt { get; set; }
        public virtual DbSet<ClassSubjects> ClassSubjects { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }



    }
}
