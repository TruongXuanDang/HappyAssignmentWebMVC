using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
[assembly: OwinStartup(typeof(HappyMVCAssignment.App_Start.IdentityConfig))]
namespace HappyMVCAssignment.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
          
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });
        }
    }
}