using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Asana.Maui.ViewModels
{
    public class ProjectsPageViewModel : INotifyPropertyChanged
    {
        private ProjectViewModel? _selectedProject;

        public ProjectsPageViewModel()
        {
            RefreshProjects();
        }

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

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
            Projects.Clear();
            var projects = ProjectServiceProxy.Current.Projects
                .Select(p => new ProjectViewModel { Model = p });

            foreach (var project in projects)
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