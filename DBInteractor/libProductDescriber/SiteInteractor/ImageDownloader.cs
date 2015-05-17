using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;

namespace libProductDescriber.SiteInteractor
{
    public class ImageDownloader
    {
        public static void GetImageFromUrlAndSave(string url, string path)
        {
            using (var webClient = new WebClient())
            {
                byte[] fileBytes = webClient.DownloadData(url);
                using (var stream = new MemoryStream(fileBytes))
                {
                    Image img = Image.FromStream(stream);
                    img.Save(path);
                }
            }
                
        }

    }
}
