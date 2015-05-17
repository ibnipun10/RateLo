using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;
using Neo4jClient;

namespace DBInteractor.DBInterface
{
    public class Neo4jController
    {
        private static string ipaddress;
        private static int iPort;
        private static string connectUri;
        public static GraphClient m_graphClient;

        public static void InitializeController(string machineIP, int port)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            ipaddress = machineIP;
            iPort = port;
            connectUri = String.Format("http://{0}:{1}/db/data", ipaddress, iPort);
            m_graphClient = new GraphClient(new Uri(connectUri));
        }

        public static void connect()
        {
            try
            {
                Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

                if (m_graphClient != null)
                    m_graphClient.Connect();
                else
                {
                    throw new Exception("Graph client object is null");
                }
            }
            catch(AggregateException ex)
            {
                throw new Exception("Neo4j client unable to connect to database...plz check ur connection......");
            }
        }

    }  
}
