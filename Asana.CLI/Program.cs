using Asana.Library.Models;
using System;
using System.Net.Http.Headers;

namespace Asana
{
    public class Program
    {
        private static List<Project> projects = new();
        private static List<ToDo > toDos = new();
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

                var choice = Console.ReadLine() ?? "0";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 1:


                            toDos.Add(new ToDo { Name = name, Description = description });
                            break;
                        case 2:
                            break;
                        default:
                            Console.WriteLine("ERROR: Unknown menu selection");
                            break;
                    }
                } else
                {
                    Console.WriteLine($"ERROR: {choice} is not a valid menu selection");
                }

                if(toDos.Any())
                {
                    Console.WriteLine(toDos.Last());
                }

            } while (choiceInt != 0);

        }
        private static void CreateToDo()
        {
            Console.Write("Name:");
            var name = Console.ReadLine();
            Console.Write("Description:");
            var description = Console.ReadLine();
            Console.Write("Priority (1-5): ");
            int.TryParse(Console.ReadLine(), out int priority);
        }
    }
}