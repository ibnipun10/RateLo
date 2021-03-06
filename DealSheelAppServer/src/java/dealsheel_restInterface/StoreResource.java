/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_restInterface;

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
@Path(Constants.APPLICATION_PATH_STORE)
public class StoreResource {

    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface;

    /**
     * Creates a new instance of StoreCardResource
     */
    public StoreResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    }

    /**
     * Retrieves representation of an instance of dealsheel_restInterface.StoreCardResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_ITEMDESCRIPTIONID) String itemDescriptionId,
            @QueryParam(Constants.QUERYPARAMS_SKIP) int skip) {
        //TODO return proper representation object
       // return Neo4jQueryInterface.GetStoreCards(categoryName, subCategoryName, stateName);
        return m_Neo4jQueryInterface.GetStoreCards(itemDescriptionId, skip, null);
    }
    
    
    
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_GETTOPVIEWEDSTORES)
    public String getTopViewedStores()
    {       
        return m_Neo4jQueryInterface.GetTopViewedStores();
      
    }
    
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_GETTOPVIEWEDSTORELIST)
    public String getTopViewedStoreList(@QueryParam(Constants.QUERYPARAMS_STOREDESCRIPTIONID) String storeDescriptionId,
            @QueryParam(Constants.QUERYPARAMS_STATEID) String stateId)
    {
        return m_Neo4jQueryInterface.GetTopViewedStoreList(storeDescriptionId, stateId);
    }

    /**
     * PUT method for updating or creating an instance of StoreCardResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String content) {
    }
}
