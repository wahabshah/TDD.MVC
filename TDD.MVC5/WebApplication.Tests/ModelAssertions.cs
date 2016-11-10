using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Tests
{
    public static class ModelAssertions
    {
        public static void AssertRegisterModel(this RegisterModel model, string username, string email,string question, string answer, string password)
        {
            Assert.AreEqual(model.username, username);
            Assert.AreEqual(model.email, email);
            Assert.AreEqual(model.password, password);
            Assert.AreEqual(model.securityQuestion, question);
            Assert.AreEqual(model.securityAnswer, answer);
        }
    }
}
