using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace libExcelInteractor
{
    public class Excelutilities
    {
        
            public static void PopulateStructure<T>(string name, string value, ref T objStructure)
            {
                name = name.ToLower();
                PropertyInfo[] props = objStructure.GetType().GetProperties();

                foreach (PropertyInfo prop in props)
                {
                    string propName = prop.Name.ToLower();
                    if (name.CompareTo(propName) == 0)
                    {
                        if (prop.PropertyType == typeof(string))
                            prop.SetValue(objStructure, value, null);
                        else if (prop.PropertyType == typeof(int))
                            prop.SetValue(objStructure, Convert.ToInt32(value), null);
                        else if (prop.PropertyType == typeof(double))
                            prop.SetValue(objStructure, Convert.ToDouble(value), null);
                        else if (prop.PropertyType == typeof(long))
                            prop.SetValue(objStructure, Convert.ToInt64(value), null);
                        else if (prop.PropertyType == typeof(UInt32))
                            prop.SetValue(objStructure, Convert.ToUInt32(value), null);
                        else if (prop.PropertyType == typeof(DateTime))
                        {
                            double d = double.Parse(value);
                            DateTime conv = DateTime.FromOADate(d);
                            prop.SetValue(objStructure, conv, null);
                        }
                        break;
                    }
                }
            }     
    }
}
