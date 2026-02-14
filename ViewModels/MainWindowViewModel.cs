using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SyncMP3App.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly EndpointEntry _endpointEntry;
    private readonly IFolderPickerService _folderPickerService;
    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string textBlockAText = "";
    [ObservableProperty]
    private string textBlockBText = "";
    [ObservableProperty]
    private string passwordBoxText = "";

    [ObservableProperty]
    private IBrush rectangleColor = Brushes.Gray;
    public MainWindowViewModel(EndpointEntry endpointEntry, IFolderPickerService folderPickerService)
    {
        _folderPickerService = folderPickerService;
        _endpointEntry = endpointEntry;
    }
    private bool CanExecuteAction() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task UpdateAsync()
    {
        try
        {
            if (await RunBusyAsync(_endpointEntry.Update))
            {
                RectangleColor = Brushes.Green;
                await CompareAsync();
            }
            else
            {
                RectangleColor = Brushes.Red;
            }

        }
        catch (Exception ex)
        {
            TextBlockBText = ex.Message;
            RectangleColor = Brushes.Red;
            Console.WriteLine(ex.Message);
        }

    }
    private async Task CompareAsync()
    {
        try
        {
            var response = await _endpointEntry.Compare();

            if (response.SongToDownload.Count > 0)
            {
                TextBlockAText = $"({response.SongToDownload.Count} New songs detected, attempting to download...";
                await _endpointEntry.RequestMusic(response.SongToDownload);
            }
            if (response.SongToUpload.Count > 0)
            {
                TextBlockBText = $"({response.SongToUpload.Count} upload requests";
                await _endpointEntry.RequestMusic(response.SongToUpload);
            }
            else
            {
                TextBlockAText = "No new songs detected";
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            TextBlockBText = ex.Message;
        }
    }


    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task CreateAsync()
    {
        await RunBusyAsync(_endpointEntry.Create);
    }

    [RelayCommand]
    private async Task FolderPickerAsync()
    {
        TextBlockAText = await EasyEndPoints.FolderPicker(_folderPickerService);
    }
    [RelayCommand]
    private async Task JoinAsync()
    {
        TextBlockAText = await RunBusyAsync(() => _endpointEntry.Join(PasswordBoxText));
    }



    private async Task<T> RunBusyAsync<T>(Func<Task<T>> action)
    {
        if (IsBusy) return default!;

        IsBusy = true;
        try
        {
            return await action();
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task RunBusyAsync(Func<Task> action)
    {
        if (IsBusy) return;

        IsBusy = true;
        try
        {
            await action();
        }
        finally
        {
            IsBusy = false;
        }
    }

}
