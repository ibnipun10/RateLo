using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common; 

namespace libDBInterface.DBStructures
{
    public class ExcelStructure
    {
        public ExcelStructure()
        {
            SubCategory = String.Empty;
            Brand = String.Empty;
            ModelNumber = String.Empty;
            Offer = String.Empty;
            FlipKart = String.Empty;
            Comment = String.Empty;
            Completed = String.Empty;
        }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; }
        public uint Price { get; set; }
        public string Offer { get; set; }
        public string FlipKart { get; set; }

        public string Completed { get; set; }
        public string Comment { get; set; }

        public Store objStore { get; set; }

    }

    public class ExcelLinkStructure
    {
        public string StoreId { get; set; }
        public string ExcelLink { get; set; }
    }
}
