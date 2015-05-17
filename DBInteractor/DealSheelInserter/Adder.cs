using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DBInteractor.Common;
using libDBInterface.DBStructures;
using libDealSheelCommonMethods;
using DBInteractor.DBInterface;
using libDealSheelCommon.Common;
using Telerik.WinControls.UI;

namespace DealSheelInserter
{
    public class Adder
    {
        private CXMLNode m_xmlNode;

        public Adder(CXMLNode xmlNode)
        {
            m_xmlNode = xmlNode;
        }


        public void RunAllExcel(List<string> fileNames, string inputFolder, string outputFolder)
        {           

            foreach(string filename in fileNames)
            {
                RunExcel(filename, inputFolder, outputFolder);
            }
        }

        public void RunExcel(string fileName, string inputFolder, string outputFolder)
        {
            try
            {
                //Get store id
                string storeid = fileName.Split('-').First();

                //create csv with column names
                ExcelStructure objExcel = new ExcelStructure();
                Logger.WriteToCSVFile(CommonMethods.GetCommaSeperatedColumnNames(objExcel), fileName, outputFolder);

                Store objStore = DBGetInterface.GetStore(storeid);
                if (objStore == null)
                    throw new Exception("Store id is null for excel : " + fileName);
                  
                //Run the query
                List<ExcelStructure> lobjExcel = new List<ExcelStructure>();
                CommonMethods.PopulateExcelData(inputFolder + "\\" + fileName, ref lobjExcel, objStore);

                foreach (ExcelStructure objExcelStruct in lobjExcel)
                {
                    SubCategory objSubCateogry = DBGetInterface.GetSubCategoryNode(objExcelStruct.SubCategory);
                    Brand objBrand = DBGetInterface.GetBrandNode(objSubCateogry, objExcelStruct.Brand);

                    ExcelStructure objexcelpop = CommonMethods.SaveToDB(objExcelStruct, objStore, objSubCateogry, objBrand, m_xmlNode.AppServer.Server, m_xmlNode.AppServer.Port);

                    Logger.WriteToCSVFile(CommonMethods.GetCommaSepearatedString(objexcelpop), fileName, outputFolder);
                   
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLogFile(ex.Message, Constants.FTPSERVER_OUTPUT_ERROR_LOGS, outputFolder);
            }



        }
    }
}
