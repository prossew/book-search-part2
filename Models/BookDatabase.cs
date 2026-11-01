using System.IO;
using Microsoft.Maui.Storage;
using SQLite;

namespace BookSearchApp.Models;

public class BookDatabase
{
    private readonly SQLiteAsyncConnection _database;
    private bool _initialized;

    public BookDatabase()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "books.db3");
        _database = new SQLiteAsyncConnection(dbPath);
    }

    private async Task Init()
    {
        if (_initialized)
            return;

        await _database.CreateTableAsync<Book>();
        await SeedDataAsync();
        _initialized = true;
    }

    private async Task SeedDataAsync()
    {
        var count = await _database.Table<Book>().CountAsync();
        if (count == 0)
        {
            await _database.InsertAllAsync(new[]
            {
                new Book { Title = "Война и мир", Author = "Лев Толстой", Genre = "Роман", Year = 1869 },
                new Book { Title = "Преступление и наказание", Author = "Фёдор Достоевский", Genre = "Роман", Year = 1866 },
                new Book { Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Genre = "Роман", Year = 1967 },
                new Book { Title = "451 градус по Фаренгейту", Author = "Рэй Брэдбери", Genre = "Фантастика", Year = 1953 },
                new Book { Title = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", Genre = "Фэнтези", Year = 1997 },
                new Book { Title = "Анна Каренина", Author = "Лев Толстой", Genre = "Роман", Year = 1878 },
                new Book { Title = "Путешествие к центру Земли", Author = "Жюль Верн", Genre = "Приключения", Year = 1864 },
                new Book { Title = "Три мушкетёра", Author = "Александр Дюма", Genre = "Приключения", Year = 1844 }
            });
        }
    }

    public async Task<List<Book>> GetBooksAsync()
    {
        await Init();
        return await _database.Table<Book>().ToListAsync();
    }

    public async Task<List<Book>> SearchAsync(string query)
    {
        await Init();
        var all = await _database.Table<Book>().ToListAsync();
        if (string.IsNullOrWhiteSpace(query))
            return all;
        return all.Where(b =>
            b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Author.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            b.Genre.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<int> AddAsync(Book book)
    {
        await Init();
        return await _database.InsertAsync(book);
    }

    public async Task<int> DeleteAsync(Book book)
    {
        await Init();
        return await _database.DeleteAsync(book);
    }
}