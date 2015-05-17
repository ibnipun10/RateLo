using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractor.Common
{
    public class Constants
    {
        public static string ImageFolder = "DealSheelImages";
        public static string DealSheelConfigFile = "Resource\\DealSheelConfig.xml";
        public static string DealSheelAppPath = "DealSheelAppServer";

        public static string COMPLETED_ERROR = "ERROR";
        public static string COMPLETED_DONE = "Done";
        public static string COMPLETED_UPDATED = "Updated";
        public static string COMPLETED_NOUPDATES = "No Updates";
        public static string COMPLETED_ADDED = "Item Added in Store";

        public static string ALREADY_IN_DB = "Already in DB";
        public static string NOT_IN_DB = "Not in DB...scrapping";

        //ftp files
        public static string FTPSERVER_INPUT_EXCELFILES = "InputExcelFilePath.txt";
        public static string FTPSERVER_INPUT_FILE = "InputFile.txt";
        public static string FTPSERVER_OUTPUT_FILE = "OutputFile.txt";
        public static string FTPSERVER_INPUT_FOLDER = "Input";
        public static string FTPSERVER_OUTPUT_FOLDER = "Output";
        public static string FTPSERVER_TEMP_OUTPUT_FILE = "OutputFileTemp.txt";
        public static string FTPSERVER_OUTPUT_ERROR_LOGS = "Error.txt";
        public static string FTPClient_Logs = "ftpClient.log";
        public static string FTPSERVER_LOGS = "ftpServer.log";


        //FlipKArtListScrapper
        public static string FLIPKART_INPUT_ROBOTS_FILE = "Robots.txt";
        public static string FLIPKART_ROBOTS_SITEMAP_KEY = "Sitemap: ";

        public static string LINK_EXTRACTED_FOLDER = "FlipKartLinkFiles";

        public static string CSV_DELIMITER = "$#";

    }

    public class NodeLabels
    {
        public static string STORE_LABEL = "Store";
        public static string CATEGORY_LABEL = "Category";
        public static  string SUBCATEGORY_LABEL = "SubCategory";
        public static string CATEGORY_ROOT = "CategoryRoot";
        public static  string ITEM_LABEL = "Item";
        public static  string COUNTRY_LABEL = "Country";
        public static string WORLD_LABEL = "World";
        public static  string STATE_LABEL = "State";
        public static string CITY_LABEL = "City";
        public static  string BRAND_LABEL = "Brand";
        public static string BRAND_ROOT = "BrandRoot";
        public static string STORE_ROOT = "StoreRoot";
        public static  string OFFER_LABEL = "Offer";
        public static  string PRICE_LABEL = "Price";
        public static string ITEM_DESCRIPTION = "ItemDescription";
        public static string BRAND_DESCRIPTION = "BrandDescription";
        public static string STORE_DESCRIPTION = "StoreDescription";
        public static string Filter_LABEL = "Filter";
    }

    public class CStatus
    {
        public static string STATUS_ACTIVE = "Active";
        public static string STATUS_INACTIVE = "InActive";
    }

    public class CateogriesID
    {
        public const int CATEGORY_ELECTRONICS = 101;
        public const int CATEGORY_HOME_APPLIANCE = 201;
    }

    public class SubCategoriesID
    {
        // ---- 101 category
        public const int SUBCATEGORY_TV = 1001;
        public const int SUBCATEGORY_CAMERA  = 1002;
        public const int SUBCATEGORY_LAPTOP = 1003;

        //---- 201 Category
        public const int SUBCATEGORY_AC = 2001;        
        public const int SUBCATEGORY_FRIDGE = 2002;
        public const int SUBCATEOGRY_MICROWAVES = 2003;
        public const int SUBCATEGORY_WASHING_MACHINE = 2004;        
    }

    
}
