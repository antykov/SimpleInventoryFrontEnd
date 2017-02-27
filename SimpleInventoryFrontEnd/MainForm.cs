using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    public partial class MainForm : Form
    {
        bool scannerConnected;

        void ConnectBarcodeScanner()
        {
            Program.scanner.DeviceEnabled = false;
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.ScannerName))
            {
                int scannerIndex = 0;
                for (int i = 0; i < Program.scanner.DeviceCount; i++)
                {
                    Program.scanner.CurrentDeviceNumber = i + 1;
                    if (Program.scanner.CurrentDeviceName.Equals(Properties.Settings.Default.ScannerName))
                    {
                        scannerIndex = i + 1;
                        break;
                    }
                }
                if (scannerIndex > 0)
                {
                    try
                    {
                        Program.scanner.CurrentDeviceNumber = scannerIndex;
                        Program.scanner.DataEvent += Scanner_DataEvent;
                        Program.scanner.DeviceEnabled = true;
                        scannerConnected = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка подключения сканера штрих-кодов:\n" + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Program.scanner.DeviceEnabled = false;
                    }
                }
            }

            toolLabelScannerConnected.Text = (scannerConnected) ? "Сканер \"" + Properties.Settings.Default.ScannerName + "\" подключен" : "Сканер не подключен";
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

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.CheckFileExists = true;
            openDialog.Filter = "Файлы с разделителями (csv)|*.csv";
            openDialog.Multiselect = false;
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;

        }
    }
}
