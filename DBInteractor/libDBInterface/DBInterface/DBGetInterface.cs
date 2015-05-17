using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBInteractor.Common;
using System.Collections;

namespace DBInteractor.DBInterface
{
    public class DBGetInterface
    {
        public static Country GetCountryNode(Country objCountry)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Country>(objCountry);

            Country objret = null;

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objCountry.getLabel() + " { Name : {countryName }})")
                .WithParam("countryName", objCountry.Name)
                .Return(A => A.As<Country>())
                .Results;

            if (result.Count() > 0)
            {
                Logger.WriteToLogFile("Country found");
                objret = result.ElementAt(0);
            }
            else
                Logger.WriteToLogFile("Country not found");

            return objret;

        }

        public static State GetStateNode(StateWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<State>(objWrap.objState);

            State objState = objWrap.objState;
            Country objCountry = objWrap.objCountry;
            State retState = null;

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objCountry.getLabel() + " { Name : {countryName }})")
                .Match("(B:" + objState.getLabel() + " { Name : {stateName }})")
                .Match("(A)-[r:" + Rel_Country.country_state + "]->(B)")
                .WithParams(new
                {
                    countryName = objCountry.Name,
                    stateName = objState.Name
                })
                .Return(B => B.As<State>())
                .Results;

            if (result.Count() > 0)
            {
                Logger.WriteToLogFile("State found");
                retState = result.ElementAt(0);
            }
            else
                Logger.WriteToLogFile("State not found");

            return retState;

        }

        public static City GetCityNode(CityWrapper objWrap)
        {
            Logger.WriteToLogFile(DBInteractor.Common.Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<City>(objWrap.objCity);

            StateWrapper objStateWrapper = new StateWrapper();
            objStateWrapper.objCountry = objWrap.objCountry;
            objStateWrapper.objState = objWrap.objState;
            City objCity = objWrap.objCity;

            State objState = GetStateNode(objStateWrapper);

            if (objState == null)
                return null;
            
            City retCity = null;

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objState.getLabel() + " { id : {Stateid }})")
                .Match("(B:" + objCity.getLabel() + " { Name : {CityName }})")
                .Match("(A)-[r:" + Rel_State.state_city + "]->(B)")
                .WithParams(new
                {
                    Stateid  = objState.id,
                    CityName = objCity.Name
                })
                .Return(B => B.As<City>())
                .Results;

            if (result.Count() > 0)
            {
                Logger.WriteToLogFile("City found");
                retCity = result.ElementAt(0);
            }
            else
                Logger.WriteToLogFile("City not found");

            return retCity;

        }

        public static Store GetStoreNode(Store objStore)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Store>(objStore);

            Store retStore = null;

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objStore.getLabel() + "{ StoreId : { Storeid }})")
                .WithParam("Storeid", objStore.StoreId)
                .Return(A => A.As<Store>())
                .Results;

            if (result.Count() > 0)
            {
                Logger.WriteToLogFile("Store found");
                retStore = result.ElementAt(0);
            }
            else
                Logger.WriteToLogFile("Store mont found");

            return retStore;
        }

        public static Category GetCategoryNode(Category objCategory)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());
            Logger.WriteObjectToLogFile<Category>(objCategory);

            Category retCategory = null;

            var result = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objCategory.getLabel() + "{ Name : { name }})")
                .WithParam("name", objCategory.Name)
                .Return(A => A.As<Category>())
                .Results;

            if (result.Count() > 0)
            {
                Logger.WriteToLogFile("Category found");
                retCategory = result.ElementAt(0);
            }
            else
                Logger.WriteToLogFile("Cateogry not found");

            return retCategory;
        }




        public static List<Country> GetAllCountry()
        {
            return GetAllNodes<Country>(new Country());
        }

        public static List<State> GetAllState(Country objCountry)
        {
            return GetAllChildrenNodes<State>(objCountry, Rel_Country.country_state);
        }

        public static List<City> GetAllCity(State objState)
        {
            return GetAllChildrenNodes<City>(objState, Rel_State.state_city);
        }

        public static List<Store> GetAllStore(City objCity)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());
            

            List<Store> lNodes = new List<Store>();
                       

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objCity.getLabel() + "{id : { nodeId }})")
                .Match("(A)-[:" + Rel_City.city_store + "]->(B)")
                .Match("(B)-[:" + Rel_Store.store_storeDescription + "]->(C)")
                .WithParam("nodeId", objCity.id)
                .Return((B, C) => new
                {
                    storeObj = B.As<Store>(),
                    StoreName = C.As<StoreDescription>().Name
                })      
                .Results;

            Logger.WriteToLogFile("Nodes count = " + results.Count());

            foreach (var result in results)
            {
                Store objStore = new Store();
                objStore = result.storeObj;
                objStore.Name = result.StoreName;
                lNodes.Add(objStore);
            }

            return lNodes;


        }

        

        public static List<Store> GetAllStore()
        {
            return GetAllNodes<Store>(new Store());
        }

        public static List<Category> GetAllCategory()
        {
            return GetAllNodes<Category>(new Category());
        }

        public static List<SubCategory> GetAllSubCategory(Category objCategory)
        {
            return GetAllChildrenNodes<SubCategory>(objCategory, Rel_Category.category_subCategory);
        }
        public static List<SubCategory> GetAllSubCategory()
        {
            return GetAllNodes<SubCategory>(new SubCategory());
        }

        public static List<Filter> GetAllFilters(SubCategory objSubCategory)
        {
            return GetAllChildrenNodes<Filter>(objSubCategory, Rel_SubCategory.subCategory_filter);
        }

        public static List<Brand> GetAllBrand(SubCategory objSubCategory)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            List<Brand> lNodes = new List<Brand>();


            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objSubCategory.getLabel() + "{id : { nodeId }})")
                .Match("(A)-[:" + Rel_SubCategory.subCategory_brand + "]->(B)")
                .Match("(B)-[:" + Rel_Brand.brand_brandDescription + "]->(C)")
                .WithParam("nodeId", objSubCategory.id)
                .Return((B, C) => new
                {
                    brandObj = B.As<Brand>(),
                    brandName = C.As<BrandDescription>().Name
                })
                .Results;

            Logger.WriteToLogFile("Nodes count = " + results.Count());

            foreach (var result in results)
            {
                Brand objBrand = new Brand();
                objBrand = result.brandObj;
                objBrand.Name = result.brandName;
                lNodes.Add(objBrand);
            }

            return lNodes;

        }

        public static ArrayList GetAllItemDecription(Brand objBrand, SubCategory objSubCategory)
        {
            ArrayList lItems = new ArrayList();
            
            switch(objSubCategory.SubCategoryID)
            {
                case SubCategoriesID.SUBCATEGORY_AC :
                    lItems = new ArrayList(GetAllChildrenNodes<ACItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                case SubCategoriesID.SUBCATEGORY_CAMERA:
                    lItems = new ArrayList(GetAllChildrenNodes<CameraItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                case SubCategoriesID.SUBCATEGORY_FRIDGE:
                    lItems = new ArrayList(GetAllChildrenNodes<FridgeItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                case SubCategoriesID.SUBCATEGORY_LAPTOP:
                     lItems = new ArrayList(GetAllChildrenNodes<LapTopItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                case SubCategoriesID.SUBCATEGORY_TV:
                     lItems = new ArrayList(GetAllChildrenNodes<TvItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                case SubCategoriesID.SUBCATEGORY_WASHING_MACHINE:
                     lItems = new ArrayList(GetAllChildrenNodes<WashinigMachineItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                case SubCategoriesID.SUBCATEOGRY_MICROWAVES:
                     lItems = new ArrayList(GetAllChildrenNodes<WashinigMachineItemDescription>(objBrand, Rel_Brand.brand_item));
                    break;
                default: break;
            }

            
                      
            return lItems;
            
        }

        public static List<T> GetAllChildrenNodes<T>(Node objNode, string relationShip)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());


            List<T> lNodes = new List<T>();

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objNode.getLabel() + "{id : { nodeId }})")
                .Match("(A)-[r:" +  relationShip + "]->(B)")
                .WithParam("nodeId", objNode.id)
                .Return(B => B.As<T>())
                .Results;

            Logger.WriteToLogFile("Nodes count = " + results.Count());

            foreach (var result in results)
            {
                lNodes.Add(result);
            }

            return lNodes;
        }

        public static List<T> GetAllNodes<T>(Node objNode)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());
                        
            List<T> lNodes = new List<T>();
            

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objNode.getLabel() + ")")
                .Return(A => A.As<T>())
                .Results;

            Logger.WriteToLogFile("Nodes count = " + results.Count());

            foreach (var result in results)
            {
                lNodes.Add(result);
            }

            return lNodes;
        }

        public static List<BrandDescription> GetAllBrandDescription()
        {
            return GetAllNodes<BrandDescription>(new BrandDescription());
        }

        public static List<StoreDescription> GetAllStoreDescription()
        {
            return GetAllNodes<StoreDescription>(new StoreDescription());
        }

        public static Store GetStore(string storeId)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod()); 
         
            Store objStore = new Store();

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objStore.getLabel() + ")")
                .Where((Store A) => A.StoreId == storeId)
                .Return(A => A.As<Store>())
                .Results;

            if (results.Count() == 0)
            {
                Logger.WriteToLogFile("No matching store found with storeId : " + storeId);
                return null;
            }
            else
                Logger.WriteToLogFile("Matching store found");

            objStore = results.First();
            Logger.WriteObjectToLogFile(objStore);

            return objStore;
            
        }

        public static State GetState(Store objStore)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            if (objStore == null)
                return null;

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(B:" + objStore.getLabel() + " {Name : { name }, StoreId : { storeId}} )")
                .Match("(A)-[r:" + Rel_City.city_store + "]->(B)")
                .WithParams(new
                {
                    Name = objStore.Name,
                    storeId = objStore.StoreId
                })
            .Return(A => A.As<State>())
            .Results;

            if (results.Count() == 0)
            {
                Logger.WriteToLogFile("No state returned ");
                return null;
            }

            State objState = results.First();
            return objState;
        }

        public static Country GetCountry(State state)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            if (state == null)
                return null;

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(B:" + state.getLabel() + " { id : { id }}")
                .Match("(A)-[r:" + Rel_Country.country_state + "]->(B)")
                .WithParam("id", state.id)
            .Return(A => A.As<Country>())
            .Results;

            if (results.Count() == 0)
            {
                Logger.WriteToLogFile("No country returned ");
                return null;
            }

            Country objcountry = results.First();
            return objcountry;
        }

        public static StoreWrapper GetStoreWrapper(string storeId)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            StoreWrapper objWrap = new StoreWrapper();
            objWrap.objStore = GetStore(storeId);
            objWrap.objState = GetState(objWrap.objStore);
            objWrap.objCountry = GetCountry(objWrap.objState);

            return objWrap;
        }

        public static ItemDescription GetItemDescription(Brand objBrand, String ModelNumber)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());
            ItemDescription objItemDescr = new ItemDescription();

            if (String.IsNullOrEmpty(ModelNumber))
                throw new Exception("Model Number is null");

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objBrand.getLabel() + "{id : { Brandid}})")
                .Match("(A)-[R:" + Rel_Brand.brand_item + "]->(B: " + objItemDescr.getLabel() + "{ Model_Name : { modelNumber}})")
                .WithParams(new
                {
                    Brandid = objBrand.id,
                    modelNumber = ModelNumber
                })
                .Return(B => B.As<ItemDescription>())
                .Results;

            if (results.Count() == 0)
                return null;
            else
                return results.First();

        }

        public static Item GetItem(Store objStore, ItemDescription objItemDesc)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Item objItem = new Item();

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A: " + objStore.getLabel() + "{ id : { storeid }})")
                .Match("(B: " + objItemDesc.getLabel() + "{ id : { itemdescid}})")
                .Match("(A)-[: " + Rel_Store.store_item + "]->(C:" + objItem.getLabel() + ")")
                .Match("(C)-[:" + Rel_Item.item_itemDescription + "]->(B)")
                .WithParams(new
                {
                    storeid = objStore.id,
                    itemdescid = objItemDesc.id
                })
                .Return(C => C.As<Item>())
                .Results;

            if (results.Count() == 0)
                return null;
            else
                return results.First();

        }        

        public static List<ItemWrapper> GetAllItemsAndDescription(ItemWrapper objWrap)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            Brand objBrand = objWrap.objBrand;
            Store objStore = objWrap.objStore;
            Item objItem = new Item();
            List<ItemWrapper> lItems = new List<ItemWrapper>();

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A:" + objBrand.getLabel() + "{ id: { brandid}})")
                .Match("(B:" + objStore.getLabel() + "{ id :{ storeid}})")
                .Match("(B)-[:" + Rel_Brand.brand_item + "]->(C: " + objItem.getLabel() + ")")
                .Match("(C)-[:" + Rel_Item.item_itemDescription + "]->(D)")
                .Match("(D)<-[:" + Rel_Brand.brand_item + "]-(A)")
                .WithParams(new
                {
                    brandid = objBrand.id,
                    storeid = objStore.id
                })
                .Return((C,D) => new
                {
                    itemDescrObj = D.As<ItemDescription>(),                    
                    itemObj = C.As<Item>()
                })                
                .Results;

            foreach (var result in results)
            {
                ItemWrapper objWrapper = new ItemWrapper();
                objWrapper.objItemDescription = result.itemDescrObj;
                objWrapper.objItem = result.itemObj;
                lItems.Add(objWrapper);
            }

            return lItems;
        }

        public static SubCategory GetSubCategoryNode(String subCategoryName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            SubCategory objSubCategory = new SubCategory();

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A: " + objSubCategory.getLabel() + ")")
                .Where("LOWER(A.Name) = '" + subCategoryName.ToLower() + "'")
                .Return(A => A.As<SubCategory>())
                .Results;

            if (results.Count() == 0)
                return null;
            else
                return results.First();
        }

        public static Brand GetBrandNode(SubCategory objSubCategory, String BrandName)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            if (objSubCategory == null)
                return null;

            Brand objBrand = new Brand();

            var results = Neo4jController.m_graphClient.Cypher
                .Match("(A: " + objSubCategory.getLabel() + " { id : { subcategoryid }})")
                .Match("(B: " + objBrand.getLabel() + ")")
                .Match("(A)-[:" + Rel_SubCategory.subCategory_brand + "]->(B)")
                .Match("(B)-[:" + Rel_Brand.brand_brandDescription + "]->(C)")
                .Where("LOWER(C.Name) = '" + BrandName.ToLower() + "'")
                .WithParam("subcategoryid", objSubCategory.id)
                .Return(B => B.As<Brand>())
                .Results;

            if (results.Count() == 0)
                return null;
            else
                return results.First();
        }
       

    }
}
