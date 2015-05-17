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
using DBInteractor.DBInterface;

namespace DBInteractor.View
{
    public partial class NewStore : Form
    {
        public NewStore()
        {
            InitializeComponent();

            InitializeComponentsFromDatabase();

            Logger.WriteToLogFile("Starting New Store Window");
        }

        private void PopulateStateComboBox(Country objCountry)
        {
            Logger.WriteToLogFile(Utilities.GetCurrentMethod());

            List<State> lstate = DBGetInterface.GetAllState(objCountry);

            comboState.Items.Clear();

            foreach (State state in lstate)
            {
                ComboboxItem objItem = new ComboboxItem();
                objItem.Text = state.Name;
                comboState.Items.Add(objItem);
            }
        }

        public void InitializeComponentsFromDatabase()
        {
            //Get the list of countries from the databse
            List<Country> lcountry = DBGetInterface.GetAllCountry();

            comboCountry.Items.Clear();

            foreach (Country country in lcountry)
            {
                ComboboxItem objItem = new ComboboxItem();
                objItem.Text = country.Name;
                comboCountry.Items.Add(objItem);
            }

            comboCountry.SelectedIndex = 0;

            PopulateStateComboBox(lcountry.First());

            
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            Logger.WriteToLogFile("Populating store object");
            StoreWrapper objWrap = new StoreWrapper(); 

            Store objStore = new Store();
            objStore.StoreId = textBoxStoreId.Text;
            objStore.Address = textBoxAddress.Text;
            objStore.Lattitude = 22.0;
            objStore.Longitude = 20.0;
            objStore.Name = textBoxName.Text;
            //objStore.PhoneNumber =
            //objStore.PhoneNumber = Convert.ToInt64(textBoxPhone.Text);
            objStore.Pincode = Convert.ToInt64(textBoxPin.Text);

            Logger.WriteToLogFile("Populating Country object");
            Country objCountry = new Country();
            ComboboxItem objComboItem = (ComboboxItem)comboCountry.SelectedItem;
            objCountry.Name = objComboItem.Text;

            Logger.WriteToLogFile("Populating State object");
            State objState = new State();
            objComboItem = (ComboboxItem)comboState.SelectedItem;
            objState.Name = objComboItem.Text;

            DBAddinterface.CreateStoreNode(objWrap);         
                        
        }
    }
}
