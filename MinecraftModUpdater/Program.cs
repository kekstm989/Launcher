using System;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Сначала запускаем окно обновления лаунчера
            using (UpdateForm updateForm = new UpdateForm())
            {
                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    // После успешного обновления запускаем MainForm
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
