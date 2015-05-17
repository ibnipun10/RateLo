using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelInteractor.ExcelInterface
{
    class ExcelController
    {
        private static Excel.Application m_ExcelApp;
        public static Excel.Workbook m_ExcelWorkbook;
        

        public static void InitializeExcel(string fileName)
        {
            m_ExcelApp = new Excel.Application();
            m_ExcelWorkbook = m_ExcelApp.Workbooks.Open(fileName);
            
        }

        public void CloseExcel()
        {
            m_ExcelWorkbook.Close();
            m_ExcelApp.Quit();
        }

        public static Excel.Worksheet GetWorkSheet(string sheetName)
        {
            return (Excel.Worksheet) m_ExcelWorkbook.Worksheets[sheetName];
        }
    }
}
