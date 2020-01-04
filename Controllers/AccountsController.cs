using HappyMVCAssignment.App_Start;
using HappyMVCAssignment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HappyMVCAssignment.Controllers
{
    public class AccountsController : Controller
    {
        private HappyMVCAssignmentContext dbContext;
        private MyUserManager userManager;

        public MyUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<MyUserManager>();
            }
            set
            {
                userManager = value;
            }
        }
        public HappyMVCAssignmentContext DbContext
        {
            get
            {
                return dbContext ?? HttpContext.GetOwinContext().Get<HappyMVCAssignmentContext>();
            }
            set
            {
                dbContext = value;
            }
        }

        public AccountsController()
        {
            
        }

        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Account account = UserManager.Find(username, password);
            if(account == null)
            {
                return HttpNotFound();
            }
            var ident = UserManager.CreateIdentity(account, DefaultAuthenticationTypes.ApplicationCookie);
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = false},ident);

            return Redirect("/Home");
        }

        [HttpPost]
        public ActionResult Logout () 
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/Home");
        }

        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProcessRegister(string username, string password)
        {
            var account = new Account()
            {
                UserName = username,
                Email = username,
                FirstName = "Xuan Hung",
                LastName = "Dao",
                Avatar = "avatar",
                Birthday = DateTime.Now,
                CreatedAt = DateTime.Now,
            };
            IdentityResult result = UserManager.Create(account, password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(account.Id, "Admin");
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                await UserManager.SendEmailAsync(account.Id, "Hello world! Please confirm your account", "<b>Please confirm your account</b> by clicking <a href=\"http://google.com.vn\">here</a>");
                return RedirectToAction("Index", "Home");
            }
            return View("Register");
        }
    }
}