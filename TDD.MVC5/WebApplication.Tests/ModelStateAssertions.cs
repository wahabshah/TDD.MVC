using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ExtensionMethods
{
    public static class ModelStateAssertions
    {
        public static void AssertErrorMessage(this ModelStateDictionary modelState, string key, string errorMessage)
        {
            Assert.IsFalse(modelState.IsValid);
            Assert.IsTrue(modelState.ContainsKey(key));
            Assert.AreEqual(modelState[key].Errors[0].ErrorMessage, errorMessage);
        }
    }
}