/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_dbController;

import com.google.gson.Gson;
import dealsheel_common.Constants;
import dealsheel_common.FileLogger;
import dealsheel_common.Utilities;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;

/**
 *
 * @author Nipun
 */
public class Neo4jUpdateInterface {
    private  Neo4jDataSourceSingleton m_Neo4jDataSource;
    
    public Neo4jUpdateInterface()
    {
        m_Neo4jDataSource = Neo4jDataSourceSingleton.GetInstance();
    }
    
    public void UpdateViewsForNode(final String nodeid)
    {
        String retValue = Constants.NO_RESULTS;
        Connection conn = null;
        

        try {
            if(nodeid == null)
                throw new Exception("Node id is null in UpdateViewsForNode");
            
            conn = m_Neo4jDataSource.OpenConnection();

            Statement stmt = conn.createStatement();

            String query = "MATCH (n) WHERE n.id = \'%s\'";
            

            ArrayList<Object> arrayList = new ArrayList<Object>() {
                {
                    add(nodeid);
                }
            };
            query += " " + "SET n.Views = n.Views + 1";
            
            query = Utilities.CreateQuery(query, arrayList);
            ResultSet rs = stmt.executeQuery(query);
            

        } catch (Exception ex) {
            FileLogger.WriteToLogFile(ex.getMessage());
            ex.printStackTrace();
        } finally {
            m_Neo4jDataSource.CloseConnection(conn);
        }        
    }
            
}
