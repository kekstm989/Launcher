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
        }

        private async void btnUpdateMods_Click(object sender, EventArgs e)
        {
            await ModUpdater.UpdateModsAsync();
            MessageBox.Show("Обновление завершено!", "Minecraft Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnUpdateLauncher_Click(object sender, EventArgs e)
        {
            bool isUpdateAvailable = await Updater.CheckForUpdateAsync();
            if (isUpdateAvailable)
            {
                await Updater.UpdateLauncherAsync();
            }
            else
            {
                MessageBox.Show("У вас уже последняя версия.", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
