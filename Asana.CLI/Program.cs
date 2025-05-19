using Asana.Library.Models;
using System;

namespace Asana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ToDo MyFirstToDo = new ToDo();
            Console.WriteLine(MyFirstToDo.Name?.Length);
        }
    }
}