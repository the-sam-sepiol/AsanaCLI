using Asana.Library.Models;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

namespace Asana
{
    public class Program
    {
        private static List<Project> projects = new();
        private static List<ToDo> toDos = new();
        private static int nextToDoId = 1;
        private static int nextProjectId = 1;
        public static void Main(string[] args)
        {
            int choiceInt;
            do
            {
                Console.WriteLine("\nChoose a menu option:");
                Console.WriteLine("1. Create a ToDo");
                Console.WriteLine("2. Delete a ToDo");
                Console.WriteLine("3. Update a ToDo");
                Console.WriteLine("4. List all ToDos");
                Console.WriteLine("5. Create a Project");
                Console.WriteLine("6. Delete a Project");
                Console.WriteLine("7. Update a Project");
                Console.WriteLine("8. List all Projects");
                Console.WriteLine("9. List all ToDos in a Project");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");

                var choice = Console.ReadLine() ?? "0";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 0:
                            break;
                        case 1:
                            var todo = CreateToDo();
                            toDos.Add(todo);
                            Console.WriteLine($"Created ToDo: {todo}");
                            break;
                        case 2:
                            if (DeleteToDo() == true)
                                Console.WriteLine("ToDo deleted.");
                            break;
                        case 3:
                            UpdateTodo();
                            break;
                        case 4:
                            ListAllToDos();
                            break;
                        case 5:
                            var proj = CreateProject();
                            projects.Add(proj);
                            Console.WriteLine($"Created project: {proj}");
                            break;
                        case 6:
                            if (DeleteProject() == true)
                                Console.WriteLine("Project deleted.");
                            break;
                        case 7:
                            UpdateProject();
                            break;
                        case 8:
                            ListAllProjects();
                            break;
                        case 9:
                            ListGivenProjectToDo();
                            break;
                        default:
                            Console.WriteLine("ERROR: Unknown menu selection.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"ERROR: {choice} is not a valid menu selection.");
                }

            } while (choiceInt != 0);

        }
        private static bool DeleteToDo()
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
        private static void UpdateTodo()
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

        private static void ListAllToDos()
        {
            Console.WriteLine("All ToDos: ");
            foreach (var t in toDos)
                Console.WriteLine($"#{t.Id}: {t}");
        }
        private static bool DeleteProject()
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

        private static void UpdateProject()
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
        private static void ListAllProjects()
        {
            Console.WriteLine("All Projects:");
            foreach (var p in projects)
                Console.WriteLine($"#{p.Id}: {p}");
        }

        private static void ListGivenProjectToDo()
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


        private static ToDo CreateToDo()
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

        private static Project CreateProject()
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