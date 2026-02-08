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
    internal async Task Update()
    {
        using var dbContext = _factory.CreateDbContext();
        await ModifyMusic.SaveAllMusicInSQL(dbContext);
    }
    internal async Task Join()
    {
        System.Console.WriteLine("Join start");
    }
}