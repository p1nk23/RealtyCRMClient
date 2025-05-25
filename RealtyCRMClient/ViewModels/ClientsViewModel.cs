using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace RealtyCRMClient.ViewModels
{
    public class ClientsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly PersonalDto _currentUser;
        private ObservableCollection<ClientListItem> _clients = new();

        public ObservableCollection<ClientListItem> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public ClientsViewModel(PersonalDto user)
        {
            _currentUser = user;
            _apiService = new ApiService();
            LoadClients();
        }

        private async Task LoadClients()
        {
            var clients = await _apiService.GetAllClientsAsync();
            Clients = new ObservableCollection<ClientListItem>(
                clients.Select(c => new ClientListItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Number = c.Number,
                    Description = c.Description,
                    Status = c.Status,
                    CardObjId = c.CardObjId
                }).ToList()
            );
        }
        public async void RefreshClients()
        {
            await LoadClients();
        }

        /////////ssd/fs/df/sdf


        public ICommand RefreshCommand => new RelayCommand(RefreshData);

        private async Task RefreshData()
        {
            await LoadClients();
        }

        public ICommand CreateClientCommand => new RelayCommand(OpenCreateClientWindow);

        private async Task OpenCreateClientWindow()
        {
            var createWindow = new CreateClientWindow();
            if (createWindow.ShowDialog() == true)
            {
                await LoadClients();
            }
        }

        public ICommand OpenFilterCommand => new RelayCommand(OpenFilterWindow);

        private async Task OpenFilterWindow()
        {
            var filterWindow = new ClientFilterWindow();
            if (filterWindow.ShowDialog() == true)
            {
                var filtered = Clients.Where(c =>
                    (string.IsNullOrEmpty(filterWindow.Filter.Name) || c.Name.Contains(filterWindow.Filter.Name, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(filterWindow.Filter.Email) || c.Email.Contains(filterWindow.Filter.Email, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(filterWindow.Filter.Number) || c.Number.Contains(filterWindow.Filter.Number, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                Clients = new ObservableCollection<ClientListItem>(filtered);
            }
        }

        public ICommand BackToMenuCommand => new RelayCommand(BackToMainMenu);




        private async Task BackToMainMenu()
        {
            // Открываем главное меню
            var mainMenu = new MainMenuWindow(_currentUser);
            mainMenu.Show();

            // Закрываем текущее окно
            Application.Current.Windows.OfType<ClientsWindow>().FirstOrDefault()?.Close();

            await Task.CompletedTask; // Для совместимости с async Task
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
