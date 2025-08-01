using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {
        private List<ToDo> _toDoList = new List<ToDo>();
        public List<ToDo> ToDos
        {
            get
            {
                return _toDoList.Take(100).ToList();
            }

            private set
            {
                if (value != _toDoList)
                {
                    _toDoList = value;
                }
            }
        }

        private ToDoServiceProxy()
        {
            // Initialize with sample data
            InitializeSampleData();
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ToDoServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToDoServiceProxy();
                }

                return instance;
            }
        }
        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            if (toDo == null)
                return null;

            // Ensure Project object is set if ProjectId is provided
            if (toDo.ProjectId.HasValue && toDo.ProjectId > 0 && toDo.Project == null)
            {
                toDo.Project = ProjectServiceProxy.Current.GetById(toDo.ProjectId.Value);
            }

            // Ensure User object is set if AssignedUserId is provided
            if (toDo.AssignedUserId.HasValue && toDo.AssignedUserId > 0 && toDo.AssignedUser == null)
            {
                toDo.AssignedUser = UserServiceProxy.Current.GetById(toDo.AssignedUserId.Value);
            }

            if (toDo.Id == 0)
            {
                toDo.Id = nextKey;
                _toDoList.Add(toDo);
            }
            else
            {
                var existing = GetById(toDo.Id);
                if (existing != null)
                {
                    // Update existing todo
                    existing.Name = toDo.Name;
                    existing.Description = toDo.Description;
                    existing.Priority = toDo.Priority;
                    existing.IsCompleted = toDo.IsCompleted;
                    existing.DueDate = toDo.DueDate;
                    existing.ProjectId = toDo.ProjectId;
                    existing.Project = toDo.Project;
                    existing.AssignedUserId = toDo.AssignedUserId;
                    existing.AssignedUser = toDo.AssignedUser;
                    
                    // Ensure Project object is set for existing todo too
                    if (existing.ProjectId.HasValue && existing.ProjectId > 0 && existing.Project == null)
                    {
                        existing.Project = ProjectServiceProxy.Current.GetById(existing.ProjectId.Value);
                    }
                    
                    // Ensure User object is set for existing todo too
                    if (existing.AssignedUserId.HasValue && existing.AssignedUserId > 0 && existing.AssignedUser == null)
                    {
                        existing.AssignedUser = UserServiceProxy.Current.GetById(existing.AssignedUserId.Value);
                    }
                }
                else
                {
                    // Add new todo with specific ID
                    _toDoList.Add(toDo);
                }
            }

            // Always update the project's completion percentage if the todo belongs to a project
            if (toDo?.ProjectId != null && toDo.ProjectId > 0)
            {
                var project = ProjectServiceProxy.Current.GetById(toDo.ProjectId.Value);
                if (project != null)
                {
                    // Ensure the todo is in the project's todo list
                    if (!project.ToDos.Contains(toDo))
                    {
                        project.ToDos.Add(toDo);
                    }
                    // This will recalculate the completion percentage
                    ProjectServiceProxy.Current.AddOrUpdate(project);
                }
            }
            else if (toDo?.Project != null)
            {
                // Fallback for when Project object is available but ProjectId might not be set
                ProjectServiceProxy.Current.AddOrUpdate(toDo.Project);
            }
            return toDo;
        }

        public void DisplayToDos(bool isShowCompleted = false)
        {
            if (isShowCompleted)
            {
                ToDos.ForEach(Console.WriteLine);
            }
            else
            {
                ToDos.Where(t => (t != null) && !(t?.IsCompleted ?? false))
                                .ToList()
                                .ForEach(Console.WriteLine);
            }
        }

        public ToDo? GetById(int id)
        {
            return ToDos.FirstOrDefault(t => t.Id == id);
        }

        public List<ToDo> GetToDosForUser(int userId)
        {
            return ToDos.Where(t => t.AssignedUserId == userId).ToList();
        }

        public List<ToDo> GetUnassignedToDos()
        {
            return ToDos.Where(t => t.AssignedUserId == null || t.AssignedUserId == 0).ToList();
        }

        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo == null)
            {
                return;
            }
            if (toDo.Project != null)
            {
                toDo.Project.ToDos.Remove(toDo);
                ProjectServiceProxy.Current.AddOrUpdate(toDo.Project);
            }
            _toDoList.Remove(toDo);
        }

        private void InitializeSampleData()
        {
            // Only initialize if no data exists
            if (_toDoList.Any()) return;

            // Ensure projects and users are initialized first
            var projects = ProjectServiceProxy.Current.Projects;
            var users = UserServiceProxy.Current.Users;

            // Create sample todos with user assignments
            var sampleTodos = new List<ToDo>
            {
                new ToDo 
                { 
                    Id = 1, 
                    Name = "Get contractor quotes", 
                    Description = "Contact 3 contractors for kitchen renovation estimates",
                    Priority = 3,
                    DueDate = DateTime.Now.AddDays(4),
                    IsCompleted = false,
                    ProjectId = projects.FirstOrDefault()?.Id,
                    AssignedUserId = users.FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 2, 
                    Name = "Choose tile samples", 
                    Description = "Visit tile store and select bathroom tiles",
                    Priority = 2,
                    DueDate = DateTime.Now.AddDays(9),
                    IsCompleted = false,
                    ProjectId = projects.FirstOrDefault()?.Id,
                    AssignedUserId = users.FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 3, 
                    Name = "Complete C# tutorial", 
                    Description = "Finish Microsoft Learn C# fundamentals course",
                    Priority = 2,
                    DueDate = DateTime.Now.AddDays(7),
                    IsCompleted = true,
                    ProjectId = projects.Skip(1).FirstOrDefault()?.Id,
                    AssignedUserId = users.Skip(1).FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 4, 
                    Name = "Build sample app", 
                    Description = "Create a todo list application using MAUI",
                    Priority = 3,
                    DueDate = DateTime.Now.AddDays(19),
                    IsCompleted = false,
                    ProjectId = projects.Skip(1).FirstOrDefault()?.Id,
                    AssignedUserId = users.Skip(1).FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 5, 
                    Name = "Morning run", 
                    Description = "30-minute jog around the neighborhood",
                    Priority = 1,
                    DueDate = DateTime.Now.AddDays(1),
                    IsCompleted = true,
                    ProjectId = projects.Skip(2).FirstOrDefault()?.Id,
                    AssignedUserId = users.Skip(2).FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 6, 
                    Name = "Meal prep", 
                    Description = "Prepare healthy meals for the week",
                    Priority = 3,
                    DueDate = DateTime.Now.AddDays(3),
                    IsCompleted = false,
                    ProjectId = projects.Skip(2).FirstOrDefault()?.Id,
                    AssignedUserId = users.Skip(2).FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 7, 
                    Name = "Buy groceries", 
                    Description = "Get ingredients for healthy meal prep",
                    Priority = 2,
                    DueDate = DateTime.Now.AddDays(1),
                    IsCompleted = false,
                    ProjectId = null,
                    AssignedUserId = users.Skip(3).FirstOrDefault()?.Id
                },
                new ToDo 
                { 
                    Id = 8, 
                    Name = "Review quarterly reports", 
                    Description = "Analyze Q3 financial performance",
                    Priority = 3,
                    DueDate = DateTime.Now.AddDays(5),
                    IsCompleted = false,
                    ProjectId = null,
                    AssignedUserId = users.FirstOrDefault()?.Id
                }
            };

            foreach (var todo in sampleTodos)
            {
                AddOrUpdate(todo);
            }
        }

    }
}