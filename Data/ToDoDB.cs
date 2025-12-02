using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TodoDatabase
{
    // SQLiteAsyncConnection kullanıyoruz ki UI'ı engellemesin
    readonly SQLiteAsyncConnection _database; 

    // Veritabanı dosya yolu parametre olarak alınır
    public TodoDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        // Eğer tablo yoksa oluştur
        _database.CreateTableAsync<TodoItem>().Wait(); 
    }

    // --- CRUD Metotları ---

    public Task<List<TodoItem>> GetItemsAsync()
    {
        return _database.Table<TodoItem>().ToListAsync();
    }

    public Task<TodoItem> GetItemAsync(int id)
    {
        return _database.GetAsync<TodoItem>(id);
    }

    public Task<int> SaveItemAsync(TodoItem item)
    {
        if (item.ID != 0)
        {
            return _database.UpdateAsync(item); // Güncelle
        }
        else
        {
            return _database.InsertAsync(item); // Ekle
        }
    }

    public Task<int> DeleteItemAsync(TodoItem item)
    {
        return _database.DeleteAsync(item);
    }
}
namespace CetTodoApp.Data;

public class ToDoDB
{
    private SQLiteAsyncConnection? _database;

    private async Task InitAsync()
    {
        if (_database is not null)
        {
            return;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "todos.db3");
        _database = new SQLiteAsyncConnection(databasePath);
        await _database.CreateTableAsync<TodoItem>();
    }



    public async Task CreateAsync(TodoItem item)
    {
        await InitAsync();
        await _database!.InsertAsync(item);
       
    }
    
    public async Task CreateAsync(string title, DateTime dueDate )
    {

        TodoItem item = new TodoItem();
        item.Title = title;
        item.DueDate = dueDate;

        await CreateAsync(item);
    }
    
    public async Task TogleCompletionStatusAync(TodoItem item)
    {
        await InitAsync();
        item.IsComplete =  !item.IsComplete;
        await _database!.UpdateAsync(item);
       
    }
    
    public async Task<List<TodoItem>> GetAllAsync()
    {
        await InitAsync();
        return await _database!.Table<TodoItem>().OrderByDescending(t=>t.DueDate).ToListAsync();
       
    }
    
    public async Task<List<TodoItem>> GetRecentlyCompletedOrNotCompletedAsync()
    {
        await InitAsync();
        return await _database!
            .Table<TodoItem>()
            .Where(t => (!t.IsComplete) || (t.IsComplete &&  t.DueDate.AddDays(-1) < DateTime.Now))
            .OrderByDescending(t=>t.DueDate)
            .ToListAsync();
       
    }

}
