using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libDBInterface.DBStructures;
using DBInteractor.Common;
using libDealSheelCommon.Common;
using DBInteractor.DBInterface;
using System.IO;

namespace DealSheelInserter
{
    class Program
    {
  
        static void Main(string[] args)
        {
            
                Logger.WriteToLogFile("Starting DealSheelRunner", Constants.FTPSERVER_LOGS, null);
                

                //Get all the directory from the current folder
                String[] dirs = Directory.GetDirectories(".", Constants.FTPSERVER_INPUT_FOLDER + "*");

                if (dirs.Length == 0)
                    return;

                foreach (string dir in dirs)
                {
                    //get the timestamp from the directory name
                    string timestamp = dir.Split('-').Last();
                    string outputFodler = Constants.FTPSERVER_OUTPUT_FOLDER + "-" + timestamp;

                    try
                    {
                        //Checking for input file 
                        if (!System.IO.File.Exists(dir + "\\" + Constants.FTPSERVER_INPUT_FILE))
                            continue;

                        
                        if (System.IO.Directory.Exists(Constants.ImageFolder))
                            System.IO.Directory.Delete(Constants.ImageFolder, true);

                        //outfolder
 
                        if (Directory.Exists(outputFodler))
                            Directory.Delete(outputFodler, true);

                        //create output  and initialize logs
                        Logger.InitializeLogs(outputFodler, Constants.FTPSERVER_TEMP_OUTPUT_FILE);
                        Logger.WriteToLogFile("Starting DealSheelInserter .....");
                        CXMLNode m_xmlNode = XMLController.PopulateXMLObject(Constants.DealSheelConfigFile);

                        if (m_xmlNode == null)
                            throw new Exception("Unable to parse xml file");

                        Logger.WriteToLogFile("Initialize Neo4j");

                        Neo4jController.InitializeController(m_xmlNode.DatabaseServer.Server, Convert.ToInt32(m_xmlNode.DatabaseServer.Port));
                        Neo4jController.connect();


                        Adder objAdder = new Adder(m_xmlNode);
                        List<string> lfiles = Utilities.GetFileList(dir + "//" + Constants.FTPSERVER_INPUT_FILE);

                        lfiles.RemoveAt(lfiles.Count() - 1);
                        objAdder.RunAllExcel(lfiles, dir, outputFodler);

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteToLogFile(ex.Message, Constants.FTPSERVER_OUTPUT_ERROR_LOGS, outputFodler);
                        
                    }
                    //work done
                    //Rename the file to output file to let us know that the process has finished
                    System.IO.File.Move(outputFodler + "//" + Constants.FTPSERVER_TEMP_OUTPUT_FILE,
                                        outputFodler + "//" + Constants.FTPSERVER_OUTPUT_FILE);
                    
                    //Remove input folder now
                    if (System.IO.Directory.Exists(dir))
                        System.IO.Directory.Delete(dir, true);
                    
                }  
            

            Logger.WriteToLogFile("Finished DealSheelRunner", Constants.FTPSERVER_LOGS, null);
        }
    }
}
