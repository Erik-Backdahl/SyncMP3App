

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HarfBuzzSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SyncMP3App.Data;
using TagLib.IFD;

class ModifyMusic
{
    public static async Task SaveAllMusicInSQL(SyncMp3AppContext dbContext)
    {
        var appSettings = await ModifyAppSettings.GetAppSettings();

        if (appSettings.RegisteredFolders.Count <= 0)
        {
            throw new Exception("No folders registered");
        }

        foreach (string folderPath in appSettings.RegisteredFolders)
        {
            string[] allFileNames = Directory.GetFiles(folderPath);

            foreach (string musicFile in allFileNames)
            {
                var musicFileAbsolutePath = Path.Combine(folderPath, musicFile);

                string tag = TryCreateTag(musicFileAbsolutePath);

                if (await dbContext.DeviceMusics.AnyAsync(dm => dm.SongGuid == tag))
                    return;

                DeviceMusic newMusicEntry = new DeviceMusic
                {
                    SongGuid = tag,
                    Name = musicFile,
                    AbsolutePath = musicFileAbsolutePath
                };
                await dbContext.DeviceMusics.AddAsync(newMusicEntry);
            }
        }


    }
    private static string TryCreateTag(string FilePath)
    {
        var file = TagLib.File.Create(FilePath);
        if (!string.IsNullOrEmpty(file.Tag.Comment))
            return file.Tag.Comment;

        file.Tag.Comment = Guid.NewGuid().ToString();

        return file.Tag.Comment;
    }
}