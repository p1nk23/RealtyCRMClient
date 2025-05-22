using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(NameBox.Text) ||
                string.IsNullOrEmpty(LoginBox.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(EmailBox.Text) ||
                string.IsNullOrEmpty(PhoneBox.Text))
            {
                MessageBox.Show("Все поля обязательны для заполнения.");
                return;
            }

            // Сбор данных
            var dto = new
            {
                Name = NameBox.Text,
                Login = LoginBox.Text,
                Password = PasswordBox.Password,
                Email = EmailBox.Text,
                Phone = PhoneBox.Text,
                Role = int.TryParse(RoleBox.Text, out var role) ? role : 1
            };

            // Сериализация и отправка
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:5001/api/Personal", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Регистрация успешна!");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка регистрации: {error}");
            }
        }
    }
}
