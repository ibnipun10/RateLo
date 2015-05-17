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
import javax.ws.rs.core.MultivaluedMap;
import dealsheel_common.Constants;
import org.eclipse.jdt.internal.compiler.impl.Constant;

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path(Constants.APPLICATION_PATH_FILTER)
public class FilterResource {

    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface;

    /**
     * Creates a new instance of FilterResource
     */
    public FilterResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    }

    /**
     * Retrieves representation of an instance of dealsheel_restInterface.FilterResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_SUBCATEGORYID) String subCategoryId) {
        //TODO return proper representation object
        return m_Neo4jQueryInterface.GetAllFilters(subCategoryId);
    }
    
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_GETFILTERVALUES)
    public String getFilterValues(@QueryParam(Constants.QUERYPARAMS_SUBCATEGORYID) String subCategoryId,
            @QueryParam(Constants.QUERYPARAMS_NAME) String filterName, 
            @QueryParam(Constants.QUERYPARAMS_ISINT) boolean bInt)
    {
        return m_Neo4jQueryInterface.GetFilterValues(subCategoryId, filterName, bInt);
    }
    
    /*
    @QueryParam("SubCategoryid") 
    @QueryParam("Stateid")
    @QueryParam("Name")
    @QueryParam("Value")
    @QueryParam("IsInt")
    */
    @GET
    @Produces("application/json")
    @Path(Constants.APPLICATION_PATH_APPLYFILTERS)
    public String applyFilterValues(@Context UriInfo uriInfo)
    {
        MultivaluedMap<String, String> queryParams =  uriInfo.getQueryParameters();
        return m_Neo4jQueryInterface.ApplyFilterValues(queryParams);
    }

    /**
     * PUT method for updating or creating an instance of FilterResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String content) {
    }
}
