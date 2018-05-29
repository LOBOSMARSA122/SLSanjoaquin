namespace Sigesoft.Node.WinClient.UI
{
    partial class frmDescargarAdjuntos
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
            this.btnDescargarAdjuntos = new System.Windows.Forms.Button();
            this.txtArchivosAdjuntos = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDescargarAdjuntos
            // 
            this.btnDescargarAdjuntos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDescargarAdjuntos.BackColor = System.Drawing.SystemColors.Control;
            this.btnDescargarAdjuntos.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDescargarAdjuntos.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnDescargarAdjuntos.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnDescargarAdjuntos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescargarAdjuntos.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDescargarAdjuntos.ForeColor = System.Drawing.Color.Black;
            this.btnDescargarAdjuntos.Image = global::Sigesoft.Node.WinClient.UI.Resources.chart_organisation;
            this.btnDescargarAdjuntos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDescargarAdjuntos.Location = new System.Drawing.Point(289, 115);
            this.btnDescargarAdjuntos.Margin = new System.Windows.Forms.Padding(2);
            this.btnDescargarAdjuntos.Name = "btnDescargarAdjuntos";
            this.btnDescargarAdjuntos.Size = new System.Drawing.Size(85, 36);
            this.btnDescargarAdjuntos.TabIndex = 51;
            this.btnDescargarAdjuntos.Text = "Descargar Adjuntos";
            this.btnDescargarAdjuntos.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnDescargarAdjuntos.UseVisualStyleBackColor = false;
            this.btnDescargarAdjuntos.Click += new System.EventHandler(this.btnDescargarAdjuntos_Click);
            // 
            // txtArchivosAdjuntos
            // 
            this.txtArchivosAdjuntos.Location = new System.Drawing.Point(12, 12);
            this.txtArchivosAdjuntos.Multiline = true;
            this.txtArchivosAdjuntos.Name = "txtArchivosAdjuntos";
            this.txtArchivosAdjuntos.Size = new System.Drawing.Size(362, 98);
            this.txtArchivosAdjuntos.TabIndex = 52;
            // 
            // frmDescargarAdjuntos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 165);
            this.Controls.Add(this.txtArchivosAdjuntos);
            this.Controls.Add(this.btnDescargarAdjuntos);
            this.Name = "frmDescargarAdjuntos";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Descargar Adjuntos";
            this.Load += new System.EventHandler(this.frmDescargarAdjuntos_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDescargarAdjuntos;
        private System.Windows.Forms.TextBox txtArchivosAdjuntos;
    }
}