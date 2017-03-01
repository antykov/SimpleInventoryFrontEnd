using System;
using System.Collections;
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
                        ORDER BY company, last_change DESC";
                    using (SQLiteDataReader reader = sqliteCommand.ExecuteReader())
                    {
                        Hashtable listViewGroups = new Hashtable();

                        while (reader.Read())
                        {
                            InventoryInfo info = new InventoryInfo(reader);

                            if (!listViewGroups.ContainsKey(info.Company))
                            {
                                ListViewGroup group = new ListViewGroup(info.Company);
                                listViewInventory.Groups.Add(group);

                                listViewGroups[info.Company] = group;
                            }

                            ListViewItem item = new ListViewItem(info.Description, (ListViewGroup)listViewGroups[info.Company]);
                            item.Tag = info;
                            listViewInventory.Items.Add(item);
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

            if (listViewInventory.Items.Count == 0)
            {
                MessageBox.Show("Отсутствуют сохраненные инвентаризации!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void listViewInventory_DoubleClick(object sender, EventArgs e)
        {
            SelectInventory();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            SelectInventory();
        }

        void SelectInventory()
        {
            if (listViewInventory.SelectedItems.Count == 0)
                return;

            DataModule.inventoryInfo = (InventoryInfo)listViewInventory.SelectedItems[0].Tag;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
