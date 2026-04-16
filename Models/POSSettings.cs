using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using EposNow.Models;
using Newtonsoft.Json;
namespace EposNow.Models
{
    public class POSSettings
    {
        public List<POSSetting> PosDetails { get; set; }

        public void IntializeStoreSettings()
        {
            DataSet dataSet = new DataSet();
            List<POSSetting> list = new List<POSSetting>();
            try
            {
                List<SqlParameter> sparams = new List<SqlParameter>();
               sparams.Add(new SqlParameter("@PosId", 46));

                string connectionString = ConfigurationManager.AppSettings.Get("LiquorAppsConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))

                {
                    using SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;

                    sqlCommand.CommandText = "usp_ts_GetStorePosSetting";
                    sqlCommand.Parameters.Add(sparams[0]);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(dataSet);
                }
                if (dataSet != null || dataSet.Tables.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        POSSetting pOSSetting = new POSSetting();
                        pOSSetting.Setting = row["Settings"].ToString();
                        StoreSetting storeSetting = new StoreSetting();
                        storeSetting.StoreId = Convert.ToInt32((row["StoreId"] == DBNull.Value) ? ((object)0) : row["StoreId"]);
                        storeSetting.POSSettings = JsonConvert.DeserializeObject<Setting>(pOSSetting.Setting);
                        pOSSetting.PosName = row["PosName"].ToString();
                        pOSSetting.PosId = Convert.ToInt32(row["PosId"]);
                        pOSSetting.StoreSettings = storeSetting;
                        if (pOSSetting.StoreSettings.POSSettings != null)
                        {
                            pOSSetting.StoreSettings.POSSettings.categoriess = storeSetting.POSSettings.categoriess;
                            pOSSetting.StoreSettings.POSSettings.Upc = storeSetting.POSSettings.Upc;
                        }
                        list.Add(pOSSetting);
                    }
                }
                PosDetails = list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
        }
    }
    public class StoreSetting
    {
        public int StoreId { get; set; }

        public Setting POSSettings { get; set; }
    }
    public class POSSetting
    {
        public int PosId { get; set; }

        public string PosName { get; set; }

        public StoreSetting StoreSettings { get; set; }

        public string Setting { get; set; }
    }
    public class Setting
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public int AccountID { get; set; }

        public string RefreshToken { get; set; }

        public string Token { get; set; }

        public string merchantId { get; set; }

        public string Store_id { get; set; }

        public string I_Storeid { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Pin { get; set; }

        public int SHOPID { get; set; }

        public string Code { get; set; }

        public string tokenid { get; set; }

        public string instock { get; set; }

        public string category { get; set; }

        public string BaseUrl { get; set; }

        public decimal tax { get; set; }

        public decimal mixtax { get; set; }

        public string PosFileName { get; set; }

        public string PosFileName2 { get; set; }

        public string APIKey { get; set; }

        public int StoreMapId { get; set; }

        public decimal liquortax { get; set; }

        public decimal liquortaxrateperlitre { get; set; }

        public List<categories> categoriess { get; set; }

        public string LocationId { get; set; }

        public bool IsSalePrice { get; set; }

        public bool IsMarkUpPrice { get; set; }

        public int MarkUpValue { get; set; }

        public bool IsApi { get; set; }

        public List<UPC> Upc { get; set; }

        public decimal beertax { get; set; }

        public decimal winetax { get; set; }

        public int client_id { get; set; }
    }
    public class UPC
    {
        public string upccode { get; set; }
    }
    public class categories
    {
        public string id { get; set; }

        public string name { get; set; }

        public decimal taxrate { get; set; }

        public bool selected { get; set; }
    }
}

