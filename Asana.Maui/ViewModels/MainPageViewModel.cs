using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _isShowCompleted;
        private ToDoViewModel? _selectedToDo;
        private string _searchText = string.Empty;
        private ObservableCollection<ToDoViewModel> _allToDos = new ObservableCollection<ToDoViewModel>();

        public MainPageViewModel()
        {
            ClearSearchCommand = new Command(() => SearchText = string.Empty);
            RefreshToDos();
        }

        public ObservableCollection<ToDoViewModel> ToDos { get; set; } = new ObservableCollection<ToDoViewModel>();

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                NotifyPropertyChanged();
                FilterToDos();
            }
        }

        public ICommand ClearSearchCommand { get; set; }

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
            _allToDos.Clear();
            var todos = ToDoServiceProxy.Current.ToDos
                .Where(t => IsShowCompleted || t.IsCompleted != true)
                .Select(t => new ToDoViewModel { Model = t });

            foreach (var todo in todos)
            {
                _allToDos.Add(todo);
            }
            
            FilterToDos();
        }

        private void FilterToDos()
        {
            ToDos.Clear();
            
            IEnumerable<ToDoViewModel> filteredToDos = _allToDos;
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredToDos = _allToDos.Where(todo =>
                    todo.Model?.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    todo.Model?.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    todo.Model?.ProjectId?.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
                );
            }

            foreach (var todo in filteredToDos)
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