using Asana.Library.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Asana.Maui.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        public Project? Model { get; set; }

        public string DisplayText => $"{Model?.Id ?? -1}. {Model?.Name}";

        public string ToDosList
        {
            get
            {
                if (Model?.ToDos == null || !Model.ToDos.Any())
                    return "No ToDos assigned";
                
                var todoNames = Model.ToDos.Take(3).Select(t => $"• {t.Name}").ToList();
                var result = string.Join("\n", todoNames);
                
                if (Model.ToDos.Count > 3)
                    result += $"\n... and {Model.ToDos.Count - 3} more";
                
                return result;
            }
        }

        public override string ToString()
        {
            return DisplayText;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}