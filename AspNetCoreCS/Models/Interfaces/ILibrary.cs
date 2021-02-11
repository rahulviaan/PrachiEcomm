using DAL.Models.Entities;
using DAL.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using ReadEdgeCore.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface ILibrary: ILibraryRepository
    {
      Task UploadConfig(IFormFile PostedFile);
      Task UploadReConfig(IFormFile PostedFile);
        Task UploadBundle(LibraryVM PostedFile);
      #region BookMarks
        bool ValidBookMark(UserBookMark userBookMark);
        Task<IEnumerable<UserBookMark>> GetBookMarksByUserAndContentId(UserBookMark userBookMark);

        #endregion

        #region Notes
        Task<IEnumerable<UserNote>> GetNotesByUserAndContentId(UserNote userNote);
        #endregion
        Task AddReadEdgeTrialUser(ReadEdgeTrialUsers readEdgeTrialUsers);
        Task<IEnumerable<ReadEdgeTrialUsers>> GetReadEdgeTrialUser();
        List<ClassSubjects> GetSubjectByClassType(int ClassType);
        List<TeacherSubjectClass> GetTeacherSubjectClasses(string TeacherID);
    }
}
