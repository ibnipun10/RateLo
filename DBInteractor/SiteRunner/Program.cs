using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libProductDescriber.SiteInteractor;
using DBInteractor.Common;
using Newtonsoft.Json;

namespace SiteRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            string response = BestBuyInteractor.GetResponse("manufacturer=Samsung&modelNumber=UN60H6400*");
            ItemDescription objItem = new TvItemDescription();
            BestBuyInteractor.DeserializeJson(response, ref objItem);

            ImageDownloader.GetImageFromUrlAndSave(objItem.image, "F:\\test.jpg");
             */

            CViews objViews = new CViews();
            objViews.id = "e82c75ef-3d36-46c1-ab8e-f8b6476d3e7e";
            string json =  JsonConvert.SerializeObject(objViews);

            using (var client = new System.Net.WebClient())
            {
                client.UploadData("http://localhost:8080/DealSheelAppServer/api/", "PUT", Encoding.Default.GetBytes(json));
            }

          //  ImageDownloader.SaveimageToRemoteLocation("F:\\test.jpg");
            
            string test = "nipun";
        }
    }

    class CViews
    {
        public string id;
    }

}
