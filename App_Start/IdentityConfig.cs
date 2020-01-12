using HappyMVCAssignment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
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
            var manager = new MyUserManager(new UserStore<Account>(new HappyMVCAssignmentContext()));
            manager.UserValidator = new UserValidator<Account>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<Account>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Account>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Account>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

    }

    public class EmailService : IIdentityMessageService
    {

        public Task SendAsync(IdentityMessage message)
        {
            ////Twilio Begin
            //var accountSid = ConfigurationManager.AppSettings["SMSAccountIdentification"];
            //var authToken = ConfigurationManager.AppSettings["SMSAccountPassword"];
            //var fromNumber = ConfigurationManager.AppSettings["SMSAccountFrom"];

            //TwilioClient.Init(accountSid, authToken);

            //MessageResource result = MessageResource.Create(
            //new PhoneNumber(message.Destination),
            //from: new PhoneNumber(fromNumber),
            //body: message.Body
            //);

            ////Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            //Trace.TraceInformation(result.Status.ToString());
            ////Twilio doesn't currently have an async API, so return success.
            ////return Task.FromResult(0);
            ////Twilio End

            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Credentials = new System.Net.NetworkCredential("truongdxth1804013@fpt.edu.vn", "phgwzunzkpeayybb");
            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;

            mail.From = new MailAddress("truongdxth1804013@fpt.edu.vn");
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;

            smtpServer.Send(mail);
            return Task.FromResult(0);
        }
    }


}