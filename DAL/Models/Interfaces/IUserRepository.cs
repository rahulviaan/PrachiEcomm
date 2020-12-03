using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Entities;

namespace DAL.Models.Interfaces
{
    public interface IUserRepository
    {
        Task GetUser(int Id);
        Task<IEnumerable<UserMaster>> GetAllUser();
        Task Add(UserMaster userMaster);
        Task Update(UserMaster userMaster);
        Task Delete(int Id);
        Task GetUserLogin(int Id);
        Task<UserLogin> GetUserLogin(string UserName, string Password);
        Task AddUserLogin(UserLogin userLogin);
        //Task GetUserRole(UserLogin userLogin);
    }

    public interface IPrachiUserRepository
    {
        Task<AspNetUsers> GetUserLogin(string UserName, string Password);
        AspNetRoles GetUserRole(string userid);
        List<UserBookIds> GetUserBookIds(string userid);
        List<ReadEdgeLogins> GetReadEdgeLogins();
        ReadEdgeLogins GetReadEdgeLoginByIds(string userid);
        ReadEdgeUserLoginInfo GetReadEdgeUserLoginInfoByIds(int id);
        int UpdateReadEdgeLogin(ReadEdgeLogins readEdgeLogins);
        int UpdtaeReadEdgeUserLoginInfo(ReadEdgeUserLoginInfo readEdgeUserLoginInfo);
        int InsertReadEdgeUserLoginInfo(ReadEdgeUserLoginInfo readEdgeUserLoginInfo);
        Task<IEnumerable<ReadEdgeTrialUsers>> GetReadEdgeTrialUsers();
        Task AddReadEdgeTrialUsers(ReadEdgeTrialUsers readEdgeTrialUsers);
        List<ClassSubjects> GetSubjectByClassType(int Classtype);
    }

}
