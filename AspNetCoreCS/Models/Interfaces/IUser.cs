using DAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface IUser:IUserRepository
    {

    }

    public interface IPrachiuser : IPrachiUserRepository
    { }

    
}
