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
        string quickSearchBuffer;
        int quickSearchColumnIndex;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PropertyInfo propertyBuffered = gridInventory.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            propertyBuffered.SetValue(gridInventory, true, null);

            toolLabelQuickSearch.Text = "";

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
            if (e.ColumnIndex != DataModule.quantity_fact_index)
                return;

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

        private void gridInventory_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DataModule.inventoryInfo == null || gridInventory.CurrentCell == null)
                return;

            if (!timerQuickSearch.Enabled)
            {
                quickSearchColumnIndex = gridInventory.CurrentCell.ColumnIndex;
                timerQuickSearch.Enabled = true;
            }

            if (quickSearchColumnIndex != gridInventory.CurrentCell.ColumnIndex)
            {
                quickSearchColumnIndex = gridInventory.CurrentCell.ColumnIndex;
                quickSearchBuffer = "";
            }

            gridInventory_SelectQuickSearchRow(quickSearchBuffer + e.KeyChar);
        }

        private void gridInventory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (quickSearchBuffer == "")
                    return;

                gridInventory_SelectQuickSearchRow(quickSearchBuffer.Substring(0, quickSearchBuffer.Length - 1));

            } else if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Return || 
                       e.KeyCode == Keys.Prior || e.KeyCode == Keys.PageUp ||
                       e.KeyCode == Keys.Next || e.KeyCode == Keys.PageDown ||
                       e.KeyCode == Keys.End ||  e.KeyCode == Keys.Home || 
                       e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || 
                       e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                timerQuickSearch_Tick(null, null);

            } else if (e.KeyCode == Keys.Add || (e.KeyCode == Keys.Oemplus && e.Shift) || 
                       e.KeyCode == Keys.Subtract || (e.KeyCode == Keys.OemMinus && e.Shift))
            {
                timerQuickSearch_Tick(null, null);

                if (DataModule.quantity_fact_index == -1)
                    return;

                int sign = (e.KeyCode == Keys.Add || (e.KeyCode == Keys.Oemplus && e.Shift)) ? 1 : -1;

                gridInventory.EditMode = DataGridViewEditMode.EditProgrammatically;
                gridInventory.CurrentCell = gridInventory.Rows[gridInventory.CurrentCell.RowIndex].Cells[DataModule.quantity_fact_index];
                gridInventory.BeginEdit(true);
                if (gridInventory.CurrentCell.Value.GetType().IsValueType)
                    gridInventory.CurrentCell.Value = Math.Max((decimal)gridInventory.CurrentCell.Value + (sign * 1), 0);
                else if (sign == 1)
                    gridInventory.CurrentCell.Value = (decimal)1;
                gridInventory.EndEdit();
                gridInventory.EditMode = DataGridViewEditMode.EditOnEnter;

                gridInventory.CurrentCell = gridInventory.Rows[gridInventory.CurrentCell.RowIndex].Cells[quickSearchColumnIndex];
            }
        }

        private void gridInventory_SelectQuickSearchRow(string newQuickSearchBuffer)
        {
            int searchPos = DataModule.FindRow(quickSearchColumnIndex, newQuickSearchBuffer);
            if (searchPos != -1)
            {
                timerQuickSearch.Enabled = false;
                timerQuickSearch.Enabled = true;

                gridInventory.CurrentCell = gridInventory.Rows[searchPos].Cells[quickSearchColumnIndex];

                quickSearchBuffer = newQuickSearchBuffer;
                toolLabelQuickSearch.Text = "Быстрый поиск: " + quickSearchBuffer.ToUpper();
            }
        }

        private void timerQuickSearch_Tick(object sender, EventArgs e)
        {
            timerQuickSearch.Enabled = false;
            quickSearchBuffer = "";
            toolLabelQuickSearch.Text = "";
        }
    }
}
