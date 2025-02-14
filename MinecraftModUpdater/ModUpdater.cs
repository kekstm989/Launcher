using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MinecraftModUpdater
{
    class ModUpdater
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static string GetModsFolderPath()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(userPath, ".minecraft", "mods");
        }

        private static async Task<List<string>> GetModListFromRepoAsync()
        {
            string apiUrl = "https://api.github.com/repos/kekstm989/Launcher/contents/MinecraftModUpdater/Mods";
            List<string> modFiles = new List<string>();

            try
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MinecraftModUpdater");
                using (HttpResponseMessage response = await httpClient.GetAsync(apiUrl))
                {
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JsonDocument json = JsonDocument.Parse(jsonResponse);
                    
                    foreach (JsonElement file in json.RootElement.EnumerateArray())
                    {
                        string fileName = file.GetProperty("name").GetString();
                        if (fileName.EndsWith(".jar"))
                        {
                            modFiles.Add(fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении списка модов: {ex.Message}");
            }

            return modFiles;
        }

        public static async Task UpdateModsAsync()
        {
            string modsFolder = GetModsFolderPath();
            if (!Directory.Exists(modsFolder))
                Directory.CreateDirectory(modsFolder);

            string repoUrl = "https://github.com/kekstm989/Launcher/raw/main/MinecraftModUpdater/Mods/";
            List<string> modFiles = await GetModListFromRepoAsync();

            foreach (string mod in modFiles)
            {
                string localFile = Path.Combine(modsFolder, mod);
                string remoteFile = repoUrl + mod;

                if (!File.Exists(localFile))
                {
                    Console.WriteLine($"Скачивание {mod}...");
                    await Utils.DownloadFileAsync(remoteFile, localFile);
                }
            }
        }
    }
}
