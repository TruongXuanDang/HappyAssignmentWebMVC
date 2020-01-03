using HappyMVCAssignment.App_Start;
using HappyMVCAssignment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(HappyMVCAssignment.Startup))]
namespace HappyMVCAssignment
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<HappyMVCAssignmentContext>(HappyMVCAssignmentContext.Create);
            app.CreatePerOwinContext<MyUserManager>(MyUserManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}