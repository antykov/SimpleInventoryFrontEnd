using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SimpleInventoryFrontEnd
{
    public class InventoryInfo: Object
    {
        public string Company;
        public string CompanyCode;
        public string Warehouse;
        public string WarehouseCode;
        public DateTime Date;
        public DateTime LastChange;
        public long ID;

        public string Description;

        public InventoryInfo()
        {
            Company = "";
            CompanyCode = "";
            Warehouse = "";
            WarehouseCode = "";
            Date = new DateTime();
            ID = 0;
        }

        public InventoryInfo(SQLiteDataReader reader)
        {
            foreach (var key in reader.GetValues().Keys)
            {
                SetValueByName(key.ToString(), reader.GetValues().GetValues(key.ToString())[0]);
            }

            Description =
                Company + " (" +
                Warehouse + ") от " +
                Date.ToString("dd.MM.yyyy");

            if (!LastChange.Equals(new DateTime()))
                Description += ", обновление от " + LastChange.ToString("dd.MM.yyyy hh:mm:ss");
        }

        public override string ToString()
        {
            return this.Description;
        }

        public void SetValueByName(string name, string value)
        {
            switch (name.ToUpper())
            {
                case "ID": Int64.TryParse(value, out ID); break;
                case "COMPANY": Company = value; break;
                case "COMPANY_CODE": CompanyCode = value; break;
                case "WAREHOUSE": Warehouse = value; break;
                case "WAREHOUSE_CODE": WarehouseCode = value; break;
                case "DATE": DateTime.TryParse(value, out Date); break;
                case "LAST_CHANGE": DateTime.TryParse(value, out LastChange); break;
            }
        }

        public bool CheckMandatoryFields()
        {
            if (String.IsNullOrWhiteSpace(CompanyCode) || String.IsNullOrWhiteSpace(WarehouseCode) || Date.Equals(new DateTime()))
                return false;

            return true;
        }
    }

    public class InventoryItem : Object
    {
        public string Code;
        public string Description;
        public string Barcode;
        public string Unit;
        public decimal Quantity;
        public decimal QuantityFact;

        public InventoryItem()
        {
            Code = "";
            Description = "";
            Barcode = "";
            Unit = "";
            Quantity = 0;
            QuantityFact = 0;
        }

        public void SetValueByName(string name, string value)
        {
            switch (name.ToUpper())
            {
                case "CODE": Code = value; break;
                case "DESCRIPTION": Description = value; break;
                case "BARCODE": Barcode= value; break;
                case "UNIT": Unit = value; break;
                case "QUANTITY":
                    Decimal.TryParse(value, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Quantity);
                    break;
            }
        }
    }

    public class DataGridViewColumnAppearance: Object
    {
        public bool Visible;
        public string HeaderText;
        public DataGridViewAutoSizeColumnMode AutoSizeMode;
        public bool ReadOnly;
        public int Width;
        public string Format;
        public DataGridViewContentAlignment Alignment;

        public DataGridViewColumnAppearance()
        {
            Visible = true;
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            ReadOnly = true;
            Width = 100;
            Format = "";
            Alignment = DataGridViewContentAlignment.MiddleLeft;
        }
    }

    public static class DataModule
    {
        #region Сканер штрихкодов

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        internal delegate int PointerToMethodInvoker();

        static public Scaner.Scaner45 scanner;
        static public bool scannerConnected;

        public static bool CheckAtolBarcodeScanner()
        {
            scannerConnected = false;

            if (Type.GetTypeFromProgID("AddIn.Scaner45") == null)
            {
                string scanerDllPath = Path.Combine(Environment.CurrentDirectory, "Scaner1C.dll");
                if (!File.Exists(scanerDllPath))
                {
                    MessageBox.Show("Отсутствует библиотека Scaner1C.dll!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                try
                {
                    IntPtr hLib = LoadLibrary(scanerDllPath);
                    if (hLib == IntPtr.Zero)
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    IntPtr dllEntryPoint = GetProcAddress(hLib, "DllRegisterServer");
                    if (dllEntryPoint == IntPtr.Zero)
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                    PointerToMethodInvoker dllRegisterDelegate =
                           (PointerToMethodInvoker)Marshal.GetDelegateForFunctionPointer(dllEntryPoint, typeof(PointerToMethodInvoker));
                    dllRegisterDelegate();

                    FreeLibrary(hLib);

                    if (Type.GetTypeFromProgID("AddIn.Scaner45") == null)
                        throw new Exception("Не удалось зарегистрировать библиотеку Scaner1C.dll!");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Ошибка регистрации библиотеки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            scanner = new Scaner.Scaner45();

            return true;
        }

        #endregion

        #region SQLite

        static public SQLiteConnection sqliteConnection;

        public static bool EstablishSQLiteConnection()
        {
            sqliteConnection = new SQLiteConnection("Data Source=inventory.db; Version=3;");
            try
            {
                sqliteConnection.Open();
            }
            catch (SQLiteException e)
            {
                MessageBox.Show(e.Message, "Ошибка открытия БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
            {
                sqliteCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS inventory_info
                        (id             INTEGER PRIMARY KEY AUTOINCREMENT,
                         date           TEXT, 
                         last_change    TEXT, 
                         company        TEXT,
                         company_code   TEXT,
                         warehouse      TEXT, 
                         warehouse_code TEXT);
                    CREATE TABLE IF NOT EXISTS inventory_items
                        (info_id        INTEGER,
                         code           TEXT, 
                         description    TEXT,
                         unit           TEXT,
                         barcode        TEXT,
                         quantity       NUMERIC,
                         quantity_fact  NUMERIC);
                    ";
                try
                {
                    sqliteCommand.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    MessageBox.Show(e.Message, "Ошибка работы с БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public static void UpdateSQLiteRow(long rowid, long info_id, string column_name, object column_value)
        {
            try
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
                {
                    sqliteCommand.CommandText = @"
                        UPDATE inventory_items
                        SET " + column_name + @" = @column_value
                        WHERE rowid = @rowid";
                    sqliteCommand.Parameters.AddWithValue("column_value", column_value);
                    sqliteCommand.Parameters.AddWithValue("rowid", rowid);
                    sqliteCommand.ExecuteNonQuery();

                    sqliteCommand.CommandText = @"
                        UPDATE inventory_info
                        SET last_change = @last_change
                        WHERE id = @info_id";
                    sqliteCommand.Parameters.AddWithValue("last_change", DateTime.Now.ToString("u"));
                    sqliteCommand.Parameters.AddWithValue("info_id", info_id);
                    sqliteCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка обновления БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Данные с инвентаризацией

        public static InventoryInfo inventoryInfo;
        public static SortedList<string, InventoryItem> inventoryItems;

        static InventoryItem ReadInventoryItem(XmlReader reader)
        {
            InventoryItem item = new InventoryItem();

            string name;

            while (reader.Read())
            {
                if (reader.Name == "item" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Element)
                {
                    name = reader.Name;
                    if (reader.Read() && reader.NodeType == XmlNodeType.Text)
                        item.SetValueByName(name, reader.Value);
                }
            }

            if (String.IsNullOrWhiteSpace(item.Code))
                return null;

            return item;
        }

        public static bool LoadFile(string file)
        {
            inventoryInfo = new InventoryInfo();
            inventoryItems = new SortedList<string, InventoryItem>();

            try
            {
                using (XmlReader reader = XmlReader.Create(file))
                {
                    if (reader.MoveToContent() != XmlNodeType.Element || reader.Name != "inventory")
                        throw new Exception("В выбранном файле отсутствует информация по инвентаризации!");

                    if (reader.HasAttributes)
                    {
                        for (int i = 0; i < reader.AttributeCount; i++)
                        {
                            reader.MoveToAttribute(i);
                            inventoryInfo.SetValueByName(reader.Name, reader.Value);
                        }
                        reader.MoveToElement();
                    }
                    if (!inventoryInfo.CheckMandatoryFields())
                        throw new Exception("В выбранном файле отсутствуют атрибуты, описывающие инвентаризацию!");

                    if (reader.ReadToFollowing("item"))
                    {
                        do
                        {
                            InventoryItem item = ReadInventoryItem(reader);
                            if (item != null)
                                inventoryItems.Add(item.Description + " / " + item.Code, item);
                        } while (reader.ReadToFollowing("item"));
                    }
                }

                if (inventoryItems.Count == 0)
                    throw new Exception("В выбранном файле отсутствуют строки с номенклатурой!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка чтения файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
            {
                try
                {
                    if (!PrepareDBtoImportInventoryData(sqliteCommand))
                        return false;

                    DoImportInventoryDataToDB(sqliteCommand);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Ошибка работы с БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private static bool PrepareDBtoImportInventoryData(SQLiteCommand sqliteCommand)
        {
            sqliteCommand.CommandText = @"
                    SELECT 
                        id
                    FROM 
                        inventory_info 
                    WHERE 
                        date = @date AND 
                        company_code = @company_code AND 
                        warehouse_code = @warehouse_code";
            sqliteCommand.Parameters.AddWithValue("date", inventoryInfo.Date.ToString("yyyy-MM-dd"));
            sqliteCommand.Parameters.AddWithValue("company_code", inventoryInfo.CompanyCode);
            sqliteCommand.Parameters.AddWithValue("warehouse_code", inventoryInfo.WarehouseCode);

            List<long> inventory_ids = new List<long>();
            using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
            {
                while (sqliteReader.Read())
                    inventory_ids.Add(sqliteReader.GetInt64(0));
            }

            if (inventory_ids.Count > 0)
            {
                if (MessageBox.Show(
                        "Данный файл уже загружался в базу данных!\nВы действительно хотите загрузить заново (данные будут очищены)?",
                        "Вопрос",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return false;

                foreach (var id in inventory_ids)
                {
                    sqliteCommand.CommandText = "DELETE FROM inventory_items WHERE info_id = @id";
                    sqliteCommand.Parameters.AddWithValue("id", id);
                    sqliteCommand.ExecuteNonQuery();

                    sqliteCommand.CommandText = "DELETE FROM inventory_info WHERE id = @id";
                    sqliteCommand.Parameters.AddWithValue("id", id);
                    sqliteCommand.ExecuteNonQuery();
                }
            }

            return true;
        }

        private static void DoImportInventoryDataToDB(SQLiteCommand sqliteCommand)
        {
            sqliteCommand.CommandText = @"
                INSERT INTO 
                    inventory_info (date, company, company_code, warehouse, warehouse_code)
                VALUES (@date, @company, @company_code, @warehouse, @warehouse_code)";
            sqliteCommand.Parameters.AddWithValue("date", inventoryInfo.Date.ToString("yyyy-MM-dd"));
            sqliteCommand.Parameters.AddWithValue("company", inventoryInfo.Company);
            sqliteCommand.Parameters.AddWithValue("company_code", inventoryInfo.CompanyCode);
            sqliteCommand.Parameters.AddWithValue("warehouse", inventoryInfo.Warehouse);
            sqliteCommand.Parameters.AddWithValue("warehouse_code", inventoryInfo.WarehouseCode);
            sqliteCommand.ExecuteNonQuery();

            sqliteCommand.CommandText = @"
                SELECT 
                    id
                FROM 
                    inventory_info 
                WHERE 
                    date = @date AND 
                    company_code = @company_code AND 
                    warehouse_code = @warehouse_code";
            using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
            {
                if (sqliteReader.Read())
                    inventoryInfo.ID = sqliteReader.GetInt64(0);
                else
                    throw new SQLiteException("Не удалось добавить запись в таблицу inventory_info!");
            }

            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            try
            {
                foreach (var item in inventoryItems)
                {
                    sqliteCommand.CommandText = @"
                        INSERT INTO 
                            inventory_items (info_id, code, description, unit, barcode, quantity)
                        VALUES (@info_id, @code, @description, @unit, @barcode, @quantity)";
                    sqliteCommand.Parameters.AddWithValue("info_id", inventoryInfo.ID);
                    sqliteCommand.Parameters.AddWithValue("code", item.Value.Code);
                    sqliteCommand.Parameters.AddWithValue("description", item.Value.Description);
                    sqliteCommand.Parameters.AddWithValue("unit", item.Value.Unit);
                    sqliteCommand.Parameters.AddWithValue("barcode", item.Value.Barcode);
                    sqliteCommand.Parameters.AddWithValue("quantity", item.Value.Quantity);
                    sqliteCommand.ExecuteNonQuery();
                }
                sqliteTransaction.Commit();
            }
            catch (Exception)
            {
                sqliteTransaction.Rollback();
                throw;
            }
        }

        #endregion

        #region DataGridView

        public static int rowid_index = -1, info_id_index = -1;

        public static Dictionary<string, DataGridViewColumnAppearance> columnsAppearance;

        public static void SetDataGridColumnsAppearance()
        {
            columnsAppearance = new Dictionary<string, DataGridViewColumnAppearance>();

            DataGridViewColumnAppearance columnRowid = new DataGridViewColumnAppearance();
            columnRowid.Visible = false;
            columnsAppearance.Add("rowid", columnRowid);

            DataGridViewColumnAppearance columnInfoId = new DataGridViewColumnAppearance();
            columnInfoId.Visible = false;
            columnsAppearance.Add("info_id", columnInfoId);

            DataGridViewColumnAppearance columnCode = new DataGridViewColumnAppearance();
            columnCode.HeaderText = "Код";
            columnCode.Width = 60;
            columnsAppearance.Add("code", columnCode);

            DataGridViewColumnAppearance columnBarcode = new DataGridViewColumnAppearance();
            columnBarcode.HeaderText = "Штрихкод";
            columnsAppearance.Add("barcode", columnBarcode);

            DataGridViewColumnAppearance columnDescription = new DataGridViewColumnAppearance();
            columnDescription.HeaderText = "Наименование";
            columnDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            columnsAppearance.Add("description", columnDescription);

            DataGridViewColumnAppearance columnUnit = new DataGridViewColumnAppearance();
            columnUnit.HeaderText = "Ед.";
            columnUnit.Width = 50;
            columnUnit.Alignment = DataGridViewContentAlignment.MiddleCenter;
            columnsAppearance.Add("unit", columnUnit);

            DataGridViewColumnAppearance columnQuantity = new DataGridViewColumnAppearance();
            columnQuantity.HeaderText = "Количество";
            columnQuantity.Width = 70;
            columnQuantity.Alignment = DataGridViewContentAlignment.MiddleRight;
            columnQuantity.Format = "N3";
            columnsAppearance.Add("quantity", columnQuantity);

            DataGridViewColumnAppearance columnQuantityFact = new DataGridViewColumnAppearance();
            columnQuantityFact.HeaderText = "Количество (факт.)";
            columnQuantityFact.ReadOnly = false;
            columnQuantityFact.Width = 70;
            columnQuantityFact.Format = "N3";
            columnQuantityFact.Alignment = DataGridViewContentAlignment.MiddleRight;
            columnsAppearance.Add("quantity_fact", columnQuantityFact);
        }

        public static void ApplyDataGridColumnAppearance(DataGridViewColumn column)
        {
            if (!columnsAppearance.ContainsKey(column.Name))
                return;

            DataGridViewColumnAppearance appearance = columnsAppearance[column.Name];

            column.Visible = appearance.Visible;
            column.HeaderText = appearance.HeaderText;
            column.AutoSizeMode = appearance.AutoSizeMode;
            column.ReadOnly = appearance.ReadOnly;
            column.Width = appearance.Width;
            column.DefaultCellStyle.Format = appearance.Format;
            column.DefaultCellStyle.Alignment = appearance.Alignment;

            switch (column.Name)
            {
                case "rowid": rowid_index = column.Index; break;
                case "info_id": info_id_index = column.Index; break;
            }
        }

        #endregion
    }
}
