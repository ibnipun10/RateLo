using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.FtpClient;
using System.Net;
using DBInteractor.Common;

namespace libFTPInteractor
{
    public class FTPInteractor
    {
        private static FTPInteractor objFTPInteractor;
        private string m_hostname;
        private string m_userName;
        private string m_password;
        private FTPInteractor(string hostname, string userName, string pass)
        {
            m_hostname = hostname;
            m_userName = userName;
            m_password = pass;
        }

        private FTPInteractor()
        {

        }

        public static  FTPInteractor getInstance(string hostname = null, string userName = null, string pass = null)
        {
            if (objFTPInteractor == null)
                objFTPInteractor = new FTPInteractor(hostname, userName, pass);

            return objFTPInteractor;
        }

        public void WriteToInputFTPFile(List<string> lfilePaths, string fileName, string folderName)
        {        

            foreach(string path in lfilePaths)
                Logger.WriteToLogFile(path, fileName, folderName);
        }

        public void UploadFilesToFTP(List<string> fileNames, string destinationFolder, string sourceFolder)
        {
            using (var ftpClient = new FtpClient())
            {
                var destPath = destinationFolder;
                

                ftpClient.Host = m_hostname;
                ftpClient.Credentials = new NetworkCredential(m_userName, m_password);

                ftpClient.Connect();

                //create destination folder
                ftpClient.CreateDirectory(destPath);

                
                    foreach (string fileName in fileNames)
                    {
                        try
                        {
                            string sourcePath = sourceFolder + "\\" + fileName;
                            using (var fileStream = File.OpenRead(sourcePath))
                            using (var ftpStream = ftpClient.OpenWrite(string.Format("{0}/{1}", destPath, fileName)))
                            {
                                var buffer = new byte[8 * 1024];
                                int count;
                                while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ftpStream.Write(buffer, 0, count);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteToLogFile(ex.Message, Constants.FTPClient_Logs, null);
                        }
                    }
                
                
            }
        }

        public bool CheckforFTPfileExists(string fileName, string folder)
        {
            using (var ftpClient = new FtpClient())
            {
                ftpClient.Host = m_hostname;
                ftpClient.Credentials = new NetworkCredential(m_userName, m_password);

                ftpClient.Connect();


                if (!ftpClient.DirectoryExists(folder))
                    return false;

                ftpClient.SetWorkingDirectory(folder);
                if(ftpClient.FileExists(fileName))
                    return true;
                else
                    return false;
                
              
            }
        }

        //download files from server to local machine
        public void DownloadFolderFromFTPServer(string folderName)
        {
            using (var ftpClient = new FtpClient())
            {
                ftpClient.Host = m_hostname;
                ftpClient.Credentials = new NetworkCredential(m_userName, m_password);

                ftpClient.Connect();

                // List all files with a .txt extension
                foreach (var ftpListItem in
                    ftpClient.GetListing(folderName, FtpListOption.Modify | FtpListOption.Size))
                {
                    var destinationPath = string.Format(@"{0}\{1}", folderName, ftpListItem.Name);

                    using (var ftpStream = ftpClient.OpenRead(ftpListItem.FullName))
                    using (var fileStream = File.Create(destinationPath, (int)ftpStream.Length))
                    {
                        var buffer = new byte[8 * 1024];
                        int count;
                        while ((count = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, count);
                        }

                        // In this example, we're deleting the file after downloading it.
                        ftpClient.DeleteFile(ftpListItem.FullName);
                    }
                }
            }


        }

        public void DeleteFolderFromServer(string folderName)
        {
            using (var ftpClient = new FtpClient())
            {
                ftpClient.Host = m_hostname;
                ftpClient.Credentials = new NetworkCredential(m_userName, m_password);

                ftpClient.Connect();

                ftpClient.DeleteDirectory(folderName, true);
            }
        }

    }
}
