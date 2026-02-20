using System.Collections.Generic;
using System.Text.Json.Serialization;

public class CompareResonseFormatApp
{
    public List<string> SongToDownload { get; set; } = new List<string>();
    public List<string> SongToUpload { get; set; } = new List<string>();
    public int NewSongsRequested { get; set; } = 0;
    public List<string> SongsNotAvailibleName {get; set;} = new List<string>();
}