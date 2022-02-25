namespace SimsigImporter
{
    partial class Form1
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
            this.comboSim = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textVersion = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.textName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textStart = new System.Windows.Forms.MaskedTextBox();
            this.textEnd = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textTemplate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAddSpreadsheet = new System.Windows.Forms.Button();
            this.comboDays = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboSim
            // 
            this.comboSim.FormattingEnabled = true;
            this.comboSim.Location = new System.Drawing.Point(89, 17);
            this.comboSim.Name = "comboSim";
            this.comboSim.Size = new System.Drawing.Size(227, 21);
            this.comboSim.TabIndex = 0;
            this.comboSim.Text = "Wolverhampton";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Simulation";
            // 
            // textVersion
            // 
            this.textVersion.Location = new System.Drawing.Point(89, 44);
            this.textVersion.Mask = "0\\.0\\.0";
            this.textVersion.Name = "textVersion";
            this.textVersion.Size = new System.Drawing.Size(100, 20);
            this.textVersion.TabIndex = 2;
            this.textVersion.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Version";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(674, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(114, 23);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(89, 70);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(227, 20);
            this.textName.TabIndex = 6;
            this.textName.Text = "A name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Name";
            // 
            // textDesc
            // 
            this.textDesc.Location = new System.Drawing.Point(89, 96);
            this.textDesc.Multiline = true;
            this.textDesc.Name = "textDesc";
            this.textDesc.Size = new System.Drawing.Size(493, 258);
            this.textDesc.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Description";
            // 
            // textStart
            // 
            this.textStart.Location = new System.Drawing.Point(89, 360);
            this.textStart.Mask = "00\\:00";
            this.textStart.Name = "textStart";
            this.textStart.Size = new System.Drawing.Size(100, 20);
            this.textStart.TabIndex = 10;
            this.textStart.Text = "0000";
            this.textStart.ValidatingType = typeof(System.DateTime);
            // 
            // textEnd
            // 
            this.textEnd.Location = new System.Drawing.Point(299, 360);
            this.textEnd.Mask = "00\\:00";
            this.textEnd.Name = "textEnd";
            this.textEnd.Size = new System.Drawing.Size(100, 20);
            this.textEnd.TabIndex = 11;
            this.textEnd.Text = "2700";
            this.textEnd.ValidatingType = typeof(System.DateTime);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 363);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Start";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(270, 365);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "End";
            // 
            // textTemplate
            // 
            this.textTemplate.Location = new System.Drawing.Point(89, 386);
            this.textTemplate.Name = "textTemplate";
            this.textTemplate.Size = new System.Drawing.Size(493, 20);
            this.textTemplate.TabIndex = 14;
            this.textTemplate.Text = "$originTime $originName-$destName ($stock)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 389);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Template";
            // 
            // btnAddSpreadsheet
            // 
            this.btnAddSpreadsheet.Location = new System.Drawing.Point(674, 41);
            this.btnAddSpreadsheet.Name = "btnAddSpreadsheet";
            this.btnAddSpreadsheet.Size = new System.Drawing.Size(114, 23);
            this.btnAddSpreadsheet.TabIndex = 16;
            this.btnAddSpreadsheet.Text = "Add spreadsheet";
            this.btnAddSpreadsheet.UseVisualStyleBackColor = true;
            this.btnAddSpreadsheet.Click += new System.EventHandler(this.btnAddSpreadsheet_Click);
            // 
            // comboDays
            // 
            this.comboDays.FormattingEnabled = true;
            this.comboDays.Items.AddRange(new object[] {
            "All - single TT",
            "All - separate TTs",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            this.comboDays.Location = new System.Drawing.Point(461, 17);
            this.comboDays.Name = "comboDays";
            this.comboDays.Size = new System.Drawing.Size(121, 21);
            this.comboDays.TabIndex = 17;
            this.comboDays.Text = "All - single TT";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(385, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Day to export";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(674, 69);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(114, 23);
            this.btnReset.TabIndex = 19;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(674, 97);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 23);
            this.btnGenerate.TabIndex = 20;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboDays);
            this.Controls.Add(this.btnAddSpreadsheet);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textTemplate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textEnd);
            this.Controls.Add(this.textStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textDesc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboSim);
            this.Name = "Form1";
            this.Text = "Simsig Timetable Importer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboSim;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox textVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textDesc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox textStart;
        private System.Windows.Forms.MaskedTextBox textEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textTemplate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAddSpreadsheet;
        private System.Windows.Forms.ComboBox comboDays;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGenerate;
    }
}

