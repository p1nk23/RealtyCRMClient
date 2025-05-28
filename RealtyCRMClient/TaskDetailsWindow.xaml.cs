using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System;

namespace RealtyCRMClient
{
    public partial class TaskDetailsWindow : Window
    {
        private readonly long _taskId;
        private readonly ApiService _apiService = new();

        public TaskDetailsWindow(long taskId)
        {
            InitializeComponent();
            _taskId = taskId;
            LoadData();
        }

        private async void LoadData()
        {
            var task = await _apiService.GetTaskByIdAsync(_taskId);
            if (task != null)
            {
                DataContext = task;

                // Загрузите всех сотрудников
                var personals = await _apiService.GetAllPersonalsAsync();
                var personalList = personals.Select(p => new PersonalListItem
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();

                Personals = new ObservableCollection<PersonalListItem>(personalList);
                PersonalComboBox.ItemsSource = Personals;

                // Установите текущего сотрудника
                if (task.Personal != null)
                {
                    var current = Personals.FirstOrDefault(p => p.Id == task.Personal.Id);
                    if (current != null)
                    {
                        PersonalComboBox.SelectedItem = current;
                    }
                }

                // Установите статус
                if (int.TryParse(task.Status, out var status))
                {
                    StatusComboBox.SelectedIndex = status;
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dto = new UpdateTaskObjectDto
            {
                Title = TitleBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                Status = ((ComboBoxItem)StatusComboBox.SelectedItem)?.Tag.ToString() ?? "0",
                StartDate = StartDatePicker.SelectedDate?.ToUniversalTime(),
                EndDate = EndDatePicker.SelectedDate?.ToUniversalTime(),
                PersonalId = (PersonalComboBox.SelectedItem as PersonalListItem)?.Id
            };
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PutAsync($"https://localhost:5001/api/TaskObject/{_taskId}", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Задача обновлена");
                Close();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка при обновлении: {error}");
            }
        }

        // Свойство для коллекции персонала
        private ObservableCollection<PersonalListItem> Personals { get; set; } = new();
    }
}