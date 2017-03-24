using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        void FillComboBox()
        {
            comboBoxScannerName.Items.Clear();
            try
            {
                Scaner.Scaner45 searchScanner = new Scaner.Scaner45();
                for (int i = 0; i < searchScanner.DeviceCount; i++)
                {
                    searchScanner.CurrentDeviceNumber = i + 1;
                    comboBoxScannerName.Items.Add(searchScanner.CurrentDeviceName);
                }

                comboBoxScannerName.SelectedIndex = comboBoxScannerName.FindStringExact(Properties.Settings.Default.ScannerName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка получения списка устройств", MessageBoxButtons.OK, MessageBoxIcon.Error);
                comboBoxScannerName.Items.Clear();
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            FillComboBox();
            checkBoxNewRow.Checked = Properties.Settings.Default.AddNewRowWhenBarcodeNotFound;
            checkBoxExportDiff.Checked = Properties.Settings.Default.ExportDiffInventoryResults;
        }

        private void buttonShowScannerProperties_Click(object sender, EventArgs e)
        {
            DataModule.scanner.ShowProperties();
            FillComboBox();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (comboBoxScannerName.SelectedIndex == -1)
                MessageBox.Show("Не выбран сканер штрих-кодов!");
            else
            {
                Properties.Settings.Default.ScannerName = comboBoxScannerName.Text;
                Properties.Settings.Default.AddNewRowWhenBarcodeNotFound = checkBoxNewRow.Checked;
                Properties.Settings.Default.ExportDiffInventoryResults = checkBoxExportDiff.Checked;
                Properties.Settings.Default.Save();
                
                DialogResult = DialogResult.OK;

                Close();
            }
        }
    }
}
