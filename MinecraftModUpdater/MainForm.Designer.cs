namespace MinecraftModUpdater
{
    partial class MainForm
    {
        private System.Windows.Forms.Button btnUpdateMods;
        private System.Windows.Forms.ListView listViewMods;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnStatus;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;

        private void InitializeComponent()
        {
            this.btnUpdateMods = new System.Windows.Forms.Button();
            this.listViewMods = new System.Windows.Forms.ListView();
            this.columnName = new System.Windows.Forms.ColumnHeader();
            this.columnStatus = new System.Windows.Forms.ColumnHeader();
            this.columnProgress = new System.Windows.Forms.ColumnHeader();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // 
            // btnUpdateMods
            // 
            this.btnUpdateMods.Location = new System.Drawing.Point(50, 250);
            this.btnUpdateMods.Name = "btnUpdateMods";
            this.btnUpdateMods.Size = new System.Drawing.Size(200, 30);
            this.btnUpdateMods.TabIndex = 0;
            this.btnUpdateMods.Text = "Обновить моды";
            this.btnUpdateMods.UseVisualStyleBackColor = true;
            this.btnUpdateMods.Click += new System.EventHandler(this.btnUpdateMods_Click);

            // 
            // listViewMods
            // 
            this.listViewMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnStatus,
            this.columnProgress});
            this.listViewMods.Location = new System.Drawing.Point(20, 20);
            this.listViewMods.Name = "listViewMods";
            this.listViewMods.Size = new System.Drawing.Size(300, 180);
            this.listViewMods.TabIndex = 1;
            this.listViewMods.View = System.Windows.Forms.View.Details;
            this.listViewMods.FullRowSelect = true;

            // 
            // columnName
            // 
            this.columnName.Text = "Название";
            this.columnName.Width = 120;

            // 
            // columnStatus
            // 
            this.columnStatus.Text = "Статус";
            this.columnStatus.Width = 100;

            // 
            // columnProgress
            // 
            this.columnProgress.Text = "Прогресс";
            this.columnProgress.Width = 80;

            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(50, 210);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(200, 20);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Готово";

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(50, 230);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(200, 10);
            this.progressBar.TabIndex = 3;

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(340, 300);
            this.Controls.Add(this.listViewMods);
            this.Controls.Add(this.btnUpdateMods);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Name = "MainForm";
            this.Text = "Minecraft Mod Updater";
            this.ResumeLayout(false);
        }
    }
}
