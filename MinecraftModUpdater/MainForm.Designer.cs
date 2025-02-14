using System.Drawing; // ✅ Добавляем для работы с цветами

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
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.BackColor = Color.Transparent;
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Minecraft Mod Updater";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // ✅ Добавляем поддержку перемещения окна без рамки
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);

            // 
            // listViewMods
            // 
            this.listViewMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnName,
                this.columnStatus,
                this.columnProgress});
            this.listViewMods.Location = new System.Drawing.Point(20, 50);
            this.listViewMods.Name = "listViewMods";
            this.listViewMods.Size = new System.Drawing.Size(500, 250);
            this.listViewMods.TabIndex = 1;
            this.listViewMods.View = System.Windows.Forms.View.Details;
            this.listViewMods.FullRowSelect = true;
            this.listViewMods.BackColor = Color.FromArgb(30, 30, 30);
            this.listViewMods.ForeColor = Color.White;

            // 
            // columnName
            // 
            this.columnName.Text = "Название";
            this.columnName.Width = 200;

            // 
            // columnStatus
            // 
            this.columnStatus.Text = "Статус";
            this.columnStatus.Width = 150;

            // 
            // columnProgress
            // 
            this.columnProgress.Text = "Прогресс";
            this.columnProgress.Width = 150;

            // 
            // btnUpdateMods
            // 
            this.btnUpdateMods.Text = "Обновить моды";
            this.btnUpdateMods.BackColor = Color.FromArgb(50, 50, 50);
            this.btnUpdateMods.ForeColor = Color.White;
            this.btnUpdateMods.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateMods.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdateMods.Location = new System.Drawing.Point(20, 320);
            this.btnUpdateMods.Size = new System.Drawing.Size(150, 30);
            this.btnUpdateMods.TabIndex = 2;
            this.btnUpdateMods.Click += new System.EventHandler(this.btnUpdateMods_Click);

            // ✅ Добавляем анимацию при наведении на кнопку
            this.btnUpdateMods.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnUpdateMods.MouseLeave += new System.EventHandler(this.Button_MouseLeave);

            // 
            // btnClose
            // 
            this.btnClose.Text = "Закрыть";
            this.btnClose.BackColor = Color.FromArgb(50, 50, 50);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(370, 320);
            this.btnClose.Size = new System.Drawing.Size(150, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // ✅ Добавляем анимацию при наведении на кнопку
            this.btnClose.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnClose.MouseLeave += new System.EventHandler(this.Button_MouseLeave);

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(180, 320);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(180, 30);
            this.progressBar.TabIndex = 4;
            this.progressBar.Visible = false;

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(550, 370);
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
