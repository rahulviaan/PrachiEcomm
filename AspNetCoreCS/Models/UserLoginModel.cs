using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class UserLoginModel
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string ExpiresIn { get; set; }
        public string UserName { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
        public string UserRoles { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
