using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelInteractor.ExcelInterface
{
    class ExcelSheets
    {
        public const string EXCELSHEET_COUNTRY = "Country";
        public const string EXCELSHEET_STATE = "State";
        public const string EXCELSHEET_CITY = "City";
        public const string EXCELSHEET_STORE = "Store";
        public const string EXCELSHEET_BRAND = "Brand";
        public const string EXCELSHEET_CATEGORY = "Category";
        public const string EXCELSHEET_SUBCATEGORY = "SubCategory";
        public const string EXCELSHEET_ITEM = "Item";
        public const string EXCELSHEET_PRICE = "Price";
        public const string EXCELSHEET_OFFER = "Offer";
        public const string EXCELSHEET_STOREID = "StoreId";
        public const string EXCELSHEET_ITEMDESCRIPTION = "ItemDescription";
        public const string EXCELCOLUMN_ITEMDESCRIPTION_ITEMDESCRIPTIONID = "ItemDescriptionId";
        public const string EXCELCOLUMN_NAME = "Name";
    }

    class Common
    {
        public const string EXCEL_PATH = "C:\\Users\\Nipun\\Desktop\\SkyDrive\\DealSheel\\DealSheelImport.xlsx";
    }

    public class CreateNodeFlags
    {

        public const int FLAG_COUNTRY = 1;
        public const int FLAG_STATE = 2;
        public const int FLAG_CITY = 4;
        public const int FLAG_STORE = 8;
        public const int FLAG_CATEGORY = 16;
        public const int FLAG_SUBCATEGORY = 32;
        public const int FLAG_BRAND = 64;
        public const int FLAG_ITEMDESCRIPTION = 128;
        public const int FLAG_ITEM = 256;
    }
}
