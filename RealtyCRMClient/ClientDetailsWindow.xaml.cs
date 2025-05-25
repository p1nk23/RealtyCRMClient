using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using RealtyCRMClient.Models;

namespace RealtyCRMClient
{
    public partial class ClientDetailsWindow : Window
    {
        private readonly long _clientId;

        public ClientDetailsWindow(long clientId)
        {
            InitializeComponent();
            _clientId = clientId;
            LoadClientData();
        }

        private async void LoadClientData()
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:5001/api/Client/{_clientId}");
            if (response.IsSuccessStatusCode)
            {
                var clientDto = await response.Content.ReadFromJsonAsync<ClientDto>();
                DataContext = clientDto;

                // Устанавливаем индекс ComboBox'а
                StatusComboBox.SelectedIndex = clientDto.Status;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранное значение статуса
            var selectedItem = StatusComboBox.SelectedItem as ComboBoxItem;
            int status = selectedItem != null ? int.Parse(selectedItem.Tag.ToString()) : 0;

            // Собираем данные из полей
            var dto = new
            {
                Name = NameBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                Number = NumberBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                Status = status,
                CardObjId = (int?)null,
                TaskObjId = (long?)null
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PutAsync($"https://localhost:5001/api/Client/{_clientId}", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Клиент обновлен");
                Close();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {error}");
            }
        }
    }
}