using DAL.Models.Entities;
using ReadEdgeCore.Models.Interfaces;
using ReadEdgeCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class Login : ILogin
    {
        private IUser _user { get; set; }
        public Login(IUser user) {
            _user = user;
        }
         async Task<UserLogin> ILogin.Login(string UserName,string Password)
        {
            UserName = UserName.Trim().ToLower();
            var aspNetUser = await _user.GetUserLogin(UserName, Cryptography.EncryptCommon(Password));
            return aspNetUser;
        }
    }
}
