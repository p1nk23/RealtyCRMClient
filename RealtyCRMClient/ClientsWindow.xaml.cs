using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using RealtyCRMClient.ViewModels;
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
    /// Логика взаимодействия для ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        private long _clientId;
        private readonly PersonalDto _currentUser;

        public ClientsWindow(PersonalDto user)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = new ClientsViewModel(user);
        }

        private void Card_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var client = border.DataContext as ClientListItem;
            if (client != null)
            {
                var detailsWindow = new ClientDetailsWindow(client.Id);
                detailsWindow.Show();
            }
        }
    }
}
