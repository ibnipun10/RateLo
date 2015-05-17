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
    public partial class EditStore : Form
    {
        public EditStore()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                
        }

        private void InitializeControls()
        {
            groupBoxStore.Enabled = false;
            buttonDelete.Enabled = false;
            buttonEdit.Enabled = false;

            textBoxStoreId.Text = String.Empty;
            textBoxName.Text = String.Empty;
            textBoxAddress.Text = String.Empty;
            textBoxPin.Text = String.Empty;
            textBoxPhone.Text = String.Empty;
            labelGuid.Text = String.Empty;
            comboCountry.Items.Clear();
            comboState.Items.Clear();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            StoreWrapper objWrap = DBGetInterface.GetStoreWrapper(textBoxFind.Text);

            PopulateAllControls(objWrap);
        }

        private void PopulateAllControls(StoreWrapper objWrap)
        {
            textBoxStoreId.Text = objWrap.objStore.StoreId;
            textBoxName.Text = objWrap.objStore.Name;
            textBoxAddress.Text = objWrap.objStore.Address;
            textBoxPin.Text = objWrap.objStore.Pincode.ToString();
            textBoxPhone.Text = objWrap.objStore.PhoneNumber.ToString();
            labelGuid.Text = objWrap.objStore.id;
            labelTimeStamp.Text = objWrap.objStore.createTime.ToString();

            comboState.Items.Add(Utilities.GetComboboxItem(objWrap.objState.Name));

            comboCountry.Items.Add(Utilities.GetComboboxItem(objWrap.objCountry.Name));

            buttonDelete.Enabled = true;
            buttonEdit.Enabled = true;

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you really want to delete");

            if (result == DialogResult.OK)
            {
                DBDeleteInterface.DeleteStore(textBoxStoreId.Text);
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
            }            
        }

        private Store PopulateStoreFromControls()
        {
            Store objStore = new Store();
            objStore.id = labelGuid.Text;
            objStore.Address = textBoxAddress.Text;
            objStore.lastUpdated = Utilities.GetDateTimeInUnixTimeStamp();
            //objStore.PhoneNumber = Convert.ToInt64(textBoxPhone.Text);
            objStore.Pincode = Convert.ToInt64(textBoxPin.Text);
            objStore.StoreId = textBoxStoreId.Text;
            objStore.createTime = Convert.ToInt64(labelTimeStamp.Text);
            objStore.Name = textBoxName.Text;

            return objStore;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            DBUpdateInterface.UpdateStore(PopulateStoreFromControls());

            InitializeControls();
        }
    }
}
