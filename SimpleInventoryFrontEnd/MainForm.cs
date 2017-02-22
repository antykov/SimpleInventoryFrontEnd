using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using Scaner;

namespace SimpleInventoryFrontEnd
{
    public partial class MainForm : Form
    {
        //Scaner45 mScaner;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();
            DialogResult result = formSettings.ShowDialog();
        }
    }
}
