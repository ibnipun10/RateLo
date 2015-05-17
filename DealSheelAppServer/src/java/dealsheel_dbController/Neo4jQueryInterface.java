/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package dealsheel_dbController;

import com.google.gson.Gson;
import dealsheel_NodeStructures.CLabel;
import dealsheel_NodeStructures.ItemCardClass;
import dealsheel_NodeStructures.NodeLabels;
import dealsheel_NodeStructures.RelationClass;
import dealsheel_NodeStructures.StoreCardClass;
import dealsheel_common.Constants;
import dealsheel_common.FileLogger;
import dealsheel_common.Utilities;
import java.lang.reflect.Field;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import javax.ws.rs.core.MultivaluedMap;
import org.eclipse.jdt.internal.compiler.impl.Constant;
import org.neo4j.cypherdsl.grammar.ForEach;

/**
 *
 * @author Nipun
 */
public class Neo4jQueryInterface {

    private  Neo4jDataSourceSingleton m_Neo4jDataSource;
    
    public Neo4jQueryInterface()
    {
        InitializeNeo4jObject();
    }

    public void InitializeNeo4jObject() {
        m_Neo4jDataSource = Neo4jDataSourceSingleton.GetInstance();
    }

    

    public  String GetNodeByid(final String id) {
           
        String query = "MATCH (n) WHERE n.id = \'%s\' RETURN n";

        ArrayList arrayList = new ArrayList();
        arrayList.add(id);
        query = Utilities.CreateQuery(query, arrayList);

        return ExecuteQuery(query);
    }
    
    public String ExecuteQuery(String query)
    {
        String retValue = Constants.NO_RESULTS;
        Connection conn = null;
    
        try
        {      
        
            conn = m_Neo4jDataSource.OpenConnection();
            Statement stmt = conn.createStatement();

            ResultSet rs = stmt.executeQuery(query);
            ArrayList arrayValue = new ArrayList();
            while (rs.next()) {
                    arrayValue.add(rs.getObject(1));
            }

            if(arrayValue.size() != 0)
                retValue = new Gson().toJson(arrayValue);
        } catch (Exception ex) {
                FileLogger.WriteToLogFile(ex.getMessage());
                ex.printStackTrace();
        } finally {
                m_Neo4jDataSource.CloseConnection(conn);
        }
        
        return retValue;

    }

    
    public  String GetAllObjectsByNodeLabel(final String label) {
        
        String query = "MATCH (n:%s) RETURN n";

        ArrayList arrayList = new ArrayList();
        arrayList.add(label);
        query = Utilities.CreateQuery(query, arrayList);
        
        return ExecuteQuery(query);
    }
    
   
    public  String GetAllSubCategories(String... args) {

        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (n:%s)";
        arrayValues.add(NodeLabels.SUBCATEGORY_LABEL);
        String returnStatement = "return n";
        
        if (args[0] != null) {
            arrayValues.add(RelationClass.Rel_Category.category_subCategory);
            arrayValues.add(NodeLabels.CATEGORY_LABEL);
            arrayValues.add(args[0]);
            String CategoryNameQuery = "MATCH (n)<-[:%s]-(:%s { id   : \'%s\' })";
                       
            query += " " + CategoryNameQuery;
        }

        query += " " + returnStatement;
        query = Utilities.CreateQuery(query, arrayValues);

        return ExecuteQuery(query);

    }

    public  String GetAllCategories() {
        return GetAllObjectsByNodeLabel(NodeLabels.CATEGORY_LABEL);
    }
    
    
    //If country name is null it will return all states;
    public  String GetAllRegionsByCountryName(String... args)
    {
        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (n:%s)";
        arrayValues.add(NodeLabels.STATE_LABEL);
        String returnStatement = "return n";

        if (args[0] != null) {
            arrayValues.add(RelationClass.Rel_Country.country_state);
            arrayValues.add(NodeLabels.COUNTRY_LABEL);
            arrayValues.add(args[0]);
            String CategoryNameQuery = "MATCH (n)<-[:%s]-(:%s { Name : \'%s\' })";
                       
            query += " " + CategoryNameQuery;
        }

        query += " " + returnStatement;
        query = Utilities.CreateQuery(query, arrayValues);

        return ExecuteQuery(query);
    }
    
