using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    public partial class UpdateForm : Form
    {
        private Timer fadeInTimer;
        private Timer fadeOutTimer;

        public UpdateForm()
        {
            InitializeComponent();
            this.Opacity = 0;
            progressBar.Visible = false; // ✅ Скрываем ProgressBar при старте
            InitFadeIn();
        }

        private async void UpdateForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Проверка обновлений...";
            lblStatus.ForeColor = Color.White;
            lblStatus.BackColor = Color.Transparent;

            bool isUpdateAvailable = await Updater.CheckForUpdateAsync();

            if (isUpdateAvailable)
            {
                lblStatus.Text = "Обновление...";
                progressBar.Visible = true;  // ✅ Показываем ProgressBar только при обновлении
                progressBar.Value = 0;
                await AnimateProgress();
                await Updater.UpdateLauncherAsync(progressBar);
            }
            else
            {
                lblStatus.Text = "У вас последняя версия.";
                StartFadeOut();
            }
        }

        private async Task AnimateProgress()
        {
            for (int i = 0; i <= 100; i += 2)
            {
                progressBar.Value = i;
                await Task.Delay(20);
            }
        }

        // Метод для применения тени к окну
        private void ApplyShadow(object sender, EventArgs e)
        {
            int val = 2;
            DwmSetWindowAttribute(this.Handle, 2, ref val, 4);

            Margins margins = new Margins() { left = 1, right = 1, top = 1, bottom = 1 };
            DwmExtendFrameIntoClientArea(this.Handle, ref margins);
        }

        // Импортируем DWM API для добавления тени
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);

        private struct Margins
        {
            public int left, right, top, bottom;
        }

        // Градиентный ЧЁРНЫЙ фон
        private void UpdateForm_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.Black,  // Чисто ЧЁРНЫЙ
                Color.FromArgb(40, 40, 40),  // Тёмно-серый для эффекта объёма
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        // Добавляем плавное появление (Fade In)
        private void InitFadeIn()
        {
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 10;
            fadeInTimer.Tick += FadeInEffect;
            fadeInTimer.Start();
        }

        private void FadeInEffect(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05;
            }
            else
            {
                fadeInTimer.Stop();
            }
        }

        // 🚀 Добавляем плавное закрытие (Fade Out)
        private void StartFadeOut()
        {
            fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 10;
            fadeOutTimer.Tick += FadeOutEffect;
            fadeOutTimer.Start();
        }

        private void FadeOutEffect(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                fadeOutTimer.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
