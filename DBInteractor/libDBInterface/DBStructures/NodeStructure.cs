using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDealSheelCommon.Common;

namespace DBInteractor.Common
{
    public class LatLong : Node
    {
        public LatLong(string label)
            : base(label)
        {
        }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Node
    {
        private string Label;

        public Node(string label)
        {
            createTime = Utilities.GetDateTimeInUnixTimeStamp();
            lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
            Label = label;
            id = Utilities.GetUniqueStringGUID();
            
        }

        public string getLabel() { return Label; }
        public string id { get; set; }
        public string Name { get; set; }
        public long createTime { get; set; }
        public long lastUpdated { get; set; }       

    }

    public class Store : LatLong
    {
        public Store() : 
            base(NodeLabels.STORE_LABEL)
        {
            Status = CStatus.STATUS_ACTIVE;
            Views = 0;
        }

        public string StoreId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber {get ;set; }
        public long Pincode {get ; set;}
        public string Landmark { get; set; }
        public String Status { get; set; }
        public int Views { get; set; }

    }

    public class StoreDescription : Node
    {
        public StoreDescription():
            base(NodeLabels.STORE_DESCRIPTION)
        {
            
        }
        

    }


    public class Category : Node
    {
        public Category() : 
            base(NodeLabels.CATEGORY_LABEL)
        {           
        }

        


    }

    public class SubCategory : Node
    {
        public SubCategory() : 
            base(NodeLabels.SUBCATEGORY_LABEL)
        {
            Views = 0;
        }

        public int SubCategoryID { get; set; }
        public int Views { get; set; }

        
    }

    public class CategoryRoot: Node
    {
        public CategoryRoot() :
            base(NodeLabels.CATEGORY_ROOT)
        {
            Name = "CategoryRoot";
            
        }
    }

    

    public class World : Node
    {
        public World() : 
            base(NodeLabels.WORLD_LABEL)
        {
           
            Name = "World";
            
        }
    }

    public class Country : LatLong
    {
        public Country() : 
            base(NodeLabels.COUNTRY_LABEL)
        {
            
        }

    }    

    public class State : LatLong
    {
        public State() : 
            base(NodeLabels.STATE_LABEL)
        {
            
        }
    }

    public class City : LatLong
    {
        public City() :
            base(NodeLabels.CITY_LABEL)
        {

        }
    }

    

    public class Brand : Node
    {
        public Brand() :
            base(NodeLabels.BRAND_LABEL)
        {
            Status = CStatus.STATUS_ACTIVE;
        }

        public String Status { get; set;}
    }

    public class BrandDescription : Node
    {
        public BrandDescription() :
            base(NodeLabels.BRAND_DESCRIPTION)
        {

        }

        
    }

    public class BrandRoot : Node
    {
        public BrandRoot() :
            base(NodeLabels.BRAND_ROOT)
        {
            Name = "BrandRoot";
            
        }
    }

    public class StoreRoot : Node
    {
        public StoreRoot() :
            base(NodeLabels.STORE_ROOT)
        {
            Name = "StoreRoot";
            

        }
    }

    public class Item : Node
    {
        public Item() :
            base(NodeLabels.ITEM_LABEL)
        {
            Status = CStatus.STATUS_ACTIVE;
            Quantity = 1;
            Views = 0;
            OfferDescription = String.Empty;
        }
        
        public int Quantity { get; set; }
        public string ProductSKU { get; set; }
        public string Status { get; set; }
        public string OfferDescription { get; set; }
        public string OfferID { get; set; }
        public int Views { get; set; }
        public uint Price { get; set; }

    }

    public class ItemDescription : Node
    {
        public ItemDescription() :
            base(NodeLabels.ITEM_DESCRIPTION)
        {
            Views = 0;
        }

        public string Model_ID { get; set; }
        public string Model_Name { get; set; }
        public int Views { get; set; }
        public string Brand { get; set; }
        public string Ratings { get; set; }
        public string shortDescription { get; set; }
        public string longDescription { get; set; }

        public string image { get; set; }
    }

    public class Filter : Node
    {
        public Filter() :
            base(NodeLabels.Filter_LABEL)
        {
            IsInt = false;
        }
        public string NameAlias { get; set; }
        public bool IsInt { get; set; }
    }

   

}