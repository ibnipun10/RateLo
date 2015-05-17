/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_common;

import dealsheel_NodeStructures.ImageObject;
import java.awt.image.BufferedImage;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import javax.imageio.ImageIO;
import sun.misc.BASE64Decoder;


/**
 *
 * @author Nipun
 */
public class Utilities {
    
        public static void SaveImageFromBase64(ImageObject objImage) throws IOException
        {
            try
            {
            FileLogger.WriteToLogFile("SaveImageFromBase64");
            
            BufferedImage image = null;
            byte[] imageByte;
            
            BASE64Decoder decoder = new BASE64Decoder();
            imageByte = decoder.decodeBuffer(objImage.base64ExcodedImage);
            ByteArrayInputStream bis = new ByteArrayInputStream(imageByte);
            image = ImageIO.read(bis);
            
            String ImagePath = GetImagesFolderPath() + "//" + objImage.pathTosaveOnServer;
            
            FileLogger.WriteToLogFile("Compelte Image Path : " + ImagePath);
            CreateDirectory(ImagePath);
            
            FileLogger.WriteToLogFile(ImagePath);
            
            ImageIO.write(image, "jpg", new File(ImagePath + "//" + objImage.fileName));
            bis.close();       
            }
            catch(Exception ex)
            {
                FileLogger.WriteToLogFile((ex.getMessage()));
            }
            
        }
        
        public static void CreateDirectory(String path)
        {
            new File(path).mkdirs();
        }
        
        public static String GetImagesFolderPath()
        {
            String ImagePath = System.getProperty( "catalina.base" ) + "//" + "webapps";
            
            FileLogger.WriteToLogFile("ImagePath : " + ImagePath);
            return ImagePath;
        }
        
        public  static  String CreateQuery(String query, ArrayList<Object> args) {
        query = String.format(query, args.toArray());

        FileLogger.WriteToLogFile(query);
        return query;
    }
}
