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
                Application.Run(updateForm);
            }

            // Если обновление завершилось — запускаем основной лаунчер
            Application.Run(new MainForm());
        }
    }
}
