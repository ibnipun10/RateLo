using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using DBInteractor.Common;
using System.Reflection;

namespace libScrapper
{
    public class CFlipkartScrapper : CScrapperBase
    {
        public string GetImage()
        {
            try
            {

                HtmlNode imageNode = m_htmlDocument.DocumentNode.Descendants("img").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("productImage  current")).First();
                return imageNode.Attributes["data-src"].Value;
                /*
                //Image 
                HtmlNode imageNode = m_htmlDocument.DocumentNode.Descendants("meta").Where(x => x.Attributes.Contains("name") && x.Attributes["name"].Value.Contains("og_image")).First();
                return imageNode.Attributes["content"].Value;
                 */
            }
            catch(Exception ex)
            {
                
            }

            return null;
        }

        public string GetRatings()
        {
            try
            {
                //Ratings
                HtmlNode ratingsNode = m_htmlDocument.DocumentNode.Descendants("meta").Where(x => x.Attributes.Contains("itemprop") && x.Attributes["itemprop"].Value.Contains("ratingValue")).First();
                return ratingsNode.Attributes["content"].Value;
            }
            catch(Exception ex)
            {
                
            }

            return null;
            
        }

        public string GetShortDescription()
        {
            try
            {
                //Title
                HtmlNode TitleNode = m_htmlDocument.DocumentNode.Descendants("h1").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("title")).First();
                return TitleNode.InnerText;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public string GetName()
        {
            try
            {
                HtmlNode NameNode = m_htmlDocument.DocumentNode.Descendants("h2").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("item_desc_title")).First();
                return NameNode.InnerText;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public string GetLongDescription()
        {
            try
            {
                HtmlNode DescriptionNode = m_htmlDocument.DocumentNode.Descendants("ul").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("keyFeaturesList")).First();
                IEnumerable<HtmlNode> trNodes = DescriptionNode.Descendants("li");
                String description = String.Empty;
                
                foreach(HtmlNode node in trNodes)
                {
                    try
                    {
                        description += node.InnerText;
                    }
                    catch(Exception ex)
                    {

                    }
                }
                return description;
            }
            catch(Exception ex)
            {

            }

            return null;
        }

        public void SetModelName(ref ItemDescription objItem)
        {
            PropertyInfo[] props = objItem.GetType().GetProperties();
 
            foreach(PropertyInfo prop in props)
            {
                if(prop.Name.Equals("Model_ID"))
                {
                    string value = (string)prop.GetValue(objItem, null);

                    if(value != null)
                        objItem.Model_Name = (string)prop.GetValue(objItem, null);
                    return;

                }
            }
        }
        public void ExtractData(ref ItemDescription objItem)
        {

            HtmlNode divId = m_htmlDocument.DocumentNode.Descendants("div").Single(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("productSpecs specSection"));
            IEnumerable<HtmlNode> trNodes = divId.Descendants("tr");

            foreach(HtmlNode trnode in trNodes)
            {
                try
                {
                    HtmlNode nameNode = trnode.Descendants("td").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("specsKey")).First();
                    string name = nameNode.InnerText;

                    HtmlNode nameValue = trnode.Descendants("td").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("specsValue")).First();
                    string value = nameValue.InnerText;

                    value = value.Replace("\n", String.Empty);
                    value = value.Replace("\t"  , String.Empty);

                    PopulateData(name, value,ref objItem);

                    
                }
                catch(Exception ex)
                {

                }

            }

            try
            {
                objItem.image = GetImage();
                objItem.Ratings = GetRatings();
                objItem.shortDescription = GetShortDescription();
                objItem.longDescription = GetLongDescription();
                objItem.Name = GetName();

                SetModelName(ref objItem);
            }
            catch(Exception ex)
            {

            }

        }
    }
}
