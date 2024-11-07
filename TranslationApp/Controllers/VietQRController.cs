using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TranslationApp.Models;

namespace TranslationApp.Controllers
{
    public class VietQRController : ApiController
    {
        string urlListBank = @"https://api.vietqr.io/v2/banks";
        string urlLookupAccOwner = @"https://api.vietqr.io/v2/lookup";
        string urlVietQR = @"https://img.vietqr.io/image/{0}-{1}-{2}.png?amount={3}&addInfo={4}&accountName={5}";
        string[] arr = new string[] { "qr_only", "compact", "compact2", "print" };

        //[HttpGet]
        //public string getTest() { return "VietQR api"; }

        //[HttpGet] // Cách 1
        //public HttpResponseMessage createQRCode([FromUri] string BankId, [FromUri] string AccountNo, string AccountOwner = null, string Amount = null, string Description = null, string Template = null)
        //{
        //    if (AccountOwner == null) AccountOwner = "";
        //    if (Amount == null) Amount = "";
        //    if (Description == null) Description = "";
        //    if (Template == null) Template = arr[arr.Length - 1]; //print
        //    /*
        //     * qr_only	480x480	Trả về ảnh QR đơn giản, chỉ bao gồm QR
        //     * compact	540x540	QR kèm logo VietQR, Napas, ngân hàng
        //     * compact2	540x640	Bao gồm : Mã QR, các logo , thông tin chuyển khoản
        //     * print	600x776	Bao gồm : Mã QR, các logo và đầy đủ thông tin chuyển khoản
        //     */
        //    if (Array.IndexOf(arr, Template) > -1) { }
        //    else
        //    {
        //        int i = 0;
        //        if (int.TryParse(Template, out i))
        //            if (i < arr.Length) Template = arr[i];
        //    }
        //    //%20
        //    string urlImage = string.Format(urlVietQR, BankId, AccountNo, Template, Amount, Description, AccountOwner);
        //    //**************************************************
        //    using (WebClient wc = new WebClient())
        //    {
        //        byte[] imgData = wc.DownloadData(urlImage);
        //        MemoryStream ms = new MemoryStream(imgData);
        //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //        response.Content = new StreamContent(ms);
        //        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        //        return response;
        //    }
        //    //**************************************************
        //}

        [HttpGet] // Cách 2
        public async Task<HttpResponseMessage> createQRCode([FromUri] string BankId, [FromUri] string AccountNo, string AccountOwner = null, string Amount = null, string Description = null, string Template = null)
        {
            if (AccountOwner == null) AccountOwner = ""; else AccountOwner = AccountOwner.Trim().Replace(" ", "%20");
            if (Amount == null) Amount = "";
            if (Description == null) Description = ""; else Description = Description.Trim().Replace(" ", "%20");
            if (Template == null) Template = arr[arr.Length - 1]; //print
            /*
             * qr_only	480x480	Trả về ảnh QR đơn giản, chỉ bao gồm QR
             * compact	540x540	QR kèm logo VietQR, Napas, ngân hàng
             * compact2	540x640	Bao gồm : Mã QR, các logo , thông tin chuyển khoản
             * print	600x776	Bao gồm : Mã QR, các logo và đầy đủ thông tin chuyển khoản
             */
            if (Array.IndexOf(arr, Template) > -1) { }
            else
            {
                int i = 0;
                if (int.TryParse(Template, out i))
                    if (i < arr.Length) Template = arr[i];
            }
            //%20
            string urlImage = string.Format(urlVietQR, BankId, AccountNo, Template, Amount, Description, AccountOwner);
            //**************************************************
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(urlImage);
                //byte[] imgData = await response.Content.ReadAsByteArrayAsync();
                byte[] imgData = await client.GetByteArrayAsync(urlImage);
                MemoryStream ms = new MemoryStream(imgData);
                HttpResponseMessage repMsg = new HttpResponseMessage(HttpStatusCode.OK);
                repMsg.Content = new StreamContent(ms);
                repMsg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return repMsg;
            }
            //**************************************************
        }

