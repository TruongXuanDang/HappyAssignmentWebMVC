using HappyMVCAssignment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
//[assembly: OwinStartup(typeof(HappyMVCAssignment.App_Start.IdentityConfig))]
namespace HappyMVCAssignment.App_Start
{
    public class IdentityConfig
    {
        
    }

    public class MyUserManager:UserManager<Account>
    {
        public MyUserManager(IUserStore<Account> store) :base(store) { }
        public static MyUserManager Create(IdentityFactoryOptions<MyUserManager> options, IOwinContext context)
        {
            var userManager = new MyUserManager(new UserStore<Account>(new HappyMVCAssignmentContext()));
            return userManager;
        }

    }
}