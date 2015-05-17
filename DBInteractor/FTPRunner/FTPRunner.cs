using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libFTPInteractor;
using DBInteractor.Common;
using libDealSheelCommon.Common;
using System.IO;

/*
 * Please find steps here
 * FTP client creates a input folder with timestamp in the name
 * FTP client copies this folder onto the server
 * Server searches for input folder and runs the excel in them
 * Server copies the output to the output folder with timestamp
 * FTP copies this output folder onto its machine and then delete the output folder on the server
 * 
 */

namespace FTPRunner
{
    class FTPRunner
    {
        private static CXMLNode m_xmlNode;
        private static string m_timestamp;
        private static string m_InputFolder;
        private static string m_OutputFolder;
        static void Main(string[] args)
        {
            try
            {
                if (System.IO.File.Exists(Constants.FTPClient_Logs))
                    System.IO.File.Delete(Constants.FTPClient_Logs);

                //delete all input and output folder
                //Get all the directory from the current folder
                String[] Inputdirs = Directory.GetDirectories(".", Constants.FTPSERVER_INPUT_FOLDER + "*");
                String[] Outputdirs = Directory.GetDirectories(".", Constants.FTPSERVER_OUTPUT_FOLDER + "*");

                foreach (string dir in Inputdirs)
                    Directory.Delete(dir, true);
                
                foreach (string dir in Outputdirs)
                    Directory.Delete(dir, true);


                Logger.WriteToLogFile("Starting FTP Runner ....", Constants.FTPClient_Logs, null);

                m_xmlNode = XMLController.PopulateXMLObject(Constants.DealSheelConfigFile);

                //Get the current timestamp
                m_timestamp = Utilities.GetDateTimeInUnixTimeStamp().ToString();

                //Delete the iput folder
                //Logger.WriteToLogFile("Delete the input folder", Constants.FTPClient_Logs, null);
                
                //create input folder
                m_InputFolder = Constants.FTPSERVER_INPUT_FOLDER + "-" + m_timestamp;

                System.IO.Directory.CreateDirectory(m_InputFolder);

                //Get ftp interactor
                FTPInteractor objftpInteractor =  FTPInteractor.getInstance(m_xmlNode.FTPServer.Server, m_xmlNode.FTPServer.UserName, m_xmlNode.FTPServer.Password);

                //Copy FTP files from this machine to the ftp machine
                List<string> lFileList = Utilities.GetFileList(Constants.FTPSERVER_INPUT_EXCELFILES);

                //Copy the files to the input folder
                lFileList =  Utilities.CopyFiles(lFileList, m_InputFolder);

                //add the input file to the list
                lFileList.Add(Constants.FTPSERVER_INPUT_FILE);

                objftpInteractor.WriteToInputFTPFile(lFileList, Constants.FTPSERVER_INPUT_FILE, m_InputFolder);
                
                Logger.WriteToLogFile("Uploading files on the server", Constants.FTPClient_Logs, null);
                objftpInteractor.UploadFilesToFTP(lFileList, m_InputFolder, m_InputFolder);


                //Wait till the output is not written
                Logger.WriteToLogFile("Checking for program completion on server machine", Constants.FTPClient_Logs, null);
                m_OutputFolder = Constants.FTPSERVER_OUTPUT_FOLDER + "-" + m_timestamp;

                while(!objftpInteractor.CheckforFTPfileExists(Constants.FTPSERVER_OUTPUT_FILE, m_OutputFolder))
                    System.Threading.Thread.Sleep(60 * 1000);

                
                System.IO.Directory.CreateDirectory(m_OutputFolder);

                //Download file from the server
                Logger.WriteToLogFile("Download files from the server", Constants.FTPClient_Logs, null);
                objftpInteractor.DownloadFolderFromFTPServer(m_OutputFolder);

                //Delete the folder from the server
                Logger.WriteToLogFile("Delete output folder from the server", Constants.FTPClient_Logs, null);
                objftpInteractor.DeleteFolderFromServer(m_OutputFolder);
            
            }
            catch(Exception ex)
            {
                Logger.WriteToLogFile(ex.Message, Constants.FTPClient_Logs, null);
            }




        
        }
    }
}
