namespace ElektroSim.HistoricData
{
    static class DownloadAndCache
    {
        private const string CacheFolderName = "download_cache";

        public static async Task<string> GetFileAsync(string url, bool commasToDots = false)
        {
            Directory.CreateDirectory(CacheFolderName);
            var fileName = Path.Combine(Path.Combine(CacheFolderName, Path.GetFileName(url)));

            if (!File.Exists(fileName))
            {
                var httpClient = new HttpClient();
                var httpStream = await httpClient.GetStreamAsync(url);
                using var outputFile = new FileStream(fileName, FileMode.Create);
                if (commasToDots)
                {
                    using var streamReader = new StreamReader(httpStream);
                    var text = await streamReader.ReadToEndAsync();
                    text = text.Replace(',', '.');
                    using var streamWriter = new StreamWriter(outputFile);
                    streamWriter.Write(text);
                }
                else
                {
                    await httpStream.CopyToAsync(outputFile);
                }
            }
            return fileName;
        }
    }
}