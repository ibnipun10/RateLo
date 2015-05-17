using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using libScrapper;
using DBInteractor.Common;
using libExcelInteractor;
using System.Threading;

namespace libLinkScrapper
{
    public class flipKartLinkScraper
    {
        private string m_affiliateId;
        private string m_affiliteToken;
        private string m_affiliateUrl;
        private static int m_threadCount;

        public flipKartLinkScraper(string affId, string affToken)
        {
            m_affiliateId = affId;
            m_affiliteToken = affToken;
            m_affiliateUrl = "https://affiliate-api.flipkart.net/affiliate/api/";
            m_threadCount = 0;
        }

        public string GetCategoriesUrl()
        {
            return m_affiliateUrl + m_affiliateId + ".json";
        }

        public List<CategoryStructure> GetCategories()
        {
            List<CategoryStructure> lcateogry = new List<CategoryStructure>();

            WebClient webClient = new WebClient();
            String jsonString = webClient.DownloadString(GetCategoriesUrl());

            Dictionary<String, dynamic> apiGroups = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(jsonString);

            if (apiGroups.ContainsKey("apiGroups"))
            {
                Dictionary<String, dynamic> affiliate = JsonConvert.DeserializeObject <Dictionary<String, dynamic>>(Convert.ToString(apiGroups["apiGroups"]));

                if (affiliate.ContainsKey("affiliate"))
                {
                    Dictionary<String, dynamic> apiListings = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(Convert.ToString(affiliate["affiliate"]));

                    if(apiListings.ContainsKey("apiListings"))
                    {
                        Dictionary<String, dynamic> dictCateogry = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(Convert.ToString(apiListings["apiListings"]));

                        foreach(var key in dictCateogry.Keys)
                        {
                            Dictionary<string, dynamic> dictCat = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(Convert.ToString(dictCateogry[key]));

                            if(dictCat.ContainsKey("availableVariants"))
                            {
                                Dictionary<string, dynamic> availableVariants = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(Convert.ToString(dictCat["availableVariants"]));

                                if(availableVariants.ContainsKey("v0.1.0"))
                                {
                                    CategoryStructure objCat =  JsonConvert.DeserializeObject<CategoryStructure>(Convert.ToString(availableVariants["v0.1.0"]));
                                    lcateogry.Add(objCat);

                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Json does not contains key apiListings");
                }                    
                else
                    throw new Exception("Json does not contains key affiliate");

            }
            else
                throw new Exception("Json does not contains key apiGroups");

            return lcateogry;
        }

        public void GetProductListing(CategoryStructure objcat)
        {
            
            WebClient client = new WebClient();
            client.Headers.Add("Fk-Affiliate-Id", m_affiliateId);
            client.Headers.Add("Fk-Affiliate-Token", m_affiliteToken);

            string nextUrl = objcat.get;
            string lastProduct = String.Empty;
            bool bcontinue = true;

            
            string fileName = Constants.LINK_EXTRACTED_FOLDER + "//" + objcat.resourceName + ".csv";
            if (!System.IO.File.Exists(fileName))
            {
                string line = "Model_Name" + Constants.CSV_DELIMITER + 
                    "productBrand" + Constants.CSV_DELIMITER + 
                    "productUrl" + Constants.CSV_DELIMITER + 
                    "productId";

                Logger.WriteToCSVFile(line, objcat.resourceName, Constants.LINK_EXTRACTED_FOLDER);
            }

            //populate the csv first in memory
            List<List<string>> matcsv = Utilities.GetCSVSheet(fileName);
            Dictionary<string, string> dictProduct = new Dictionary<string, string>();
            string[] colNames = matcsv.First().ToArray();
            matcsv.RemoveAt(0);

            foreach (List<string> row in matcsv)
                dictProduct.Add(row.ElementAt(3), null);

            

            while (bcontinue)
            {
                string jsonString = client.DownloadString(nextUrl);
                Dictionary<String, dynamic> productObject = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(jsonString);

                string newLastProduct = productObject["lastProductId"];
                if (lastProduct.Equals(newLastProduct, StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("Last product id is either null or same as current last product id");
                else
                    lastProduct = newLastProduct;
                
                if (productObject.ContainsKey("nextUrl"))
                {
                    nextUrl = productObject["nextUrl"];
                    if(String.IsNullOrEmpty(nextUrl))
                        bcontinue = false;
                }
                else
                    bcontinue = false;

                if (productObject.ContainsKey("productInfoList"))
                {
                    string strpodcut = Convert.ToString(productObject["productInfoList"]);
                    List<ProductInfoList> lproductInfoList = JsonConvert.DeserializeObject <List<ProductInfoList>>(Convert.ToString(productObject["productInfoList"]));
                    List<ProductStructure> lproducts = new List<ProductStructure>();

                    foreach (ProductInfoList objpr in lproductInfoList)
                    {
                        objpr.productBaseInfo.productAttributes.productId = objpr.productBaseInfo.productIdentifier.productId;
                        lproducts.Add(objpr.productBaseInfo.productAttributes);
                    }

                    Logger.WriteToLogFile("Total Count : " + lproducts.Count);

                    WorkerThread objworkerThread = new WorkerThread(lproducts, Constants.LINK_EXTRACTED_FOLDER + "//" + objcat.resourceName, dictProduct);
                    System.Threading.Thread t = new System.Threading.Thread(objworkerThread.ScrapModelNumberAndWriteToCSV);
                    t.Start();
                    m_threadCount++;

                    while (m_threadCount == 3)
                        Thread.Sleep(5000);
            
                }
                else
                    throw new Exception("Json does not contains key productInfoList");
                
            }

            
            
        }

        class WorkerThread
        {
            List<ProductStructure> m_lProduct;
            string m_fileName;
            Dictionary<string, string> m_dictProduct;
            public WorkerThread(List<ProductStructure> lprod, string fileName, Dictionary<string, string> dictProduct)
            {
                m_lProduct = lprod;
                m_fileName = fileName;
                m_dictProduct = dictProduct;
            }
            public void ScrapModelNumberAndWriteToCSV()
            {
                //scrap the model number
                CFlipkartScrapper objscrapper = new CFlipkartScrapper();
                
                

                foreach(ProductStructure objpr in m_lProduct)
                {

                    //Get the product id and search if its already there in the csv
                    if (m_dictProduct.ContainsKey(objpr.productId))
                        continue;

                    try
                    {
                        ItemDescription objItem = new ItemDescription();

                        objscrapper.SetHTMLDocument(objpr.productUrl);
                        objscrapper.ExtractData(ref objItem);

                        if (objItem.Model_ID != null)
                            objpr.Model_Name = objItem.Model_ID;
                        else
                            objpr.Model_Name = objItem.Model_Name;

                        //Write to csv file
                        string line = objpr.Model_Name + Constants.CSV_DELIMITER + objpr.productBrand + Constants.CSV_DELIMITER + objpr.productUrl + Constants.CSV_DELIMITER + objpr.productId;
                        Logger.WriteToCSVFile(line, m_fileName, System.IO.Path.GetDirectoryName(m_fileName));
                    }
                    catch(Exception ex)
                    {

                    }
                }

                m_threadCount--;

                
            }
        }
        
    }
}
