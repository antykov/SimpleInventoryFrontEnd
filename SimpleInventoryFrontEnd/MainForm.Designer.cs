namespace SimpleInventoryFrontEnd
{
    partial class MainForm
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolLabelScannerConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolLabelInventoryInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridInventory = new System.Windows.Forms.DataGridView();
            this.menuStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(574, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importMenuItem,
            this.openMenuItem,
            this.exportMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem});
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.настройкиToolStripMenuItem.Text = "Файл";
            // 
            // importMenuItem
            // 
            this.importMenuItem.Name = "importMenuItem";
            this.importMenuItem.ShortcutKeyDisplayString = "";
            this.importMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importMenuItem.Size = new System.Drawing.Size(217, 22);
            this.importMenuItem.Text = "Импорт из файла...";
            this.importMenuItem.Click += new System.EventHandler(this.importMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openMenuItem.Size = new System.Drawing.Size(217, 22);
            this.openMenuItem.Text = "Открыть...";
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // exportMenuItem
            // 
            this.exportMenuItem.Name = "exportMenuItem";
            this.exportMenuItem.Size = new System.Drawing.Size(217, 22);
            this.exportMenuItem.Text = "Экспорт...";
            this.exportMenuItem.Click += new System.EventHandler(this.exportMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(214, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.settingsToolStripMenuItem.Text = "Настройки...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // statusStripMain
            // 
            this.statusStripMain.AllowMerge = false;
            this.statusStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLabelScannerConnected,
            this.toolLabelInventoryInfo});
            this.statusStripMain.Location = new System.Drawing.Point(0, 457);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(574, 24);
            this.statusStripMain.TabIndex = 1;
            // 
            // toolLabelScannerConnected
            // 
            this.toolLabelScannerConnected.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolLabelScannerConnected.Name = "toolLabelScannerConnected";
            this.toolLabelScannerConnected.Size = new System.Drawing.Size(136, 19);
            this.toolLabelScannerConnected.Text = "Подключение сканера";
            // 
            // toolLabelInventoryInfo
            // 
            this.toolLabelInventoryInfo.Name = "toolLabelInventoryInfo";
            this.toolLabelInventoryInfo.Size = new System.Drawing.Size(239, 19);
            this.toolLabelInventoryInfo.Text = "Данные по инвентаризации не загружены";
            // 
            // gridInventory
            // 
            this.gridInventory.AllowUserToAddRows = false;
            this.gridInventory.AllowUserToDeleteRows = false;
            this.gridInventory.AllowUserToResizeRows = false;
            this.gridInventory.AutoGenerateColumns = false;
            this.gridInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridInventory.DataSource = this.bindingSource;
            this.gridInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridInventory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridInventory.Location = new System.Drawing.Point(0, 24);
            this.gridInventory.MultiSelect = false;
            this.gridInventory.Name = "gridInventory";
            this.gridInventory.RowHeadersVisible = false;
            this.gridInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridInventory.ShowEditingIcon = false;
            this.gridInventory.Size = new System.Drawing.Size(574, 433);
            this.gridInventory.TabIndex = 2;
            this.gridInventory.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridInventory_CellValueChanged);
            this.gridInventory.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.gridInventory_ColumnAdded);
            this.gridInventory.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridInventory_DataError);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(574, 481);
            this.Controls.Add(this.gridInventory);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.Text = "Проведение инвентаризации";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolLabelScannerConnected;
        private System.Windows.Forms.ToolStripStatusLabel toolLabelInventoryInfo;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.DataGridView gridInventory;
    }
}

