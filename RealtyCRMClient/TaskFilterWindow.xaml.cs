using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RealtyCRMClient
{
    public partial class TaskFilterWindow : Window
    {
        public TaskFilter Filter { get; private set; } = new();

        public TaskFilterWindow()
        {
            InitializeComponent();
            LoadPersonals();
        }

        private async void LoadPersonals()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:5001/api/Personal");
            if (response.IsSuccessStatusCode)
            {
                var personals = await response.Content.ReadFromJsonAsync<List<PersonalDto>>();
                foreach (var personal in personals)
                {
                    AssignedToComboBox.Items.Add(personal.Name);
                }
            }
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            Filter.Title = Title.Text.Trim();
            Filter.Description = Description.Text.Trim();
            Filter.PersonalName = AssignedToComboBox.SelectedItem?.ToString() ?? string.Empty;

            DialogResult = true;
            Close();
        }
    }
}