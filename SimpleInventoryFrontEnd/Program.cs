using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleInventoryFrontEnd
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (!DataModule.CheckAtolBarcodeScanner())
                return;
            if (!DataModule.EstablishSQLiteConnection())
                return;

            DataModule.SetDataGridColumnsAppearance();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            DataModule.sqliteConnection.Dispose();
        }
    }
}
