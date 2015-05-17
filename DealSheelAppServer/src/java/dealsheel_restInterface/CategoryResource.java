/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package dealsheel_restInterface;

import dealsheel_NodeStructures.NodeLabels;
import dealsheel_common.FileLogger;
import dealsheel_dbController.Neo4jDataSourceSingleton;
import dealsheel_dbController.Neo4jQueryInterface;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import javax.servlet.ServletContext;
import javax.servlet.http.HttpServletRequest;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.PUT;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.UriInfo;
import org.eclipse.jdt.internal.compiler.impl.Constant;
import org.neo4j.jdbc.Neo4jColumnMetaData;
import dealsheel_common.Constants;

/**
 * REST Web Service
 *
 * @author Nipun
 */
@Path(Constants.APPLICATION_PATH_CATEGORY)
public class CategoryResource {
    
    @Context
    private UriInfo context;
    private Neo4jQueryInterface m_Neo4jQueryInterface;

    public CategoryResource()
    {
        m_Neo4jQueryInterface = new Neo4jQueryInterface();
    }
    /**
     * Retrieves representation of an instance of
     * dealsheel_restInterface.Category
     *
     * @return an instance of java.lang.String
     */
    @GET
    @Produces("application/json")
    public String getJson() {
        //TODO return proper representation object

        //Get all Categories
        
        return m_Neo4jQueryInterface.GetAllCategories();
    }

    /**
     * PUT method for updating or creating an instance of Category
     *
     * @param content representation for the resource
     * @return an HTTP response with content of the updated or created resource.
     */
    @PUT
    @Consumes("application/json")
    public void putJson(String content) {
    }
}
