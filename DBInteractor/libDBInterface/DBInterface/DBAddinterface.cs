using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;

namespace DBInteractor.DBInterface
{
    public class DBAddinterface
    {
        public static void CreateWorldNode(World objWorld)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<World>(objWorld);

            Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objWorld.getLabel() + "{ Name : {Name} })")
                .OnCreate()
                .Set("A = { objWorld }")
                .WithParams(new
                {
                    Name = objWorld.Name,
                    objWorld = objWorld
                })
                .ExecuteWithoutResults();
        }

        public static void CreateCountryNode(Country objCountry)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Country>(objCountry);

            
            World objWorld = new World();

            var result = Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objWorld.getLabel() + " { Name : {Name} })")
                .OnCreate()
                .Set("A = { objWorld }")
                .Merge("(B:" + objCountry.getLabel() + "{ Name : {objCountryName}})")
                .OnCreate()
                .Set("B = { objCountry }")
                .OnMatch()
                .Set("B = { objCountry }")
                .Merge("(A)-[R:" + Rel_World.world_country + "]->(B)")
                .WithParams(new
                {
                    Name = objWorld.Name,
                    objCountryName = objCountry.Name,
                    objCountry = objCountry,
                    objWorld = objWorld
                })
                .Return((B, R) => new
                {
                    CountryCount = B.Count(),
                    RelationCount = R.Count()
                })
                .Results
                .Single();

            if (result.CountryCount == 1)
                Logger.WriteToLogFile("Successfully created country");
            else
                Logger.WriteToLogFile("Unable to create country");

        }

        public static void CreateStateNode(StateWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<State>(objWrap.objState);

            Country objcountry = objWrap.objCountry;
            State objState = objWrap.objState;

            if (objcountry == null)
                throw new Exception("Country is null");
            

            var result = Neo4jController.
                m_graphClient.Cypher
                .Match("(A:" + objcountry.getLabel() + " { id : {id} })")
                .Merge("(A)-[R:" + Rel_Country.country_state + "]->(B:" + objState.getLabel() + " { Name : {StateName}})")
                .OnCreate()
                .Set("B = {objState}")
                .OnMatch()
                .Set("B = {objState}")
                .WithParams(new
                {
                    id = objcountry.id,
                    objState = objState,
                    StateName = objState.Name
                })
                .Return((B, R) => new
                {
                    StateCount = B.Count(),
                    RelationCount = R.Count()
                })
                .Results
                .Single();

            if (result.StateCount == 1)
                Logger.WriteToLogFile("Successfully created State");
            else
                Logger.WriteToLogFile("Unable to create State");

        }

        public static void CreateCityNode(CityWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<City>(objWrap.objCity);

            
            City objCity = objWrap.objCity;
            State objState = objWrap.objState;

            if (objState == null)
            {
                Logger.WriteToLogFile("Unable to find State...No creating city");
                throw new Exception("State is null");
            }

            var result = Neo4jController.
                m_graphClient.Cypher
                .Match("(A:" + objState.getLabel() + " { id : {Stateid }})")
                .Merge("(A)-[R:" + Rel_State.state_city + "]->(B:" + objCity.getLabel() + "{ Name : {cityName} })")
                .OnCreate()
                .Set("B = { objCity}")
                .WithParams(new
                {
                    Stateid = objState.id,
                    objCity = objCity,
                    cityName = objCity.Name
                })
                .Return((B, R) => new
                {
                    StateCount = B.Count(),
                    RelationCount = R.Count()
                })
                .Results
                .Single();

            if (result.StateCount == 1)
                Logger.WriteToLogFile("Successfully created City");
            else
                Logger.WriteToLogFile("Unable to create City");

        }

        public static void CreateStoreNode(StoreWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Store>(objWrap.objStore);

            
            City objCity = objWrap.objCity;
            Store objStore = objWrap.objStore;
            StoreDescription objStoreDesc = new StoreDescription();
            objStoreDesc.Name = objStore.Name;
            objStore.Name = null;
          
            var result = Neo4jController.m_graphClient.Cypher
               .Match("(A:" + objCity.getLabel() + " { id : {Cityid }})")
               .Match("(C:" + objStoreDesc.getLabel() + " { Name : { Name }})")
               .Merge("(A)-[R:" + Rel_City.city_store + "]->(B:" + objStore.getLabel() + "{ StoreId : { storeid} })")
               .OnCreate()
               .Set("B = { objStore }")
               .Merge("(B)-[:" + Rel_Store.store_storeDescription + "]->(C)")
               .WithParams(new
               {
                   Cityid = objCity.id,
                   objStore = objStore,
                   storeid = objStore.StoreId,
                   Name = objStoreDesc.Name
               })
               .Return(B => B.As<Store>())
               .Results;


            if (result.Count() != 0)
                Logger.WriteToLogFile("Successfully created Store");
            else
                Logger.WriteToLogFile("Unable to create Store");
        }

        public static void CreateCategoryRoot(CategoryRoot objCategoryRoot)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<CategoryRoot>(objCategoryRoot);

            Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objCategoryRoot.getLabel() + "{ Name : {Name} })")
                .OnCreate()
                .Set("A = { objCategoryRoot }")
                .WithParams(new
                {
                    Name = objCategoryRoot.Name,
                    objCategoryRoot = objCategoryRoot
                })
                .ExecuteWithoutResults();
        }

        public static void CreateCategoryNode(Category objCategory)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Category>(objCategory);

            CategoryRoot objRoot = new CategoryRoot();

            var result = Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objRoot.getLabel() + " { Name : {Name} })")
                .OnCreate()
                .Set("A = { objRoot }")
                .Merge("(A)-[R:" + Rel_CategoryRoot.categoryroot_category + "]->(B:" + objCategory.getLabel() + "{ Name : {CategoryName} })")
                .OnCreate()
                .Set("B = { objCategory }")
                .OnMatch()
                .Set("B = {objCategory }")
                .WithParams(new
                {
                    Name = objRoot.Name,
                    objCategory = objCategory,
                    objRoot = objRoot,
                    CategoryName = objCategory.Name

                })
                .Return((B, R) => new
                {
                    CategoryCount = B.Count(),
                    RelationCount = R.Count()
                })
                .Results
                .Single();

            if (result.CategoryCount == 1)
                Logger.WriteToLogFile("Successfully created category");
            else
                Logger.WriteToLogFile("Unable to create category");
        }

        public static void CreateSubCategoryNode(SubCategoryWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<SubCategory>(objWrap.objSubCategory);

            Category objCategory = objWrap.objCategory;
            SubCategory objSubCategory = objWrap.objSubCategory;

            if (objCategory == null)
            {
                Logger.WriteToLogFile("Category not found");
                throw new Exception("Cateogry is null");
            }

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objCategory.getLabel() + " { id : { id }})")
                .Merge("(A)-[R:" + Rel_Category.category_subCategory + "]->(B:" + objSubCategory.getLabel() + "{ Name : { Name }})")
                .OnCreate()
                .Set("B = { objSubCategory }")
                .OnMatch()
                .Set("B = { objSubCategory }")
                .WithParams(new
                {
                    id = objCategory.id,
                    objSubCategory = objSubCategory,
                    Name = objSubCategory.Name
                })
                .Return((B, R) => new
                {
                    SubCategoryCount = B.Count(),
                    RelationCount = R.Count()
                })
                .Results
                .Single();

            if (result.SubCategoryCount == 1)
                Logger.WriteToLogFile("Successfully created Subcategory");
            else
                Logger.WriteToLogFile("unable to create Subcategory");

        }

        public static void CreateBrandNode(BrandWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Brand>(objWrap.objBrand);

            Brand objBrand = objWrap.objBrand;
            SubCategory objSubCategory = objWrap.objSubCategory;
            BrandDescription objbrandDesc = new BrandDescription();
            objbrandDesc.Name = objBrand.Name;
            objBrand.Name = null;

            if (objSubCategory == null)
            {
                Logger.WriteToLogFile("Sub Category not found");
                throw new Exception("Sub Category is null");
            }

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objSubCategory.getLabel() + " { id : { id }})")
                .Match("(B:" + objbrandDesc.getLabel() + " { Name : { Name }})")
                .Merge("(A)-[R:" + Rel_SubCategory.subCategory_brand + "]->(C:" + objBrand.getLabel() + "{ Name : { Name }})")
                .OnCreate()
                .Set("C = { objBrand }")
                .OnMatch()
                .Set("B = { objBrand }")
                .Merge("(C)-[R2:" + Rel_Brand.brand_brandDescription + "]->(B)")
                .WithParams(new
                {
                    id = objSubCategory.id,
                    objBrand = objBrand,
                    Name = objbrandDesc.Name
                })
                .Return((B, R) => new
                {
                    Count = B.Count(),
                    RelationCount = R.Count()
                })
                .Results
                .Single();

            if (result.Count == 1)
                Logger.WriteToLogFile("Successfully created brand");
            else
                Logger.WriteToLogFile("unable to create brand");
        }

        /*
         * This created a brandroot-[has_brand]->brand relation and nodes
         * 
         */
        public static BrandDescription CreateBrandDescriptionNode(BrandDescription objBrand)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<BrandDescription>(objBrand);

            BrandRoot objRoot = new BrandRoot();


            var result = Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objRoot.getLabel() + " { Name : { Rootname}} )")
                .OnCreate()
                .Set("A = { objRoot}")
                .Merge("(A)-[R:" + Rel_BrandRoot.brandroot_brand + "]->(B:" + objBrand.getLabel() + "{ Name : { Name }})")
                .OnCreate()
                .Set("B = { objBrand }")
                .WithParams(new
                {
                    Rootname = objRoot.Name,
                    objRoot = objRoot,
                    objBrand = objBrand,
                    Name = objBrand.Name
                })
                .Return((B, R) => new
                {
                    BrandCount = B.Count(),
                    RelationCount = R.Count(),
                    retObj = B.As<BrandDescription>()
                })
                .Results
                .Single();

            if (result.BrandCount == 1)
            {
                Logger.WriteToLogFile("Successfully created Brand");
                objBrand = result.retObj;
            }
            else
            {
                Logger.WriteToLogFile("unable to create Brand");
                objBrand = null;
            }

            return objBrand;

        }


        public static StoreDescription CreateStoreDescriptionNode(StoreDescription objNode)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<StoreDescription>(objNode);

            StoreRoot objRoot = new StoreRoot();


            var result = Neo4jController.m_graphClient.Cypher
                .Merge("(A:" + objRoot.getLabel() + " { Name : { Rootname}} )")
                .OnCreate()
                .Set("A = { objRoot}")
                .Merge("(A)-[R:" + Rel_StoreRoot.storeroot_store + "]->(B:" + objNode.getLabel() + "{ Name : { Name }})")
                .OnCreate()
                .Set("B = { objNode }")
                .WithParams(new
                {
                    Rootname = objRoot.Name,
                    objRoot = objRoot,
                    objNode = objNode,
                    Name = objNode.Name
                })
                .Return((B, R) => new
                {
                    Count = B.Count(),
                    RelationCount = R.Count(),
                    retObj = B.As<StoreDescription>()
                })
                .Results
                .Single();

            if (result.Count == 1)
            {
                Logger.WriteToLogFile("Successfully created Node");
                objNode = result.retObj;
            }
            else
            {
                Logger.WriteToLogFile("unable to create Node");
                objNode = null;
            }

            return objNode;

        }
        public static void CreateItemNode(ItemWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Item>(objWrap.objItem);

            
            Brand objBrand = objWrap.objBrand;                        
            Item objItem = objWrap.objItem;
            ItemDescription objItemdescription = objWrap.objItemDescription;
            Store objStore = objWrap.objStore;

             Neo4jController.m_graphClient.Cypher
                .Match("(A1:" + objStore.getLabel() + "{ id : { objstoreid }})")
                .Match("(A2:" + objBrand.getLabel() + "{ id : {brandid }})")
                .Merge("(B2: " + objItemdescription.getLabel() + "{Model_Name: { modeNumber}})")
                .OnCreate()
                .Set("B2 = {objItemDesc}")
                .Merge("(A2)-[R2:" + Rel_Brand.brand_item + "]->(B2)")
                .Merge("(A1)-[:" + Rel_Store.store_item + "]->(A3 : " + objItem.getLabel() + ")-[:" + Rel_Item.item_itemDescription + "]->(B2)")
                .OnCreate()
                .Set("A3 = { objItem }")
                .WithParams(new
                {
                    objstoreid = objStore.id,
                    brandid = objBrand.id,
                    objItem = objItem,
                    modeNumber = objItemdescription.Model_Name,
                    objItemDesc = objItemdescription
                })
                .ExecuteWithoutResults();


                /*
                .Return((B2) => new
                {
                    NodeCount = B2.Count(),                    
                    itemObj = B2.As<Item>()
                })
                .Results
                .Single();
                



            if (result.NodeCount == 1)
            {
                objItem = result.itemObj;
                Logger.WriteToLogFile("Successfully created item");
            }
            else
                Logger.WriteToLogFile("unable to create item");


            */
        }

        public static void CreateItemDescriptionNode(ItemDescriptionWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<ItemDescription>(objWrap.objItem);

            ItemDescription objItem = objWrap.objItem;
            Brand objBrand = objWrap.objBrand;
            

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objBrand.getLabel() + " { id: { brandid }})")
                .Create("(B:" + objItem.getLabel() + "{ objItem })")
                .Merge("(A)-[:" + Rel_Brand.brand_item  + "]->(B)")
                .WithParams(new
                {
                    brandid = objBrand.id,
                    objItem = objItem
                })
                .Return((B) => new
                {
                    NodeCount = B.Count()
                    
                })
                .Results
                .Single();

            if (result.NodeCount == 1)
                Logger.WriteToLogFile("Successfully created Item");
            else
                Logger.WriteToLogFile("unable to create Item");

        }

        public static void CreateFilterNode(FilterWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Filter>(objWrap.objfilter);

            Filter objfilter = objWrap.objfilter;
            SubCategory objSubCategory = objWrap.objSubCategory;


            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objSubCategory.getLabel() + " { id :{ subcategoryid }})")
                .Create("(B:" + objfilter.getLabel() + "{ objFilter })")
                .Merge("(A)-[:" + Rel_SubCategory.subCategory_filter + "]->(B)")
                .WithParams(new
                {
                    subcategoryid = objSubCategory.id,
                    objFilter = objfilter
                })
                .Return((B) => new
                {
                    NodeCount = B.Count()

                })
                .Results
                .Single();

            if (result.NodeCount == 1)
                Logger.WriteToLogFile("Successfully created Item");
            else
                Logger.WriteToLogFile("unable to create Item");
        }
    }

}
