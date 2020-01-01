using HappyMVCAssignment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyMVCAssignment.Controllers
{
    public class AccountsController : Controller
    {
        private HappyMVCAssignmentContext dbContext = new HappyMVCAssignmentContext();
        private UserManager<Account> userManager;

        public AccountsController()
        {
            UserStore<Account> userStore = new UserStore<Account>(dbContext);
            userManager = new UserManager<Account>(userStore);
        }

        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Account account = userManager.Find(username, password);
            if(account == null)
            {
                return HttpNotFound();
            }
            var ident = userManager.CreateIdentity(account, DefaultAuthenticationTypes.ApplicationCookie);
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
        public ActionResult ProcessRegister(string username, string password)
        {
            var account = new Account()
            {
                UserName = username,
                FirstName = "Xuan Hung",
                LastName = "Dao",
                Avatar = "avatar",
                Birthday = DateTime.Now,
                CreatedAt = DateTime.Now,
            };
            IdentityResult result = userManager.Create(account, password);
            userManager.AddToRole(account.Id, "Admin");
            return View("Register");
        }
    }
}