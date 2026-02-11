using Avalonia.Controls;
using SyncMP3App.ViewModels;

namespace SyncMP3App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        App.MainWindow = this;
    }
}