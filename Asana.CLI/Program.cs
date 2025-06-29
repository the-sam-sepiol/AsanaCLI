using Asana.Library.Models;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using Asana.Library.Services;

namespace Asana
{
    public class Program
    {
        private static List<Project> projects = new();
        private static List<ToDo> toDos = new();
        private static int nextToDoId = 1;
        
        static int nextProjectId = 1;
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
                            var todo = ToDoServiceProxy.CreateToDo(ref nextToDoId);
                            toDos.Add(todo);
                            Console.WriteLine($"Created ToDo: {todo}");
                            break;
                        case 2:
                            if (ToDoServiceProxy.DeleteToDo(toDos) == true)
                                Console.WriteLine("ToDo deleted.");
                            break;
                        case 3:
                            ToDoServiceProxy.UpdateTodo(toDos);
                            break;
                        case 4:
                            ToDoServiceProxy.ListAllToDos(toDos);
                            break;
                        case 5:
                            var proj = ToDoServiceProxy.CreateProject(ref nextProjectId);
                            projects.Add(proj);
                            Console.WriteLine($"Created project: {proj}");
                            break;
                        case 6:
                            if (ToDoServiceProxy.DeleteProject(projects) == true)
                                Console.WriteLine("Project deleted.");
                            break;
                        case 7:
                            ToDoServiceProxy.UpdateProject(toDos, projects);
                            break;
                        case 8:
                            ToDoServiceProxy.ListAllProjects(projects);
                            break;
                        case 9:
                            ToDoServiceProxy.ListGivenProjectToDo(projects);
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
        
        
    }
}