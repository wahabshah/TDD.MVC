using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        System.Web.Security.MembershipProvider _provider;
        public AccountController() :this(null){ }
        public AccountController(System.Web.Security.MembershipProvider provider)
        {
            _provider = provider?? System.Web.Security.Membership.Provider;
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
                ModelState.AddModelError("username", "username is required");
            else if (!AppHelper.IsValidUsername(username))
                ModelState.AddModelError("username", "username is invalid");
            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError("email", "email is required");
            else if (!AppHelper.IsValidEmail(email))
                ModelState.AddModelError("email", "email is invalid");
            if (string.IsNullOrEmpty(password))
                ModelState.AddModelError("password", "password is required");
            if (string.IsNullOrEmpty(securityquestion))
                ModelState.AddModelError("securityQuestion", "security question is required");
            if (string.IsNullOrEmpty(securityanswer))
                ModelState.AddModelError("securityAnswer", "security answer is required");

            if (!ModelState.IsValid)
            {
                return View();
            }
            System.Web.Security.MembershipCreateStatus status;
           var newUser = _provider.CreateUser(username,  password, email,securityquestion, securityanswer, true, null, out status);

            if(newUser != null)
                return RedirectToAction("Index", "Home");

            if (status == System.Web.Security.MembershipCreateStatus.DuplicateUserName)
                ModelState.AddModelError("provider", "username is not unique");
            if (status == System.Web.Security.MembershipCreateStatus.DuplicateEmail)
                ModelState.AddModelError("provider", "email is not unique");


             return View();
        }
    }
}