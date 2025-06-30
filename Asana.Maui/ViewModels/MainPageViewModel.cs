using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _isShowCompleted;
        private ToDoViewModel? _selectedToDo;

        public MainPageViewModel()
        {
            RefreshToDos();
        }

        public ObservableCollection<ToDoViewModel> ToDos { get; set; } = new ObservableCollection<ToDoViewModel>();

        public bool IsShowCompleted
        {
            get => _isShowCompleted;
            set
            {
                _isShowCompleted = value;
                RefreshToDos();
                NotifyPropertyChanged();
            }
        }

        public ToDoViewModel? SelectedToDo
        {
            get => _selectedToDo;
            set
            {
                _selectedToDo = value;
                NotifyPropertyChanged();
            }
        }

        public void RefreshToDos()
        {
            ToDos.Clear();
            var todos = ToDoServiceProxy.Current.ToDos
                .Where(t => IsShowCompleted || t.IsCompleted != true)
                .Select(t => new ToDoViewModel { Model = t });

            foreach (var todo in todos)
            {
                ToDos.Add(todo);
            }
        }

        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo != null)
            {
                ToDoServiceProxy.Current.DeleteToDo(toDo);
                RefreshToDos();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}