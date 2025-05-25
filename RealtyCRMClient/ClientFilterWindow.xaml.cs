using RealtyCRMClient.DTOs;
using System.Windows;

namespace RealtyCRMClient
{
    public partial class ClientFilterWindow : Window
    {
        public ClientFilter Filter { get; private set; } = new ClientFilter();

        public ClientFilterWindow()
        {
            InitializeComponent();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            Filter.Name = NameBox.Text.Trim();
            Filter.Email = EmailBox.Text.Trim();
            Filter.Number = NumberBox.Text.Trim();
            Filter.Status = int.Parse(StatusComboBox.SelectedItem?.ToString() ?? "-1");
            DialogResult = true;
            Close();
        }
    }
}