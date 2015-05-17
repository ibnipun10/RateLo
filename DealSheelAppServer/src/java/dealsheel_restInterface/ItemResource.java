/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_restInterface;

import dealsheel_common.Constants;
import dealsheel_dbController.Neo4jQueryInterface;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import javax.ws.rs.PathParam;
import javax.ws.rs.Consumes;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.GET;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import dealsheel_common.Constants;

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path("Item")
public class ItemResource {

    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface; 
    /**
     * Creates a new instance of ItemCardResource
     */
    public ItemResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    }

    /**
     * Retrieves representation of an instance of dealsheel_restInterface.ItemCardResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_SUBCATEGORYID) String subCategoryid,
            @QueryParam(Constants.QUERYPARAMS_STATEID) String stateid,
            @QueryParam(Constants.QUERYPARAMS_SKIP) int skip) {
        //TODO return proper representation object
        return m_Neo4jQueryInterface.GetItemCards(subCategoryid, stateid, skip, null);
    }
    
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_GETTOPVIEWSITEMS)
    public String getTopViewedItems()
    {       
        return m_Neo4jQueryInterface.getTopViewedItems();
      
    }
    
    
    /**
     * PUT method for updating or creating an instance of ItemCardResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String content) {
    }
}
