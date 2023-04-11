using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

using Google.Cloud.Translation.V2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TranslationApp.Models;
using TranslationApp.Utilities;

namespace TranslationApp.Controllers
{
    public class TranslateV2Controller : ApiController
    {
        [HttpPost] /* Translate multible/single item */
        public JObject postTranslateText([FromBody]TranslateV2Request rqu)
        {
            TranslateV2Response rpo = new TranslateV2Response();
            rpo.data = new DataResponse[] { };
            rpo.status = (int)HttpStatusCode.Gone;
            rpo.message = HttpStatusCode.Gone.ToString(); // api is no longer available
            //
            if (!clsAuthentication.Authenticate(rqu.key, rqu.user))
            {
                rpo.status = (int)HttpStatusCode.NonAuthoritativeInformation;
                rpo.message = HttpStatusCode.NonAuthoritativeInformation.ToString();
            }
            else if (rqu.data.Length == 0)
            {
                rpo.status = (int)HttpStatusCode.LengthRequired;
                rpo.message = HttpStatusCode.LengthRequired.ToString();
            }
            else
            {
                bool _valid = false, _exceedLimit = false;
                List<string> _lstRequests = new List<string>();
                foreach (string s in rqu.data)
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
                    foreach (string s in _lstRequests)
                    {
                        DataResponse r = new DataResponse();
                        r.text = s;
                        r.source = (string.IsNullOrEmpty(rqu.source) || string.IsNullOrWhiteSpace(rqu.source)) ? CommonSettings.DefaultLanguagesCode : rqu.source;
                        _lstOutput.Add(r);
                    }
                    rpo.data = _lstOutput.ToArray();
                    rpo.status = (int)HttpStatusCode.OK;
                    rpo.message = HttpStatusCode.NoContent.ToString();
                }
                else
                {
                    int chargeCount = 0;
                    try
                    {
                        List<DataResponse> _lstOutput = new List<DataResponse>();
                        IList<TranslationResult> _results;
                        TranslationClient _client = TranslationClient.CreateFromApiKey(CommonSettings.ApiKey);
                        if (string.IsNullOrEmpty(rqu.source) || string.IsNullOrWhiteSpace(rqu.source)) _results = _client.TranslateText(_lstRequests.ToArray(), rqu.target, sourceLanguage: null);
                        else _results = _client.TranslateText(_lstRequests.ToArray(), rqu.target, sourceLanguage: rqu.source);
                        //
                        for (int i = 0; i < _results.Count; i++)
                        {
                            TranslationResult _item = _results[i];
                            //Console.WriteLine($"Result: {_item.TranslatedText}; detected language {_item.DetectedSourceLanguage}");
                            DataResponse r = new DataResponse();
                            r.text = _item.TranslatedText;
                            r.source = (string.IsNullOrEmpty(rqu.source) || string.IsNullOrWhiteSpace(rqu.source)) ? _item.DetectedSourceLanguage : rqu.source;
                            r.charge = _lstRequests[i].Length;
                            _lstOutput.Add(r);
                            //
                            chargeCount += r.charge;
                        }
                        //
                        rpo.data = _lstOutput.ToArray();
                        rpo.status = (int)HttpStatusCode.OK;
                        rpo.message = HttpStatusCode.OK.ToString()
                            + "-" + CommonSettings.GGNotAvailableMsg
                            + (_exceedLimit ? "-" + CommonSettings.ExceedLimitMsg : string.Empty);
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                        rpo.data = new DataResponse[] { };
                        rpo.status = (int)HttpStatusCode.BadRequest;
                        rpo.message = HttpStatusCode.BadRequest.ToString() + "-" + ex.Message;
                    }
                    try
                    {
                        AgentInfo agent = clsUtilities.GetAgentInfo();
                        PR_Statistics objStatistics = new PR_Statistics();
                        StatisticsStruct item = objStatistics.Create(rqu.user, chargeCount, agent.Ip, agent.UserAgent, agent.Country, agent.Region, agent.City, agent.Isp, string.Empty);
                        objStatistics.Save(item);
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                    }
                }
            }
            string _response = JsonConvert.SerializeObject(rpo);
            return JObject.Parse(_response);
        }

        /*Example
         * request's url example: http://localhost:8083/api/TranslateV2
         * request's body example in Postman--------------------------------
         {
            "data":[
                "   ",
                "Now copy the content and paste here to validate",
                "Android、iOS のスマート言語翻訳ツール。",
                "Услуги Google предоставляются бесплатно"
            ],
            "target":"vi",
            "source":"",
            "key":""
            "user":""
         }
         * 
         * 
         * response's content example-----------------------------------------
         {
            "data": [
                {
                    "text": "",
                    "source": "en",
                    "charge": "0"
                },
                {
                    "text": "Bây giờ hãy sao chép nội dung và dán vào đây để xác thực",
                    "source": "en",
                    "charge": "47"
                },
                {
                    "text": "Trình dịch ngôn ngữ thông minh cho Android và iOS.",
                    "source": "ja",
                    "charge": "25"
                },
                {
                    "text": "Các dịch vụ của Google miễn phí",
                    "source": "ru",
                    "charge": "39"
                }
            ],
            "status": 200,
            "message": "OK-(charge: is counting by web services, GG is not available)"
        }
         */

    }
}
