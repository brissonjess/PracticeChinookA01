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
using ChinookSystem.Data.Entities;                  //entity classes
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
        public List<UnRegisteredUserProfile> ListAllUnRegisteredUser()
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
                                                CustomerEmployeeId = emp.EmployeeId,
                                                FirstName = emp.FirstName,
                                                LastName = emp.LastName,
                                                UserType = UnRegisteredUserType.Employee
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
                                                CustomerEmployeeId = cust.CustomerId,
                                                FirstName = cust.FirstName,
                                                LastName = cust.LastName,
                                                UserType = UnRegisteredUserType.Customer
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
                UserName = userinfo.AssignedUserName,
                Email = userinfo.AssignedEmail,
            };

            //set the customerId or employeeId (switch)
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        newuseraccount.CustomerId = userinfo.CustomerEmployeeId;
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        newuseraccount.EmployeeId = userinfo.CustomerEmployeeId;
                        break;
                    }
            }
            //create the actual AspNetUser record 
            this.Create(newuseraccount, STR_DEFAULT_PASSWORD);

            //assign user to an appropriate role 
            //uses the guid like user Id from the User's table 
            switch (userinfo.UserType)
            {
                case UnRegisteredUserType.Customer:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.RegisteredUsers);
                        break;
                    }
                case UnRegisteredUserType.Employee:
                    {
                        this.AddToRole(newuseraccount.Id, SecurityRoles.Staff);
                        break;
                    }
            }


        }//eom

        //list all current users 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UserProfile> ListAllUsers()
        {
            //we will be using the RoleManager to get roles
            var rm = new RoleManager();
            //get the current users off the user security table 
            var results = from person in Users.ToList() //stores all info into memory 
                          select new UserProfile()
                          {
                              UserId = person.Id,
                              UserName = person.UserName,
                              Email = person.Email,
                              EmailConfirm = person.EmailConfirmed,
                              CustomerId = person.CustomerId,
                              EmployeeId = person.EmployeeId,
                              RoleMemberships = person.Roles.Select(r => rm.FindById(r.RoleId).Name)//every role that a person is assigned to will be selected and displayed by name based off the RoleId
                          };
            //using our own data tables gather the user FirstName and LastName 
            using (var context = new ChinookContext())
            {
                Employee etemp;
                Customer ctemp;
                foreach(var person in results)
                {//for everyone we just collected 

                    if (person.EmployeeId.HasValue)
                    {
                        etemp = context.Employees.Find(person.EmployeeId);
                        person.FirstName = etemp.FirstName;
                        person.LastName = etemp.LastName;
                    }
                    else if (person.CustomerId.HasValue)
                    {
                        ctemp = context.Customers.Find(person.CustomerId);
                        person.FirstName = ctemp.FirstName;
                        person.LastName = ctemp.LastName;
                    }
                    else
                    {
                        person.FirstName = "Unknown person";
                        person.LastName = "";
                    }
                }// \ for each
            }// \ using 
            return results.ToList();
        }//eom 

        //add a user to the User Table (from ListView)
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void AddUser(UserProfile userinfo)
        {
            //create an instance representing the new user 
            var useraccount = new ApplicationUser()
            {
                UserName = userinfo.UserName,
                Email = userinfo.Email
            };

            //create the new user on the physical Users table 
            this.Create(useraccount, STR_DEFAULT_PASSWORD);
            //create the UserRoles which were chosen at insert time 
            foreach(var rolename in userinfo.RoleMemberships)
            {
                this.AddToRole(useraccount.Id, rolename);
            }
        }//eom


        //delete user from the User table (from Listview)
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void RemoveUser(UserProfile userinfo)
        {
            // business rules executed here 
            //in this case: the webmaster cannot be deleted 

            //realize that the only information you have at this time is the data key names value 
            //which is the UserId (on the user security table -- the field is Id)

            //1. we need to obtain the username from the security user table using the UserId value 
            string UserName = this.Users.Where(u => u.Id == userinfo.UserId)
                              .Select(u => u.UserName).SingleOrDefault().ToString();// states expect a single value or use the default
                            // ^^ this gets the users table, and you want it equal to the id you passed in, when you want to select the UserName and convert it to a string 
            
            //business rule ends and the functionality begins here:

            //remove the user 
            if (UserName.Equals(STR_WEBMASTER_USERNAME))
            {
                throw new Exception("The webmaster account cannot be removed.");
            }
            this.Delete(this.FindById(userinfo.UserId));
        }//eom


    }//eoc - (end of class)
}//eon (end of namespace)
