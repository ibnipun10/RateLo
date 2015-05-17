using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDBInterface.DBStructures;
using libDealSheelCommonMethods;

namespace DealSheelInserterTestProject
{
    class DelaSheelInserterTestRunner
    {
        static void Main(string[] args)
        {
            List<ExcelStructure> lobjExcel = new List<ExcelStructure>();
            CommonMethods.PopulateExcelData("C:\\Users\\Nipun\\Desktop\\Input-1427374734\\Input-1427374734\\VijaySales1-2015-03-14.xlsx", ref lobjExcel, null);
        }
    }
}
