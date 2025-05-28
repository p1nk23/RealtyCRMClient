using RealtyCRM.DTOs;
using RealtyCRMClient.DTOs;
using RealtyCRMClient.Models;
using RealtyCRMClient.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RealtyCRMClient
{
    public partial class TasksWindow : Window
    {
        private readonly PersonalDto _currentUser;
        private object _draggedItem; // Для перетаскивания

        public TasksWindow(PersonalDto user)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = new TasksViewModel(); // Убедитесь, что TasksViewModel создан
        }

        // === Методы для Drag & Drop ===

        private void ItemsControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border?.DataContext is TaskListItem item)
            {
                _draggedItem = item;
                DragDrop.DoDragDrop(border, item, DragDropEffects.Move);
            }
        }

        private void OpenMainMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainMenu = new MainMenuWindow(_currentUser);
            mainMenu.Show();
            this.Close();
        }

        private void ItemsControl_DragEnter(object sender, DragEventArgs e)
        {
            if (_draggedItem is TaskListItem && sender is ItemsControl)
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private async void ItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (_draggedItem is TaskListItem task && sender is ItemsControl targetList)
            {
                // Получаем целевую коллекцию
                if (targetList.ItemsSource is ObservableCollection<TaskListItem> targetCollection)
                {
                    // Удаляем из текущего списка
                    switch (task.Status)
                    {
                        case 0:
                            (DataContext as TasksViewModel).QueueItems.Remove(task);
                            break;
                        case 1:
                            (DataContext as TasksViewModel).InWorkItems.Remove(task);
                            break;
                        case 2:
                            (DataContext as TasksViewModel).WaitingItems.Remove(task);
                            break;
                        case 3:
                            (DataContext as TasksViewModel).DoneItems.Remove(task);
                            break;
                    }

                    // Обновляем статус в зависимости от целевой колонки
                    int targetStatus = targetCollection == (DataContext as TasksViewModel).QueueItems ? 0 :
                                       targetCollection == (DataContext as TasksViewModel).InWorkItems ? 1 :
                                       targetCollection == (DataContext as TasksViewModel).WaitingItems ? 2 : 3;

                    task.Status = targetStatus;

                    // Добавляем в новую колонку
                    targetCollection.Add(task);

                    // Отправляем обновление на сервер
                    await (DataContext as TasksViewModel).UpdateTaskStatus(task.Id, targetStatus);
                }
            }
        }


        private void OpenFilterWindow()
        {
            var filterWindow = new TaskFilterWindow();
            if (filterWindow.ShowDialog() == true)
            {
                var viewModel = DataContext as TasksViewModel;
                viewModel.ApplyTaskFilter(filterWindow.Filter);
            }
        }

        // === Открытие деталей задачи по клику ===
        private void Card_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var task = border.DataContext as TaskListItem;

            if (task != null)
            {
                var detailsWindow = new TaskDetailsWindow(task.Id);
                detailsWindow.Show();
            }
        }
    }
}