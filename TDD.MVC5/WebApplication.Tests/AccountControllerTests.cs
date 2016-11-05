using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using WebApplication.Controllers;
using System.Collections.Generic;
using System.Diagnostics;

namespace WebApplication.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        string username = "wahabshah";
        string email = "wahab.shah@wiesheu.de";
        string password = "hallo";
        string securityQuestion = "what is your mother maiden name?";
        string securityAnswer = "abida";


        [TestMethod]
        public void Register_Can_Get_To_Register_View()
        {
            var ac = new AccountController();
            var result = ac.Register();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual("Register", ac.ViewData["Title"]);
            
        }
        [TestMethod]
        public void Register_Redirects_To_Home_Index_On_Success()
        {
            var ac = new AccountController();
            var result = ac.Register(username,email,password,securityQuestion,securityAnswer);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            object controllerName = "";
            ((RedirectToRouteResult)result).RouteValues.TryGetValue("controller", out controllerName);
            Assert.AreEqual(controllerName, "Home");
            object actionName = "";
            ((RedirectToRouteResult)result).RouteValues.TryGetValue("action", out actionName);
            Assert.AreEqual(actionName, "Index");
        }
       // public void Register_Can_Create_User_Successfully()
        //{
        //    var ac = new AccountController();
        //    ac.Register(username, email, password, securityQuestion, securityAnswer);
        //   // Moq.Mock<System.Web.Security.Membership.Provider>()

        //}
    }
}
