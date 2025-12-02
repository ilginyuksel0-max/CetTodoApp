using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using CetTodoApp.Models;

namespace CetTodoApp
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TaskItem> Tasks { get; set; } = new ObservableCollection<TaskItem>();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            DueDatePicker.Date = DateTime.Today;
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            string title = TitleEntry.Text?.Trim() ?? "";
            DateTime dueDate = DueDatePicker.Date;

            // 🔥 Validation 1: Title boş olamaz
            if (string.IsNullOrWhiteSpace(title))
            {
                await DisplayAlert("Error", "Title is required!", "OK");
                return;
            }

            // 🔥 Validation 2: Due date geçmiş olamaz
            if (dueDate.Date < DateTime.Today)
            {
                await DisplayAlert("Error", "Due date cannot be in the past!", "OK");
                return;
            }

            Tasks.Add(new TaskItem
            {
                Id = Tasks.Count + 1,
                Title = title,
                DueDate = dueDate,
                IsCompleted = false
            });

            TitleEntry.Text = "";
            DueDatePicker.Date = DateTime.Today;
        }

        private void OnToggleCompleteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TaskItem task)
            {
                task.IsCompleted = !task.IsCompleted;

                int index = Tasks.IndexOf(task);
                if (index >= 0)
                {
                    Tasks.RemoveAt(index);
                    Tasks.Insert(index, task); // UI refresh
                }
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TaskItem task)
            {
                bool confirm = await DisplayAlert("Delete", $"Delete '{task.Title}'?", "Yes", "No");
                if (confirm)
                {
                    Tasks.Remove(task);
                }
            }
        }
    }
}
