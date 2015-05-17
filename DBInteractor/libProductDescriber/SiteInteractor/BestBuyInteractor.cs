using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;
using System.Net;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using libProductDescriber.Common;
using System.Collections;

namespace libProductDescriber.SiteInteractor
{
    public class BestBuyInteractor
    {
        public static string m_BestBuyApiKey = "437wvyh8s4e47tknbc43a254";


        public static string CreateRequst(string searchQuery)
        {
            string urlRequest = "http://api.remix.bestbuy.com/v1/products({0})" +
                "?show=customerReviewAverage,customerReviewCount,customerTopRated, width,weight,longDescription,description,name,color,depth,manufacturer,modelNumber,shortDescription,details.name,details.value,features.feature,image" + 
                "&format=json" + 
                "&apiKey={1}";

            return String.Format(urlRequest, searchQuery, m_BestBuyApiKey);
        }

        public static string GetResponse(string searchQuery)
        {
            string strResponse;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(CreateRequst(searchQuery));
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";

            WebResponse response =  httpWebRequest.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                strResponse = sr.ReadToEnd();
            }

            return strResponse;
        }

        public static void DeserializeJson(string response, ref ItemDescription objItem)
        {
            var jss = new JavaScriptSerializer();
            var dict = jss.Deserialize < Dictionary<string, dynamic>>(response);
            PropertyInfo[] props = objItem.GetType().GetProperties();

            ArrayList productArray = dict[Common.Common.JSON_PRODUCT_ATTRIBUTE_PRODUCTS];
            

            if (productArray == null || productArray.Count == 0)
                throw new Exception("Unable to find any product");

            Dictionary<string, dynamic> productDict = (Dictionary<string,dynamic>)productArray[0];

            ArrayList detailsArray = productDict[Common.Common.JSON_PRODUCT_ATTRIBUTE_DETAILS];
          

            if(detailsArray != null)
            {
                
                foreach (var detail in detailsArray)
                {
                    Dictionary<string, dynamic> detaildict = (Dictionary<string, dynamic>)detail;
                    string detailName = detaildict[Common.Common.JSON_PRODUCT_ATTRIBUTE_NAME];

                    foreach (PropertyInfo prop in props)
                    {
                        string Propname = GetConvertedPropertyName(prop.Name);
                        if (detailName.CompareTo(Propname) == 0)
                        {
                            prop.SetValue(objItem, detaildict[Common.Common.JSON_PRODUCT_ATTRIBUTE_VALUE], null);
                            break;
                        }                                            

                    }
                }
            }

            foreach(PropertyInfo prop in props)
            {
                string Propname = GetConvertedPropertyName(prop.Name);
                string value = null;
                if (!productDict.ContainsKey(Propname))
                  continue;

                if (Propname.CompareTo(Common.Common.JSON_PRODUCT_ATTRIBUTE_FEATURES) == 0)
                {
                    ArrayList featureArray = productDict[Common.Common.JSON_PRODUCT_ATTRIBUTE_FEATURES];
                    string featureValue = String.Empty;
                    foreach (var feature in featureArray)
                    {
                        Dictionary<string, dynamic> featuredict = (Dictionary<string, dynamic>)feature;

                        featureValue += featuredict[Common.Common.JSON_PRODUCT_ATTRIBUTE_FEATURE];
                    }

                    value = featureValue;
                    
                }
                else
                    value = "" + productDict[Propname];

                if (value == null)
                    continue;

                if (prop.PropertyType == typeof(int))
                    prop.SetValue(objItem, Convert.ToInt32(value), null);
                else if (prop.PropertyType == typeof(float))
                    prop.SetValue(objItem, Convert.ToSingle(value), null);
                else if (prop.PropertyType == typeof(bool))
                    prop.SetValue(objItem, Convert.ToBoolean(value), null);
                else
                    prop.SetValue(objItem, value, null);
            }

        }


        public static string GetConvertedPropertyName(string propName)
        {
            string newName;
            //_hyphen_
            newName = propName.Replace("_hyphen_", "-");
            newName = newName.Replace("_bracketsOpen_", "(");
            newName = newName.Replace("_bracketsClosed_", ")");
            newName = newName.Replace("_", " ");

            return newName;
        }
    }
}
