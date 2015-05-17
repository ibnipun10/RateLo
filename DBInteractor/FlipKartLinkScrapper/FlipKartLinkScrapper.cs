using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;
using System.IO.Compression;
using System.Xml.Serialization;
using System.IO;
using libLinkScrapper;

namespace FlipKartLinkScrapper
{
    class FlipKartLinkScrapper
    {
        private static string m_CategoyFileName = "Categories.txt";
        private static string m_logFile = "flipKartLinkScrapperLog.txt";
        static void Main(string[] args)
        {
            if (File.Exists(m_logFile))
                    File.Delete(m_logFile);

                Logger.InitializeLogs(null, m_logFile);

                Logger.WriteToLogFile("FlipkartLinkScrapper Started");

            try
            {
                               

                flipKartLinkScraper objFlip = new flipKartLinkScraper("ibnipun10", "c7afe3f0ee284ae887f05e291aedb110");

                List<CategoryStructure> lCat = objFlip.GetCategories();
                Logger.WriteToLogFile("Total Cateogries extracted : " + lCat.Count);

                //read categories file
                StreamReader sr = new StreamReader(m_CategoyFileName);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        foreach (CategoryStructure objcat in lCat)
                        {
                            if (objcat.resourceName.Equals(line, StringComparison.InvariantCultureIgnoreCase))
                            {
                                Logger.WriteToLogFile("Starting extraction for category : " + objcat.resourceName);
                                objFlip.GetProductListing(objcat);
                                continue;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.WriteToLogFile("Exception occured : " + ex.Message);
                    }

                }
                sr.Close();

            }
            catch(Exception ex)
            {
                Logger.WriteToLogFile("Exception occured : " + ex.Message);
            }
        }

        static void DownloadFileFromURL(string url, string fileName)
        {
            using (WebClient Client = new WebClient())
            {
                Client.DownloadFile(url, fileName);
            }
        }
    }
}
