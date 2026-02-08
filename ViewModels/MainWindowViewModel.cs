using System;
using System.Threading.Tasks;
using Avalonia.Controls;
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

    public MainWindowViewModel(EndpointEntry endpointEntry, IFolderPickerService folderPickerService)
    {
        _folderPickerService = folderPickerService;
        _endpointEntry = endpointEntry;
    }
    private bool CanExecuteAction() => !IsBusy;

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task UpdateAsync()
        => await RunBusyAsync(_endpointEntry.Update);

    [RelayCommand(CanExecute = nameof(CanExecuteAction))]
    private async Task JoinAsync()
        => await RunBusyAsync(_endpointEntry.Join);
        
    [RelayCommand]
    private async Task FolderPickerAsync()
        => await EasyEndPoints.FolderPicker(_folderPickerService);



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
