using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;

namespace DBInteractor.DBInterface
{
    public class DBDeleteInterface
    {
        public static void DeleteStore(string storeId)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Store objStore = new Store();

            Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objStore.getLabel() + ")")
                .Match("(A)-[r]-()")
                .Where((Store A) => A.StoreId == storeId)
                .Delete("A, r")
                .ExecuteWithoutResults();
        }

       

        public static void DeleteNode<T>(Node  objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objNode.getLabel() + "{ id : { NodeId} })")
                .Match("(A)-[r]-()")
                .Delete("A, r")
                .WithParams(new
                {
                    NodeId = objNode.id
                })
                .ExecuteWithoutResults();
        }

        public static void DeleteCountryNode(Country objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(A)-[R]-()")
                .OptionalMatch("(A)-[:" + Rel_Country.country_state + "]->(B)")
                .Delete("A,R")
                .With("B")
                .Match("(B)-[R]-()")
                .OptionalMatch("(B)-[:" + Rel_State.state_city + "]->(C)")
                .Delete("B,R")
                .With("C")
                .Match("(C)-[R]-()")
                .OptionalMatch("(C)-[:" + Rel_City.city_store + "]->(D)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Store.store_item + "]->(E)")
                .Delete("D,R")
                .With("E")                
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
            
        }

        public static void DeleteStateNode(State objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());


            Neo4jController.m_graphClient.Cypher
                .Match("(B:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(B)-[R]-()")
                .OptionalMatch("(B)-[:" + Rel_State.state_city + "]->(C)")
                .Delete("B,R")
                .With("C")
                .Match("(C)-[R]-()")
                .OptionalMatch("(C)-[:" + Rel_City.city_store + "]->(D)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Store.store_item + "]->(E)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
        }

        public static void DeleteCityNode(City objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());


            Neo4jController.m_graphClient.Cypher
                .Match("(C:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(C)-[R]-()")
                .OptionalMatch("(C)-[:" + Rel_City.city_store + "]->(D)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Store.store_item + "]->(E)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
        }

        public static void DeleteStoreNode(Store objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());


            Neo4jController.m_graphClient.Cypher
                .Match("(D:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(D)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Store.store_item + "]->(E)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
        }

        public static void DeleteItemNode(Item objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());


            Neo4jController.m_graphClient.Cypher
                .Match("(E:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
        }

        public static void DeleteCategoryNode(Category objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(A)-[R]-()")
                .OptionalMatch("(A)-[:" + Rel_Category.category_subCategory + "]->(B)")
                .Delete("A,R")
                .With("B")
                .Match("(B)-[R]-()")
                .OptionalMatch("(B)-[:" + Rel_SubCategory.subCategory_brand + "]->(C)")
                .Delete("B,R")
                .With("C")
                .Match("(C)-[R]-()")
                .OptionalMatch("(C)-[:" + Rel_Brand.brand_item + "]->(D)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(E)-[:" + Rel_Item.item_itemDescription + "]->(D)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();

        }

        public static void DeleteSubCategoryNode(SubCategory objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(B:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(B)-[R]-()")
                .OptionalMatch("(B)-[:" + Rel_SubCategory.subCategory_brand + "]->(C)")
                .Delete("B,R")
                .With("C")
                .Match("(C)-[R]-()")
                .OptionalMatch("(C)-[:" + Rel_Brand.brand_item + "]->(D)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(E)-[:" + Rel_Item.item_itemDescription + "]->(D)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();

        }

        public static void DeleteBrandNode(Brand objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(C:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(C)-[R]-()")
                .OptionalMatch("(C)-[:" + Rel_Brand.brand_item + "]->(D)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(E)-[:" + Rel_Item.item_itemDescription + "]->(D)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .Delete("E,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();

        }

        public static void DeleteBrandDescriptionNode(BrandDescription objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(C:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(C)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Brand.brand_brandDescription + "]->(C)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Brand.brand_item + "]->(E)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .OptionalMatch("(F)-[:" + Rel_Item.item_itemDescription + "]->(E)")
                .Delete("E,R")
                .With("F")
                .Match("(F)-[R]-()")
                .Delete("F,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();

        }
        public static void DeleteItemDescriptionNode(ItemDescription objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            Neo4jController.m_graphClient.Cypher
                .Match("(C:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(C)-[R]-()")
                .OptionalMatch("(D)-[:" + Rel_Item.item_itemDescription + "]->(C)")
                .Delete("C,R")
                .With("D")
                .Match("(D)-[R]-()")
                .Delete("D,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
         }

        public static void DeleteStoreDescription(StoreDescription objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());


            Neo4jController.m_graphClient.Cypher
                .Match("(D:" + objNode.getLabel() + "{ id : { Nodeid }})")
                .Match("(D)-[R]-()")
                .OptionalMatch("(E)-[:" + Rel_Store.store_storeDescription + "]->(D)")
                .Delete("D,R")
                .With("E")
                .Match("(E)-[R]-()")
                .OptionalMatch("(E)-[:" + Rel_Store.store_item + "]->(F)")
                .Delete("E,R")
                .With("F")
                .Match("(F)-[R]-()")
                .Delete("F,R")
                .WithParams(new
                {
                    Nodeid = objNode.id
                })
                .ExecuteWithoutResults();
        }
    }
}
