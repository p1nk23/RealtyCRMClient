using Newtonsoft.Json;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RealtyCRMClient
{
    /// <summary>
    /// Логика взаимодействия для CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {
        private readonly ApiService _apiService = new();
        public CreateTaskWindow()
        {
            InitializeComponent();
            LoadPersonals();
        }
        private async void LoadPersonals()
        {
            var personals = await _apiService.GetAllPersonalsAsync(); // Убедитесь, что метод реализован
            Personals = new ObservableCollection<PersonalListItem>(
                personals.Select(p => new PersonalListItem
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            );

            AssignedToComboBox.ItemsSource = Personals;
        }

        private ObservableCollection<PersonalListItem> Personals { get; set; } = new();



        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPersonal = AssignedToComboBox.SelectedItem as ComboBoxItem;
            var statusItem = StatusComboBox.SelectedItem as ComboBoxItem;

            var dto = new UpdateTaskObjectDto
            {
                Title = TitleBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                Status = statusItem?.Tag.ToString() ?? "0",
                StartDate = StartDatePicker.SelectedDate?.ToUniversalTime(),
                EndDate = EndDatePicker.SelectedDate?.ToUniversalTime(),
                PersonalId = (AssignedToComboBox.SelectedItem as PersonalListItem)?.Id
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:5001/api/TaskObject", content);
            if (response.IsSuccessStatusCode)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Ошибка при создании задачи");
            }
        }
    }
}
