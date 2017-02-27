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
            this.buttonCancel.Location = new System.Drawing.Point(454, 48);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(95, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(353, 48);
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
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(559, 79);
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
    }
}