using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrachiIndia.Portal.BAL
{
    internal interface IRepositry
    {
        int Add(object item);
        object Get(int id);
        IEnumerable<object> GetAll();
        bool Remove(int id);
        bool Update(object item);
    }
}