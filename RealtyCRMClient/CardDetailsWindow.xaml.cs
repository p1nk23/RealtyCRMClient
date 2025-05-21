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
    /// Логика взаимодействия для CardDetailsWindow.xaml
    /// </summary>
    public partial class CardDetailsWindow : Window
    {
        private readonly int _cardId;

        public CardDetailsWindow(int cardId)
        {
            InitializeComponent();
            _cardId = cardId;
            LoadCardDetails();
        }

        private async void LoadCardDetails()
        {
            var apiService = new ApiService();
            var card = await apiService.GetCardWithCommentsAsync(_cardId);
            DataContext = card;
        }

        private async void AddComment_Click(object sender, RoutedEventArgs e)
        {
            var text = CommentTextBox.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            var comment = new
            {
                Text = text,
                CardObjId = _cardId,
                Time = DateTime.UtcNow.AddHours(4)
            };

            var json = JsonConvert.SerializeObject(comment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await new HttpClient().PostAsync("https://localhost:5001/api/Comment", content);
            if (response.IsSuccessStatusCode)
            {

                LoadCardDetails(); // Обновите данные
            }
            else
            {
                MessageBox.Show("Ошибка при добавлении комментария.");
            }
        }
    }
}
