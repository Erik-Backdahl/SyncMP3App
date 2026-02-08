using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

class ModifyAppSettings
{
    internal async static Task TryAddRegisteredFolder(string selectedFolder)
    {
        var musicData = await GetAppSettings();

        if (musicData.RegisteredFolders.Any(rf => rf == selectedFolder))
        {
            throw new Exception("Folder already added");
        }

        musicData.RegisteredFolders.Add(selectedFolder);
        if(string.IsNullOrEmpty(musicData.DownloadFolder))
        {
            musicData.DownloadFolder = selectedFolder;
        }

        var updatedJson = JsonSerializer.Serialize(musicData);

        File.WriteAllText(DatabaseConfig.AppSettingsJson, updatedJson);
    }
    internal async static Task<AppSettingsFormat> GetAppSettings()
    {
        string jsonContent = await File.ReadAllTextAsync(DatabaseConfig.AppSettingsJson);
        var musicData = JsonSerializer.Deserialize<AppSettingsFormat>(jsonContent);

        if (musicData == null)
        {
            throw new Exception("No appsettings.json file detected");
        }
        return musicData;
    }
}