using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public static class ToDoServiceProxy
    {

        public static bool DeleteToDo(List<ToDo> toDos)
        {
            Console.Write("Enter ToDo ID: ");
            if (int.TryParse(Console.ReadLine(), out var id))
            {
                int index = -1;
                for (int i = 0; i < toDos.Count; i++)
                    if (toDos[i].Id == id)
                    {
                        index = i;
                        break;
                    }
                if (index >= 0)
                {
                    toDos.RemoveAt(index);
                    return true;
                }
                else
                {
                    Console.WriteLine("ToDo ID invalid.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("ToDo ID invalid.");
                return false;
            }
        }
        public static void UpdateTodo(List<ToDo> toDos)
        {
            Console.Write("Enter ToDo ID: ");
            if (int.TryParse(Console.ReadLine(), out var id))
            {
                int index = -1;
                for (int i = 0; i < toDos.Count; i++)
                    if (toDos[i].Id == id)
                    {
                        index = i;
                        break;
                    }
                if (index >= 0)
                {
                    Console.Write("Name: ");
                    toDos[index].Name = Console.ReadLine();
                    Console.Write("Description: ");
                    toDos[index].Description = Console.ReadLine();
                    Console.Write("Priority (1-5): ");
                    int.TryParse(Console.ReadLine(), out int priority);
                    toDos[index].Priority = priority;
                    Console.Write("Complete? (y/n): ");
                    var complete = Console.ReadLine();
                    bool done = false;
                    if (complete?.ToLower() == "y")
                        done = true;
                    else
                        done = false;
                    toDos[index].IsCompleted = done;
                    Console.WriteLine("ToDo updated.");
                    return;
                }
            }
        }

        public static void ListAllToDos(List<ToDo> toDos)
        {
            Console.WriteLine("All ToDos: ");
            foreach (var t in toDos)
                Console.WriteLine($"#{t.Id}: {t}");
        }
        public static bool DeleteProject(List<Project> projects)
        {
            Console.Write("Enter Project ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                int index = -1;
                for (int i = 0; i < projects.Count; i++)
                    if (projects[i].Id == id)
                    {
                        index = i;
                        break;
                    }
                if (index >= 0)
                {
                    projects.RemoveAt(index);
                    return true;
                }
                else
                {
                    Console.WriteLine("Project ID invalid.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Project ID invalid.");
                return false;
            }

        }

        public static void UpdateProject(List<ToDo> toDos, List<Project> projects)
        {
            Console.Write("Enter Project ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                int index = -1;
                for (int i = 0; i < projects.Count; i++)
                    if (projects[i].Id == id)
                    {
                        index = i;
                        break;
                    }
                if (index >= 0)
                {
                    Console.Write("Project Name: ");
                    projects[index].Name = Console.ReadLine();
                    Console.Write("Description: ");
                    projects[index].Description = Console.ReadLine();
                    Console.Write("Add ToDo to Project (ToDo ID): ");
                    if (int.TryParse(Console.ReadLine(), out int toDoId))
                    {
                        int toDoIndex = -1;
                        for (int i = 0; i < toDos.Count; i++)
                            if (toDos[i].Id == toDoId)
                            {
                                toDoIndex = i;
                                break;
                            }
                        projects[index].ToDos.Add(toDos[toDoIndex]);
                    }
                    Console.Write("Completion Percentage: ");
                    if (int.TryParse(Console.ReadLine(), out int percent))
                        projects[index].CompletionPercent = percent;

                }
                else
                    Console.WriteLine("Project ID not found.");
            }
        }
        public static void ListAllProjects(List<Project> projects)
        {
            Console.WriteLine("All Projects:");
            foreach (var p in projects)
                Console.WriteLine($"#{p.Id}: {p}");
        }

        public static void ListGivenProjectToDo(List<Project> projects)
        {
            Console.Write("Enter Project ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Project ID invalid."); return;
            }

            Project? proj = null;
            foreach (var p in projects)
                if (p.Id == id)
                {
                    proj = p;
                    break;
                }
            if (proj == null)
            {
                Console.WriteLine("Project ID invalid");
                return;
            }
            Console.WriteLine($"ToDos in {proj.Name}:");
            foreach (var t in proj.ToDos)
            {
                Console.WriteLine($"#{t.Id}: {t}");
            }

        }


        public static ToDo CreateToDo(ref int nextToDoId)
        {
            Console.Write("Name: ");
            var name = Console.ReadLine();
            Console.Write("Description: ");
            var description = Console.ReadLine();
            Console.Write("Priority (1-5): ");
            int.TryParse(Console.ReadLine(), out int priority);

            return new ToDo
            {
                Id = nextToDoId++,
                Name = name,
                Description = description,
                Priority = priority,
                IsCompleted = false
            };

        }

        public static Project CreateProject(ref int nextProjectId)
        {
            Console.Write("Project Name: ");
            var name = Console.ReadLine();
            Console.Write("Description: ");
            var description = Console.ReadLine();

            return new Project
            {
                Id = nextProjectId++,
                Name = name,
                Description = description,
                CompletionPercent = 0,
            };
        }
    }
}
