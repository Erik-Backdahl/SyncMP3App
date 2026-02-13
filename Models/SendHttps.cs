using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SyncMP3App.Data;
using Tmds.DBus.Protocol;

class SendHttps
{
    internal static async Task<bool> PingRequest()
    {
        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("GET", "/ping");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());
        request.Headers.Add("GUID", await ModifyAppSettings.GetGuid());

        var response = await client.SendAsync(request);

        var parsedResponse = await ParseHTTP.GetResponseHeadersAndMessage(response);
        if (response.IsSuccessStatusCode)
        {
            //TODO save messages here
            return true;
        }
        else
        {
            return false;
        }
    }
    internal static async Task<CompareResonseFormatApp> CompareRequest(SyncMp3AppContext dbContext)
    {
        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("PUT", "/compare");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());
        request.Headers.Add("GUID", await ModifyAppSettings.GetGuid());
        var deviceMusicList = dbContext.DeviceMusics.Select(m => new DeviceMusicDto()
        {
            SongGuid = m.SongGuid,
            Name = m.Name,
            OriginalUpload = m.OriginalUpload
        });
        var jsonContent = JsonSerializer.Serialize(deviceMusicList);

        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        if (string.IsNullOrEmpty(jsonContent))
            throw new Exception("No music detected, no reason to send to server");

        var response = await client.SendAsync(request);
        var (headers, body)  = await ParseHTTP.GetResponseHeadersAndMessage(response);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<CompareResonseFormatApp>(body);

            if (result == null)
                throw new Exception("Failed to deserialize response");

            return result;
        }
        else
        {
            throw new Exception($"{response.StatusCode}" + "message");
        }

    }
    internal static async Task<Dictionary<string,string>> JoinRequest(string password)
    {
        if (password.Length != 6)
            throw new Exception("invalid password. must be 6 characters");
        if (!string.IsNullOrEmpty(await ModifyAppSettings.GetGuid()))
            throw new Exception("Already in a network");


        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("PATCH", "/join-network");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());
        request.Headers.Add("GUID", await ModifyAppSettings.GetGuid());
        request.Headers.Add("NetworkPassword", password);

        var response = await client.SendAsync(request);
        var (headers, body) = await ParseHTTP.GetResponseHeadersAndMessage(response);
        if (response.IsSuccessStatusCode)
        {
            return headers;
        }
        else
        {
            throw new Exception($"Failed to join network: {body}");
        }
    }
    internal static async Task<bool> CreateRequest()
    {
        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("POST", "/create-network");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());
        request.Headers.Add("GUID", await ModifyAppSettings.GetGuid());
        var response = await client.SendAsync(request);
        var parsedResponse = await ParseHTTP.GetResponseHeadersAndMessage(response);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal static async Task SongRequest(CompareResonseFormatApp serverResponse, SyncMp3AppContext dbContext)
    {
        int successfulDownloads = 0;
        int totalSongs = serverResponse.SongGuid.Count;
        foreach (var songGuid in serverResponse.SongGuid)
        {
            try
            {
                var client = new HttpClient();
                var request = ParseHTTP.HTTPRequestFormat("GET", "/get-music");

                request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());
                request.Headers.Add("GUID", await ModifyAppSettings.GetGuid());
                request.Headers.Add("SongGUID", songGuid);

                var response = await client.SendAsync(request);
                var (headers, body)  = await ParseHTTP.GetResponseHeadersAndMessage(response);
                if (response.IsSuccessStatusCode)
                {
                    ModifyMusic.TrySaveSong(body);
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}