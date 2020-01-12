using HappyMVCAssignment.App_Start;
using HappyMVCAssignment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartupAttribute(typeof(HappyMVCAssignment.Startup))]
namespace HappyMVCAssignment
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.CreatePerOwinContext<HappyMVCAssignmentContext>(HappyMVCAssignmentContext.Create);
            app.CreatePerOwinContext<MyUserManager>(MyUserManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions() { 
                ClientId= "474968986415-4fg76s2qkm8tstqjfqpm7589gek8kdjv.apps.googleusercontent.com",
                ClientSecret = "W_QovJuIjviYqerLqasxtbEb",
            });

            //app.UseFacebookAuthentication(
            //   appId: "3401166659956762",
            //   appSecret: "2303fc546f53d98440adf15562714632");
        }
    }
}