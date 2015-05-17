using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using DBInteractor.Common;
using System.Reflection;
using libScrapper.Common;

namespace libScrapper
{

    public abstract class CScrapperBase
    {
        public HtmlDocument m_htmlDocument;
        public void SetHTMLDocument(string link)
        {
            if (string.IsNullOrEmpty(link))
                throw new Exception("Flipkart link is null or empty");

            HtmlWeb htmlWeb = new HtmlWeb();
            int trials = 1;
            while (trials <= 3)
            {
                try
                {
                    m_htmlDocument = htmlWeb.Load(link);
                    break;
                }
                catch (Exception ex)
                {
                    trials++;

                    if (trials > 3)
                        throw ex;
                }
            }
        }

       
        public void PopulateData(string strPropName, string strPropValue, ref ItemDescription objItem)
        {
            PropertyInfo[] props = objItem.GetType().GetProperties(); 
            foreach(PropertyInfo prop in props)
            {
                string Propname =  Utilities.GetConvertedPropertyName(prop.Name);
                object PropValue = prop.GetValue(objItem, null);

                if (Propname.CompareTo(strPropName) == 0 && PropValue == null)
                    prop.SetValue(objItem, strPropValue, null);

            }
        }
    }
}
