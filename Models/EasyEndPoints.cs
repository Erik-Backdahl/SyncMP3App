using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

class EasyEndPoints
{
    internal static async Task FolderPicker(IFolderPickerService folderPickerService)
    {
        var selectedFolder = await folderPickerService.PickFolderAsync();

        if (selectedFolder != null)
        {
            await ModifyAppSettings.TryAddRegisteredFolder(selectedFolder);
        }
    }
}