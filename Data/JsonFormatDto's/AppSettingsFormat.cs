using System.Collections.Generic;
public class AppSettingsFormat
{
    public string GUID { get; set; } = "";
    public string UUID { get; set; } = null!;
    public string DownloadFolder { get; set; } = null!;
    public List<string> RegisteredFolders { get; set; } = null!;
}
