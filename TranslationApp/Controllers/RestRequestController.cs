using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Google.Cloud.Translation.V2;
using TranslationApp.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TranslationApp.Controllers
{
    public class RestRequestController : ApiController
    {
        // No used
        [HttpPost]
        public string postTranslateText([FromBody]RestClient p)
        {
            //string _url = @"https://translation.googleapis.com/language/translate/v2?key=" + CommonSettings.ApiKey;
            //RestServer r = new RestServer();
            //r.format = "text";
            //r.q = p.text;
            //r.source = p.source;
            //r.target = p.target;

            string _translatedText = "";
            //using (HttpClient _client = new HttpClient())
            //{
            //    string _json = JsonConvert.SerializeObject(r);
            //    var _content = new StringContent(_json, Encoding.UTF8, "application/json");
            //    HttpResponseMessage _reponse = _client.PostAsync(_url, _content).Result;

            //    if (_reponse.StatusCode == HttpStatusCode.OK)
            //    {
            //        string _result = _reponse.Content.ReadAsStringAsync().Result;
            //        List<object> jsonData = JsonConvert.DeserializeObject<List<object>>(_result.ToString());
            //        JArray ArrJson = JArray.Parse(jsonData[0].ToString()); // gọi web service để test chỗ này
            //        JArray ArrJson2 = (JArray)(ArrJson[0]);
            //        _translatedText = ArrJson2.First.ToString();
            //        //Console.WriteLine(TranslatedText);
            //    }
            //    else
            //    {
            //        _translatedText = "";
            //    }
            //}
            return _translatedText;
        }





    }
}
