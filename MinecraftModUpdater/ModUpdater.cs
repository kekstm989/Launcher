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
                        DateTime lastCommitDate = DateTime.UtcNow; // ❗ Можно расширить, чтобы получить точную дату коммита

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

        public static async Task UpdateModsAsync(ListView listView)
        {
            string modsFolder = GetModsFolderPath();
            string cacheFolder = GetCacheFolderPath();

            if (!Directory.Exists(modsFolder))
                Directory.CreateDirectory(modsFolder);

            if (!Directory.Exists(cacheFolder))
                Directory.CreateDirectory(cacheFolder);

            Dictionary<string, DateTime> repoMods = await GetModListFromRepoAsync();

            listView.Items.Clear();

            foreach (var mod in repoMods)
            {
                string modName = mod.Key;
                DateTime remoteDate = mod.Value;
                string localFilePath = Path.Combine(modsFolder, modName);
                string cacheFilePath = Path.Combine(cacheFolder, modName + ".date");

                bool needsUpdate = true;

                if (File.Exists(localFilePath) && File.Exists(cacheFilePath))
                {
                    DateTime? cachedDate = GetCachedDate(cacheFilePath);

                    // ✅ Проверяем, изменилась ли дата обновления мода на сервере
                    if (cachedDate.HasValue && cachedDate.Value >= remoteDate)
                    {
                        needsUpdate = false;
                    }
                }

                ListViewItem item = new ListViewItem(modName);
                item.SubItems.Add(needsUpdate ? "Требует обновления" : "Актуальная версия");
                item.SubItems.Add(needsUpdate ? "0%" : "-");
                listView.Items.Add(item);

                if (needsUpdate)
                {
                    // ✅ Удаляем старую версию перед обновлением
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
                    File.WriteAllText(cacheFilePath, remoteDate.ToString("o")); // ✅ Записываем дату обновления
                }
            }
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

        private static DateTime? GetCachedDate(string filePath)
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath).Trim();
                if (DateTime.TryParse(content, out DateTime cachedDate))
                {
                    return cachedDate;
                }
            }
            return null;
        }

        private static string GetModsFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
        private static string GetCacheFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods_cache");
    }
}
