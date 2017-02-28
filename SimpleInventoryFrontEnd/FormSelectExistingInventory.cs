using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    public partial class FormSelectExistingInventory : Form
    {
        public FormSelectExistingInventory()
        {
            InitializeComponent();
        }

        private void FormSelectExistingInventory_Load(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(DataModule.sqliteConnection))
                {
                    sqliteCommand.CommandText = @"
                        SELECT id, company, warehouse, date, last_change
                        FROM inventory_info
                        ORDER BY last_change DESC";
                    using (SQLiteDataReader reader = sqliteCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBoxInventory.Items.Add(new InventoryInfo(reader));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка работы с БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }

            if (listBoxInventory.Items.Count == 0)
            {
                MessageBox.Show("Отсутствуют сохраненные инвентаризации!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void listBoxInventory_DoubleClick(object sender, EventArgs e)
        {
            SelectInventory();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            SelectInventory();
        }

        void SelectInventory()
        {
            if (listBoxInventory.SelectedItem == null)
                return;

            DataModule.inventoryInfo = (InventoryInfo)listBoxInventory.SelectedItem;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
