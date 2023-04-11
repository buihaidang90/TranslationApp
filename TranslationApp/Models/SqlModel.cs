using System;
using System.Collections.Generic;
using BHD_Framework;
using TranslationApp.Utilities;
using System.Data;
using System.Data.SqlClient;
using BHD_Framework;

namespace TranslationApp.Models
{
    public class SqlModel
    {
    }


    public class PR_Customer
    {
        SqlObject SqlObj = new SqlObject(clsUtilities.GetConnectionString());
        private string queryMaster = @"select * from PR_Customer";
        private CustomerStruct getCust(DataRow row)
        {
            if (row == null) return new CustomerStruct();
            CustomerStruct cust = new CustomerStruct();
            cust.Code = row["Code"].ToString();
            cust.Name = row["Name"].ToString();
            cust.Address = row["Address"].ToString();
            cust.TaxCode = row["TaxCode"].ToString();
            cust.Phone = row["Phone"].ToString();
            cust.Remark = row["Remark"].ToString();
            return cust;
        }
        public CustomerStruct Get(string Code)
        {
            Code = Code.Trim().Replace("'", "''");
            string sql = string.Format(@"select * from ({0}) where Code='{1}'", queryMaster, Code);
            DataTable dt = new DataTable();
            try { dt = SqlObj.ExecDataTable(sql); } catch { }
            if (dt.Rows.Count == 0) return new CustomerStruct();
            return getCust(dt.Rows[0]);
        }
        public List<CustomerStruct> Get()
        {
            List<CustomerStruct> lstCust = new List<CustomerStruct>();
            string sql = queryMaster;
            DataTable dt = new DataTable();
            try { dt = SqlObj.ExecDataTable(sql); } catch { }
            if (dt.Rows.Count == 0) return lstCust;
            foreach (DataRow row in dt.Rows) lstCust.Add(getCust(row));
            return lstCust;
        }
        public bool Exists(string Code)
        {
            Code = Code.Trim().Replace("'", "''");
            if (Code == "") return false;
            string sql = string.Format(@"select 1 from ({0}) where Code='{1}'", queryMaster, Code);
            string r = "";
            try { r = SqlObj.ExecScalar(sql).ToString(); } catch { }
            if (r == "1") return true;
            return false;
        }
        public bool Exists(CustomerStruct Customer) { return Exists(Customer.Code.Trim().Replace("'", "''")); }
        public bool Save(CustomerStruct[] Customers, int Mode) {
            List<CustomerStruct> lst = new List<CustomerStruct>();
            foreach (CustomerStruct cust in Customers) lst.Add(cust);
            return Save(lst, Mode);
        }
        public bool Save(List<CustomerStruct> Customers, int Mode)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[3];
                paras[0] = new SqlParameter("@Mode", SqlDbType.Int); paras[0].SqlValue = Mode;
                paras[1] = new SqlParameter("@HasError", SqlDbType.Bit); paras[1].Direction = ParameterDirection.Output;
                paras[2] = new SqlParameter("@Xml", SqlDbType.Xml); paras[2].SqlValue = BHD_Framework.Utility.CreateXml(createData(Customers, "MS"));
                SqlObj.ExecSql("sp_SaveCustomer", SqlSettings.SqlCommandType.StoredProcedure, paras);
                bool hasError = true; bool.TryParse(paras[1].Value.ToString(), out hasError);
                return !hasError;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public CustomerStruct Create(string sCode, string sName, string sAddress, string sTaxCode, string sPhone, string sRemark)
        {
            CustomerStruct cust = new CustomerStruct();
            cust.Code = sCode;
            cust.Name = sName;
            cust.Address = sAddress;
            cust.TaxCode = sTaxCode;
            cust.Phone = sPhone;
            cust.Remark = sRemark;
            return cust;
        }
        private DataTable createData(List<CustomerStruct> ListCustomer, string TableName)
        {
            string capCode = "Code", capName = "Name", capAddress = "Address", capTaxCode = "TaxCode", capPhone = "Phone", capRemark = "Remark";
            DataTable dt = new DataTable(TableName == "" ? "MS" : TableName);
            dt.Columns.Add(capCode);
            dt.Columns.Add(capName);
            dt.Columns.Add(capAddress);
            dt.Columns.Add(capTaxCode);
            dt.Columns.Add(capPhone);
            dt.Columns.Add(capRemark);
            foreach (CustomerStruct cust in ListCustomer)
            {
                DataRow row = dt.NewRow();
                row[capCode] = cust.Code;
                row[capName] = cust.Name;
                row[capAddress] = cust.Address;
                row[capTaxCode] = cust.TaxCode;
                row[capPhone] = cust.Phone;
                row[capRemark] = cust.Remark;
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
    public struct CustomerStruct
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string Phone { get; set; }
        public string Remark { get; set; }
    }

    public class PR_Statistics
    {
        SqlObject SqlObj = new SqlObject(clsUtilities.GetConnectionString());
        public bool Save(StatisticsStruct Item)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[2];
                paras[0] = new SqlParameter("@HasError", SqlDbType.Bit); paras[0].Direction = ParameterDirection.Output;
                paras[1] = new SqlParameter("@Xml", SqlDbType.Xml); paras[1].SqlValue =BHD_Framework.Utility.CreateXml(createData(new List<StatisticsStruct>() { Item }, "MS"));
                SqlObj.ExecScalar("sp_SaveStatistics", SqlSettings.SqlCommandType.StoredProcedure, paras);
                bool hasError = true; bool.TryParse(paras[0].Value.ToString(), out hasError);
                return !hasError;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public StatisticsStruct Create(string sCustomer, int iChargeCharacters, string sIpAddress, string sUserAgent, string sCountry, string sRegion, string sCity, string sISP, string sRemark)
        {
            StatisticsStruct item = new StatisticsStruct();
            item.Customer = sCustomer;
            item.ChargeCharaters = iChargeCharacters;
            item.IpAddress = sIpAddress;
            item.UserAgent = sUserAgent;
            item.Country = sCountry;
            item.Region = sRegion;
            item.City = sCity;
            item.ISP = sISP;
            item.Remark = sRemark;
            return item;
        }
        private DataTable createData(List<StatisticsStruct> ListItem, string TableName)
        {
            string capChargeCharaters = "ChargeCharaters", capCustomer = "Customer", capIpAddress = "IpAddress", capUserAgent = "UserAgent", capCountry = "Country", capRegion = "Region", capCity = "City", capISP = "ISP", capRemark = "Remark";
            DataTable dt = new DataTable(TableName == "" ? "MS" : TableName);
            dt.Columns.Add(capChargeCharaters);
            dt.Columns.Add(capCustomer);
            dt.Columns.Add(capIpAddress);
            dt.Columns.Add(capUserAgent);
            dt.Columns.Add(capCountry);
            dt.Columns.Add(capRegion);
            dt.Columns.Add(capCity);
            dt.Columns.Add(capISP);
            dt.Columns.Add(capRemark);
            foreach (StatisticsStruct item in ListItem)
            {
                DataRow row = dt.NewRow();
                row[capChargeCharaters] = item.ChargeCharaters;
                row[capCustomer] = item.Customer;
                row[capIpAddress] = item.IpAddress;
                row[capUserAgent] = item.UserAgent;
                row[capCountry] = item.Country;
                row[capRegion] = item.Region;
                row[capCity] = item.City;
                row[capISP] = item.ISP;
                row[capRemark] = item.Remark;
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
    public struct StatisticsStruct
    {
        public int ChargeCharaters { get; set; }
        public string Customer { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string ISP { get; set; }
        public string Remark { get; set; }
    }







}