using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace DBInteractor.Common
{
    public class Logger
    {
        private static string m_filename;
        private static string logFolder = "Log";
        private static System.Object lockThis = new System.Object();

        public static void InitializeLogs(string logFolder, string logfileName)
        {
            m_filename = logfileName;

            if (logFolder != null)
            {
                m_filename = logFolder + "/" + m_filename;


                //Create log folder if it does not exists
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);
            }

        }

        public static void WriteToLogFile(string line, string fileName, string logFileFolder)
        {
            if (logFileFolder != null)
            {
                fileName = logFileFolder + "/" + fileName;

                //Create log folder if it does not exists
                if (!Directory.Exists(logFileFolder))
                    Directory.CreateDirectory(logFileFolder);
            }

            string currentTime = DateTime.Now.ToString();
            string lineToWrite = currentTime + " : " + line;
            
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(line);
            }
        }

        public static void WriteToCSVFile(string line, string fileName, string logFileFolder)
        {

            fileName =  Path.GetFileNameWithoutExtension(fileName);
            fileName = fileName + ".csv";

            if (logFileFolder != null)
            {
                fileName = logFileFolder + "/" + fileName;

                //Create log folder if it does not exists
                if (!Directory.Exists(logFileFolder))
                    Directory.CreateDirectory(logFileFolder);
            }

            lock (lockThis)
            {
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine(line);
                }
            }
            
        }

      

        public static void WriteToLogFile(string line)
        {

            string currentTime = DateTime.Now.ToString();
            string lineToWrite = currentTime + " : " + line;
            using (StreamWriter sw = File.AppendText(m_filename))
            {
                sw.WriteLine(lineToWrite);
            }
        }

       
      

        public static void WriteObjectToLogFile<T>(T obj)
        {
            Utilities.GetCurrentMethod();
            string strPrint = "";

            if (obj == null)
                WriteToLogFile(obj.GetType().FullName + " Obj is null");

            WriteToLogFile("Object to print is " + obj.GetType().FullName);

            PropertyInfo[] props = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                strPrint += " Name : " + prop.Name + "Value : " + prop.GetValue(obj, null) + "/n";
            }
            
            WriteToLogFile(strPrint);

            
        }
    }
}
