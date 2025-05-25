using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace RealtyCRMClient
{
    public partial class CreateClientWindow : Window
    {

        private Dictionary<int, string> _statusOptions = new()
        {
            { 0, "Не активен" },
            { 1, "Активен" },
            { 2, "В архиве" }
        };
        private int SelectedStatus { get; set; }

        public CreateClientWindow()
        {
            InitializeComponent();
            StatusComboBox.ItemsSource = _statusOptions;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var dto = new
            {
                Name = NameBox.Text.Trim(),
                Email = EmailBox.Text.Trim(),
                Number = NumberBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                Status = SelectedStatus,
                CardObjId = (int?)null,
                TaskObjId = (long?)null
            };

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:5001/api/Client", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Клиент успешно создан!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {error}");
            }

        }
    }
}