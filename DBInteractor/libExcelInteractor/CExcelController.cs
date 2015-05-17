using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace libExcelInteractor
{
    public class CExcelController
    {

        private static ExcelPackage m_ExcelApp;
        public static ExcelWorkbook m_ExcelWorkbook;


        public static void InitializeExcel(string fileName)
        {
            m_ExcelApp = new ExcelPackage(new FileInfo(fileName));
            m_ExcelWorkbook = m_ExcelApp.Workbook;
        }

        public static void CloseExcel()
        {            
            m_ExcelApp.Dispose();
        }

        public static ExcelWorksheet GetWorkSheet(string sheetName)
        {
            return m_ExcelWorkbook.Worksheets[sheetName];
        }

        public static ExcelWorksheet GetWorkSheet(int index)
        {
            return m_ExcelWorkbook.Worksheets[index];
        }
    }
}
