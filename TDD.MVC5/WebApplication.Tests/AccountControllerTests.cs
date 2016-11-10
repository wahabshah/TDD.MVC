using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using WebApplication.Controllers;
using Moq;
using System.Web.Security;
using ExtensionMethods;
using WebApplication.Helpers;
using WebApplication.Models;

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
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
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
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            // run tests
            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
            var result = ac.Register(username, email, password, securityQuestion, securityAnswer);
           
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
            ac.ViewData.ModelState.AssertErrorMessage("username", "username is required");
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
            ac.ViewData.ModelState.AssertErrorMessage("password", "password is required");
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Question_Is_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(username, email, password, null, securityAnswer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ViewData.ModelState.AssertErrorMessage("securityQuestion", "security question is required");
        }
        [TestMethod]
        public void Register_Should_Return_Error_If_Answer_Is_Null()
        {
            var ac = new AccountController();
            var result = ac.Register(username, email, password, securityQuestion, "");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ViewData.ModelState.AssertErrorMessage("securityAnswer", "security answer is required");
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
            Assert.IsFalse(AppHelperTest.IsValidEmail(invalidEmail), $"Email validation failed for {invalidEmail}");
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
        
        [TestMethod]
        public void Register_ValidEmail_Successfully_Registers_User()
        {
            
            var mockProvider = new Mock<MembershipProvider>();
            MembershipCreateStatus createStatus = MembershipCreateStatus.Success;
            var mockUser = new Mock<MembershipUser>();
            mockProvider.Setup(m => m.CreateUser(username, password, "wahab.shah1986@wiesheu.de", securityQuestion, securityAnswer, true, null, out createStatus))
                        .Returns(() => mockUser.Object);
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
            var result = ac.Register(username, "wahab.shah1986@wiesheu.de", password,  securityQuestion, securityAnswer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            Assert.IsTrue(ac.ViewData.ModelState.IsValid);           
        }
        [TestMethod]
        public void Register_InvalidEmail_Should_Return_Error()
        {
            var mockProvider = new Mock<MembershipProvider>();
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
            var result = ac.Register(username, "wahab.shah@  wiesheu.de", password, securityQuestion, securityAnswer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            Assert.IsTrue(!ac.ViewData.ModelState.IsValid);
            Assert.AreEqual(ac.ViewData.ModelState["email"].Errors[0].ErrorMessage, "email is invalid");
        }
        [DataTestMethod]
        [DataRow("A34")]
        [DataRow("3A")]
        [DataRow("3")]
        [DataRow("      ")]
        public void IsValidUsername_ShortUsernames_Should_Return_False(string invalidUsername)
        {
            Assert.IsFalse(AppHelperTest.IsValidUsername(invalidUsername));
        }
        [DataTestMethod]
        [DataRow("_34")]
        [DataRow("3AAA")]
        [DataRow(" $AA")]
        public void IsValidUsername_Usernames_Starting_With_Non_Alpha_Should_Return_False(string invalidUsername)
        {
            Assert.IsFalse(AppHelperTest.IsValidUsername(invalidUsername));
        }
        [DataTestMethod]
        [DataRow("user!")]
        [DataRow("user@")]
        [DataRow("user#")]
        [DataRow("user$")]
        public void IsValidUsername_Username_With_Invalid_Characters_Should_Return_False(string invalidUsername)
        {
            Assert.IsFalse(AppHelperTest.IsValidUsername(invalidUsername));
        }
        [DataTestMethod]
        [DataRow("user123")]
        [DataRow("us1_")]   
        public void IsValidUsername_Valid_Username_Should_Return_True(string username)
        {
            Assert.IsTrue(AppHelperTest.IsValidUsername(username));
        }
        [TestMethod]
        public void Register_ValidUsername_Should_Allow_Not_Return_Error()
        {
            var mockProvider = new Mock<MembershipProvider>();
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
            var register = ac.Register("wahab_shah19", email, password, securityQuestion, securityAnswer);

            Assert.IsNotNull(register);
            Assert.IsInstanceOfType(register, typeof(ActionResult));
            Assert.IsTrue(ac.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Register_InvalidUsername_Should_Return_Error()
        {
            var mockProvider = new Mock<MembershipProvider>();
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
            var register = ac.Register("1h$", email, password, securityQuestion, securityAnswer);

            Assert.IsNotNull(register);
            Assert.IsInstanceOfType(register, typeof(ActionResult));
            Assert.IsFalse(ac.ViewData.ModelState.IsValid);
            Assert.AreEqual(ac.ViewData.ModelState["username"].Errors[0].ErrorMessage, "username is invalid");
        }
        [DataTestMethod]
        [DataRow(System.Web.Security.MembershipCreateStatus.DuplicateUserName,"username is not unique")]
        [DataRow(System.Web.Security.MembershipCreateStatus.DuplicateEmail, "email is not unique")]
        public void Register_Should_Fail_If_Provider_Create_Fails(System.Web.Security.MembershipCreateStatus createStatus, string error)
        {
            var mockProvider = new Mock<MembershipProvider>();
             MembershipUser mockUser = null; //new Mock<MembershipUser>();
            //CreateUser on error return null
            mockProvider.Setup(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out createStatus))
                .Returns(()=> mockUser);
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object, mockFormsAuthentication.Object);
            var result = ac.Register(username, email, password, securityQuestion, securityAnswer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            ac.ViewData.ModelState.AssertErrorMessage("provider", error);
            mockProvider.VerifyAll();
        }
        [TestMethod]
        public void Register_Should_Call_SetAuthCookie_On_Success()
        {
            var mockProvider = new Mock<MembershipProvider>();
            MembershipCreateStatus createStauts= MembershipCreateStatus.Success;
            var mockUser = new Mock<MembershipUser>();
            mockProvider.Setup(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out createStauts))
                        .Returns(mockUser.Object);
            var mockFormsAuthentication = new Mock<IFormsAuthentication>();
            mockFormsAuthentication.Setup(m => m.SetAuthCookie(username,false));

            var ac = new AccountController(mockProvider.Object,mockFormsAuthentication.Object);
            var result = ac.Register(username, email, password, securityQuestion, securityAnswer);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            mockProvider.VerifyAll();
            mockFormsAuthentication.VerifyAll();              
        }
        [TestMethod]
        public void Register_Invalid_Input_Should_Call_View_With_Same_Model()
        {
            var mockProvider = new Mock<MembershipProvider>();
            MembershipCreateStatus mockCreateStatus = MembershipCreateStatus.DuplicateEmail;
            MembershipUser mockUser = null;//new Mock<MembershipUser>();
            mockProvider.Setup(m => m.CreateUser(username, password, email, securityQuestion, securityAnswer, true, null, out mockCreateStatus))
                        .Returns(mockUser);
            var mockFormsAuth = new Mock<IFormsAuthentication>();
            mockFormsAuth.Setup(m => m.SetAuthCookie(username, false));

            var ac = new AccountController(mockProvider.Object,mockFormsAuth.Object);
            var result = ac.Register(username, email, password, securityQuestion, securityAnswer);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var registerModel = ((ViewResult)result).Model as RegisterModel;
            Assert.IsNotNull(registerModel);
            registerModel.AssertRegisterModel(username,email, securityQuestion, securityAnswer, password);                     
        }
    }
}
