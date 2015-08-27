using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubMap.DbModels;
using ClubMap.DbModels.Sqlite;
using ClubMap.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ClubMap.Models;
using Constants = ClubMap.Common.Constants;

namespace ClubMap
{
    public partial class Startup
    {
        /// <summary>
        /// Gets or sets OAuthOptions.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// Client Id
        /// </summary>
        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
            
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
        public static void InitDatabaseAndUsers()
        {
            try
            {
                using (var sqlite = new SqLiteDbContext())
                using (var db = new ApplicationDbContext())
                {
                    SetupUsers(db);
                    PopulateDatabase(sqlite);
                    db.SaveChanges();
                    sqlite.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Init database and users failed with error(s):\n" + e.Message);
            }
        }
        
        private class UserSetup
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool LockoutEnabled { get; set; }
        }

        private static readonly List<UserSetup> UsersToSetup = new List<UserSetup>() 
        {
            new UserSetup { Email = "wiktor@clubmap.com", Password = @"W1kt0r!32", LockoutEnabled = false },
        };

        private static void PopulateDatabase(SqLiteDbContext db)
        {
            foreach (var day in Constants.Days)
            {
                db.Days.AddOrUpdate(x => x.Id, 
                new Day
                {
                    Id = day.Value,
                    Name = day.Key,
                });
            }

            db.EnterPriceDays.AddOrUpdate(x => x.Id,
                new EnterPriceDay
                {
                    Id = 1,
                    Price = "Closed",
                });
            db.EnterPriceDays.AddOrUpdate(x => x.Id, 
                new EnterPriceDay
                {
                    Id = 2,
                    Price = "",
                });
            db.EnterPriceDays.AddOrUpdate(x => x.Id, 
                new EnterPriceDay
                {
                    Id = 3,
                    Price = "k10/m20",
                });
        }

        private static void SetupUsers(ApplicationDbContext db)
        {
            using (var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                // Creating roles
                foreach (var role in Enum.GetValues(typeof(Constants.UserRole)))
                {
                    if (rm.RoleExists(role.ToString())) continue;
                    var result = rm.Create(new IdentityRole(role.ToString()));

                    if (!result.Succeeded)
                        throw new ApplicationException("Creating role " + role + " failed with error(s):\n" + GetAllErrors(result));
                }
                // Creating users
                foreach (var newUser in UsersToSetup)
                {
                    var existingUser = um.FindByEmail(newUser.Email);
                    if (existingUser == null)
                    {
                        var result = um.Create(new ApplicationUser
                        {
                            Email = newUser.Email,
                            EmailConfirmed = true,
                            UserName = newUser.Email,
                            LockoutEnabled = newUser.LockoutEnabled
                        },
                        newUser.Password);

                        if (!result.Succeeded)
                            throw new ApplicationException("Creating user " + newUser.Email + " failed with error(s):\n" + GetAllErrors(result));
                    }
                    existingUser = um.FindByEmail(newUser.Email);

                    if (!um.IsInRole(existingUser.Id, Constants.UserRole.Admin.ToString()))
                    {
                        var result = um.AddToRole(existingUser.Id, Constants.UserRole.Admin.ToString());

                        if (!result.Succeeded)
                            throw new ApplicationException("Adding role " + Constants.UserRole.Admin + " for " + newUser.Email + " failed with error(s):\n" + GetAllErrors(result));
                    }
                }
                db.SaveChanges();
            }
        }
        private static string GetAllErrors(IdentityResult result)
        {
            if (result.Errors == null ||
                !result.Errors.Any())
                return null;
            var sb = new StringBuilder();
            foreach (var error in result.Errors)
                sb.AppendLine(error);
            return sb.ToString();
        }
    }
}