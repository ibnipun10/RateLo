using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DBInteractor.Common
{
    public class Utilities
    {

 
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        public static long GetDateTimeInUnixTimeStamp()
        {
            TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            return (long)span.TotalSeconds;
        }

        public static string GetCurrentDateTime()
        {
            // dd/mm/yyyy
            DateTime dt = DateTime.Now;
            return String.Format("{0:dd/MM/yyyy}", dt);
        }

        public static string GetUniqueStringGUID()
        {
            return Guid.NewGuid().ToString();
        }

        public static ComboboxItem GetComboboxItem(string text)
        {
            ComboboxItem comboitem = new ComboboxItem();
            comboitem.Text = text;
            return comboitem;
        }

        public static void CreateFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);
        }

        public static string GetImageDir(string label)
        {
            return Constants.ImageFolder + "/" + label;
        }

        public static string GetImagePath(string dir, string id)
        {
            return dir + "/" + AppendImageType(id);
        }

        public static string AppendImageType(string id)
        {
            return id + ".jpg";
        }

        public static string GetImageServerUrl(string server, string port, string label, string id)
        {
            return "http://" + server + ":" + port +
                "/" + GetImagePath(GetImageDir(label), id);
        }

        public static List<string> GetFileList(string inputFile)
        {
            List<string> lfilePaths = new List<string>();
            string line = "";
            StreamReader file = new StreamReader(inputFile);
            while ((line = file.ReadLine()) != null)
            {
                lfilePaths.Add(line);
            }

            file.Close();
            return lfilePaths;
        }

        public static void CopyFile(string source, string dest)
        {
            File.Copy(source, dest, true);
        }

        public static List<string> CopyFiles(List<string> lsource, string destfolder)
        {
            List<string> fileList = new List<string>();
            foreach(string source in lsource)
            {
                try
                {
                    string fileName = Path.GetFileName(source);
                    string dest = destfolder + "\\" + fileName;
                    CopyFile(source, dest);
                    fileList.Add(fileName);
                }
                catch(Exception ex)
                {
                    Logger.WriteToLogFile(ex.Message, Constants.FTPClient_Logs, null);
                }
            }

            return fileList;
        }

        public static List<List<string>> GetCSVSheet(string fileName)
        {
            List<List<string>> csvMatrix = new List<List<string>>();

            using(StreamReader sw = new StreamReader(fileName))
            {
                string line = String.Empty;
                int i = 0;
                while((line = sw.ReadLine() ) != null)
                {
                    String[] columns = line.Split(new String[]{Constants.CSV_DELIMITER}, StringSplitOptions.None );
                    List<string> lcolumns = new List<string>();
                    foreach(string col in columns)
                    {
                        lcolumns.Add(col);                        
                    }

                    csvMatrix.Add(lcolumns);                    
                }
            }

            return csvMatrix;
        }


    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

   



   
}
