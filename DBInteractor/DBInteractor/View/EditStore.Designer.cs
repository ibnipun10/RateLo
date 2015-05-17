namespace DBInteractor.View
{
    partial class EditStore
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonFind = new System.Windows.Forms.Button();
            this.labelfind = new System.Windows.Forms.Label();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.groupBoxStore = new System.Windows.Forms.GroupBox();
            this.comboState = new System.Windows.Forms.ComboBox();
            this.comboCountry = new System.Windows.Forms.ComboBox();
            this.textBoxPhone = new System.Windows.Forms.TextBox();
            this.labelPhoe = new System.Windows.Forms.Label();
            this.textBoxPin = new System.Windows.Forms.TextBox();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxStoreId = new System.Windows.Forms.TextBox();
            this.labelPin = new System.Windows.Forms.Label();
            this.labelCountry = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelStoreId = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.storeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.storeWrapperBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.storeBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.labelGuid = new System.Windows.Forms.Label();
            this.labelTimeStamp = new System.Windows.Forms.Label();
            this.groupBoxStore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeWrapperBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonFind
            // 
            this.buttonFind.Location = new System.Drawing.Point(353, 44);
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(75, 23);
            this.buttonFind.TabIndex = 0;
            this.buttonFind.Text = "Find";
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // labelfind
            // 
            this.labelfind.AutoSize = true;
            this.labelfind.Location = new System.Drawing.Point(58, 55);
            this.labelfind.Name = "labelfind";
            this.labelfind.Size = new System.Drawing.Size(69, 13);
            this.labelfind.TabIndex = 1;
            this.labelfind.Text = "Enter store id";
            // 
            // textBoxFind
            // 
            this.textBoxFind.Location = new System.Drawing.Point(148, 47);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(165, 20);
            this.textBoxFind.TabIndex = 2;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(142, 504);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(75, 23);
            this.buttonEdit.TabIndex = 4;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(247, 504);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // groupBoxStore
            // 
            this.groupBoxStore.Controls.Add(this.labelTimeStamp);
            this.groupBoxStore.Controls.Add(this.labelGuid);
            this.groupBoxStore.Controls.Add(this.comboState);
            this.groupBoxStore.Controls.Add(this.comboCountry);
            this.groupBoxStore.Controls.Add(this.textBoxPhone);
            this.groupBoxStore.Controls.Add(this.labelPhoe);
            this.groupBoxStore.Controls.Add(this.textBoxPin);
            this.groupBoxStore.Controls.Add(this.textBoxAddress);
            this.groupBoxStore.Controls.Add(this.textBoxName);
            this.groupBoxStore.Controls.Add(this.textBoxStoreId);
            this.groupBoxStore.Controls.Add(this.labelPin);
            this.groupBoxStore.Controls.Add(this.labelCountry);
            this.groupBoxStore.Controls.Add(this.labelState);
            this.groupBoxStore.Controls.Add(this.labelAddress);
            this.groupBoxStore.Controls.Add(this.labelStoreId);
            this.groupBoxStore.Controls.Add(this.labelName);
            this.groupBoxStore.Enabled = false;
            this.groupBoxStore.Location = new System.Drawing.Point(61, 127);
            this.groupBoxStore.Name = "groupBoxStore";
            this.groupBoxStore.Size = new System.Drawing.Size(419, 360);
            this.groupBoxStore.TabIndex = 6;
            this.groupBoxStore.TabStop = false;
            this.groupBoxStore.Text = "Store ";
            // 
            // comboState
            // 
            this.comboState.FormattingEnabled = true;
            this.comboState.Location = new System.Drawing.Point(102, 193);
            this.comboState.Name = "comboState";
            this.comboState.Size = new System.Drawing.Size(121, 21);
            this.comboState.TabIndex = 32;
            // 
            // comboCountry
            // 
            this.comboCountry.FormattingEnabled = true;
            this.comboCountry.Location = new System.Drawing.Point(102, 147);
            this.comboCountry.Name = "comboCountry";
            this.comboCountry.Size = new System.Drawing.Size(121, 21);
            this.comboCountry.TabIndex = 31;
            // 
            // textBoxPhone
            // 
            this.textBoxPhone.Location = new System.Drawing.Point(102, 283);
            this.textBoxPhone.Name = "textBoxPhone";
            this.textBoxPhone.Size = new System.Drawing.Size(100, 20);
            this.textBoxPhone.TabIndex = 30;
            // 
            // labelPhoe
            // 
            this.labelPhoe.AutoSize = true;
            this.labelPhoe.Location = new System.Drawing.Point(38, 283);
            this.labelPhoe.Name = "labelPhoe";
            this.labelPhoe.Size = new System.Drawing.Size(38, 13);
            this.labelPhoe.TabIndex = 29;
            this.labelPhoe.Text = "Phone";
            // 
            // textBoxPin
            // 
            this.textBoxPin.Location = new System.Drawing.Point(102, 236);
            this.textBoxPin.Name = "textBoxPin";
            this.textBoxPin.Size = new System.Drawing.Size(100, 20);
            this.textBoxPin.TabIndex = 28;
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(102, 111);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(100, 20);
            this.textBoxAddress.TabIndex = 27;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(102, 74);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 26;
            // 
            // textBoxStoreId
            // 
            this.textBoxStoreId.Location = new System.Drawing.Point(102, 32);
            this.textBoxStoreId.Name = "textBoxStoreId";
            this.textBoxStoreId.Size = new System.Drawing.Size(100, 20);
            this.textBoxStoreId.TabIndex = 25;
            // 
            // labelPin
            // 
            this.labelPin.AutoSize = true;
            this.labelPin.Location = new System.Drawing.Point(38, 244);
            this.labelPin.Name = "labelPin";
            this.labelPin.Size = new System.Drawing.Size(22, 13);
            this.labelPin.TabIndex = 24;
            this.labelPin.Text = "Pin";
            // 
            // labelCountry
            // 
            this.labelCountry.AutoSize = true;
            this.labelCountry.Location = new System.Drawing.Point(38, 150);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(43, 13);
            this.labelCountry.TabIndex = 23;
            this.labelCountry.Text = "Country";
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(38, 202);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(32, 13);
            this.labelState.TabIndex = 22;
            this.labelState.Text = "State";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(38, 111);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(45, 13);
            this.labelAddress.TabIndex = 21;
            this.labelAddress.Text = "Address";
            // 
            // labelStoreId
            // 
            this.labelStoreId.AutoSize = true;
            this.labelStoreId.Location = new System.Drawing.Point(35, 32);
            this.labelStoreId.Name = "labelStoreId";
            this.labelStoreId.Size = new System.Drawing.Size(41, 13);
            this.labelStoreId.TabIndex = 20;
            this.labelStoreId.Text = "StoreId";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(35, 65);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 19;
            this.labelName.Text = "Name";
            // 
            // storeBindingSource
            // 
            this.storeBindingSource.DataSource = typeof(DBInteractor.Common.Store);
            // 
            // storeWrapperBindingSource
            // 
            this.storeWrapperBindingSource.DataSource = typeof(DBInteractor.Common.StoreWrapper);
            // 
            // storeBindingSource1
            // 
            this.storeBindingSource1.DataSource = typeof(DBInteractor.Common.Store);
            // 
            // labelGuid
            // 
            this.labelGuid.AutoSize = true;
            this.labelGuid.Location = new System.Drawing.Point(270, 32);
            this.labelGuid.Name = "labelGuid";
            this.labelGuid.Size = new System.Drawing.Size(57, 13);
            this.labelGuid.TabIndex = 7;
            this.labelGuid.Text = "Store Guid";
            this.labelGuid.Visible = false;
            // 
            // labelTimeStamp
            // 
            this.labelTimeStamp.AutoSize = true;
            this.labelTimeStamp.Location = new System.Drawing.Point(273, 80);
            this.labelTimeStamp.Name = "labelTimeStamp";
            this.labelTimeStamp.Size = new System.Drawing.Size(54, 13);
            this.labelTimeStamp.TabIndex = 33;
            this.labelTimeStamp.Text = "timestamp";
            this.labelTimeStamp.Visible = false;
            // 
            // EditStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 554);
            this.Controls.Add(this.groupBoxStore);
            this.Controls.Add(this.textBoxFind);
            this.Controls.Add(this.labelfind);
            this.Controls.Add(this.buttonFind);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonEdit);
            this.Name = "EditStore";
            this.Text = "SearchStore";
            this.groupBoxStore.ResumeLayout(false);
            this.groupBoxStore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeWrapperBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.storeBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Label labelfind;
        private System.Windows.Forms.TextBox textBoxFind;
        private System.Windows.Forms.BindingSource storeBindingSource;
        private System.Windows.Forms.BindingSource storeWrapperBindingSource;
        private System.Windows.Forms.BindingSource storeBindingSource1;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.GroupBox groupBoxStore;
        private System.Windows.Forms.ComboBox comboState;
        private System.Windows.Forms.ComboBox comboCountry;
        private System.Windows.Forms.TextBox textBoxPhone;
        private System.Windows.Forms.Label labelPhoe;
        private System.Windows.Forms.TextBox textBoxPin;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxStoreId;
        private System.Windows.Forms.Label labelPin;
        private System.Windows.Forms.Label labelCountry;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelStoreId;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelGuid;
        private System.Windows.Forms.Label labelTimeStamp;
    }
}