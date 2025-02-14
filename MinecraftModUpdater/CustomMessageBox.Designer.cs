using System;
using System.Drawing; // ✅ Добавлено
using System.Windows.Forms; // ✅ Добавлено

namespace MinecraftModUpdater
{
    partial class CustomMessageBox
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnOK;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.BackColor = Color.Transparent;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Size = new System.Drawing.Size(260, 25);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblMessage
            this.lblMessage.Font = new System.Drawing.Font("Arial", 10F);
            this.lblMessage.ForeColor = Color.White;
            this.lblMessage.BackColor = Color.Transparent;
            this.lblMessage.Location = new System.Drawing.Point(12, 40);
            this.lblMessage.Size = new System.Drawing.Size(260, 50);
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnOK
            this.btnOK.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.BackColor = Color.FromArgb(50, 50, 50);
            this.btnOK.FlatStyle = FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(90, 100);
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            // CustomMessageBox
            this.ClientSize = new System.Drawing.Size(284, 150);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.Paint += new PaintEventHandler(this.CustomMessageBox_Paint);
            this.ResumeLayout(false);
        }
    }
}
