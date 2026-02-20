using Microsoft.EntityFrameworkCore;

namespace SyncMP3App.Data;

public partial class SyncMp3AppContext : DbContext
{
    public SyncMp3AppContext(DbContextOptions<SyncMp3AppContext> options)
        : base(options)
    {
    }
    public DbSet<DeviceMusic> DeviceMusics { get; set; }
    public DbSet<Messages> Messages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeviceMusic>();
        modelBuilder.Entity<Messages>();
    }
}
