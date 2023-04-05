using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

using Google.Cloud.Translation.V2;
using TranslationApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TranslationApp.Controllers
{
    public class SqlTranslateV2Controller : ApiController
    {
        /*
         * first record is alway status and message of translate process
         */
        [HttpPost] /* Translate request from sql */
        public JObject postTranslateText([FromBody]string[] data, [FromUri]string key, [FromUri]string target, [FromUri]string source)
        {
            //"},{"
            if (key == null) key = string.Empty;
            target = target.ToString();
            if (source == null) source = string.Empty;
            SqlTranslateV2Response rpo = new SqlTranslateV2Response();
            rpo.data = new DataResponse[] { };
            //message = HttpStatusCode.Gone.ToString(); // api is no longer available
            //
            if (data == null)
            {
                DataResponse _dr = new DataResponse();
                _dr.text = raiseMessage(HttpStatusCode.LengthRequired);// first record
                rpo.data = new DataResponse[] { _dr };
            }
            if (data.Length == 0)
            {
                DataResponse _dr = new DataResponse();
                _dr.text = raiseMessage(HttpStatusCode.LengthRequired);// first record
                rpo.data = new DataResponse[] { _dr };
            }
            else
            {
                bool _valid = false, _exceedLimit = false;
                List<string> _lstRequests = new List<string>();
                foreach (string s in data)
                {
                    if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                        _lstRequests.Add(string.Empty);
                    else if (s.Length > CommonSettings.MaxCharacterNum)
                    {
                        _lstRequests.Add(string.Empty); // ignore string that over maximum limit
                        _exceedLimit = true;
                    }
                    else
                    {
                        _lstRequests.Add(s);
                        _valid = true;
                    }
                }
                if (!_valid) // All of requests array are null or empty
                {
                    List<DataResponse> _lstOutput = new List<DataResponse>();
                    DataResponse _dr = new DataResponse();
                    _dr.text = raiseMessage(HttpStatusCode.NoContent);// first record
                    _lstOutput.Add(_dr);
                    foreach (string s in _lstRequests)
                    {
                        DataResponse r = new DataResponse();
                        r.text = s;
                        r.source = (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source)) ? CommonSettings.DefaultLanguagesCode : source;
                        _lstOutput.Add(r);
                    }
                    rpo.data = _lstOutput.ToArray();
                }
                else
                {
                    try
                    {
                        List<DataResponse> _lstOutput = new List<DataResponse>();
                        DataResponse _dr = new DataResponse();
                        _dr.text = raiseMessage(HttpStatusCode.OK,
                            CommonSettings.GGNotAvailableMsg +
                            (_exceedLimit ? "-" + CommonSettings.ExceedLimitMsg : string.Empty)
                            );// first record
                        _lstOutput.Add(_dr);
                        //
                        IList<TranslationResult> _results;
                        TranslationClient _client = TranslationClient.CreateFromApiKey(CommonSettings.ApiKey);
                        if (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source)) _results = _client.TranslateText(_lstRequests.ToArray(), target, sourceLanguage: null);
                        else _results = _client.TranslateText(_lstRequests.ToArray(), target, sourceLanguage: source);
                        //
                        for (int i = 0; i < _results.Count; i++)
                        {
                            TranslationResult _item = _results[i];
                            //Console.WriteLine($"Result: {_item.TranslatedText}; detected language {_item.DetectedSourceLanguage}");
                            DataResponse r = new DataResponse();
                            r.text = _item.TranslatedText;
                            r.source = (string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source)) ? _item.DetectedSourceLanguage : source;
                            r.charge = _lstRequests[i].Length;
                            _lstOutput.Add(r);
                        }
                        //
                        rpo.data = _lstOutput.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        DataResponse _dr = new DataResponse();
                        _dr.text = raiseMessage(HttpStatusCode.BadRequest, ex.Message);// first record
                        rpo.data = new DataResponse[] { _dr };
                    }
                }
            }
            string _response = JsonConvert.SerializeObject(rpo);
            return JObject.Parse(_response);
        }

        private string raiseMessage(HttpStatusCode statusCode, string additionMgs = null)
        {
            if (additionMgs == null)
                additionMgs = "";
            return
            string.Format(@"{0}-{1}{2}",
                (int)statusCode,
                statusCode.ToString(),
                additionMgs.ToString().Trim() == "" ? string.Empty : ("-" + additionMgs)
                );
        }

        /*Example
         * request's url example: http://localhost:8083/api/SqlTranslateV2?key=&source=&target=vi
         * request's body example in Postman--------------------------------
         [
            "How to Pass Multiple Parameters to a GET Method?",
            "Existem mais maneiras de passar vários parâmetros para o método",
            "ဤရွေးချယ်မှုတွင်၊ လက်ရှိတောင်းဆိုမှုမှ လမ်းကြောင်းဒေတာကို အသုံးပြု၍ ဘောင်များကို ကန့်သတ်ထားသည်။"
        ]
         * 
         * 
         * response's content example-----------------------------------------
        {
            "data": [
                {
                    "text": "200-OK-(charge: is counting by web services, GG is not available)",
                    "source": null,
                    "charge": 0
                },
                {
                    "text": "Làm cách nào để truyền nhiều tham số cho phương thức GET?",
                    "source": "en",
                    "charge": "48"
                },
                {
                    "text": "Có nhiều cách hơn để truyền nhiều tham số cho phương thức",
                    "source": "pt",
                    "charge": "63"
                },
                {
                    "text": "Trong tùy chọn này, Giới hạn các tham số bằng cách sử dụng dữ liệu tuyến đường từ yêu cầu hiện tại.",
                    "source": "my",
                    "charge": "95"
                }
            ]
        }
         */
















    }
}
