using RealtyCRMClient.AdminModule.Exceptions;
using RealtyCRMClient.AdminModule.Models;
using RealtyCRMClient.AdminModule.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Windows;

namespace RealtyCRMClient.AdminModule.ViewModels
{
    /// <summary>
    /// ViewModel для окна администратора.
    /// </summary>
    public class AdminWindowViewModel : INotifyPropertyChanged
    {
        private readonly EntityService _entityService;
        private readonly ILogger _logger;
        private EntityItem _selectedEntity;
        private ObservableCollection<DataGridItem> _entities;
        private string _statusMessage;
        private long _deleteId;

        public AdminWindowViewModel(EntityService entityService, ILogger logger)
        {
            _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Entities = new ObservableCollection<DataGridItem>();
            EntityItems = GetEntityItems();
            DeleteCommand = new AsyncRelayCommand(
                execute: async () => await DeleteEntityAsync(),
                canExecute: () =>
                {
                    bool canExecute = DeleteId > 0 && SelectedEntity != null;
                    _logger.Information("DeleteCommand CanExecute checked: {CanExecute}, DeleteId: {DeleteId}, SelectedEntity: {SelectedEntity}",
                        canExecute, DeleteId, SelectedEntity?.DisplayName);
                    return canExecute;
                });

            try
            {
                _logger.Information("AdminWindowViewModel инициализировано");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка логирования: {ex.Message}";
            }
        }

        /// <summary>
        /// Список типов сущностей для ComboBox.
        /// </summary>
        public List<EntityItem> EntityItems { get; }

        /// <summary>
        /// Выбранный тип сущности.
        /// </summary>
        public EntityItem SelectedEntity
        {
            get => _selectedEntity;
            set
            {
                if (_selectedEntity != value)
                {
                    _selectedEntity = value;
                    OnPropertyChanged();
                    _logger.Information("SelectedEntity изменён: {DisplayName}", value?.DisplayName);
                    _ = LoadEntitiesAsync();
                    DeleteCommand.RaiseCanExecuteChanged(); // Обновляем состояние команды
                }
            }
        }

        /// <summary>
        /// Список записей для отображения в DataGrid.
        /// </summary>
        public ObservableCollection<DataGridItem> Entities
        {
            get => _entities;
            set
            {
                _entities = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Сообщение о статусе операции.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ID записи для удаления.
        /// </summary>
        public long DeleteId
        {
            get => _deleteId;
            set
            {
                if (_deleteId != value)
                {
                    _deleteId = value;
                    OnPropertyChanged();
                    _logger.Information("DeleteId изменён: {DeleteId}", value);
                    DeleteCommand.RaiseCanExecuteChanged(); // Обновляем состояние команды
                }
            }
        }

        /// <summary>
        /// Команда для удаления записи.
        /// </summary>
        public AsyncRelayCommand DeleteCommand { get; }

        /// <summary>
        /// Загружает список записей для выбранного типа сущности.
        /// </summary>
        private async Task LoadEntitiesAsync()
        {
            if (SelectedEntity == null)
            {
                Entities.Clear();
                StatusMessage = "Выберите тип сущности";
                return;
            }

            StatusMessage = "Загрузка данных...";
            try
            {
                var entities = await _entityService.GetEntitiesAsync(SelectedEntity.Type);
                Entities = new ObservableCollection<DataGridItem>(entities);
                StatusMessage = $"Загружено {entities.Count} записей";
                _logger.Information("Успешно загружено {Count} записей для {EntityType}", entities.Count, SelectedEntity.Type);
            }
            catch (ApiException ex)
            {
                StatusMessage = $"Ошибка: {ex.Message} (Код: {ex.StatusCode})";
                _logger.Error(ex, "Ошибка при загрузке сущностей типа {EntityType}", SelectedEntity.Type);
            }
            catch (Exception ex)
            {
                StatusMessage = "Неизвестная ошибка при загрузке данных";
                _logger.Error(ex, "Неизвестная ошибка при загрузке сущностей типа {EntityType}", SelectedEntity.Type);
            }
        }

        /// <summary>
        /// Удаляет запись по указанному ID.
        /// </summary>
        private async Task DeleteEntityAsync()
        {
            _logger.Information("Начало удаления записи с ID {DeleteId} для типа {EntityType}", DeleteId, SelectedEntity.Type);
            if (MessageBox.Show($"Вы уверены, что хотите удалить запись с ID {DeleteId}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                StatusMessage = "Удаление отменено";
                _logger.Information("Удаление записи с ID {DeleteId} отменено пользователем", DeleteId);
                return;
            }

            StatusMessage = "Удаление записи...";
            try
            {
                await _entityService.DeleteEntityAsync(SelectedEntity.Type, DeleteId);
                StatusMessage = $"Запись с ID {DeleteId} успешно удалена";
                _logger.Information("Успешно удалена запись типа {EntityType} с ID {Id}", SelectedEntity.Type, DeleteId);
                await LoadEntitiesAsync();
                DeleteId = 0;
            }
            catch (ApiException ex)
            {
                string message = ex.Message.Contains("23503") || ex.Message.Contains("foreign key")
                    ? $"Нельзя удалить {SelectedEntity.DisplayName}, так как запись связана с комментариями"
                    : $"Ошибка при удалении: {ex.Message} (Код: {ex.StatusCode})";
                StatusMessage = message;
                _logger.Error(ex, "Ошибка при удалении сущности типа {EntityType} с ID {Id}", SelectedEntity.Type, DeleteId);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Неизвестная ошибка при удалении: {ex.Message}";
                _logger.Error(ex, "Неизвестная ошибка при удалении сущности типа {EntityType} с ID {Id}", SelectedEntity.Type, DeleteId);
            }
        }

        /// <summary>
        /// Возвращает список типов сущностей для ComboBox.
        /// </summary>
        private List<EntityItem> GetEntityItems()
        {
            return new List<EntityItem>
            {
                new EntityItem { DisplayName = "Клиенты", Type = EntityType.Client },
                new EntityItem { DisplayName = "Контракты", Type = EntityType.Contract },
                new EntityItem { DisplayName = "Карточки недвижимости", Type = EntityType.CardObjectRielty },
                new EntityItem { DisplayName = "Комментарии", Type = EntityType.Comment },
                new EntityItem { DisplayName = "Шаблоны документов", Type = EntityType.DocumentTemplate },
                new EntityItem { DisplayName = "Персонал", Type = EntityType.Personal },
                new EntityItem { DisplayName = "Задачи", Type = EntityType.TaskObject }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Простая реализация команды для MVVM.
    /// </summary>
    
}
