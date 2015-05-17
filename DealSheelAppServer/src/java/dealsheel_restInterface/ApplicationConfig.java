/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package dealsheel_restInterface;

import dealsheel_common.Constants;
import dealsheel_common.FileLogger;
import dealsheel_dbController.Neo4jDataSourceSingleton;
import java.util.Set;
import javax.ws.rs.core.Application;
import dealsheel_dbController.Neo4jQueryInterface;

/**
 *
 * @author Nipun
 */
@javax.ws.rs.ApplicationPath(Constants.APPLICATION_PATH_API)
public class ApplicationConfig extends Application {

    public ApplicationConfig() {
        //Initialize logger here
        FileLogger.InitialzieLog();
        FileLogger.WriteToLogFile("Logs initialized");

        
        
    }

    @Override
    public Set<Class<?>> getClasses() {
        Set<Class<?>> resources = new java.util.HashSet<>();
        addRestResourceClasses(resources);
        return resources;
    }

    /**
     * Do not modify addRestResourceClasses() method. It is automatically
     * populated with all resources defined in the project. If required, comment
     * out calling this method in getClasses().
     */
    private void addRestResourceClasses(Set<Class<?>> resources) {
        resources.add(dealsheel_restInterface.CategoryResource.class);
        resources.add(dealsheel_restInterface.FilterResource.class);
        resources.add(dealsheel_restInterface.ImageResource.class);
        resources.add(dealsheel_restInterface.ItemResource.class);
        resources.add(dealsheel_restInterface.LocationResource.class);
        resources.add(dealsheel_restInterface.RootResource.class);
        resources.add(dealsheel_restInterface.SearchResource.class);
        resources.add(dealsheel_restInterface.StoreResource.class);
        resources.add(dealsheel_restInterface.SubCategoryResource.class);
    }

}
