using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MinecraftModUpdater
{
    class Utils
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task DownloadFileAsync(string url, string savePath)
        {
            try
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    await using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {url}: {ex.Message}");
            }
        }
    }
}
