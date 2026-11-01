using BookSearchApp.Models;

namespace BookSearchApp.Views;

public partial class AddItem : ContentPage
{
    public AddItem()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TitleEntry.Text) || string.IsNullOrWhiteSpace(AuthorEntry.Text))
            {
                await DisplayAlert("Ошибка", "Название и автор обязательны", "ОК");
                return;
            }

            int? year = null;
            if (!string.IsNullOrWhiteSpace(YearEntry.Text) && int.TryParse(YearEntry.Text, out int y))
                year = y;

            var book = new Book
            {
                Title = TitleEntry.Text.Trim(),
                Author = AuthorEntry.Text.Trim(),
                Genre = GenreEntry.Text?.Trim() ?? string.Empty,
                Year = year
            };

            await App.Database.AddAsync(book);

            TitleEntry.Text = string.Empty;
            AuthorEntry.Text = string.Empty;
            GenreEntry.Text = string.Empty;
            YearEntry.Text = string.Empty;

            await DisplayAlert("Готово", "Книга добавлена", "ОК");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }
}