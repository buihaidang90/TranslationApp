using System;
using TranslationApp.Models;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BHD_Framework;

namespace TranslationApp.Utilities
{
    public class clsUtilities
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


        public static CompactAgentInfo GetCompactAgentInfo()
        {
            CompactAgentInfo agent = new CompactAgentInfo();
            agent.Ip = "";
            agent.UserAgent = "";

            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //ip = HttpContext.Current.Request.UserHostAddress;
            string userAgent = "";
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                //ip = HttpContext.Current.Request.ServerVariables["ALL_RAW"];
                //ip = HttpContext.Current.Request.ServerVariables["REQUEST_METHOD"];
                //ip = HttpContext.Current.Request.ServerVariables["REMOTE_USER"];
                userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            }
            agent.Ip = ip;
            agent.UserAgent = userAgent;
            return agent;
        }
        public static AgentInfo GetAgentInfo()
        {
            CompactAgentInfo compactAgent = GetCompactAgentInfo();
            AgentInfo agent = new AgentInfo();
            agent.Ip = "";
            agent.UserAgent = "";
            agent.RequestTime = "";
            agent.Country = "";
            agent.Region = "";
            agent.City = "";
            agent.Isp = "";
            if (!Network.IsIpAddress(compactAgent.Ip)) return agent;
            Network.LocationDetails geoInfo = Network.GetLocationDetail(compactAgent.Ip);
            agent.Ip = compactAgent.Ip;
            agent.UserAgent = compactAgent.UserAgent;
            agent.RequestTime = DateTime.Now.ToString();
            agent.Country = geoInfo.country;
            agent.Region = geoInfo.regionName;
            agent.City = geoInfo.city;
            agent.Isp = string.Concat(geoInfo.isp, " - ", geoInfo.org);
            return agent;
        }


        public static JObject GetJObjectMessage(string Msg)
        {
            if (Msg == null) Msg = "";
            string str = "{ \"message\": \"" + Msg + "\"}";
            JObject json = JObject.Parse(str);
            return json;
        }
        public static JObject GetJObjectMessage(object Obj)
        {
            string _response = JsonConvert.SerializeObject(Obj);
            return JObject.Parse(_response);
        }












    }

}