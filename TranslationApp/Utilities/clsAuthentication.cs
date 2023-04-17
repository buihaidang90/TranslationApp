using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranslationApp.Utilities
{
    public class clsAuthentication
    {
        public static readonly string DefaultUser = "haidang@mankichi.net";
        public static bool Authenticate(object YourKey, string YourUser)
        {
            if (YourKey == null) return false;
            if (YourKey.ToString() == "") return false;
            if (YourUser == null) return false;
            if (YourUser == "") return false;
            return YourKey.ToString() == md5Checksum(string.Concat(YourUser, DateTime.UtcNow.ToString("yyyyMMdd")));
        }
        private static string md5Checksum(string YourData)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] arrInput = System.Text.Encoding.ASCII.GetBytes(YourData);
            byte[] arrHash = md5.ComputeHash(arrInput);
            return BitConverter.ToString(arrHash).Replace("-", string.Empty).ToUpper();
        }

    }






}