    public String GetSearchStores(String itemDescId, String searchText, int skip)
    {
        String whereQuery = "where D.Name =~ '(?i).*%s.*' ";
        
        ArrayList arrayParams = new ArrayList();
        arrayParams.add(searchText);
        arrayParams.add(searchText);
        
        whereQuery = Utilities.CreateQuery(whereQuery, arrayParams);
        
        return GetStoreCards(itemDescId, skip, whereQuery);
    }
    
    public String GetTopViewedStoreList(String storeDescriptionId, String stateid)
    {
        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (A:%s { id : '%s'}) "                                                         +   //Store Description
                       "MATCH (B)-[: " + RelationClass.Rel_Store.store_storeDescription + "]->(A) "       +   //Store->Store Description
                       "MATCH (C: %s { id : '%s'})-[: " + RelationClass.Rel_State.state_city + "]->(D) "  +   //State->city
                       "MATCH (D)-[: " + RelationClass.Rel_City.city_store  + "]->(B) "                    +   //City->Store
                       "RETURN (B)";
        
       arrayValues.add(NodeLabels.STORE_DESCRIPTION);
       arrayValues.add(storeDescriptionId);
       arrayValues.add(NodeLabels.STATE_LABEL);
       arrayValues.add(stateid);
       
       query = Utilities.CreateQuery(query, arrayValues);
       return ExecuteQuery(query);       
       
    }
    
    public  String GetStoreCards(String ItemDescriptionid, int skip, String whereQuery)
    {
       String query = "MATCH (A:%s { id : '%s'}) " +                                                //ItemDescriptionid
                      "MATCH (B)-[:" + RelationClass.Rel_Item.item_itemDescription + "]->(A) "   +   //Item->ItemDesscription      
                      "MATCH (C)-[:" + RelationClass.Rel_Store.store_item + "]->(B) "            +   //Store->Item
                      "MATCH (C)-[:" + RelationClass.Rel_Store.store_storeDescription + "]->(D) ";   //Store->StoreDescription
        
       
       ArrayList arrayParams = new ArrayList();
       arrayParams.add(NodeLabels.ITEM_DESCRIPTION);
       arrayParams.add(ItemDescriptionid);
       
       if(whereQuery != null)
           query += whereQuery;
       
       query += "RETURN C as Store, " + 
                "B as Item, " + 
                "HEAD(labels(D)) as label, "   +
                "D as StoreDescription "      +
                "ORDER BY B.Price "            + 
                "SKIP " + skip                 +
                " LIMIT " + Constants.LIMIT_LIST;
       
       String retValue = Constants.NO_RESULTS;
       Connection conn = null;
       
        try {
            
            query = Utilities.CreateQuery(query, arrayParams);
            
            conn = m_Neo4jDataSource.OpenConnection();
                
            Statement stmt = conn.createStatement();

            ResultSet rs = stmt.executeQuery(query);

            ArrayList arrayValue = new ArrayList();
            while (rs.next()) {
                StoreCardClass objStoreCard = new StoreCardClass();
                objStoreCard.objStore = rs.getObject(1);
                objStoreCard.objItem = rs.getObject(2);
                objStoreCard.label = rs.getString(3);
                objStoreCard.objStoreDescription = rs.getObject(4);
                arrayValue.add(objStoreCard);
            }

            if(arrayValue.size() != 0)
                retValue = new Gson().toJson(arrayValue);

        } catch (Exception ex) {
            FileLogger.WriteToLogFile(ex.getMessage());
            ex.printStackTrace();
        } finally {
            m_Neo4jDataSource.CloseConnection(conn);
        }

        return retValue;
    
    }
    
