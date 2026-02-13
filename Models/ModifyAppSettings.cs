using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia;
using SyncMP3App;

class ModifyAppSettings
{
    internal static string? appUuid = GetUuid().ToString();
    internal static string? appGuid = GetGuid().ToString();
    internal static async Task RegisterNetwork(string networkGuid)
    {
        try
        {
            var appData = await GetAppSettings();

            if(!string.IsNullOrEmpty(appData.GUID))
                throw new Exception("Already in a Network. leave network first if you want to change");
            
            appData.GUID = networkGuid;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    internal static async Task<string> TryAddRegisteredFolder(string selectedFolder)
    {
        var appData = await GetAppSettings();

        if (appData.RegisteredFolders.Any(rf => rf == selectedFolder))
        {
            return $"Folder already registered {selectedFolder}";
        }

        appData.RegisteredFolders.Add(selectedFolder);
        if (string.IsNullOrEmpty(appData.DownloadFolder))
        {
            appData.DownloadFolder = selectedFolder;
        }

        var updatedJson = JsonSerializer.Serialize(appData);

        File.WriteAllText(DatabaseConfig.AppSettingsJson, updatedJson);
        return $"Folder added {selectedFolder}";
    }
    internal static async Task<AppSettingsFormat> GetAppSettings()
    {
        string jsonContent = await File.ReadAllTextAsync(DatabaseConfig.AppSettingsJson);
        var appData = JsonSerializer.Deserialize<AppSettingsFormat>(jsonContent);

        if (appData == null)
        {
            throw new Exception("No appsettings.json file detected");
        }
        return appData;
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