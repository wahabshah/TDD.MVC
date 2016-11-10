using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Helpers
{
    public class FormsAuthenticationWrapper : IFormsAuthentication
    {
        public void SetAuthCookie(string userName, bool createPersistenCookie)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(userName, createPersistenCookie);
        }
    }
}