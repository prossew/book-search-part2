using System.Collections.ObjectModel;
using BookSearchApp.Models;

namespace BookSearchApp.Views;

public partial class ItemPage : ContentPage
{
    private readonly ObservableCollection<Book> _books = new();

    public ItemPage()
    {
        InitializeComponent();
        BooksCollection.ItemsSource = _books;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadBooksAsync();
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            await LoadBooksAsync(e.NewTextValue);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }

    private async Task LoadBooksAsync(string query = null)
    {
        var books = string.IsNullOrWhiteSpace(query)
            ? await App.Database.GetBooksAsync()
            : await App.Database.SearchAsync(query);

        _books.Clear();
        foreach (var book in books)
            _books.Add(book);

        EmptyLabel.IsVisible = _books.Count == 0;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.CommandParameter is Book book)
            {
                bool confirm = await DisplayAlert("Удаление", $"Удалить книгу \"{book.Title}\"?", "Да", "Нет");
                if (confirm)
                {
                    await App.Database.DeleteAsync(book);
                    await LoadBooksAsync(SearchBox.Text);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }
}