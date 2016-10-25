using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.AspNet.Identity.EntityFramework;    //UserStore
using Microsoft.AspNet.Identity;                    //UserManager
using System.ComponentModel;                        //ODS
using ChinookSystem.DAL;                            //context class
#endregion
namespace ChinookSystem.Security
{
    [DataObject]
    public class UserManager : UserManager<ApplicationUser>
    {
        public UserManager()
            : base(new UserStore<ApplicationUser>(new ApplicationDbContext()))
        {
        }
        //setting up the default webMaster
        #region Constants 
        private const string STR_DEFAULT_PASSWORD = "Pa$$word1";
        private const string STR_USERNAME_FORMAT = "{0}.{1}";
        private const string STR_EMAIL_FORMAT = "{0}@Chinook.ca";
        private const string STR_WEBMASTER_USERNAME = "Webmaster";
        #endregion
        public void AddWebmaster() {
            if (!Users.Any(u => u.UserName.Equals(STR_WEBMASTER_USERNAME)))
            {
                var webMasterAccount = new ApplicationUser()
                {
                    UserName = STR_WEBMASTER_USERNAME,
                    Email = string.Format(STR_EMAIL_FORMAT, STR_WEBMASTER_USERNAME)

                };
                //this create command is from the inherited UserManager class
                //this command creates a record on the security Users table (AspNetUsers)
                this.Create(webMasterAccount, STR_DEFAULT_PASSWORD);
                //this AddToRole command is from the inherited UserManager Class
                //this command creates a record on the security UserRole table (AspNetUserRole)
                this.AddToRole(webMasterAccount.Id, SecurityRoles.WebsiteAdmins);
            }
        }// eom

        //create the CRUD methods for adding a user to the security User table
        //read of data to display on grid view
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnRegisteredUserProfile> ListAllUnregisteredUser()
        {
            using (var context = new ChinookContext())
            {
                //the data needs to be in memory for execution by the next query 
                    // to accomplish this use .ToList() which will force the query to execute
                    // the data will not load into memory if you use IEnumerable data sets
                    // you convert the data sets to list by wrapping your select statement in brackets and use the .ToList() method
                //List() set containing the list of employeeIds
                var registeredEmployees = (from emp in Users
                                          where emp.EmployeeId.HasValue
                                          select emp.EmployeeId).ToList();
                //compare the List() set to the user data table Emloyees
                var unregisteredEmployees = (from emp in context.Employees
                                            where !registeredEmployees.Any(eid => emp.EmployeeId == eid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                UserId = emp.EmployeeId,
                                                FirstName = emp.FirstName,
                                                LastName = emp.LastName,
                                                UserType = UnregisteredUserType.Employee
                                            }).ToList();

                //List() set containing the list of customerIds
                var registeredCustomers = (from cust in Users
                                          where cust.CustomerId.HasValue
                                          select cust.CustomerId).ToList();
                //compare the List() set to the user data table Emloyees
                var unregisteredCustomers = (from cust in context.Customers
                                            where !registeredCustomers.Any(cid => cust.CustomerId == cid)
                                            select new UnRegisteredUserProfile()
                                            {
                                                UserId = cust.CustomerId,
                                                FirstName = cust.FirstName,
                                                LastName = cust.LastName,
                                                UserType = UnregisteredUserType.Customer
                                            }).ToList();
                //combine the two physically identical layout datasets
                return unregisteredEmployees.Union(unregisteredCustomers).ToList();
            } // \ of using
            
                             
        }//eom
        
        //register a user to the User table (from gridview)
        public void RegisterUser(UnRegisteredUserProfile userinfo)
        {
            //basic information needed for the security user record: password, email, and user name 
            //note you could randomly generate a password, we will use the default password
            //the instance of the required user is based on our ApplicationUser, which inherits from IdentityUser 
            var newuseraccount = new ApplicationUser()
            {
                UserName = userinfo.UserName,
                Email = userinfo.Email,
            };

            //set the customerId or employeeId (switch)
            switch (userinfo.UserType)
            {
                case UnregisteredUserType.Customer:
                    {
                        newuseraccount.Id = userinfo.UserId.ToString();
                        break;
                    }
                case UnregisteredUserType.Employee:
                    {
                        newuseraccount.Id = userinfo.UserId.ToString();
                        break;
                    }
            }
            //create the actual AspNetUser record 
            this.Create(newuseraccount, STR_DEFAULT_PASSWORD);

            //now that the user record is in there you can also assign it a role 
            switch (userinfo.UserType)
            {
                case UnregisteredUserType.Customer:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.RegisteredUsers);
                        break;
                    }
                case UnregisteredUserType.Employee:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.Staff);
                        break;
                    }
            }


        }//eom

        //add a user to the User Table (from ListView)

        //delete user from the User table (from Listview)


    }//eoc - (end of class)
}//eon (end of namespace)
