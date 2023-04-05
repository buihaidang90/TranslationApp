using System;
using TranslationApp.Models;
using BHD_Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace TranslationApp.Ultility
{
    public class clsUltility
    {
        public static string GetConnectionString()
        {
            string _result = "";
            string _webConfig = "";
            try
            {
                _webConfig = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
                string[] uidText = new string[] { "user id" };
                string[] pwdText = new string[] { "pwd", "password" };
                string[] arrItems = _webConfig.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> lstItems = new List<string>();
                foreach (string item in arrItems)
                {
                    string str = item;
                    bool assigned = false;
                    if (!assigned) foreach (string s in uidText)
                        {
                            string _value = Utility.GetValueOf(s, item);
                            if (_value == "") continue;
                            str = string.Concat(s, "=", Cipher.ToggleMankichi(_value));
                            assigned = true;
                            break;
                        }
                    if (!assigned) foreach (string s in pwdText)
                        {
                            string _value = Utility.GetValueOf(s, item);
                            if (_value == "") continue;
                            str = string.Concat(s, "=", Cipher.ToggleMankichi(_value));
                            assigned = true;
                        }
                    lstItems.Add(str.Trim());
                }
                foreach (string item in lstItems) _result += string.Concat(item, ";");
            }
            catch { _result = _webConfig; }
            return _result;
        }

        public static string GetApiKey()
        {
            string _webConfig = System.Configuration.ConfigurationManager.AppSettings["API_key"];
            string _result = Cipher.Decrypt(_webConfig, CommonSettings.CompanyName);
            return _result;
        }
    }







}