using Newtonsoft.Json;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text;
            var password = PasswordBox.Password;

            var client = new HttpClient();
            var loginData = new
            {
                Login = login,
                Password = password
            };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:5001/api/Personal/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PersonalDto>();
                //MessageBox.Show($"Добро пожаловать, {result.Name}!");

                var mainWindow = new MainWindow();
                //mainWindow.DataContext = new MainViewModel();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }

        }
        private void OpenRegisterWindow_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно регистрации
            var registerWindow = new RegisterWindow();
            registerWindow.Show(); // Можно использовать registerWindow.ShowDialog(), если хотите модальное окно

            // Закрываем текущее окно
            this.Close();
        }
    }
}
