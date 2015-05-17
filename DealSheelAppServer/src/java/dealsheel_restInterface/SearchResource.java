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

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path(Constants.APPLICATION_PATH_SEARCH)
public class SearchResource {

    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface;

    /**
     * Creates a new instance of SearchResource
     */
    public SearchResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    }

    /**
     * Retrieves representation of an instance of dealsheel_restInterface.SearchResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_STATEID) String stateId,
            @QueryParam(Constants.QUERYPARAMS_SEARCHTEXT) String searchText,
            @QueryParam(Constants.QUERYPARAMS_SKIP) int skip) {
        //TODO return proper representation object
        return m_Neo4jQueryInterface.GetSearchItems(stateId, skip, searchText);
    }
    
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_SEARCH_STORES)
    public String getSearchStores(@QueryParam(Constants.QUERYPARAMS_ITEMDESCRIPTIONID) String ItemDescrId,
            @QueryParam(Constants.QUERYPARAMS_SEARCHTEXT) String searchText,
            @QueryParam(Constants.QUERYPARAMS_SKIP) int skip) {
        //TODO return proper representation object
        return m_Neo4jQueryInterface.GetSearchStores(ItemDescrId, searchText, skip);
    }

    /**
     * PUT method for updating or creating an instance of SearchResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String content) {
    }
}
