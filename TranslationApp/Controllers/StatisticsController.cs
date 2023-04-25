using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TranslationApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TranslationApp.Utilities;
using System.Globalization;

namespace TranslationApp.Controllers
{
    public class StatisticsController : ApiController
    {
        [HttpPost] /* Translate multible/single item */
        public JObject postLoadData([FromBody]StatisticsRequest rqu)
        {
            StatisticsResponse rpo = new StatisticsResponse();
            rpo.data = new PR_Statistics.StatisticsRecord[] { };
            if (!clsAuthentication.Authenticate(rqu.key, rqu.user))
            {
                rpo.status = (int)HttpStatusCode.NonAuthoritativeInformation;
                rpo.message = HttpStatusCode.NonAuthoritativeInformation.ToString();
            }
            else if (string.IsNullOrEmpty(rqu.FDate) || string.IsNullOrWhiteSpace(rqu.FDate))
            {
                rpo.status = (int)HttpStatusCode.NotAcceptable;
                rpo.message = HttpStatusCode.NotAcceptable.ToString();
            }
            DateTime FDate, TDate;
            if (!DateTime.TryParseExact(rqu.FDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out FDate) ||
                !DateTime.TryParseExact(rqu.TDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out TDate))
            {
                rpo.status = (int)HttpStatusCode.BadRequest;
                rpo.message = HttpStatusCode.BadRequest.ToString();
            }
            else
            {
                string Cust = (string.IsNullOrEmpty(rqu.Customer) || string.IsNullOrWhiteSpace(rqu.Customer) ? "" : rqu.Customer);
                PR_Statistics statistics = new PR_Statistics();
                List<PR_Statistics.StatisticsRecord> lst = statistics.loadStatisticsRecord(FDate, TDate, Cust);
                rpo.data = lst.ToArray();
                rpo.status = (int)HttpStatusCode.OK;
                rpo.message = HttpStatusCode.NoContent.ToString();
            }
            string _response = JsonConvert.SerializeObject(rpo);
            return JObject.Parse(_response);
        }

    }
}
