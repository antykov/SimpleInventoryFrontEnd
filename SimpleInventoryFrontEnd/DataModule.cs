using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        private string comment_for_descr => (string.IsNullOrWhiteSpace(Comment)) ? "" : " (" + Comment + ")";
        private string document_for_descr => (string.IsNullOrWhiteSpace(DocumentNumber)) ? "" : " №" + DocumentNumber;
        private string last_change_for_descr => (LastChange.Equals(new DateTime())) ? "" : ", обновление от " + LastChange.ToString("dd.MM.yyyy HH:mm:ss");

        public string Version;
        public string Company;
        public string CompanyCode;
        public string Warehouse;
        public string WarehouseCode;
        public string DocumentNumber;
        public string Comment;
        public DateTime Date;
        public DateTime LastChange;
        public long ID;

        public string Description => Warehouse + document_for_descr + " от " + Date.ToString("dd.MM.yyyy") + last_change_for_descr + comment_for_descr;
        public string FullDescriptionWithoutLastChange => Company + " (" + Warehouse + ")" + document_for_descr + " от " + Date.ToString("dd.MM.yyyy");
        public string FullDescription => FullDescriptionWithoutLastChange + last_change_for_descr + comment_for_descr;

        public InventoryInfo()
        {
            Version = "";
            Company = "";
            CompanyCode = "";
            Warehouse = "";
            WarehouseCode = "";
            DocumentNumber = "";
            Comment = "";
            Date = new DateTime();
            ID = 0;
        }

        public InventoryInfo(SQLiteDataReader reader)
        {
            foreach (var key in reader.GetValues().Keys)
            {
                SetValueByName(key.ToString(), reader.GetValues().GetValues(key.ToString())[0]);
            }
        }

        public override string ToString()
        {
            return this.FullDescription;
        }

        public void SetValueByName(string name, string value)
        {
            switch (name.ToUpper())
            {
                case "ID": Int64.TryParse(value, out ID); break;
                case "VERSION": Version = value; break;
                case "COMPANY": Company = value; break;
                case "COMPANY_CODE": CompanyCode = value; break;
                case "WAREHOUSE": Warehouse = value; break;
                case "WAREHOUSE_CODE": WarehouseCode = value; break;
                case "DOCUMENT_NUMBER": DocumentNumber = value; break;
                case "COMMENT": Comment = value; break;
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
        public decimal Price;

        public InventoryItem()
        {
            Code = "";
            Description = "";
            Barcode = "";
            Unit = "";
            Quantity = 0;
            QuantityFact = 0;
            Price = 0;
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
                case "PRICE":
                    Decimal.TryParse(value, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out Price);
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
        public static readonly string VERSION = "1.1.1";

        public static InventoryInfo inventoryInfo = null;

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

        public static void ConnectBarcodeScanner()
        {
            scanner.DeviceEnabled = false;
            if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.ScannerName))
            {
                int scannerIndex = 0;
                for (int i = 0; i < scanner.DeviceCount; i++)
                {
                    scanner.CurrentDeviceNumber = i + 1;
                    if (scanner.CurrentDeviceName.Equals(Properties.Settings.Default.ScannerName))
                    {
                        scannerIndex = i + 1;
                        break;
                    }
                }
                if (scannerIndex > 0)
                {
                    try
                    {
                        scanner.CurrentDeviceNumber = scannerIndex;
                        scanner.DataEvent += Scanner_DataEvent;
                        scanner.DeviceEnabled = true;
                        scannerConnected = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Ошибка подключения сканера штрих-кодов:\n" + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        scanner.DeviceEnabled = false;
                    }
                }
            }

            ((ToolStripStatusLabel)formElements["toolLabelScannerConnected"]).Text = (scannerConnected) ? "Сканер \"" + Properties.Settings.Default.ScannerName + "\" подключен" : "Сканер не подключен";
        }

        private static void Scanner_DataEvent()
        {
            if (formElements == null || inventoryInfo == null || barcode_index == -1)
                return;

            BindingSource bindingSource = (BindingSource)formElements["bindingSource"];
            DataGridView dataGridView = (DataGridView)formElements["gridInventory"];

            int position = bindingSource.Find("barcode", scanner.ScanData);
            if (position == -1)
            {
                if (Properties.Settings.Default.AddNewRowWhenBarcodeNotFound)
                    using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
                    {
                        sqliteCommand.CommandText = @"
                            INSERT INTO inventory_items (info_id, barcode, quantity, quantity_fact, price) 
                            VALUES (@info_id, @barcode, 0, 1, 0)";
                        sqliteCommand.Parameters.AddWithValue("info_id", inventoryInfo.ID);
                        sqliteCommand.Parameters.AddWithValue("barcode", scanner.ScanData);
                        sqliteCommand.ExecuteNonQuery();

                        sqliteCommand.CommandText = @"
                            SELECT rowid 
                            FROM inventory_items 
                            WHERE info_id = @info_id AND barcode = @barcode";
                        using (SQLiteDataReader reader = sqliteCommand.ExecuteReader())
                        {
                            if (reader.Read())
                                UpdateInventoryGrid(reader.GetInt64(0));
                        }
                    }
                else
                {
                    Console.Beep(800, 100);
                    Console.Beep(800, 100);
                    Console.Beep(800, 100);
                }

                return;
            }

            bindingSource.Position = position;

            if (quantity_fact_index == -1)
                return;

            dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView.CurrentCell = dataGridView.Rows[position].Cells[quantity_fact_index];
            dataGridView.BeginEdit(true);
            if (dataGridView.CurrentCell.Value.GetType().IsValueType)
                dataGridView.CurrentCell.Value = (decimal)dataGridView.CurrentCell.Value + 1;
            else
                dataGridView.CurrentCell.Value = (decimal)1;
            dataGridView.EndEdit();
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
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
                        (id                   INTEGER PRIMARY KEY AUTOINCREMENT,
                         date                 TEXT, 
                         last_change          TEXT, 
                         company              TEXT,
                         company_code         TEXT,
                         warehouse            TEXT, 
                         warehouse_code       TEXT,
                         document_number      TEXT,
                         comment              TEXT);
                    CREATE TABLE IF NOT EXISTS inventory_items
                        (info_id              INTEGER,
                         code                 TEXT, 
                         description          TEXT,
                         unit                 TEXT,
                         barcode              TEXT,
                         quantity             NUMERIC,
                         quantity_fact        NUMERIC,
                         price                NUMERIC);
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

                bool isPriceColumnExist = false;
                sqliteCommand.CommandText = @"PRAGMA table_info('inventory_items');";
                using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                {
                    while (sqliteReader.Read()) 
                        if (sqliteReader.GetString(1) == "price")
                        {
                            isPriceColumnExist = true;
                            break;
                        }
                }
                if (!isPriceColumnExist)
                {
                    sqliteCommand.CommandText = "ALTER TABLE inventory_items ADD COLUMN price NUMERIC";
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
                        warehouse_code = @warehouse_code AND
                        document_number = @document_number AND
                        comment = @comment";
            sqliteCommand.Parameters.AddWithValue("date", inventoryInfo.Date.ToString("yyyy-MM-dd"));
            sqliteCommand.Parameters.AddWithValue("company_code", inventoryInfo.CompanyCode);
            sqliteCommand.Parameters.AddWithValue("warehouse_code", inventoryInfo.WarehouseCode);
            sqliteCommand.Parameters.AddWithValue("document_number", inventoryInfo.DocumentNumber);
            sqliteCommand.Parameters.AddWithValue("comment", inventoryInfo.Comment);

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
                    DeleteInventory(id);
            }

            return true;
        }

        private static void DoImportInventoryDataToDB(SQLiteCommand sqliteCommand)
        {
            sqliteCommand.CommandText = @"
                INSERT INTO 
                    inventory_info (
                        date, 
                        company, company_code, 
                        warehouse, warehouse_code, 
                        document_number, 
                        comment)
                VALUES (
                    @date, 
                    @company, @company_code, 
                    @warehouse, @warehouse_code, 
                    @document_number,
                    @comment)";
            sqliteCommand.Parameters.AddWithValue("date", inventoryInfo.Date.ToString("yyyy-MM-dd"));
            sqliteCommand.Parameters.AddWithValue("company", inventoryInfo.Company);
            sqliteCommand.Parameters.AddWithValue("company_code", inventoryInfo.CompanyCode);
            sqliteCommand.Parameters.AddWithValue("warehouse", inventoryInfo.Warehouse);
            sqliteCommand.Parameters.AddWithValue("warehouse_code", inventoryInfo.WarehouseCode);
            sqliteCommand.Parameters.AddWithValue("document_number", inventoryInfo.DocumentNumber);
            sqliteCommand.Parameters.AddWithValue("comment", inventoryInfo.Comment);
            sqliteCommand.ExecuteNonQuery();

            sqliteCommand.CommandText = @"
                SELECT 
                    id
                FROM 
                    inventory_info 
                WHERE 
                    date = @date AND 
                    company_code = @company_code AND 
                    warehouse_code = @warehouse_code AND
                    document_number = @document_number AND
                    comment = @comment";
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
                            inventory_items (info_id, code, description, unit, barcode, quantity, quantity_fact, price)
                        VALUES (@info_id, @code, @description, @unit, @barcode, @quantity, 0, @price)";
                    sqliteCommand.Parameters.AddWithValue("info_id", inventoryInfo.ID);
                    sqliteCommand.Parameters.AddWithValue("code", item.Value.Code);
                    sqliteCommand.Parameters.AddWithValue("description", item.Value.Description);
                    sqliteCommand.Parameters.AddWithValue("unit", item.Value.Unit);
                    sqliteCommand.Parameters.AddWithValue("barcode", item.Value.Barcode);
                    sqliteCommand.Parameters.AddWithValue("quantity", item.Value.Quantity);
                    sqliteCommand.Parameters.AddWithValue("price", item.Value.Price);
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

        public static void UpdateInventoryItem(long rowid, string column_name, object column_value)
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

                    inventoryInfo.LastChange = DateTime.Now;

                    sqliteCommand.CommandText = @"
                        UPDATE inventory_info
                        SET last_change = @last_change
                        WHERE id = @info_id";
                    sqliteCommand.Parameters.AddWithValue("last_change", inventoryInfo.LastChange.ToString("yyyy-MM-dd HH:mm:ss"));
                    sqliteCommand.Parameters.AddWithValue("info_id", inventoryInfo.ID);
                    sqliteCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка обновления БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void AddEmptyInventory(InventoryInfo info)
        {
            try
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
                {
                    sqliteCommand.CommandText = @"
                        INSERT INTO 
                            inventory_info (
                                date, 
                                company, company_code, 
                                warehouse, warehouse_code, 
                                document_number,
                                comment)
                        VALUES (
                            @date, 
                            @company, @company_code, 
                            @warehouse, @warehouse_code, 
                            @document_number,
                            @comment)";
                    sqliteCommand.Parameters.AddWithValue("date", DateTime.Now.ToString("yyyy-MM-dd"));
                    sqliteCommand.Parameters.AddWithValue("company", info.Company);
                    sqliteCommand.Parameters.AddWithValue("company_code", info.CompanyCode);
                    sqliteCommand.Parameters.AddWithValue("warehouse", info.Warehouse);
                    sqliteCommand.Parameters.AddWithValue("warehouse_code", info.WarehouseCode);
                    sqliteCommand.Parameters.AddWithValue("document_number", "");
                    sqliteCommand.Parameters.AddWithValue("comment", info.Comment);
                    sqliteCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка добавления инвентаризации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void DeleteInventory(long id)
        {
            SQLiteTransaction sqliteTransaction = null;
            try
            {
                sqliteTransaction = sqliteConnection.BeginTransaction();
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
                {
                    sqliteCommand.CommandText = @"
                        DELETE FROM inventory_items
                        WHERE info_id = @id";
                    sqliteCommand.Parameters.AddWithValue("id", id);
                    sqliteCommand.ExecuteNonQuery();

                    sqliteCommand.CommandText = @"
                        DELETE FROM inventory_info
                        WHERE id = @id";
                    sqliteCommand.ExecuteNonQuery();

                    sqliteTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                sqliteTransaction?.Rollback();
                MessageBox.Show(e.Message, "Ошибка добавления инвентаризации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool CheckInventoryExisting(long id)
        {
            try
            {
                using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
                {
                    sqliteCommand.CommandText = @"
                        SELECT COUNT(*) 
                        FROM inventory_info
                        WHERE id = @id";
                    sqliteCommand.Parameters.AddWithValue("id", id);

                    if ((long)sqliteCommand.ExecuteScalar() > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка работы с БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        #endregion

        #region Чтение / запись файла с данными

        static SortedList<string, InventoryItem> inventoryItems;

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
                    if (inventoryInfo.Version != VERSION)
                        throw new Exception($"Версия файла выгрузки ({inventoryInfo.Version}) не соответствует версии утилиты ({VERSION})!");

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

        public static void ExportToXML(string file)
        {
            try
            {
                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Encoding = Encoding.GetEncoding("windows-1251");
                writerSettings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(file, writerSettings))
                {
                    writer.WriteProcessingInstruction("xml", "version = '1.0' encoding = 'windows-1251'");

                    writer.WriteStartElement("inventory_result");
                    writer.WriteAttributeString("version", VERSION);
                    writer.WriteAttributeString("company_code", inventoryInfo.CompanyCode);
                    writer.WriteAttributeString("company", inventoryInfo.Company);
                    writer.WriteAttributeString("warehouse_code", inventoryInfo.WarehouseCode);
                    writer.WriteAttributeString("warehouse", inventoryInfo.Warehouse);
                    writer.WriteAttributeString("document_number", inventoryInfo.DocumentNumber);
                    writer.WriteAttributeString("date", inventoryInfo.Date.ToString("yyyy-MM-dd"));
                    writer.WriteAttributeString("last_change", (inventoryInfo.LastChange.Equals(new DateTime())) ? "" : inventoryInfo.LastChange.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteAttributeString("id", inventoryInfo.ID.ToString());

                    writer.WriteStartElement("items");

                    using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
                    {
                        sqliteCommand.CommandText = @"
                            SELECT code, barcode, description, unit, quantity, quantity_fact
                            FROM inventory_items
                            WHERE info_id = @info_id";
                        if (Properties.Settings.Default.ExportDiffInventoryResults)
                            sqliteCommand.CommandText += " AND quantity <> quantity_fact";
                        sqliteCommand.Parameters.AddWithValue("info_id", inventoryInfo.ID);
                        using (SQLiteDataReader sqliteReader = sqliteCommand.ExecuteReader())
                        {
                            while (sqliteReader.Read())
                            {
                                writer.WriteStartElement("item");
                                writer.WriteElementString("code", sqliteReader.GetValue(0).ToString());
                                writer.WriteElementString("barcode", sqliteReader.GetValue(1).ToString());
                                writer.WriteElementString("description", sqliteReader.GetValue(2).ToString());
                                writer.WriteElementString("unit", sqliteReader.GetValue(3).ToString());
                                writer.WriteElementString("quantity", sqliteReader.GetDecimal(4).ToString("0.000", CultureInfo.InvariantCulture));
                                writer.WriteElementString("quantity_fact", sqliteReader.GetDecimal(5).ToString("0.000", CultureInfo.InvariantCulture));
                                writer.WriteEndElement();
                            }
                        }
                    }

                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка экспорта в XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region DataGridView

        public static DataGridViewCellStyle regularQuantityStyle, boldQuantityStyle;
        public static int rowid_index = -1, description_index = -1, barcode_index = -1, quantity_fact_index = -1;
        public static Hashtable formElements = null;

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

            DataGridViewColumnAppearance columnPrice = new DataGridViewColumnAppearance();
            columnPrice.HeaderText = "Цена";
            columnPrice.Width = 70;
            columnPrice.Alignment = DataGridViewContentAlignment.MiddleRight;
            columnPrice.Format = "N3";
            columnsAppearance.Add("price", columnPrice);

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
                case "description": description_index = column.Index; break;
                case "barcode": barcode_index = column.Index; break;
                case "quantity_fact": quantity_fact_index = column.Index; break;
            }
        }

        public static void ApplyDataGridCellsStyle()
        {
            DataGridView dataGridView = (DataGridView)formElements["gridInventory"];

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (description_index != -1)
                {
                    if (String.IsNullOrWhiteSpace(dataGridView.Rows[i].Cells[description_index].Value.ToString()))
                        dataGridView.Rows[i].Cells[description_index].ReadOnly = false;
                }
                if (quantity_fact_index != -1)
                {
                    decimal value = 0;
                    if (Decimal.TryParse(dataGridView.Rows[i].Cells[quantity_fact_index].Value.ToString(), out value))
                    {
                        if (value != 0)
                            dataGridView.Rows[i].Cells[quantity_fact_index].Style.ApplyStyle(DataModule.boldQuantityStyle);
                    }
                }
            }
        }

        public static void UpdateInventoryGrid(long rowid = -1)
        {
            if (formElements == null)
                return;

            BindingSource bindingSource = (BindingSource)formElements["bindingSource"];
            DataGridView dataGridView = (DataGridView)formElements["gridInventory"];

            ((ToolStripStatusLabel)formElements["toolLabelInventoryInfo"]).Text = inventoryInfo.FullDescriptionWithoutLastChange;

            DataTable table = new DataTable();

            using (SQLiteCommand sqliteCommand = new SQLiteCommand(sqliteConnection))
            {
                sqliteCommand.CommandText = @"
                    SELECT rowid, info_id, code, barcode, description, unit, price, quantity, quantity_fact
                    FROM inventory_items
                    WHERE info_id = @info_id
                    ORDER BY description";
                sqliteCommand.Parameters.AddWithValue("info_id", inventoryInfo.ID);
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqliteCommand))
                {
                    adapter.Fill(table);
                    bindingSource.DataSource = table;
                }
            }

            dataGridView.AutoGenerateColumns = true;

            int position = bindingSource.Find("rowid", rowid);
            if (position != -1 && quantity_fact_index != -1)
            {
                bindingSource.Position = position;
                dataGridView.CurrentCell = dataGridView.Rows[position].Cells[quantity_fact_index];
            }

            ApplyDataGridCellsStyle();
        }

        public static int FindRow(int column_index, string value)
        {
            if (formElements == null)
                return -1;

            DataGridView dataGridView = (DataGridView)formElements["gridInventory"];

            for (int i = 0; i < dataGridView.Rows.Count; i++)
                if (dataGridView.Rows[i].Cells[column_index].Value.ToString().StartsWith(value, StringComparison.InvariantCultureIgnoreCase))
                    return i;

            return -1;
        }

        #endregion
    }
}
