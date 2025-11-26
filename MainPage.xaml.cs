using CetTodoApp.Data;

namespace CetTodoApp;

public partial class MainPage : ContentPage
{
    private ToDoDB db=new ToDoDB();

    public MainPage()
    {
        InitializeComponent();
        //FakeDb.AddToDo("Test1" ,DateTime.Now.AddDays(-1));
        //FakeDb.AddToDo("Test2" ,DateTime.Now.AddDays(1));
        //FakeDb.AddToDo("Test3" ,DateTime.Now);
        RefreshListView();
        ;


    }


    private async void AddButton_OnClicked(object? sender, EventArgs e)
    {
        //FakeDb.AddToDo(Title.Text, DueDate.Date);
        await db.CreateAsync(Title.Text, DueDate.Date);
        Title.Text = string.Empty;
        DueDate.Date=DateTime.Now;
        await RefreshListView();
    }

    private async Task RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = await db.GetAllAsync();
        //. FakeDb.Data.Where(x => !x.IsComplete ||
        //                                             (x.IsComplete && x.DueDate > DateTime.Now.AddDays(-1)))
        //.ToList();
    }

    private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        var item = e.SelectedItem as TodoItem;
        await db.TogleCompletionStatusAync(item!);
      // FakeDb.ChageCompletionStatus(item);
       await RefreshListView();
       
    }
}