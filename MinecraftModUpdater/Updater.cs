using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    class Updater
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string RepoApiUrl = "https://api.github.com/repos/kekstm989/Launcher/commits?path=MinecraftModUpdater/bin/Launcher/MinecraftModUpdater.exe";
        private const string DownloadUrl = "https://github.com/kekstm989/Launcher/raw/main/MinecraftModUpdater/bin/Launcher/MinecraftModUpdater.exe";

        public static async Task<bool> CheckForUpdateAsync()
        {
            try
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MinecraftModUpdater");

                using (HttpResponseMessage response = await httpClient.GetAsync(RepoApiUrl))
                {
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(jsonResponse);

                    string remoteCommitDate = json.RootElement[0].GetProperty("commit").GetProperty("committer").GetProperty("date").GetString();
                    DateTime remoteDate = DateTime.Parse(remoteCommitDate).ToUniversalTime();

                    string localFile = Application.ExecutablePath;
                    DateTime localDate = File.GetLastWriteTimeUtc(localFile);

                    return remoteDate > localDate; // Если удалённая дата новее — обновляем
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки обновлений: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static async Task UpdateLauncherAsync()
        {
            try
            {
                string localFile = Application.ExecutablePath;
                string tempFile = localFile + ".new";

                using (HttpResponseMessage response = await httpClient.GetAsync(DownloadUrl))
                {
                    response.EnsureSuccessStatusCode();
                    await using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                string backupFile = localFile + ".bak";
                if (File.Exists(backupFile)) File.Delete(backupFile);
                File.Move(localFile, backupFile); // Резервная копия старого exe
                File.Move(tempFile, localFile); // Обновляем exe

                MessageBox.Show("Обновление завершено! Перезапустите приложение.", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(localFile);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
