using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libScrapper;
using DBInteractor.Common;

namespace ScrapperRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            CFlipkartScrapper objscrapper = new CFlipkartScrapper();
            ItemDescription objItem = new MicrowaveItemDescription();

            objscrapper.SetHTMLDocument("http://www.flipkart.com/samsung-40h5100-40-inches-57900-tv/p/itmdu7ycguexxhzm?pid=TVSDU5QTVF9BX57Z");
            objscrapper.ExtractData(ref objItem);                 
            


            string path = "DealSheelImages//" + objItem.getLabel();
            string fileName = objItem.id + ImageController.GetFileTypeFromImage(objItem.image);
            ImageController.GetImageFromUrlAndSave(objItem.image, fileName);
         
            ImageController.SaveimageToRemoteLocation(path, fileName, "localhost", "8080");

        }
    }
}
