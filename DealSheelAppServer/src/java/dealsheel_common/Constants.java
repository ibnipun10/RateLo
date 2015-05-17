/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_common;

/**
 *
 * @author Nipun
 */
public class Constants {
    
    // Logger constants 
    public static String LOG_FILE_FOLDER = "Log";
    public static String Log_FILETYPE = ".log";
    public static String NO_RESULTS = "No Result";
    public static String SUCCESS = "Success";
    
    //Neo4j DB Constants
    public static String NEO4J_DRIVER = "org.neo4j.jdbc.Driver";
    public static String NEO4J_DB_IPADDRESS = "localhost";
    public static String NEO4J_DB_PORT = "7474";
    public static String NEO4J_USERNAME = "";
    public static String NEO4J_PASSWORD = "";
    public static String NEO4J_JDBC_CONNECTIVITY_STRING = "jdbc:neo4j://%s:%s/";
    
    
    //Rest Application path
    public static final String APPLICATION_PATH_API = "api";
    public static final String APPLICATION_PATH_CATEGORY = "Category";
    public static final String APPLICATION_PATH_FILTER = "Filter";
    public static final String APPLICATION_PATH_GETFILTERVALUES = "getFilterValues";
    public static final String APPLICATION_PATH_APPLYFILTERS = "applyFilters";
    public static final String APPLICATION_PATH_IMAGE = "Image";
    public static final String APPLICATION_PATH_GETTOPVIEWSITEMS = "getTopViewedItems";
    public static final String APPLICATION_PATH_LOCATION = "Location";
    public static final String APPLICATION_PATH_STORE = "Store";
    public static final String APPLICATION_PATH_GETTOPVIEWEDSTORES = "getTopViewedStores";
    public static final String APPLICATION_PATH_GETTOPVIEWEDSTORELIST = "getTopViewedStoreList";
    public static final String APPLICATION_PATH_SUBCATEGORY = "SubCategory";
    public static final String APPLICATION_PATH_GETTOPVIEWEDSUBCATEGROIES = "getTopViewedSubCategories";
    public static final String APPLICATION_PATH_SEARCH = "Search";
    public static final String APPLICATION_PATH_SEARCH_STORES = "getSearchStores";
    
    
    //Query Paramaters
    public static final String QUERYPARAMS_SUBCATEGORYID = "SubCategoryId";
    public static final String QUERYPARAMS_NAME = "Name";
    public static final String QUERYPARAMS_VALUE = "Value";
    public static final String QUERYPARAMS_ISINT = "isInt";
    public static final String QUERYPARAMS_STATEID = "Stateid";
    public static final String QUERYPARAMS_COUNTRYNAME = "CountryName";
    public static final String QUERYPARAMS_ID = "id";
    public static final String QUERYPARAMS_ITEMDESCRIPTIONID = "ItemDescriptionId";
    public static final String QUERYPARAMS_CATEGORYID = "Categoryid";
    public static final String QUERYPARAMS_SEARCHTEXT = "SearchText";
    public static final String QUERYPARAMS_SKIP = "Skip";
    public static final String QUERYPARAMS_STOREDESCRIPTIONID = "StoreDescriptionId";
    
    public static final int LIMIT_LIST = 15;
    public static final int LIMIT_TOPVIEWED = 5;
   
}
