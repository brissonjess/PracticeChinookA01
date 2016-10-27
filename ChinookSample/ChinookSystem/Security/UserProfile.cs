using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Security
{
    public class UserProfile
    {
        public string UserId { get; set; } //from AspNet User table
        public string UserName { get; set; }
        public int? EmployeeId { get; set; }//from AspNet User table
        public int? CustomerId { get; set; } //from AspNet User table
        public string FirstName { get; set; } //from Employee or Customer table
        public string LastName { get; set; }//from Employee or Customer table
        public string Email { get; set; } // from AspNet User table
        public bool EmailConfirm { get; set; } //from AspNet User table
        public IEnumerable<string> RoleMemberships { get; set; }


    }
}
