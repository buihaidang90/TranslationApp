using BHD_Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using TranslationApp.Ultility;

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
                    return getJObjectMessage("Application is ready (with empty param).");
                }
                else if (r.ToLower() == "ConnectSql".ToLower() || r.ToLower() == "SqlConnect".ToLower())
                {
                    SqlObject sql = new SqlObject(clsUltility.GetConnectionString());
                    if (sql.GetConnectionState())
                        return getJObjectMessage("State: connected - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                    else
                        return getJObjectMessage("State: can not connect - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                }
                //else if (r.ToLower() == "print".ToLower())
                //{
                //    SqlObject sql = new SqlObject(Ultility.Ultility.GetConnectionString());
                //    //sql.ConnectionTimeout = 8;
                //    if (sql.GetConnectionState())
                //    {
                //        return getJObjectMessage("State: connected - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                //    }
                //    else
                //        return getJObjectMessage("State: can not connect - Timeout: " + sql.ConnectionTimeout.ToString() + " second");
                //}
                else
                {
                    return getJObjectMessage("Your value is [" + r + "]");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                return getJObjectMessage("");
            }
        }

        private JObject getJObjectMessage(string Msg)
        {
            if (Msg == null) Msg = "";
            string str = "{ \"message\": \"" + Msg + "\"}";
            JObject json = JObject.Parse(str);
            return json;
        }



    }
}
