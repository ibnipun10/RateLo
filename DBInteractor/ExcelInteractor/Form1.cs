using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelInteractor.ExcelInterface;
using DBInteractor.DBInterface;
using DBCommon = DBInteractor.Common;

namespace ExcelInteractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Initialize log            
            DBCommon.Logger.InitializeLogs("Log", DBCommon.Utilities.GetDateTimeInUnixTimeStamp().ToString());

            ExcelController.InitializeExcel(Common.EXCEL_PATH);

            //Initialize the neo4j controller
            Neo4jController.InitializeController("localhost", 7474);
            Neo4jController.connect();
        }

        private void buttonCountry_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_COUNTRY);
        }

        private void buttonState_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_STATE);
        }

        private void buttonCity_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_CITY);

        }

        private void buttonStore_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_STORE);

        }

        private void buttonBrand_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_BRAND);
        }

        private void buttonCategory_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_CATEGORY);
        }

        private void buttonSubCategory_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_SUBCATEGORY);
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_ITEM);
        }        

        private void buttonItemDescription_Click(object sender, EventArgs e)
        {
            CreateNodes(CreateNodeFlags.FLAG_ITEMDESCRIPTION);
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            //Create all at once from excel
            labelStatus.Text = "Creating All Nodes";


            CreateNodes(CreateNodeFlags.FLAG_COUNTRY |
                CreateNodeFlags.FLAG_STATE |
                CreateNodeFlags.FLAG_CITY |
                CreateNodeFlags.FLAG_STORE |
                CreateNodeFlags.FLAG_CATEGORY |
                CreateNodeFlags.FLAG_SUBCATEGORY |
                CreateNodeFlags.FLAG_BRAND |
                CreateNodeFlags.FLAG_ITEM |
                CreateNodeFlags.FLAG_ITEMDESCRIPTION);
            
            
            
            
            
        }

        private void CreateNodes(int flag)
        {
            if ((flag & CreateNodeFlags.FLAG_COUNTRY) != 0)
            {
                labelStatus.Text = "Creating Country nodes";
                ExcelAddInterface.AddCountry(ExcelSheets.EXCELSHEET_COUNTRY);
            }
            if((flag & CreateNodeFlags.FLAG_STATE) != 0)
            {
                labelStatus.Text = "Creating State Nodes";
                ExcelAddInterface.AddState(ExcelSheets.EXCELSHEET_STATE);

            }
            if((flag & CreateNodeFlags.FLAG_CITY) != 0)
            {
                labelStatus.Text = "Creating city Nodes";
                ExcelAddInterface.AddCity(ExcelSheets.EXCELSHEET_CITY);

            }
            if((flag & CreateNodeFlags.FLAG_STORE) != 0)
            {
                labelStatus.Text = "Creating Store Nodes";
                ExcelAddInterface.AddStore(ExcelSheets.EXCELSHEET_STORE);

            }
            if((flag & CreateNodeFlags.FLAG_CATEGORY) != 0)
            {
                labelStatus.Text = "Creating Cateogry Nodes";
                ExcelAddInterface.AddCategory(ExcelSheets.EXCELSHEET_CATEGORY);
            
            }
            if((flag & CreateNodeFlags.FLAG_SUBCATEGORY) != 0)
            {
                labelStatus.Text = "Creating subCategory Nodes";
                ExcelAddInterface.AddSubCategory(ExcelSheets.EXCELSHEET_SUBCATEGORY);
            }
            if((flag & CreateNodeFlags.FLAG_BRAND) != 0)
            {
                labelStatus.Text = "Creating Brand nodes";
                ExcelAddInterface.AddBrand(ExcelSheets.EXCELSHEET_BRAND);
            }
            if((flag & CreateNodeFlags.FLAG_ITEMDESCRIPTION) != 0)
            {
                labelStatus.Text = "Creating ItemDescription nodes";
                ExcelAddInterface.AddItemDescription(ExcelSheets.EXCELSHEET_ITEMDESCRIPTION);
            }
            if((flag & CreateNodeFlags.FLAG_ITEM) != 0)
            {
                labelStatus.Text = "Creating Item Nodes";
                ExcelAddInterface.AddItem(ExcelSheets.EXCELSHEET_ITEM);
            }
            
            labelStatus.Text = "Completed";  

        } 
    }
}
