using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.DBInterface;
using libDBInterface.DBStructures;
using DBInteractor.Common;
using libScrapper;
using System.Threading;
using Telerik.WinControls.UI;
using libExcelInteractor;
using OfficeOpenXml;

namespace libDealSheelCommonMethods
{
    public class CommonMethods
    {
        public static string m_CategoryMappingTxt = "CategoryMapping.txt";
        public static string m_ResourcesFolder = "Resource";
        public static ExcelStructure SaveToDB(ExcelStructure objExcelStruct, Store objStore, SubCategory objSubCategory, Brand objBrand, string server, string port, GridViewRowInfo row = null)
        {

            try
            {
                if (objStore == null)
                    throw new Exception("Please select a store .....");
                if (objSubCategory == null)
                    throw new Exception("Subcateogry do not match or is empty");
                if (objBrand == null)
                    throw new Exception("Please add brand for this subcategory ir brand is null or not matching");


                ItemDescription objItemDescr = DBGetInterface.GetItemDescription(objBrand, objExcelStruct.ModelNumber);

                if (objItemDescr == null)
                {
                    //Call flipkart scrapper

                    CFlipkartScrapper objscrapper = new CFlipkartScrapper();
                    ItemDescription objItemdesc = ItemFactory.GetItem(objSubCategory);

                    if (String.IsNullOrEmpty(objExcelStruct.FlipKart))
                    {
                        //check if the item link is already scrapped, find in csv 
                        string strFilePath = m_ResourcesFolder + "//" + m_CategoryMappingTxt;
                        if (System.IO.File.Exists(strFilePath))
                        {
                            //File exists
                            //read the file and check for that file in LinkFodler
                            try
                            {
                                using (System.IO.StreamReader sw = new System.IO.StreamReader(strFilePath))
                                {
                                    string line = String.Empty;
                                    while ((line = sw.ReadLine()) != null)
                                    {
                                        bool bflag = false;
                                        string[] cat = line.Split(' ');
                                        if (cat[0].Equals(objSubCategory.SubCategoryID.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            //search model number in that subcategory file
                                            string file = Constants.LINK_EXTRACTED_FOLDER + "//" + cat[1] + ".csv";

                                            //search in this csv file
                                            List<List<string>> lmap = Utilities.GetCSVSheet(file);
                                            foreach (List<string> product in lmap)
                                            {
                                                if (product.ElementAt(0).Equals(objExcelStruct.ModelNumber, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    objExcelStruct.FlipKart = product.ElementAt(2);
                                                    bflag = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (bflag)
                                            break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }

                    objscrapper.SetHTMLDocument(objExcelStruct.FlipKart);
                    objscrapper.ExtractData(ref objItemdesc);

                    string image = objItemdesc.image;
                    objItemdesc.image = null;

                    //Now do the following
                    //Create item, create item description.
                    Item objItem = new Item();
                    objItem.Price = Convert.ToUInt32(objExcelStruct.Price);
                    objItem.OfferDescription = objExcelStruct.Offer;

                    ItemWrapper objWrap = new ItemWrapper();
                    objWrap.objBrand = objBrand;
                    objWrap.objStore = objStore;
                    objWrap.objItemDescription = objItemdesc;
                    objWrap.objItem = objItem;
                    DBAddinterface.CreateItemNode(objWrap);

                    //Copy image
                    //Create a seperate thread for it as image copying is a tedious process
                    //Copy image to local machine and then copy it to tomcat machine
                    //build path
                    string imageDir = Utilities.GetImageDir(objItemdesc.getLabel());
                    Utilities.CreateFolder(imageDir);
                    string imagePath = Utilities.GetImagePath(imageDir, objItemdesc.id);
                    ImageController.GetImageFromUrlAndSave(image, imagePath);


                    //Create a seperate thread for this to do 
                    Thread SaveImageThread = new Thread(() => SaveImageToRemoteMachineThread(imageDir, Utilities.AppendImageType(objItemdesc.id), server, port));
                    SaveImageThread.Start();

                    
                    objExcelStruct.Completed = Constants.COMPLETED_DONE;
                    objExcelStruct.Comment = String.Empty;

                }
                else
                {
                    
                    objExcelStruct.Comment = Constants.ALREADY_IN_DB;
                    //Just update the price and offer of the item
                    Item objItem = DBGetInterface.GetItem(objStore, objItemDescr);
                    if (objItem == null)
                    {
                        //Add this node
                        objItem = new Item();
                        objItem.Price = Convert.ToUInt32(objExcelStruct.Price);
                        objItem.OfferDescription = objExcelStruct.Offer;

                        ItemWrapper objWrap = new ItemWrapper();
                        objWrap.objBrand = objBrand;
                        objWrap.objStore = objStore;
                        objWrap.objItemDescription = objItemDescr;
                        objWrap.objItem = objItem;
                        DBAddinterface.CreateItemNode(objWrap);

                        objExcelStruct.Completed = Constants.COMPLETED_ADDED;

                    }
                    else
                    {
                        //Check for any updates
                        bool bUpdate = false;
                        if (objItem.Price != Convert.ToUInt32(objExcelStruct.Price))
                        {
                            objItem.Price = Convert.ToUInt32(objExcelStruct.Price);
                            bUpdate = true;
                        }

                        string offer = String.Empty;
                        if (objExcelStruct.Offer != null)
                            offer = objExcelStruct.Offer;

                        if (!objItem.OfferDescription.Equals(offer, StringComparison.InvariantCultureIgnoreCase))
                        {
                            objItem.OfferDescription = objExcelStruct.Offer;
                            bUpdate = true;
                        }

                        if (bUpdate)
                        {
                            objItem.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                            DBUpdateInterface.UpdateNode<Item>(objItem);
                            objExcelStruct.Completed = Constants.COMPLETED_UPDATED;
                            
                        }
                        else
                        {
                           objExcelStruct.Completed = Constants.COMPLETED_NOUPDATES;
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                objExcelStruct.Completed = Constants.COMPLETED_ERROR;
                objExcelStruct.Comment = ex.Message;
                           

            }

            if (row != null)
            {
                row.Cells["Completed"].Value = objExcelStruct.Completed;
                row.Cells["Comment"].Value = objExcelStruct.Comment;
            }

            return objExcelStruct;
        }

        public static void SaveImageToRemoteMachineThread(string imageDir, string fileName, string server, string port)
        {
            try
            {
                ImageController.SaveimageToRemoteLocation(imageDir, fileName, server, port);
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex.Message);
            }
        }

        public static string GetCommaSepearatedString(ExcelStructure objexceStruct)
        {
            return objexceStruct.SubCategory + Constants.CSV_DELIMITER +
                objexceStruct.Brand + Constants.CSV_DELIMITER +
                objexceStruct.ModelNumber + Constants.CSV_DELIMITER +
                objexceStruct.Price + Constants.CSV_DELIMITER +
                objexceStruct.Offer + Constants.CSV_DELIMITER +
                objexceStruct.FlipKart + Constants.CSV_DELIMITER +
                objexceStruct.Completed + Constants.CSV_DELIMITER +
                objexceStruct.Comment;
        }

        public static string GetCommaSeperatedColumnNames(ExcelStructure objexceStruct)
        {
            return "SubCategory" + Constants.CSV_DELIMITER +
                "Brand" + Constants.CSV_DELIMITER +
                "ModelNumber" + Constants.CSV_DELIMITER +
                "Price" + Constants.CSV_DELIMITER +
                "Offer" + Constants.CSV_DELIMITER +
                "FlipKart" + Constants.CSV_DELIMITER +
                "Completed" + Constants.CSV_DELIMITER +
                "Comment";
        }

        public static void PopulateExcelData(String fileName, ref List<ExcelStructure> lobjExcelStruct, Store objStore)
        {

            CExcelController.InitializeExcel(fileName);
            ExcelWorksheet sheet = CExcelController.GetWorkSheet(1);
            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;


            //Do not consider row 1 as its the header
            for (int Row = start.Row + 1; Row <= end.Row; Row++)
            {
                ExcelStructure objExcel = new ExcelStructure();

                for (int Col = start.Column; Col <= end.Column; Col++)
                {
                    string name = sheet.Cells[start.Row, Col].GetValue<string>();

                    if (name == null)
                        continue;

                    dynamic dValue = sheet.Cells[Row, Col].Value;
                    String value = null;
                    if (dValue != null)
                        value = dValue.ToString();
                    else
                        continue;
                    Excelutilities.PopulateStructure<ExcelStructure>(name, value, ref objExcel);

                }

                objExcel.objStore = objStore;

                lobjExcelStruct.Add(objExcel);
            }

            CExcelController.CloseExcel();
        }
    }
}
