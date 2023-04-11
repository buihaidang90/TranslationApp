using BHD_Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Http;
using TranslationApp.Utilities;
using TranslationApp.Models;

namespace TranslationApp.Controllers
{
    public class TestController : ApiController // only for test connection from client
    {
        [HttpGet]
        public string getState() { return "Application is ready."; }


        [HttpGet]
        public JObject getState([FromUri]string r)
        {
            try
            {
                if (r == null) r = "";
                if (r == "")
                {
                    return clsUtilities.GetJObjectMessage("Application is ready (with empty param).");
                }
                else if (r.ToLower() == "ConnectSql".ToLower() || r.ToLower() == "SqlConnect".ToLower())
                {
                    SqlObject sql = new SqlObject(clsUtilities.GetConnectionString());
                    if (sql.GetConnectionState())
                        return clsUtilities.GetJObjectMessage("State: connected - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                    else
                        return clsUtilities.GetJObjectMessage("State: can not connect - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                }
                //else if (r.ToLower() == "print".ToLower())
                //{
                //    SqlObject sql = new SqlObject(Ultility.Ultility.GetConnectionString());
                //    //sql.ConnectionTimeout = 8;
                //    if (sql.GetConnectionState())
                //    {
                //        return clsUltility.GetJObjectMessage("State: connected - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                //    }
                //    else
                //        return clsUltility.GetJObjectMessage("State: can not connect - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                //}
                else if (r.ToLower() == "MyInfo".ToLower())
                {
                    AgentInfo agent =clsUtilities.GetAgentInfo();
                    if (agent.Ip=="") return clsUtilities.GetJObjectMessage("Can not get your info!");
                    return clsUtilities.GetJObjectMessage(agent);
                }
                else
                {
                    return clsUtilities.GetJObjectMessage("Your value is [" + r + "]");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return clsUtilities.GetJObjectMessage("");
            }
        }


    }
}
