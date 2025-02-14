using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Opacity = 0;
            InitFadeIn();
        }

        private async void btnUpdateMods_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Обновление модов...";
            progressBar.Visible = true;
            await ModUpdater.UpdateModsAsync(listViewMods);
            progressBar.Visible = false;
            lblTitle.Text = "Minecraft Mod Updater";
            MessageBox.Show("Обновление завершено!", "Minecraft Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.Black,
                Color.FromArgb(40, 40, 40),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void InitFadeIn()
        {
            Timer fadeInTimer = new Timer();
            fadeInTimer.Interval = 10;
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.05;
                }
                else
                {
                    fadeInTimer.Stop();
                }
            };
            fadeInTimer.Start();
        }
    }
}
