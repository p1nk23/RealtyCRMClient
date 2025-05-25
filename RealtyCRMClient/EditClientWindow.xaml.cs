using Newtonsoft.Json;
using RealtyCRMClient.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;

namespace RealtyCRMClient
{
    public partial class EditClientWindow : Window
    {
        private readonly long _clientId;
        private ClientDto _client;

        public EditClientWindow(long clientId)
        {
            InitializeComponent();
            _clientId = clientId;
            LoadClientData();
        }

        private async void LoadClientData()
        {
            var response = await new HttpClient().GetFromJsonAsync<ClientDto>($"https://localhost:5001/api/Client/{_clientId}");
            if (response != null)
            {
                _client = response;
                DataContext = _client;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dto = new
            {
                Name = _client.Name,
                Email = _client.Email,
                Number = _client.Number,
                Description = _client.Description,
                Status = _client.Status,
                CardObjId = _client.CardObjId,
                TaskObjId = _client.TaskObjId
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PutAsync($"https://localhost:5001/api/Client/{_clientId}", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Клиент обновлен!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка обновления клиента.");
            }
        }
    }
}