using System.ComponentModel.DataAnnotations;
namespace SyncMP3App.Data;
public class DeviceMusic
{
    public int Id { get; set; }
    [MaxLength(36)]
    public string SongGuid { get; set; } = null!;
    [MaxLength(1000)]
    public string Name {get; set;} = null!;
    [MaxLength(1000)]
    public string AbsolutePath {get; set;} = null!;
    public bool OriginalUpload {get; set;} = true;

}