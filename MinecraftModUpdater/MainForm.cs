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
            listViewMods.Items.Clear();
            await ModUpdater.UpdateModsAsync(listViewMods);
            MessageBox.Show("Обновление завершено!", "Minecraft Mod Updater", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
