using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface ILogin
    {
        Task<UserLogin> Login(string UserName, string Password);
    }
}
