using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Security
{
    public enum UnregisteredUserType
    {
        Undefined, Employee, Customer
    }
    class UnregisteredUserProfile
    {
        public string UserId { get; set; } //generated when user added
        public string UserName { get; set; } //collected from form
        public string FirstName { get; set; } //comes from user table
        public string LastName { get; set; } //comes from user table
        public string Email { get; set; } //collected from form
        public UnregisteredUserType UserType { get; set; }

        
    }
}
