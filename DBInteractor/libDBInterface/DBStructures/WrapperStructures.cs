using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractor.Common
{
    public class CountryWrapper
    {
        public World objWorld;
        public Country objCountry;
    }

    public class StoreWrapper
    {
        public Country objCountry { get; set; }
        public State objState { get; set; }
        public City objCity { get; set; }
        public Store objStore { get; set; }
    }

    public class StateWrapper
    {
        public Country objCountry { get; set; }
        public State objState { get; set; }
    }

    public class CityWrapper
    {
        public Country objCountry { get; set; }
        public State objState { get; set; }
        public City objCity { get; set; }
    }

    public class ItemWrapper
    {
        public Item objItem;
        public Brand objBrand;
        public SubCategory objSubCetegory;
        public Store objStore;
        public Category objCategory;
        public ItemDescription objItemDescription;

    }

    public class ItemDescriptionWrapper
    {
        public ItemDescription objItem;
        public Category objCategory;
        public SubCategory objSubCetegory;
        public Brand objBrand;
    }

    public class CategoryWrapper
    {
        public CategoryRoot objCategoryRoot;
        public Category objCategory;
    }

    public class SubCategoryWrapper
    {
        public Category objCategory;
        public SubCategory objSubCategory;
    }


    public class BrandDescriptionWrapper
    {
        public BrandRoot objRoot;
        public BrandDescription objBrand;
    }

    public class BrandWrapper
    {
        public Brand objBrand;
        public SubCategory objSubCategory;
    }

    public class FilterWrapper
    {
        public SubCategory objSubCategory;
        public Filter objfilter;
    }


   
}
