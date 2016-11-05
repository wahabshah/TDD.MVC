using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        [AcceptVerbs("GET")]
        // GET: Account
        public ActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }
        [AcceptVerbs("POST")]
        public ActionResult Register(string username, string email,string password, string securityquestion,string answer)
        {
            return RedirectToAction("Index","Home");
        }

    }
}