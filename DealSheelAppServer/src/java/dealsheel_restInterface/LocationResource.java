
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_restInterface;

import dealsheel_dbController.Neo4jQueryInterface;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import dealsheel_common.Constants;

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path(Constants.APPLICATION_PATH_LOCATION)
public class LocationResource {

    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface;

    /**
     * Creates a new instance of LocationResource
     */
    public LocationResource() {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    }

    /**
     * Retrieves representation of an instance of dealsheel_restInterface.LocationResource
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson(@QueryParam(Constants.QUERYPARAMS_COUNTRYNAME) String countryName) {
        //TODO return proper representation object
        return m_Neo4jQueryInterface.GetAllRegionsByCountryName(countryName);
    }

    /**
     * PUT method for updating or creating an instance of LocationResource
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String content) {
    }
}
