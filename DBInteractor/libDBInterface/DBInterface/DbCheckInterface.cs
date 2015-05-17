using DBInteractor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.DBInterface;

namespace DBInteractor.DBInterface
{
    class DbCheckInterface
    {
        public static bool IsCountryPresent(Country objCountry)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            var countryresult = Neo4jController.m_graphClient.Cypher
                .Match("(A : " + objCountry.getLabel() + ")")
                .Where((Country A) => A.Name == objCountry.Name)
                .Return(A => A.As<Country>())
                .Results;

            if (countryresult.Count() > 0)
                return true;
            else
                return false;
        }

      
        public static bool IsStorePresent(Store objStore)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            IEnumerable<Store> storeResult = Neo4jController.m_graphClient.Cypher
                .Match("(A : " + objStore.getLabel() + ")")
                .Where((Store A) => A.StoreId == objStore.StoreId)
                .Return(A => A.As<Store>())
                .Results;

            if (storeResult.Count() > 0)
                return true;
            else
                return false;
        }

        public static bool IsSubCategoryPresent(SubCategory objSubCategory)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A : " + objSubCategory.getLabel() + ")")
                .Where((SubCategory A) => A.Name == objSubCategory.Name)
                .Return(A => A.As<SubCategory>())
                .Results;

            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        public static bool IsCategoryPresent(Category objCategory)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A : " + objCategory.getLabel() + ")")
                .Where((Category A) => A.Name == objCategory.Name)
                .Return(A => A.As<Category>())
                .Results;

            if (result.Count() > 0)
                return true;
            else
                return false;
        }
    }
}
