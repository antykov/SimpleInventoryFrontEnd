﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    public partial class MainForm : Form
    {
        void ConnectBarcodeScanner()
        {
            DataModule.scanner.DeviceEnabled = false;
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.ScannerName))
            {
                int scannerIndex = 0;
                for (int i = 0; i < DataModule.scanner.DeviceCount; i++)
                {
                    DataModule.scanner.CurrentDeviceNumber = i + 1;
                    if (DataModule.scanner.CurrentDeviceName.Equals(Properties.Settings.Default.ScannerName))
                    {
                        scannerIndex = i + 1;
                        break;
                    }
                }
                if (scannerIndex > 0)
                {
                    try
                    {
                        DataModule.scanner.CurrentDeviceNumber = scannerIndex;
                        DataModule.scanner.DataEvent += Scanner_DataEvent;
                        DataModule.scanner.DeviceEnabled = true;
                        DataModule.scannerConnected = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка подключения сканера штрих-кодов:\n" + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        DataModule.scanner.DeviceEnabled = false;
                    }
                }
            }

            toolLabelScannerConnected.Text = (DataModule.scannerConnected) ? "Сканер \"" + Properties.Settings.Default.ScannerName + "\" подключен" : "Сканер не подключен";
        }

        private void Scanner_DataEvent()
        {
            throw new NotImplementedException();
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ConnectBarcodeScanner();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            if (formSettings.ShowDialog() == DialogResult.OK)
                ConnectBarcodeScanner();
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

        private void UpdateInventoryGrid()
        {
            toolLabelInventoryInfo.Text =
                DataModule.inventoryInfo.Company + " (" +
                DataModule.inventoryInfo.Warehouse + ") от " +
                DataModule.inventoryInfo.Date.ToString("dd.MM.yyyy");

            DataTable table = new DataTable();

            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT rowid, info_id, code, barcode, description, unit, quantity, quantity_fact from inventory_items", DataModule.sqliteConnection))
            {
                adapter.Fill(table);
                bindingSource.DataSource = table;
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
