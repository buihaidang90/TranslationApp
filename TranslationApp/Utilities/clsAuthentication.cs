using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranslationApp.Utilities
{
    public class clsAuthentication
    {
        public static readonly string DefaultUser = "buihaidang90@gmail.com";
        public static bool Authenticate(object YourKey, string YourUser)
        {
            if (YourKey == null) return false;
            if (YourKey.ToString() == "") return false;
            if (YourUser == null) return false;
            if (YourUser == "") return false;
            return true;
        }
    }






}