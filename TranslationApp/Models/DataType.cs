using TranslationApp.Utilities;

namespace TranslationApp.Models
{
    #region RestParamServer & Client
    // REST parameters of server send to api
    public struct RestServer
    {
        public string q { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string format { get; set; }
    }

    // REST parameters of client send to server
    public struct RestClient
    {
        public string text { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string key { get; set; }
    }
    #endregion

    #region Translate Request & Response
    public struct TranslateV2Request
    {
        public string[] data { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string key { get; set; } // authentication required
        public string user { get; set; } // authentication required (for log)
    }
    public struct TranslateV2Response
    {
        public DataResponse[] data { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
    public struct DataResponse
    {
        public string text { get; set; }
        public string source { get; set; }
        public int charge { get; set; }
    }
    #endregion

    #region Sql Translate Request & Response
    public struct SqlTranslateV2Request { public string[] data { get; set; } }
    public struct SqlTranslateV2Response { public DataResponse[] data { get; set; } }
    #endregion

    #region Customer Request & Response
    public struct CustomerRequest
    {
        public CustomerStruct[] data { get; set; }
        public string key { get; set; }
        public string user { get; set; }
    }
    public struct CustomerResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }
    #endregion

    #region StatisticsRequest & Response
    public struct StatisticsRequest
    {
        public string FDate { get; set; } //yyyyMMdd
        public string TDate { get; set; } //yyyyMMdd
        public string Customer { get; set; }
        public string key { get; set; }
        public string user { get; set; }
    }
    public struct StatisticsResponse
    {
        public PR_Statistics.StatisticsRecord[] data { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
    #endregion

    #region Agent information
    public class AgentInfo
    {
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string RequestTime { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Isp { get; set; }
    }

    public class CompactAgentInfo
    {
        public string Ip { get; set; }
        public string UserAgent { get; set; }
    }
    #endregion

    #region VietQR Request & Response
    public struct CreateVietQrRequest
    {
        public string BankId { get; set; }
        public string AccountNo { get; set; }
        public string AccountOwner { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
    }
    public struct ListBankResponse
    {
        public BankDetail[] data { get; set; }
    }
    public struct BankDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string bin { get; set; }
        public string shortName { get; set; }
        public string logo { get; set; }//https://api.vietqr.io/img/ICB.png
        public int transferSupported { get; set; }
        public int lookupSupported { get; set; }
        public string short_name { get; set; }
        public int support { get; set; }
        public int isTransfer { get; set; }
        public string swift_code { get; set; }
    }
    #endregion

    public static class CommonSettings
    {
        public static readonly string CompanyName = "MankichiSoftwareVietNam";
        public static readonly string StringMode = "DevMode";
        public static readonly string ApiKey = clsUtilities.GetApiKey();
        public static readonly int MaxCharacterNum = 2000; // actualy is 5000 characters per request, include header
        public static readonly string GGNotAvailableMsg = "(charge: is counting by web services, GG is not available)"; // 2023.03.10 - gg api chưa có method để lấy thông số này (kể cả xem report trên Dashboard console)
        public static readonly string ExceedLimitMsg = " (Some requests exceed the maximum character number limit)";
        public static readonly string DefaultLanguagesCode = "en"; // English
        public enum ResponseType
        {
            json,
            xml
        }

    }













}
