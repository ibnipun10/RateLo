using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using DBInteractor.Common;
using ExcelInteractor.ExcelCommon;
using DBInteractor.DBInterface;


namespace ExcelInteractor.ExcelInterface
{
    class ExcelAddInterface
    {
        
        public static void AddCountry(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;
            
            World objWorld = new World();
            CountryWrapper objWrap = new CountryWrapper();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                Country objCountry = new Country();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1,Col] as Excel.Range).Value2;

                    dynamic dValue = (usedRange.Cells[Row, Col] as Excel.Range).Value2;
                    String value = null;
                    if (dValue != null)
                        value = dValue.ToString();
                    else
                        continue;
                    ExcelUtilities.PopulateStructure<Country>(name, value, ref objCountry);
                }

                //Add the country in the Neo4j Database
                
                objWrap.objWorld = objWorld;
                objWrap.objCountry = objCountry;

                DBAddinterface.CreateCountryNode(objCountry);
                
            }
        }

        public static void AddState(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;
            
            StateWrapper objWrap = new StateWrapper();
            Country objCountry = new Country();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                State objState = new State();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    if (name.CompareTo(ExcelSheets.EXCELSHEET_COUNTRY) == 0)
                    {
                        objCountry.Name = value;
                        continue;
                    }
                    ExcelUtilities.PopulateStructure<State>(name, value, ref objState);
                }

                //Add the country in the Neo4j Database

                objWrap.objState = objState;
                objWrap.objCountry = objCountry;

                DBAddinterface.CreateStateNode(objWrap);

            }
        }

        public static void AddCity(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;

            CityWrapper objWrap = new CityWrapper();
            Country objCountry = new Country();
            State objState = new State();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                City objCity = new City();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    name = name.Trim();
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    if (name.CompareTo(ExcelSheets.EXCELSHEET_COUNTRY) == 0)
                    {
                        objCountry.Name = value;
                        continue;
                    }
                    else 
                    if (name.CompareTo(ExcelSheets.EXCELSHEET_STATE) == 0)
                    {
                        objState.Name = value;
                        continue;
                    }

                    ExcelUtilities.PopulateStructure<City>(name, value, ref objCity);
                }

                //Add the country in the Neo4j Database

                objWrap.objState = objState;
                objWrap.objCountry = objCountry;
                objWrap.objCity = objCity;

                DBAddinterface.CreateCityNode(objWrap);

            }
        }

        public static void AddStore(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;

            StoreWrapper objWrap = new StoreWrapper();
            Country objCountry = new Country();
            State objState = new State();
            City objCity = new City();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                Store objStore  = new Store();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    name = name.Trim();
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    if (name.CompareTo(ExcelSheets.EXCELSHEET_COUNTRY) == 0)
                    {
                        objCountry.Name = value;
                        continue;
                    }
                    else if (name.CompareTo(ExcelSheets.EXCELSHEET_STATE) == 0)
                    {
                        objState.Name = value;
                        continue;
                    }
                    else if (name.CompareTo(ExcelSheets.EXCELSHEET_CITY) == 0)
                    {
                        objCity.Name = value;
                        continue;
                    }

                    ExcelUtilities.PopulateStructure<Store>(name, value, ref objStore);
                }

                //Add the country in the Neo4j Database

                objWrap.objState = objState;
                objWrap.objCountry = objCountry;
                objWrap.objCity = objCity;
                objWrap.objStore = objStore;

                DBAddinterface.CreateStoreNode(objWrap);

            }
        }

        public static void AddBrand(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;

            BrandDescriptionWrapper objWrap = new BrandDescriptionWrapper();
            
            

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                BrandDescription objBrand = new BrandDescription();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    name = name.Trim();
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    ExcelUtilities.PopulateStructure<BrandDescription>(name, value, ref objBrand);
                }

                //Add the country in the Neo4j Database


                objWrap.objRoot = new BrandRoot();
                objWrap.objBrand = objBrand;

                DBAddinterface.CreateBrandDescriptionNode(objBrand);

            }
        }


        public static void AddCategory(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;

            CategoryRoot objcategoryRoot = new CategoryRoot();
            CategoryWrapper objWrap = new CategoryWrapper();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                Category objCategory = new Category();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;

                    dynamic dValue = (usedRange.Cells[Row, Col] as Excel.Range).Value2;
                    string value = null;

                    if (dValue != null)
                        value = dValue.ToString();
                    else
                        continue;

                    ExcelUtilities.PopulateStructure<Category>(name, value, ref objCategory);
                }

                //Add the country in the Neo4j Database

                objWrap.objCategoryRoot = objcategoryRoot;
                objWrap.objCategory = objCategory;

                DBAddinterface.CreateCategoryNode(objCategory);

            }
        }

        public static void AddSubCategory(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;

            SubCategoryWrapper objWrap = new SubCategoryWrapper();
            Category objCategory = new Category();
            


            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                SubCategory objSubCategory = new SubCategory();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    name = name.Trim();
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    if (name.CompareTo(ExcelSheets.EXCELSHEET_CATEGORY) == 0)
                    {
                        objCategory.Name = value;
                        continue;
                    }
                    ExcelUtilities.PopulateStructure<SubCategory>(name, value, ref objSubCategory);
                }

                //Add the country in the Neo4j Database


                objWrap.objCategory = objCategory;
                objWrap.objSubCategory = objSubCategory;

                DBAddinterface.CreateSubCategoryNode(objWrap);

            }
        }

        public static void AddItemDescription(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);
           
            Excel.Range usedRange = sheet.UsedRange;

            ItemDescriptionWrapper objWrap = new ItemDescriptionWrapper();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                
                
                Brand objBrand = new Brand();
                Category objCategory = new Category();
                SubCategory objSubCategory = new SubCategory();
                ItemDescription objItem = null;


                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    name = name.Trim();
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    switch (name)
                    {
                        case ExcelSheets.EXCELSHEET_CATEGORY:
                            objCategory.Name = value;
                            continue;
                            break;
                        case ExcelSheets.EXCELSHEET_BRAND:
                            objBrand.Name = value;
                            continue;
                            break;
                        case ExcelSheets.EXCELSHEET_SUBCATEGORY:
                            objSubCategory.Name = value;
                            objItem = ItemFactory.GetItem(objSubCategory);
                            continue;
                            break;
                        default: break;

                    }

                    ExcelUtilities.PopulateStructure<ItemDescription>(name, value, ref objItem);
                }

                //Add the country in the Neo4j Database
                                
                objWrap.objBrand = objBrand;
                objWrap.objCategory = objCategory;
                objWrap.objSubCetegory = objSubCategory;
                
                objWrap.objItem = objItem;

                DBAddinterface.CreateItemDescriptionNode(objWrap);

            }
        }

        private static bool GetItemFromDescriptionId(int DescriptionId, ref Brand objBrand, ref Category objCategory, ref SubCategory objSubCateogry, ref ItemDescription objItemDescription)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(ExcelSheets.EXCELSHEET_ITEMDESCRIPTION);
            Excel.Range usedRange = sheet.UsedRange;

            int iteDescriptionIDColumnNumber = 0;
            bool bFound = false;

            //Get item description column number
            //Do not consider row 1 as its the header
            for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
            {
                string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                name = name.Trim();
                
                if(name.CompareTo(ExcelSheets.EXCELCOLUMN_ITEMDESCRIPTION_ITEMDESCRIPTIONID) == 0)
                {
                    iteDescriptionIDColumnNumber = Col;
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
                return bFound;

            bFound = false;
            int itemFoundRowId = 0;

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                //Find the row which contails itemDescriptinid 
                int value = (int)(usedRange.Cells[Row, iteDescriptionIDColumnNumber] as Excel.Range).Value2;
                if(value == DescriptionId)
                {
                    bFound = true;
                    itemFoundRowId = Row;
                    break;
                }
            }

            if (!bFound)
                return bFound;

            for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
            {
                string strName = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                strName = strName.Trim();
                string value = null;

                if ((usedRange.Cells[itemFoundRowId, Col] as Excel.Range).Value2 != null)
                    value = (usedRange.Cells[itemFoundRowId, Col] as Excel.Range).Value2.ToString();
                else
                    continue;

                switch (strName)
                {
                    case ExcelSheets.EXCELSHEET_CATEGORY:
                        objCategory.Name = value;
                        continue;
                        break;
                    case ExcelSheets.EXCELSHEET_BRAND:
                        objBrand.Name = value;
                        continue;
                        break;
                    case ExcelSheets.EXCELSHEET_SUBCATEGORY:
                        objSubCateogry.Name = value;
                        continue;
                        break;
                   
                    default: break;

                }

                ExcelUtilities.PopulateStructure<ItemDescription>(strName, value, ref objItemDescription);
                
            }


            return bFound;

        }   

        public static void AddItem(string sheetName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Excel.Worksheet sheet = ExcelController.GetWorkSheet(sheetName);

            Excel.Range usedRange = sheet.UsedRange;

            ItemWrapper objWrap = new ItemWrapper();

            //Do not consider row 1 as its the header
            for (int Row = 2; Row <= usedRange.Rows.Count; Row++)
            {
                Item objItem = new Item();
                Store objStore = new Store();
                Brand objBrand = new Brand();
                Category objCategory = new Category();
                SubCategory objSubCategory = new SubCategory();
                ItemDescription objItemDescription = new ItemDescription();

                for (int Col = 1; Col <= usedRange.Columns.Count; Col++)
                {
                    string name = (string)(usedRange.Cells[1, Col] as Excel.Range).Value2;
                    name = name.Trim();
                    string value = null;

                    if ((usedRange.Cells[Row, Col] as Excel.Range).Value2 != null)
                        value = (usedRange.Cells[Row, Col] as Excel.Range).Value2.ToString();
                    else
                        continue;

                    switch (name)
                    {
                        case ExcelSheets.EXCELSHEET_STOREID: 
                            objStore.StoreId = value;
                            continue;
                            break;                        
                        case ExcelSheets.EXCELCOLUMN_ITEMDESCRIPTION_ITEMDESCRIPTIONID:
                            if (!GetItemFromDescriptionId(Convert.ToInt32(value), ref objBrand, ref objCategory, ref objSubCategory, ref objItemDescription))
                                Col += usedRange.Columns.Count;
                            continue;
                            break;
                        default: break;

                    }                   
                    
                    ExcelUtilities.PopulateStructure<Item>(name, value, ref objItem);
                }

                //Add the country in the Neo4j Database

                objWrap.objStore = objStore;
                objWrap.objBrand = objBrand;
                objWrap.objCategory = objCategory;
                objWrap.objSubCetegory = objSubCategory;
                objWrap.objItem = objItem;
                objWrap.objItemDescription = objItemDescription;

                DBAddinterface.CreateItemNode(objWrap);

            }
        }

               

    }
}
