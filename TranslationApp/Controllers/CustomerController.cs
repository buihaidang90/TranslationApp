using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using TranslationApp.Models;
using TranslationApp.Utilities;

namespace TranslationApp.Controllers
{
    public class CustomerController : ApiController
    {
        [HttpPost] /* Translate multible/single item */
        public JObject postTranslateText([FromBody]CustomerRequest rqu)
        {
            CustomerResponse rpo = new CustomerResponse();
            rpo.status = (int)HttpStatusCode.Gone;
            rpo.message = HttpStatusCode.Gone.ToString(); // api is no longer available
            bool saveSuccess = false;
            if (!clsAuthentication.Authenticate(rqu.key, rqu.user))
            {
                rpo.status = (int)HttpStatusCode.NonAuthoritativeInformation;
                rpo.message = HttpStatusCode.NonAuthoritativeInformation.ToString();
            }
            if (rqu.data.Length == 0)
            {
                rpo.status = (int)HttpStatusCode.LengthRequired;
                rpo.message = HttpStatusCode.LengthRequired.ToString();
            }
            else
            {
                CompactAgentInfo agent = clsUtilities.GetCompactAgentInfo();
                List<CustomerStruct> lstCust = new List<CustomerStruct>();
                foreach (CustomerStruct cust in rqu.data)
                {
                    if (cust.Code == "") continue;
                    CustomerStruct c = new CustomerStruct();
                    c.Code = cust.Code;
                    c.Name = (cust.Name == null ? string.Empty : cust.Name);
                    c.Address = (cust.Address == null ? string.Empty : cust.Address);
                    c.TaxCode = (cust.TaxCode == null ? string.Empty : cust.TaxCode);
                    c.Phone = (cust.Phone == null ? string.Empty : cust.Phone);
                    c.Remark = (cust.Remark == null ? agent.UserAgent : cust.Remark);
                    lstCust.Add(c);
                }
                PR_Customer clsCustomer = new PR_Customer();
                saveSuccess = clsCustomer.Save(lstCust, 0);
                if (saveSuccess)
                {
                    rpo.status = (int)HttpStatusCode.OK;
                    rpo.message = HttpStatusCode.OK.ToString();
                }
                else
                {
                    rpo.status = (int)HttpStatusCode.NotModified;
                    rpo.message = HttpStatusCode.NotModified.ToString();
                }
            }
            string _response = JsonConvert.SerializeObject(rpo);
            return JObject.Parse(_response);
        }

    }

}
