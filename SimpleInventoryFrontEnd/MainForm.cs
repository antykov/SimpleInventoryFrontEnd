using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
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
            PropertyInfo propertyBuffered = gridInventory.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyBuffered.SetValue(gridInventory, true, null);

            DataModule.formElements = new Hashtable();
            DataModule.formElements["gridInventory"] = gridInventory;
            DataModule.formElements["bindingSource"] = bindingSource;
            DataModule.formElements["toolLabelScannerConnected"] = toolLabelScannerConnected;
            DataModule.formElements["toolLabelInventoryInfo"] = toolLabelInventoryInfo;

            DataModule.ConnectBarcodeScanner();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            if (formSettings.ShowDialog() == DialogResult.OK)
                DataModule.ConnectBarcodeScanner();
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

            DataModule.UpdateInventoryGrid();
        }

        private void gridInventory_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            DataModule.ApplyDataGridColumnAppearance(e.Column);
        }

        private void gridInventory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // согласно columnsAppearance единственная колонка, доступная для редактирования - количество фактическое
            // поэтому никаких проверок не делаем

            string errorText = ((string)((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue).Replace(',', '.');
            decimal result;
            if (Decimal.TryParse(errorText, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result))
            {
                e.Cancel = false;
                ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value = result;
            }
        }

        private void gridInventory_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // согласно columnsAppearance единственная колонка, доступная для редактирования - количество фактическое
            // поэтому никаких проверок не делаем

            if (e.RowIndex == -1 || e.ColumnIndex == -1 || DataModule.rowid_index == -1)
                return;

            DataModule.UpdateSQLiteRow(
                (long)gridInventory.Rows[e.RowIndex].Cells[DataModule.rowid_index].Value,
                gridInventory.Columns[e.ColumnIndex].Name,
                gridInventory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            FormSelectExistingInventory formSelect = new FormSelectExistingInventory();
            if (formSelect.ShowDialog() == DialogResult.OK)
                DataModule.UpdateInventoryGrid();
        }

        private void exportMenuItem_Click(object sender, EventArgs e)
        {
            if (DataModule.inventoryInfo == null)
                return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.CheckFileExists = false;
            saveDialog.Filter = "Файлы xml|*.xml";
            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;

            switch (Path.GetExtension(saveDialog.FileName).ToUpper())
            {
                case ".XML": DataModule.ExportToXML(saveDialog.FileName); break;
            }
        }
    }
}
