namespace ExcelInteractor
{
    partial class Form1
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
            this.groupBoxAdd = new System.Windows.Forms.GroupBox();
            this.buttonItemDescription = new System.Windows.Forms.Button();
            this.buttonItem = new System.Windows.Forms.Button();
            this.buttonSubCategory = new System.Windows.Forms.Button();
            this.buttonCategory = new System.Windows.Forms.Button();
            this.buttonBrand = new System.Windows.Forms.Button();
            this.buttonStore = new System.Windows.Forms.Button();
            this.buttonCity = new System.Windows.Forms.Button();
            this.buttonState = new System.Windows.Forms.Button();
            this.buttonCountry = new System.Windows.Forms.Button();
            this.buttonAll = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBoxAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxAdd
            // 
            this.groupBoxAdd.Controls.Add(this.buttonItemDescription);
            this.groupBoxAdd.Controls.Add(this.buttonItem);
            this.groupBoxAdd.Controls.Add(this.buttonSubCategory);
            this.groupBoxAdd.Controls.Add(this.buttonCategory);
            this.groupBoxAdd.Controls.Add(this.buttonBrand);
            this.groupBoxAdd.Controls.Add(this.buttonStore);
            this.groupBoxAdd.Controls.Add(this.buttonCity);
            this.groupBoxAdd.Controls.Add(this.buttonState);
            this.groupBoxAdd.Controls.Add(this.buttonCountry);
            this.groupBoxAdd.Location = new System.Drawing.Point(30, 27);
            this.groupBoxAdd.Name = "groupBoxAdd";
            this.groupBoxAdd.Size = new System.Drawing.Size(436, 133);
            this.groupBoxAdd.TabIndex = 0;
            this.groupBoxAdd.TabStop = false;
            this.groupBoxAdd.Text = "Add";
            // 
            // buttonItemDescription
            // 
            this.buttonItemDescription.Location = new System.Drawing.Point(212, 62);
            this.buttonItemDescription.Name = "buttonItemDescription";
            this.buttonItemDescription.Size = new System.Drawing.Size(75, 23);
            this.buttonItemDescription.TabIndex = 9;
            this.buttonItemDescription.Text = "ItemDescription";
            this.buttonItemDescription.UseVisualStyleBackColor = true;
            this.buttonItemDescription.Click += new System.EventHandler(this.buttonItemDescription_Click);
            // 
            // buttonItem
            // 
            this.buttonItem.Location = new System.Drawing.Point(212, 91);
            this.buttonItem.Name = "buttonItem";
            this.buttonItem.Size = new System.Drawing.Size(82, 23);
            this.buttonItem.TabIndex = 7;
            this.buttonItem.Text = "Item";
            this.buttonItem.UseVisualStyleBackColor = true;
            this.buttonItem.Click += new System.EventHandler(this.buttonItem_Click);
            // 
            // buttonSubCategory
            // 
            this.buttonSubCategory.Location = new System.Drawing.Point(116, 92);
            this.buttonSubCategory.Name = "buttonSubCategory";
            this.buttonSubCategory.Size = new System.Drawing.Size(75, 23);
            this.buttonSubCategory.TabIndex = 6;
            this.buttonSubCategory.Text = "SubCateogry";
            this.buttonSubCategory.UseVisualStyleBackColor = true;
            this.buttonSubCategory.Click += new System.EventHandler(this.buttonSubCategory_Click);
            // 
            // buttonCategory
            // 
            this.buttonCategory.Location = new System.Drawing.Point(116, 62);
            this.buttonCategory.Name = "buttonCategory";
            this.buttonCategory.Size = new System.Drawing.Size(75, 23);
            this.buttonCategory.TabIndex = 5;
            this.buttonCategory.Text = "Category";
            this.buttonCategory.UseVisualStyleBackColor = true;
            this.buttonCategory.Click += new System.EventHandler(this.buttonCategory_Click);
            // 
            // buttonBrand
            // 
            this.buttonBrand.Location = new System.Drawing.Point(212, 32);
            this.buttonBrand.Name = "buttonBrand";
            this.buttonBrand.Size = new System.Drawing.Size(75, 23);
            this.buttonBrand.TabIndex = 4;
            this.buttonBrand.Text = "Brand";
            this.buttonBrand.UseVisualStyleBackColor = true;
            this.buttonBrand.Click += new System.EventHandler(this.buttonBrand_Click);
            // 
            // buttonStore
            // 
            this.buttonStore.Location = new System.Drawing.Point(116, 31);
            this.buttonStore.Name = "buttonStore";
            this.buttonStore.Size = new System.Drawing.Size(75, 23);
            this.buttonStore.TabIndex = 3;
            this.buttonStore.Text = "Store";
            this.buttonStore.UseVisualStyleBackColor = true;
            this.buttonStore.Click += new System.EventHandler(this.buttonStore_Click);
            // 
            // buttonCity
            // 
            this.buttonCity.Location = new System.Drawing.Point(18, 92);
            this.buttonCity.Name = "buttonCity";
            this.buttonCity.Size = new System.Drawing.Size(75, 23);
            this.buttonCity.TabIndex = 2;
            this.buttonCity.Text = "City";
            this.buttonCity.UseVisualStyleBackColor = true;
            this.buttonCity.Click += new System.EventHandler(this.buttonCity_Click);
            // 
            // buttonState
            // 
            this.buttonState.Location = new System.Drawing.Point(18, 62);
            this.buttonState.Name = "buttonState";
            this.buttonState.Size = new System.Drawing.Size(75, 23);
            this.buttonState.TabIndex = 1;
            this.buttonState.Text = "State";
            this.buttonState.UseVisualStyleBackColor = true;
            this.buttonState.Click += new System.EventHandler(this.buttonState_Click);
            // 
            // buttonCountry
            // 
            this.buttonCountry.Location = new System.Drawing.Point(18, 32);
            this.buttonCountry.Name = "buttonCountry";
            this.buttonCountry.Size = new System.Drawing.Size(75, 23);
            this.buttonCountry.TabIndex = 0;
            this.buttonCountry.Text = "Country";
            this.buttonCountry.UseVisualStyleBackColor = true;
            this.buttonCountry.Click += new System.EventHandler(this.buttonCountry_Click);
            // 
            // buttonAll
            // 
            this.buttonAll.Location = new System.Drawing.Point(191, 207);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new System.Drawing.Size(75, 23);
            this.buttonAll.TabIndex = 1;
            this.buttonAll.Text = "All";
            this.buttonAll.UseVisualStyleBackColor = true;
            this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(154, 352);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(144, 20);
            this.labelStatus.TabIndex = 10;
            this.labelStatus.Text = "Status Message";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 393);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonAll);
            this.Controls.Add(this.groupBoxAdd);
            this.Name = "Form1";
            this.Text = "ExcelInteractor";
            this.groupBoxAdd.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxAdd;
        private System.Windows.Forms.Button buttonCountry;
        private System.Windows.Forms.Button buttonState;
        private System.Windows.Forms.Button buttonCity;
        private System.Windows.Forms.Button buttonStore;
        private System.Windows.Forms.Button buttonBrand;
        private System.Windows.Forms.Button buttonCategory;
        private System.Windows.Forms.Button buttonSubCategory;
        private System.Windows.Forms.Button buttonItem;
        private System.Windows.Forms.Button buttonItemDescription;
        private System.Windows.Forms.Button buttonAll;
        private System.Windows.Forms.Label labelStatus;
    }
}

