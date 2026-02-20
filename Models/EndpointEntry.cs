using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncMP3App.Data;

public class EndpointEntry
{
    private readonly IDbContextFactory<SyncMp3AppContext> _factory;

    public EndpointEntry(IDbContextFactory<SyncMp3AppContext> factory)
    {
        _factory = factory;
    }
    internal async Task<bool> Update()
    {
        using var dbContext = _factory.CreateDbContext();
        return await SendHttps.PingRequest();
    }
    internal async Task<CompareResonseFormatApp> Compare()
    {
        using var dbContext = _factory.CreateDbContext();
        await ModifyMusic.SaveAllMusicInSQL(dbContext);

        return await SendHttps.CompareRequest(dbContext);
    }
    internal async Task Create()
    {
        await SendHttps.CreateRequest();
    }
    internal async Task<string> Join(string password)
    {
        try
        {
            var response = await SendHttps.JoinRequest(password);
            await ModifyAppSettings.RegisterNetwork(response["newGUID"]!);
            return "Joined network successfully";
        }
        catch (Exception ex)
        {
            return $"Failed to join network{ex.Message}";
        }
    }
    internal async Task RequestMusic(List<string> downloadMusicGuids)
    {
        using var dbContext = _factory.CreateDbContext();
        await SendHttps.SongDownload(downloadMusicGuids, dbContext);
    }
    internal async Task<string> UploadMusic(List<string> uploadMusicGuids)
    {
        using var dbContext = _factory.CreateDbContext();
        await SendHttps.SongUpload(uploadMusicGuids, dbContext);

        return "";
    }

}