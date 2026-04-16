using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using EposNow;
namespace EposNow.Models
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "EPOSNOW API";
            string to = ConfigurationManager.AppSettings["DeveloperId"];
            try
            {
                POSSettings pOSSettings = new POSSettings();
                pOSSettings.IntializeStoreSettings();
                foreach (POSSetting posDetail in pOSSettings.PosDetails)
                {
                    try
                    {
                        /*if (posDetail.StoreSettings.StoreId == 13017 || posDetail.StoreSettings.StoreId == 13018)
                        {
                            Console.WriteLine("Fetching storeId : " + posDetail.StoreSettings.StoreId);
                        }
                        else
                        {
                            continue;
                        }*/
                        if (posDetail.PosName.ToUpper() == "EPOSNOW" && posDetail.StoreSettings.POSSettings.IsApi)
                        {
                            EposnowCsvProducts eposnowCsvProducts = new EposnowCsvProducts(posDetail.StoreSettings.StoreId, posDetail.StoreSettings.POSSettings.tax, posDetail.StoreSettings.POSSettings.BaseUrl, posDetail.StoreSettings.POSSettings.Token);
                            Console.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                    }
                }
            }
            catch (Exception ex2)
            {
                new clsEmail().sendEmail(to, "", "", "Error in EposNow @" + DateTime.UtcNow.ToString() + " GMT", ex2.Message + "<br/>" + ex2.StackTrace);
                Console.WriteLine(ex2.Message);
            }
            finally
            {
            }
        }
    }
}
