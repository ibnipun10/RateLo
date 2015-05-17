using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libScrapper.Common
{
    public class Utilities
    {
        public static string GetConvertedPropertyName(string propName)
        {
            string newName;
            //_hyphen_
            newName = propName.Replace("_hyphen_", "-");
            newName = newName.Replace("_bracketsOpen_", "(");
            newName = newName.Replace("_bracketsClosed_", ")");
            newName = newName.Replace("_amphasant_", "&");
            newName = newName.Replace("_", " ");
            newName = newName.TrimStart();
            return newName;
        }
    }
}
