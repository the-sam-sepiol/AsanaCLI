using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        private ProjectViewModel? _selectedProject;
        private string _searchText = string.Empty;
        private ObservableCollection<ProjectViewModel> _allProjects = new ObservableCollection<ProjectViewModel>();

        public ProjectsPageViewModel()
        {
            ClearSearchCommand = new Command(() => SearchText = string.Empty);
            RefreshProjects();
        }

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                NotifyPropertyChanged();
                FilterProjects();
            }
        }

        public ICommand ClearSearchCommand { get; set; }

        public ProjectViewModel? SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                NotifyPropertyChanged();
            }
        }

        public void RefreshProjects()
        {
            _allProjects.Clear();
            var projects = ProjectServiceProxy.Current.Projects
                .Select(p => new ProjectViewModel { Model = p });

            foreach (var project in projects)
            {
                _allProjects.Add(project);
            }
            
            FilterProjects();
        }

        private void FilterProjects()
        {
            Projects.Clear();
            
            IEnumerable<ProjectViewModel> filteredProjects = _allProjects;
            
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredProjects = _allProjects.Where(project =>
                    project.Model?.Name?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    project.Model?.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true
                );
            }

            foreach (var project in filteredProjects)
            {
                Projects.Add(project);
            }
        }

        public void DeleteProject(Project? project)
        {
            if (project != null)
            {
                ProjectServiceProxy.Current.DeleteProject(project);
                RefreshProjects();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}