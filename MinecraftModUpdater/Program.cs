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

            // Запускаем `UpdateForm`
            using (UpdateForm updateForm = new UpdateForm())
            {
                if (updateForm.ShowDialog() == DialogResult.OK)
                {
                    // После `Fade Out` открываем `MainForm`
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
