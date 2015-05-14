using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Foody.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Net;

namespace Foody.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: /Account/
        public ActionResult Index()
        {
            return View(db.AccountList.ToList());
        }

        // GET: /Account/Edit/
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.AccountList.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: /Account2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                account.SetUpdatedInfo(User.Identity.Name);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                }
                return RedirectToAction("Index");
            }
            return View(account);
        }
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid /*&& WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe)*/)
            {
                string passModel = Account.Encrypt(model.Password);
                Account account = db.AccountList.FirstOrDefault(i => i.UserName == model.UserName && i.Password == passModel);
                if (account != null)
                {
                    if (account.Status == "I")
                    {
                        ModelState.AddModelError("", "The user has been inactivated.");
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                }
            }

            // If we got this far, something failed, redisplay form
            if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel register)
        {
            Account account = new Account();
            account.UserName = register.UserName;
            account.Password = Account.Encrypt(register.Password);
            account.Status = "A";
            if (User.Identity.Name != null)
            {
                account.CreatedBy = User.Identity.Name;
                account.LastUpdatedBy = User.Identity.Name;
            }
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    if ((db.AccountList.Count(i => i.UserName.Trim() == account.UserName.Trim()) > 0) || db.AccountList.Count(i => i.UserName.Trim() == account.UserName.Trim()) > 0)
                    {
                        if ((db.AccountList.Count(i => i.UserName.Trim() == account.UserName.Trim()) > 0))
                        { ModelState.AddModelError("", "The user name already exists. Please enter a different user name."); }

                        if (db.AccountList.Count(i => i.Email.Trim() == account.Email.Trim()) > 0)
                        {
                            ModelState.AddModelError("", "The email already exists. Please enter a different email.");
                        }
                    }
                    else
                    {
                        db.AccountList.Add(account);
                        db.SaveChanges();
                        if (String.IsNullOrEmpty(User.Identity.Name))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Account");
                        }
                    }

                }
                catch
                {
                    ModelState.AddModelError("", "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(register);
        }

        // POST: /Account/LogOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(string returnUrl)
        {
            FormsAuthentication.SignOut();
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel { UserName = User.Identity.Name };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            Account account = db.AccountList.Find(model.UserName);
            if (account == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {

                if (account.Password == Account.Encrypt(model.OldPassword))
                {
                    if (account.Password == Account.Encrypt(model.NewPassword))
                    {
                        ModelState.AddModelError("", "New password and current password must be different.");
                    }
                    else
                    {
                        account.Password = Account.Encrypt(model.NewPassword);
                        account.SetUpdatedInfo(User.Identity.Name);

                        //Save changes
                        db.Entry(account).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    {
                        ModelState.AddModelError("", "Current password is incorrect.");
                    }
                }

            }

            return View(model);
        }
        //private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        //{
        //    // See http://go.microsoft.com/fwlink/?LinkID=177550 for
        //    // a full list of status codes.
        //    switch (createStatus)
        //    {
        //        case MembershipCreateStatus.DuplicateUserName:
        //            return "User name already exists. Please enter a different user name.";

        //        case MembershipCreateStatus.DuplicateEmail:
        //            return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

        //        case MembershipCreateStatus.InvalidPassword:
        //            return "The password provided is invalid. Please enter a valid password value.";

        //        case MembershipCreateStatus.InvalidEmail:
        //            return "The e-mail address provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidAnswer:
        //            return "The password retrieval answer provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidQuestion:
        //            return "The password retrieval question provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.InvalidUserName:
        //            return "The user name provided is invalid. Please check the value and try again.";

        //        case MembershipCreateStatus.ProviderError:
        //            return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        case MembershipCreateStatus.UserRejected:
        //            return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

        //        default:
        //            return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
        //    }
        //}
    }
}