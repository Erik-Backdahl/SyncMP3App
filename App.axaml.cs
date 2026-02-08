using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SyncMP3App.ViewModels;
using SyncMP3App.Views;
using System;
using Microsoft.Extensions.DependencyInjection;
using SyncMP3App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace SyncMP3App;

public partial class App : Application
{
    public static Visual? MainWindow { get; internal set; }
    public IServiceProvider? Services { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        StartUpAction.CheckEssentialFiles();
        var services = new ServiceCollection();

        services.AddDbContextFactory<SyncMp3AppContext>(options =>
            options.UseSqlite(DatabaseConfig.ConnectionString));

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainWindow>();
        services.AddTransient<EndpointEntry>();

        services.AddSingleton<IFolderPickerService, FolderPickerService>();

        Services = services.BuildServiceProvider();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}