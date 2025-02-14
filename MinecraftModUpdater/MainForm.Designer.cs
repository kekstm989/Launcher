namespace MinecraftModUpdater
{
    partial class MainForm
    {
        private System.Windows.Forms.Button btnUpdateMods;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;

        private void InitializeComponent()
        {
            this.btnUpdateMods = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // 
            // btnUpdateMods
            // 
            this.btnUpdateMods.Location = new System.Drawing.Point(50, 30);
            this.btnUpdateMods.Name = "btnUpdateMods";
            this.btnUpdateMods.Size = new System.Drawing.Size(150, 30);
            this.btnUpdateMods.TabIndex = 0;
            this.btnUpdateMods.Text = "Обновить моды";
            this.btnUpdateMods.UseVisualStyleBackColor = true;
            this.btnUpdateMods.Click += new System.EventHandler(this.btnUpdateMods_Click);

            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(50, 70);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(150, 20);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Готово";

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(50, 100);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(150, 20);
            this.progressBar.TabIndex = 2;

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(250, 140);
            this.Controls.Add(this.btnUpdateMods);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.Name = "MainForm";
            this.Text = "Minecraft Mod Updater";
            this.ResumeLayout(false);
        }
    }
}
