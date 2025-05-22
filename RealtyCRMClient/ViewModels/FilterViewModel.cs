using RealtyCRMClient.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace RealtyCRMClient.ViewModels
{
    public class FilterViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public FilterViewModel()
        {
            _apiService = new ApiService();
            LoadClients();
            ApplyFilterCommand = new RelayCommand(ApplyFilter);
        }

        private ObservableCollection<ClientDto> _clients = new();
        public ObservableCollection<ClientDto> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        private ClientDto _selectedClient;
        public ClientDto SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                if (_selectedClient?.CardObject != null)
                {
                    // Автоматически заполняем поля из объекта клиента
                    Title = _selectedClient.CardObject.Title;
                    Address = _selectedClient.CardObject.Address;
                    CeilingType = _selectedClient.CardObject.CeilingType;
                    WindowView = _selectedClient.CardObject.WindowView;
                    Bathroom = _selectedClient.CardObject.Bathroom;
                    Balcony = _selectedClient.CardObject.Balcony;
                    TotalArea = _selectedClient.CardObject.TotalArea;
                    Parking = _selectedClient.CardObject.Parking;
                    Heating = _selectedClient.CardObject.Heating;
                    GasSupply = _selectedClient.CardObject.GasSupply;
                }
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private string _ceilingType;
        public string CeilingType
        {
            get => _ceilingType;
            set
            {
                _ceilingType = value;
                OnPropertyChanged();
            }
        }

        private string _windowView;
        public string WindowView
        {
            get => _windowView;
            set
            {
                _windowView = value;
                OnPropertyChanged();
            }
        }

        private string _bathroom;
        public string Bathroom
        {
            get => _bathroom;
            set
            {
                _bathroom = value;
                OnPropertyChanged();
            }
        }

        private string _balcony;
        public string Balcony
        {
            get => _balcony;
            set
            {
                _balcony = value;
                OnPropertyChanged();
            }
        }

        private string _totalArea;
        public string TotalArea
        {
            get => _totalArea;
            set
            {
                _totalArea = value;
                OnPropertyChanged();
            }
        }

        private string _parking;
        public string Parking
        {
            get => _parking;
            set
            {
                _parking = value;
                OnPropertyChanged();
            }
        }

        private string _heating;
        public string Heating
        {
            get => _heating;
            set
            {
                _heating = value;
                OnPropertyChanged();
            }
        }

        private string _gasSupply;
        public string GasSupply
        {
            get => _gasSupply;
            set
            {
                _gasSupply = value;
                OnPropertyChanged();
            }
        }

        public ICommand ApplyFilterCommand { get; }

        private async void LoadClients()
        {
            var clients = await _apiService.GetAllClientsAsync();
            Clients = new ObservableCollection<ClientDto>(clients);
        }

        private async Task ApplyFilter()
        {
            //MessageBox.Show("Метод ApplyFilter вызван"); // Для проверки
            // Передаем фильтр в MainViewModel
            var filter = new CardFilter
            {
                Title = Title,
                Address = Address,
                CeilingType = CeilingType,
                WindowView = WindowView,
                Bathroom = Bathroom,
                Balcony = Balcony,
                TotalArea = TotalArea,
                Parking = Parking,
                Heating = Heating,
                GasSupply = GasSupply
            };
            


            // Вызовите метод фильтрации в MainViewModel
            if (Application.Current?.MainWindow?.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.ApplyCardFilter(filter);
            }
            else
            {
                MessageBox.Show("MainViewModel не найден");
            }

            // Закрываем окно фильтрации
            var window = Application.Current.Windows.OfType<FilterWindow>().FirstOrDefault();
            if (window != null)
            {
                window.Close();
            }

            // Возвращаем завершённую задачу
            await Task.CompletedTask;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
