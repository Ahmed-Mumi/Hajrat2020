using Hajrat2020.Models;
using Hajrat2020.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hajrat2020.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = "/Donations/GetDonations";
            if (!Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("GetDonations", "Donations");
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid || !GetUser(model))
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Unesite ispravne podatke.");
                    return View(model);
            }
        }

        public bool GetUser(LoginViewModel model)
        {
            var user = ctx.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("", "Korisnik ne postoji!");
                return false;
            }
            else
            {
                if (!user.Active)
                {
                    ModelState.AddModelError("", "Korisnik nije aktivan!");
                    return false;
                }
            }
            return true;
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        ApplicationDbContext ctx = new ApplicationDbContext();
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userViewModel = new UserViewModel()
                {
                    Genders = ctx.Genders.ToList(),
                    Cities = ctx.Cities.ToList()
                };
                return RedirectToAction("AddUser", "User", userViewModel);
            }
            var user = new ApplicationUser();
            if (!String.IsNullOrWhiteSpace(model.Id))
            {
                var editActionResult = EditUser(model, user);
                if (editActionResult != null)
                {
                    return editActionResult;
                }
            }
            else
            {
                var addActionResult = AddUser(model, user);
                if (addActionResult != null)
                {
                    return addActionResult;
                }
            }
            ctx.SaveChanges();
            return RedirectToAction("GetUsers", "User");
        }

        public ActionResult AddUser(UserViewModel model,ApplicationUser user)
        {
            AutoMapper.Mapper.Map(model, user);
            var result = UserManager.Create(user, model.PasswordHash);
            if (!result.Succeeded)
            {
                model.Cities = ctx.Cities.ToList();
                model.Genders = ctx.Genders.ToList();
                AddErrors(result, model.Email);
                return View("~/Views/User/AddEditUser.cshtml", model);
            }
            AddRole(model, user);
            return null;
        }

        public ActionResult EditUser(UserViewModel model, ApplicationUser user)
        {
            user = ctx.Users.Where(x => x.Id == model.Id).FirstOrDefault();
            AutoMapper.Mapper.Map(model, user);
            if (!String.IsNullOrWhiteSpace(model.NewPassword))
            {
                return ChangePassword(model);
            }
            ChangeRole(model, user);
            if (model.Id == User.Identity.GetUserId())
            {
                return SaveStayOnProfile(model);
            }
            return null;
        }

        public ActionResult ChangePassword(UserViewModel model)
        {
                string resetToken = UserManager.GeneratePasswordResetToken(model.Id);
                var result = UserManager.ResetPassword(model.Id, resetToken, model.NewPassword);
                if (!result.Succeeded)
                {
                    model.Cities = ctx.Cities.ToList();
                    model.Genders = ctx.Genders.ToList();
                    var loggedUserId = User.Identity.GetUserId();
                    model.FamilyUsers = ctx.FamilyUsers.Include(x => x.FamilyInNeed).Where(x => x.UserId == loggedUserId);
                    AddErrors(result, model.Email);
                    return View("~/Views/User/AddEditUser.cshtml", model);
                }
            return null;
        }

        public ActionResult SaveStayOnProfile(UserViewModel model)
        {
                ctx.SaveChanges();
                return RedirectToAction("EditUser", "User", new { id = model.Id });
        }

        public void ChangeRole(UserViewModel model, ApplicationUser user)
        {
            if (model.IsChosenRoleAdmin && user.RoleName == RoleName.User)
            {
                UserManager.RemoveFromRole(user.Id, RoleName.User);
                UserManager.AddToRole(user.Id, RoleName.Admin);
                user.RoleName = RoleName.Admin;
            }
            else if (!model.IsChosenRoleAdmin && user.RoleName == RoleName.Admin)
            {
                UserManager.RemoveFromRole(user.Id, RoleName.Admin);
                UserManager.AddToRole(user.Id, RoleName.User);
                user.RoleName = RoleName.User;
            }
        }

        public void AddRole(UserViewModel model, ApplicationUser user)
        {
            var userRoleName = ctx.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            if (!model.IsChosenRoleAdmin)
            {
                UserManager.AddToRole(user.Id, RoleName.User);
                userRoleName.RoleName = RoleName.User;
            }
            else
            {
                UserManager.AddToRole(user.Id, RoleName.Admin);
                userRoleName.RoleName = RoleName.Admin;
            }
        }

        //public async Task<ActionResult> Save(UserViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var userViewModel = new UserViewModel()
        //        {
        //            Genders = ctx.Genders.ToList(),
        //            Cities = ctx.Cities.ToList()
        //        };
        //        return RedirectToAction("AddUser", "User", userViewModel);
        //    }
        //    var user = new ApplicationUser();
        //    if (!String.IsNullOrWhiteSpace(model.Id))
        //    {
        //        user = ctx.Users.Where(x => x.Id == model.Id).FirstOrDefault();
        //        AutoMapper.Mapper.Map(model, user);
        //        if (!String.IsNullOrWhiteSpace(model.NewPassword))
        //        {
        //            string resetToken = await UserManager.GeneratePasswordResetTokenAsync(model.Id);
        //            var result = await UserManager.ResetPasswordAsync
        //                (model.Id, resetToken, model.NewPassword);
        //            if (!result.Succeeded)
        //            {
        //                model.Cities = ctx.Cities.ToList();
        //                model.Genders = ctx.Genders.ToList();
        //                var loggedUserId = User.Identity.GetUserId();
        //                model.FamilyUsers = ctx.FamilyUsers.Include(x => x.FamilyInNeed)
        //                    .Where(x => x.UserId == loggedUserId);
        //                AddErrors(result, model.Email);
        //                return View("~/Views/User/AddEditUser.cshtml", model);
        //            }
        //        }
        //        if (model.IsChosenRoleAdmin)
        //        {
        //            if (user.RoleName == RoleName.User)
        //            {
        //                await UserManager.RemoveFromRoleAsync(user.Id, RoleName.User);
        //                await UserManager.AddToRoleAsync(user.Id, RoleName.Admin);
        //                user.RoleName = RoleName.Admin;
        //            }
        //        }
        //        else
        //        {
        //            if (user.RoleName == RoleName.Admin)
        //            {
        //                await UserManager.RemoveFromRoleAsync(user.Id, RoleName.Admin);
        //                await UserManager.AddToRoleAsync(user.Id, RoleName.User);
        //                user.RoleName = RoleName.User;
        //            }
        //        }
        //        if (model.Id == User.Identity.GetUserId())
        //        {
        //            ctx.SaveChanges();
        //            return RedirectToAction("EditUser", "User", new { id = model.Id });
        //        }
        //    }
        //    else
        //    {
        //        AutoMapper.Mapper.Map(model, user);
        //        var result = await UserManager.CreateAsync(user, model.PasswordHash);
        //        if (!result.Succeeded)
        //        {
        //            model.Cities = ctx.Cities.ToList();
        //            model.Genders = ctx.Genders.ToList();
        //            AddErrors(result, model.Email);
        //            return View("~/Views/User/AddEditUser.cshtml", model);
        //        }
        //        var userRoleName = ctx.Users.Where(x => x.Id == user.Id).FirstOrDefault();
        //        if (!model.IsChosenRoleAdmin)
        //        {
        //            await UserManager.AddToRoleAsync(user.Id, RoleName.User);
        //            userRoleName.RoleName = RoleName.User;
        //        }
        //        else
        //        {
        //            await UserManager.AddToRoleAsync(user.Id, RoleName.Admin);
        //            userRoleName.RoleName = RoleName.Admin;
        //        }
        //    }
        //    ctx.SaveChanges();
        //    //var result = await UserManager.CreateAsync(user, model.Password);
        //    //if (result.Succeeded)
        //    //{
        //    //var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
        //    //var roleManger = new RoleManager<IdentityRole>(roleStore);
        //    //await roleManger.CreateAsync(new IdentityRole("Superadmin"));
        //    //await UserManager.AddToRoleAsync(user.Id, RoleName.User);
        //    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
        //    // Send an email with this link
        //    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //    //}
        //    return RedirectToAction("GetUsers", "User");
        //}

        private void AddErrors(IdentityResult result, string email = null)
        {
            var digit = "Passwords must have at least one digit ('0'-'9').";
            var uppercase = "Passwords must have at least one uppercase ('A'-'Z').";
            foreach (var error in result.Errors)
            {
                var errorMessage = error;
                if (errorMessage.StartsWith("Email"))
                {
                    ModelState.AddModelError("Email", String.Format("Email {0} je već zauzet.", email));
                }
                if (errorMessage.Contains(digit) || errorMessage.Contains(uppercase))
                {
                    errorMessage = errorMessage.Replace(digit, "Šifra mora imati barem jednu cifru.");
                    errorMessage = errorMessage.Replace(uppercase, "Šifra mora imati barem jedno veliko slovo.");
                    ModelState.AddModelError("NewPassword", errorMessage);
                }
                ModelState.AddModelError("", errorMessage);
            }
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("GetDonations", "Donations");
        }

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

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}