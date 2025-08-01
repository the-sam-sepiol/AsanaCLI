using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.Services
{
    public class CurrentUserService
    {
        private static CurrentUserService? instance;
        private User? _currentUser;

        private CurrentUserService()
        {
        }

        public static CurrentUserService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new CurrentUserService();
                }
                return instance;
            }
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public bool IsLoggedIn => _currentUser != null;

        public void Login(User user)
        {
            _currentUser = user;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public event EventHandler? UserChanged;

        protected virtual void OnUserChanged()
        {
            UserChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
