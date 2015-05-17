using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;
using DBInteractor.DBInterface;

namespace DBInteractor.DBInterface
{
    public class DBUpdateInterface
    {
        public static void UpdateStore(Store objStore)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objStore.getLabel() + " { id : {id}})")
                .OnMatch()
                .Set("A= { objStore }")
                .WithParams(new
                {
                    id = objStore.id,
                    objStore = objStore
                })
                .ExecuteWithoutResults();
        }

        

        public static void UpdateNode<T>(Node objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Node>(objNode);

            if (objNode == null)
                throw new Exception("Node not found..please use \"add row\" to add new  rows");
            
            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objNode.getLabel() + " { id : {NodeId}})")
                .Set("A = { objNode }")
                .WithParams(new
                {
                    NodeId = objNode.id,
                    objNode = objNode,

                })
                .Return(A => new
                {
                    Count = A.Count()
                })
                .Results
                .Single();

            if (result.Count == 1)
                Logger.WriteToLogFile("Successfully created node");
            else
            {
                Logger.WriteToLogFile("Unable to create node");
                throw new Exception("Unable to create node...Either node not found or you are not using \"add row\" to add new rows");
            }

        }
    }
}
