using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libLinkScrapper
{
    public class CategoryStructure
    {
        public string resourceName { get; set; }
        public string get{get; set;}
    }

    public class ProductStructure
    {
        public string productUrl {get; set;}
        public string productBrand {get; set;}
        public string Model_Name { get; set; }
        public string productId { get; set; }
    }

    public class ProductIdentifier
    {
        public string productId { get; set; }
    }

    public class ProductInfoList
    {
        public ProductAttributes productBaseInfo;
    }

    public class ProductAttributes
    {
        public ProductStructure productAttributes;
        public ProductIdentifier productIdentifier;
    }

    
}
