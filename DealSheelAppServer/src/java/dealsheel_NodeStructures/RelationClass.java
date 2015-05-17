/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_NodeStructures;

/**
 *
 * @author Nipun
 */
public class RelationClass {
    
    public static class Rel_World
    {
        public static String world_country = "has_country";
    }

    public static class Rel_Country
    {
        public static String country_state = "has_state";
    }

    public static class Rel_State
    {
        public static String state_city = "has_city";
    }

    public static class Rel_City
    {
        public static String city_store = "has_store";
    }

    public static class Rel_CategoryRoot
    {
        public static String categoryroot_category = "has_category";
    }

    public static class Rel_BrandRoot
    {
        public static String brandroot_brand = "has_brand";
    }

    public static class Rel_Category
    {
        public static String category_subCategory = "has_subCategory";
    }

    public static class Rel_Store
    {
        public static String store_item = "has_item";
        public static String store_storeDescription = "has_storeDescription";
    }

    public static class Rel_Brand
    {
        public static String brand_item = "has_item";
        public static String brand_brandDescription = "has_brandDescription";
    }

    public static class Rel_SubCategory
    {
        public static String subCategory_brand = "has_brand";
        public static String subCategory_filter = "has_filter";
    }

    public static class Rel_Item
    {
        public static String item_itemDescription = "has_description";
    }
}
