using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.Load += async (s, e) => await CheckForUpdateOnStartAsync();
        }

        private async Task CheckForUpdateOnStartAsync()
        {
            lblStatus.Text = "Проверка обновлений...";
            bool isUpdateAvailable = await Updater.CheckForUpdateAsync();

            if (isUpdateAvailable)
            {
                lblStatus.Text = "Обновление...";
                await Updater.UpdateLauncherAsync(progressBar);
            }
            else
            {
                lblStatus.Text = "У вас последняя версия.";
                MessageBox.Show("У вас уже последняя версия лаунчера.", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnUpdateMods_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Обновление модов...";
            await ModUpdater.UpdateModsAsync(listViewMods);
            lblStatus.Text = "Обновление завершено!";
            MessageBox.Show("Обновление завершено!", "Minecraft Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
