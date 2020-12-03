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
     public class UserRepository : IUserRepository
     {
        private readonly ReadEdgeCoreContext context;
        public UserRepository(ReadEdgeCoreContext readEdgeCoreContext)
        {
            context = readEdgeCoreContext;
        }
        public async Task Add(UserMaster userMaster)
        {
            await context.AddAsync(userMaster);
            await context.SaveChangesAsync();
            //return userMaster;
        }

        public async Task AddUserLogin(UserLogin userLogin)
        {
            await context.AddAsync(userLogin);
            await context.SaveChangesAsync();
            //return userLogin;
        }
        public async Task Delete(int Id)
        {
            UserMaster userMaster = context.UserMaster.Find(Id);
            if (userMaster != null)
            {
                context.UserMaster.Remove(userMaster);
                await context.SaveChangesAsync();
            }
            //return userMaster;
        }

        public async Task<IEnumerable<UserMaster>> GetAllUser()
        {
            return await context.UserMaster.ToListAsync();
        }

        public async Task GetUserLogin(int Id)
        {
            await context.UserLogin.FindAsync(Id);
        }

        public async Task GetUser(int Id)
        {
           await context.UserMaster.FindAsync(Id);
        }

        public async Task Update(UserMaster userMaster)
        {
            var employee = context.UserMaster.Attach(userMaster);
            employee.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<UserLogin> GetUserLogin(string UserName, string Password)
        {
            try
            {
                var res = await context.UserLogin.FirstOrDefaultAsync(x => x.UserName == UserName && x.Password == Password);
                return res;
            }
            catch (Exception ex)
            { return null; }

            //return await context.UserLogin.FirstOrDefaultAsync(x=>x.UserName==UserName && x.Password==Password);
        }

    }

    public class PrachiUserLoginRepository :IPrachiUserRepository
    {
        private readonly UserContext context;
        public PrachiUserLoginRepository(UserContext userContext)
        {
            context = userContext;
        }



        public async Task<AspNetUsers> GetUserLogin(string UserName, string Password)
        {
            try
            {
                //Task<AspNetUsers> user =  context.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == UserName && x.PasswordHash == Password);
                AspNetUsers user =  context.AspNetUsers.Select(x=> new AspNetUsers{Id=x.Id,Email=x.Email,UserName=x.UserName,PasswordHash=x.PasswordHash}).FirstOrDefault(x => x.UserName == UserName && x.PasswordHash == Password);
                var res =   user;
               // var res = await context.AspNetUsers.FirstOrDefaultAsync(x => x.UserName == UserName && x.PasswordHash == Password);
                return res;
            }
            catch (Exception ex)
            { return null; }

            //return await context.UserLogin.FirstOrDefaultAsync(x=>x.UserName==UserName && x.Password==Password);
        }

        public AspNetRoles GetUserRole(string userid)
        {
            try
            {
                var aspNetRoles = context.AspNetRoles.FromSql("GETUserRole @p0", userid)
                       .FirstOrDefault();
                return aspNetRoles;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UserBookIds> GetUserBookIds(string userid)
        {
            try
            {
                var userBookIds = context.UserBookIds.FromSql("GETUserBookIds @p0", userid)
                       .ToList();
                return userBookIds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ClassSubjects> GetSubjectByClassType(int Classtype)
        {
            try
            {
                var ClassSubjects = context.ClassSubjects.FromSql("USP_GET_SUBJECT_BY_CLASS @p0", Classtype)
                       .ToList();
                return ClassSubjects;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ReadEdgeLogins> GetReadEdgeLogins()
        {
            return context.ReadEdgeLogins.ToList();
        }

        public ReadEdgeLogins GetReadEdgeLoginByIds(string userid)
        {
            try
            {
                return context.ReadEdgeLogins.FirstOrDefault(x => x.Userid == userid);
            }
            catch (Exception ex) {
                var result = ex;
                return null;
            }
        }
        public int UpdateReadEdgeLogin(ReadEdgeLogins readEdgeLogins)
        {
             context.Entry(readEdgeLogins).State=EntityState.Modified;
             return context.SaveChanges();
        }

        public int InsertReadEdgeUserLoginInfo(ReadEdgeUserLoginInfo readEdgeUserLoginInfo)
        {
            context.ReadEdgeUserLoginInfo.Add(readEdgeUserLoginInfo);
            return context.SaveChanges();
        }
        public int UpdtaeReadEdgeUserLoginInfo(ReadEdgeUserLoginInfo readEdgeUserLoginInfo)
        {
            context.Entry(readEdgeUserLoginInfo).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public ReadEdgeUserLoginInfo GetReadEdgeUserLoginInfoByIds(int id)
        {
            try
            {
                return context.ReadEdgeUserLoginInfo.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                var result = ex;
                return null;
            }
        }

        public async Task<IEnumerable<ReadEdgeTrialUsers>> GetReadEdgeTrialUsers()
        {
            return await context.ReadEdgeTrialUsers.ToListAsync();
            //return userLogin;
        }

        public async Task AddReadEdgeTrialUsers(ReadEdgeTrialUsers readEdgeTrialUsers)
        {
            try
            {
                await context.AddAsync(readEdgeTrialUsers);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            //return userLogin;
        }
    }
}
