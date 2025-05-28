using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RealtyCRM.DTOs;
using RealtyCRMClient.Models;
using System.Threading.Tasks;
using RealtyCRMClient.ViewModels;
using System.Diagnostics;
using Newtonsoft.Json;
using RealtyCRMClient.DTOs;
using System.Net.Http;
using System.Text;

namespace RealtyCRMClient.ViewModels
{
    public class TasksViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private ObservableCollection<TaskListItem> _allTasks = new();

        // Списки задач по статусу
        public ObservableCollection<TaskListItem> QueueItems { get; set; } = new();
        public ObservableCollection<TaskListItem> InWorkItems { get; set; } = new();
        public ObservableCollection<TaskListItem> WaitingItems { get; set; } = new();
        public ObservableCollection<TaskListItem> DoneItems { get; set; } = new();

        public ICommand RefreshCommand => new RelayCommand(RefreshData);
        public ICommand CreateTaskCommand => new RelayCommand(OpenCreateTaskWindow);

        public async Task UpdateTaskStatus(long id, int newStatus)
        {
            var dto = new UpdateTaskObjectDto
            {
                Status = newStatus.ToString()
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PutAsync($"https://localhost:5001/api/TaskObject/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Ошибка при обновлении статуса задачи");
            }
        }



        public TasksViewModel()
        {

            try
            {
                _apiService = new ApiService();
                LoadTasks();

            }
            catch (Exception ex)
            {
                // Это поможет понять, где ошибка
                Debug.WriteLine($"Ошибка создания TasksViewModel: {ex.Message}");
            }
        }

        private async Task LoadTasks()
        {
            try
            {
                QueueItems.Clear();
                InWorkItems.Clear();
                WaitingItems.Clear();
                DoneItems.Clear();

                var tasks = await _apiService.GetAllTasksAsync(); // Должен быть реализован в ApiService

                foreach (var task in tasks)
                {
                    var item = new TaskListItem
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = int.TryParse(task.Status, out var status) ? (int?)status : null,
                        PersonalName = task.Personal?.Name ?? "Не назначен"
                    };

                    switch (item.Status)
                    {
                        case 0:
                            QueueItems.Add(item);
                            break;
                        case 1:
                            InWorkItems.Add(item);
                            break;
                        case 2:
                            WaitingItems.Add(item);
                            break;
                        case 3:
                            DoneItems.Add(item);
                            break;
                        default:
                            QueueItems.Add(item); // По умолчанию — очередь
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}");
            }
        }

        //Фильтр задач

        public ICommand OpenFilterCommand => new RelayCommand(OpenFilterWindow);

        private async Task OpenFilterWindow()
        {
            var filterWindow = new TaskFilterWindow();
            if (filterWindow.ShowDialog() == true)
            {
               await ApplyTaskFilter(filterWindow.Filter);
            }
        }

        public async Task ApplyTaskFilter(TaskFilter filter)
        {
            QueueItems.Clear();
            InWorkItems.Clear();
            WaitingItems.Clear();
            DoneItems.Clear();

            var tasks = await _apiService.GetAllTasksAsync();

            var filtered = tasks.Where(t =>
                (string.IsNullOrEmpty(filter.Title) || t.Title.Contains(filter.Title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filter.Description) || t.Description.Contains(filter.Description, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filter.PersonalName) || t.Personal.Name.Contains(filter.PersonalName, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            foreach (var task in filtered)
            {
                var item = new TaskListItem
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = int.TryParse(task.Status, out var status) ? (int?)status : null,
                    PersonalName = task.Personal?.Name ?? "Не назначен"
                };

                switch (item.Status)
                {
                    case 0:
                        QueueItems.Add(item);
                        break;
                    case 1:
                        InWorkItems.Add(item);
                        break;
                    case 2:
                        WaitingItems.Add(item);
                        break;
                    case 3:
                        DoneItems.Add(item);
                        break;
                    default:
                        QueueItems.Add(item); // По умолчанию — очередь
                        break;
                }
            }
        }


        // Команда обновления данных


        private async Task RefreshData()
        {
            await LoadTasks(); // Перезагружаем данные
        }
        private async Task OpenCreateTaskWindow()
        {
            var createWindow = new CreateTaskWindow();
            if (createWindow.ShowDialog() == true)
            {
               await LoadTasks();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}