        //[HttpPost]
        //public async Task<HttpResponseMessage> genQRCode([FromBody] CreateVietQrRequest Req)
        //{
        //    string BankId = Req.BankId;
        //    string AccountNo = Req.AccountNo;
        //    string AccountOwner = Req.AccountOwner;
        //    string Amount = Req.Amount;
        //    string Description = Req.Description;
        //    string Template = Req.Template;
        //    //
        //    if (AccountOwner == null) AccountOwner = ""; else AccountOwner = AccountOwner.Trim().Replace(" ", "%20");
        //    if (Amount == null) Amount = "";
        //    if (Description == null) Description = ""; else Description = Description.Trim().Replace(" ", "%20");
        //    if (Template == null) Template = arr[arr.Length - 1]; //print
        //    /*
        //     * qr_only	480x480	Trả về ảnh QR đơn giản, chỉ bao gồm QR
        //     * compact	540x540	QR kèm logo VietQR, Napas, ngân hàng
        //     * compact2	540x640	Bao gồm : Mã QR, các logo , thông tin chuyển khoản
        //     * print	600x776	Bao gồm : Mã QR, các logo và đầy đủ thông tin chuyển khoản
        //     */
        //    if (Array.IndexOf(arr, Template) > -1) { }
        //    else
        //    {
        //        int i = 0;
        //        if (int.TryParse(Template, out i))
        //            if (i < arr.Length) Template = arr[i];
        //    }
        //    string urlImage = string.Format(urlVietQR, BankId, AccountNo, Template, Amount, Description, AccountOwner);
        //    //**************************************************
        //    using (HttpClient client = new HttpClient())
        //    {
        //        //HttpResponseMessage response = await client.GetAsync(urlImage); //(1)
        //        //byte[] imgData = await response.Content.ReadAsByteArrayAsync(); //(2)
        //        byte[] imgData = await client.GetByteArrayAsync(urlImage); // (3) = dòng (1) và (2)
        //        MemoryStream ms = new MemoryStream(imgData);
        //        HttpResponseMessage repMsg = new HttpResponseMessage(HttpStatusCode.OK);
        //        repMsg.Content = new StreamContent(ms);
        //        repMsg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        //        return repMsg;
        //    }
        //    //**************************************************
        //}

        [HttpPost]
        public string lookupAccountOwner(LookupAccountOwnerRequest req)
        {
            //using (HttpClient client = new HttpClient())
            //{
            //    //client.Timeout = -1;
            //    HttpResponseMessage response = client.GetAsync(urlListBank).Result;
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        string result = response.Content.ReadAsStringAsync().Result;
            //        List<BankDetail> lstBankDetail = JsonConvert.DeserializeObject<List<BankDetail>>(result.ToString());
            //    }
            //}

            string json = JsonConvert.SerializeObject(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", "demo-f5e14830-cab4-4d3a-93c7-926f937d5376a");
            client.DefaultRequestHeaders.Add("x-client-id", "demo-dd58bc3f-7544-4d22-b8d6-ed77b03242baa");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            HttpResponseMessage reponse = client.PostAsync(urlLookupAccOwner, content).Result;
            if (reponse.StatusCode == HttpStatusCode.OK)
            {
                string result = reponse.Content.ReadAsStringAsync().Result;
                //List<object> jsonData = JsonConvert.DeserializeObject<List<object>>(result.ToString());
                //JArray ArrJson = JArray.Parse(jsonData[0].ToString()); // gọi web service để test chỗ này
                //JArray ArrJson2 = (JArray)(ArrJson[0]);
                //_translatedText = ArrJson2.First.ToString();
                return result;
            }
            //    var client = new RestClient("https://api.vietqr.io/v2/lookup");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("x-client-id", "<CLIENT_ID_HERE>");
            //request.AddHeader("x-api-key", "<API_KEY_HERE>");
            //request.AddHeader("Content-Type", "application/json");
            //var body = @"{" + "\n" +
            //@"    ""bin"": ""970432""," + "\n" +
            //@"    ""accountNumber"": ""222777313""" + "\n" +
            //@"}";
            //request.AddParameter("application/json", body, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return string.Empty;
        }
        public struct LookupAccountOwnerRequest
        {
            public string BinCode { get; set; }
            public string AccountNo { get; set; }
        }


    }
}
