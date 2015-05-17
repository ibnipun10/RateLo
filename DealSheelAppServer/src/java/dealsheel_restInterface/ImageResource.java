/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_restInterface;

import dealsheel_NodeStructures.ImageObject;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import javax.ws.rs.PathParam;
import javax.ws.rs.Consumes;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.GET;
import javax.ws.rs.Produces;
import com.google.gson.Gson;
import dealsheel_common.Constants;
import dealsheel_common.Utilities;
import java.io.File;
import java.nio.channels.FileLock;
import javax.servlet.ServletContext;
/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path(Constants.APPLICATION_PATH_IMAGE)
public class ImageResource {

    @Context
    private ServletContext context;

    /**
     * Creates a new instance of ImageResource
     */
    public ImageResource() {
    }
    
    
    /**
     * Retrieves representation of an instance of dealsheel_restInterface.ImageResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson() {
        //TODO return proper representation object
        
        
        throw new UnsupportedOperationException();
    }

    /**
     * PUT method for updating or creating an instance of ImageResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public String putJson(String objJson) {
        
        try
        {
        Gson gson = new Gson();
        ImageObject objImage =  gson.fromJson(objJson, ImageObject.class);
        
        //Convert back from base64
        //Then convert to image and save to the path
        Utilities.SaveImageFromBase64(objImage);
        
        }
        catch(Exception ex)
        {
            return ex.getMessage();
        }
        
        return Constants.SUCCESS;
    }
}
