using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using WebApplication.Controllers;
using Moq;
using System.Web.Security;
using ExtensionMethods;

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
            var mockProvider = new Mock<System.Web.Security.MembershipProvider>();
            System.Web.Security.MembershipCreateStatus status;
            mockProvider.Setup(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out status))
                        .Returns(new Mock<System.Web.Security.MembershipUser>().Object);

            var ac = new AccountController(mockProvider.Object);
            var result = ac.Register(username, email, password, securityQuestion, securityAnswer);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            object controllerName = "";
            ((RedirectToRouteResult)result).RouteValues.TryGetValue("controller", out controllerName);
            Assert.AreEqual(controllerName, "Home");
            object actionName = "";
            ((RedirectToRouteResult)result).RouteValues.TryGetValue("action", out actionName);
            Assert.AreEqual(actionName, "Index");

        }
        [TestMethod]
        public void Register_Can_Create_User_Successfully()
        {
            //create mocks and set expectations
            var mockProvider = new Mock<MembershipProvider>();
            MembershipCreateStatus state;
            var user = new Mock<MembershipUser>();
            mockProvider.Setup(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out state))
                .Returns(user.Object);

            // run tests
            var ac = new AccountController(mockProvider.Object);
            var result = ac.Register(username, email, password, securityQuestion, securityAnswer);
            //mockProvider.Verify(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out state));
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            object actionName;
            ((RedirectToRouteResult)result).RouteValues.TryGetValue("action", out actionName);
            Assert.AreEqual("Index", actionName);
            object controllerName;
            ((RedirectToRouteResult)result).RouteValues.TryGetValue("controller", out controllerName);
            Assert.AreEqual("Home", controllerName);

            mockProvider.Verify(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out state), Times.AtLeastOnce());
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Username_Is_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(string.Empty, email, password, securityQuestion, securityAnswer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ModelState.AssertErrorMessage("username", "username is required");
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Email_Is_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(username, null, password, securityQuestion, securityAnswer);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ViewData.ModelState.AssertErrorMessage("email", "email is required");
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Password_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(username, email, "", securityQuestion, securityAnswer);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ModelState.AssertErrorMessage("password", "password is required");
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Question_Is_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(username, email, password, null, securityAnswer);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ModelState.AssertErrorMessage("securityQuestion", "security question is required");
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Answer_Is_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(username, email, password, securityQuestion, "");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ModelState.AssertErrorMessage("securityAnswer", "security answer is required");
        }
        [DataTestMethod]
        [DataRow("sdfsdf dsfsdf")]
        [DataRow("text@test_test.com")]
        [DataRow("sdfdf@.com")]
        [DataRow("sdfdf@dfdfdf")]
        [DataRow("sdfdf@fdfd. com")]
        [DataRow("s&#dfdf@fdfd.com")]
        [DataRow("sd@fdf@fdfd.com")]
        public void IsValidEmail_Invalid_Emails_Should_Return_False(string invalidEmail)
        {
            Assert.IsFalse(AppHelper.IsValidEmail(invalidEmail), $"Email validation failed for {invalidEmail}");
        }
        [DataTestMethod]
        [DataRow("test@test.com")]
        [DataRow("123@test.com")]
        [DataRow("first_last@test.com")]
        [DataRow("international@test.com.eg")]
        [DataRow("someorg@test.org")]
        [DataRow("somenet@test.net")]
        public void IsValidEmail_Valid_Emails_Should_Return_True(string validEmail)
        {
            Assert.IsTrue(AppHelperTest.IsValidEmail(validEmail), $"Email validation failed for {validEmail}");
        }
        [DataTestMethod]
        [DataRow("A34")]
        [DataRow("3AAA")]
        [DataRow("A$AA")]
        public void IsValidUsername_Invalid_Usernames_Should_Return_False(string invalidUsername)
        {
            Assert.IsFalse(AppHelperTest.IsValidUsername(invalidUsername), $"Username validation failed for {invalidUsername}");
        }
       
    }
}
