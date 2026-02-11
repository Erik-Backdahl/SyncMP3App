using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using SyncMP3App;

class ModifyAppSettings
{
    internal static async Task<string> TryAddRegisteredFolder(string selectedFolder)
    {
        var musicData = await GetAppSettings();

        if (musicData.RegisteredFolders.Any(rf => rf == selectedFolder))
        {
            return $"Folder already registered {selectedFolder}";
        }

        musicData.RegisteredFolders.Add(selectedFolder);
        if (string.IsNullOrEmpty(musicData.DownloadFolder))
        {
            musicData.DownloadFolder = selectedFolder;
        }

        var updatedJson = JsonSerializer.Serialize(musicData);

        File.WriteAllText(DatabaseConfig.AppSettingsJson, updatedJson);
        return $"Folder added {selectedFolder}";
    }
    internal static async Task<AppSettingsFormat> GetAppSettings()
    {
        string jsonContent = await File.ReadAllTextAsync(DatabaseConfig.AppSettingsJson);
        var musicData = JsonSerializer.Deserialize<AppSettingsFormat>(jsonContent);

        if (musicData == null)
        {
            throw new Exception("No appsettings.json file detected");
        }
        return musicData;
    }
    internal static async Task<string> GetUuid()
    {
        var appSettings = await GetAppSettings();
        if (string.IsNullOrEmpty(appSettings.UUID))
            throw new Exception("No UUID header");
            
        return appSettings.UUID;
    }
    internal static async Task<string> GetGuid()
    {
        var appSettings = await GetAppSettings();
        if (string.IsNullOrEmpty(appSettings.UUID))
            throw new Exception("No GUID header, A network is required for this action");
        return appSettings.GUID;
    }
}