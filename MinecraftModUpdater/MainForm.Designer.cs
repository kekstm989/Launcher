using System.Drawing; // ✅ Для работы с цветами
using System.IO;

namespace MinecraftModUpdater
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ListView listViewMods;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnStatus;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.Button btnUpdateMods;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar progressBar;

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
            string iconPath = Path.Combine(Directory.GetCurrentDirectory(), "MinecraftModUpdater", "Resources", "logo.ico"); // ✅ Используем ICO
            
            if (File.Exists(iconPath))
            {
                this.Icon = new System.Drawing.Icon(iconPath); // ✅ Правильный путь к иконке
            }

            this.lblTitle = new System.Windows.Forms.Label();
            this.listViewMods = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnStatus = new System.Windows.Forms.ColumnHeader();
            this.columnProgress = new System.Windows.Forms.ColumnHeader();
            this.btnUpdateMods = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(650, 430);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.listViewMods);
            this.Controls.Add(this.btnUpdateMods);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.ResumeLayout(false);
        }
    }
}
