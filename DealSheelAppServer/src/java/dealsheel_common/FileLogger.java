/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package dealsheel_common;

import java.awt.image.BufferedImage;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.sql.Timestamp;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.logging.FileHandler;
import java.util.logging.Handler;
import java.util.logging.Logger;
import java.util.logging.SimpleFormatter;
import dealsheel_common.Constants;
import javax.servlet.http.HttpServletRequest;
/**
 *
 * @author Nipun
 */
public class FileLogger {
    
    private static String m_FilePath;

    public static void InitialzieLog() {
        File theDir = new File(Constants.LOG_FILE_FOLDER);
        if (!theDir.exists()) {
            boolean bDirCreated = theDir.mkdir();

        }
        java.util.Date date = new java.util.Date();
        m_FilePath = theDir + "//" + new Timestamp(date.getTime()).getTime() + Constants.Log_FILETYPE;

    }
    
    

    public static void WriteToLogFile(String line) {
        try {
            if (m_FilePath == null) {
                return;
            }

            File filehandle = new File(m_FilePath);

            if (!filehandle.exists()) {
                filehandle.createNewFile();
            }

            FileWriter fw = new FileWriter(filehandle.getAbsoluteFile(), true);
            BufferedWriter bw = new BufferedWriter(fw);

            DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
            Date date = new Date();
            String newLine = dateFormat.format(date) + " : " + line;

            bw.write(newLine);
            bw.newLine();
            bw.close();
            fw.close();

        } catch (Exception ex) {

        }
    }
    
    public static void WriteToLogFile(HttpServletRequest httpRequest, String line)
    {
        if(httpRequest != null)
        {
            String localIP = httpRequest.getLocalAddr();
            String localName = httpRequest.getLocalName();
            line = localName + "(" + localIP + ") : " + line; 
        }
        
        WriteToLogFile(line);
    }

}
