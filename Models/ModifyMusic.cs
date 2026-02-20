

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml.Styling;
using HarfBuzzSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SyncMP3App;
using SyncMP3App.Data;
using TagLib.IFD;

class ModifyMusic
{
    internal static async Task TrySaveSong(Dictionary<string, string> headers, HttpResponseMessage response)
    {
        using var memoryStream = new MemoryStream();
        await response.Content.CopyToAsync(memoryStream);
        byte[] songBytes = memoryStream.ToArray();

        var appSettings = DatabaseConfig.AppSettingsJson;

        Console.Write(appSettings);

    }
    public static async Task SaveAllMusicInSQL(SyncMp3AppContext dbContext)
    {
        var appSettings = await ModifyAppSettings.GetAppSettings();

        if (appSettings.RegisteredFolders.Count <= 0)
        {
            throw new Exception("No folders registered");
        }
        await dbContext.Database.BeginTransactionAsync();

        foreach (string folderPath in appSettings.RegisteredFolders)
        {
            string[] allFileNames = Directory.GetFiles(folderPath);

            foreach (string musicFile in allFileNames)
            {
                string tag = TryCreateTag(musicFile);

                if (await dbContext.DeviceMusics.AnyAsync(dm => dm.SongGuid == tag))
                    continue;

                DeviceMusic newMusicEntry = new DeviceMusic
                {
                    SongGuid = tag,
                    Name = Path.GetFileName(musicFile),
                    AbsolutePath = musicFile
                };
                await dbContext.DeviceMusics.AddAsync(newMusicEntry);
            }
        }

        await dbContext.SaveChangesAsync();
        await dbContext.Database.CommitTransactionAsync();
    }
    private static string TryCreateTag(string FilePath)
    {
        var file = TagLib.File.Create(FilePath);
        if (!string.IsNullOrEmpty(file.Tag.Comment))
            return file.Tag.Comment.Replace("UniqueID:", "");

        file.Tag.Comment = Guid.NewGuid().ToString();
        file.Save();

        return file.Tag.Comment.Replace("UniqueID:", "");
    }
}