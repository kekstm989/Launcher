namespace MinecraftModUpdater
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnUpdateMods;
        private System.Windows.Forms.Button btnUpdateLauncher;

        private void InitializeComponent()
        {
            this.btnUpdateMods = new System.Windows.Forms.Button();
            this.btnUpdateLauncher = new System.Windows.Forms.Button();
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
            // btnUpdateLauncher
            // 
            this.btnUpdateLauncher.Location = new System.Drawing.Point(50, 70);
            this.btnUpdateLauncher.Name = "btnUpdateLauncher";
            this.btnUpdateLauncher.Size = new System.Drawing.Size(150, 30);
            this.btnUpdateLauncher.TabIndex = 1;
            this.btnUpdateLauncher.Text = "Обновить лаунчер";
            this.btnUpdateLauncher.UseVisualStyleBackColor = true;
            this.btnUpdateLauncher.Click += new System.EventHandler(this.btnUpdateLauncher_Click);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(250, 120);
            this.Controls.Add(this.btnUpdateMods);
            this.Controls.Add(this.btnUpdateLauncher);
            this.Name = "MainForm";
            this.Text = "Minecraft Mod Updater";
            this.ResumeLayout(false);
        }
    }
}
