namespace SimpleInventoryFrontEnd
{
    partial class FormSelectExistingInventory
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
            this.listViewInventory = new System.Windows.Forms.ListView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.buttonAddEmpty = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.panelBottom.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewInventory
            // 
            this.listViewInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewInventory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewInventory.HideSelection = false;
            this.listViewInventory.Location = new System.Drawing.Point(0, 0);
            this.listViewInventory.MultiSelect = false;
            this.listViewInventory.Name = "listViewInventory";
            this.listViewInventory.Size = new System.Drawing.Size(509, 280);
            this.listViewInventory.TabIndex = 3;
            this.listViewInventory.TileSize = new System.Drawing.Size(470, 20);
            this.listViewInventory.UseCompatibleStateImageBehavior = false;
            this.listViewInventory.View = System.Windows.Forms.View.Tile;
            this.listViewInventory.DoubleClick += new System.EventHandler(this.listViewInventory_DoubleClick);
            this.listViewInventory.Resize += new System.EventHandler(this.listViewInventory_Resize);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.panelLeft);
            this.panelBottom.Controls.Add(this.panelRight);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 245);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(509, 35);
            this.panelBottom.TabIndex = 4;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.buttonCancel);
            this.panelRight.Controls.Add(this.buttonSelect);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(343, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(166, 35);
            this.panelRight.TabIndex = 5;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(84, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(3, 6);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 1;
            this.buttonSelect.Text = "Выбрать";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.buttonDelete);
            this.panelLeft.Controls.Add(this.buttonAddEmpty);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(215, 35);
            this.panelLeft.TabIndex = 6;
            // 
            // buttonAddEmpty
            // 
            this.buttonAddEmpty.Location = new System.Drawing.Point(5, 6);
            this.buttonAddEmpty.Name = "buttonAddEmpty";
            this.buttonAddEmpty.Size = new System.Drawing.Size(123, 23);
            this.buttonAddEmpty.TabIndex = 7;
            this.buttonAddEmpty.Text = "Создать пустую...";
            this.buttonAddEmpty.UseVisualStyleBackColor = true;
            this.buttonAddEmpty.Click += new System.EventHandler(this.buttonAddEmpty_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(134, 6);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 8;
            this.buttonDelete.Text = "Удалить";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // FormSelectExistingInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(509, 280);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.listViewInventory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormSelectExistingInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выбор сохраненной инвентаризации";
            this.Load += new System.EventHandler(this.FormSelectExistingInventory_Load);
            this.panelBottom.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listViewInventory;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAddEmpty;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSelect;
    }
}