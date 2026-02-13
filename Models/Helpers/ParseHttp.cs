
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

class ParseHTTP
{
    public static string baseAddress = "https://server.syncmp3.com";
    public static HttpRequestMessage HTTPRequestFormat(string requestType, string path)
    {
        HttpMethod HttpType;

        if (requestType == "GET")
            HttpType = HttpMethod.Get;
        else if (requestType == "POST")
            HttpType = HttpMethod.Post;
        else if (requestType == "PUT")
            HttpType = HttpMethod.Put;
        else if (requestType == "PATCH")
            HttpType = HttpMethod.Patch;
        else
            HttpType = HttpMethod.Get;

        HttpRequestMessage request = new(HttpType, $"{baseAddress}" + $"{path}");

        return request;
    }
    public static async Task<GenericResponse> GetResponseHeadersAndMessage(HttpResponseMessage serverResponse)
    {
        GenericResponse response = new();
        
        foreach (var header in serverResponse.Headers)
        {
            response.Headers.Add(header.Key, header.Value.ToString());
        }

        if (serverResponse.Content != null)
        {
            response.Message = await serverResponse.Content.ReadAsStringAsync();
        }
    
        return response;
    }
    public static async Task<(Dictionary<string, string>, byte[] musicBody)> GetSongHeadersAndSongBytes(HttpResponseMessage serverResponse)
    {
        var headers = new Dictionary<string, string>();

        foreach (var header in serverResponse.Headers)
        {
            headers[header.Key] = string.Join(", ", header.Value);
        }

        byte[] musicBody = await serverResponse.Content.ReadAsByteArrayAsync();

        return (headers, musicBody);
    }
}