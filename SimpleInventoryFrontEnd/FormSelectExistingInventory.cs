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
        public bool IsUpdateNeeded = false;

        public FormSelectExistingInventory()
        {
            InitializeComponent();
        }

        private void FillExistingInventories(string description = "")
        {
            listViewInventory.Clear();

            try
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(DataModule.sqliteConnection))
                {
                    sqliteCommand.CommandText = @"
                        SELECT 
                            id, 
                            company, company_code, 
                            warehouse, warehouse_code, 
                            inventory_group_code, inventory_group, 
                            document_number,
                            date, last_change
                        FROM inventory_info
                        ORDER BY company, warehouse, inventory_group, document_number, last_change DESC";
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
        }

        private void FormSelectExistingInventory_Load(object sender, EventArgs e)
        {
            FillExistingInventories();
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

        private void listViewInventory_Resize(object sender, EventArgs e)
        {
            listViewInventory.TileSize = new Size(listViewInventory.Width - 30, listViewInventory.TileSize.Height);
        }

        private void buttonAddEmpty_Click(object sender, EventArgs e)
        {
            string current_description = "";
            if (listViewInventory.SelectedItems.Count > 0)
                current_description = listViewInventory.SelectedItems[0].Text;

            FormAddEmptyInventory formAddEmpty = new FormAddEmptyInventory();
            if (formAddEmpty.ShowDialog() == DialogResult.OK)
                FillExistingInventories(current_description);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listViewInventory.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show(
                    "Вы действительно хотите удалить инвентаризацию?",
                    "Вопрос",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            DataModule.DeleteInventory(((InventoryInfo)listViewInventory.SelectedItems[0].Tag).ID);

            IsUpdateNeeded = true;

            FillExistingInventories();
        }
    }
}
