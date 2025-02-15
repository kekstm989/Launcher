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
        }

        public static async Task UpdateModsAsync(ListView listView)
        {
            await ShowAndUpdateModsAsync(listView);
        }

        public static async Task ShowAndUpdateModsAsync(ListView listView)
        {
            string modsFolder = GetModsFolderPath();
            string cacheFolder = GetCacheFolderPath();

            Directory.CreateDirectory(modsFolder);
            Directory.CreateDirectory(cacheFolder);

            Dictionary<string, DateTime> localMods = GetLocalMods(modsFolder);
            listView.Items.Clear();

            foreach (var mod in localMods)
            {
                ListViewItem item = new ListViewItem(mod.Key);
                item.SubItems.Add("Локальный");
                item.SubItems.Add("-");
                listView.Items.Add(item);
            }

            await Task.Delay(2000);

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
                    string backupPath = localFilePath + ".bak";

                    if (File.Exists(localFilePath))
                    {
                        if (IsFileLocked(localFilePath))
                        {
                            MessageBox.Show($"Файл {localFilePath} используется другим процессом. Закройте Minecraft и попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }

                        File.Move(localFilePath, backupPath, true);
                    }

                    await DownloadModAsync(RepoRawUrl + modName, localFilePath, item);
                    File.WriteAllText(cacheFilePath, remoteDate.ToString("o"));

                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }
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
                        DateTime lastCommitDate = await GetCommitDateAsync(fileName, fileSha);

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

        private static async Task<DateTime> GetCommitDateAsync(string fileName, string fileSha)
        {
            string commitUrl = $"https://api.github.com/repos/kekstm989/Launcher/commits/{fileSha}";

            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(commitUrl))
                {
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(jsonResponse);

                    string commitDate = json.RootElement.GetProperty("commit")
                                                        .GetProperty("committer")
                                                        .GetProperty("date").GetString();

                    return DateTime.Parse(commitDate).ToUniversalTime();
                }
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        private static async Task DownloadModAsync(string url, string savePath, ListViewItem listItem)
        {
            try
            {
                if (IsFileLocked(savePath))
                {
                    MessageBox.Show($"Файл {savePath} используется другим процессом. Закройте Minecraft и попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    long totalBytes = response.Content.Headers.ContentLength ?? -1;

                    if (totalBytes <= 0)
                    {
                        MessageBox.Show($"Ошибка: Не удалось получить размер файла {url}", "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string tempFilePath = savePath + ".tmp";
                    if (File.Exists(tempFilePath)) File.Delete(tempFilePath);

                    await using (FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
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

                    if (File.Exists(savePath))
                    {
                        try
                        {
                            File.Delete(savePath);
                        }
                        catch (IOException)
                        {
                            MessageBox.Show($"Файл {savePath} не удалось удалить. Возможно, он используется. Попробуйте перезапустить компьютер.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    File.Move(tempFilePath, savePath);
                    listItem.SubItems[1].Text = "Готово";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки {url}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsFileLocked(string filePath)
        {
            if (!File.Exists(filePath)) return false;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        private static string GetModsFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
        private static string GetCacheFolderPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods_cache");
    }
}