     public String ApplyFilterValues(MultivaluedMap<String,String> mapValues)
    {        
        //Use F for ItemDescription as in GetItemCards we use F.
        FileLogger.WriteToLogFile("In ApplyFilterValues");
        
        String SubCategoryId = mapValues.getFirst(Constants.QUERYPARAMS_SUBCATEGORYID);
        String Stateid = mapValues.getFirst(Constants.QUERYPARAMS_STATEID);
        
        int skip = 0;
        if(mapValues.getFirst(Constants.QUERYPARAMS_SKIP) != null)
            skip = Integer.parseInt(mapValues.getFirst(Constants.QUERYPARAMS_SKIP)); 
        
        List<String> lName = mapValues.get(Constants.QUERYPARAMS_NAME);
        List<String> lValues = mapValues.get(Constants.QUERYPARAMS_VALUE);
        List<String> lIsInt = mapValues.get(Constants.QUERYPARAMS_ISINT);
        
        FileLogger.WriteToLogFile("Name = " + lName.size() + " Values = " + lValues.size() + " IsInt = " + lIsInt.size());
        
        String whereQuery = "where ";
        for(int index = 0; index < lName.size(); index++)
        {
            String Name = lName.get(index);
            FileLogger.WriteToLogFile("Name = " + Name);

            String value = lValues.get(index);
            FileLogger.WriteToLogFile("Value = " + value);
            
            boolean isInt = Boolean.parseBoolean(lIsInt.get(index));
            FileLogger.WriteToLogFile("IsInt = " + isInt);
            List<String> valuesArray = Arrays.asList(value.split(","));
            
            if(isInt)
            {
                for(int i = 0; i< valuesArray.size(); i++)
                {
                    List<String> rangeArray = Arrays.asList(valuesArray.get(i).split("-"));                    
                    String condition = "TOINT(HEAD(SPLIT(TOSTRING(F.%s), ' '))) ";
                    
                    condition = String.format(condition, Name, valuesArray.get(i));
                    
                    whereQuery += condition + " >= " + rangeArray.get(0) + " and ";
                    whereQuery += condition + " <= " + rangeArray.get(1);
                    
                    if(i !=  (valuesArray.size() - 1))
                        whereQuery += " or ";               
                }
            }
            else
            {
                for(int i =0; i< valuesArray.size(); i++)
                {
                    whereQuery += "F.%s = '%s' ";
                    if(i !=  (valuesArray.size() - 1))
                        whereQuery += " or ";
                    whereQuery = String.format(whereQuery, Name, valuesArray.get(i));
                }
                
            }
            
            if(index != (lName.size() -1 ))
                whereQuery += " and ";
        }
        
        FileLogger.WriteToLogFile(whereQuery);
        return GetItemCards(SubCategoryId, Stateid, skip, whereQuery);
          
    }
     
    
     
    public String GetSearchItems(String Stateid, int skip, String searchText)
    {
        String whereQuery = "where F.Brand =~ '(?i).*%s.*' OR "  +
                            "F.Model_Name =~ '(?i).*%s.*' ";
        
        ArrayList arrayParams = new ArrayList();
        arrayParams.add(searchText);
        arrayParams.add(searchText);
        
        whereQuery = Utilities.CreateQuery(whereQuery, arrayParams);
        
        return GetItemCards(null, Stateid, skip, whereQuery);
    }
    
