using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    class ModUpdater
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string GitHubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

        private const string RepoApiUrl = "https://api.github.com/repos/kekstm989/Launcher/contents/MinecraftModUpdater/Mods";
        private const string RepoRawUrl = "https://raw.githubusercontent.com/kekstm989/Launcher/main/MinecraftModUpdater/Mods/";

        static ModUpdater()
        {
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MinecraftModUpdater");

            if (!string.IsNullOrEmpty(GitHubToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GitHubToken);
            }
            else
            {
                MessageBox.Show("Ошибка: GitHub API Token отсутствует! Добавьте его в переменные среды.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async Task UpdateModsAsync(ListView listView)
        {
            await ShowAndUpdateModsAsync(listView);
        }

        public static async Task ShowAndUpdateModsAsync(ListView listView)
        {
            string modsFolder = GetModsFolderPath();
            string cacheFolder = GetCacheFolderPath();

            if (!Directory.Exists(modsFolder))
                Directory.CreateDirectory(modsFolder);

            if (!Directory.Exists(cacheFolder))
                Directory.CreateDirectory(cacheFolder);

            // 1️⃣ **Показываем локальные моды**
            Dictionary<string, DateTime> localMods = GetLocalMods(modsFolder);
            listView.Items.Clear();

            foreach (var mod in localMods)
            {
                ListViewItem item = new ListViewItem(mod.Key);
                item.SubItems.Add("Локальный");
                item.SubItems.Add("-");
                listView.Items.Add(item);
            }

            await Task.Delay(2000); // ⚡ Даем 2 секунды на отображение списка

            // 2️⃣ **Проверяем обновления с GitHub**
            Dictionary<string, DateTime> repoMods = await GetModListFromRepoAsync();

            foreach (var mod in repoMods)
            {
                string modName = mod.Key;
                DateTime remoteDate = mod.Value;
                string localFilePath = Path.Combine(modsFolder, modName);
                string cacheFilePath = Path.Combine(cacheFolder, modName + ".date");

                bool needsUpdate = !localMods.ContainsKey(modName) || localMods[modName] < remoteDate;

                ListViewItem item = new ListViewItem(modName);
                item.SubItems.Add(needsUpdate ? "Требует обновления" : "Актуальная версия");
                item.SubItems.Add(needsUpdate ? "0%" : "-");
                listView.Items.Add(item);

                if (needsUpdate)
                {
                    if (File.Exists(localFilePath))
                    {
                        try
                        {
                            File.Delete(localFilePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка удаления старой версии {modName}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    await DownloadModAsync(RepoRawUrl + modName, localFilePath, item);
                    File.WriteAllText(cacheFilePath, remoteDate.ToString("o"));
                }
            }
        }

        private static Dictionary<string, DateTime> GetLocalMods(string modsFolder)
        {
            Dictionary<string, DateTime> localMods = new Dictionary<string, DateTime>();

            foreach (string file in Directory.GetFiles(modsFolder, "*.jar"))
            {
                localMods[Path.GetFileName(file)] = File.GetLastWriteTimeUtc(file);
            }

            return localMods;
        }

        private static async Task<Dictionary<string, DateTime>> GetModListFromRepoAsync()
        {
            Dictionary<string, DateTime> modFiles = new Dictionary<string, DateTime>();

            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(RepoApiUrl))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Ошибка 401: Неверный GitHub API Token!", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return modFiles;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        MessageBox.Show("Ошибка 403: Превышен лимит запросов к GitHub API!", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return modFiles;
                    }

                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(jsonResponse);

                    foreach (JsonElement file in json.RootElement.EnumerateArray())
                    {
                        if (!file.TryGetProperty("name", out JsonElement fileNameElement) ||
                            !file.TryGetProperty("sha", out JsonElement fileShaElement))
                        {
                            continue;
                        }

                        string fileName = fileNameElement.GetString();
                        string fileSha = fileShaElement.GetString();
                        DateTime lastCommitDate = DateTime.UtcNow; // ❗ Получать точную дату коммита можно через другой API

                        if (fileName.EndsWith(".jar"))
                        {
                            modFiles[fileName] = lastCommitDate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении списка модов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return modFiles;
        }

        private static async Task DownloadModAsync(string url, string savePath, ListViewItem listItem)
        {
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    long totalBytes = response.Content.Headers.ContentLength ?? -1;

                    await using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
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
                                    listItem.SubItems[2].Text = $"{progress}%";
                                }
                            }
                        }
                    }
                }

                listItem.SubItems[1].Text = "Готово";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки {url}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetModsFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
        private static string GetCacheFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods_cache");
    }
}
