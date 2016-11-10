using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class RegisterModel
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string securityQuestion { get; set; }
        public string securityAnswer { get; set; }
    }
}