using RealtyCRM.DTOs;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.ViewModels;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealtyCRMClient
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PersonalDto _currentUser;

        public MainWindow(PersonalDto user)
        {
            InitializeComponent();
            DataContext = new MainViewModel();//удалить если что
            _currentUser = user;
            Title = $"CRM Недвижимость - {user.Name}";
        }

        private object _draggedItem;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ApplyFilters(List<CardObjectRieltyDto> filteredCards)
        {
            // Перезаписываем текущие данные в MainViewModel
            if (DataContext is MainViewModel viewModel)
            {
                // Преобразуем данные в ObservableCollection
                var cards = filteredCards.Select(c => new CardListItem
                {
                    Id = c.Id,
                    Title = c.Title,
                    Status = c.Status,
                    Address = c.Address
                }).ToList();

                // Фильтрация по статусам
                viewModel.QueueItems = new ObservableCollection<CardListItem>(cards.Where(c => c.Status == 0 || c.Status == null));
                viewModel.InWorkItems = new ObservableCollection<CardListItem>(cards.Where(c => c.Status == 1));
                viewModel.WaitingItems = new ObservableCollection<CardListItem>(cards.Where(c => c.Status == 2));
                viewModel.DoneItems = new ObservableCollection<CardListItem>(cards.Where(c => c.Status == 3));
            }
        }

        private void Card_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var card = border.DataContext as CardListItem;

            if (card != null)
            {
                var detailsWindow = new CardDetailsWindow(card.Id);
                detailsWindow.Show();
            }
        }

        private void ItemsControl_DragEnter(object sender, DragEventArgs e)
        {
            if (!_draggedItem.Equals(e.Data.GetData(e.Data.GetFormats()[0])))
                e.Effects = DragDropEffects.None;
        }

        private void ItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (_draggedItem is CardListItem card && sender is ItemsControl targetList)
            {
                // Получаем коллекцию, привязанную к ItemsControl
                if (targetList.ItemsSource is ObservableCollection<CardListItem> targetCollection)
                {
                    // Удаляем из текущего списка
                    switch (card.Status)
                    {
                        case 0:
                            (DataContext as MainViewModel).QueueItems.Remove(card);
                            break;
                        case 1:
                            (DataContext as MainViewModel).InWorkItems.Remove(card);
                            break;
                        case 2:
                            (DataContext as MainViewModel).WaitingItems.Remove(card);
                            break;
                        case 3:
                            (DataContext as MainViewModel).DoneItems.Remove(card);
                            break;
                    }

                    // Обновите статус в зависимости от целевого списка
                    int targetStatus = targetCollection == (DataContext as MainViewModel).QueueItems ? 0 :
                                       targetCollection == (DataContext as MainViewModel).InWorkItems ? 1 :
                                       targetCollection == (DataContext as MainViewModel).WaitingItems ? 2 : 3;

                    card.Status = targetStatus;
                    targetCollection.Add(card);

                    // Отправьте обновление на сервер
                    (DataContext as MainViewModel).UpdateCardStatus(card.Id, targetStatus);
                }
            }
        }
    }
}