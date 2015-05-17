using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.Text;

namespace libScrapper
{
    public class ImageObject
    {
        public string base64ExcodedImage {get; set;}
        public string pathTosaveOnServer {get ;set;}

        public string fileName { get; set; }
    }

    public class ImageController
    {
        
            public static void GetImageFromUrlAndSave(string url, string fileName)
            {
                MemoryStream ms = GetStreamFromUrl(url);
                        Image img = Image.FromStream(ms);
                        img.Save(fileName);
               
            }

        public static string GetFileTypeFromImage(string url)
        {
            return Path.GetExtension(url);
        }

        public static MemoryStream GetStreamFromUrl(string url)
            {
                using (var webClient = new WebClient())
                {
                    byte[] fileBytes = webClient.DownloadData(url);
                    return new MemoryStream(fileBytes);                    
                }
            }

        public static void SaveimageToRemoteLocation(string localImagePath, string fileName, string serverName, string port)
        {
            ImageObject objImage = new ImageObject();
            
            objImage.base64ExcodedImage = ConvertImageToBase64(localImagePath + "/" + fileName);
            objImage.pathTosaveOnServer = localImagePath;
            objImage.fileName = fileName;

            string jsonImage = JsonConvert.SerializeObject(objImage);
       

            using(var client = new System.Net.WebClient()) 
            {
                    client.UploadData(GetImageServerPath(serverName, port),"PUT", Encoding.Default.GetBytes(jsonImage));
            }
                
        }

        public static void SaveImageToLocalMachine(string fromLocalPath, string toLocalPath)
        {
            File.Copy(fromLocalPath, toLocalPath, true);
        }

        private static string GetImageServerPath(string appServer, string Port)
        {
            string Uri = "http://" + appServer + ":" + Port + "/DealSheelAppServer/api/Image";
            return Uri;
        }

        private static string ConvertImageToBase64(string localfilePath)
        {
           using (Image image = Image.FromFile(localfilePath))
            {                 
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }                  
            }
        }

        
    }
}
