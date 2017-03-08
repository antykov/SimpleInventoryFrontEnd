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
    public partial class FormAddEmptyInventory : Form
    {
        public FormAddEmptyInventory()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DataModule.AddEmptyInventory(new InventoryInfo()
            {
                CompanyCode = textBoxCompanyCode.Text,
                Company = textBoxCompany.Text,
                WarehouseCode = textBoxWarehouseCode.Text,
                Warehouse = textBoxWarehouse.Text,
                Comment = textBoxComment.Text
            });

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
