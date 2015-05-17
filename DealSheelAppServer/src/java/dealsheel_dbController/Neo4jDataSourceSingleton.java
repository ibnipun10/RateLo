/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package dealsheel_dbController;

import dealsheel_common.Constants;
import dealsheel_common.FileLogger;
import java.sql.Connection;
import java.sql.SQLException;
import org.apache.tomcat.dbcp.dbcp2.BasicDataSource;

/**
 *
 * @author Nipun
 */
public class Neo4jDataSourceSingleton {
    
    private static Neo4jDataSourceSingleton m_objNeo4jDataSourceSingleton;
    private BasicDataSource ds;
    
    private String GetUrl()
    {      
        return String.format(Constants.NEO4J_JDBC_CONNECTIVITY_STRING, Constants.NEO4J_DB_IPADDRESS, Constants.NEO4J_DB_PORT);
    }
    
    private Neo4jDataSourceSingleton() 
    {
        ds = new BasicDataSource();
        ds.setDriverClassName(Constants.NEO4J_DRIVER);
        ds.setUsername(Constants.NEO4J_USERNAME);
        ds.setPassword(Constants.NEO4J_PASSWORD);
        ds.setUrl(GetUrl());
    }
    
    public static Neo4jDataSourceSingleton GetInstance()
    {
        if(m_objNeo4jDataSourceSingleton == null)
            m_objNeo4jDataSourceSingleton = new Neo4jDataSourceSingleton();
        
        return m_objNeo4jDataSourceSingleton;
    }
    
    public Connection OpenConnection() throws SQLException
    {
        return this.ds.getConnection();
    }
    
    public void CloseConnection(Connection conn) 
    {
        try
        {
        if(conn != null)
            conn.close();
        }
        catch(SQLException ex)
        {
            ex.printStackTrace();
            FileLogger.WriteToLogFile(ex.getMessage());
        }
    }
    
    
    
}
