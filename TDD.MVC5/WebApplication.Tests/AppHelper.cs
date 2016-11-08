﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApplication.Tests
{
    public class AppHelperTest
    {
        public static bool IsValidEmail(string Email)
        {
            var pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|" +
                          @"(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var regex = new Regex(pattern,RegexOptions.Compiled|RegexOptions.Singleline);
            return regex.IsMatch(Email);         
        }
        public static bool IsValidUsername(string Username)
        {
            var pattern = @"/\w{4}/g";
            var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);
            return regex.IsMatch(Username);
        }
    }
}
