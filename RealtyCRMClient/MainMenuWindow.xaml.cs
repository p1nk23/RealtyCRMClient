using RealtyCRMClient.DTOs;
using System.Windows;
using Serilog;
using RealtyCRMClient.AdminModule;

namespace RealtyCRMClient
{
    public partial class MainMenuWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly ILogger _logger;
        public PersonalDto CurrentUser { get; }


        public MainMenuWindow(PersonalDto user)
        {
            InitializeComponent();
            try
            {
                _logger = SerilogConfig.Configure();
                _apiService = new ApiService(_logger);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации логирования: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw; // Прерываем запуск, чтобы разработчик заметил проблему
            }
            CurrentUser = user;
            DataContext = this;
        }

        //Панель администратора
        private void OpenAdminWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var adminWindow = new AdminWindow(_apiService, _logger);
                adminWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Ошибка при открытии окна администратора");
                MessageBox.Show($"Ошибка при открытии окна администратора: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Договоры
        private void OpenContracts_Click(object sender, RoutedEventArgs e)
        {
            var window = new ContractsWindow();
            window.Show();
        }

        //Аналитика
        private void OpenAnalytics_Click(object sender, RoutedEventArgs e)
        {
            var analyticsWindow = new AnalyticsWindow();
            analyticsWindow.Show(); // Или ShowDialog() для модального окна
        }


        // Сменить пользователя
        private void SwitchUser_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        // Открыть базу объектов (MainWindow)
        private void OpenObjects_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(CurrentUser);
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void OpenClients_Click(object sender, RoutedEventArgs e)
        {
            var clientsWindow = new ClientsWindow(CurrentUser); // Передаем пользователя
            clientsWindow.Show();
            this.Close();
        }

        // Открыть Задачи
        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            var tasksWindow = new TasksWindow(CurrentUser);
            tasksWindow.Show();
            this.Close();
        }

        // Открыть Документы
        private void OpenDocuments_Click(object sender, RoutedEventArgs e)
        {
            var docsWindow = new DocumentsTemplateWindow();
            docsWindow.Show();
        }
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Завершить приложение
        }
    }
}