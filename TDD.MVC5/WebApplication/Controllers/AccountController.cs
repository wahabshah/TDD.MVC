using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication.Helpers;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        System.Web.Security.MembershipProvider _provider;
        IFormsAuthentication _formsAuth;
        public AccountController() :this(null,null){ }
        public AccountController(System.Web.Security.MembershipProvider provider,IFormsAuthentication formsAuth)
        {
            _provider = provider?? System.Web.Security.Membership.Provider;
            _formsAuth = formsAuth ?? new FormsAuthenticationWrapper();
        }
        [AcceptVerbs("GET")]
        // GET: Account
        public ActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }
        [AcceptVerbs("POST")]
        public ActionResult Register(string username, string email,string password, string securityquestion,string securityanswer)
        {
            if (string.IsNullOrEmpty(username))
                ViewData.ModelState.AddModelError("username", "username is required");
            else if (!AppHelper.IsValidUsername(username))
                ViewData.ModelState.AddModelError("username", "username is invalid");
            if (string.IsNullOrEmpty(email))
                ViewData.ModelState.AddModelError("email", "email is required");
            else if (!AppHelper.IsValidEmail(email))
                ViewData.ModelState.AddModelError("email", "email is invalid");
            if (string.IsNullOrEmpty(password))
                ViewData.ModelState.AddModelError("password", "password is required");
            if (string.IsNullOrEmpty(securityquestion))
                ViewData.ModelState.AddModelError("securityQuestion", "security question is required");
            if (string.IsNullOrEmpty(securityanswer))
                ViewData.ModelState.AddModelError("securityAnswer", "security answer is required");

            if (!ViewData.ModelState.IsValid)
            {
                return View(new RegisterModel() {email=email,password=password,username=username,securityQuestion=securityquestion, securityAnswer=securityanswer });
            }
            System.Web.Security.MembershipCreateStatus status;
           var newUser = _provider.CreateUser(username,  password, email,securityquestion, securityanswer, true, null, out status);

            if (newUser != null)
            {
                _formsAuth.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Home");
            }

            if (status == System.Web.Security.MembershipCreateStatus.DuplicateUserName)
                ViewData.ModelState.AddModelError("provider", "username is not unique");
            if (status == System.Web.Security.MembershipCreateStatus.DuplicateEmail)
                ViewData.ModelState.AddModelError("provider", "email is not unique");

            return View(new RegisterModel() { email = email, password = password, username = username, securityQuestion = securityquestion, securityAnswer = securityanswer });
        }

        [AcceptVerbs("GET")]
        public ActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [AcceptVerbs("POST")]
        public ActionResult Login(string username, string password,bool bRememberPassword)
        {
            if (string.IsNullOrEmpty(username))
                ViewData.ModelState.AddModelError("username", "username is required");
            if (string.IsNullOrEmpty(password))
                ViewData.ModelState.AddModelError("password", "password is required");

            if (!ViewData.ModelState.IsValid)
                return View();

            var user = _provider.GetUser(username, bRememberPassword);

            if (user == null)
            {
                ViewData.ModelState.AddModelError("provider", "username or password is incorrect");
                return View();
            }

            
            return RedirectToAction("Index","Home");
        }
    }
}