    public  String GetItemCards(String SubCateogryid, String Stateid, int skip, String whereQuery)
    {
        ArrayList arrayParams = new ArrayList();
        
       String query = "MATCH (A:%s { id : '%s'}) " +      //Stateid                     
                      "MATCH (A)-[:" + RelationClass.Rel_State.state_city + "]->(C) "                +   //State->City
                      "MATCH (C)-[:" + RelationClass.Rel_City.city_store + "]->(D) "                 +   //City->Store
                      "MATCH (D)-[:" + RelationClass.Rel_Store.store_item + "]->(E) "                +   //Store->Item
                      "MATCH (E)-[:" + RelationClass.Rel_Item.item_itemDescription + "]->(F) "       +   //Item->ItemDescription
                      "MATCH (G)-[:" + RelationClass.Rel_Brand.brand_item + "]->(F) ";                  //Brand->ItemDescription
                              
       arrayParams.add(NodeLabels.STATE_LABEL);
       arrayParams.add(Stateid);
       
       if(SubCateogryid != null)    
       {
            query +="MATCH (B:%s { id : '%s'}) " +      //SubCategory id 
                    "MATCH (B)-[:" + RelationClass.Rel_SubCategory.subCategory_brand + "]->(G) ";   //SubCateogry->Brand
            
            arrayParams.add(NodeLabels.SUBCATEGORY_LABEL);
            arrayParams.add(SubCateogryid);
       
       }
       
       if(whereQuery != null)
           query += whereQuery + " ";
       
       //Return Query
       query +=       "RETURN F.id as id, " + 
                      "F.Model_Name as ModelNumber, " + 
                      "F.Brand as Brand, " + 
                      "min(E.Price) as MinPrice, " + 
                      "Count(D) as StoreCount, " + 
                      "HEAD(labels(F)) as label " +
                      "SKIP " + skip                 +
                      " LIMIT "  + Constants.LIMIT_LIST ;
             
       
       String retValue = Constants.NO_RESULTS;
       Connection conn = null;

        try {
            conn = m_Neo4jDataSource.OpenConnection();
            
            query = Utilities.CreateQuery(query, arrayParams);
            Statement stmt = conn.createStatement();

            ResultSet rs = stmt.executeQuery(query);

            ArrayList arrayValue = new ArrayList();
            while (rs.next()) {
                ItemCardClass objStoreCard = GetClassObjectFromResultSet(rs, ItemCardClass.class);
                arrayValue.add(objStoreCard);
            }

            if(arrayValue.size() != 0)
                retValue = new Gson().toJson(arrayValue);

        } catch (Exception ex) {
            FileLogger.WriteToLogFile(ex.getMessage());
            ex.printStackTrace();
        } finally {
            m_Neo4jDataSource.CloseConnection(conn);
        }

        return retValue;


    }
    
    
    
    public String GetTopViewedStores()
    {
        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (n:%s) " + 
                       "MATCH (n)-[:%s]->(b) " +
                       "WITH b, n " + 
                       "ORDER BY n.Views DESC " +
                       "RETURN DISTINCT b as Store, HEAD(labels(b)) as label " +
                       "LIMIT " + Constants.LIMIT_TOPVIEWED;
        
        arrayValues.add(NodeLabels.STORE_LABEL);
        arrayValues.add(RelationClass.Rel_Store.store_storeDescription);
        
        String retValue = Constants.NO_RESULTS;
        Connection conn = null;
      
        query = Utilities.CreateQuery(query, arrayValues);

        try {
            conn = m_Neo4jDataSource.OpenConnection();

            Statement stmt = conn.createStatement();

            ResultSet rs = stmt.executeQuery(query);

            ArrayList arrayValue = new ArrayList();
            while (rs.next()) {
                CLabel objlabel = new CLabel();
                objlabel.objclass = rs.getObject(1);
                objlabel.label = rs.getString(2);
                arrayValue.add(objlabel);
            }

            if(arrayValue.size() != 0)
                retValue = new Gson().toJson(arrayValue);

        } catch (Exception ex) {
            FileLogger.WriteToLogFile(ex.getMessage());
            ex.printStackTrace();
        } finally {
            m_Neo4jDataSource.CloseConnection(conn);
        }

        return retValue;

    }
    
     public String getTopViewedItems()
    {
        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (n:%s) " +                        
                       "RETURN n as Item, HEAD(labels(n)) as label " + 
                       "ORDER BY n.Views DESC " +
                       "LIMIT " + Constants.LIMIT_TOPVIEWED;
        
        arrayValues.add(NodeLabels.ITEM_DESCRIPTION);
        
        
        String retValue = Constants.NO_RESULTS;
        Connection conn = null;
      
        query = Utilities.CreateQuery(query, arrayValues);

        try {
            conn = m_Neo4jDataSource.OpenConnection();

            Statement stmt = conn.createStatement();

            ResultSet rs = stmt.executeQuery(query);

            ArrayList arrayValue = new ArrayList();
            while (rs.next()) {
                CLabel objlabel = new CLabel();
                objlabel.objclass = rs.getObject(1);
                objlabel.label = rs.getString(2);
                arrayValue.add(objlabel);
            }

            if(arrayValue.size() != 0)
                retValue = new Gson().toJson(arrayValue);

        } catch (Exception ex) {
            FileLogger.WriteToLogFile(ex.getMessage());
            ex.printStackTrace();
        } finally {
            m_Neo4jDataSource.CloseConnection(conn);
        }

        return retValue;

    }
     
