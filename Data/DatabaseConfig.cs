using System;
using System.IO;
using System.Linq;

public static class DatabaseConfig
{
    private static string GetBaseDataDirectory()
    {
        var baseDir = AppContext.BaseDirectory;
        var dir = new DirectoryInfo(baseDir);

        for (int i = 0; i < 12 && dir != null; i++)
        {
            var csprojFile = dir.GetFiles("SyncMP3App.csproj").FirstOrDefault();
            if (csprojFile != null)
            {
                var projectData = Path.Combine(dir.FullName, "Data", "UserData");
                if (!Directory.Exists(projectData))
                    Directory.CreateDirectory(projectData);
                return projectData;
            }
            dir = dir.Parent;
        }

        // fallback: Data next to the running exe
        var exeSide = Path.Combine(AppContext.BaseDirectory, "Data", "UserData");
        if (!Directory.Exists(exeSide))
            Directory.CreateDirectory(exeSide);
        return exeSide;
    }
    public static string DbFilePath
    {
        get
        {
            var folder = Path.Combine(GetBaseDataDirectory());

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, "syncmp3app.db");
        }
    }
    public static string AppSettingsJson
    {
        get
        {
            var folder = Path.Combine(GetBaseDataDirectory());

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, "appsettings.json");
        }
    }
    public static string MessagesJson
    {
        get
        {
            var folder = Path.Combine(GetBaseDataDirectory());

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, "messages.json");
        }
    }

    public static string ConnectionString => $"Data Source={DbFilePath}";
}