using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftModUpdater
{
    class ModUpdater
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private const string RepoApiUrl = "https://api.github.com/repos/kekstm989/Launcher/contents/MinecraftModUpdater/Mods";
        private const string RepoRawUrl = "https://github.com/kekstm989/Launcher/raw/main/MinecraftModUpdater/Mods/";

        private static string GetModsFolderPath()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(userPath, ".minecraft", "mods");
        }

        private static async Task<Dictionary<string, DateTime>> GetModListFromRepoAsync()
        {
            Dictionary<string, DateTime> modFiles = new Dictionary<string, DateTime>();

            try
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MinecraftModUpdater");
                using (HttpResponseMessage response = await httpClient.GetAsync(RepoApiUrl))
                {
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(jsonResponse);

                    foreach (JsonElement file in json.RootElement.EnumerateArray())
                    {
                        string fileName = file.GetProperty("name").GetString();
                        string lastCommitDate = file.GetProperty("sha").GetString(); // Коммит ID (не точная дата, но можно использовать)
                        DateTime commitDate = DateTime.UtcNow; // Место для даты коммита (опционально)

                        if (fileName.EndsWith(".jar"))
                        {
                            modFiles[fileName] = commitDate;
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
            if (!Directory.Exists(modsFolder))
                Directory.CreateDirectory(modsFolder);

            Dictionary<string, DateTime> repoMods = await GetModListFromRepoAsync();
            string[] localMods = Directory.GetFiles(modsFolder, "*.jar");

            foreach (var mod in repoMods)
            {
                string modName = mod.Key;
                DateTime remoteDate = mod.Value;
                string localFilePath = Path.Combine(modsFolder, modName);

                bool needsUpdate = !File.Exists(localFilePath) || File.GetLastWriteTimeUtc(localFilePath) < remoteDate;

                ListViewItem item = new ListViewItem(modName);
                item.SubItems.Add(needsUpdate ? "Требует обновления" : "Актуальная версия");
                item.SubItems.Add(needsUpdate ? "0%" : "-");
                listView.Items.Add(item);

                if (needsUpdate)
                {
                    await DownloadModAsync(RepoRawUrl + modName, localFilePath, item);
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
    }
}
