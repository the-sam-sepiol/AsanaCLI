using Asana.Library.Models;
using Asana.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Asana.Maui.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private User? _selectedUser;

        public LoginViewModel()
        {
            RefreshUsers();
        }

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(HasSelectedUser));
            }
        }

        public bool HasSelectedUser => SelectedUser != null;

        public void RefreshUsers()
        {
            Users.Clear();
            var users = UserServiceProxy.Current.Users;

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
