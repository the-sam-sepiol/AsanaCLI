using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public enum ToDoSortOption
    {
        Name,
        Priority,
        DueDate,
        Project
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        private bool _isShowCompleted;
        private ToDoViewModel? _selectedToDo;
        private string _searchText = string.Empty;
        private ObservableCollection<ToDoViewModel> _allToDos = new ObservableCollection<ToDoViewModel>();
        private ToDoSortOption _selectedSortOption = ToDoSortOption.Name;
        private bool _isSortDescending = false;

        public MainPageViewModel()
        {
            ClearSearchCommand = new Command(() => SearchText = string.Empty);
            SortCommand = new Command<string>(SortToDos);
            ToggleSortDirectionCommand = new Command(() => 
            {
                IsSortDescending = !IsSortDescending;
                ApplySortingAndFiltering();
            });
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
                ApplySortingAndFiltering();
            }
        }

        public ICommand ClearSearchCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand ToggleSortDirectionCommand { get; set; }

        public ToDoSortOption SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                NotifyPropertyChanged();
                ApplySortingAndFiltering();
            }
        }

        public bool IsSortDescending
        {
            get => _isSortDescending;
            set
            {
                _isSortDescending = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(SortDirectionText));
            }
        }

        public string SortDirectionText => IsSortDescending ? "DESC" : "ASC";

        public List<string> SortOptions => new List<string> { "Name", "Priority", "DueDate", "Project" };

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
            
            ApplySortingAndFiltering();
        }

        private void SortToDos(string sortOption)
        {
            if (Enum.TryParse<ToDoSortOption>(sortOption, out var option))
            {
                SelectedSortOption = option;
            }
        }

        private void ApplySortingAndFiltering()
        {
            FilterToDos();
        }

        private void FilterToDos()
        {
            IEnumerable<ToDoViewModel> filteredToDos = _allToDos;
            
            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredToDos = filteredToDos.Where(todo =>
                    todo.Model?.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    todo.Model?.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    todo.Model?.ProjectId?.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
                );
            }

            // Apply sorting
            filteredToDos = SelectedSortOption switch
            {
                ToDoSortOption.Name => IsSortDescending 
                    ? filteredToDos.OrderByDescending(t => t.Model?.Name) 
                    : filteredToDos.OrderBy(t => t.Model?.Name),
                ToDoSortOption.Priority => IsSortDescending 
                    ? filteredToDos.OrderByDescending(t => t.Model?.Priority) 
                    : filteredToDos.OrderBy(t => t.Model?.Priority),
                ToDoSortOption.DueDate => IsSortDescending 
                    ? filteredToDos.OrderByDescending(t => t.Model?.DueDate) 
                    : filteredToDos.OrderBy(t => t.Model?.DueDate),
                ToDoSortOption.Project => IsSortDescending 
                    ? filteredToDos.OrderByDescending(t => t.Model?.Project?.Name) 
                    : filteredToDos.OrderBy(t => t.Model?.Project?.Name),
                _ => filteredToDos
            };

            ToDos.Clear();
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