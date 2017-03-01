using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Type dgvType = gridInventory.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(gridInventory, true, null);

            DataModule.gridInventory = gridInventory;
            DataModule.bindingSource = bindingSource;
            DataModule.ConnectBarcodeScanner(toolLabelScannerConnected);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            if (formSettings.ShowDialog() == DialogResult.OK)
                DataModule.ConnectBarcodeScanner(toolLabelScannerConnected);
        }

        private void importMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.CheckFileExists = true;
            openDialog.Filter = "Файлы xml|*.xml";
            openDialog.Multiselect = false;
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

            toolLabelInventoryInfo.Text = "";

            if (!DataModule.LoadFile(openDialog.FileName))
                return;

            UpdateInventoryGrid();
        }

        void UpdateInventoryGrid()
        {
            toolLabelInventoryInfo.Text =
                DataModule.inventoryInfo.Company + " (" +
                DataModule.inventoryInfo.Warehouse + ") от " +
                DataModule.inventoryInfo.Date.ToString("dd.MM.yyyy");

            DataTable table = new DataTable();

            using (SQLiteCommand sqliteCommand = new SQLiteCommand(DataModule.sqliteConnection))
            {
                sqliteCommand.CommandText = @"
                    SELECT rowid, info_id, code, barcode, description, unit, quantity, quantity_fact 
                    FROM inventory_items
                    WHERE info_id = @info_id";
                sqliteCommand.Parameters.AddWithValue("info_id", DataModule.inventoryInfo.ID);
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqliteCommand))
                {
                    adapter.Fill(table);
                    bindingSource.DataSource = table;
                }
            }

            gridInventory.AutoGenerateColumns = true;
        }

        private void gridInventory_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            DataModule.ApplyDataGridColumnAppearance(e.Column);
        }

        private void gridInventory_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || DataModule.rowid_index == -1 || DataModule.info_id_index == -1)
                return;

            DataModule.UpdateSQLiteRow(
                (long)gridInventory.Rows[e.RowIndex].Cells[DataModule.rowid_index].Value,
                (long)gridInventory.Rows[e.RowIndex].Cells[DataModule.info_id_index].Value,
                gridInventory.Columns[e.ColumnIndex].Name,
                gridInventory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            FormSelectExistingInventory formSelect = new FormSelectExistingInventory();
            if (formSelect.ShowDialog() == DialogResult.OK)
                UpdateInventoryGrid();
        }
    }
}
