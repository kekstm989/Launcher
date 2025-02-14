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

                    return remoteDate > localDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки обновлений: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static async Task<bool> UpdateLauncherAsync(ProgressBar progressBar)
        {
            try
            {
                string localFile = Application.ExecutablePath;
                string tempFile = localFile + ".new";
                string backupFile = localFile + ".bak";
                string scriptFile = Path.Combine(Path.GetTempPath(), "cleanup.bat");

                using (HttpResponseMessage response = await httpClient.GetAsync(DownloadUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    long totalBytes = response.Content.Headers.ContentLength ?? -1;
                    await using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (var contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            byte[] buffer = new byte[8192];
                            long totalRead = 0;
                            int bytesRead;
                            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fs.WriteAsync(buffer, 0, bytesRead);
                                totalRead += bytesRead;

                                if (totalBytes > 0)
                                {
                                    int progress = (int)((totalRead * 100) / totalBytes);
                                    progressBar.Invoke(new Action(() =>
                                    {
                                        progressBar.Value = progress;
                                    }));
                                }
                            }
                        }
                    }
                }

                // Переименовываем старый .exe в .bak и заменяем новым
                File.Move(localFile, backupFile, true);
                File.Move(tempFile, localFile, true);

                // Создаём батник для удаления .bak после перезапуска
                File.WriteAllText(scriptFile, $@"
@echo off
timeout /t 2 >nul
del ""{backupFile}""
del ""{scriptFile}""
exit
");

                // Запускаем cleanup.bat и перезапускаем лаунчер
                Process.Start(new ProcessStartInfo
                {
                    FileName = scriptFile,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                MessageBox.Show("Обновление завершено! Перезапуск лаунчера.", "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(localFile);
                Environment.Exit(0);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