     public String GetTopViewedSubCategories()
    {
        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (n:%s) " +                        
                       "RETURN n as SubCategory, HEAD(labels(n)) as label " +
                       "ORDER BY n.Views DESC " +
                       "LIMIT " + Constants.LIMIT_TOPVIEWED;
        
        arrayValues.add(NodeLabels.SUBCATEGORY_LABEL);
        
        
        String retValue = Constants.NO_RESULTS;
        Connection conn = null;
      
        query = Utilities.CreateQuery(query, arrayValues);

        try {
            conn = m_Neo4jDataSource.OpenConnection();

            Statement stmt = conn.createStatement();

            ResultSet rs = stmt.executeQuery(query);

            ArrayList arrayValue = new ArrayList();
            while (rs.next()) {
                CLabel objlabel = new CLabel();
                objlabel.objclass = rs.getObject(1);
                objlabel.label = rs.getString(2);
                arrayValue.add(objlabel);
            }

            if(arrayValue.size() != 0)
                retValue = new Gson().toJson(arrayValue);

        } catch (Exception ex) {
            FileLogger.WriteToLogFile(ex.getMessage());
            ex.printStackTrace();
        } finally {
            m_Neo4jDataSource.CloseConnection(conn);
        }

        return retValue;

    }
     
    public  String GetAllFilters(String subCategoryid) {
        return GetAllChildren(subCategoryid, RelationClass.Rel_SubCategory.subCategory_filter);
    }
    
    public  String GetAllChildren(String id, String rel) {

        ArrayList arrayValues = new ArrayList();
        
        String query = "MATCH (n { id: '%s'}) "   +   //SubCategory
                       "MATCH (n)-[:%s]->(a) "        +   //SubCategory->filter
                       "RETURN a";
        
        

        arrayValues.add(id);
        arrayValues.add(rel);
                
        query = Utilities.CreateQuery(query, arrayValues);

        return ExecuteQuery(query);

    }
    
    public String GetFilterValues(String SubCategoryId, String filterName, boolean isInt)
    {
        String query = "";
        ArrayList arrayValues = new ArrayList();
        
        if(isInt)
        {
            query = "Match (n:%s) "                                                         +   //ItemDescription
                    "MATCH (A {id : '%s'})-[:%s]->()-[:%s]->(n) "                           +   //SubCategory->Brand->ItemDescription
                    "WITH DISTINCT n.%s AS filterType "                                     +
                    "WHERE filterType IS NOT NULL "                                         +
                    "WITH MIN(TOINT(HEAD(SPLIT(TOSTRING(filterType), \' \')))) AS IntMin, " +
                    "MAX(TOINT(HEAD(SPLIT(TOSTRING(filterType), \' \')))) AS IntMax "       +
                    "RETURN [IntMin, IntMax] AS COLLECTION";
            
        }
        else
        {
            query = "MATCH (n:%s) "                                                         +   //ItemDescription 
                    "MATCH (A {id : '%s'})-[:%s]->()-[:%s]->(n) "                           +   //SubCategory->Brand->ItemDescription
                    "RETURN DISTINCT n.%s";
        }
        
        
        arrayValues.add(NodeLabels.ITEM_DESCRIPTION);
        arrayValues.add(SubCategoryId);
        arrayValues.add(RelationClass.Rel_SubCategory.subCategory_brand);
        arrayValues.add(RelationClass.Rel_Brand.brand_item);
        arrayValues.add(filterName);
        
        
        query = Utilities.CreateQuery(query, arrayValues);
        return ExecuteQuery(query);
    }
    
   

        
    private  <T> T GetClassObjectFromResultSet(ResultSet rs, Class<T> cls) throws InstantiationException, IllegalAccessException, SQLException
    {
        
        T retObj = cls.newInstance();
        
        Field[] fields = cls.getFields();
        
        for(Field field : fields)
        {
            String fieldName = field.getName();
            if(field.getType().equals(String.class))
                field.set(retObj, rs.getString(fieldName));
            if(field.getType().equals(int.class))
                field.set(retObj, rs.getInt(fieldName));
            if(field.getType().equals(float.class))
                field.set(retObj, rs.getFloat(fieldName));
        }
        
        return retObj;
    }
}
