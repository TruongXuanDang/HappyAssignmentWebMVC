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
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;

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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            //public override void ExecuteResult(ControllerContext context)
            //{
            //    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            //    if (UserId != null)
            //    {
            //        properties.Dictionary[XsrfKey] = UserId;
            //    }
            //    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            //}
        }
    }
}