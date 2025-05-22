using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RealtyCRM.DTOs;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;

namespace RealtyCRMClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private readonly ApiService _apiService;
        private ObservableCollection<CardListItem> _allCards = new();
        private ObservableCollection<CardListItem> _queueItems = new();
        private ObservableCollection<CardListItem> _inWorkItems = new();
        private ObservableCollection<CardListItem> _waitingItems = new();
        private ObservableCollection<CardListItem> _doneItems = new();

        public ICommand RefreshCommand => new RelayCommand(RefreshData);

        private async Task RefreshData()
        {
            await LoadCards();
        }
        //Фильтр


        public ICommand CreateCardCommand => new RelayCommand(OpenCreateCardWindow);

        private async Task OpenCreateCardWindow()
        {
            var addWindow = new AddCardWindow();
            addWindow.ShowDialog(); // Это синхронный вызов, но он совместим
            await Task.CompletedTask; // Позволяет методу возвращать Task
        }

        public ObservableCollection<CardListItem> AllCards
        {
            get => _allCards;
            set
            {
                _allCards = value;
                OnPropertyChanged();
            }
        }

        

        public ObservableCollection<CardListItem> QueueItems
        {
            get => _queueItems;
            set
            {
                _queueItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CardListItem> InWorkItems
        {
            get => _inWorkItems;
            set
            {
                _inWorkItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CardListItem> WaitingItems
        {
            get => _waitingItems;
            set
            {
                _waitingItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CardListItem> DoneItems
        {
            get => _doneItems;
            set
            {
                _doneItems = value;
                OnPropertyChanged();
            }
        }
        

        public MainViewModel()
        {
            _apiService = new ApiService();
            LoadCards();
            SelectedCards = QueueItems;
        }

        private async Task LoadCards()
        {
            var allCards = await _apiService.GetAllCardsAsync();
            AllCards = new ObservableCollection<CardListItem>(allCards);

            QueueItems = new ObservableCollection<CardListItem>(
                AllCards.Where(c => c.Status == 0 || c.Status == null)
            );

            InWorkItems = new ObservableCollection<CardListItem>(
                AllCards.Where(c => c.Status == 1)
            );

            WaitingItems = new ObservableCollection<CardListItem>(
                AllCards.Where(c => c.Status == 2)
            );

            DoneItems = new ObservableCollection<CardListItem>(
                AllCards.Where(c => c.Status == 3)
            );
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private ObservableCollection<CardListItem> _selectedCards = new();
        public ObservableCollection<CardListItem> SelectedCards
        {
            get => _selectedCards;
            set
            {
                _selectedCards = value;
                OnPropertyChanged(); // Должно вызываться
            }
        }



        private CardListItem _selectedCard;
        public CardListItem SelectedCard
        {
            get => _selectedCard;
            set
            {
                _selectedCard = value;
                if (value != null)
                    OpenCardDetails();
            }
        }

        public ICommand OpenFilterCommand => new RelayCommand(OpenFilterWindow);

        private async Task OpenFilterWindow()
        {
            //MessageBox.Show("Открытие окна фильтрации");
            var filterWindow = new FilterWindow();
            filterWindow.Show(); // Или ShowDialog(), в зависимости от логики
            await Task.CompletedTask;
        }

        private CardFilter _currentFilter = new();

        public void ApplyCardFilter(CardFilter filter, int? cardObjId = null)
        {
            var filteredCards = AllCards.Where(c =>
            {
                // Если есть ID клиента — ищем только по нему
                if (cardObjId.HasValue)
                {
                    return c.Id == cardObjId.Value;
                }

                // Иначе — ищем по другим полям
                return
                    (string.IsNullOrEmpty(filter.Title) || c.Title?.Contains(filter.Title, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.Address) || c.Address?.Contains(filter.Address, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.CeilingType) || c.CeilingType?.Contains(filter.CeilingType, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.WindowView) || c.WindowView?.Contains(filter.WindowView, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.Bathroom) || c.Bathroom?.Contains(filter.Bathroom, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.Balcony) || c.Balcony?.Contains(filter.Balcony, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.TotalArea) || c.TotalArea?.Contains(filter.TotalArea, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.Parking) || c.Parking?.Contains(filter.Parking, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.Heating) || c.Heating?.Contains(filter.Heating, StringComparison.OrdinalIgnoreCase) == true) &&
                    (string.IsNullOrEmpty(filter.GasSupply) || c.GasSupply?.Contains(filter.GasSupply, StringComparison.OrdinalIgnoreCase) == true);
            }).ToList();

            // Обновите отображаемые данные
            QueueItems = new ObservableCollection<CardListItem>(filteredCards.Where(c => c.Status == 0 || c.Status == null));
            InWorkItems = new ObservableCollection<CardListItem>(filteredCards.Where(c => c.Status == 1));
            WaitingItems = new ObservableCollection<CardListItem>(filteredCards.Where(c => c.Status == 2));
            DoneItems = new ObservableCollection<CardListItem>(filteredCards.Where(c => c.Status == 3));
        }

        public ICommand ShowQueueCommand => new RelayCommand(LoadQueue);
        public ICommand ShowInWorkCommand => new RelayCommand(LoadInWork);
        public ICommand ShowWaitingCommand => new RelayCommand(LoadWaiting);
        public ICommand ShowDoneCommand => new RelayCommand(LoadDone);

        private async Task LoadQueue()
        {
            SelectedCards = QueueItems;
        }

        private async Task LoadInWork()
        {
            SelectedCards = InWorkItems;
        }

        private async Task LoadWaiting()
        {
            SelectedCards = WaitingItems;
        }

        private async Task LoadDone()
        {
            SelectedCards = DoneItems;
        }

        private void OpenCardDetails()
        {
            var window = new CardDetailsWindow(SelectedCard.Id);
            window.Show();
        }
        public async void UpdateCardStatus(int cardId, int newStatus)
        {
            try
            {
                var card = await _apiService.GetCardByIdAsync(cardId);
                if (card != null)
                {
                    card.Status = newStatus;
                    await _apiService.UpdateCardAsync(cardId, card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса: {ex.Message}");
            }
        }



    }
}