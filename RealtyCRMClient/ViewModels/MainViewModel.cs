using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using RealtyCRM.DTOs;
using RealtyCRMClient.DTOs;

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
            await LoadCards(); // Теперь это работает
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
                OnPropertyChanged();
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