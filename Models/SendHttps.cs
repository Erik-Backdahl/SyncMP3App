using System;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SyncMP3App.Data;

class SendHttps
{
    internal static async Task<bool> PingRequest()
    {
        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("GET", "/ping");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());

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
    internal static async Task<string> CompareRequest(SyncMp3AppContext dbContext)
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
        var parsedResponse  = await ParseHTTP.GetResponseHeadersAndMessage(response);
        if (response.IsSuccessStatusCode)
        {
            return parsedResponse.Message;
        }
        else
        {
            throw new Exception($"{response.StatusCode}" + "message");
        }

    }
    internal static async Task<GenericResponse> JoinRequest(string password)
    {
        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("PATCH", "/join-network");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());
        request.Headers.Add("password", password);

        if (!string.IsNullOrEmpty(ModifyAppSettings.appGuid))
            throw new Exception("Already in a network");

        var response = await client.SendAsync(request);
        var parsedResponse = await ParseHTTP.GetResponseHeadersAndMessage(response);
        if (response.IsSuccessStatusCode)
        {
            return parsedResponse;
        }
        else
        {
            throw new Exception($"Failed to join network: {parsedResponse.Message}");
        }
    }
    internal static async Task<bool> CreateRequest()
    {
        var client = new HttpClient();
        var request = ParseHTTP.HTTPRequestFormat("POST", "/create-network");

        request.Headers.Add("UUID", await ModifyAppSettings.GetUuid());

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

    internal static async Task SongRequest(CompareResonseFormat serverResponse, SyncMp3AppContext dbContext)
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
                var parsedResponse = await ParseHTTP.GetResponseHeadersAndMessage(response);
                if (response.IsSuccessStatusCode)
                {
                    ModifyMusic.TrySaveSong(parsedResponse.Message);
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