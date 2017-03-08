namespace SimpleInventoryFrontEnd
{
    partial class FormAddEmptyInventory
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
            this.groupBoxCompany = new System.Windows.Forms.GroupBox();
            this.textBoxCompanyCode = new System.Windows.Forms.TextBox();
            this.labelCompanyCode = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.textBoxCompany = new System.Windows.Forms.TextBox();
            this.groupBoxWarehouse = new System.Windows.Forms.GroupBox();
            this.labelWarehouse = new System.Windows.Forms.Label();
            this.textBoxWarehouse = new System.Windows.Forms.TextBox();
            this.labelWarehouseCode = new System.Windows.Forms.Label();
            this.textBoxWarehouseCode = new System.Windows.Forms.TextBox();
            this.labelComment = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.groupBoxCompany.SuspendLayout();
            this.groupBoxWarehouse.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCompany
            // 
            this.groupBoxCompany.Controls.Add(this.labelCompany);
            this.groupBoxCompany.Controls.Add(this.textBoxCompany);
            this.groupBoxCompany.Controls.Add(this.labelCompanyCode);
            this.groupBoxCompany.Controls.Add(this.textBoxCompanyCode);
            this.groupBoxCompany.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCompany.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCompany.Name = "groupBoxCompany";
            this.groupBoxCompany.Size = new System.Drawing.Size(437, 49);
            this.groupBoxCompany.TabIndex = 0;
            this.groupBoxCompany.TabStop = false;
            this.groupBoxCompany.Text = "Организация:";
            // 
            // textBoxCompanyCode
            // 
            this.textBoxCompanyCode.Location = new System.Drawing.Point(47, 18);
            this.textBoxCompanyCode.Name = "textBoxCompanyCode";
            this.textBoxCompanyCode.Size = new System.Drawing.Size(76, 20);
            this.textBoxCompanyCode.TabIndex = 0;
            // 
            // labelCompanyCode
            // 
            this.labelCompanyCode.AutoSize = true;
            this.labelCompanyCode.Location = new System.Drawing.Point(12, 21);
            this.labelCompanyCode.Name = "labelCompanyCode";
            this.labelCompanyCode.Size = new System.Drawing.Size(29, 13);
            this.labelCompanyCode.TabIndex = 1;
            this.labelCompanyCode.Text = "Код:";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(129, 21);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(86, 13);
            this.labelCompany.TabIndex = 3;
            this.labelCompany.Text = "Наименование:";
            // 
            // textBoxCompany
            // 
            this.textBoxCompany.Location = new System.Drawing.Point(221, 18);
            this.textBoxCompany.Name = "textBoxCompany";
            this.textBoxCompany.Size = new System.Drawing.Size(204, 20);
            this.textBoxCompany.TabIndex = 2;
            // 
            // groupBoxWarehouse
            // 
            this.groupBoxWarehouse.Controls.Add(this.labelWarehouse);
            this.groupBoxWarehouse.Controls.Add(this.textBoxWarehouse);
            this.groupBoxWarehouse.Controls.Add(this.labelWarehouseCode);
            this.groupBoxWarehouse.Controls.Add(this.textBoxWarehouseCode);
            this.groupBoxWarehouse.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxWarehouse.Location = new System.Drawing.Point(0, 49);
            this.groupBoxWarehouse.Name = "groupBoxWarehouse";
            this.groupBoxWarehouse.Size = new System.Drawing.Size(437, 49);
            this.groupBoxWarehouse.TabIndex = 1;
            this.groupBoxWarehouse.TabStop = false;
            this.groupBoxWarehouse.Text = "Склад:";
            // 
            // labelWarehouse
            // 
            this.labelWarehouse.AutoSize = true;
            this.labelWarehouse.Location = new System.Drawing.Point(129, 21);
            this.labelWarehouse.Name = "labelWarehouse";
            this.labelWarehouse.Size = new System.Drawing.Size(86, 13);
            this.labelWarehouse.TabIndex = 3;
            this.labelWarehouse.Text = "Наименование:";
            // 
            // textBoxWarehouse
            // 
            this.textBoxWarehouse.Location = new System.Drawing.Point(221, 18);
            this.textBoxWarehouse.Name = "textBoxWarehouse";
            this.textBoxWarehouse.Size = new System.Drawing.Size(204, 20);
            this.textBoxWarehouse.TabIndex = 2;
            // 
            // labelWarehouseCode
            // 
            this.labelWarehouseCode.AutoSize = true;
            this.labelWarehouseCode.Location = new System.Drawing.Point(12, 21);
            this.labelWarehouseCode.Name = "labelWarehouseCode";
            this.labelWarehouseCode.Size = new System.Drawing.Size(29, 13);
            this.labelWarehouseCode.TabIndex = 1;
            this.labelWarehouseCode.Text = "Код:";
            // 
            // textBoxWarehouseCode
            // 
            this.textBoxWarehouseCode.Location = new System.Drawing.Point(47, 18);
            this.textBoxWarehouseCode.Name = "textBoxWarehouseCode";
            this.textBoxWarehouseCode.Size = new System.Drawing.Size(76, 20);
            this.textBoxWarehouseCode.TabIndex = 0;
            // 
            // labelComment
            // 
            this.labelComment.AutoSize = true;
            this.labelComment.Location = new System.Drawing.Point(12, 101);
            this.labelComment.Name = "labelComment";
            this.labelComment.Size = new System.Drawing.Size(80, 13);
            this.labelComment.TabIndex = 5;
            this.labelComment.Text = "Комментарий:";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(15, 117);
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(410, 20);
            this.textBoxComment.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(350, 143);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(269, 143);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 6;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // FormAddEmptyInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(437, 172);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.labelComment);
            this.Controls.Add(this.textBoxComment);
            this.Controls.Add(this.groupBoxWarehouse);
            this.Controls.Add(this.groupBoxCompany);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAddEmptyInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Параметры новой инвентаризации";
            this.groupBoxCompany.ResumeLayout(false);
            this.groupBoxCompany.PerformLayout();
            this.groupBoxWarehouse.ResumeLayout(false);
            this.groupBoxWarehouse.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCompany;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.TextBox textBoxCompany;
        private System.Windows.Forms.Label labelCompanyCode;
        private System.Windows.Forms.TextBox textBoxCompanyCode;
        private System.Windows.Forms.GroupBox groupBoxWarehouse;
        private System.Windows.Forms.Label labelWarehouse;
        private System.Windows.Forms.TextBox textBoxWarehouse;
        private System.Windows.Forms.Label labelWarehouseCode;
        private System.Windows.Forms.TextBox textBoxWarehouseCode;
        private System.Windows.Forms.Label labelComment;
        private System.Windows.Forms.TextBox textBoxComment;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAdd;
    }
}