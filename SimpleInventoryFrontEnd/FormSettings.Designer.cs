namespace SimpleInventoryFrontEnd
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelScanerDeviceName = new System.Windows.Forms.Label();
            this.buttonShowScannerProperties = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxScannerName = new System.Windows.Forms.ComboBox();
            this.checkBoxNewRow = new System.Windows.Forms.CheckBox();
            this.checkBoxExportDiff = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelScanerDeviceName
            // 
            this.labelScanerDeviceName.AutoSize = true;
            this.labelScanerDeviceName.Location = new System.Drawing.Point(7, 15);
            this.labelScanerDeviceName.Name = "labelScanerDeviceName";
            this.labelScanerDeviceName.Size = new System.Drawing.Size(143, 13);
            this.labelScanerDeviceName.TabIndex = 1;
            this.labelScanerDeviceName.Text = "Имя сканера штрих-кодов:";
            // 
            // buttonShowScannerProperties
            // 
            this.buttonShowScannerProperties.Location = new System.Drawing.Point(353, 12);
            this.buttonShowScannerProperties.Name = "buttonShowScannerProperties";
            this.buttonShowScannerProperties.Size = new System.Drawing.Size(196, 23);
            this.buttonShowScannerProperties.TabIndex = 2;
            this.buttonShowScannerProperties.Text = "Показать свойства драйвера...";
            this.buttonShowScannerProperties.UseVisualStyleBackColor = true;
            this.buttonShowScannerProperties.Click += new System.EventHandler(this.buttonShowScannerProperties_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(454, 87);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(95, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(353, 87);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(95, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "Сохранить";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxScannerName
            // 
            this.comboBoxScannerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScannerName.FormattingEnabled = true;
            this.comboBoxScannerName.Location = new System.Drawing.Point(156, 12);
            this.comboBoxScannerName.Name = "comboBoxScannerName";
            this.comboBoxScannerName.Size = new System.Drawing.Size(191, 21);
            this.comboBoxScannerName.TabIndex = 5;
            // 
            // checkBoxNewRow
            // 
            this.checkBoxNewRow.AutoSize = true;
            this.checkBoxNewRow.Location = new System.Drawing.Point(261, 41);
            this.checkBoxNewRow.Name = "checkBoxNewRow";
            this.checkBoxNewRow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxNewRow.Size = new System.Drawing.Size(288, 17);
            this.checkBoxNewRow.TabIndex = 6;
            this.checkBoxNewRow.Text = "Добавлять новую строку, если штрихкод не найден";
            this.checkBoxNewRow.UseVisualStyleBackColor = false;
            // 
            // checkBoxExportDiff
            // 
            this.checkBoxExportDiff.AutoSize = true;
            this.checkBoxExportDiff.Location = new System.Drawing.Point(188, 64);
            this.checkBoxExportDiff.Name = "checkBoxExportDiff";
            this.checkBoxExportDiff.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxExportDiff.Size = new System.Drawing.Size(361, 17);
            this.checkBoxExportDiff.TabIndex = 7;
            this.checkBoxExportDiff.Text = "Экспортировать результаты инвентаризации только с отличиями";
            this.checkBoxExportDiff.UseVisualStyleBackColor = false;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(559, 119);
            this.Controls.Add(this.checkBoxExportDiff);
            this.Controls.Add(this.checkBoxNewRow);
            this.Controls.Add(this.comboBoxScannerName);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonShowScannerProperties);
            this.Controls.Add(this.labelScanerDeviceName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelScanerDeviceName;
        private System.Windows.Forms.Button buttonShowScannerProperties;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxScannerName;
        private System.Windows.Forms.CheckBox checkBoxNewRow;
        private System.Windows.Forms.CheckBox checkBoxExportDiff;
    }
}