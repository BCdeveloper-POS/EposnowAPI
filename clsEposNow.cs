using EposNow;
using EposNow.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace EposNow.Models
{
    public class clsEposNow
    {
        private string StoreId;



        private string AccessToken = "";

        public clsEposNow(int StoreId, decimal tax, string BaseUrl, string RefreshToken)
        {
            try
            {
                Console.WriteLine("Generating EposNow " + StoreId + " Product File....");
                Console.WriteLine("Generating EposNow " + StoreId + " Fullname File....");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " EposNow " + StoreId);
            }
        }

        //method for productdetails
        #region hardcoded pagination
        //public List<EposnowProdList.Root> EposnowSetting(int StoreId, decimal tax, string BaseUrl, string Token)
        //{
        //    List<EposnowProdList.Root> list = new List<EposnowProdList.Root>();
        //    for (int i = 1; i <= 25; i++)
        //    {
        //        List<EposnowProdList.Root> list2 = EposNowProduct(i, StoreId, tax, BaseUrl, Token);
        //        if (list2.Count != 0)
        //        {
        //            list.AddRange(list2);
        //            continue;
        //        }
        //        break;
        //    }
        //    File.WriteAllText($"Products_response_{StoreId}.json" , JsonConvert.SerializeObject(list)); 
        //    return list;
        //}

        #endregion
        #region dynamic pagination  
        public List<EposnowProdList.Root> EposnowSetting(int StoreId, decimal tax, string BaseUrl, string Token)
        {
            List<EposnowProdList.Root> list = new List<EposnowProdList.Root>();

            for (int page = 1; ; page++)
            {
                List<EposnowProdList.Root> list2 = EposNowProduct(page, StoreId, tax, BaseUrl, Token);
                if (list2 == null || list2.Count == 0) { break; }

                list.AddRange(list2);
            }

            File.WriteAllText($"Products_response_{StoreId}.json", JsonConvert.SerializeObject(list));

            return list;
        }

        #endregion


        //method for stockdetails
        public List<EposnowStockList.Root> EposnowStockSetting(int StoreId, decimal tax, string BaseUrl, string Token)
        {
            List<EposnowStockList.Root> list = new List<EposnowStockList.Root>();
           

            for (int page = 1; ; page++)
            {
                List<EposnowStockList.Root> stklist = EposNowStock(page, StoreId, tax, BaseUrl, Token);
                if (stklist == null || stklist.Count == 0) { break; }

                list.AddRange(stklist);
            }

            File.WriteAllText($"Stock_response_{StoreId}.json", JsonConvert.SerializeObject(list));
            return list;
        }
        // method for category saving 
        public List<CatList> EposnowCatsSetting(int storeid, decimal tax, string BaseUrl, string Token)
        {
            List<CatList> clist = new List<CatList>();

         

            for (int page = 1; ; page++)
            {
                List<CatList> listcat = EposNowCats(page, storeid, tax, BaseUrl, Token);
                if (listcat == null || listcat.Count == 0) { break; }

                clist.AddRange(listcat);
            }
            return clist;
        }

        //method for product api call  
        public List<EposnowProdList.Root> EposNowProduct(int PageNo, int StoreId, decimal tax, string BaseUrl, string Token)
        {
            List<EposnowProdList.Root> result = new List<EposnowProdList.Root>();
            string text = null;
            EposnowProdList.Root root = new EposnowProdList.Root();
            RestClient restClient = new RestClient(BaseUrl + "Product/?page=" + PageNo + "&limit=200");
            RestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Authorization", Token);
            restRequest.AddHeader("Content-Type", "application/json");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse restResponse = restClient.Execute(restRequest);
            List<Parameter> list = restResponse.Headers.ToList();
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    text = restResponse.Content;
                    List<EposnowProdList.Root> source = JsonConvert.DeserializeObject<List<EposnowProdList.Root>>(text, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    result = source.ToList();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }



        //method for stock api call  
        public List<EposnowStockList.Root> EposNowStock(int PageNo, int StoreId, decimal tax, string BaseUrl, string Token)
        {
            List<JArray> list = new List<JArray>();
            List<EposnowStockList.Root> result = new List<EposnowStockList.Root>();
            string text = null;
            Root root = new Root();
            RestClient restClient = new RestClient(BaseUrl + "ProductStock?page=" + PageNo + "&limit=200");
            restClient.Timeout = -1;
            RestRequest restRequest = new RestRequest(Method.GET);
            restRequest.AddHeader("Authorization", Token);
            restRequest.AddHeader("Content-Type", "application/json");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse restResponse = restClient.Execute(restRequest);
            List<Parameter> list2 = restResponse.Headers.ToList();
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    text = restResponse.Content;
                    List<EposnowStockList.Root> source = JsonConvert.DeserializeObject<List<EposnowStockList.Root>>(text, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    result = source.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
        // method for categories api call 
        public List<CatList> EposNowCats(int PageNo, int StoreId, decimal tax, string BaseUrl, string Token)
        {
            List<CatList> result = new List<CatList>();
            string text = null;
            RestClient restClient = new RestClient(BaseUrl + "Category?page=" + PageNo + "&limit=200");
            restClient.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", Token);
            request.AddHeader("Content-Type", "application/json");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse restResponse = restClient.Execute(request);
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    text = restResponse.Content;
                    List<CatList> source = JsonConvert.DeserializeObject<List<CatList>>(text, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    result = source.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
        public List<containerFee> EposNowDepositAPI(string BaseUrl, string Token)//Added by PK on 08/01/2025
        {
            List<containerFee> result = new List<containerFee>();
            string text = null;
            RestClient restClient = new RestClient(BaseUrl + "ContainerFee");
            restClient.Timeout = -1;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", Token);
            request.AddHeader("Content-Type", "application/json");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse restResponse = restClient.Execute(request);
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    text = restResponse.Content;
                    List<containerFee> source = JsonConvert.DeserializeObject<List<containerFee>>(text, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    result = source.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
    }
}

