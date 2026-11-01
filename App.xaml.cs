using BookSearchApp.Models;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Graphics;

namespace BookSearchApp;

public partial class App : Application
{
    public static BookDatabase Database { get; private set; }

    public App()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            ShowErrorScreen("UnhandledException: " + e.ExceptionObject);

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            ShowErrorScreen("UnobservedTaskException: " + e.Exception);
            e.SetObserved();
        };

        try
        {
            InitializeComponent();
            Database = new BookDatabase();
            MainPage = new MainPage();
        }
        catch (Exception ex)
        {
            ShowErrorScreen(ex.ToString());
        }
    }

    private void ShowErrorScreen(string message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            MainPage = new ContentPage
            {
                BackgroundColor = Colors.White,
                Content = new ScrollView
                {
                    Content = new Label
                    {
                        Text = message,
                        TextColor = Colors.Red,
                        FontSize = 14,
                        Margin = 20
                    }
                }
            };
        });
    }
}