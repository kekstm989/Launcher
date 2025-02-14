using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        private async void UpdateForm_Load(object sender, EventArgs e)
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
                this.DialogResult = DialogResult.OK; // Закрываем окно обновления с флагом "ОК"
                this.Close();
            }
        }
    }
}
