using RealtyCRMClient.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для AddCardWindow.xaml
    /// </summary>
    public partial class AddCardWindow : Window
    {
        private readonly ApiService _apiService = new();

        public AddCardWindow()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var card = new CardObjectRieltyDto
            {
                Title = TitleBox.Text,
                Address = AddressBox.Text,
                Price = PriceBox.Text
            };

            await _apiService.CreateCardAsync(card);
            MessageBox.Show("Карточка создана!");
            Close();
        }
    }
}
