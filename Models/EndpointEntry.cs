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
    internal async Task<CompareResonseFormat> Compare()
    {
        using var dbContext = _factory.CreateDbContext();
        await ModifyMusic.SaveAllMusicInSQL(dbContext);

        return JsonSerializer.Deserialize<CompareResonseFormat>(await SendHttps.CompareRequest(dbContext))
            ?? throw new Exception("Compare request failed");
    }
    internal async Task RequestMusic(CompareResonseFormat allMusicGuids)
    {
        using var dbContext = _factory.CreateDbContext();
        await SendHttps.SongRequest(allMusicGuids, dbContext);
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
            await ModifyAppSettings.RegisterNetwork(response.Headers["newGUID"]!);
            return "Joined network successfully";
        }
        catch (Exception ex)
        {
            return $"Failed to join network{ex.Message}";
        }
    }

}