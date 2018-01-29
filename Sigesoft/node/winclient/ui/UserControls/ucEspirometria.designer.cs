namespace Sigesoft.Node.WinClient.UI.UserControls
{
    partial class ucEspirometria
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtcvf = new System.Windows.Forms.TextBox();
            this.txtvef1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtdiv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtrp = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Capacidad Vital Forzada [CVF]";
            // 
            // txtcvf
            // 
            this.txtcvf.Location = new System.Drawing.Point(163, 13);
            this.txtcvf.Name = "txtcvf";
            this.txtcvf.Size = new System.Drawing.Size(100, 20);
            this.txtcvf.TabIndex = 5;
            this.txtcvf.TextChanged += new System.EventHandler(this.txtcvf_TextChanged);
            // 
            // txtvef1
            // 
            this.txtvef1.Location = new System.Drawing.Point(466, 13);
            this.txtvef1.Name = "txtvef1";
            this.txtvef1.Size = new System.Drawing.Size(100, 20);
            this.txtvef1.TabIndex = 7;
            this.txtvef1.TextChanged += new System.EventHandler(this.txtvef1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Volumen Espiratorio Forzado [VEF1]";
            // 
            // txtdiv
            // 
            this.txtdiv.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtdiv.Location = new System.Drawing.Point(659, 13);
            this.txtdiv.Name = "txtdiv";
            this.txtdiv.ReadOnly = true;
            this.txtdiv.Size = new System.Drawing.Size(100, 20);
            this.txtdiv.TabIndex = 9;
            this.txtdiv.TextChanged += new System.EventHandler(this.txtdiv_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(577, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "[CVF] / [VEF1]";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtdiv);
            this.groupBox1.Controls.Add(this.txtcvf);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtvef1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(772, 41);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Espirometria";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtrp
            // 
            this.txtrp.BackColor = System.Drawing.Color.Thistle;
            this.txtrp.Location = new System.Drawing.Point(166, 50);
            this.txtrp.Name = "txtrp";
            this.txtrp.ReadOnly = true;
            this.txtrp.Size = new System.Drawing.Size(403, 20);
            this.txtrp.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "CONCLUSIÓN ";
            // 
            // ucEspirometria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtrp);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucEspirometria";
            this.Size = new System.Drawing.Size(786, 81);
            this.Load += new System.EventHandler(this.ucEspirometria_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtcvf;
        private System.Windows.Forms.TextBox txtvef1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtdiv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtrp;
        private System.Windows.Forms.Label label4;
    }
}
