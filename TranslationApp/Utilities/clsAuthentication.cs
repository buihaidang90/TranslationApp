using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranslationApp.Models;

namespace TranslationApp.Utilities
{
    public class clsAuthentication
    {
        public static readonly string DefaultUser = "haidang@mankichi.net";
        public static bool Authenticate(object YourKey, string YourUser)
        {
            string webConfig = System.Configuration.ConfigurationManager.AppSettings[CommonSettings.StringMode];
            string[] arr = new string[] { "1", "yes", "true" };
            bool isDevMode = Array.IndexOf(arr, webConfig.ToLower()) > -1;
            //
            if (YourKey == null) return false;
            if (string.IsNullOrEmpty(YourKey.ToString()) || string.IsNullOrWhiteSpace(YourKey.ToString())) return false;
            if (string.IsNullOrEmpty(YourUser) || string.IsNullOrWhiteSpace(YourUser)) return false;
            if (isDevMode && YourKey.ToString() == string.Concat(DefaultUser, DateTime.UtcNow.ToString("yyyyMMdd"))) return true;
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