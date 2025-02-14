namespace MinecraftModUpdater
{
    partial class UpdateForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblStatus;
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();

            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(20, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(250, 20);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Проверка обновлений...";

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 50);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(250, 20);
            this.progressBar.TabIndex = 1;

            // 
            // UpdateForm
            // 
            this.ClientSize = new System.Drawing.Size(300, 100);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обновление лаунчера";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);
        }
    }
}
