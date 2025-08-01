using Asana.Library.Models;
using Asana.Library.Services;
using System.Text;

namespace Asana.Maui.Services
{
    public class ExportImportService
    {
        public static async Task<string> ExportToTextAsync()
        {
            var exportText = new StringBuilder();
            
            // Export header
            exportText.AppendLine("=== ASANA CLI EXPORT ===");
            exportText.AppendLine($"Exported on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            exportText.AppendLine();

            // Export projects
            exportText.AppendLine("=== PROJECTS ===");
            foreach (var project in ProjectServiceProxy.Current.Projects)
            {
                exportText.AppendLine("PROJECT_START");
                exportText.AppendLine($"ID: {project.Id}");
                exportText.AppendLine($"NAME: {project.Name}");
                exportText.AppendLine($"DESCRIPTION: {project.Description ?? ""}");
                exportText.AppendLine($"COMPLETION: {project.CompletionPercent:F2}");
                exportText.AppendLine("PROJECT_END");
                exportText.AppendLine();
            }

            // Export users
            exportText.AppendLine("=== USERS ===");
            foreach (var user in UserServiceProxy.Current.Users)
            {
                exportText.AppendLine("USER_START");
                exportText.AppendLine($"ID: {user.Id}");
                exportText.AppendLine($"NAME: {user.Name}");
                exportText.AppendLine($"EMAIL: {user.Email ?? ""}");
                exportText.AppendLine($"USERNAME: {user.Username ?? ""}");
                exportText.AppendLine("USER_END");
                exportText.AppendLine();
            }

            // Export ToDos
            exportText.AppendLine("=== TODOS ===");
            var todos = ToDoServiceProxy.Current.ToDos;
            foreach (var todo in todos)
            {
                exportText.AppendLine("TODO_START");
                exportText.AppendLine($"ID: {todo.Id}");
                exportText.AppendLine($"NAME: {todo.Name}");
                exportText.AppendLine($"DESCRIPTION: {todo.Description ?? ""}");
                exportText.AppendLine($"PRIORITY: {todo.Priority}");
                exportText.AppendLine($"DUE_DATE: {todo.DueDate?.ToString("yyyy-MM-dd") ?? ""}");
                exportText.AppendLine($"IS_COMPLETED: {todo.IsCompleted}");
                exportText.AppendLine($"PROJECT_ID: {todo.ProjectId ?? -1}");
                exportText.AppendLine($"ASSIGNED_USER_ID: {todo.AssignedUserId ?? -1}");
                exportText.AppendLine("TODO_END");
                exportText.AppendLine();
            }

            return exportText.ToString();
        }

        public static async Task<bool> ImportFromTextAsync(string content)
        {
            try
            {
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(line => line.Trim())
                                  .ToArray();

                var projects = new List<Project>();
                var users = new List<User>();
                var todos = new List<ToDo>();

                // Parse Projects, Users, and ToDos
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == "PROJECT_START")
                    {
                        var project = ParseProject(lines, ref i);
                        if (project != null)
                        {
                            projects.Add(project);
                        }
                    }
                    else if (lines[i] == "USER_START")
                    {
                        var user = ParseUser(lines, ref i);
                        if (user != null)
                        {
                            users.Add(user);
                        }
                    }
                    else if (lines[i] == "TODO_START")
                    {
                        var todo = ParseToDo(lines, ref i);
                        if (todo != null)
                        {
                            todos.Add(todo);
                        }
                    }
                }

                // Clear existing data and import new data
                ProjectServiceProxy.Current.Projects.Clear();
                UserServiceProxy.Current.Users.Clear();
                ToDoServiceProxy.Current.ToDos.Clear();

                // Import projects first
                foreach (var project in projects)
                {
                    ProjectServiceProxy.Current.AddOrUpdate(project);
                }

                // Import users
                foreach (var user in users)
                {
                    UserServiceProxy.Current.AddOrUpdate(user);
                }

                // Import todos
                foreach (var todo in todos)
                {
                    ToDoServiceProxy.Current.AddOrUpdate(todo);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Import error: {ex.Message}");
                return false;
            }
        }

        private static Project? ParseProject(string[] lines, ref int index)
        {
            var project = new Project();
            index++; // Skip PROJECT_START

            while (index < lines.Length && lines[index] != "PROJECT_END")
            {
                var line = lines[index];
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    switch (key)
                    {
                        case "ID":
                            if (int.TryParse(value, out int id))
                                project.Id = id;
                            break;
                        case "NAME":
                            project.Name = value;
                            break;
                        case "DESCRIPTION":
                            project.Description = string.IsNullOrEmpty(value) ? null : value;
                            break;
                        case "COMPLETION":
                            if (double.TryParse(value, out double completion))
                                project.CompletionPercent = (int)Math.Round(completion);
                            break;
                    }
                }
                index++;
            }

            return string.IsNullOrEmpty(project.Name) ? null : project;
        }

        private static User? ParseUser(string[] lines, ref int index)
        {
            var user = new User();
            index++; // Skip USER_START

            while (index < lines.Length && lines[index] != "USER_END")
            {
                var line = lines[index];
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    switch (key)
                    {
                        case "ID":
                            if (int.TryParse(value, out int id))
                                user.Id = id;
                            break;
                        case "NAME":
                            user.Name = value;
                            break;
                        case "EMAIL":
                            user.Email = string.IsNullOrEmpty(value) ? null : value;
                            break;
                        case "USERNAME":
                            user.Username = string.IsNullOrEmpty(value) ? null : value;
                            break;
                    }
                }
                index++;
            }

            return string.IsNullOrEmpty(user.Name) ? null : user;
        }

        private static ToDo? ParseToDo(string[] lines, ref int index)
        {
            var todo = new ToDo();
            index++; // Skip TODO_START

            while (index < lines.Length && lines[index] != "TODO_END")
            {
                var line = lines[index];
                var parts = line.Split(':', 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    switch (key)
                    {
                        case "ID":
                            if (int.TryParse(value, out int id))
                                todo.Id = id;
                            break;
                        case "NAME":
                            todo.Name = value;
                            break;
                        case "DESCRIPTION":
                            todo.Description = string.IsNullOrEmpty(value) ? null : value;
                            break;
                        case "PRIORITY":
                            if (int.TryParse(value, out int priority))
                                todo.Priority = priority;
                            break;
                        case "DUE_DATE":
                            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, out DateTime dueDate))
                                todo.DueDate = dueDate;
                            break;
                        case "IS_COMPLETED":
                            if (bool.TryParse(value, out bool isCompleted))
                                todo.IsCompleted = isCompleted;
                            break;
                        case "PROJECT_ID":
                            if (int.TryParse(value, out int projectId) && projectId != -1)
                                todo.ProjectId = projectId;
                            break;
                        case "ASSIGNED_USER_ID":
                            if (int.TryParse(value, out int userId) && userId != -1)
                                todo.AssignedUserId = userId;
                            break;
                    }
                }
                index++;
            }

            return string.IsNullOrEmpty(todo.Name) ? null : todo;
        }

        public static async Task<string> GetExportFilePathAsync()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"AsanaExport_{timestamp}.txt";
            
            // For desktop platforms, use Documents folder
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documentsPath, fileName);
        }

        public static async Task SaveToFileAsync(string content, string filePath)
        {
            await File.WriteAllTextAsync(filePath, content);
        }

        public static async Task<string> LoadFromFileAsync(string filePath)
        {
            return await File.ReadAllTextAsync(filePath);
        }
    }
}
