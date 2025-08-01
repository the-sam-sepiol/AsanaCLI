using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public List<ToDo> AssignedToDos { get; set; } = new List<ToDo>();

        public override string ToString()
        {
            return $"User Id: {Id}, Name: {Name}, Email: {Email}, Username: {Username}";
        }
    }
}
