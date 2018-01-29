namespace Sigesoft.Node.WinClient.UI.UserControls
{
    partial class ucOftalmologia
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbILAE = new System.Windows.Forms.ComboBox();
            this.cbDLAE = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbILCC = new System.Windows.Forms.ComboBox();
            this.cbDLCC = new System.Windows.Forms.ComboBox();
            this.cbILSC = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDLSC = new System.Windows.Forms.ComboBox();
            this.txtDiag = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbICCC2 = new System.Windows.Forms.ComboBox();
            this.cbDCCC2 = new System.Windows.Forms.ComboBox();
            this.chkFiguras = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEdad = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbICCC = new System.Windows.Forms.ComboBox();
            this.cbDCCC = new System.Windows.Forms.ComboBox();
            this.cbICSC = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cbDCSC = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbILAE);
            this.groupBox1.Controls.Add(this.cbDLAE);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbILCC);
            this.groupBox1.Controls.Add(this.cbDLCC);
            this.groupBox1.Controls.Add(this.cbILSC);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbDLSC);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(443, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visión de Lejos";
            // 
            // cbILAE
            // 
            this.cbILAE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbILAE.FormattingEnabled = true;
            this.cbILAE.Location = new System.Drawing.Point(329, 59);
            this.cbILAE.Name = "cbILAE";
            this.cbILAE.Size = new System.Drawing.Size(93, 21);
            this.cbILAE.TabIndex = 22;
            this.cbILAE.SelectedValueChanged += new System.EventHandler(this.cbILAE_SelectedValueChanged);
            // 
            // cbDLAE
            // 
            this.cbDLAE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDLAE.FormattingEnabled = true;
            this.cbDLAE.Location = new System.Drawing.Point(329, 32);
            this.cbDLAE.Name = "cbDLAE";
            this.cbDLAE.Size = new System.Drawing.Size(93, 21);
            this.cbDLAE.TabIndex = 21;
            this.cbDLAE.SelectedValueChanged += new System.EventHandler(this.cbDLAE_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Coral;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(329, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "AE";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(196, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Con Corrector";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(97, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Sin Corrector";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbILCC
            // 
            this.cbILCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbILCC.FormattingEnabled = true;
            this.cbILCC.Location = new System.Drawing.Point(196, 59);
            this.cbILCC.Name = "cbILCC";
            this.cbILCC.Size = new System.Drawing.Size(93, 21);
            this.cbILCC.TabIndex = 13;
            this.cbILCC.SelectedValueChanged += new System.EventHandler(this.cbICC_SelectedValueChanged);
            // 
            // cbDLCC
            // 
            this.cbDLCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDLCC.FormattingEnabled = true;
            this.cbDLCC.Location = new System.Drawing.Point(196, 32);
            this.cbDLCC.Name = "cbDLCC";
            this.cbDLCC.Size = new System.Drawing.Size(93, 21);
            this.cbDLCC.TabIndex = 11;
            this.cbDLCC.SelectedValueChanged += new System.EventHandler(this.cbDCC_SelectedValueChanged);
            // 
            // cbILSC
            // 
            this.cbILSC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbILSC.FormattingEnabled = true;
            this.cbILSC.Location = new System.Drawing.Point(97, 59);
            this.cbILSC.Name = "cbILSC";
            this.cbILSC.Size = new System.Drawing.Size(93, 21);
            this.cbILSC.TabIndex = 12;
            this.cbILSC.SelectedValueChanged += new System.EventHandler(this.cbISC_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "OD";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(73, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "OI";
            // 
            // cbDLSC
            // 
            this.cbDLSC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDLSC.FormattingEnabled = true;
            this.cbDLSC.Location = new System.Drawing.Point(97, 32);
            this.cbDLSC.Name = "cbDLSC";
            this.cbDLSC.Size = new System.Drawing.Size(93, 21);
            this.cbDLSC.TabIndex = 10;
            this.cbDLSC.SelectedValueChanged += new System.EventHandler(this.cbDSC_SelectedValueChanged);
            // 
            // txtDiag
            // 
            this.txtDiag.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtDiag.Location = new System.Drawing.Point(42, 201);
            this.txtDiag.Multiline = true;
            this.txtDiag.Name = "txtDiag";
            this.txtDiag.ReadOnly = true;
            this.txtDiag.Size = new System.Drawing.Size(509, 150);
            this.txtDiag.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Dx.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cbICCC2);
            this.groupBox2.Controls.Add(this.cbDCCC2);
            this.groupBox2.Controls.Add(this.chkFiguras);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtEdad);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cbICCC);
            this.groupBox2.Controls.Add(this.cbDCCC);
            this.groupBox2.Controls.Add(this.cbICSC);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.cbDCSC);
            this.groupBox2.Location = new System.Drawing.Point(5, 104);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(563, 91);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Visión de Cerca";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightBlue;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(327, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "2do Corrector";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbICCC2
            // 
            this.cbICCC2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbICCC2.FormattingEnabled = true;
            this.cbICCC2.Location = new System.Drawing.Point(327, 59);
            this.cbICCC2.Name = "cbICCC2";
            this.cbICCC2.Size = new System.Drawing.Size(93, 21);
            this.cbICCC2.TabIndex = 28;
            this.cbICCC2.SelectedValueChanged += new System.EventHandler(this.cbICCC2_SelectedValueChanged);
            // 
            // cbDCCC2
            // 
            this.cbDCCC2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDCCC2.FormattingEnabled = true;
            this.cbDCCC2.Location = new System.Drawing.Point(327, 32);
            this.cbDCCC2.Name = "cbDCCC2";
            this.cbDCCC2.Size = new System.Drawing.Size(93, 21);
            this.cbDCCC2.TabIndex = 27;
            this.cbDCCC2.SelectedValueChanged += new System.EventHandler(this.cbDCCC2_SelectedValueChanged);
            // 
            // chkFiguras
            // 
            this.chkFiguras.AutoSize = true;
            this.chkFiguras.Location = new System.Drawing.Point(469, 71);
            this.chkFiguras.Name = "chkFiguras";
            this.chkFiguras.Size = new System.Drawing.Size(60, 17);
            this.chkFiguras.TabIndex = 26;
            this.chkFiguras.Text = "Figuras";
            this.chkFiguras.UseVisualStyleBackColor = true;
            this.chkFiguras.CheckedChanged += new System.EventHandler(this.chkFiguras_CheckedChanged);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Coral;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(453, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Tipo Ev.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEdad
            // 
            this.txtEdad.Location = new System.Drawing.Point(453, 32);
            this.txtEdad.Name = "txtEdad";
            this.txtEdad.ReadOnly = true;
            this.txtEdad.Size = new System.Drawing.Size(93, 20);
            this.txtEdad.TabIndex = 24;
            this.txtEdad.TextChanged += new System.EventHandler(this.txtEdad_TextChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Coral;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(453, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Edad";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightBlue;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(196, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Con Corrector";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightBlue;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(97, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Sin Corrector";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbICCC
            // 
            this.cbICCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbICCC.FormattingEnabled = true;
            this.cbICCC.Location = new System.Drawing.Point(196, 59);
            this.cbICCC.Name = "cbICCC";
            this.cbICCC.Size = new System.Drawing.Size(93, 21);
            this.cbICCC.TabIndex = 13;
            this.cbICCC.SelectedValueChanged += new System.EventHandler(this.cbICCC_SelectedValueChanged);
            // 
            // cbDCCC
            // 
            this.cbDCCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDCCC.FormattingEnabled = true;
            this.cbDCCC.Location = new System.Drawing.Point(196, 32);
            this.cbDCCC.Name = "cbDCCC";
            this.cbDCCC.Size = new System.Drawing.Size(93, 21);
            this.cbDCCC.TabIndex = 11;
            this.cbDCCC.SelectedValueChanged += new System.EventHandler(this.cbDCCC_SelectedValueChanged);
            // 
            // cbICSC
            // 
            this.cbICSC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbICSC.FormattingEnabled = true;
            this.cbICSC.Location = new System.Drawing.Point(97, 59);
            this.cbICSC.Name = "cbICSC";
            this.cbICSC.Size = new System.Drawing.Size(93, 21);
            this.cbICSC.TabIndex = 12;
            this.cbICSC.SelectedValueChanged += new System.EventHandler(this.cbICSC_SelectedValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(68, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 13);
            this.label12.TabIndex = 8;
            this.label12.Text = "OD";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(73, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(18, 13);
            this.label13.TabIndex = 9;
            this.label13.Text = "OI";
            // 
            // cbDCSC
            // 
            this.cbDCSC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDCSC.FormattingEnabled = true;
            this.cbDCSC.Location = new System.Drawing.Point(97, 32);
            this.cbDCSC.Name = "cbDCSC";
            this.cbDCSC.Size = new System.Drawing.Size(93, 21);
            this.cbDCSC.TabIndex = 10;
            this.cbDCSC.SelectedValueChanged += new System.EventHandler(this.cbDCSC_SelectedValueChanged);
            // 
            // ucOftalmologia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtDiag);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucOftalmologia";
            this.Size = new System.Drawing.Size(1080, 365);
            this.Load += new System.EventHandler(this.ucOftalmologia_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbILCC;
        private System.Windows.Forms.ComboBox cbDLCC;
        private System.Windows.Forms.ComboBox cbILSC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDLSC;
        private System.Windows.Forms.TextBox txtDiag;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbILAE;
        private System.Windows.Forms.ComboBox cbDLAE;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtEdad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbICCC;
        private System.Windows.Forms.ComboBox cbDCCC;
        private System.Windows.Forms.ComboBox cbICSC;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbDCSC;
        private System.Windows.Forms.CheckBox chkFiguras;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbICCC2;
        private System.Windows.Forms.ComboBox cbDCCC2;
    }
}
