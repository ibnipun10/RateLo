using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractor.Common
{
    class Rel_World
    {
        public static string world_country = "has_country";
    }

    class Rel_Country
    {
        public static string country_state = "has_state";
    }

    class Rel_State
    {
        public static string state_city = "has_city";
    }

    class Rel_City
    {
        public static string city_store = "has_store";
    }

    class Rel_CategoryRoot
    {
        public static string categoryroot_category = "has_category";
    }

    class Rel_BrandRoot
    {
        public static string brandroot_brand = "has_brand";
    }

    class Rel_StoreRoot
    {
        public static string storeroot_store = "has_store";
    }

    class Rel_Category
    {
        public static string category_subCategory = "has_subCategory";
    }

    class Rel_Store
    {
        public static string store_item = "has_item";
        public static string store_storeDescription = "has_storeDescription";
    }

    class Rel_Brand
    {
        public static string brand_item = "has_item";
        public static string brand_brandDescription = "has_brandDescription";
    }

    class Rel_SubCategory
    {
        public static string subCategory_brand = "has_brand";
        public static string subCategory_filter = "has_filter";
    }

    class Rel_Item
    {
        public static string item_itemDescription = "has_description";
    }

   
   
}
