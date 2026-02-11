using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SyncMP3App;
using SyncMP3App.Data;

public static class StartUpAction
{
    internal static void CheckEssentialFiles()
    {
        TryCreateEmptyDatabase();
        TryCreateEmptyMessagesJson();
        TryCreateEmptyAppSettingsJson();
    }
    private static void TryCreateEmptyDatabase()
    {
        var options = new DbContextOptionsBuilder<SyncMp3AppContext>()
            .UseSqlite(DatabaseConfig.ConnectionString)
            .Options;

        using var db = new SyncMp3AppContext(options);

        db.Database.Migrate();
    }

    private static void TryCreateEmptyAppSettingsJson()
    {
        if (File.Exists(DatabaseConfig.AppSettingsJson))
            return;

        var jsonFormat = new
        {
            GUID = "",
            UUID = Guid.NewGuid().ToString(),
            DownloadFolder = "",
            RegisteredFolders = new List<string> { }
        };
        string jsonString = JsonSerializer.Serialize(jsonFormat, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DatabaseConfig.AppSettingsJson, jsonString);
    }
    private static void TryCreateEmptyMessagesJson()
    {
        if (File.Exists(DatabaseConfig.MessagesJson))
            return;

        var jsonFormat = new
        {
            Messages = new List<string> { }
        };
        string jsonString = JsonSerializer.Serialize(jsonFormat, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(DatabaseConfig.MessagesJson, jsonString);
    }

}