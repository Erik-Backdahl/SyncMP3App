using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using SyncMP3App;

public class FolderPickerService : IFolderPickerService
{
    public async Task<string?> PickFolderAsync()
    {
        // Get the main window
        if (App.MainWindow is not Window window)
            return null;

        // Get the storage provider
        var storageProvider = window.StorageProvider;

        // Show folder picker dialog
        var result = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Folder",
            AllowMultiple = false
        });

        // Return the selected folder path
        return result.FirstOrDefault()?.Path.LocalPath;
    }
}