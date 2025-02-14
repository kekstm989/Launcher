namespace MinecraftModUpdater
{
    partial class MainForm
    {
        private System.Windows.Forms.Button btnUpdateMods;

        private void InitializeComponent()
        {
            this.btnUpdateMods = new System.Windows.Forms.Button();
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
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(250, 100);
            this.Controls.Add(this.btnUpdateMods);
            this.Name = "MainForm";
            this.Text = "Minecraft Mod Updater";
            this.ResumeLayout(false);
        }
    }
}
