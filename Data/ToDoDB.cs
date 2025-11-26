using SQLite;

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