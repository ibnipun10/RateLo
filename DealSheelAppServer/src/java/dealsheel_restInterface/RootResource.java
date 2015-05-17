/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_restInterface;


import com.google.gson.Gson;
import dealsheel_NodeStructures.CView;
import java.sql.Connection;
import javax.servlet.http.HttpServletRequest;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import dealsheel_dbController.Neo4jQueryInterface;
import dealsheel_dbController.Neo4jUpdateInterface;
import dealsheel_common.Constants;

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path("/")
public class RootResource {

    @Context
    private UriInfo context;
    private Connection m_connection;
    private Neo4jQueryInterface m_Neo4jQueryInterface;
    private Neo4jUpdateInterface m_Neo4jUpdateInterface; 

    public RootResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
        m_Neo4jUpdateInterface = new Neo4jUpdateInterface();
    }
    
    /**
     * Retrieves representation of an instance of dealsheel_restInterface.RootResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_ID) String id) {
        //TODO return proper representation object
        return m_Neo4jQueryInterface.GetNodeByid(id);
        
    }

    /**
     * PUT method for updating or creating an instance of RootResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String objJson) {
         Gson gson = new Gson();
        CView objView =  gson.fromJson(objJson, CView.class);
        
        //Increment the view for the subcategory.
        
        m_Neo4jUpdateInterface.UpdateViewsForNode(objView.id);
    }
}
