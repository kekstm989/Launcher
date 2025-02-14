using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    public partial class MainForm : Form
    {
        private Timer fadeInTimer;
        private Timer fadeOutTimer;

        public MainForm()
        {
            InitializeComponent();
            this.Opacity = 0;
            this.Icon = new Icon("MinecraftModUpdater/Resources/logo.png"); // ✅ Устанавливаем кастомную иконку
            InitFadeIn();
        }

        private async void btnUpdateMods_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Обновление модов...";
            progressBar.Visible = true;
            await AnimateProgressBar(100);
            await ModUpdater.UpdateModsAsync(listViewMods);
            progressBar.Visible = false;
            lblTitle.Text = "Minecraft Mod Updater";
            MessageBox.Show("Обновление завершено!", "Minecraft Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            StartFadeOut(); // ✅ При закрытии окна теперь плавное исчезновение
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

        // ✅ Анимация появления (Fade In)
        private void InitFadeIn()
        {
            fadeInTimer = new Timer();
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

        // ✅ Анимация закрытия (Fade Out)
        private void StartFadeOut()
        {
            fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10;
            fadeOutTimer.Tick += (s, e) =>
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.05;
                }
                else
                {
                    fadeOutTimer.Stop();
                    Application.Exit();
                }
            };
            fadeOutTimer.Start();
        }

        // ✅ Анимация заполнения ProgressBar
        private async Task AnimateProgressBar(int targetValue)
        {
            int step = 2;
            while (progressBar.Value < targetValue)
            {
                progressBar.Value += step;
                await Task.Delay(20);
            }
        }
    }
}
