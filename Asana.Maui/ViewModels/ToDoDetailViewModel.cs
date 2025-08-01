using Asana.Library.Models;
using Asana.Library.Services;
using Asana.Maui.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class UserViewModel
    {
        public User? Model { get; set; }
        public string DisplayText => Model?.Name ?? "Unknown User";
    }

    public class ToDoDetailViewModel
    {
        public ToDoDetailViewModel()
        {
            if (Model == null)
            {
                Model = new ToDo();
            }
        }

        public ToDoDetailViewModel(int id)
        {
            Model = ToDoServiceProxy.Current.GetById(id) ?? new ToDo();
        }

        public ToDoDetailViewModel(ToDo? model)
        {
            Model = model ?? new ToDo();
        }

        public ToDo? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }

        public List<int> Priorities => new List<int> {1, 2, 3, 4, 5};

        public ObservableCollection<ProjectViewModel> Projects
        {
            get
            {
                var allProjects = new List<ProjectViewModel>
                {
                    new ProjectViewModel { Model = new Project { Id = 0, Name = "No Project" } }
                };
                
                var projects = ProjectServiceProxy.Current.Projects
                    .Select(p => new ProjectViewModel { Model = p });
                
                allProjects.AddRange(projects);
                return new ObservableCollection<ProjectViewModel>(allProjects);
            }
        }

        public ObservableCollection<UserViewModel> Users
        {
            get
            {
                var allUsers = new List<UserViewModel>
                {
                    new UserViewModel { Model = new User { Id = 0, Name = "Unassigned" } }
                };
                
                var users = UserServiceProxy.Current.Users
                    .Select(u => new UserViewModel { Model = u });
                
                allUsers.AddRange(users);
                return new ObservableCollection<UserViewModel>(allUsers);
            }
        }

        public int SelectedPriority
        {
            get => Model?.Priority ?? 4;
            set
            {
                if (Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                }
            }
        }

        public UserViewModel? SelectedUser
        {
            get
            {
                if (Model?.AssignedUserId == null || Model.AssignedUserId == 0)
                    return Users.FirstOrDefault(u => u.Model?.Id == 0);
                
                return Users.FirstOrDefault(u => u.Model?.Id == Model?.AssignedUserId);
            }
            set
            {
                if (Model != null)
                {
                    if (value?.Model?.Id == 0)
                    {
                        Model.AssignedUserId = null;
                        Model.AssignedUser = null;
                    }
                    else
                    {
                        Model.AssignedUserId = value?.Model?.Id;
                        Model.AssignedUser = value?.Model;
                    }
                }
            }
        }

        public ProjectViewModel? SelectedProject
        {
            get
            {
                if (Model?.ProjectId == null || Model.ProjectId == 0)
                    return Projects.FirstOrDefault(p => p.Model?.Id == 0);
                
                return Projects.FirstOrDefault(p => p.Model?.Id == Model?.ProjectId);
            }
            set
            {
                if (Model != null)
                {
                    if (value?.Model?.Id == 0)
                    {
                        Model.ProjectId = null;
                        Model.Project = null;
                    }
                    else
                    {
                        Model.ProjectId = value?.Model?.Id;
                        Model.Project = value?.Model;
                    }
                }
            }
        }

        public DateTime DueDate
        {
            get => Model?.DueDate ?? DateTime.Today.AddDays(7);
            set
            {
                if (Model != null)
                {
                    Model.DueDate = value;
                }
            }
        }

        public void DoDelete()
        {
            ToDoServiceProxy.Current.DeleteToDo(Model);
        }

        public void AddOrUpdateToDo()
        {
            // Handle project assignment
            if (Model?.Project != null && Model.ProjectId.HasValue && Model.ProjectId > 0)
            {
                var project = ProjectServiceProxy.Current.GetById(Model.ProjectId.Value);
                if (project != null && !project.ToDos.Contains(Model))
                {
                    project.ToDos.Add(Model);
                    ProjectServiceProxy.Current.AddOrUpdate(project);
                }
            }
            
            ToDoServiceProxy.Current.AddOrUpdate(Model);
        }
    }
}