using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBInteractor.Common;
using DBInteractor.View;
using DBInteractor.DBInterface;
using System.Collections;
using Telerik.WinControls.UI;
using libDealSheelCommon.Common;
using libDBInterface.DBStructures;
using libExcelInteractor;
using libScrapper;
using System.Threading;
using System.Net;
using System.IO;
using OfficeOpenXml;
using libDealSheelCommonMethods;


namespace DBInteractor
{
    public partial class AdminConsole : Form
    {
        private ArrayList m_listNodes;
        private CXMLNode m_xmlNode;

        public AdminConsole()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                InitializeComponent();
                Logger.InitializeLogs("Log", Utilities.GetDateTimeInUnixTimeStamp().ToString());
                m_xmlNode = XMLController.PopulateXMLObject(Constants.DealSheelConfigFile);

                if (m_xmlNode == null)
                    throw new Exception("Unable to parse xml file");

                Neo4jController.InitializeController(m_xmlNode.DatabaseServer.Server, Convert.ToInt32(m_xmlNode.DatabaseServer.Port));
                Neo4jController.connect();


                PopulateCountryGrid();
                PopulateStoreDescGrid();

                SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.CountryTemplate));
                radGridViewCountry.CommandCellClick += radGridViewCountry_CommandCellClick;
                radGridViewCategory.CommandCellClick += radGridViewCountry_CommandCellClick;
                radGridViewBrandDescr.CommandCellClick += radGridViewCountry_CommandCellClick;
                radGridViewStoreDesc.CommandCellClick += radGridViewCountry_CommandCellClick;


                Logger.WriteToLogFile("Starting Admin console");

            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex.Message);
                MessageBox.Show(ex.Message);
                this.Close();
            }

            Cursor.Current = Cursors.Default;
        }

        void radGridViewCountry_CommandCellClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                GridCommandCellElement cell = (GridCommandCellElement)(sender);
                UploadImageOnCellCommandClick(cell.RowInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void UploadImageOnCellCommandClick(GridViewRowInfo row)
        {
            //Open the File Dialog.
            OpenFileDialog objFileDialog = new OpenFileDialog();
            DialogResult dr = objFileDialog.ShowDialog();
            string completeFilePath = "";
            dynamic dlRow = row.DataBoundItem;


            if (dr == DialogResult.OK)
            {
                completeFilePath = objFileDialog.FileName;
                string fileDir = Utilities.GetImageDir(dlRow.getLabel());
                string destLocalFilePath = Utilities.GetImagePath(fileDir, dlRow.id);

                Utilities.CreateFolder(fileDir);
                ImageController.SaveImageToLocalMachine(completeFilePath, destLocalFilePath);

                try
                {
                    //Create a seperate thread for this to do
                    ImageController.SaveimageToRemoteLocation(fileDir, Utilities.AppendImageType(dlRow.id), m_xmlNode.AppServer.Server, m_xmlNode.AppServer.Port);
                    MessageBox.Show("Uploaded");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to upload Image");
                }


            }
        }



        private void SetImageInGrid(GridViewTemplate gridTemplate)
        {
            //Returning as no need for this function anymore......
            return;

            foreach (GridViewRowInfo row in gridTemplate.Rows)
            {
                try
                {
                    dynamic dt = row.DataBoundItem;
                    string url = Utilities.GetImageServerUrl(m_xmlNode.AppServer.Server, m_xmlNode.AppServer.Port, dt.getLabel(), dt.id);
                    MemoryStream ms = ImageController.GetStreamFromUrl(url);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                    GridViewCellInfo cell = (GridViewCellInfo)row.Cells["Image"];
                    cell.Value = img;

                }
                catch (Exception ex)
                {
                    //do not do anything if exception occurs
                }
            }


        }



        private void SetGridViewComboBoxStatus(GridViewTemplate gridTemplate)
        {
            GridViewComboBoxColumn StatusScroll = (GridViewComboBoxColumn)gridTemplate.Columns["Status"];
            StatusScroll.DataSource = GetDataSourceForStatusColumn();
        }

        private void ValidateRow(Node objNode, List<object> lNodes)
        {
            if (String.IsNullOrEmpty(objNode.Name))
                throw new Exception("Name is null or empty...Please provide proper values");

            for (int i = 0; i < lNodes.Count - 1; i++)
            {
                dynamic node = lNodes[i];
                if (objNode.Name.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception("Row with same Name already present");
                }
            }
        }

        private void ValidateItemDescriptionRow(ItemDescription objItem, List<object> lNodes)
        {
            if (String.IsNullOrEmpty(objItem.Model_Name))
                throw new Exception("Model Name is null or empty...Please provide proper values");

            for (int i = 0; i < lNodes.Count - 1; i++)
            {
                dynamic node = lNodes[i];
                if (objItem.Model_Name.Equals(node.Model_Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception("Row with same Model_Name already present");
                }
            }


        }

        public void ValidateItemModelname(ExcelStructure objExcel, List<object> lNodes)
        {
            if (String.IsNullOrEmpty(objExcel.ModelNumber))
                throw new Exception("Model Number should not be blank");

            for (int i = 0; i < lNodes.Count - 1; i++)
            {
                dynamic node = lNodes[i];
                if (objExcel.ModelNumber.Equals(node.ModelNumber, StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("Row with same model number already present");
            }
        }


        private void PopulateCountryGrid()
        {
            m_listNodes = new ArrayList(DBGetInterface.GetAllCountry());

            if (m_listNodes != null && m_listNodes.Count != 0)
                radGridViewCountry.DataSource = m_listNodes;
            else
            {
                ArrayList plist = new ArrayList();
                plist.Add(new Country());
                radGridViewCountry.DataSource = plist;
            }
        }

        private void PopulateStoreDescGrid()
        {
            m_listNodes = new ArrayList(DBGetInterface.GetAllStoreDescription());

            if (m_listNodes != null && m_listNodes.Count != 0)
                radGridViewStoreDesc.DataSource = m_listNodes;
            else
            {

                ArrayList plist = new ArrayList();
                plist.Add(new StoreDescription());
                radGridViewStoreDesc.DataSource = plist;
            }
        }

        private void OnTabSelected(object sender, TabControlEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                if (tabControl.SelectedTab == tabControl.TabPages[0])
                {
                    PopulateCountryGrid();
                    SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.CountryTemplate));

                    PopulateStoreDescGrid();
                    SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.StoreDescriptionTemplate));
                }
                else if (tabControl.SelectedTab == tabControl.TabPages[1])
                {
                    m_listNodes = new ArrayList(DBGetInterface.GetAllCategory());

                    if (m_listNodes != null && m_listNodes.Count != 0)
                        radGridViewCategory.DataSource = m_listNodes;
                    else
                    {

                        ArrayList plist = new ArrayList();
                        plist.Add(new Category());
                        radGridViewCategory.DataSource = plist;
                    }
                    SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.CategoryTemplete));

                    //BrandDescription
                    m_listNodes = new ArrayList(DBGetInterface.GetAllBrandDescription());

                    if (m_listNodes != null && m_listNodes.Count != 0)
                        radGridViewBrandDescr.DataSource = m_listNodes;
                    else
                    {

                        ArrayList plist = new ArrayList();
                        plist.Add(new BrandDescription());
                        radGridViewBrandDescr.DataSource = plist;
                    }

                    SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.BrandDescriptionTemplate));
                }
                else if (tabControl.SelectedTab == tabControl.TabPages[2])
                {
                    //comboBoxStore.DataSource = GetAllStoreIds();
                    comboBoxStore.DataSource = DBGetInterface.GetAllStore();
                    comboBoxCategory.DataSource = DBGetInterface.GetAllCategory();

                    Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                    if (objBrand == null)
                        EnableControls(false);

                    radGridViewExcel.DataSource = null;
                    radGridViewItem.DataSource = null;

                }
                else if (tabControl.SelectedTab == tabControl.TabPages[3])
                {
                    m_listNodes = new ArrayList(DBGetInterface.GetAllSubCategory());
                    radGridViewFilter.DataSource = m_listNodes;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void EnableControls(bool bEnable)
        {
            
            radButtonGetItems.Enabled = bEnable;

        }


        private GridViewTemplate GetGridTemplates(CommonEnum.eGridTemplates eTemplate)
        {
            GridViewTemplate gridTemplate = radGridViewCountry.MasterTemplate;
            switch (eTemplate)
            {
                case CommonEnum.eGridTemplates.CountryTemplate:
                    gridTemplate = radGridViewCountry.MasterTemplate;
                    break;
                case CommonEnum.eGridTemplates.StateTemplate:
                    gridTemplate = radGridViewCountry.MasterTemplate.Templates[0];
                    break;
                case CommonEnum.eGridTemplates.CityTemplate:
                    gridTemplate = radGridViewCountry.MasterTemplate.Templates[0].Templates[0];
                    break;
                case CommonEnum.eGridTemplates.StoreTemplate:
                    gridTemplate = radGridViewCountry.MasterTemplate.Templates[0].Templates[0].Templates[0];
                    break;
                case CommonEnum.eGridTemplates.CategoryTemplete:
                    gridTemplate = radGridViewCategory.MasterTemplate;
                    break;
                case CommonEnum.eGridTemplates.SubCategoryTemplate:
                    gridTemplate = radGridViewCategory.MasterTemplate.Templates[0];
                    break;
                case CommonEnum.eGridTemplates.BrandTemplate:
                    gridTemplate = radGridViewCategory.MasterTemplate.Templates[0].Templates[0];
                    break;
                case CommonEnum.eGridTemplates.BrandDescriptionTemplate:
                    gridTemplate = radGridViewBrandDescr.MasterTemplate;
                    break;
                case CommonEnum.eGridTemplates.StoreDescriptionTemplate:
                    gridTemplate = radGridViewStoreDesc.MasterTemplate;
                    break;
                case CommonEnum.eGridTemplates.ItemDescriptionTemplate:
                    gridTemplate = radGridViewCategory.MasterTemplate.Templates[0].Templates[0].Templates[0];
                    break;
                case CommonEnum.eGridTemplates.ItemTemplate:
                    gridTemplate = radGridViewItem.MasterTemplate;
                    break;
                case CommonEnum.eGridTemplates.SubCategoryFilterTemplate:
                    gridTemplate = radGridViewFilter.MasterTemplate;
                    break;
                case CommonEnum.eGridTemplates.FilterTemplate:
                    gridTemplate = radGridViewFilter.MasterTemplate.Templates[0];
                    break;
                default: break;
            }

            return gridTemplate;
        }

        private void GridRowsHandler(GridViewCollectionChangedEventArgs e)
        {
            try
            {
                if (e.NewItems != null && e.NewItems.Count > 0)
                {
                    GridViewRowInfo gridRowInfo = (GridViewRowInfo)e.NewItems[0];

                    if (e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.ItemChanged)
                    {
                        UpdateGridRows(gridRowInfo);
                    }
                    if (e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.Add)
                    {
                        try
                        {
                            AddGridRows(gridRowInfo);
                        }
                        catch (Exception ex)
                        {
                            gridRowInfo.Delete();

                            throw ex;
                        }
                    }
                    if (e.Action == Telerik.WinControls.Data.NotifyCollectionChangedAction.Remove)
                    {
                        DeleteGridRows(gridRowInfo);
                        MessageBox.Show("Row deleted Successfully");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void UpdateGridRows(GridViewRowInfo gridRowInfo)
        {

            if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CountryTemplate))
            {
                Country objCountry = (Country)gridRowInfo.DataBoundItem;
                objCountry.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();

                DBUpdateInterface.UpdateNode<Country>(objCountry);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate))
            {
                State objState = (State)gridRowInfo.DataBoundItem;
                objState.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<State>(objState);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate))
            {
                City objCity = (City)gridRowInfo.DataBoundItem;
                objCity.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<City>(objCity);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate))
            {
                Store objStore = (Store)gridRowInfo.DataBoundItem;
                objStore.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<Store>(objStore);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CategoryTemplete))
            {
                Category objNode = (Category)gridRowInfo.DataBoundItem;
                objNode.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<Category>(objNode);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate))
            {
                SubCategory objNode = (SubCategory)gridRowInfo.DataBoundItem;
                objNode.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<SubCategory>(objNode);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate))
            {
                Brand objNode = (Brand)gridRowInfo.DataBoundItem;
                objNode.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DeleteGridRows(gridRowInfo);

                AddGridRows(gridRowInfo);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandDescriptionTemplate))
            {
                BrandDescription objNode = (BrandDescription)gridRowInfo.DataBoundItem;
                objNode.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<BrandDescription>(objNode);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StoreDescriptionTemplate))
            {
                StoreDescription objNode = (StoreDescription)gridRowInfo.DataBoundItem;
                objNode.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<StoreDescription>(objNode);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.ItemDescriptionTemplate))
            {
                ItemDescription objNode = (ItemDescription)gridRowInfo.DataBoundItem;
                objNode.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<ItemDescription>(objNode);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.ItemTemplate))
            {
                ExcelStructure objExcel = (ExcelStructure)gridRowInfo.DataBoundItem;

                Store objStore = (Store)comboBoxStore.SelectedItem;
                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                SubCategory objSubCategory = (SubCategory)comboBoxSubCategory.SelectedItem;

                CommonMethods.SaveToDB(objExcel, objStore, objSubCategory, objBrand, m_xmlNode.AppServer.Server, m_xmlNode.AppServer.Port);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.FilterTemplate))
            {
                Filter objFilter = (Filter)gridRowInfo.DataBoundItem;
                objFilter.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
                DBUpdateInterface.UpdateNode<Filter>(objFilter);
            }

        }

        private void DeleteGridRows(GridViewRowInfo gridRowInfo)
        {
            if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CountryTemplate))
            {
                Country objCountry = (Country)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteCountryNode(objCountry);

            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate))
            {
                State objState = (State)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteStateNode(objState);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate))
            {
                City objCity = (City)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteCityNode(objCity);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate))
            {
                Store objStore = (Store)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteStoreNode(objStore);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CategoryTemplete))
            {
                Category objCategory = (Category)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteCategoryNode(objCategory);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate))
            {
                SubCategory objSubCategory = (SubCategory)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteSubCategoryNode(objSubCategory);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate))
            {
                Brand objBrand = (Brand)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteBrandNode(objBrand);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandDescriptionTemplate))
            {
                BrandDescription objBranddesc = (BrandDescription)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteBrandDescriptionNode(objBranddesc);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StoreDescriptionTemplate))
            {
                StoreDescription objStoreDesc = (StoreDescription)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteStoreDescription(objStoreDesc);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.ItemDescriptionTemplate))
            {
                ItemDescription objItemDesc = (ItemDescription)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteItemDescriptionNode(objItemDesc);

            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.ItemTemplate))
            {
                ExcelStructure objExcel = (ExcelStructure)gridRowInfo.DataBoundItem;

                Store objStore = (Store)comboBoxStore.SelectedItem;
                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                SubCategory objSubCategory = (SubCategory)comboBoxSubCategory.SelectedItem;

                ItemDescription objItemDescr = DBGetInterface.GetItemDescription(objBrand, objExcel.ModelNumber);
                if (objItemDescr != null)
                {
                    Item objItem = DBGetInterface.GetItem(objStore, objItemDescr);
                    DBDeleteInterface.DeleteItemNode(objItem);
                }
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.FilterTemplate))
            {
                Filter objFilter = (Filter)gridRowInfo.DataBoundItem;
                DBDeleteInterface.DeleteNode<Filter>(objFilter);
            }

        }

        private void AddGridRows(GridViewRowInfo gridRowInfo)
        {
            List<object> lNodes = new List<object>();

            try
            {

                lNodes = ((IEnumerable)gridRowInfo.ViewTemplate.DataSource).Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                //Ignore this exception 

            }

            if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CountryTemplate))
            {
                Country objCountry = (Country)gridRowInfo.DataBoundItem;
                ValidateRow(objCountry, lNodes);

                DBAddinterface.CreateCountryNode(objCountry);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate))
            {
                State objState = (State)gridRowInfo.DataBoundItem;
                ValidateRow(objState, lNodes);
                StateWrapper objWrap = new StateWrapper();
                objWrap.objState = objState;

                objWrap.objCountry = (Country)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;

                DBAddinterface.CreateStateNode(objWrap);

            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate))
            {
                City objCity = (City)gridRowInfo.DataBoundItem;
                ValidateRow(objCity, lNodes);
                CityWrapper objWrap = new CityWrapper();
                objWrap.objCity = objCity;
                objWrap.objState = (State)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;
                DBAddinterface.CreateCityNode(objWrap);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate))
            {
                Store objStore = (Store)gridRowInfo.DataBoundItem;

                ValidateStoreRow(objStore, lNodes);

                StoreWrapper objWrap = new StoreWrapper();
                objWrap.objCity = (City)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;
                objWrap.objStore = objStore;


                DBAddinterface.CreateStoreNode(objWrap);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CategoryTemplete))
            {
                Category objCategory = (Category)gridRowInfo.DataBoundItem;
                ValidateRow(objCategory, lNodes);

                DBAddinterface.CreateCategoryNode(objCategory);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate))
            {
                SubCategory objSubCategory = (SubCategory)gridRowInfo.DataBoundItem;
                ValidateRow(objSubCategory, lNodes);

                SubCategoryWrapper objWrap = new SubCategoryWrapper();
                objWrap.objCategory = (Category)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;
                objWrap.objSubCategory = objSubCategory;
                DBAddinterface.CreateSubCategoryNode(objWrap);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate))
            {
                Brand objBrand = (Brand)gridRowInfo.DataBoundItem;
                ValidateRow(objBrand, lNodes);

                BrandWrapper objWrap = new BrandWrapper();
                objWrap.objBrand = objBrand;
                objWrap.objSubCategory = (SubCategory)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;
                DBAddinterface.CreateBrandNode(objWrap);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandDescriptionTemplate))
            {
                BrandDescription objBrand = (BrandDescription)gridRowInfo.DataBoundItem;
                ValidateRow(objBrand, lNodes);
                DBAddinterface.CreateBrandDescriptionNode(objBrand);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StoreDescriptionTemplate))
            {
                StoreDescription objStore = (StoreDescription)gridRowInfo.DataBoundItem;
                ValidateRow(objStore, lNodes);
                DBAddinterface.CreateStoreDescriptionNode(objStore);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.ItemDescriptionTemplate))
            {
                ItemDescription objItem = (ItemDescription)gridRowInfo.DataBoundItem;
                ValidateItemDescriptionRow(objItem, lNodes);

                ItemDescriptionWrapper objWrap = new ItemDescriptionWrapper();
                objWrap.objItem = objItem;
                objWrap.objBrand = (Brand)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;
                DBAddinterface.CreateItemDescriptionNode(objWrap);
            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.ItemTemplate))
            {
                ExcelStructure objExcel = (ExcelStructure)gridRowInfo.DataBoundItem;
                ValidateItemModelname(objExcel, lNodes);

                Store objStore = (Store)comboBoxStore.SelectedItem;
                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                SubCategory objSubCategory = (SubCategory)comboBoxSubCategory.SelectedItem;

                ItemDescription objItemDescr = DBGetInterface.GetItemDescription(objBrand, objExcel.ModelNumber);
                if (objItemDescr != null)
                {
                    Item objItem = new Item();
                    objItem.Price = objExcel.Price;
                    objItem.OfferDescription = objExcel.Offer;

                    ItemWrapper objWrap = new ItemWrapper();
                    objWrap.objBrand = objBrand;
                    objWrap.objStore = objStore;
                    objWrap.objItem = objItem;
                    objWrap.objItemDescription = objItemDescr;

                    DBAddinterface.CreateItemNode(objWrap);
                }

            }
            else if (gridRowInfo.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.FilterTemplate))
            {
                Filter objfilter = (Filter)gridRowInfo.DataBoundItem;
                FilterWrapper objWrap = new FilterWrapper();

                objWrap.objSubCategory = (SubCategory)((GridViewRowInfo)gridRowInfo.Parent).DataBoundItem;
                objWrap.objfilter = objfilter;
                DBAddinterface.CreateFilterNode(objWrap);
            }
        }

        #region Sheet3


        private void comboBoxCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                PopulateSubCategoryComboBox();
                PopulateBrandComboBox();

                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                if (objBrand == null)
                    EnableControls(false);
                else
                    EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;

        }

        private void comboBoxSubCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                PopulateBrandComboBox();

                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                if (objBrand == null)
                    EnableControls(false);
                else
                    EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void comboBoxBrand_SelectedValueChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                if (objBrand == null)
                    EnableControls(false);
                else
                    EnableControls(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void PopulateSubCategoryComboBox()
        {
            Category objCategory = (Category)comboBoxCategory.SelectedItem;

            if (objCategory != null)
                comboBoxSubCategory.DataSource = DBGetInterface.GetAllSubCategory(objCategory);
        }



        private void PopulateBrandComboBox()
        {
            SubCategory objSubCategory = (SubCategory)comboBoxSubCategory.SelectedItem;

            if (objSubCategory != null)
                comboBoxBrand.DataSource = DBGetInterface.GetAllBrand(objSubCategory);
        }



        private void radButtonImportExcel_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //Hide the other grid and unhide this grid
                radGridViewItem.Hide();
                radGridViewExcel.Hide();
                radGridViewExcelLink.Show();

                OpenFileDialog objFileDialog = new OpenFileDialog();
                objFileDialog.Multiselect = true;

                DialogResult dr = objFileDialog.ShowDialog();
                string[] fileNames;
                

                if (dr == DialogResult.OK)
                {
                    fileNames = objFileDialog.FileNames;
                    List<ExcelLinkStructure> lexcelLink = new List<ExcelLinkStructure>();

                    foreach (string fileName in fileNames)
                    {
                        ExcelLinkStructure excelLink = new ExcelLinkStructure();
                        excelLink.ExcelLink = fileName;
                        lexcelLink.Add(excelLink);
                    }
                    GridViewComboBoxColumn StoreidColumn = (GridViewComboBoxColumn)radGridViewExcelLink.Columns["StoreId"];

                    List<Store> lStores = new List<Store>();
                    lStores = DBGetInterface.GetAllStore();
                    //lStores.Add(new Store());
                    StoreidColumn.DataSource = lStores;

                    radGridViewExcelLink.DataSource = lexcelLink;

                    //------- check if models are already in db----------

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        

        private Store GetStoreFromList(List<Store> lstore, string storeid)
        {
            foreach(Store objStore in lstore)
            {
                if (objStore.StoreId.Equals(storeid, StringComparison.InvariantCultureIgnoreCase))
                    return objStore;
            }

            return null;
        }

        private void radButtonSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;           

            try
            {
                
                

                GridViewRowCollection lExcelRows = radGridViewExcelLink.Rows;
                List<ExcelStructure> lobjExcelStruct = new List<ExcelStructure>();

                GridViewComboBoxColumn objStoreCombo = (GridViewComboBoxColumn)lExcelRows[0].Cells["StoreId"].ColumnInfo;
                List<Store> lStores =  (List<Store>)(objStoreCombo.DataSource);
                
                foreach (GridViewRowInfo row in lExcelRows)
                {

                    object storeid = row.Cells["StoreId"].Value;
                    if (storeid == null)
                        throw new Exception("Please enter Store id ");

                    //Search for Store id in the list
                    Store objStore = GetStoreFromList(lStores, storeid.ToString());

                    CommonMethods.PopulateExcelData(row.Cells["ExcelLink"].Value.ToString(), ref lobjExcelStruct, objStore);
                }

                radGridViewExcel.DataSource = lobjExcelStruct;

                radGridViewItem.Hide();
                radGridViewExcel.Show();

                GridViewRowCollection lRows = radGridViewExcel.Rows;
                foreach (GridViewRowInfo row in lRows)
                {
                    ExcelStructure objExcelStruct = (ExcelStructure)row.DataBoundItem;

                    Store objStore = objExcelStruct.objStore;
                    SubCategory objSubCateogry = DBGetInterface.GetSubCategoryNode(objExcelStruct.SubCategory);
                    Brand objBrand = DBGetInterface.GetBrandNode(objSubCateogry, objExcelStruct.Brand);

                    try
                    {
                        CommonMethods.SaveToDB(objExcelStruct, objStore, objSubCateogry, objBrand, m_xmlNode.AppServer.Server, m_xmlNode.AppServer.Port, row);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                
            }
            Cursor.Current = Cursors.Default;

        }

        

        private void radButtonGetItems_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;


                radGridViewItem.Show();

                Brand objBrand = (Brand)comboBoxBrand.SelectedItem;
                SubCategory objSubCategory = (SubCategory)comboBoxSubCategory.SelectedItem;

                ItemWrapper objWrap = new ItemWrapper();
                objWrap.objStore = (Store)comboBoxStore.SelectedItem;
                objWrap.objBrand = objBrand;

                List<ItemWrapper> lItem = DBGetInterface.GetAllItemsAndDescription(objWrap);
                GridViewTemplate itemTemplate = GetGridTemplates(CommonEnum.eGridTemplates.ItemTemplate);

                ArrayList lItemDesc = DBGetInterface.GetAllItemDecription(objBrand, objSubCategory);

                GridViewComboBoxColumn modelComboBox = (GridViewComboBoxColumn)itemTemplate.Columns["ModelNumber"];
                modelComboBox.DataSource = lItemDesc;

                itemTemplate.DataSource = GetExcelStructureFromItem(lItem);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;

        }

        private List<Item> GetItemsFromWrapper(List<ItemWrapper> lWrap)
        {
            List<Item> litem = new List<Item>();
            foreach (ItemWrapper itemwrap in lWrap)
            {
                litem.Add(itemwrap.objItem);
            }

            return litem;
        }

        private List<ItemDescription> GetItemDescriptionFromWrapper(List<ItemWrapper> lWrap)
        {
            List<ItemDescription> litem = new List<ItemDescription>();
            foreach (ItemWrapper itemwrap in lWrap)
            {
                litem.Add(itemwrap.objItemDescription);
            }

            return litem;
        }

        private List<ExcelStructure> GetExcelStructureFromItem(List<ItemWrapper> lWrap)
        {
            List<ExcelStructure> lexcel = new List<ExcelStructure>();
            foreach (ItemWrapper wrap in lWrap)
            {
                ExcelStructure objExcel = new ExcelStructure();
                Item objItem = wrap.objItem;
                ItemDescription objItemDesc = wrap.objItemDescription;

                if (objItem != null)
                {
                    objExcel.Offer = objItem.OfferDescription;
                    objExcel.Price = objItem.Price;
                }
                if (objItemDesc != null)
                    objExcel.ModelNumber = objItemDesc.Model_Name;

                lexcel.Add(objExcel);
            }

            return lexcel;
        }



        #endregion

        #region Sheet1

        private void radGridViewCountry_ChildViewExpanding(object sender, Telerik.WinControls.UI.ChildViewExpandingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (!e.IsExpanded)
                {
                    GridViewHierarchyRowInfo parentRow = e.ParentRow;

                    if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CountryTemplate))
                    {
                        //country row is expanded
                        Country objCountry = (Country)parentRow.DataBoundItem;
                        ArrayList stateList = new ArrayList(DBGetInterface.GetAllState(objCountry));


                        if (stateList != null && stateList.Count > 0)
                            GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate).DataSource = stateList;
                        else
                        {

                            ArrayList pList = new ArrayList();
                            pList.Add(new State());
                            GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate).DataSource = pList;
                        }

                        SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate));
                    }
                    else if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.StateTemplate))
                    {
                        State objState = (State)parentRow.DataBoundItem;
                        ArrayList cityList = new ArrayList(DBGetInterface.GetAllCity(objState));

                        if (cityList != null && cityList.Count > 0)
                            GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate).DataSource = cityList;
                        else
                        {

                            ArrayList pList = new ArrayList();
                            pList.Add(new City());
                            GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate).DataSource = pList;
                        }

                        SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate));
                    }
                    else if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CityTemplate))
                    {
                        City objCity = (City)parentRow.DataBoundItem;
                        ArrayList storeList = new ArrayList(DBGetInterface.GetAllStore(objCity));
                        GridViewTemplate gridTemplate = GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate);

                        SetGridViewComboBoxStatus(gridTemplate);

                        GridViewComboBoxColumn StoreNameScroll = (GridViewComboBoxColumn)gridTemplate.Columns["Name"];
                        StoreNameScroll.DataSource = DBGetInterface.GetAllStoreDescription();

                        if (storeList != null && storeList.Count > 0)
                            GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate).DataSource = storeList;
                        else
                        {

                            ArrayList pList = new ArrayList();
                            pList.Add(new Store());
                            GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate).DataSource = pList;
                        }
                        SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.StoreTemplate));
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;

        }

        private ArrayList GetDataSourceForStatusColumn()
        {
            ArrayList comboStatus = new ArrayList();
            comboStatus.Add(CStatus.STATUS_ACTIVE);
            comboStatus.Add(CStatus.STATUS_INACTIVE);
            return comboStatus;
        }

        private ArrayList GetDataSourceForStoreNameColumn()
        {
            ArrayList comboStoreName = new ArrayList();
            List<StoreDescription> lStores = DBGetInterface.GetAllStoreDescription();

            foreach (StoreDescription store in lStores)
                comboStoreName.Add(store.Name);

            return comboStoreName;
        }



        private void radGridViewCountry_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;

        }




        private void ValidateStoreRow(Store objNode, List<object> lNodes)
        {
            if (String.IsNullOrEmpty(objNode.Name) || String.IsNullOrEmpty(objNode.StoreId))
                throw new Exception("Name or Storeid is null or empty...Please provide proper values");

            for (int i = 0; i < lNodes.Count - 1; i++)
            {
                dynamic node = lNodes[i];
                if (objNode.StoreId.Equals(node.StoreId, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception("Row with same StoreId already present");
                }
            }
        }


        #endregion



        #region Sheet2
        private void MasterTemplate_ChildViewExpanding(object sender, ChildViewExpandingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!e.IsExpanded)
                {
                    GridViewHierarchyRowInfo parentRow = e.ParentRow;

                    if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.CategoryTemplete))
                    {

                        Category objNode = (Category)parentRow.DataBoundItem;
                        ArrayList nodeList = new ArrayList(DBGetInterface.GetAllSubCategory(objNode));

                        if (nodeList != null && nodeList.Count > 0)
                            GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate).DataSource = nodeList;
                        else
                        {
                            ArrayList pList = new ArrayList();
                            pList.Add(new SubCategory());
                            GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate).DataSource = pList;
                        }

                        SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate));

                    }
                    else if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryTemplate))
                    {
                        SubCategory objNode = (SubCategory)parentRow.DataBoundItem;
                        ArrayList nodeList = new ArrayList(DBGetInterface.GetAllBrand(objNode));
                        GridViewTemplate gridTemplate = GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate);
                        GridViewComboBoxColumn NameScroll = (GridViewComboBoxColumn)gridTemplate.Columns["Name"];
                        NameScroll.DataSource = GetDataSourceForBrandNameColumn();

                        SetGridViewComboBoxStatus(gridTemplate);

                        if (nodeList != null && nodeList.Count > 0)
                            GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate).DataSource = nodeList;
                        else
                        {
                            ArrayList pList = new ArrayList();
                            pList.Add(new Brand());
                            GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate).DataSource = pList;
                        }

                        SetImageInGrid(GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate));
                    }
                    else if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate))
                    {
                        Brand objBrand = (Brand)parentRow.DataBoundItem;
                        GridViewRowInfo subCategoryRow = (GridViewRowInfo)parentRow.Parent;
                        SubCategory objSubCategory = (SubCategory)subCategoryRow.DataBoundItem;

                        ArrayList nodelist = new ArrayList(DBGetInterface.GetAllItemDecription(objBrand, objSubCategory));
                        GridViewTemplate gridTemplate = GetGridTemplates(CommonEnum.eGridTemplates.ItemDescriptionTemplate);
                        GridViewTemplate parentGridTemplate = GetGridTemplates(CommonEnum.eGridTemplates.BrandTemplate);



                        if (nodelist != null && nodelist.Count > 0)
                        {
                            gridTemplate.DataSource = nodelist;

                        }
                        else
                        {
                            ArrayList plist = new ArrayList();
                            plist.Add(ItemFactory.GetItem(objSubCategory));
                            gridTemplate.DataSource = plist;
                        }

                        HideItemDescriptionColumnsInGrid(gridTemplate);

                        if (gridTemplate.Columns["ImageLink"] == null)
                        {
                            GridViewCommandColumn ImageLinkColumn = new GridViewCommandColumn();
                            ImageLinkColumn.HeaderText = "ImageLink";
                            ImageLinkColumn.Name = "ImageLink";
                            gridTemplate.Columns.Add(ImageLinkColumn);
                        }


                        //gridTemplate.auto
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;

        }

        private void HideItemDescriptionColumnsInGrid(GridViewTemplate gridTemplate)
        {
            try
            {
                gridTemplate.Columns["id"].IsVisible = false;
                gridTemplate.Columns["createTime"].IsVisible = false;
                gridTemplate.Columns["lastUpdated"].IsVisible = false;
                gridTemplate.Columns["Image"].IsVisible = false;
            }
            catch (Exception ex)
            {

            }
        }

        private ArrayList GetDataSourceForBrandNameColumn()
        {
            List<BrandDescription> lnodes = DBGetInterface.GetAllBrandDescription();
            ArrayList brandDescriptionnodes = new ArrayList();

            foreach (BrandDescription node in lnodes)
            {
                brandDescriptionnodes.Add(node.Name);
            }

            return brandDescriptionnodes;
        }

        private void MasterTemplate_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }




        private void radGridViewBrandDescr_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;

        }



        #endregion

        #region Sheet4
        private void MasterTemplate_RowsChanged_1(object sender, GridViewCollectionChangedEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }


        #endregion

        private void radGridViewItem_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void radGridViewStoreDesc_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void MasterTemplate_RowsChanged_2(object sender, GridViewCollectionChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                GridRowsHandler(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void MasterTemplate_ChildViewExpanding_1(object sender, ChildViewExpandingEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (!e.IsExpanded)
                {
                    GridViewHierarchyRowInfo parentRow = e.ParentRow;

                    if (parentRow.ViewTemplate == GetGridTemplates(CommonEnum.eGridTemplates.SubCategoryFilterTemplate))
                    {

                        SubCategory objSubcategory = (SubCategory)parentRow.DataBoundItem;
                        ArrayList filterList = new ArrayList(DBGetInterface.GetAllFilters(objSubcategory));


                        if (filterList != null && filterList.Count > 0)
                            GetGridTemplates(CommonEnum.eGridTemplates.FilterTemplate).DataSource = filterList;
                        else
                        {

                            ArrayList pList = new ArrayList();
                            pList.Add(new Filter());
                            GetGridTemplates(CommonEnum.eGridTemplates.FilterTemplate).DataSource = pList;

                        }


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

      

    }
}
