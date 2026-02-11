using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

class EasyEndPoints
{
    internal static async Task<string> FolderPicker(IFolderPickerService folderPickerService)
    {
        try
        {
            var selectedFolder = await folderPickerService.PickFolderAsync();

            if (selectedFolder != null)
            {
                var message = await ModifyAppSettings.TryAddRegisteredFolder(selectedFolder);
                return message;
            }
            return "No folder selected";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}