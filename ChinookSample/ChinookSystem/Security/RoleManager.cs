﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Microsoft.AspNet.Identity;                    //RoleManger
using Microsoft.AspNet.Identity.EntityFramework;    //IdentityRole, RoleStore
using System.ComponentModel;                        //Needed for ODS
#endregion
namespace ChinookSystem.Security
{
    [DataObject]
    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager() :
            base(new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {

        }

        //this method will be executed when the application starts up
        //under IIS
        public void AddStartupRoles()
        {
            foreach (string roleName in SecurityRoles.StartupSecurityRoles)
            {
                //check if the roles already exists in the Security Tables
                //located in the database
                if (!Roles.Any(r => r.Name.Equals(roleName)))
                {
                    //role is not currently on the database
                    this.Create(new IdentityRole(roleName));
                }
            }
        }//eom

        // SELECT A ROLE (READ)
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RoleProfile> ListAllRoles()
        {
            var um = new UserManager();
            //the data from Roles needs to be in memory for use by the query
            //use .toList() to be used by the query. this returns all information and stores it in memory
            var results = from role in Roles.ToList()
                          select new RoleProfile
                          {
                              RoleId = role.Id,         //security table
                              RoleName = role.Name,     //security table
                              UserNames = role.Users.Select(r => um.FindById(r.UserId).UserName)//using a role > in user > select the id by a articular role > return the name

                          };
            return results.ToList();
        }//eom 

        //INSERT A NEW RECORD (ROLE)
        [DataObjectMethod(DataObjectMethodType.Insert,true)]
        //putting Insert, true and it will insert into the default
        public void AddRole(RoleProfile role)
        {
            //any business rules to consider?
            //the role should not already exist on the Roles table
            if (!this.RoleExists(role.RoleName))
            {
                this.Create(new IdentityRole(role.RoleName));
            }
        }//eom

        //DELETE A RECORD (ROLE)
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void RemoveRole(RoleProfile role)
        {
            this.Delete(this.FindById(role.RoleId));

        }//eom

        //this method will produce a list of all role names
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<string> ListAllRoleNames()
        {
            return this.Roles.Select(r => r.Name).ToList();
        }


    }//eoc
}//eon
