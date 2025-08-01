using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class UserServiceProxy
    {
        private List<User> _userList = new List<User>();

        public List<User> Users
        {
            get
            {
                return _userList.Take(100).ToList();
            }
            private set
            {
                if (value != _userList)
                {
                    _userList = value;
                }
            }
        }

        private UserServiceProxy()
        {
            // Add some default users
            _userList.Add(new User { Id = 1, Name = "John Doe", Email = "john@example.com", Username = "john" });
            _userList.Add(new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Username = "jane" });
            _userList.Add(new User { Id = 3, Name = "Bob Johnson", Email = "bob@example.com", Username = "bob" });
            _userList.Add(new User { Id = 4, Name = "Alice Brown", Email = "alice@example.com", Username = "alice" });
        }

        private static UserServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (Users.Any())
                {
                    return Users.Select(u => u.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static UserServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserServiceProxy();
                }
                return instance;
            }
        }

        public User? AddOrUpdate(User? user)
        {
            if (user == null)
                return null;

            if (user.Id == 0)
            {
                user.Id = nextKey;
                _userList.Add(user);
            }
            else
            {
                var existing = GetById(user.Id);
                if (existing != null)
                {
                    existing.Name = user.Name;
                    existing.Email = user.Email;
                    existing.Username = user.Username;
                    existing.AssignedToDos = user.AssignedToDos ?? new List<ToDo>();
                }
                else
                {
                    // Add new user with specific ID
                    _userList.Add(user);
                }
            }
            return user;
        }

        public void DisplayUsers()
        {
            Users.ForEach(Console.WriteLine);
        }

        public User? GetById(int id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return Users.FirstOrDefault(u => u.Username?.Equals(username, StringComparison.OrdinalIgnoreCase) == true);
        }

        public void DeleteUser(User? user)
        {
            if (user == null)
                return;

            // Unassign all todos from this user
            var toDoSvc = ToDoServiceProxy.Current;
            var userTodos = toDoSvc.ToDos.Where(t => t.AssignedUserId == user.Id).ToList();
            foreach (var todo in userTodos)
            {
                todo.AssignedUserId = null;
                todo.AssignedUser = null;
                toDoSvc.AddOrUpdate(todo);
            }

            _userList.Remove(user);
        }
    }
}
