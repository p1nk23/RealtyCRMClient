using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace RealtyCRMClient
{
    public partial class AddCardWindow : Window
    {
        public AddCardWindow()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранное значение статуса
            var selectedItem = StatusComboBox.SelectedItem as ComboBoxItem;
            int? selectedStatus = selectedItem != null ? int.Parse(selectedItem.Tag.ToString()) : (int?)null;

            // Считываем данные из всех полей
            var card = new
            {
                Title = TitleBox.Text.Trim(),
                Address = AddressBox.Text.Trim(),
                Price = PriceBox.Text.Trim(),
                Status = selectedStatus, // Отправляем статус как int?
                City = int.TryParse(CityBox.Text.Trim(), out var city) ? (int?)city : null,
                MetroNearby = MetroNearbyBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                NumberOfRooms = int.TryParse(NumberOfRoomsBox.Text.Trim(), out var rooms) ? (int?)rooms : null,
                CeilingType = CeilingTypeBox.Text.Trim(),
                CeilingHeight = float.TryParse(CeilingHeightBox.Text.Trim(), out var height) ? (float?)height : null,
                WindowView = WindowViewBox.Text.Trim(),
                Renovation = RenovationBox.Text.Trim(),
                Bathroom = BathroomBox.Text.Trim(),
                Balcony = BalconyBox.Text.Trim(),
                RoomsType = RoomsTypeBox.Text.Trim(),
                RosreestrCheck = long.TryParse(RosreestrCheckBox.Text.Trim(), out var rosreestr) ? (long?)rosreestr : null,
                ListingId = ListingIdBox.Text.Trim(),
                Link = LinkBox.Text.Trim(),
                Level = LevelBox.Text.Trim(),
                HousingType = HousingTypeBox.Text.Trim(),
                LivingArea = LivingAreaBox.Text.Trim(),
                SoldWithFurniture = SoldWithFurnitureBox.Text.Trim(),
                ConstructionYear = ConstructionYearBox.Text.Trim(),
                HouseType = HouseTypeBox.Text.Trim(),
                Parking = ParkingBox.Text.Trim(),
                Entrances = EntrancesBox.Text.Trim(),
                Heating = HeatingBox.Text.Trim(),
                EmergencyStatus = EmergencyStatusBox.Text.Trim(),
                GasSupply = GasSupplyBox.Text.Trim(),
                TotalArea = TotalAreaBox.Text.Trim(),
                KitchenArea = KitchenAreaBox.Text.Trim(),
                Personal_id = long.TryParse(PersonalIdBox.Text.Trim(), out var personalId) ? (long?)personalId : null,
                Task_id = long.TryParse(TaskIdBox.Text.Trim(), out var taskId) ? (long?)taskId : null
            };

            // Проверка обязательных полей
            if (string.IsNullOrEmpty(card.Title) || string.IsNullOrEmpty(card.Address))
            {
                MessageBox.Show("Поля 'Название' и 'Адрес' обязательны для заполнения.");
                return;
            }

            // Сериализация JSON
            var json = JsonConvert.SerializeObject(card);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Отправка POST-запроса
            var client = new HttpClient { BaseAddress = new Uri("https://localhost:5001/api/") };
            var response = await client.PostAsync("CardObjectRielty", content);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Карточка успешно создана!");
                Close(); // Закрываем окно
            }
            else
            {
                MessageBox.Show("Ошибка при создании карточки. Проверьте все поля.");
            }
        }
    }
}