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
        private HappyMVCAssignmentContext db = new HappyMVCAssignmentContext();
        private UserManager<Account> userManager;
        private RoleManager<AccountRole> roleManager;

        public AccountsController()
        {
            UserStore<Account> userStore = new UserStore<Account>(db);
            userManager = new UserManager<Account>(userStore);
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
            return View("Register");
        }
    }
}