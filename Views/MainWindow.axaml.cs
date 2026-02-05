using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SyncMP3App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void ToggleButtons(bool ToggleOn = true)
    {
        if (ToggleOn)
        {
            UpdateBtn.IsEnabled = true;
            JoinBtn.IsEnabled = true;
            GeneratePasswordBtn.IsEnabled = true;
            ViewMessagesBtn.IsEnabled = true;
            LeaveBtn.IsEnabled = true;
        }
        else
        {
            UpdateBtn.IsEnabled = false;
            JoinBtn.IsEnabled = false;
            GeneratePasswordBtn.IsEnabled = false;
            ViewMessagesBtn.IsEnabled = false;
            LeaveBtn.IsEnabled = false;
        }
    }
    private void BtnUpdate(object? sender, RoutedEventArgs e)
    {
        ToggleButtons(false);
        try
        {
            TextBlockA.Text = DatabaseConfig.ConnectionString.ToString();
        }
        catch
        {

        }
        finally
        {
            ToggleButtons(true);
        }
    }
    private void BtnJoin(object sender, RoutedEventArgs e)
    {
        ToggleButtons(false);
        try
        {

        }
        catch
        {

        }
        finally
        {
            ToggleButtons(true);
        }
    }
    private void BtnGeneratePassword(object sender, RoutedEventArgs e)
    {
        ToggleButtons(false);
        try
        {

        }
        catch
        {

        }
        finally
        {
            ToggleButtons(true);
        }
    }
    private void BtnViewMessages(object sender, RoutedEventArgs e)
    {
        ToggleButtons(false);
        try
        {

        }
        catch
        {

        }
        finally
        {
            ToggleButtons(true);
        }
    }
    private void BtnLeave(object sender, RoutedEventArgs e)
    {
        ToggleButtons(false);
        try
        {

        }
        catch
        {

        }
        finally
        {
            ToggleButtons(true);
        }
    }

}