/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_restInterface;

import com.google.gson.Gson;
import dealsheel_NodeStructures.CView;
import dealsheel_dbController.Neo4jQueryInterface;
import dealsheel_dbController.Neo4jUpdateInterface;
import javax.swing.text.html.ObjectView;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import org.eclipse.jdt.internal.compiler.impl.Constant;
import dealsheel_common.Constants;

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path(Constants.APPLICATION_PATH_SUBCATEGORY)
public class SubCategoryResource {

    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface;
    

    /**
     * Creates a new instance of SubCategoryResource
     */
    public SubCategoryResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    
    }

    /**
     * Retrieves representation of an instance of dealsheel_restInterface.SubCategoryResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_CATEGORYID) String id) {
        //TODO return proper representation object
        
        return m_Neo4jQueryInterface.GetAllSubCategories(id);
    }
    
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_GETTOPVIEWEDSUBCATEGROIES)
    public String getTopViewedSubCategories()
    {       
        return m_Neo4jQueryInterface.GetTopViewedSubCategories();
      
    }

    /**
     * PUT method for updating or creating an instance of SubCategoryResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String objJson) {
       
        
    }
}
