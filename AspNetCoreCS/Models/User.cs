using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models.Entities;
using DAL.Models.Interfaces;
using ReadEdgeCore.Models.Interfaces;
using ReadEdgeCore.Utilities;

namespace ReadEdgeCore.Models
{
    public class User:IUser
    {
        private IUserRepository _userRepository;

        public User(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Add(UserMaster userMaster)
        {
            await  _userRepository.Add(userMaster);
        }

        public async Task AddUserLogin(UserLogin userLogin)
        {
            await _userRepository.AddUserLogin(userLogin);
        }

        public async Task Delete(int Id)
        {
            await _userRepository.Delete(Id);
        }

        public async Task<IEnumerable<UserMaster>> GetAllUser()
        {
          return await _userRepository.GetAllUser();
        }

        public async Task GetUser(int Id)
        {
            await _userRepository.GetUser(Id);
        }

        public async Task GetUserLogin(int Id)
        {
            await _userRepository.GetUserLogin(Id);
        }

        //public async Task<UserLogin> GetUserLoginAsync(string UserName, string Password)
        //{
        //    UserName = UserName.Trim().ToLower();
        //    var aspNetUser = await _userRepository.GetUserLoginAsync(UserName, Cryptography.EncryptCommon(Password));
        //    return aspNetUser;
        //}

        public async Task<UserLogin> GetUserLogin(string UserName, string Password)
        {
            UserName = UserName.Trim().ToLower();
            var aspNetUser = await _userRepository.GetUserLogin(UserName, Cryptography.EncryptCommon(Password));
            return aspNetUser;
        }
        public async Task Update(UserMaster userMaster)
        {
            await _userRepository.Update(userMaster);
        }

 
    }

    public class PrachiUser : IPrachiuser
    {
        private IPrachiUserRepository _prachiUserRepository;

        public PrachiUser(IPrachiUserRepository userRepository)
        {
            _prachiUserRepository = userRepository;
        }

        public async Task<AspNetUsers> GetUserLogin(string UserName, string Password)
        {
            UserName = UserName.Trim().ToLower();
            //var aspNetUser = await _prachiUserRepository.GetUserLogin(UserName, Cryptography.EncryptCommon(Password));
            var encPassword = Cryptography.EncryptCommon(Password);
            Task<AspNetUsers> user = _prachiUserRepository.GetUserLogin(UserName, encPassword);
            var aspNetUser = await user;
            return aspNetUser;
        }

        public AspNetRoles GetUserRole(string userid)
        {
            var aspNetRoles = _prachiUserRepository.GetUserRole(userid);
            return aspNetRoles;
        }
    }
}
