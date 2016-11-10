using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Helpers
{
    public interface IFormsAuthentication
    {
         void SetAuthCookie(string userName, bool createPersistenCookie);
    }
}